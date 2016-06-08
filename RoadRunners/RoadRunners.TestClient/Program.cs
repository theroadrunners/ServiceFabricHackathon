namespace RoadRunners.TestClient
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using System.Linq;
    using Newtonsoft.Json;
    using Models;

    class Program
    {
        static void Main(string[] args)
        {
            var tasks = Enumerable.Range(start: 1, count: 10000)
                .Select(i => new CarSimulator(SenderClient.SendAsync))
                .Select(cs => cs.RunAsync())
                .ToArray();

            Task.WaitAll(tasks);
        }
    }

    public class CarSimulator
    {
        static Random _r = new Random();

        static string GetRandomLicensePlate()
        {
            var a = _r.Next(minValue: 1, maxValue: 99);
            var c = _r.Next(minValue: 1, maxValue: 99);

            return $"{a.ToString("00")}-FF-{c.ToString("00")}";
        }

        static TimeSpan GetSmallDelay(int seconds)
        {
            return TimeSpan.FromSeconds(_r.Next(seconds));
        }

        public string LicensePlate { get; } = GetRandomLicensePlate();
        public TimeSpan InitialDelay { get; } = GetSmallDelay(10);
        public TimeSpan WithinSectionDelay { get; } = GetSmallDelay(10);

        private Func<CarScan, Task> SendEvent;

        public CarSimulator(Func<CarScan, Task> sendEvent)
        {
            this.SendEvent = sendEvent;
        }

        public async Task RunAsync()
        {
            await Task.Delay(this.InitialDelay);
            await SendEvent(new CarScan()
            {
                Action = CarStates.Start,
                StartScannerId = "A",
                LicensePlate = this.LicensePlate
            });

            await Task.Delay(this.WithinSectionDelay);
            await SendEvent(new CarScan()
            {
                Action = CarStates.End,
                StartScannerId = "B",
                LicensePlate = this.LicensePlate
            });
        }
    }

    class SenderClient
    {
        public async static Task SendAsync(CarScan cs)
        {
            using (var client = new HttpClient {
                BaseAddress = new Uri("http://localhost:8664/")
            })
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var carContent = JsonConvert.SerializeObject(cs);
                HttpContent content = new StringContent(
                        content: JsonConvert.SerializeObject(carContent),
                        encoding: Encoding.UTF8,
                        mediaType: "application/json");

                var result = await client.PostAsync("/api/Scanner", content);
                var response = await result.Content.ReadAsStringAsync();

                Console.WriteLine(response);
            }
        }
    }
}
