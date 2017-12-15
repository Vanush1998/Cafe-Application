using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Device.Location;
using System.Data.SqlClient;

namespace CafeApplication
{
    public class GeoCode
    {
        public static GeoCoordinate GEOCodeAddress(String Address)
        {
            var address = String.Format("http://maps.google.com/maps/api/geocode/json?address={0}&sensor=false", Address.Replace(" ", "+"));
            var result = new System.Net.WebClient().DownloadString(address);
            return new GeoCoordinate(JsonConvert.DeserializeObject<RootObject>(result).results[0].geometry.location.lat, JsonConvert.DeserializeObject<RootObject>(result).results[0].geometry.location.lng);
        }
        public static string GetFormattedAddress(String Address)
        {
            var address = String.Format("http://maps.google.com/maps/api/geocode/json?address={0}&sensor=false", Address.Replace(" ", "+"));
            var result = new System.Net.WebClient().DownloadString(address);
            return JsonConvert.DeserializeObject<RootObject>(result).results[0].formatted_address;
        }
    }
    public class AddressComponent
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public List<string> types { get; set; }
    }

    public class Bounds
    {
        public Location northeast { get; set; }
        public Location southwest { get; set; }
    }

    public class Location
    {
        public string addressName { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public static int InsertLocation(double lat, double lng, string addressName)
        {
            string queryString = String.Format
                ("exec dbo.UDSP_InsertLocation @latitude = {0}, @longitude = {1},@addressName = '{2}'", lat, lng, addressName);
            SqlCommand command = new SqlCommand(
            queryString, DbConnection.GetConnection());
            SqlDataReader reader = command.ExecuteReader();
            int insertedLocationId = 0;
            while (reader.Read())
            {
                insertedLocationId = reader.GetInt32(0);
            }
            return insertedLocationId;
        }
    }

    public class Geometry
    {
        public Bounds bounds { get; set; }
        public Location location { get; set; }
        public string location_type { get; set; }
        public Bounds viewport { get; set; }
    }

    public class Result
    {
        public List<AddressComponent> address_components { get; set; }
        public string formatted_address { get; set; }
        public Geometry geometry { get; set; }
        public bool partial_match { get; set; }
        public List<string> types { get; set; }
    }

    public class RootObject
    {
        public List<Result> results { get; set; }
        public string status { get; set; }
    }
}


