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
using Nancy.Json;
using Newtonsoft.Json;
using System.Collections.Specialized;

namespace BankServiceMvc.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        
        [HttpGet]
        [Route("/display")]
        public string display()
        {
         string responseFromServer;   
       var ctx = HttpContext.Request.Headers;
foreach (var key in ctx.Keys)
{
    string headers="";
   headers += key + "=" + ctx[key] + Environment.NewLine;
            Console.WriteLine(headers);
}

            string reqbody = new StreamReader(HttpContext.Request.Body).ReadToEndAsync().Result;
            Console.WriteLine("Printing request body: "+reqbody);
            return "Hello World-from display\n";
            
        }

        [HttpGet]
        [Route("/component")]
        public string Get()
        {
            string responseFromServer;   
       var ctx = HttpContext.Request.Headers;
foreach (var key in ctx.Keys)
{
    string headers="";
   headers += key + "=" + ctx[key] + Environment.NewLine;
            Console.WriteLine(headers);
}

            string reqbody = new StreamReader(HttpContext.Request.Body).ReadToEndAsync().Result;
            Console.WriteLine("Printing request body: "+reqbody);
            Console.WriteLine("Hello World-from display\n");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:3502/v1.0/invoke/serviceB/method/display");
            request.Headers.Add("Authorization",Request.Headers["Authorization"]);
            try{
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
            catch(Exception e)
            {
                return "Invalid Authorization header - Can't access python app";
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
                /*byte[] byteArray = Encoding.UTF8.GetBytes(obj.ToString());
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();*/
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
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return "Not Found";
            }
            // Close the response.

            //return "Working";
        }

        [HttpPost]
        [Route("/addstate")]
        public string PostDetails()//JObject obj)
        {
            try
            {
                string responseFromServer;
                //string myobj = JsonConvert.SerializeObject(obj);
                //var byteData = Encoding.ASCII.GetBytes(myobj);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:3504/v1.0/invoke/serviceB/method/addstate");
                request.Method = "POST";
                //request.ContentType= "application/json";
                /*using (var stream = request.GetRequestStream())
                {
                    stream.Write(byteData, 0, byteData.Length);
                }*/
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
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return "Not Found";
            }
            // Close the response.

            //return "Working";
        }
        
        
        
    }
}
