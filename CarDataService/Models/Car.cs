using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDataService.Models
{
    public class Car
    {
        public Guid Id { get; set; }
        public String Manufacturer { get; set; }
        public String Model { get; set; }
        public String Year { get; set; }

        public Car(string manufacturer, string model, string year)
        {
            Id = new Guid();
            Manufacturer = manufacturer;
            Model = model;
            Year = year;
        }

        public Int32 Engine_Cylinders { get; set; }
        public Int32 Fuel_Efficiency_Rating_Index { get; set; }
        public Int32 Greenhouse_Gas_Rating_Index { get; set; }
        public String Manufacturers_Release_Date { get; set; }




    }
}
