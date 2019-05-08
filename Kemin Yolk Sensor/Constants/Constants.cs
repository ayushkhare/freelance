using System;
using System.IO;

namespace Kemin_Yolk_Sensor.Constants
{
    public class Constants
    {
        public static readonly string DB_FILE_PATH = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Personal),
            "users.db");
        public static readonly string ADMIN_USERNAME = "admin";
        public static readonly string ADMIN_PASSWORD = "admin";
    }
}
