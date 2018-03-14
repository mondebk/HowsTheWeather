using System;
using Android.Views;
using Android.App;
using Android.Content;
using Android.Provider;
using Android.Widget;
using Java.Lang;
using System.Collections.Generic;
using HowsTheWeather.Objects;
using HowsTheWeather.Classes;
using System.Linq;

namespace HowsTheWeather.Adapters
{
    /// <summary>
    /// Forecast item adapter. Bind forecast data from a day into the row ListView
    /// </summary>
    public class ForecastItemAdapter : BaseAdapter<ForecastResponseList>
    {
        private ImagePickerHelper ImagePicker = ImagePickerHelper.Instance;
        private List<ForecastResponseList> listForecast;
        private Activity activity;
        
        public ForecastItemAdapter(Activity _activity, List<ForecastResponseList> forecast_list)
        {
            activity = _activity;
            listForecast = forecast_list;
        }

        public override ForecastResponseList this[int position]{
            get { return listForecast[position]; }
        }

        public override int Count{
            get { return listForecast.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Weather weather_data = listForecast[position].weather.First();
            MainForecast main_data = listForecast[position].main;

            //Use recycled view
            View view = convertView;
            if(view == null)
            {
                view = activity.LayoutInflater.Inflate(Resource.Layout.forecast_item, null);
            }
            var forecast_time = view.FindViewById<TextView>(Resource.Id.lblItemTime);
            var forecast_image = view.FindViewById<ImageView>(Resource.Id.imgItemWeather);
            var forecast_temp = view.FindViewById <TextView>(Resource.Id.lblItemWeather);
            var forecast_min = view.FindViewById<TextView>(Resource.Id.lblItemMin);
            var forecast_max = view.FindViewById<TextView>(Resource.Id.lblItemMax);

            forecast_time.Text = listForecast[position].dt_txt;
            forecast_image.SetImageResource(ImagePicker.GetImageResourceId(weather_data.icon));

            main_data.Temp = main_data.Temp - 273.00;
            main_data.Temp_Max = main_data.Temp_Max - 273.00;
            main_data.Temp_Min = main_data.Temp_Min - 273.00;

            forecast_temp.Text = Convert.ToInt32(main_data.Temp).ToString() + "°C";
            forecast_min.Text = Convert.ToInt32(main_data.Temp_Min).ToString() + "°C";
            forecast_max.Text = Convert.ToInt32(main_data.Temp_Max).ToString() + "°C";

            return view;
        }
    }
}
