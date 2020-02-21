using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using TwentyFiveHours.API.Azure;
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

            if (client == null || client.ProfileImagePath.Equals(string.Empty))
                return NotFound();

            var imageContent = System.IO.File.ReadAllBytes(client.ProfileImagePath);

            return File(imageContent, "image/jpeg");
        }

        [HttpPost("{id:length(24)}/profile-image")]
        public async Task<IActionResult> PostClientProfileImage(string id, IFormFile file)
        {
            var path = Path.GetRandomFileName();
            using (var stream = new FileStream(path, FileMode.Create))
                await file.CopyToAsync(stream);

            var client = this._service.Get(id);
            client.ProfileImagePath = path;
            this._service.Update(id, client);

            return Ok(new { Count = 1, Size = file.Length, Path = path });
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

            return CreatedAtAction(nameof(GetClientMeeting), new { id, index = meeting.Index }, meeting);
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

        [HttpPost("{id:length(24)}/meetings/{index}/upload-audio")]
        public async Task<IActionResult> PostClientMeetingAudio(string id, int index, IFormFile file)
        {
            var path = Path.GetRandomFileName();
            using (var stream = new FileStream(path, FileMode.Create))
                await file.CopyToAsync(stream);

            System.Diagnostics.Debug.WriteLine("file created");

            var client = this._service.Get(id);

            var speech = new SpeechRecognitionWrapper("key", "regionstring");
            string textPath = await speech.RecognizeIntoFile(path);
            System.Diagnostics.Debug.WriteLine("text file created");

            using (var text = new TextAnalyticsWrapper("key", "endpoint"))
            {
                client.Meetings[index].Keywords = text.GetKeyPhrasesFromFile(textPath);
                client.Meetings[index].Summary = text.GetSummariesFromFile(textPath, 3);
            }

            client.Meetings[index].RawAudioLocation = path;
            client.Meetings[index].RawTextLocation = textPath;
            this._service.Update(id, client);

            return Ok(new { Count = 1, Size = file.Length, Path = path });
        }

        #endregion // Meeting-related
    }
}