using System;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Rest
{
    class CityWeather
    {
        private string appId = "USER_INPUT";
        private string baseAddress = "http://api.openweathermap.org/data/2.5";
        private HttpClient client;
        public CityWeather()
        {
            this.client = new HttpClient();
        }
       
        public Rootobject GetWeather(string city)
        {
            var address = $"{baseAddress}/weather?q={city}&appid={appId}";
            try
            {
                Task<HttpResponseMessage> responseTask = client.GetAsync(address);
                if (responseTask.Wait(3000))
                {
                    HttpResponseMessage response = responseTask.Result;
                    response.EnsureSuccessStatusCode();
                    string jsonResult = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<Rootobject>(jsonResult);
                }
                else
                {
                    Console.WriteLine("request timeout");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Request is failed because of " + ex.Message);
                return null;
                //using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonResult)))
                //{
                //    DataContractJsonSerializer weathershowSerializer = new DataContractJsonSerializer(typeof(WeatherShow));
                //    return weathershowSerializer.ReadObject(ms) as WeatherShow;
                //
            }
        }
    }
}
