
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace HowsTheWeather.Activities
{
    [Activity(MainLauncher = true, Icon = "@mipmap/logo")]
    public class SplashScreenActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SplashScreen);
            // Create your application here

            var logoImage = FindViewById<ImageView>(Resource.Id.imgSplashLogo);
            logoImage.SetImageResource(Resource.Mipmap.Logo);
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { GetLocationPerission(); });
            startupWork.Start();
        }

        // Simulates background work that happens behind the splash screen
        async void GetLocationPerission()
        {
            //Log.Debug(Tag, "Performing some startup work that takes a bit of time.");
            await Task.Delay(8000); // Simulate a bit of startup work.
            //Log.Debug(TAG, "Startup work is finished - starting MainActivity.");
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}
