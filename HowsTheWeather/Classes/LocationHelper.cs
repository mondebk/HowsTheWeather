using System;
using System.Collections.Generic;
using System.Linq;
using Android.Locations;
using SQLite;

namespace HowsTheWeather.Classes
{
    public class LocationHelper
    {
        private DBHelper db_helper = DBHelper.Instance; 
        private LocationManager location_manager;
        private string strLocationProvider;

        /// <summary>
        /// Initializes a new instance of the location helper class.
        /// </summary>
        /// <param name="locManager">Location manager.</param>
        public LocationHelper(LocationManager locManager)
        {
            location_manager = locManager; //location manager from the activity
            InitialiseLocationManager();
        }

        /// <summary>
        /// Initialises the location manager.
        /// </summary>
        private void InitialiseLocationManager()
        {
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine //uses GPS for acquiring locatin - may fail due to GPS taking time to acquire location.
            };
            IList<string> acceptableLocationProviders = location_manager.GetProviders(criteriaForLocationService, true);
            if (acceptableLocationProviders.Any())
            {
                strLocationProvider = acceptableLocationProviders.First();
            }
            else
            {
                strLocationProvider = string.Empty;
            }
            SetLocation(location_manager.GetLastKnownLocation(strLocationProvider));
        }

        /// <summary>
        /// Sets the location and saves it to the database.
        /// </summary>
        /// <param name="location">Location.</param>
        private void SetLocation(Location location)
        {
            var currentLocation = location;
            if (currentLocation == null)
            {
                //Retrieve saved location from database

                LocationDTO stored_location = db_helper.GetSavedLocation();
                Latitude = stored_location.Latitude;
                Longitude = stored_location.Longitude;
            }
            else
            {
                //Set newly updated location strings, and update database with new location.

                Latitude = currentLocation.Latitude.ToString();
                Longitude = currentLocation.Longitude.ToString();

                LocationDTO new_location = new LocationDTO();
                new_location.Latitude = Latitude;
                new_location.Longitude = Longitude;

                db_helper.SaveLocation(new_location);
            }
        }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string Location { get; set; }

    }

    //SQLite DTO
    [Table("Location")]
    public class LocationDTO
    {
        [PrimaryKey, NotNull, Unique, AutoIncrement]
        public int lID { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; } 
    }
}
