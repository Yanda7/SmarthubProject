#region using Directives
using Microsoft.EntityFrameworkCore;
using Smarthub.API.Data;
using Smarthub.API.Models;
using System.Linq.Expressions;
using System.Net;
using static Smarthub.API.Services.IUnitOfWork;
#endregion

namespace Smarthub.API.Repository
{
    /// <summary>
    /// Repository for orders, implementing the unit of work pattern
    /// </summary>
    public class OrderRepository : IUnitOfWork<Order>
    {
        #region Fields

        /// <summary>
        /// The application database context
        /// </summary>
        private readonly ApplicationDbContext _dbContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderRepository"/> class
        /// </summary>
        /// <param name="dbContext">The application database context</param>
        public OrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks if an entity exists in the database
        /// </summary>
        /// <typeparam name="TEntity">The type of entity to check</typeparam>
        /// <param name="predicate">An optional predicate to filter the results</param>
        /// <returns>True if the entity exists, false otherwise</returns>
        public bool DoesEntityExist<TEntity>(Expression<Func<TEntity, bool>> predicate = null) where TEntity : class
        {
            IQueryable<TEntity> data = _dbContext.Set<TEntity>();

            return data.Any(predicate);
        }

        /// <summary>
        /// Asynchronously creates a new order
        /// </summary>
        /// <param name="order">The order to create</param>
        /// <returns>The created order</returns>
        public async Task<Order> OnItemCreationAsync(Order order)
        {
            try
            {
                await _dbContext.AddAsync(order);

                return order;
            }
            catch (Exception)
            {
                throw new Exception("Error: Failed to add order ");
            }
        }

        #endregion

        #region Order Retrieval and Modification

        /// <summary>
        /// Asynchronously loads an order by its ID
        /// </summary>
        /// <param name="OrderId">The ID of the order to load</param>
        /// <returns>The loaded order</returns>
        public async Task<Order> OnLoadItemAsync(Guid OrderId)
        {
            try
            {
                return await _dbContext.Orders.Include(o => o.OrderLines).FirstOrDefaultAsync(o => o.OrderId == OrderId);
            }
            catch (Exception)
            {
                throw new Exception("Error: Unable to load order");
            }
        }

        /// <summary>
        /// Asynchronously loads all orders
        /// </summary>
        /// <returns>A list of all orders</returns>
        public async Task<List<Order>> OnLoadItemsAsync()
        {
            try
            {
                return await _dbContext.Orders.Include(o => o.OrderLines).ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("Error: Orders could not be loaded!");
            }
        }

        /// <summary>
        /// Asynchronously modifies an existing order
        /// </summary>
        /// <param name="order">The order to modify</param>
        /// <returns>The modified order</returns>
        public async Task<Order> OnModifyItemAsync(Order order)
        {
            Order results = new();

            try
            {
                results = await _dbContext.Orders.FindAsync(order.OrderId);

                if (results != null)
                {
                    results.CustomerName = order.CustomerName;
                    results.OrderCreatedDate = order.OrderCreatedDate;
                    results.OrderDate = order.OrderDate;
                    results.OrderStatus = order.OrderStatus;
                    results.OrderNumber = order.OrderNumber;
                    results.OrderType = order.OrderType;
                    results.OrderLines = order.OrderLines;
                    results.OrderId = order.OrderId;

                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw new Exception("Error: Order couldn't be Modified!");
            }

            return results;
        }

        #endregion

        #region Order Removal

        /// <summary>
        /// Asynchronously removes an order by its ID
        /// </summary>
        /// <param name="OrderId">The ID of the order to remove</param>
        /// <returns>The number of records affected by the removal</returns>
        public async Task<int> OnRemoveItemAsync(Guid OrderId)
        {
            Order order = new();

            int record = 0;

            try
            {
                order = await _dbContext.Orders.FirstOrDefaultAsync(m => m.OrderId == OrderId);

                if (order is not null)
                {
                    _dbContext.Remove(order);

                    record = await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw new Exception("Error: Delete Failed");
            }

            return record;
        }

        #endregion

        #region Database Operations

        /// <summary>
        /// Asynchronously saves changes to the database
        /// </summary>
        /// <returns>The number of records affected by the save operation</returns>
        public async Task<int> ItemSaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        #endregion

      
    }
}
