using Newtonsoft.Json;
using System.Text;
using System.Net.Http;


namespace TestProject2
{
    [TestClass]
    public class UnitTest1
    {

        private static HttpClient httpClient;

        private static readonly string BaseURL = "https://petstore.swagger.io/v2/";

        private static readonly string PetEndpoint = "pet";

        private static string GetURL(string enpoint) => $"{BaseURL}{enpoint}";

        private static Uri GetURI(string endpoint) => new Uri(GetURL(endpoint));

        private readonly List<ModelForPet> cleanUpList = new List<ModelForPet>();

        [TestInitialize]
        public void TestInitialize()
        {
            httpClient = new HttpClient();
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            // Create Json Object
            ModelForPet petData = new ModelForPet()
            {
                id = 1234,
                name = "Blacky",
                photoUrls = new string[]
                {
                    "Running", "Sleeping"
                }
                

                
            };

            // Serialize Content
            var request = JsonConvert.SerializeObject(petData);
            var postRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Send Post Request
            await httpClient.PostAsync(GetURL(PetEndpoint), postRequest);
        }

        [TestMethod]
        public void TestMethod2()
        {
        }
    }
}