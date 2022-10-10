using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using System.Net;


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
                category =  new Category() {
                    id = 5678,
                    name = "PetCategory" },
                
               
                name = "Blacky",
                photoUrls = new string[]
                {
                    "Running", "Sleeping"
                },
                
                tags = new Tag[]
                {
                    new Tag() 
                    {
                     id = 0,
                     name = "PetTag" 

                    }
                    

                },
                status = "available"



            };

            // Serialize Content
            var request = JsonConvert.SerializeObject(petData);
            var postRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Send Post Request
            await httpClient.PostAsync(GetURL(PetEndpoint), postRequest);

            #region get pet of the created data

            // Get Request
            var getResponse = await httpClient.GetAsync(GetURI($"{PetEndpoint}/{petData.id}"));

            // Deserialize Content
            var listPetData = JsonConvert.DeserializeObject<ModelForPet>(getResponse.Content.ReadAsStringAsync().Result);

            // filter created data
            var createdPetData = listPetData.name;

            #endregion

            #region send put request to update data

            // Update Json Object
             petData = new ModelForPet()
            {
                id = 1234,
                category = new Category()
                {
                    id = 5678,
                    name = "PetCategory"
                },


                name = "Whity",
                photoUrls = new string[]
                {
                    "Running", "Sleeping"
                },

                tags = new Tag[]
                {
                    new Tag()
                    {
                     id = 0,
                     name = "PetTag"

                    }


                },
                status = "available"



            };
            

            // Serialize Content
            request = JsonConvert.SerializeObject(petData);
            postRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Send Put Request
            var httpResponse = await httpClient.PutAsync(GetURL($"{PetEndpoint}"), postRequest);

            // Get Status Code
            var statusCode = httpResponse.StatusCode;

            #endregion

            #region get updated data

            // Get Request
            getResponse = await httpClient.GetAsync(GetURI($"{PetEndpoint}/{petData.id}"));

            // Deserialize Content
            listPetData = JsonConvert.DeserializeObject<ModelForPet>(getResponse.Content.ReadAsStringAsync().Result);

            // filter created data
            createdPetData = listPetData.name;

            #endregion

            #region cleanup data

            // Add data to cleanup list
            cleanUpList.Add(listPetData);

            #endregion

            #region assertion

            // Assertion
            Assert.AreEqual(HttpStatusCode.OK, statusCode, "Status code is not equal to 201");
            Assert.AreEqual(petData.name, createdPetData, "Petdata is not matching");

            #endregion
        }




        [TestMethod]
        public void TestMethod2()
        {
        }


        [TestCleanup]
        public async Task TestCleanUp()
        {
            foreach (var data in cleanUpList)
            {
                var httpResponse = await httpClient.DeleteAsync(GetURL($"{PetEndpoint}/{data.id}"));
            }
        }

    }
}