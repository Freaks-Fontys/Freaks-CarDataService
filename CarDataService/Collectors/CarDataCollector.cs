using CarDataService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CarDataService.Collectors
{
    public class CarDataCollector
    {
        HttpClient client = new HttpClient();
        public CarDataCollector()
        {

        }
        
        //TODO
        public async Task<Car> GetCarDataOnVin(string vinNumber)
        {
            Car car;

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://vindecoder.p.rapidapi.com/salvage_check?" + vinNumber),
                Headers =
                {
                    { "x-rapidapi-key", "d673fcc095msh800ca7a24f9acd0p1f6e9djsncd36dd03c6fa" },
                    { "x-rapidapi-host", "vindecoder.p.rapidapi.com" },
                },
            };

            HttpResponseMessage response = await client.SendAsync(request);
            //response.EnsureSuccessStatusCode();

            
            var body = await response.Content.ReadAsStringAsync();
            // asign body data to car IN RIGHT WAY
            car = new Car(
                body.Substring(0, 10),
                body.Substring(0, 10),
                body.Substring(0, 10)
            );
            return car;
        }

        public async Task<Car> GetCarDataOnModel(string manufacturer, string model, string year)
        {
            Car car = new Car(manufacturer, model, year);

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://apis.solarialabs.com/shine/v1/vehicle-stats/specs?make={manufacturer}&model={model}&year={year}&full-data=true&apikey={"6piGP6vIejIM9jkUQmxjt3GCOaODG0gL"}")
            };

            HttpResponseMessage response = await client.SendAsync(request);
            // try catch for succesed message. catch equals dummydata
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();
            if(body != "")
            {

            }

            // change dummydata with data from api
            car.Greenhouse_Gas_Rating_Index = 7;
            car.Fuel_Efficiency_Rating_Index = 8;
            car.Engine_Cylinders = 6;
            car.Manufacturers_Release_Date = "08-08-2018";

            return car;
        }


    }
}
