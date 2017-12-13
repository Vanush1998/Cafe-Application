using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Device.Location;
namespace CafeApplication
{
    public class Cafe
    {
        public Cafe() { }
        public Cafe(string name, string address, string phone, TimeSpan open, TimeSpan close,
            GeoCoordinate location, bool[] workdays, string email, string webpage)
        {
            if (email.Equals("eMail"))
                email = "Doesn't have";
            if (webpage.Equals("eMail"))
                webpage = "Doesn't have";
            for (int i = 0; i < Cafe.cafes.Count; i++)
            {
                if (Cafe.cafes[i].Name.Equals(name))
                    throw new Exception("Cafe with this name already exists");
                if (Cafe.cafes[i].Address.ToLower().Equals(address.ToLower()))
                    throw new Exception("Cafe with this address already exists");
            }
            if (this.Reviews == null)
                this.Reviews = new List<String>();
            if (email.Equals("eMail"))
                this.Email = "Doesn,t have";
            if (webpage.Equals("Web page"))
                this.WebPage = "Doesn't have";
            Name = name;
            Address = address;
            Phone = phone;
            Open = open;
            Close = close;
            Rating = 5;
            this.WorkDays = new bool[6];
            Menu = new Dictionary<string, int>();
            this.AddDefaultMenu();
            this.Location = location;
            this.Email = email;
            this.WebPage = webpage;
            this.Rates = new Dictionary<int, int>();
            this.WorkDays = workdays;
        }
        static Cafe()
        {
            Cafe.cafes = new List<Cafe>();
        }
        public GeoCoordinate Location { get; set; }
        public TimeSpan Open = new TimeSpan();
        public TimeSpan Close = new TimeSpan();
        public bool[] WorkDays { get; set; }
        public Dictionary<string, int> Menu { get; set; }
        public List<String> Reviews { get; set; }
        public Dictionary<int, int> Rates { get; set; }
        public static List<Cafe> cafes;
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string WebPage { get; set; }
        public double Rating { get; set; }
        private void AddDefaultMenu()
        {
            Menu.Add("Salad Cesar", 850);
            Menu.Add("Salad with meat", 900);
            Menu.Add("Pizza", 3500);
            Menu.Add("Mohito", 2500);
            Menu.Add("Cake", 1600);
            Menu.Add("Wine", 2000);
            Menu.Add("Sushi", 8000);
            Menu.Add("Juce", 600);
            Menu.Add("Hot chocolatte", 700);
            Menu.Add("Ice latte", 600);
            Menu.Add("Whiskey", 1200);
            Menu.Add("Fri", 500);
            Menu.Add("Steak", 2000);
        }
        public void Rate()
        {
            int sum = 0;
            foreach (KeyValuePair<int, int> entry in this.Rates)
                sum += entry.Value;
            this.Rating = sum / (this.Rates.Count);
        }
        public string OpenStatus()
        {
            if (DateTime.Now.TimeOfDay >= Open && DateTime.Now.TimeOfDay <= Close)
                return "Open now";
            else
                return "Close now";
        }
        public static void SortByRate()
        {
            cafes.Sort(delegate (Cafe c1, Cafe c2)
            {
                if (c1.Rating > c2.Rating)
                    return -1;
                else if (c1.Rating < c2.Rating)
                    return 1;
                else
                    return 0;
            });
        }
        public static void SortByDistance(GeoCoordinate activeUserLocation)
        {
            cafes.Sort(delegate (Cafe c1, Cafe c2)
            {
                if (c1.Location.GetDistanceTo(activeUserLocation) > c2.Location.GetDistanceTo(activeUserLocation))
                    return 1;
                else if (c1.Location.GetDistanceTo(activeUserLocation) < c2.Location.GetDistanceTo(activeUserLocation))
                    return -1;
                else
                    return 0;
            });
        }
        public static int InsertCafe(string name, string address, string phone, TimeSpan open, TimeSpan close,
            GeoCoordinate location, bool[] workdays, string email, string webpage)
        {
            int insertedLocationId = CafeApplication.Location.InsertLocation(location.Latitude, location.Longitude, address);
            string queryString = String.Format(@"EXEC dbo.UDSP_InsertCafe 
                                                    @openTime = '{0}',
                                                    @closeTime = '{1}',
                                                    @locationId = {2},
                                                    @name = '{3}',
                                                    @phone = '{4}',
                                                    @email = '{5}',
                                                    @webPage = '{6}'",
                                                    open.ToString(@"hh\:mm"),
                                                    close.ToString(@"hh\:mm"),
                                                    insertedLocationId,
                                                    name,
                                                    phone,
                                                    email,
                                                    webpage);
            SqlCommand command = new SqlCommand(
            queryString, DbConnection.GetConnection());
            SqlDataReader reader = command.ExecuteReader();
            int cafeId = 0;
            while (reader.Read())
            {
                cafeId = reader.GetInt32(0);
            }
            return cafeId;
        }
        public static void LoadCafes()
        {
            string queryString = @"EXEC dbo.UDSP_LoadCafes";
            SqlCommand command = new SqlCommand(
            queryString, DbConnection.GetConnection());
            SqlDataReader reader = command.ExecuteReader();
            List<Cafe> loadedCafes = new List<Cafe>();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                TimeSpan open = new TimeSpan(0, int.Parse(reader.GetString(1).Split(':')[0]), int.Parse(reader.GetString(1).Split(':')[1]), 0);
                TimeSpan close = new TimeSpan(0, int.Parse(reader.GetString(2).Split(':')[0]), int.Parse(reader.GetString(2).Split(':')[1]), 0);
                int locationId = reader.GetInt32(3);
                string name = reader.GetString(4);
                string phone = reader.GetString(5);
                string email = reader.GetString(6);
                string webPage = reader.GetString(7);
                
                //reading locations 
                queryString = @"SELECT  longitude, latitude, addressName
                                FROM    dbo.Locations
                                WHERE   locationId = " + locationId;
                command = new SqlCommand(
                queryString, DbConnection.GetConnection());
                SqlDataReader locationReader = command.ExecuteReader();
                double longitude = 0;
                double latitude = 0;
                string addressName = "";
                while (locationReader.Read())
                {
                    longitude = locationReader.GetDouble(0);
                    latitude = locationReader.GetDouble(1);
                    addressName = locationReader.GetString(2);
                }
                GeoCoordinate location = new GeoCoordinate(latitude, longitude);
                //loading the rating of the cafe (calculating average)                
                double rating;
                queryString = @"SELECT  AVG(rate)
                                FROM    dbo.CafeRates
                                WHERE cafeId = " + id + "group by cafeId";
                command = new SqlCommand(
                queryString, DbConnection.GetConnection());
                SqlDataReader ratingReader = command.ExecuteReader();
                while (ratingReader.Read())
                {
                    rating = ratingReader.GetInt32(0);
                }
                Dictionary<int, int> rates = new Dictionary<int, int>();
                queryString = @"SELECT  userId ,
                                        rate
                                FROM    dbo.CafeRates
                                WHERE   cafeId = " + id;
                command = new SqlCommand(
                queryString, DbConnection.GetConnection());
                SqlDataReader ratesReader = command.ExecuteReader();
                while (ratesReader.Read())
                {
                    rates.Add(ratesReader.GetInt32(0), ratesReader.GetInt32(1));
                }
                //loading the workdays of the cafe
                bool[] workDays = new bool[7];
                queryString = @"SELECT  monday ,
                                        tuesday ,
                                        wednesday ,
                                        thusrday ,
                                        friday ,
                                        saturday ,
                                        sunday
                                FROM    dbo.Workdays
                                WHERE   cafeId = 1";
                command = new SqlCommand(
                queryString, DbConnection.GetConnection());
                SqlDataReader workDaysReader = command.ExecuteReader();
                while (workDaysReader.Read())
                {
                    workDays[0] = workDaysReader.GetBoolean(0);
                    workDays[1] = workDaysReader.GetBoolean(1);
                    workDays[2] = workDaysReader.GetBoolean(2);
                    workDays[3] = workDaysReader.GetBoolean(3);
                    workDays[4] = workDaysReader.GetBoolean(4);
                    workDays[5] = workDaysReader.GetBoolean(5);
                    workDays[6] = workDaysReader.GetBoolean(6);
                }
                //loading the menu of the cafe
                Dictionary<string, int> menu = new Dictionary<string, int>();
                queryString = @"SELECT  foodName ,
                                        price
                                FROM    dbo.Foods
                                WHERE   cafeId = " + id;
                command = new SqlCommand(
                queryString, DbConnection.GetConnection());
                SqlDataReader menuReader = command.ExecuteReader();
                while (menuReader.Read())
                {
                    menu.Add(menuReader.GetString(0), menuReader.GetInt32(1));
                }
                //loading the reviews of the cafe from the users
                List<String> reviews = new List<string>();
                queryString = @"SELECT  Users.name + ' ' + Users.lastName ,
                                        review
                                FROM    dbo.Reviews
                                        JOIN dbo.Users ON Users.userId = Reviews.userId
                                WHERE   cafeId = " + id;
                command = new SqlCommand(
                queryString, DbConnection.GetConnection());
                SqlDataReader reviewsReader = command.ExecuteReader();
                while (reviewsReader.Read())
                {
                    reviews.Add(reviewsReader.GetString(0) + " : " + reviewsReader.GetString(1));
                }
                Cafe cafe = new Cafe(name, addressName, phone,
                    open, close, location,
                    workDays, email, webPage);
                cafe.Menu = menu;
                cafe.Rates = rates;
                cafe.Reviews = reviews;
                loadedCafes.Add(cafe);
            }
            Cafe.cafes = loadedCafes;
        }
    }
}
