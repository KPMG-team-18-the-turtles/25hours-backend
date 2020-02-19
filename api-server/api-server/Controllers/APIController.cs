using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using TwentyFiveHours.API.Models;
using TwentyFiveHours.API.Services;

namespace TwentyFiveHours.API.Controllers
{
    using ClientService = MongoService<ClientModel>;

    /// <summary>
    /// Controller that responds all requests to <c>/api/clients</c>.
    /// </summary>
    [Route("api/clients")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly ClientService _service;

        public APIController(ClientService service)
        {
            this._service = service;
        }

        /// <summary>
        /// Get all clients as a list.
        /// </summary>
        /// <returns>List of all registered clients.</returns>
        [HttpGet]
        public ActionResult<IEnumerable<ClientModel>> GetClients()
        {
            return this._service.Get();
        }

        [HttpGet("{id:length(24)}")]
        public ActionResult<ClientModel> GetClient(string id)
        {
            var client = this._service.Get(id);

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }
    }
}