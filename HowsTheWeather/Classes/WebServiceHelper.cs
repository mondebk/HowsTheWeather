using System;
using RestSharp;
using HowsTheWeather.Objects;
using Newtonsoft.Json;
using System.Collections.Generic;
using SQLite;
using System.Linq;

namespace HowsTheWeather.Classes
{
    /// <summary>
    /// Web service helper class. Used to retrieve weather data for a location.
    /// </summary>
    public class WebServiceHelper
    {
        private const string CONNECTION_STRING = "http://api.openweathermap.org/data/2.5/";
        private const string API_KEY = "0fb1e1e9662a58d4852103b07c8e7af4";
        private DBHelper db_helper = DBHelper.Instance;
        private ErrorLogger error_logger = ErrorLogger.Instance;
        private RestRequest rest_request;
        private RestClient rest_client;
        private WeatherResponse objRestResponse;
        private ForecastResponse objForecastRestResponse;
        private MainDTO localMain;
        private WeatherDTO localWeather;
        private ForecastDTO localForecast;
        private string strLocalLocation;
        private string strLatitude, strLongitude;

        /// <summary>
        /// Initializes a new instance of the Web Service helper class.
        /// </summary>
        /// <param name="latitude">Latitude.</param>
        /// <param name="longitude">Longitude.</param>
        public WebServiceHelper(string latitude, string longitude)
        {
            rest_client = new RestClient(CONNECTION_STRING);
            strLatitude = latitude;
            strLongitude = longitude;
            RequestSingleDayForecast();
            RequestFiveDayForecast();
        }

        /// <summary>
        ///     Initializes a new instance of the Web Service helper class using a city.
        /// </summary>
        /// <param name="city">City</param>
        public WebServiceHelper(string city)
        {
            rest_client = new RestClient(CONNECTION_STRING);
            rest_request = new RestRequest(string.Format("weather?q={0}&APPID={1}", city, API_KEY));
            RequestSingleDayForecast();
            RequestFiveDayForecast();
        }

        /// <summary>
        /// Requests the single day forecast. This will retrive the main weather information for today. 
        /// </summary>
        private void RequestSingleDayForecast()
        {
            
            rest_request = new RestRequest(string.Format("weather?lat={0}&lon={1}&APPID={2}", strLatitude, strLongitude, API_KEY));
            rest_request.RequestFormat = DataFormat.Json;

            var response = rest_client.Execute(rest_request);
            if (string.IsNullOrWhiteSpace(response.Content) || response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                LoadLocalSingleDay();
            }
            else
            {
                try
                {
                    objRestResponse = JsonConvert.DeserializeObject<WeatherResponse>(response.Content);
                }
                catch (Exception ex)
                {
                    error_logger.LogError("RequestSingleDayForecast", ex.GetType().FullName, ex.Message, "Couldn't get single day forecast. Issue with web service or couldn't get location from GPS", 008);
                    LoadLocalSingleDay();
                }
            }
        }

        /// <summary>
        /// Requests the five day forecast. This will retrieve data for the next five days.
        /// </summary>
        private void RequestFiveDayForecast()
        {
            rest_request = new RestRequest(string.Format("forecast?lat={0}&lon={1}&APPID={2}", strLatitude, strLongitude, API_KEY));

            var response = rest_client.Execute(rest_request);
            if (string.IsNullOrWhiteSpace(response.Content) || response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                LoadLocalSingleDay();
            }
            else
            {
                try
                {
                    objForecastRestResponse = JsonConvert.DeserializeObject<ForecastResponse>(response.Content);
                }
                catch (Exception ex)
                {
                    error_logger.LogError("RequestFiveDayForecast", ex.GetType().FullName, ex.Message, "Failed to load five day forecast.", 004);
                    LoadLocalForecast();
                }
            }
        }

        private void LoadLocalSingleDay()
        {
            strLocalLocation = db_helper.GetSavedLocationText();
            localMain = db_helper.GetSavedMainWeather();
            localWeather = db_helper.GetSavedWeather();
        }

        private void LoadLocalForecast()
        {
            localForecast = db_helper.GetSavedForecast();
        }

        public Main GetMainWeatherInfo()
        {
            Main main_weather = new Main();
            try
            {
                main_weather.Temp = objRestResponse.main.Temp - 273.00; //Conversion from Kelvin to Celcius
                main_weather.Temp_Max = objRestResponse.main.Temp_Max - 273.00;
                main_weather.Temp_Min = objRestResponse.main.Temp_Min - 273.00;
                main_weather.Humidity = objRestResponse.main.Humidity;
                main_weather.Pressure = objRestResponse.main.Pressure;
                db_helper.SaveMain(main_weather);
            }
            catch(Exception ex)
            {
                error_logger.LogError("GetMainWeatherInfo", ex.GetType().FullName, ex.Message, "'Main' type couldn't be created from web service result since it couldn't retrieve the JSON data.", 002);

                main_weather.Temp = localMain.Temp - 273.00;
                main_weather.Temp_Max = localMain.Temp_Max - 273.00;
                main_weather.Temp_Min = localMain.Temp_Min - 273.00;
                main_weather.Humidity = localMain.Humidity;
                main_weather.Pressure = localMain.Pressure;
            }
            return main_weather; 
        }

        /// <summary>
        /// Gets the weather info.
        /// </summary>
        /// <returns>The weather info.</returns>
        public Weather GetWeatherInfo()
        {
            Weather weather = objRestResponse.weather.FirstOrDefault(); //Only return information from the first weather result
            return weather;
        }

        /// <summary>
        /// Gets the location info.
        /// </summary>
        /// <returns>The location info.</returns>
        public string GetLocationInfo()
        {
            try
            {
                return objRestResponse.Name; //Gets the area name pertaining to the weather information
            }
            catch (Exception ex)
            {
                error_logger.LogError("GetLocationInfo", ex.GetType().FullName, ex.Message, "Couldn't get location string. Issue with web service or couldn't get location from GPS", 008);
                return strLocalLocation; //return locally saved location
            } 
        }

        /// <summary>
        /// Gets the forecast response list.
        /// </summary>
        /// <returns>The forecast response list.</returns>
        public List<ForecastResponseList> GetForecastResponseList()
        {
            try
            {
                db_helper.SaveNewForecast(objForecastRestResponse);
                return objForecastRestResponse.list;
            }
            catch(Exception ex)
            {
                error_logger.LogError("GetForecastResponseList", ex.GetType().FullName, ex.Message, "Unable to fetch forecast list data.", 009);
                return null;
            }
        }
    }
}
