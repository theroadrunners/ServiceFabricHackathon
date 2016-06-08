using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace RESTSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = WebApp.Start<Startup>(Job.JobURL);

            Console.WriteLine($"Host is listening at {Job.JobURL}");

            Job.SendJob("Christian").Wait();

            app.Dispose();
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Add(config.Formatters.JsonFormatter);
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            // config.EnsureInitialized();

            app.UseWebApi(config);
        }
    }

    public class ValuesController : ApiController
    {
        // [Route("/")]
        [HttpPost]
        public async Task<IHttpActionResult> PostAsync(Job job)
        {
            Console.WriteLine($"Received {job.Name}");

            return Ok();
        }
    }

    public class Job
    {
        public string Name { get; set; }

        public static readonly string JobURL = "http://localhost:5000/";

        public static async Task SendJob(string name)
        {
            var client = new HttpClient { BaseAddress = new Uri(JobURL) };

            var response = await client.SendAsync(
                new HttpRequestMessage(HttpMethod.Post, "api/values")
                {
                    Content = new StringContent(
                        content: JsonConvert.SerializeObject(new Job { Name = name }),
                        encoding: Encoding.UTF8,
                        mediaType: "application/json")
                });
        }
    }
}
