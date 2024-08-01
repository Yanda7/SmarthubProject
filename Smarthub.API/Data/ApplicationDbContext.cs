using Microsoft.EntityFrameworkCore;
using Smarthub.API.Models;

namespace Smarthub.API.Data
{

    #region Application Database Context

    /// <summary>
    /// The main database context for the application, derived from DbContext
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ApplicationDbContext class
        /// </summary>
        /// <param name="options">The options for the DbContext</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        #endregion

        #region DbSets

        /// <summary>
        /// Gets or sets the DbSet of orders
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// Gets or sets the DbSet of order lines
        /// </summary>
        public DbSet<OrderLine> OrderLines { get; set; }

        #endregion
    }

    #endregion


}
    

