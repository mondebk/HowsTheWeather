using System;
namespace HowsTheWeather.Objects
{
    public class City
    {
        public int id { get; set; }

        public string name { get; set; }

        public Coord coord { get; set; }

        public string country { get; set; }
    }
}
