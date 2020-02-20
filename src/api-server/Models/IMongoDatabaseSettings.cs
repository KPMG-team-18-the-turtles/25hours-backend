using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwentyFiveHours.API.Models
{
    /// <summary>
    /// Represents a MongoDB setting container.
    /// </summary>
    public interface IMongoDatabaseSettings
    {
        /// <summary>
        /// Name of the model collection.
        /// </summary>
        string ModelCollectionName { get; set; }

        /// <summary>
        /// Connection string to be used when establishing connection.
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// Name of the target database.
        /// </summary>
        string DatabaseName { get; set; }
    }
}
