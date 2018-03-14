using System;
using SQLite;

namespace HowsTheWeather.Objects
{
    public class Weather
    {
        public int id { get; set; }

        public string main { get; set; }

        public string description { get; set; }

        public string icon { get; set; }

    }

    [Table("Weather")]
    public class WeatherDTO 
    {    
        [PrimaryKey, NotNull, Unique, AutoIncrement]
        public int wID { get; set; }

        public string wMain { get; set; }

        public string wDescription { get; set; }
    }
}
