using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using RoadRunners.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace RoadRunners.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            DoAction(CarStates.Start,"A");
            Console.ReadLine();
            DoAction(CarStates.End, "B");

        }

        private static void DoAction(CarStates state, string scanner)
        {
            CarScan cs;
            if (state == CarStates.Start)
            {
                cs = new CarScan()
                {
                    Action = state,
                    StartScannerId = scanner,
                    LicensePlate = "1-ABC-AA"
                };
            }
            else
            {
                cs = new CarScan()
                {
                    Action = state,
                    EndScannerId = scanner,
                    LicensePlate = "1-ABC-AA"
                };
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8664/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var carContent = JsonConvert.SerializeObject(cs);
                HttpContent content = new StringContent(
                        content: JsonConvert.SerializeObject(carContent),
                        encoding: Encoding.UTF8,
                        mediaType: "application/json");

                client.PostAsync("/api/Scanner", content)
                    .ContinueWith(task =>
                    {

                        var response = task.Result;
                        Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                    });

                Console.ReadLine();


            }
        }
    }
}
