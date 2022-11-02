using RealTimeWeather.Classes;
using RealTimeWeather.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeatherElements : MonoBehaviour
{

    #region Private const Variables

    private const string ktime = "00:00";
    private const string kDay = "day";
    private const string kCelsiusDegreeStr = "°C";
    private const string kCity = "°C";

    #endregion

    #region Public Variables
      
    public TMP_Text _time;
    public TMP_Text _day;
    public TMP_Text _temperature;
    public TMP_Text _city;
    #endregion

    private void Awake()
    {
        if (RealTimeWeatherManager.instance)
        {
            RealTimeWeatherManager.instance.OnCurrentWeatherUpdate += OnCurrentWeatherUpdate;
            RealTimeWeatherManager.instance.OnHourlyWeatherUpdate += OnHourlyWeatherUpdate;
        }

    }

    private void OnDestroy()
    {
        if (RealTimeWeatherManager.instance)
        {
            RealTimeWeatherManager.instance.OnCurrentWeatherUpdate -= OnCurrentWeatherUpdate;
            RealTimeWeatherManager.instance.OnHourlyWeatherUpdate -= OnHourlyWeatherUpdate;
        }
    }

    private void OnCurrentWeatherUpdate(WeatherData weatherData)
    {
        if (weatherData == null)
        {
            return;
        }

        SetLocalizationInfo(weatherData.Localization.City, weatherData.Localization.Country);
        SetDateTime(weatherData);
        SetWeatherStateInfo(weatherData);
        Debug.Log("weather Data" + weatherData.DateTime.ToLongTimeString() + weatherData.DateTime.Minute + "  "
            + weatherData.DateTime.TimeOfDay.ToString()
            + weatherData.DateTime.Date.ToLongDateString()
            + " temp " + weatherData.Temperature.ToString() + DateTime.UtcNow + " "
            + weatherData.UTCOffset.Hours + weatherData.UTCOffset.Minutes);


        

    }

    /// <summary>
    /// Handles the hourly weather data update event.
    /// </summary>
    /// <param name="weatherDataList">A WeatherData class instance list that represents the received hourly weather data.</param>
    private void OnHourlyWeatherUpdate(List<WeatherData> weatherDataList)
    {
        if (weatherDataList.Count > 0)
        {
            Debug.Log("Hourly Weather Update:");
            foreach (var weatherData in weatherDataList)
            {
                Debug.Log(weatherData.ToString());
            }
        }

        //if required add hourly forcast and update text
    //    SetHourlyForecast(weatherDataList.Count);
    }


    private void SetWeatherStateInfo(WeatherData weatherData)
    {
        _temperature.text = weatherData.Temperature.ToString() + kCelsiusDegreeStr;

        //Add Information of weather type image
    }

    private void SetDateTime(WeatherData weatherData)
    {
        _day.text = weatherData.DateTime.Date.ToLongDateString();
        
        DateTime cityTime = DateTime.UtcNow.AddHours(weatherData.UTCOffset.Hours);
        cityTime.AddMinutes(weatherData.UTCOffset.Minutes);
       // Debug.Log(cityTime.ToShortTimeString());
        _time.text = cityTime.ToShortTimeString();

    }

    /// <summary>
    /// Set the localization information, city, and country.
    /// </summary>
    /// <param name="city">A string value that represents the city.</param>
    /// <param name="country">A string value that represents the country.</param>
    private void SetLocalizationInfo(string city, string country)
    {
        _city.text = city.ToUpper() + " , " + country.ToUpper();
    }


}
