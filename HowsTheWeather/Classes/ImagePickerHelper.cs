using System;
using Android.App;

namespace HowsTheWeather.Classes
{
    /// <summary>
    /// Image picker helper. This class' main purpose is to read teh icon code retrieved 
    /// from the web service and map it to an image resource stored locally.
    /// </summary>
    public class ImagePickerHelper
    {
        private static readonly ImagePickerHelper instance = new ImagePickerHelper(); //Singleton

        public ImagePickerHelper() { }

        static ImagePickerHelper() { }

        public static ImagePickerHelper Instance
        {
            get {return instance;}
        }

        /// <summary>
        /// Gets the image resource identifier.
        /// </summary>
        /// <returns>The image resource identifier.</returns>
        /// <param name="image">Image</param>
        public int GetImageResourceId(string image)
        {
            if(image == "01d" || (image == "01n"))
            {
                return Resource.Mipmap.sunny_warm;
            }
            else if(image == "02d" || (image == "02n"))
            {
                return Resource.Mipmap.partly_sunny;
            }
            else if(image == "03d" || (image == "03n"))
            {
                return Resource.Mipmap.overcast;
            }
            else if (image == "04d" || (image == "04n"))
            {
                return Resource.Mipmap.dark_cloudy;
            }
            else if (image == "09d" || (image == "09n"))
            {
                return Resource.Mipmap.drizzle;
            }
            else if (image == "10d"|| (image == "10n"))
            {
                return Resource.Mipmap.rain;
            }
            else if (image == "11d"|| (image == "11n"))
            {
                return Resource.Mipmap.dark_thunderstorm;
            }
            else if (image == "13d"|| (image == "13n"))
            {
                return Resource.Mipmap.snow;
            }
            else if (image == "50d"|| (image == "50n"))
            {
                return Resource.Mipmap.fog;
            }
            return 0;
        }
    }
}
