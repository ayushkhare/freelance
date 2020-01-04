using System;
using System.Collections.Generic;
using System.Linq;
using Kemin_Yolk_Sensor.Model;
using SQLite;

namespace Kemin_Yolk_Sensor.Database
{
    public class DatabaseManager
    {
        protected static DatabaseManager manager;
        private SQLiteConnection connection = null;

        static DatabaseManager()
        {
            manager = new DatabaseManager();
        }

        protected DatabaseManager()
        {
            connection = new SQLiteConnection(Constants.Constants.DB_FILE_PATH);
            connection.CreateTable<User>();
        }

        public static int SaveUser(User user)
        {            
            manager.connection.Insert(user);            
            return user.Id;
        }    

        public static User GetUser(int id)
        {
            return manager.connection.Get<User>(u => u.Id == id);
        }

        public static int UpdateUser(User user)
        {
            manager.connection.Update(user);
            return user.Id;
        }

        public static User FindUser(string username, string password)
        {
            return manager.connection.Table<User>().FirstOrDefault(
                u => u.Username == username && u.Password == password);
        }

        public static List<User> GetUsers()
        {
            return manager.connection.Table<User>().ToList();
        }

        public static int DeleteUser(int id)
        {
            User user = GetUser(id);
            return manager.connection.Delete(user);
        }
    }
}
