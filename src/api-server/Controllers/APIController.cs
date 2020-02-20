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

        #region Client-related

        [HttpGet]
        public ActionResult<IEnumerable<ClientModel>> GetClients()
        {
            return this._service.Get();
        }

        [HttpPost]
        public IActionResult PostClient(ClientModel client)
        {
            client.ID = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            this._service.Create(client);

            return CreatedAtAction(nameof(GetClient), new { id = client.ID }, client);
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

            try
            {
                var imageContent = System.IO.File.ReadAllBytes(client.ProfileImagePath);

                return File(imageContent, "image/jpeg");
            }
            catch (FileNotFoundException e)
            {
                return NotFound();
            }
        }

        #endregion // Client-related

        #region Meeting-related

        [HttpGet("{id:length(24)}/meetings")]
        public ActionResult<IList<MeetingModel>> GetClientMeetings(string id)
        {
            var client = this._service.Get(id);

            if (client == null)
                return NotFound();

            return client.Meetings.ToList();
        }

        [HttpPost("{id:length(24)}/meetings")]
        public IActionResult PostClientMeeting(string id, MeetingModel meeting)
        {
            var client = this._service.Get(id);

            if (client == null)
                return NotFound();

            meeting.Index = client.Meetings.Count;
            client.Meetings.Add(meeting);
            this._service.Update(id, client);

            return CreatedAtAction(nameof(GetClientMeeting), new { index = meeting.Index }, meeting);
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

        #endregion // Meeting-related
    }
}