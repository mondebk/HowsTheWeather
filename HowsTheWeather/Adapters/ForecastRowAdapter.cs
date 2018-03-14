using System;
using Android.Views;
using Android.App;
using Android.Content;
using Android.Provider;
using Android.Widget;
using Java.Lang;
using HowsTheWeather.Classes;
using HowsTheWeather.Objects;
using System.Collections.Generic;
using System.Linq;

namespace HowsTheWeather.Adapters
{
    public class ForecastRowAdapter : BaseAdapter<List<ForecastResponseList>>
    {
        private List<List<ForecastResponseList>> OuterForecastList;
        private Activity activity;
        private ForecastItemAdapter forecastItemAdapter;

        public ForecastRowAdapter(Activity _activity, List<List<ForecastResponseList>> forecast_outer_list)
        {
            activity = _activity;
            OuterForecastList = forecast_outer_list;
        }

        public override List<ForecastResponseList> this[int position] 
        {
            get { return OuterForecastList[position]; }
        }

        public override int Count 
        {
            get { return OuterForecastList.Count; }
        }


        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            List<ForecastResponseList> inner_list = OuterForecastList[position];
            View view = convertView;
            if(view == null)
            {
               view = activity.LayoutInflater.Inflate(Resource.Layout.forecast_row, parent, false);   
            }
            var lblDayOfWeek = view.FindViewById<TextView>(Resource.Id.lblTheDay);
            var listDay = view.FindViewById<ListView>(Resource.Id.lvForcastItemList);

            string dayText = inner_list[position].dt_txt;
            DateTime dtTheDay = DateTime.Parse(dayText);
            lblDayOfWeek.Text = dtTheDay.Date.ToString("dd MMM yy");

            listDay.Adapter = new ForecastItemAdapter(activity, inner_list);
            return view;
        }
    }
}
