using System;
namespace HowsTheWeather.Objects
{
    public class Sys
    {
        public Sys()
        {
        }

        public int Type { get; set; }

        public int ID { get; set; }

        public string Message { get; set; }

        //Country code (GB, JP etc.)
        public string Country { get; set; } 

        public int Sunrise { get; set; }

        //Sunset time, unix, UTC
        public int Sunset { get; set; }
    }
}
