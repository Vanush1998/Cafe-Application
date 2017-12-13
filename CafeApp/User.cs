using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

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
        public int adminSetterID;
        public int Cash { get; set; }
        public bool isAdmin;
        public bool isBlocked;
        public Dictionary<string, int> OrderList = new Dictionary<string, int>();

        public List<string> Favorite { get; set; }

        public static List<User> users;
        static User()
        {
            User.users = new List<User>();
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

        public void DeletCafe(Cafe cafe)
        {
            this.Favorite.Remove(cafe.Name);
            string queryString = String.Format("exec UDSP_DeleteCafe {0}", cafe.ID);
            SqlCommand command = new SqlCommand(
            queryString, DbConnection.GetConnection());
            command.ExecuteReader();
            Cafe.cafes.Remove(cafe);
        }
        //end Admin functions
        public void PayBill()
        {
            if (this.Cash < this.Bill)
            {
                throw new Exception("You don't have enough money");
            }
            else
            {
                this.Cash -= this.Bill;
                this.Bill = 0;
                string queryString1 = @"UPDATE dbo.Users SET bill = 0 WHERE userId = " + this.ID;
                string queryString2 = String.Format(@"UPDATE dbo.Users SET cash = {0} WHERE userId = " + this.ID, this.Cash);
                SqlCommand command = new SqlCommand(
                queryString1, DbConnection.GetConnection());
                command.ExecuteReader();
                command = new SqlCommand(queryString2, DbConnection.GetConnection());
                command.ExecuteReader();
                this.OrderList.Clear();
            }
        }
        public void AddMoney(int money)
        {
            this.Cash += money;
            string queryString = String.Format(@"UPDATE dbo.Users SET cash = {0} WHERE userId = " + this.ID, this.Cash);
            SqlCommand command = new SqlCommand(
            queryString, DbConnection.GetConnection());
            command.ExecuteReader();
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
                queryString = "exec UDSP_SelectFavorites " + userId;
                command = new SqlCommand(
                queryString, DbConnection.GetConnection());
                SqlDataReader reader1 = command.ExecuteReader();
                User user = new User(userId,
                    isAdmin,
                    adminSetter,
                    isBlocked,
                    bill,
                    username,
                    password,
                    cash,
                    name,
                    lastname);
                user.Favorite = new List<string>();
                while (reader1.Read())
                {
                    user.Favorite.Add(reader1.GetString(0));
                }
                loadedUsers.Add(user);
            }
            User.users = loadedUsers;
        }
        public static int InsertUser(bool isAdmin, int adminSetterId,
            bool isBlocked, int bill, string username, string password,
            int cash, string name, string lastName)
        {
            string queryString = String.Format(@"EXEC dbo.UDSP_InsertUser @name = {0},
                                                        @lastName = {1}, 
                                                        @userName = {2}, 
                                                        @password = {3}, 
                                                        @cash = {4}, 
                                                        @bill = {5}, 
                                                        @isAdmin = {6}, 
                                                        @adminSetterId = {7}, 
                                                        @isBlocked = {8} ",
                                                        name,
                                                        lastName,
                                                        username,
                                                        password,
                                                        cash,
                                                        bill,
                                                        isAdmin,
                                                        adminSetterId,
                                                        isBlocked);
            SqlCommand command = new SqlCommand(
            queryString, DbConnection.GetConnection());
            SqlDataReader reader = command.ExecuteReader();
            int userId = 0;
            while (reader.Read())
            {
                userId = reader.GetInt32(0);
            }
            return userId;
        }
        public static void UpdateUser(User user)
        {

        }
        public static void DeleteUser(User user)
        {
            if (User.users.Remove(user))
            {
                string queryString = String.Format("exec UDSP_DeleteUser {0}", user.ID);
                SqlCommand command = new SqlCommand(
                queryString, DbConnection.GetConnection());
                SqlDataReader reader = command.ExecuteReader();
            }
            else
            {
                throw new ArgumentException("User was not found");            
            }
        }
    }
}
