﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.Data;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.DomainServices
{
    public class SalesOrdersService : ISalesOrdersService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public SalesOrdersService(IMapper mapper, AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<TransOrderEventDto> AddManualEvent(int orderId, TransOrderEventCreateDto dto)
        {
            SalesOrder? order = await _dbContext.SalesOrders.FirstOrDefaultAsync(i => i.Id == orderId);
            if (order == null)
            {
                throw new EntityNotFoundException();
            }

            SalesOrderEvent item = order.AddManualEvent(dto.Type, dto.Location, dto.Message);

            await _dbContext.SaveChangesAsync();
            return GetOrderEventDto(item);
        }

        public async Task<SalesOrderDto> CreateSalesOrderAsync(SalesOrderCreateDto dto, string userId)
        {
            Customer? customer = await _dbContext.Customers.FindAsync(dto.CustomerId);
            if (customer == null)
            {
                throw new EntityNotFoundException("Customer not found");
            }

            var item = new SalesOrder
            {
                ToLocation = dto.ToLocation ?? customer.DefaultLocation,
                CustomerId = dto.CustomerId,
                CreateUserId = userId,
            };

            if (dto.ProductionFacilityId != null)
            {
                int facilityId = (int)dto.ProductionFacilityId;
                item.ProductionFacilityId = facilityId;
                item.FromLocation = await GetLocationOfProductionFacilityAsync(facilityId);
            }

            await AddOrderItemsFromDtos(item, dto.Items);
            item.Start(userId);

            _dbContext.SalesOrders.Add(item);
            await _dbContext.SaveChangesAsync();
            return GetSalesOrderDto(item);
        }

        public async Task<SalesOrderDto?> GetSalesOrderAsync(int id)
        {
            SalesOrder? item = await _dbContext.SalesOrders
                .Include(i => i.Items).ThenInclude(i => i.Product)
                .Include(i => i.Customer)
                .Include(i => i.ProductionFacility)
                .Include(i => i.Events)
                .Include(i => i.CreateUser)
                .Include(i => i.FinishUser)
                .FirstOrDefaultAsync(i => i.Id == id);
            return _mapper.Map<SalesOrderDto?>(item);
        }

        public async Task<IList<SalesOrderDto>> GetSalesOrdersAsync()
        {
            IList<SalesOrder> items = await _dbContext.SalesOrders
                .Include(i => i.Customer)
                .Include(i => i.ProductionFacility)
                .Include(i => i.CreateUser)
                .Include(i => i.FinishUser)
                .ToListAsync();
            return _mapper.Map<IList<SalesOrderDto>>(items);
        }

        public async Task<TransOrderEventDto> UpdateEvent(int id, int orderId, OrderEventUpdateDto dto)
        {
            SalesOrder? order = await _dbContext.SalesOrders
                .Include(i => i.Events)
                .FirstOrDefaultAsync(i => i.Id == orderId);
            if (order == null)
            {
                throw new EntityNotFoundException();
            }

            SalesOrderEvent item = order.UpdateEvent(id, dto.Location, dto.Message);

            await _dbContext.SaveChangesAsync();
            return GetOrderEventDto(item);
        }

        public async Task<SalesOrderDto> UpdateSalesOrderAsync(
            int id,
            SalesOrderUpdateDto dto,
            string userId)
        {
            SalesOrder? item = await _dbContext.SalesOrders
                .Include(i => i.Items).ThenInclude(i => i.Product)
                .Include(i => i.Customer)
                .Include(i => i.ProductionFacility)
                .Include(i => i.Events)
                .Include(i => i.CreateUser)
                .Include(i => i.FinishUser)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (item == null)
            {
                throw new EntityNotFoundException();
            }

            if (dto.PaymentCompleted ?? false)
            {
                if (dto.PaymentAmount == null)
                {
                    throw new InvalidDomainOperationException(
                            "Cannot complete payment without payment amount"
                        );
                }
                item.CompletePayment((decimal)dto.PaymentAmount);
                await _dbContext.SaveChangesAsync();
                return GetSalesOrderDto(item);
            }

            if (item.Finished)
            {
                throw new InvalidDomainOperationException(
                        "Cannot update order because it is finished"
                    );
            }

            if (item.Status == TransOrderStatus.Processing)
            {
                if (dto.ProductionFacilityId != null)
                {
                    int facilityId = (int)dto.ProductionFacilityId;
                    item.ProductionFacilityId = facilityId;
                    item.FromLocation = await GetLocationOfProductionFacilityAsync(facilityId);
                }

                if (dto.Items != null)
                {
                    _dbContext.RemoveRange(item.Items);
                    await AddOrderItemsFromDtos(item, dto.Items);
                }
            }

            if (item.Status != TransOrderStatus.Delivered)
            {
                item.ToLocation = dto.ToLocation ?? item.Customer.DefaultLocation;
            }

            switch (dto.Status)
            {
                case TransOrderStatusSelection.Delivering:
                    item.StartDelivery();
                    break;

                case TransOrderStatusSelection.Delivered:
                    item.FinishDelivery();
                    break;

                case TransOrderStatusSelection.Completed:
                    item.Complete(userId);
                    break;

                case TransOrderStatusSelection.Canceled:
                    if (dto.Problem == null)
                    {
                        throw new InvalidDomainOperationException(
                                "Cannot cancel an order without a problem."
                            );
                    }
                    item.Cancel(userId, dto.Problem);
                    break;

                case TransOrderStatusSelection.Returned:
                    if (dto.Problem == null)
                    {
                        throw new InvalidDomainOperationException(
                                "Cannot return an order without a problem."
                            );
                    }
                    item.Return(userId, dto.Problem);
                    break;
            }

            await _dbContext.SaveChangesAsync();
            return GetSalesOrderDto(item);
        }

        private async Task AddOrderItemsFromDtos(SalesOrder order, IEnumerable<TransOrderItemInputDto> dtos)
        {
            IList<int> productIds = dtos.Select(i => i.ItemId).ToList();
            IDictionary<int, Product> products = await _dbContext
                .Products
                .Where(i => productIds.Contains(i.Id))
                .ToDictionaryAsync(i => i.Id);

            order.Items.Clear();
            foreach (var dto in dtos)
            {
                var item = new SalesOrderItem
                {
                    ItemId = dto.ItemId,
                    Unit = products[dto.ItemId].Unit,
                    UnitPrice = products[dto.ItemId].Price,
                    Quantity = dto.Quantity
                };
                order.AddItem(item);
            }
        }

        private async Task<string> GetLocationOfProductionFacilityAsync(int facilityId)
        {
            ProductionFacility? facility = await _dbContext
                .ProductionFacilities
                .FindAsync(facilityId);
            if (facility == null)
            {
                throw new EntityNotFoundException("Production facility not found");
            }
            return facility.Location;
        }

        private TransOrderEventDto GetOrderEventDto(SalesOrderEvent item)
        {
            return _mapper.Map<TransOrderEventDto>(item);
        }

        private SalesOrderDto GetSalesOrderDto(SalesOrder item)
        {
            return _mapper.Map<SalesOrderDto>(item);
        }
    }
}
