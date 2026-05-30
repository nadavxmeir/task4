using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using task4.Models;

/// <summary>
/// DBServices is a class created by me to provides some DataBase Services
/// </summary>
public class DBservices
{
    

    public DBservices()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {

        // read the connection string from the configuration file
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString("myProjDB");
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureGeneral(String spName, SqlConnection con, Dictionary<string, object> paramDic)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        if (paramDic != null)
            foreach (KeyValuePair<string, object> param in paramDic)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value);

            }


        return cmd;
    }


    //--------------------------------------------------------------------------------------------------
    // User Methods
    //--------------------------------------------------------------------------------------------------

     
    // This method inserts a User to the Users table 
     public int InsertUser(User user)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@Name", user.Name);
        paramDic.Add("@Email", user.Email);
        paramDic.Add("@Password", user.Password);
        paramDic.Add("@IsActive", user.Active);

        cmd = CreateCommandWithStoredProcedureGeneral("SP_INSERT_USER_2026", con, paramDic);          // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }


    public List<User> GetUsers()
    {
        SqlConnection con;
        SqlCommand cmd;
        List<User> usersList = new List<User>();

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        // No parameters needed for GET ALL, so we pass null instead of a dictionary
        cmd = CreateCommandWithStoredProcedureGeneral("SP_GET_USERS_2026", con, null); // create the command

        try
        {
            using (SqlDataReader dr = cmd.ExecuteReader()) // execute the command
            {
                while (dr.Read())
                {
                    User u = new User();
                    u.Id = Convert.ToInt32(dr["id"]);
                    u.Name = dr["name"].ToString();
                    u.Email = dr["email"].ToString();
                    u.Password = dr["password"].ToString();
                    u.Active = Convert.ToBoolean(dr["isActive"]);

                    usersList.Add(u);
                }
            }
            return usersList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

     
    // This method updates a User in the Users table 

    public int UpdateUser(int id, string name, string password)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@Id", id);
        paramDic.Add("@Name", name);
        paramDic.Add("@Password", password);

        cmd = CreateCommandWithStoredProcedureGeneral("SP_UPDATE_USER_2026", con, paramDic); // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close(); // close the db connection
            }
        }
    }


     // This method deletes a User in the Users table 

    public int DeleteUser(int id)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@Id", id);
        

        cmd = CreateCommandWithStoredProcedureGeneral("SP_DELETE_USER_2026", con, paramDic); // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close(); // close the db connection
            }
        }
    }

    
    // This method logins a User in the Users table 

    public User LoginUser(string email, string password)
    {
        SqlConnection con;
        SqlCommand cmd;
        User user = null;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@Email", email);
        paramDic.Add("@Password", password);

        cmd = CreateCommandWithStoredProcedureGeneral("SP_LOGIN_USER_2026", con, paramDic); // create the command

        try
        {
            using (SqlDataReader dr = cmd.ExecuteReader()) // execute the command
            {
                // אם dr.Read() מחזיר true, נמצאה שורה תואמת במסד הנתונים
                if (dr.Read())
                {
                    user = new User();
                    user.Id = Convert.ToInt32(dr["id"]);
                    user.Name = dr["name"].ToString();
                    user.Email = dr["email"].ToString();
                    user.Password = dr["password"].ToString();
                    user.Active = Convert.ToBoolean(dr["isActive"]);
                }
            }
            return user; // יחזיר את המשתמש המלא, או null אם הפרטים שגויים
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close(); // close the db connection
            }
        }
    }



    //--------------------------------------------------------------------------------------------------
    // Game Methods
    //--------------------------------------------------------------------------------------------------


    // This method gets all games from the Games table 

    public List<Game> GetGames()
    {
        SqlConnection con;
        SqlCommand cmd;
        List<Game> gamesList = new List<Game>();

        try { con = connect("myProjDB"); }
        catch (Exception ex) { throw (ex); }

        cmd = CreateCommandWithStoredProcedureGeneral("SP_GET_ALL_GAMES_WITH_TAGS_2026", con, null);

        try
        {
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                Game currentGame = null;
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["id"]);
                    if (currentGame == null || currentGame.Id != id)
                    {
                        currentGame = new Game();
                        currentGame.Id = id;
                        currentGame.Name = dr["name"].ToString();
                        currentGame.SteamUrl = dr["steamUrl"].ToString();
                        currentGame.Image = dr["image"].ToString();
                        currentGame.ReleaseDate = dr["releaseDate"].ToString();
                        currentGame.ReviewSummary = dr["reviewSummary"].ToString();
                        currentGame.Price = Convert.ToInt32(dr["price"]);
                        currentGame.Windows = Convert.ToBoolean(dr["windows"]);
                        currentGame.Mac = Convert.ToBoolean(dr["mac"]);
                        currentGame.Linux = Convert.ToBoolean(dr["linux"]);
                        currentGame.Tags = new List<string>();
                        gamesList.Add(currentGame);
                    }
                    if (dr["TagName"] != DBNull.Value)
                    {
                        currentGame.Tags.Add(dr["TagName"].ToString());
                    }
                }
            }
            return gamesList;
        }
        catch (Exception ex) { throw (ex); }
        finally { if (con != null) con.Close(); }
    }


    // This method inserts a new game to the Games table 
    public int InsertGame(Game game)
    {

        SqlConnection con;
        SqlCommand cmd;
        int newGameId = 0;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@Name", game.Name);
        paramDic.Add("@SteamUrl", game.SteamUrl);
        paramDic.Add("@Image", game.Image);
        paramDic.Add("@ReleaseDate", game.ReleaseDate);
        paramDic.Add("@ReviewSummary", game.ReviewSummary);
        paramDic.Add("@Price", game.Price);
        paramDic.Add("@Windows", game.Windows);
        paramDic.Add("@Mac", game.Mac);
        paramDic.Add("@Linux", game.Linux);

        cmd = CreateCommandWithStoredProcedureGeneral("SP_INSERT_GAME_2026", con, paramDic);          // create the command

        try
        {
            object result = cmd.ExecuteScalar();
            if (result != null)
            {
                newGameId = Convert.ToInt32(result);
            }

            // --- שלב ב': הרצת לולאה להכנסת כל התגיות של המשחק הזה ---
            if (newGameId > 0 && game.Tags != null)
            {
                foreach (string tag in game.Tags)
                {
                    Dictionary<string, object> tagParams = new Dictionary<string, object>();
                    tagParams.Add("@GameId", newGameId);
                    tagParams.Add("@TagName", tag);

                    // יצירת פקודה חדשה עבור פרוצדורת התגיות
                    SqlCommand tagCmd = CreateCommandWithStoredProcedureGeneral("SP_INSERT_TAG_2026", con, tagParams);
                    tagCmd.ExecuteNonQuery(); // מכניס את התגית הנוכחית
                }
            }

            return newGameId; // מחזירים את ה-ID של המשחק החדש שנוצר
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    // This method updates a game in the Games table 

    public int UpdateGame(int id, Game game)
    {
        SqlConnection con;
        try { con = connect("myProjDB"); }
        catch (Exception ex) { throw (ex); }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@Id", id);
        paramDic.Add("@Name", game.Name);
        paramDic.Add("@SteamUrl", game.SteamUrl);
        paramDic.Add("@Image", game.Image);
        // המרה בטוחה: אם זה תאריך תקין נשלח בפורמט SQL, אם ריק/לא תקין נשלח NULL
        if (DateTime.TryParse(game.ReleaseDate, out DateTime parsedDate))
        {
            paramDic.Add("@ReleaseDate", parsedDate.ToString("yyyy-MM-dd"));
        }
        else
        {
            paramDic.Add("@ReleaseDate", DBNull.Value);
        }
        paramDic.Add("@ReviewSummary", game.ReviewSummary);
        paramDic.Add("@Price", game.Price);
        paramDic.Add("@Windows", game.Windows);
        paramDic.Add("@Mac", game.Mac);
        paramDic.Add("@Linux", game.Linux);

        SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("SP_UPDATE_GAME_2026", con, paramDic);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            if (numEffected > 0)
            {
                // עדכון התגיות: מחיקה והכנסה מחדש
                Dictionary<string, object> delDic = new Dictionary<string, object> { { "@GameId", id } };
                SqlCommand delCmd = CreateCommandWithStoredProcedureGeneral("SP_DELETE_GAME_TAGS_2026", con, delDic);
                delCmd.ExecuteNonQuery();

                if (game.Tags != null)
                {
                    foreach (string tag in game.Tags)
                    {
                        Dictionary<string, object> tagDic = new Dictionary<string, object> { { "@GameId", id }, { "@TagName", tag } };
                        SqlCommand tagCmd = CreateCommandWithStoredProcedureGeneral("SP_INSERT_TAG_2026", con, tagDic);
                        tagCmd.ExecuteNonQuery();
                    }
                }
            }
            return numEffected;
        }
        catch (Exception ex) { 
            
            throw (ex); }

        finally { if (con != null) con.Close(); }
    }


    // This method deletes a game from the Games table 
    public int DeleteGame(int id)
    {
        SqlConnection con;
        try { con = connect("myProjDB"); }
        catch (Exception ex) { throw (ex); }

        Dictionary<string, object> paramDic = new Dictionary<string, object> { { "@Id", id } };
        SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("SP_DELETE_GAME_2026", con, paramDic);

        try { return cmd.ExecuteNonQuery(); }
        catch (Exception ex) { throw (ex); }
        finally { if (con != null) con.Close(); }
    }




    //--------------------------------------------------------------------------------------------------
    // Games per User Methods
    //--------------------------------------------------------------------------------------------------


    // This method connects user and game and inserts it to the UserGames table 

    public int AddGameToUserCollection(int userId, int gameId)
    {
        SqlConnection con;
        try { con = connect("myProjDB"); }
        catch (Exception ex) { throw (ex); }

        Dictionary<string, object> paramDic = new Dictionary<string, object> {
            { "@UserId", userId },
            { "@GameId", gameId }
        };
        SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("SP_ADD_GAME_TO_USER_2026", con, paramDic);

        try { return cmd.ExecuteNonQuery(); }
        catch (Exception ex) { throw (ex); }
        finally { if (con != null) con.Close(); }
    }


    // This method gets all the games associated with user from UserGames table 

    public List<Game> GetGamesByUser(int userId)
    {
        SqlConnection con;
        SqlCommand cmd;
        List<Game> userGames = new List<Game>();

        try { con = connect("myProjDB"); }
        catch (Exception ex) { throw (ex); }

        Dictionary<string, object> paramDic = new Dictionary<string, object> { { "@UserId", userId } };
        cmd = CreateCommandWithStoredProcedureGeneral("SP_GET_USER_GAMES_WITH_TAGS_2026", con, paramDic);

        try
        {
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                Game currentGame = null;
                while (dr.Read())
                {
                    int id = Convert.ToInt32(dr["id"]);
                    if (currentGame == null || currentGame.Id != id)
                    {
                        currentGame = new Game();
                        currentGame.Id = id;
                        currentGame.Name = dr["name"].ToString();
                        currentGame.SteamUrl = dr["steamUrl"].ToString();
                        currentGame.Image = dr["image"].ToString();
                        currentGame.ReleaseDate = dr["releaseDate"].ToString();
                        currentGame.ReviewSummary = dr["reviewSummary"].ToString();
                        currentGame.Price = Convert.ToInt32(dr["price"]);
                        currentGame.Windows = Convert.ToBoolean(dr["windows"]);
                        currentGame.Mac = Convert.ToBoolean(dr["mac"]);
                        currentGame.Linux = Convert.ToBoolean(dr["linux"]);
                        currentGame.Tags = new List<string>();
                        userGames.Add(currentGame);
                    }
                    if (dr["TagName"] != DBNull.Value)
                    {
                        currentGame.Tags.Add(dr["TagName"].ToString());
                    }
                }
            }
            return userGames;
        }
        catch (Exception ex) { throw (ex); }
        finally { if (con != null) con.Close(); }
    }


    // This method removes game from the UserGames table 

    public int RemoveGameFromCollection(int userId, int gameId)
    {
        SqlConnection con;
        SqlCommand cmd;

        try { con = connect("myProjDB"); }
        catch (Exception ex) { throw (ex); }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserId", userId);
        paramDic.Add("@GameId", gameId);

        cmd = CreateCommandWithStoredProcedureGeneral("SP_REMOVE_GAME_FROM_USER_2026", con, paramDic);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex) { throw (ex); }
        finally { if (con != null) con.Close(); }
    }



    //--------------------------------------------------------------------------------------------------
    // tags Methods
    //--------------------------------------------------------------------------------------------------

    // returns the distinct list of tags of the games in the dataBase
    public List<string> GetAllDistinctTags()
    {
        SqlConnection con;
        SqlCommand cmd;
        List<string> tagsList = new List<string>();

        try { con = connect("myProjDB"); }
        catch (Exception ex) { throw (ex); }

        cmd = CreateCommandWithStoredProcedureGeneral("SP_GET_ALL_DISTINCT_TAGS_2026", con, null);

        try
        {
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    tagsList.Add(dr["TagName"].ToString());
                }
            }
            return tagsList;
        }
        catch (Exception ex) { throw (ex); }
        finally { if (con != null) con.Close(); }
    }

    public List<string> GetGameTags(int gameId)
    {
        List<string> tags = new List<string>();
        SqlConnection con = null;

        try
        {
            con = connect("myProjDB");
            string sql = "SELECT TagName FROM TagGame_2026 WHERE gameId = @id";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", gameId);

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                tags.Add(dr["TagName"].ToString());
            }
            return tags;
        }
        catch (Exception ex) { throw ex; }
        finally { if (con != null) con.Close(); }
    }

    public List<Game> GetGamesByTag(string tag)
    {
        List<Game> gamesList = new List<Game>();
        SqlConnection con;
        SqlCommand cmd;

        try { con = connect("myProjDB"); }
        catch (Exception ex) { throw (ex); }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@TagsList", tag);

        cmd = CreateCommandWithStoredProcedureGeneral("SP_GET_GAMES_BY_TAG_2026", con, paramDic);

        try
        {
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                // כאן אתה ממיר את השורה ב-DB לאובייקט Game בדיוק כמו בפונקציה Read() הרגילה שלך
                Game g = new Game();
                g.Id = Convert.ToInt32(dr["id"]);
                g.Name = dr["name"].ToString();
                g.SteamUrl = dr["steamUrl"].ToString();
                g.Image = dr["image"].ToString();
                g.ReleaseDate = dr["releaseDate"].ToString();
                g.ReviewSummary = dr["reviewSummary"].ToString();
                g.Price = Convert.ToInt32(dr["price"]);
                g.Windows = Convert.ToBoolean(dr["windows"]);
                g.Mac = Convert.ToBoolean(dr["mac"]);
                g.Linux = Convert.ToBoolean(dr["linux"]);
                g.Tags = GetGameTags(g.Id);
                gamesList.Add(g);
            }
            return gamesList;
        }
        catch (Exception ex) { throw (ex); }
        finally { if (con != null) con.Close(); }
    }


    public List<Game> GetRecommendations(int userId)
    {
        List<Game> recommendedGames = new List<Game>();
        SqlConnection con;
        SqlCommand cmd;

        try { con = connect("myProjDB"); }
        catch (Exception ex) { throw (ex); }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@UserId", userId);

        // קוראים ל-SP החכם שלנו
        cmd = CreateCommandWithStoredProcedureGeneral("SP_GET_RECOMMENDED_GAMES_2026", con, paramDic);

        try
        {
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Game g = new Game();
                g.Id = Convert.ToInt32(dr["id"]);
                g.Name = dr["name"].ToString();
                g.SteamUrl = dr["steamUrl"].ToString();
                g.Image = dr["image"].ToString();
                g.ReleaseDate = dr["releaseDate"].ToString();
                g.ReviewSummary = dr["reviewSummary"].ToString();
                g.Price = Convert.ToInt32(dr["price"]);
                g.Windows = Convert.ToBoolean(dr["windows"]);
                g.Mac = Convert.ToBoolean(dr["mac"]);
                g.Linux = Convert.ToBoolean(dr["linux"]);
                g.Tags = GetGameTags(g.Id);
                recommendedGames.Add(g);
            }
            return recommendedGames;
        }
        catch (Exception ex) { throw (ex); }
        finally { if (con != null) con.Close(); }
    }
}

