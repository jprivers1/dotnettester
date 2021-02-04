using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {

        public class DataObject
        {
            public int userId { get; set; }
            public int id { get; set; }
            public string title { get; set; }
            public string body { get; set; }

        }

       
        static async Task Main(string[] args)
        {
            const string URL = "https://jsonplaceholder.typicode.com/posts";

            Task<IEnumerable<DataObject>> dataTask = GetResponse(URL);
            IEnumerable<DataObject> dataObjects = await dataTask;
            if (dataObjects != null)
            {
                foreach (var d in dataObjects)
                {
                    Console.WriteLine($"{d.title}");
                }
            }          
        }
        
        private static async Task<IEnumerable<DataObject>> GetResponse(string url) {
            IEnumerable<DataObject> dataObjects = null;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(url).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                dataObjects = response.Content.ReadAsAsync<IEnumerable<DataObject>>().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            // Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
            client.Dispose();
            return dataObjects;
        }
    }
}
