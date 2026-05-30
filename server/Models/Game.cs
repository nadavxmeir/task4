namespace task4.Models
{
    public class Game
    {
        int id;
        string name;
        string steamUrl;
        string image;
        string releaseDate;
        string reviewSummary;
        int price;
        List<string> tags;
        bool windows;
        bool mac;
        bool linux;

 
        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string SteamUrl { get => steamUrl; set => steamUrl = value; }
        public string Image { get => image; set => image = value; }
        public string ReleaseDate { get => releaseDate; set => releaseDate = value; }
        public string ReviewSummary { get => reviewSummary; set => reviewSummary = value; }
        public int Price { get => price; set => price = value; }
        public List<string> Tags { get => tags; set => tags = value; }
        public bool Windows { get => windows; set => windows = value; }
        public bool Mac { get => mac; set => mac = value; }
        public bool Linux { get => linux; set => linux = value; }

        public bool AddGame()
        {
            try
            {
                DBservices dbs = new DBservices();

                // מפעילים את הפונקציה ב-DBservices ומקבלים את ה-ID החדש שנוצר
                int newId = dbs.InsertGame(this);

                if (newId > 0)
                {
                    this.Id = newId; // מעדכנים את האובייקט הנוכחי ב-ID שקיבל מה-SQL
                    return true;
                }
                return false;
            }
            catch (Exception) { return false; }
        }
        public static List<Game> Read()
        {
            DBservices dbs = new DBservices();
            return dbs.GetGames();
        }
        public static bool Delete(int id)
        {
            DBservices dbs = new DBservices();
            int result = dbs.DeleteGame(id);
            if (result > 0)
            {
                return true;
            }
               return false;

        }

        public List<Game> GetByName(string name)
        {
            if (string.IsNullOrEmpty(name)) return new List<Game>();

            var allGamesFromDB = Read();
            return allGamesFromDB.Where(g => g.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public static bool UpdateGame(int id, Game updatedGame)
        {
            DBservices dbs = new DBservices();
            return dbs.UpdateGame(id, updatedGame) > 0;
        }




        public static List<Game> GetByTags(string tag)
        {
            DBservices dbs = new DBservices();
            return dbs.GetGamesByTag(tag);
        }

        // דרישה: מנגנון המלצות חכם מבוסס תגיות של המשתמש
        public static List<Game> GetRecommendations(int userId)
        {
            DBservices dbs = new DBservices();
            return dbs.GetRecommendations(userId);
        }
    }
}
