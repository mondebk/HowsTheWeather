using System;
using SQLite;

namespace HowsTheWeather.Objects
{
    public class Main
    {
        public Main()
        {
        }

        public double Temp { get; set; }

        public int Pressure { get; set; }

        public int Humidity { get; set; }

        public double Temp_Min { get; set; }

        public double Temp_Max { get; set; }

        public MainDTO ConvertToDTO()
        {
            MainDTO mainDTO = new MainDTO();
            mainDTO.Temp = Temp;
            mainDTO.Temp_Max = Temp_Max;
            mainDTO.Temp_Min = Temp_Min;
            mainDTO.Humidity = Humidity;
            mainDTO.Pressure = Pressure;
            return mainDTO;
        }
    }

    /// <summary>
    ///     Main (weather) DTO class for SQLite based off Main (weather)
    /// </summary>
    [Table("Main")]
    public class MainDTO
    {
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public int _id { get; set; }

        public double Temp { get; set; }

        public int Pressure { get; set; }

        public int Humidity { get; set; }

        public double Temp_Min { get; set; }

        public double Temp_Max { get; set; }

        public DateTime TimeCreated { get { return DateTime.Now; } }
    }
}