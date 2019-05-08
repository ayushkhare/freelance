using System;
using SQLite;

namespace Kemin_Yolk_Sensor.Model
{
    [Table("user")]
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime ExpiryDate { get; set; }
        public Role Role { get; set; }

        public User()
        {
        }

        public User(string Username, string Password, DateTime ExpiryDate, 
            Role Role)
        {
            this.Username = Username;
            this.Password = Password;
            this.ExpiryDate = ExpiryDate;
            this.Role = Role;

        }

        public override string ToString()
        {
            return $"[User: Id={Id}, Username={Username}, Password={Password}, ExpiryDate={ExpiryDate}, Roles={Role}]";
        }

    }
}
