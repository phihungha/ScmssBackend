﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.Controllers
{
    [Authorize(Roles = "Director,Finance,SalesSpecialist,SalesManager,LogisticsSpecialist")]
    [Route("api/[controller]")]
    [ApiController]
    public class SalesOrdersController : CustomControllerBase
    {
        private readonly ISalesOrdersService _salesOrdersService;

        public SalesOrdersController(ISalesOrdersService salesOrdersService,
                                     UserManager<User> userManager)
            : base(userManager)
        {
            _salesOrdersService = salesOrdersService;
        }

        [Authorize(Roles = "SalesSpecialist,SalesManager,LogisticsSpecialist")]
        [HttpPost("{orderId}/events")]
        public async Task<ActionResult<TransOrderEventDto>> AddManualEvent(
            int orderId,
            [FromBody] TransOrderEventCreateDto body)
        {
            TransOrderEventDto item = await _salesOrdersService.AddManualEventAsync(orderId, body);
            return Ok(item);
        }

        [Authorize(Roles = "SalesSpecialist,SalesManager")]
        [HttpPost]
        public async Task<ActionResult<SalesOrderDto>> Create([FromBody] SalesOrderCreateDto body)
        {
            SalesOrderDto item = await _salesOrdersService.CreateAsync(body, CurrentIdentity);
            return Ok(item);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalesOrderDto>> Get(int id)
        {
            SalesOrderDto? item = await _salesOrdersService.GetAsync(id, CurrentIdentity);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpGet]
        public async Task<ActionResult<IList<SalesOrderDto>>> GetMany(
            [FromQuery] TransOrderQueryDto<SalesOrderSearchCriteria> query)
        {
            IList<SalesOrderDto> items = await _salesOrdersService.GetManyAsync(query, CurrentIdentity);
            return Ok(items);
        }

        [Authorize(Roles = "Finance,SalesSpecialist,SalesManager")]
        [HttpPatch("{id}")]
        public async Task<ActionResult<SalesOrderDto>> Update(
            int id,
            [FromBody] SalesOrderUpdateDto body)
        {
            SalesOrderDto item = await _salesOrdersService.UpdateAsync(id, body, CurrentIdentity);
            return Ok(item);
        }

        [Authorize(Roles = "SalesSpecialist,SalesManager,LogisticsSpecialist")]
        [HttpPatch("{orderId}/events/{id}")]
        public async Task<ActionResult<TransOrderEventDto>> UpdateEvent(
            int orderId,
            int id,
            [FromBody] OrderEventUpdateDto body)
        {
            TransOrderEventDto item = await _salesOrdersService.UpdateEventAsync(id, orderId, body);
            return Ok(item);
        }
    }
}
