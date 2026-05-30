using Microsoft.AspNetCore.Mvc;
using task4.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace task4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/<UsersController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // שים לב! אנחנו לא עושים new User() בשום מקום.
                // אנחנו פונים ישירות למחלקה User וקוראים לפונקציה הסטטית שלה.
                List<User> usersList = task4.Models.User.Read();

                // בדיקה אם הייתה שגיאה במסד הנתונים (הפונקציה החזירה null)
                if (usersList == null)
                {
                    return StatusCode(500, "אירעה שגיאה בשליפת הנתונים ממסד הנתונים.");
                }

                // החזרת הנתונים ללקוח (ל-Swagger או לאתר) עם סטטוס 200
                return Ok(usersList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאת שרת: {ex.Message}");
            }
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsersController>
        [HttpPost("register")]
        public IActionResult Register([FromBody] User newUser)
        {
            if (newUser == null) return BadRequest("נתוני משתמש לא תקינים");
            bool isRegistered = newUser.Register();

            if (isRegistered) {
                return Ok(true);
            }

            return Conflict("המשתמש כבר קיים");
        }

        // POST: api/Users/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest("נתוני התחברות לא תקינים");
            }

            // הפעלת הפונקציה הסטטית מהמודל עם הנתונים שהגיעו מה-DTO
            User loggedInUser = task4.Models.User.Login(loginDto.Email, loginDto.Password);

            if (loggedInUser != null)
            {
                // התחברות הצליחה! מחזירים את אובייקט המשתמש המלא (כולל ה-ID שלו לעבודה ב-Frontend)
                return Ok(loggedInUser);
            }

            // אם חזר null, סימן שהאימייל או הסיסמה לא נכונים במסד הנתונים
            return Unauthorized("אימייל או סיסמה שגויים, גישה נדחתה.");
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateProfileDTO profileDto)
        {
            if (profileDto == null) return BadRequest("נתוני משתמש לא תקינים");

            // מוודאים שה-Id מה-URL מוזרק לתוך המשתמש שרוצים לעדכן
            User userToUpdate = new User();
            userToUpdate.Id = id;
            userToUpdate.Name = profileDto.Name;
            userToUpdate.Password = profileDto.Password;
            bool isUpdated = userToUpdate.Update();

            if (isUpdated)
            {
                return Ok("המשתמש עודכן בהצלחה בשרת");
            }

            return NotFound("המשתמש לא נמצא, או שלא בוצע שום שינוי בנתונים");
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool isDeleted = task4.Models.User.Delete(id);

            if (isDeleted)
            {
                return Ok("המשתמש נמחק בהצלחה ממסד הנתונים");
            }

            return NotFound("המחיקה נכשלה. המשתמש לא נמצא במערכת");
        }


        // דרישה: הוספת משחק לאוסף של משתמש ספציפי (מפתח זר רבים לרבים)
        [HttpPost("AddGameToCollection")]
        public IActionResult AddGameToCollection(int userId, int gameId)
        {
            try
            {
                DBservices dbs = new DBservices();
                int rowsAffected = dbs.AddGameToUserCollection(userId, gameId);
                if (rowsAffected > 0) return Ok("המשחק נוסף לאוסף המשתמש בהצלחה");
                return Conflict("המשחק כבר קיים בספרייה שלך");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאת שרת: {ex.Message}");
            }
        }

        [HttpDelete("RemoveGameFromCollection")]
        public IActionResult RemoveGameFromCollection(int userId, int gameId)
        {
            try
            {
                DBservices dbs = new DBservices();
                int rowsAffected = dbs.RemoveGameFromCollection(userId, gameId);

                if (rowsAffected > 0)
                    return Ok("המשחק הוסר מהאוסף שלך בהצלחה");

                return NotFound("המשחק לא נמצא באוסף שלך");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאת שרת: {ex.Message}");
            }
        }

        // דרישה: GET (GetUserGames) - קבלת כל המשחקים המשויכים למשתמש
        [HttpGet("GetUserGames/{userId}")]
        public IActionResult GetUserGames(int userId)
        {
            try
            {
                DBservices dbs = new DBservices();
                List<Game> userGames = dbs.GetGamesByUser(userId);
                return Ok(userGames);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאת שרת: {ex.Message}");
            }
        }

        // דרישה: מנגנון המלצות מבוסס ניתוח תגיות המשתמש (Recommendations)
        // GET: api/Users/Recommendations?userId=1
        [HttpGet("GetRecommendations/{userId}")]
        public IActionResult Recommendations(int userId)
        {
            try
            {
                // קריאה למודל שעכשיו שולף את ההמלצות ישירות באמצעות השאילתה ב-DB
                List<Game> recommendedGames = Game.GetRecommendations(userId);

                // מחזירים רשימה (יכולה להיות גם ריקה אם אין המלצות, זה תקין)
                return Ok(recommendedGames);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאת שרת: {ex.Message}");
            }
        }
    }
}
