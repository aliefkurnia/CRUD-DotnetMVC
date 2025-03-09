using CRUD_EF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_EF.Services
{
    public class OrderService
    {
        private readonly ADbContext _context;

        public OrderService(ADbContext context)
        {
            _context = context;
        }

        public async Task<List<SO_ORDER>> GetOrdersAsync(string searchKeyword,DateTime? orderDate)
        {
            var query = _context.SO_ORDER
                .Include(o => o.Customer)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchKeyword))
            {
                query = query.Where(o =>
                    (o.OrderNo != null && o.OrderNo.Contains(searchKeyword)) || 
                    (o.SoOrderId.ToString().Contains(searchKeyword)) || 
                    (o.Customer != null && o.Customer.CustomerName.Contains(searchKeyword)) 
                );
            }

            if (orderDate.HasValue)
            {
                query = query.Where(o => o.OrderDate.Date == orderDate.Value.Date);
            }

            return await query.ToListAsync();
        }
        public async Task<SO_ORDER?> GetOrderByIdAsync(long id)
        {
            return await _context.SO_ORDER
                .Include(o => o.Customer)
                .Include(o => o.Items) 
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.SoOrderId == id);
        }

        public async Task<bool> CreateOrderAsync(SO_ORDER order)
        {
            if (order == null || string.IsNullOrWhiteSpace(order.OrderNo) || order.ComCustomerId <= 0)
            {
                return false;
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.SO_ORDER.AddAsync(order);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error creating order: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> UpdateOrderAsync(SO_ORDER order)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingOrder = await _context.SO_ORDER
                    .Include(o => o.Items) 
                    .FirstOrDefaultAsync(o => o.SoOrderId == order.SoOrderId);

                if (existingOrder == null)
                {
                    return false; 
                }

                _context.Entry(existingOrder).CurrentValues.SetValues(order);

                var existingItems = existingOrder.Items.ToList();
                var newItemIds = order.Items.Where(i => i.SoItemId > 0).Select(i => i.SoItemId).ToHashSet();

                var itemsToDelete = existingItems.Where(i => !newItemIds.Contains(i.SoItemId)).ToList();
                if (itemsToDelete.Any())
                {
                    _context.SO_ITEM.RemoveRange(itemsToDelete);
                }

                foreach (var item in order.Items)
                {
                    if (item.SoItemId > 0) 
                    {
                        var existingItem = existingItems.FirstOrDefault(i => i.SoItemId == item.SoItemId);
                        if (existingItem != null)
                        {
                            _context.Entry(existingItem).CurrentValues.SetValues(item);
                        }
                    }
                    else 
                    {
                        item.SoOrderId = order.SoOrderId;
                        _context.SO_ITEM.Add(item);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error updating order: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteOrderAsync(long id)
        {
            var order = await _context.SO_ORDER
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.SoOrderId == id);

            if (order == null)
            {
                return false; 
            }

            try
            {
                _context.SO_ITEM.RemoveRange(order.Items);
                _context.SO_ORDER.Remove(order); 
                await _context.SaveChangesAsync(); 

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<COM_CUSTOMER>> GetCustomersAsync()
        {
            return await _context.COM_CUSTOMER.AsNoTracking().ToListAsync();
        }
    }
}