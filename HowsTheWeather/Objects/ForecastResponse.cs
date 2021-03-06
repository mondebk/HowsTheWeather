﻿using System;
using System.Collections.Generic;
using SQLite;

namespace HowsTheWeather.Objects
{
    public class ForecastResponse
    {
        public string code { get; set; }

        public string message { get; set; }

        public int cnt { get; set; }

        public List<ForecastResponseList> list { get; set; }

        public City city { get; set; }

        public ForecastDTO ConvertToDTO()
        {
            ForecastDTO forecastDTO = new ForecastDTO();
            forecastDTO.code = code;
            forecastDTO.message = message;
            forecastDTO.cnt = cnt;
            forecastDTO.list = list;
            forecastDTO.city = city;
            return forecastDTO;
        }
    }

    public class ForecastResponseList {

        public string dt { get; set; }

        public MainForecast main { get; set; }

        public List<Weather> weather { get; set; }

        public Clouds clouds { get; set; }

        public Wind wind { get; set; }

        public Rain rain { get; set; }

        public Snow snow { get; set; }

        public string dt_txt { get; set; }

    }

    public class MainForecast
    {
        public double Temp { get; set; }

        public double Pressure { get; set; }

        public int Humidity { get; set; }

        public double Temp_Min { get; set; }

        public double Temp_Max { get; set; }
    }

    [Table("Forecast")]
    public class ForecastDTO
    {
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public int fID { get; set; }
        
        public string code { get; set; }

        public string message { get; set; }

        public int cnt { get; set; }

        public List<ForecastResponseList> list { get; set; }

        public City city { get; set; }
    }
}