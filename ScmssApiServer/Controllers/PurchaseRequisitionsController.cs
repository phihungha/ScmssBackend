﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.Controllers
{
    [Authorize(Roles =
        "Director," +
        "Finance," +
        "ProductionPlanner," +
        "ProductionManager," +
        "PurchaseSpecialist," +
        "PurchaseManager")]
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseRequisitionsController : CustomControllerBase
    {
        private readonly IPurchaseRequisitionsService _purchaseRequisitionsService;

        public PurchaseRequisitionsController(IPurchaseRequisitionsService purchaseRequisitionsService,
                                              UserManager<User> userManager)
            : base(userManager)
        {
            _purchaseRequisitionsService = purchaseRequisitionsService;
        }

        [Authorize(Roles = "ProductionPlanner,ProductionManager")]
        [HttpPost]
        public async Task<ActionResult<PurchaseRequisitionDto>> Create([FromBody] PurchaseRequisitionCreateDto body)
        {
            PurchaseRequisitionDto item = await _purchaseRequisitionsService.CreateAsync(body, CurrentIdentity);
            return Ok(item);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseRequisitionDto>> Get(int id)
        {
            PurchaseRequisitionDto? item = await _purchaseRequisitionsService.GetAsync(id, CurrentIdentity);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpGet]
        public async Task<ActionResult<IList<PurchaseRequisitionDto>>> GetMany([FromQuery] PurchaseRequisitionQueryDto query)
        {
            IList<PurchaseRequisitionDto> items = await _purchaseRequisitionsService.GetManyAsync(query, CurrentIdentity);
            return Ok(items);
        }

        [Authorize(Roles = "Finance,ProductionPlanner,ProductionManager")]
        [HttpPatch("{id}")]
        public async Task<ActionResult<PurchaseRequisitionDto>> Update(
            int id,
            [FromBody] PurchaseRequisitionUpdateDto body)
        {
            PurchaseRequisitionDto item = await _purchaseRequisitionsService.UpdateAsync(id, body, CurrentIdentity);
            return Ok(item);
        }
    }
}
