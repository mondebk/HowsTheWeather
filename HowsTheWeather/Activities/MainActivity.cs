using Android.App;
using Android.Widget;
using Android.OS;
using Felipecsl.GifImageViewLibrary;
using HowsTheWeather.Classes;
using Android.Locations;
using Android.Runtime;
using System.Collections.Generic;
using Android.Util;
using System.Linq;
using System;
using HowsTheWeather.Objects;
using HowsTheWeather.Adapters;

namespace HowsTheWeather
{
    [Activity(Label = "How's The Weather?", MainLauncher = true, Icon = "@mipmap/logo")]
    public class MainActivity : Activity
    {
        private DBHelper db_helper = DBHelper.Instance;
        private ErrorLogger error_logger = new ErrorLogger();
        private WebServiceHelper web_helper;
        private LocationHelper location_helper;
        private LocationManager location_manager;
        private ImagePickerHelper image_picker_helper = ImagePickerHelper.Instance;
        private TextView tvGreetingMessage, tvDate, tvCurrentTemp, tvMinTemp, tvCurrentLocation;
        private TextView tvDayOne, tvDayTwo, tvDayThree, tvDayFour, tvDayFive;
        private ImageView ivWeatherImage;
        private ListView lvForecastDayOne, lvForecastDayTwo, lvForecastDayThree, lvForecastDayFour, lvForecastDayFive;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            InitialiseUIComponents();

        }

        protected override void OnStart()
        {
            base.OnStart();
            GetLocationUpdate();
            UpdateUIComponents();
        }

        /// <summary>
        /// Initialises the UI Components for the activity.
        /// </summary>
        private void InitialiseUIComponents()
        {
            ivWeatherImage = FindViewById<ImageView>(Resource.Id.imgMainWeather);
            tvGreetingMessage = FindViewById<TextView>(Resource.Id.lblMainMessage);
            tvDate = FindViewById<TextView>(Resource.Id.lblMainDate);
            tvCurrentTemp = FindViewById<TextView>(Resource.Id.lblMainTemp);
            tvMinTemp = FindViewById<TextView>(Resource.Id.lblMainMinTemp);
            tvCurrentLocation = FindViewById<TextView>(Resource.Id.lblMainCurrentLocation);
            lvForecastDayOne = FindViewById<ListView>(Resource.Id.lvForcastItemListA);
            lvForecastDayTwo = FindViewById<ListView>(Resource.Id.lvForcastItemListB);
            lvForecastDayThree = FindViewById<ListView>(Resource.Id.lvForcastItemListC);
            lvForecastDayFour = FindViewById<ListView>(Resource.Id.lvForcastItemListD);
            lvForecastDayFive = FindViewById<ListView>(Resource.Id.lvForcastItemListE);
            tvDayOne = FindViewById<TextView>(Resource.Id.lblTheDayA);
            tvDayTwo = FindViewById<TextView>(Resource.Id.lblTheDayB);
            tvDayThree = FindViewById<TextView>(Resource.Id.lblTheDayC);
            tvDayFour = FindViewById<TextView>(Resource.Id.lblTheDayD);
            tvDayFive = FindViewById<TextView>(Resource.Id.lblTheDayE);
            tvGreetingMessage.Text = GetTimeOfDay();
            tvDate.Text = DateTime.Today.ToString("dd MMM yyyy");
        }

        /// <summary>
        /// Gets the user's current location and 
        /// </summary>
        private void GetLocationUpdate()
        {
            try
            {
                //Retrieve the device's current location
                location_manager = (LocationManager)GetSystemService(LocationService);
                location_helper = new LocationHelper(location_manager);

                //Retrieve the single-day forcast for the current location
                web_helper = new WebServiceHelper(location_helper.Latitude, location_helper.Longitude);
            }
            catch (System.Exception ex)
            {
                error_logger.LogError("GetLocationUpdate", ex.GetType().ToString(), ex.Message, "Failure within web helper class", 001);
            }
        }

        /// <summary>
        /// Updates the UIC omponents.
        /// </summary>
        private void UpdateUIComponents()
        {
            Main main = web_helper.GetMainWeatherInfo();
            Weather weather = web_helper.GetWeatherInfo();
            string Location = web_helper.GetLocationInfo();

            tvCurrentTemp.Text = Convert.ToInt32(main.Temp).ToString() + "°C";
            tvMinTemp.Text = Convert.ToInt32(main.Temp_Max).ToString() + "°C" + "/" + Convert.ToInt32(main.Temp_Min).ToString() + "°C";

            ivWeatherImage.SetImageResource(image_picker_helper.GetImageResourceId(weather.icon));
            tvCurrentLocation.Text = Location;

            var ForecastCollection = web_helper.GetForecastResponseList();
            AttachRowToCard(ForecastCollection);
        }


        /// <summary>
        ///  Row adapter which binds forecast information to each card.
        /// </summary>
        /// <param name="forecast_collection">Forecast collection.</param>
        public void AttachRowToCard(List<ForecastResponseList> forecast_collection)
        {
            List<ForecastResponseList> listDayOne = new List<ForecastResponseList>();
            List<ForecastResponseList> listDayTwo = new List<ForecastResponseList>();
            List<ForecastResponseList> listDayThree = new List<ForecastResponseList>();
            List<ForecastResponseList> listDayFour = new List<ForecastResponseList>();
            List<ForecastResponseList> listDayFive = new List<ForecastResponseList>();

            foreach (ForecastResponseList forecast in forecast_collection)
            {
                DateTime day_to_compare = DateTime.Parse(forecast.dt_txt);

                if (DateTime.Today.Day == day_to_compare.Day)
                {
                    tvDayOne.Text = day_to_compare.ToString("ddd");
                    listDayOne.Add(forecast);
                }
                if (DateTime.Today.Day + 1 == day_to_compare.Day)
                {
                    tvDayTwo.Text = day_to_compare.ToString("ddd");
                    listDayTwo.Add(forecast);
                }
                if (DateTime.Today.Day + 2 == day_to_compare.Day)
                {
                    tvDayThree.Text = day_to_compare.ToString("ddd");
                    listDayThree.Add(forecast);
                }
                if (DateTime.Today.Day + 3 == day_to_compare.Day)
                {
                    tvDayFour.Text = day_to_compare.ToString("ddd");
                    listDayFour.Add(forecast);
                }
                if (DateTime.Today.Day + 4 == day_to_compare.Day)
                {
                    tvDayFive.Text = day_to_compare.ToString("ddd");
                    listDayFive.Add(forecast);
                }
            }

            //Add list adapter to each card individually.
            lvForecastDayOne.Adapter = new ForecastItemAdapter(this, listDayOne);
            lvForecastDayTwo.Adapter = new ForecastItemAdapter(this, listDayTwo);
            lvForecastDayThree.Adapter = new ForecastItemAdapter(this, listDayThree);
            lvForecastDayFour.Adapter = new ForecastItemAdapter(this, listDayFour);
            lvForecastDayFive.Adapter = new ForecastItemAdapter(this, listDayFive);
        }
       

        /// <summary>
        ///     Gets the time of day message from string resources.
        /// </summary>
        /// <returns>The time of day.</returns>
        public string GetTimeOfDay()
        {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            if(currentTime > TimeSpan.Parse("00:00") && currentTime < TimeSpan.Parse("12:00"))
            {
                return GetString(Resource.String.morning_message);
            }
            else if(currentTime > TimeSpan.Parse("11:59") && currentTime < TimeSpan.Parse("18:00"))
            {
                return GetString(Resource.String.afternoon_message);
            }
            else if(currentTime > TimeSpan.Parse("17:59"))
            {
                return GetString(Resource.String.evening_message);
            }
            return "";
        }
    }
}