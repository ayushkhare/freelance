using System;
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
    }
}
