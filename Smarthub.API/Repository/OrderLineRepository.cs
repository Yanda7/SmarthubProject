#region using directives
using Microsoft.EntityFrameworkCore;
using Smarthub.API.Data;
using Smarthub.API.Models;
using System.Linq.Expressions;
using static Smarthub.API.Services.IUnitOfWork;

#endregion

namespace Smarthub.API.Repository
{
    #region Order Line Repository

    /// <summary>
    /// Repository for order lines, implementing the unit of work pattern
    /// </summary>
    public class OrderLineRepository : IUnitOfWork<OrderLine>
    {
        /// <summary>
        /// The database context for the application
        /// </summary>
        /// 
        private readonly ApplicationDbContext _dbContext;
        /// <summary>
        /// Initializes a new instance of the OrderLineRepository class
        /// </summary>
        /// <param name="dbContext">The application database context</param>
        public OrderLineRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #region Entity Existence Check

        /// <summary>
        /// Checks if an entity exists in the database based on a predicate
        /// </summary>
        /// <typeparam name="TEntity">The type of entity to check</typeparam>
        /// <param name="predicate">An optional predicate to filter the entities</param>
        /// <returns>True if the entity exists, false otherwise</returns>
        public bool DoesEntityExist<TEntity>(Expression<Func<TEntity, bool>> predicate = null) where TEntity : class
        {
            // Get the IQueryable set for the entity type
            IQueryable<TEntity> data = _dbContext.Set<TEntity>();

            // Check if any entities match the predicate
            return data.Any(predicate);
        }

        #endregion


        #region Order Line Creation

        /// <summary>
        /// Asynchronously creates a new order line
        /// </summary>
        /// <param name="orderLine">The order line to create</param>
        /// <returns>The created order line</returns>
        public async Task<OrderLine> OnItemCreationAsync(OrderLine orderLine)
        {
            try
            {
                // Add the order line to the database
                await _dbContext.AddAsync(orderLine);

                // Return the created order line
                return orderLine;
            }
            catch (Exception)
            {
                // Handle any exceptions that occur during the creation process
                throw new Exception("Error: Failed to add order line ");
            }
        }

        #endregion


        #region Order Line Retrieval

        /// <summary>
        /// Asynchronously loads an order line by its ID
        /// </summary>
        /// <param name="OrderLineId">The ID of the order line to load</param>
        /// <returns>The loaded order line</returns>
        public async Task<OrderLine> OnLoadItemAsync(Guid OrderLineId)
        {
            try
            {
                // Retrieve the order line from the database using the provided ID
                return await _dbContext.OrderLines.FirstOrDefaultAsync(ol => ol.OrderLineId == OrderLineId);
            }
            catch (Exception)
            {
                // Handle any exceptions that occur during the loading process
                throw new Exception("Error: Unable to load Order Line");
            }
        }

        /// <summary>
        /// Asynchronously loads all order lines
        /// </summary>
        /// <returns>A list of all order lines</returns>
        public async Task<List<OrderLine>> OnLoadItemsAsync()
        {
            try
            {
                // Retrieve all order lines from the database
                return await _dbContext.OrderLines.AsNoTracking().ToListAsync() ?? Enumerable.Empty<OrderLine>().ToList();
            }
            catch (Exception)
            {
                // Handle any exceptions that occur during the loading process
                throw new Exception("Error: Order Line could not be loaded!");
            }
        }

        #endregion

        #region Order Line Modification

        /// <summary>
        /// Asynchronously modifies an existing order line
        /// </summary>
        /// <param name="orderLine">The order line to modify</param>
        /// <returns>The modified order line</returns>
        public async Task<OrderLine> OnModifyItemAsync(OrderLine orderLine)
        {
            OrderLine results = new();

            try
            {
                // Retrieve the existing order line from the database
                results = await _dbContext.OrderLines.FindAsync(orderLine.OrderLineId);

                if (results != null)
                {
                    // Update the order line properties with the new values
                    results.ProductCode = orderLine.ProductCode;
                    results.ProductCostPrice = orderLine.ProductCostPrice;
                    results.ProductSalesPrice = orderLine.ProductSalesPrice;
                    results.ProductType = orderLine.ProductType;
                    results.LineNumber = orderLine.LineNumber;
                    results.Quantity = orderLine.Quantity;
                    results.OrderLineId = orderLine.OrderLineId;
                    results.OrderId = orderLine.OrderId;

                    // Save the changes to the database
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                // Handle any exceptions that occur during the modification process
                throw new Exception("Error: Order line could not be modified!");
            }

            return results;
        }

        #endregion

        #region Order Removal

        /// <summary>
        /// Asynchronously removes an order lists by its ID
        /// </summary>
        /// <param name="OrderId">The ID of the order to remove</param>
        /// <returns>The number of records affected by the removal</returns>
        public async Task<int> OnRemoveItemAsync(Guid OrderLineId)
        {
            OrderLine orderLine = new();

            int record = 0;

            try
            {
                orderLine = await _dbContext.OrderLines.FirstOrDefaultAsync(m => m.OrderId == OrderLineId);

                if (orderLine is not null)
                {
                    _dbContext.Remove(orderLine);

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
    #endregion
}

