using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
//using Nancy.Json;
//using Newtonsoft.Json;
using System.Collections.Specialized;

namespace BankServiceMvc.Controllers
{
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        [Route("/display")]
        public string display()
        {
            var ctx = HttpContext.Request.Headers;
            foreach (var key in ctx.Keys)
            {
                string headers="";
                headers += key + "=" + ctx[key] + Environment.NewLine;
                Console.WriteLine(headers);
            }
            string result = new StreamReader(HttpContext.Request.Body).ReadToEndAsync().Result;
            Console.WriteLine("Printing request body "+result);
            return "Hello World-from display";
        }

        [HttpGet]
        [Route("/getall")]
        public string Get()
        {
            string responseFromServer;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:3504/v1.0/invoke/serviceB/method/getall");
            WebResponse response = request.GetResponse();
            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                responseFromServer = reader.ReadToEnd();
                // Display the content.
                Console.WriteLine(responseFromServer);
                response.Close();
                return responseFromServer;
            }
            

        }

        [HttpGet]
        [Route("/getstate/{str}")]
        public string getstate(string str)
        {
            try
            {
                string responseFromServer;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:3504/v1.0/invoke/serviceB/method/getstate/" + str);
                WebResponse response = request.GetResponse();
                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                    Console.WriteLine(responseFromServer);
                    response.Close();
                    return responseFromServer;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return "Not Found";
            }
        }

        [HttpPost]
        [Route("/addstate")]
        public string PostDetails()
        {
            try
            {
                string responseFromServer;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:3504/v1.0/invoke/serviceB/method/addstate");
                request.Method = "POST";
                WebResponse response = request.GetResponse();
                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                    Console.WriteLine(responseFromServer);
                    response.Close();
                    return responseFromServer;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return "Could Not Add";
            }
        }
        
        
        
    }
}
