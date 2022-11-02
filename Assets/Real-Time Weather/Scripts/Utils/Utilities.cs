// 
// Copyright(c) 2021 Real-Time Weather ASSIST-Software
// https://www.assist.ro
//
// For information about the licensing and copyright of this software please
// contact assist contact@assist.ro
//

using GeoTimeZone;
using NodaTime;
using NodaTime.TimeZones;
using System;
using TimeZoneConverter;
using UnityEngine;

namespace RealTimeWeather
{
    public class Utilities
    {
        #region Const Members
        private const double kAbsoluteKelvinTemperature = 273.15d;
        private const double kDifferenceFahrToCelsius = 32.0d;
        private const double kFahrToCelsius = 5.0d / 9.0d;
        private const double kKilometerPerHourFromMS = 3.6d;
        private const double kKilometerPerHourFromMH = 1.60934d;
        private const double kOneKMToMeters = 1000.0f;
        private const double kOneHundred = 100.0d;
        #endregion

        /// <summary>
        /// Converts a degree to Vector2.
        /// </summary>
        /// <param name="degree">A float value that represents the degree.</param>
        public static Vector2 DegreeToVector2(float degree)
        {
            return RadianToVector2(degree * Mathf.Deg2Rad);
        }

        /// <summary>
        /// Converts a radian to Vector2.
        /// </summary>
        /// <param name="">A float value that represents the radian.</param>
        public static Vector2 RadianToVector2(float radian)
        {
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }

        /// <summary>
        /// This method obtains the time zone data from longitude and latitude.
        /// </summary>
        /// <param name="locationLatitude">Latitude data calculated</param>
        /// <param name="locationLongitude">Longitude data calculated</param>
        /// <returns>The current time zone in format "Continent/Region"</returns>
        public static string GetTimeZone(float locationLatitude, float locationLongitude)
        {
            string ianaTimeZoneResult = TimeZoneLookup.GetTimeZone(locationLatitude, locationLongitude).Result;
            string resultedTimeZone = string.Empty;

#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX || UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX
            resultedTimeZone = TZConvert.GetTimeZoneInfo(ianaTimeZoneResult).DisplayName;
#endif

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || UNITY_ANDROID || UNITY_IOS 
            resultedTimeZone = TZConvert.IanaToWindows(ianaTimeZoneResult);
#endif
            return resultedTimeZone;
        }

        /// <summary>
        /// This method parses the UTC offset data.
        /// </summary>
        /// <param name="timeZone">Time zone data</param>
        /// <returns>The current utc offset in hours</returns>
        public static TimeSpan GetUTCOffsetData(string timeZone)
        {
            TimeZoneInfo timeZoneInfo = TZConvert.GetTimeZoneInfo(timeZone);
            return timeZoneInfo.GetUtcOffset(DateTime.Now);
        }

        /// <summary>
        /// This method calculates the utc offset data.
        /// </summary>
        /// <param name="timeZoneWindowsId">Time zone data windows ID. For example 'GTB Standard Time'.</param>
        /// <returns>The current utc offset in hours:minutes</returns>
        public static TimeSpan GetUTCOffsetOnAndroid(string timeZoneWindowsId)
        {
            Offset offset = new Offset();
            string tzdbId;

            if (!TzdbDateTimeZoneSource.Default.WindowsMapping.PrimaryMapping.TryGetValue(timeZoneWindowsId, out tzdbId))
            {
                return offset.ToTimeSpan();
            }

            DateTimeZone timeZoneDate = TzdbDateTimeZoneSource.Default.ForId(tzdbId);
            IClock systemClock = SystemClock.Instance;
            offset = timeZoneDate.GetUtcOffset(systemClock.GetCurrentInstant());

            return offset.ToTimeSpan();
        }

        /// <summary>
        /// This method converts Kelvin units in Degrees units
        /// </summary>
        /// <param name="kelvinValue">The value in Kelvin units</param>
        /// <returns>A new value in Degrees units</returns>
        public static double ConvertKelvinToDegrees(double kelvinValue)
        {
            return kelvinValue - kAbsoluteKelvinTemperature;
        }

        /// <summary>
        /// This method converts Fahrenheit units in Degrees units
        /// </summary>
        /// <param name="fahrenheitValue">The value in Fahrenheit units</param>
        /// <returns>A new value in Degrees units</returns>
        public static double ConvertFahrenheitToDegrees(double fahrenheitValue)
        {
            return (fahrenheitValue - kDifferenceFahrToCelsius) * kFahrToCelsius;
        }

        /// <summary>
        /// This method converts {meters per second} units in {kilometers per hour} units
        /// </summary>
        /// <param name="metersPerSecond">The value in {meters per second} units</param>
        /// <returns>A new value in {kilometers per hour} units</returns>
        public static double ConvertMeterPerSecondToKMPerHour(double metersPerSecond)
        {
            return metersPerSecond * kKilometerPerHourFromMS;
        }

        /// <summary>
        /// This method converts {miles per hour} units in {kilometers per hour} units
        /// </summary>
        /// <param name="milesPerHour">The value in {miles per hour} units</param>
        /// <returns>A new value in {kilometers per hour} units</returns>
        public static double ConvertMilePerHourToKMPerHour(double milesPerHour)
        {
            return milesPerHour * kKilometerPerHourFromMH;
        }

        /// <summary>
        /// This method calculates the dewpoint temperature based on temperature and humidity
        /// </summary>
        /// <param name="temperatureInDegrees">The temperature in degrees units</param>
        /// <param name="humidityInPercentages">The humidity represented in percentages</param>
        /// <returns>Dewpoint represented in degrees units</returns>
        public static double CalculateDewpointTemperature(double temperatureInDegrees, double humidityInPercentages)
        {
            return temperatureInDegrees - ((kOneHundred - humidityInPercentages) / 5.0f);
        }

        /// <summary>
        /// This method converts meters units in kilometers units
        /// </summary>
        /// <param name="meters">The value in meters units</param>
        /// <returns>A new value in kilometers units</returns>
        public static double ConvertMetersToKilometers(double meters)
        {
            return meters / kOneKMToMeters;
        }
    }
}