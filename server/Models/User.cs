using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
namespace task4.Models
{
    public class User
    {
        private int id;
        private string name;
        private string email;
        private string password;
        private bool active;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public bool Active { get => active; set => active = value; }

        private static string HashPassword(string plainPassword)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(plainPassword));
                return Convert.ToBase64String(bytes);
            }
        }

        public bool Register()
        {
            if (string.IsNullOrEmpty(this.Name) || !Regex.IsMatch(this.Name, @"^[a-zA-Zא-ת\s]{2,}$")) return false;
            // ולידציה: סיסמה לפחות 8 תווים, אות גדולה ומספר
            if (string.IsNullOrEmpty(this.Password) || !Regex.IsMatch(this.Password, @"^(?=.*[A-Z])(?=.*\d).{8,}$")) return false;

            this.Password = HashPassword(this.Password);
            DBservices dbs = new DBservices();

            try
            {
                int rowsAffected = dbs.InsertUser(this);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }

        public static User Login(string email, string password)
        {
            try
            {
                string hashedInput = HashPassword(password);
                DBservices dbs = new DBservices();
                return dbs.LoginUser(email, hashedInput);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<User> Read()
        {
            try
            {
                DBservices dbs = new DBservices();
                return dbs.GetUsers();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool Update()
        {
            try
            {
                if (!string.IsNullOrEmpty(this.Name) && !Regex.IsMatch(this.Name, @"^[a-zA-Zא-ת\s]{2,}$")) return false;
                if (!string.IsNullOrEmpty(this.Password) && !Regex.IsMatch(this.Password, @"^(?=.*[A-Z])(?=.*\d).{8,}$")) return false;

                DBservices dbs = new DBservices();
                this.Password = HashPassword(this.Password);

                int rowsAffected = dbs.UpdateUser(this.Id, this.Name, this.Password);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static bool Delete(int id)
        {
            try
            {
                DBservices dbs = new DBservices();

                int rowsAffected = dbs.DeleteUser(id);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }

}
