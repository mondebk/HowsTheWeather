using System;
using System.Collections.Generic;
using SQLite;

namespace HowsTheWeather.Objects
{
    public class WeatherResponse
    {
        public Coord coord { get; set; }

        public List<Weather> weather { get; set; }

        public string @base { get; set; }

        public Main main { get; set; }

        public int visibility { get; set; }

        public Wind wind { get; set; }

        public Clouds clouds { get; set; }

        public Rain rain { get; set; }

        public Snow snow { get; set; }

        /// <summary>
        ///     Gets or sets the dt.
        /// </summary>
        /// <value>Time of data calculation, unix, UTC</value>
        public int dt { get; set; }

        public Sys sys { get; set; }

        //City ID
        public int id { get; set; } 

        //City name
        public string Name { get; set; }

        //OpenWeatherMap internal parameter
        public int cod { get; set; }

    }

    [Table("LocationText")]
    public class LocationTextDTO
    {
        [PrimaryKey, NotNull, Unique, AutoIncrement]
        public int ltID { get; set; }

        public string Name { get; set; }
    }
}
