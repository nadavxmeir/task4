using Microsoft.AspNetCore.Mvc;
using task4.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace task4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        // GET: api/<GamesController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(Game.Read());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאת שרת: {ex.Message}");
            }
        }

        // GET api/<GamesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("getByName")]
        public IEnumerable<Game> GetByName(string name)
        {
            Game game = new Game();
            return game.GetByName(name);
        }

        // POST api/<GamesController>
        [HttpPost]
        public IActionResult Post([FromBody] Game game)
        {
            if (game == null) return BadRequest();
            bool isInserted = game.AddGame();

            if (isInserted)
            {
                // אם הצליח - מחזירים 200 OK או 201 Created
                return Ok(true);
            }
            else
            {
                // אם נכשל בגלל כפילות - מחזירים 409 Conflict
                return Conflict("Your game already exists in the database.");
            }
        }

        // PUT api/<GamesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Game game)
        {
            if (game == null) return BadRequest("Invalid game data");

            bool isUpdated = Game.UpdateGame(id, game);
            if (isUpdated)
            {
                return Ok("המשחק עודכן בהצלחה");
            }
            return NotFound("המשחק לא נמצא בספרייה");
        }

        // DELETE api/<GamesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool success = Game.Delete(id);
            if (success)
            {
                return Ok("המשחק נמחק בהצלחה");
            }
            return NotFound("המשחק לא נמצא");
        }

        // דרישה חדשה: שליפת רשימת התגיות הייחודיות במערכת (Unique Tags)
        [HttpGet("GetAllTags")]
        public IActionResult GetAllTags()
        {
            try
            {
                DBservices dbs = new DBservices();
                return Ok(dbs.GetAllDistinctTags());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאת שרת: {ex.Message}");
            }
        }

        [HttpGet("GetByTags")]
        public IActionResult GetByTags(string tags)
        {
            try
            {
                // קריאה למודל שעכשיו שולח ישירות ל-SQL
                List<Game> games = Game.GetByTags(tags);

                if (games != null && games.Count > 0)
                {
                    return Ok(games);
                }
                return NotFound("לא נמצאו משחקים עם התגית המבוקשת.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאת שרת: {ex.Message}");
            }
        }
    }

}
