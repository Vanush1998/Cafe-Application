using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Device.Location;
using Newtonsoft.Json;
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
            cafes.Add(this);
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
        public List<String> Reviews;
        [JsonProperty]
        public Dictionary<int, int> Rates { get; set; }
        public static List<Cafe> cafes = new List<Cafe>();
        public string Name { get;  set; }
        public string Address { get; set; }
        public string Phone { get;  set; }
        public string Email { get;  set; }
        public string WebPage { get; set; }
        public double Rating { get; set; }
        public void MakeAnOrder(string order, User user)
        {
            string[] str = order.Split();
            for (int i = 0; i < str.Length; i++)
            {
                if (!this.Menu.ContainsKey(str[i]))
                {
                    throw new ArgumentException();

                }
                user.OrderList.Add(str[i], Menu[str[i]]);
            }
        }
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
        public void PayBill(int money)
        {

        }
        public string ShowMenu()
        {
            string output = "";
            foreach (KeyValuePair<string, int> entry in this.Menu)
            {
                output += entry.Key + "_________" + entry.Value + "\n";
            }
            return output;
        }
        public static string ShowAllCafes()
        {
            string output = "";
            for (int i = 0; i < Cafe.cafes.Count; i++)
                output += Cafe.cafes[i].Name + "\n";
            return output;
        }

        public override string ToString()
        {
            return string.Format("Cafe {0}\nAdress {1}\nOpen status: {2}   {3} - {4}\nRating {5}", Name, Address, OpenStatus(), Open, Close, Rating);
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
        public static void SortByDistance(ActiveUser activeUser)
        {
            cafes.Sort(delegate (Cafe c1, Cafe c2)
            {
                if (c1.Location.GetDistanceTo(activeUser.Location) > c2.Location.GetDistanceTo(activeUser.Location))
                    return 1;
                else if (c1.Location.GetDistanceTo(activeUser.Location) < c2.Location.GetDistanceTo(activeUser.Location))
                    return -1;
                else
                    return 0;
            });
        }
    }
}
