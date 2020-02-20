using System;
using System.Collections.Generic;
using System.IO;
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
                return NotFound();

            return client;
        }

        [HttpGet("{id:length(24)}/name")]
        public ActionResult<string> GetClientName(string id)
        {
            var client = this._service.Get(id);

            if (client == null)
                return NotFound();

            return client.Name;
        }

        [HttpGet("{id:length(24)}/contact")]
        public ActionResult<string> GetClientContact(string id)
        {
            var client = this._service.Get(id);

            if (client == null)
                return NotFound();

            return client.Contact;
        }

        [HttpGet("{id:length(24)}/profile-image")]
        public IActionResult GetClientProfileImage(string id)
        {
            var client = this._service.Get(id);

            if (client == null)
                return NotFound();

            var imageContent = System.IO.File.ReadAllBytes(client.ProfileImagePath);

            return File(imageContent, "image/jpeg");
        }

        [HttpGet("{id:length(24)}/last-meeting")]
        public ActionResult<(long, DateTime)> GetClientLastMeeting(string id)
        {
            var client = this._service.Get(id);

            if (client == null)
                return NotFound();

            return client.LastMeeting;
        }

        [HttpGet("{id:length(24)}/meetings")]
        public ActionResult<IList<MeetingModel>> GetClientMeetings(string id)
        {
            var client = this._service.Get(id);

            if (client == null)
                return NotFound();

            return client.Meetings.ToList();
        }

        [HttpPost("{id:length(24)}/meetings")]
        public ActionResult<MeetingModel> PostClientMeeting(string id, MeetingModel meeting)
        {
            var client = this._service.Get(id);

            if (client == null)
                return NotFound();

            meeting.Index = client.Meetings.Count;
            client.Meetings.Add(meeting);
            this._service.Update(id, client);

            return meeting;
        }

        [HttpGet("{id:length(24)}/meetings/{index}")]
        public ActionResult<MeetingModel> GetClientMeeting(string id, int index)
        {
            var client = this._service.Get(id);

            if (client == null)
                return NotFound();

            return client.Meetings[index];
        }

        [HttpDelete("{id:length(24)}/meetings/{index}")]
        public IActionResult DeleteClientMeeting(string id, int index)
        {
            var client = this._service.Get(id);

            if (client == null
                || (index < 0 || index >= client.Meetings.Count))
                return NotFound();

            client.Meetings.RemoveAt(index);

            // Update the indices as they all must be messed up
            for (int i = 0; i < client.Meetings.Count; i++)
                client.Meetings[i].Index = i;

            this._service.Update(id, client);

            return NoContent();
        }
    }
}