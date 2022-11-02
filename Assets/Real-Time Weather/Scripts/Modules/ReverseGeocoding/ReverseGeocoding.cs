//
// Copyright(c) 2021 Real-Time Weather ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at hello@assist.ro
//

using RealTimeWeather.AlertSystem;
using RealTimeWeather.Geocoding.Classes;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace RealTimeWeather.Geocoding
{
    /// <summary>
    /// <para>
    /// This class is responsible for the reverse geocoding functionality and BigData API communication.
    /// </para>
    /// <para>
    /// Reverse geocoding is a process that converts latitude and longitude to readable locality properties.
    /// </para>
    /// <para>
    /// BigData API information is available at https://www.bigdatacloud.com/geocoding-apis/free-reverse-geocode-to-city-api?gclid=EAIaIQobChMIwsiX0urK7gIVidCyCh1BNgiAEAAYASAAEgJG7PD_BwE.
    /// </para>
    /// </summary>
    public class ReverseGeocoding
    {
        #region Private const Variables
        private const string kBigDataURL = "https://api.bigdatacloud.net/data/reverse-geocode-client?";
        private const string kSeparatorStr = "&";
        private const string kLatitudeStr = "latitude=";
        private const string kLongitudeStr = "longitude=";
        private const string kLocalityLanguageStr = "localityLanguage=";
        #endregion

        #region Public Methods
        /// <summary>
        /// A coroutine that makes a web request to the BigData API in order to get the reverse geocoding data.
        /// </summary>
        /// <param name="latitude">Is a float value that represets a geographic coordinate that specifies the north–south position of a point on the Earth's surface.</param>
        /// <param name="longitude">Is a float value that represents a geographic coordinate that specifies the east-west position of a point on the Earth's surface.</param>
        /// <param name="language">Is a string value that represents the language input parameter. The default value is English.</param>
        public IEnumerator RequestGeocodingInformation(float latitude, float longitude, string language = "en")
        {
            string url = GenerateTheURL(latitude, longitude, language);

            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    LogFile.Write(webRequest.error);
                    Debug.Log(webRequest.error);
                    yield return null;
                }
                else
                {
                    GeocodingData geoData = JsonUtility.FromJson<GeocodingData>(webRequest.downloadHandler.text);
                    yield return geoData;
                }
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Generates the URL for the BigData API call.
        /// </summary>
        /// <param name="latitude">Is a float value that represets a geographic coordinate that specifies the north–south position of a point on the Earth's surface.</param>
        /// <param name="longitude">Is a float value that represents a geographic coordinate that specifies the east-west position of a point on the Earth's surface.</param>
        /// <param name="language">Is a string value that represents the language input parameter. The default value is English.</param>
        /// <returns>
        /// A string value that represents the URL of the resource to retrieve via HTTP GET.</param>
        /// </returns>
        private string GenerateTheURL(float latitude, float longitude, string language = "en")
        {
            string url = kBigDataURL;
            url += kLatitudeStr + latitude + kSeparatorStr + kLongitudeStr + longitude;
            url += kSeparatorStr + kLocalityLanguageStr + language;
            return url;
        }
        #endregion
    }
}