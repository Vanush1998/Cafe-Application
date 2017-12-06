using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Device.Location;
using Newtonsoft.Json;

namespace CafeApplication
{

    public class User : Person
    {

        public int Bill { get; set; }
        private string password = string.Empty;
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                if (value.Length < 8 || value.Length > 30)
                    throw new ArgumentException("Password must contain 8-30 characters");
                this.password = value;
            }
        }
        [JsonProperty]
        public int adminSetterID;
        public int Cash { get; set; }
        public bool isAdmin;
        public bool isBlocked;
        public Dictionary<string, int> OrderList = new Dictionary<string, int>();

        public List<string> Favorite { get; set; }

        public static List<User> users = new List<User>();
        static User()
        {
            User.users = new List<User>();
            User.LoadUsers();
        }

        public User(string name, string lastname, string username, string password) : base(name, lastname)
        {
            if (users == null)
            { users = new List<User>(); }
            for (int i = 0; i < User.users.Count; i++)
            {
                if (User.users[i].UserName.Equals(username))
                    throw new ArgumentException("Username is already taken");
            }
            if (password.Length < 8 || password.Length > 30)
                throw new ArgumentException("Password must contain 8-30 characters");

            OrderList = new Dictionary<string, int>();
            if (users.Count != 0)
                this.ID = User.users[User.users.Count - 1].ID + 1;
            else
                this.ID = 1;
            OrderList = new Dictionary<string, int>();
            Favorite = new List<string>();
            this.isBlocked = false;
            users.Add(this);
            this.UserName = username;
            this.Password = password;
            this.Bill = 0;

        }
        public User() { }
        private User(int userId, bool isAdmin, int adminSetterId,
            bool isBlocked, int bill, string username, string password,
            int cash, string name, string lastName)
        {
            this.ID = userId;
            this.isAdmin = isAdmin;
            this.adminSetterID = adminSetterId;
            this.isBlocked = isBlocked;
            this.Bill = bill;
            this.UserName = username;
            this.password = password;
            this.Cash = cash;
            this.Name = name;
            this.LastName = lastName;
        }


        public static User LogIn(string username, string password)
        {
            for (int i = 0; i < User.users.Count(); i++)
            {
                if (User.users[i].UserName.Equals(username) &&
                    User.users[i].Password.Equals(password))
                {
                    return User.users[i];
                }
            }
            throw new ArgumentException("Wrong username or pssword");
        }
        //Admin functions
        public void DeleteUser(string username)
        {
            for (int i = 0; i < User.users.Count; i++)
            {
                if (username.Equals(User.users[i].UserName))
                {
                    User.users.RemoveAt(i);
                    return;
                }
            }
            throw new ArgumentException("User was not found");
        }
        public void DeletCafe(Cafe cafe)
        {
            this.Favorite.Remove(cafe.Name);
            Cafe.cafes.Remove(cafe);
        }
        // end Admin functions
        public void PayBill(Cafe cafe)
        {
            if (this.Cash < this.Bill)
            {
                throw new Exception("You don't have enough money");
            }
            else
            {
                this.Cash -= this.Bill;
                this.OrderList.Clear();
            }
        }
        public void AddMoney(int money)
        {
            this.Cash += money;
        }
        public override string ToString()
        {
            if (isAdmin)
                return this.Name + " " + this.LastName + " (Admin)";
            else
                return this.Name + " " + this.LastName + " (User)";
        }
        public static void LoadUsers()
        {
            string queryString = "exec UDSP_LoadUsers";
            SqlCommand command = new SqlCommand(
            queryString, DbConnection.GetConnection());
            SqlDataReader reader = command.ExecuteReader();
            List<User> loadedUsers = new List<User>();
            while (reader.Read())
            {
                int userId = reader.GetInt32(0);
                bool isAdmin = reader.GetBoolean(1);
                int adminSetter = reader.GetInt32(2);
                bool isBlocked = reader.GetBoolean(3);
                int bill = reader.GetInt32(4);
                string username = reader.GetString(5);
                string password = reader.GetString(6);
                int cash = reader.GetInt32(7);
                string name = reader.GetString(8);
                string lastname = reader.GetString(9);
                loadedUsers.Add(new User(
                    userId,
                    isAdmin,
                    adminSetter,
                    isBlocked,
                    bill,
                    username,
                    password,
                   cash,
                    name,
                    lastname));
            }
            User.users = loadedUsers;
        }
    }
}
