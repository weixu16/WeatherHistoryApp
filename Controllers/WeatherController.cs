using Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [RoutePrefix("api/weather")]
    public class WeatherController : ApiController
    {
        [Route("getHistory/{city}")]
        public WeatherHistoryModel GetWeatherHistory(string city)
        {
            city = city.ToLowerInvariant();
            var weatherDataManager = new WeatherDataManager();
            if(weatherDataManager.IsCityInMonitorList(city))
            {
                List<WeatherInfoEntity> weatherInfo = weatherDataManager.GetWeatherHistory(city);

                weatherInfo = weatherInfo.OrderBy(o => DateTime.Parse(o.Date)).ToList();

                List<double> highTempList = new List<double>();
                List<double> lowTempList = new List<double>();
                List<string> dateList = new List<string>();
                foreach(WeatherInfoEntity entity in weatherInfo)
                {
                    highTempList.Add(entity.HighTemp);
                    lowTempList.Add(entity.LowTemp);
                    dateList.Add(entity.Date);
                }

                return new WeatherHistoryModel()
                {
                    City = city,
                    HighTemp = highTempList.ToArray(),
                    LowTemp = lowTempList.ToArray(),
                    Date = dateList.ToArray()
                };
            }
            else
            {
                //Query OpenWeather
                CityWeather cityWeather = new CityWeather();
                Rootobject weatherInfo = cityWeather.GetWeather(city);
                if(weatherInfo != null)
                {
                    string today = DateTime.Now.ToString("d");
                    weatherDataManager.UpdateCityInfoToTable(city, today);

                    WeatherInfoEntity model = new WeatherInfoEntity(city, convertKtoF(weatherInfo.main.temp_max), convertKtoF(weatherInfo.main.temp_min), today);
                    
                    weatherDataManager.UpdateHistoryInfoToTable(model);
                    return new WeatherHistoryModel() {
                        City = city,
                        HighTemp=new double[] {model.HighTemp},
                        LowTemp = new double[] {model.LowTemp},
                        Date = new string[] {today}
                    };
                }
                else
                {
                    return null; // error
                }
            }
        }

        [Route("getAllCities")]
        public string[] GetAllCities()
        {
            var weatherDataManager = new WeatherDataManager();
            return weatherDataManager.GetAllCities();
        }

        [Route("updateWeatherInfo")]
        public void UpdateWeatherInfo()
        {
            CityWeather cityWeather = new CityWeather();
            var weatherDataManager = new WeatherDataManager();
            string[] allCities = weatherDataManager.GetAllCities();
            foreach(string c in allCities)
            {
                Thread.Sleep(1000); // sleep 1s to avoid throttle
                this.QueryOpenWeatherAndUpdateTable(cityWeather, weatherDataManager, c);
            }
        }

        private void QueryOpenWeatherAndUpdateTable(CityWeather cityWeather, WeatherDataManager weatherDataManager, string city)
        {
            Rootobject weatherInfo = cityWeather.GetWeather(city);
            if (weatherInfo != null)
            {
                string today = DateTime.Now.ToString("d");
                // string today = "3/9/2017";
                weatherDataManager.UpdateCityInfoToTable(city, today);

                WeatherInfoEntity model = new WeatherInfoEntity(city, convertKtoF(weatherInfo.main.temp_max), convertKtoF(weatherInfo.main.temp_min), today);
                weatherDataManager.UpdateHistoryInfoToTable(model);
            }
        }

        private double convertKtoF(float k)
        {
            return (k * 9.0 / 5.0 - 459.67);
        }
    }
}
