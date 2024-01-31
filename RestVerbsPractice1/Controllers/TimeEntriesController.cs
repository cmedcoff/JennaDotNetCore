using RestVerbsPractice1.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace RestVerbsPractice1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TimeEntriesController : ControllerBase
    {
        private readonly ILogger<TimeEntriesController> _logger;
        private readonly ITimeEntryRepository _timeEntryRepository;

        public TimeEntriesController(ILogger<TimeEntriesController> logger,
                ITimeEntryRepository timeEntryRepository)
        {
            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));
            _timeEntryRepository = timeEntryRepository ?? 
                throw new ArgumentNullException(nameof(timeEntryRepository));
        }

        // GET: api/<TimeEntriesController>
        [HttpGet]
        public ActionResult<IEnumerable<TimeEntry>> Get()
        {
            try
            {
                return Ok(_timeEntryRepository.List());
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get time entries: {e}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        // GET api/<TimeEntriesController>/5
        [HttpGet("{id}")]
        public ActionResult<TimeEntry?> Get(int id)
        {
            try
            {
                _logger.LogInformation("Getting a time entry");
                TimeEntry? timeEntry = _timeEntryRepository.Find(id);
                if (timeEntry == null)
                {
                    return NotFound();
                }

                return Ok(timeEntry);    
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get time entry: {e}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
       }

        // POST api/<TimeEntriesController>
        [HttpPost(Name="Create")]
        public ActionResult<TimeEntry> Post([FromBody] TimeEntryPost timeEntry)
        {
            try
            {
                _logger.LogInformation("Creating a new time entry");

                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                TimeEntry toCreate = new TimeEntry() {
                    Description = timeEntry.Description,
                    StartTime = timeEntry.StartTime,
                    EndTime = timeEntry.EndTime
               };

                TimeEntry created = _timeEntryRepository.Create(toCreate);
                return CreatedAtRoute("Create", new {id=created.Id}, created);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to create time entry: {e}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
       }

        // PUT api/<TimeEntriesController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] TimeEntry timeEntry)
        {
            try
            {
                _logger.LogInformation("Update/PUT a time entry");
                
                // interesting to note that I get an EF exceptions
                // if I leave this block in.  I'm not sure why,
                // need to investigate. I think it has todo with the scope of
                // the context.
                //if (_timeEntryRepository.Find(id) == null)
                //{
                //   return NotFound();
                //}
                //

                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                 _timeEntryRepository.Update(id, timeEntry);
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to update time entry: {e}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPatch("{id}")]
        public ActionResult Patch(int id, [FromBody] JsonPatchDocument<TimeEntry> timeEntry)
        {
            try
            {
                _logger.LogInformation("Update/PATCH a time entry");

                TimeEntry? timeEntryToUpdate = _timeEntryRepository.Find(id);
                if (timeEntryToUpdate == null)
                {
                    _logger.LogInformation("Time entry not found");
                    return NotFound();
                }

                // There's an overload which allows passing the ModelState
                // this is so that the patching can be validated.
                // Otherwise the patching might contain invalid patch operations
                // and we won't know about those, unless we use the ModelState
                // to validate data resulting from the patching.
                timeEntry.ApplyTo(timeEntryToUpdate, ModelState);

                if (!ModelState.IsValid)
                {
                    _logger.LogInformation("Invalid ModelState after patching");
                    return BadRequest(ModelState);
                }

                _timeEntryRepository.Update(id, timeEntryToUpdate);
                return NoContent();
                
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to update time entry: {e}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }


        // DELETE api/<TimeEntriesController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _logger.LogInformation("Delete a time entry");
                if (_timeEntryRepository.Find(id) == null)
                {
                    return NotFound();
                }

                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
 
                _timeEntryRepository.Delete(id);
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to delete time entry: {e}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
    }
}
