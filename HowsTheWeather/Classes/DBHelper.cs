using System;
using System.IO;
using HowsTheWeather.Objects;
using SQLite;

namespace HowsTheWeather.Classes
{
    /// <summary>
    /// SQLite database helper class.
    /// </summary>
    public class DBHelper
    {
        private static readonly DBHelper instance = new DBHelper(); //Singleton
        private ErrorLogger errorLogger = ErrorLogger.Instance; //Singleton
        private static string DB_PATH;
        private static SQLiteConnection sqLiteConnection;

        static DBHelper()
        {
            DB_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "howsthedatabase.db3");
            sqLiteConnection = new SQLiteConnection(DB_PATH);
            CreateTables();
        }

        public DBHelper()
        {
            DB_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "howsthedatabase.db3");
            sqLiteConnection = new SQLiteConnection(DB_PATH);
            CreateTables();
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance of the DB helper</value>
        public static DBHelper Instance
        {
            get { return instance; }    
        }

        /// <summary>
        /// Creates the tables if they do not exist
        /// </summary>
        private static void CreateTables()
        {
            sqLiteConnection.CreateTable<MainDTO>();
            sqLiteConnection.CreateTable<LocationDTO>();
            sqLiteConnection.CreateTable<WeatherDTO>();
            sqLiteConnection.CreateTable<LocationTextDTO>();
            sqLiteConnection.CreateTable<ErrorLogDTO>();
            sqLiteConnection.CreateTable<ForecastDTO>();
        }


        #region Weather storage methods
        public void SaveLocation(LocationDTO locationDTO)
        {
            //Check if the table is not empty.
            if (sqLiteConnection.Query<LocationDTO>("SELECT * FROM Location").Count > 0)
            {
                //Retrieve the first row entry. This entry will be updated with the latest location.
                var item = sqLiteConnection.Get<LocationDTO>(1); 
                item.Latitude = locationDTO.Latitude;
                item.Longitude = locationDTO.Longitude;
                sqLiteConnection.Update(item);
            }
            else
            {
                //Add a new entry if the table is empty.
                sqLiteConnection.Insert(locationDTO);
            }
        }

        /// <summary>
        /// Stores the current main weather data to the database
        /// </summary>
        /// <param name="main">Main.</param>
        public void SaveMain(Main main)
        {
            MainDTO mainDTO = main.ConvertToDTO();

            if(sqLiteConnection.Query<MainDTO>("SELECT * FROM Main").Count > 0)
            {
                var currentEntry = sqLiteConnection.Get<MainDTO>(1);
                mainDTO._id = currentEntry._id; //swap ID to always ensure it will be record 1 being updated - only keeping one entry in db
                currentEntry = mainDTO;
                sqLiteConnection.Update(currentEntry);
            }
            else
            {
                sqLiteConnection.Insert(main);
            }
        }

        /// <summary>
        /// Saves the new error to the error log
        /// </summary>
        /// <param name="errorLog">Error log.</param>
        public void SaveNewError(ErrorLogDTO errorLog)
        {
            sqLiteConnection.Insert(errorLog);
        }

        public void SaveNewForecast(ForecastResponse forecast)
        {
            ForecastDTO forecastDTO = forecast.ConvertToDTO();
            sqLiteConnection.Insert(forecastDTO);
        }
        #endregion

        #region Weather retrieval functions
        /**
         * The following functions retrieve information from the SQLite database.
         */

        public LocationDTO GetSavedLocation()
        {
            try
            {
                var StoredLocation = sqLiteConnection.Get<LocationDTO>(1);
                return StoredLocation;    
            }
            catch(Exception ex)
            {
                errorLogger.LogError("GetSavedLocation", ex.GetType().FullName, ex.Message, "Couldn't retrieve location. Possibly empty table", 010);
                return new LocationDTO();
            }
        }

        public WeatherDTO GetSavedWeather()
        {
            try
            {
                var StoredWeather = sqLiteConnection.Get<WeatherDTO>(1);
                return StoredWeather;
            }
            catch (Exception ex)
            {
                errorLogger.LogError("GetSavedWeather", ex.GetType().FullName, ex.Message, "Couldn't retrieve weather. Possibly empty table", 011);
                return new WeatherDTO();
            }
        }

        public MainDTO GetSavedMainWeather()
        {
            try
            {
                var StoredMainWeather = sqLiteConnection.Get<MainDTO>(1);
                return StoredMainWeather;
            }
            catch (Exception ex)
            {
                errorLogger.LogError("GetSavedMainWeather", ex.GetType().FullName, ex.Message, "Couldn't retrieve location. Possibly empty table", 012);
                return new MainDTO();
            }
        }

        public string GetSavedLocationText()
        {
            try
            {
                var StoredTextLocation = sqLiteConnection.Get<LocationTextDTO>(1);
                return StoredTextLocation.Name;
            }
            catch (Exception ex)
            {
                errorLogger.LogError("GetSavedLocationText", ex.GetType().FullName, ex.Message, "Couldn't retrieve location text. Possibly empty table", 013);
                return "Unable to retrieve location.";
            }
        }

        public ForecastDTO GetSavedForecast()
        {
            try
            {
                var StoredForecast = sqLiteConnection.Query<ForecastDTO>("SELECT * FROM Forecast ORDER BY fID DESC");
                ForecastDTO forecast = StoredForecast[0];
                return forecast;
            }
            catch (Exception ex)
            {
                errorLogger.LogError("GetSavedLocationText", ex.GetType().FullName, ex.Message, "Couldn't retrieve location text. Possibly empty table", 013);
                return new ForecastDTO();
            }
        }
        #endregion
    }
}
