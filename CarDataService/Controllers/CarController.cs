﻿using CarDataService.Collectors;
using CarDataService.MessageQueue;
using CarDataService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDataService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarController : ControllerBase
    {
        private readonly RabbitMQHandler _mqHandler;
        CarDataCollector dataCollector = new CarDataCollector();

        public CarController(RabbitMQHandler mQHandler)
        {
            _mqHandler = mQHandler;
            MQListener();
        }

        [HttpGet]
        public ActionResult<Car> Get(string vinNumber)
        {
            try
            {
                dataCollector.GetCarDataOnVin("WDDNG76X47A074546");
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest(vinNumber);
            }
        }

        private void MQListener()
        {
            _mqHandler.QueueListener();
        }
    }
}
