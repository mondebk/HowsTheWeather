using System;
using SQLite;

namespace HowsTheWeather.Classes
{
    /// <summary>
    /// Error logger - stores all errors in a SQLite database.
    /// </summary>
    public class ErrorLogger
    {
        /* 
            In theory, this class would store all the info to the database and then periodically send 
            the error logs back to the developer. This resubmission portion was omitted in this demo
            due to time constraints and not having the relevant resources.
        */

        private static readonly ErrorLogger instance = new ErrorLogger();
        private static DBHelper db_helper;
        private static string strDeviceMake;
        private static string strDeviceModel;
        private static string strAndroidVersion;

        public ErrorLogger()
        {
            db_helper = DBHelper.Instance;
            strDeviceMake = Android.OS.Build.Manufacturer;
            strDeviceModel = Android.OS.Build.Model;
            strAndroidVersion = Android.OS.Build.VERSION.Codename;
        }

        static ErrorLogger()
        {
            db_helper = DBHelper.Instance;
            strDeviceMake = Android.OS.Build.Manufacturer;
            strDeviceModel = Android.OS.Build.Model;
            strAndroidVersion = Android.OS.Build.VERSION.Codename;
        }

        public static ErrorLogger Instance
        {
            get { return instance; }
        }

        public void LogError(string function_name, string exception_type, string exception_message, string dev_message, int error_type_id)
        {
            ErrorLogDTO errorLogDTO = new ErrorLogDTO();
            errorLogDTO.FunctionName = function_name;
            errorLogDTO.ExceptionType = exception_type;
            errorLogDTO.ExceptionMessage = exception_message;
            errorLogDTO.DevMessage = dev_message;
            errorLogDTO.DeviceMake = strDeviceMake;
            errorLogDTO.DeviceModel = strDeviceModel;
            errorLogDTO.AndroidVersion = strAndroidVersion;
            db_helper.SaveNewError(errorLogDTO);
        }
    }

    [Table("ErrorLog")]
    public class ErrorLogDTO
    {
        [PrimaryKey, NotNull, AutoIncrement, Unique]
        public int elID { get; set; }

        public string FunctionName { get; set; }

        public string ExceptionType { get; set; }

        public string ExceptionMessage { get; set; }

        public string DevMessage { get; set; }

        public int InternalErrorTypeID { get; set; }

        public string DeviceMake { get; set; }

        public string DeviceModel { get; set; }

        public string AndroidVersion { get; set; }
    }
}