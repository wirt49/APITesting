using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using APIDemo;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using APIDemo.DTO;
using APIDemo.Helpers;
using AventStack.ExtentReports;

namespace APITests
{
    [TestClass]
    public class RegressionTests
    {
        public TestContext TestContext { get; set; }
        public HttpStatusCode statusCode;

        [ClassInitialize]
        public static void SetUp(TestContext testContext)
        {
            var dir = testContext.TestRunDirectory;
            Reporter.SetupExtentReport("API regression tests", "API regression test report", dir);

        }

        [TestInitialize]
        public void SetupTest()
        {
            Reporter.CreateTest(TestContext.TestName);
        }
        
        [TestCleanup]
        public void CleanUpTest()
        {
            var testStatus = TestContext.CurrentTestOutcome;
            Status logStatus;

            switch(testStatus)
            {
                case UnitTestOutcome.Failed:
                    logStatus = Status.Fail;
                    Reporter.TestStatus(logStatus.ToString());
                    break;
                case UnitTestOutcome.Inconclusive:
                    break;
                case UnitTestOutcome.Passed:
                    logStatus = Status.Pass;
                    Reporter.TestStatus(logStatus.ToString());
                    break;
                case UnitTestOutcome.InProgress:
                    break;
                case UnitTestOutcome.Error:
                    break;
                case UnitTestOutcome.Timeout:
                    break;
                case UnitTestOutcome.Aborted:
                    break;
                case UnitTestOutcome.Unknown:
                    break;
                case UnitTestOutcome.NotRunnable:
                    break;
                default:
                    break;
            }
        }
        [ClassCleanup]
        public static void CleanUp()
        {
            Reporter.FlushReport();
        }

        [TestMethod]

        public void VerifyFirstUserName()
        {
            var demo = new Demo<ListOfUsersDTO>();
            var user = demo.GetUsers("api/users?page=1");
            Assert.AreEqual("George", user.Data[0].first_name);
            Reporter.LogReport(Status.Pass, "First user's names matches");
        }

        [TestMethod]
        public void VerifyUsersAmountOnFirstPage()
        {
            var demo = new Demo<ListOfUsersDTO>();
            var user = demo.GetUsers("api/users?page=1");
            Assert.AreEqual(1, user.Page);
            Assert.AreEqual(6, user.Data.Count);
            Reporter.LogReport(Status.Info, "User amount matches");

        }

        [TestMethod]
        public void VerifyResponseNotFoundUser()
        {
            var demo = new Demo<ListOfUsersDTO>();
            var response = demo.GetUsersResponse("api/users/23");
            statusCode = response.StatusCode;
            var str = statusCode.ToString();
            Assert.AreEqual("NotFound", str);

            Reporter.LogReport(Status.Info, str);

        }


        [TestMethod]
        public void GetUserByHTTP()
        {
            string html;
            string url = "https://reqres.in/api/users/2";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream stream = response.GetResponseStream();
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            var resp = JsonConvert.DeserializeObject<RootObject>(html);
            Assert.IsTrue(resp.data.id.Equals(2));
        }

        [TestMethod]
        public void CreateUserByHTTP()
        {
            string html;

            var reqObject = new CreateUserRequestDTO();
            reqObject.name = "morpheus";
            reqObject.job = "leader";

            string request = JsonConvert.SerializeObject(reqObject);
            string url = "https://reqres.in/api/users";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(request);
            }
            HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream stream = response.GetResponseStream();
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            var apiResponse = JsonConvert.DeserializeObject<CreateUserDTO>(html);
            Assert.IsTrue(apiResponse.name.Equals("morpheus") && apiResponse.job.Equals("leader"));
        }


        [DeploymentItem("Data\\TestCase.csv"),
            DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TestCase.csv", "TestCase#csv", DataAccessMethod.Sequential)]

        [TestMethod]
        public void CreateUserByRestSharp()
        {
            var users = new CreateUserRequestDTO();
            users.name = TestContext.DataRow["name"].ToString();
            users.job = TestContext.DataRow["job"].ToString();

            var demo = new Demo<CreateUserDTO>();
            var user = demo.CreateUser("api/users", users);
            
            Assert.AreEqual("morph", user.name);
            Reporter.LogReport(Status.Info, "User first names match");
        }


        [TestMethod]
        public void UpdateUserByRestSharp()
        {
            string requestBody = @"{""name"": ""morph"",
                                    ""job"": ""lider""}";

            var demo = new Demo<UpdateUserDTO>();
            var user = demo.UpdateUser("api/users/2", requestBody);

            Assert.AreEqual("morph", user.name);
            Assert.AreEqual("lider", user.job);
            Reporter.LogReport(Status.Pass, "User first names match");
        }


        [TestMethod]
        public void VerifyPUTResponse()
        {
            string requestBody = @"{""name"": ""Steve"",
                                    ""job"": ""Jobs""}";

            var demo = new Demo<UpdateUserDTO>();
            var response = demo.GetUpdateUserResponse("api/users/2", requestBody);
            statusCode = response.StatusCode;
            var code = statusCode.ToString();
            Assert.AreEqual("OK", code);

            Reporter.LogReport(Status.Pass, "Status code:" + code);
        }


        [TestMethod]
        public void VerifyUserIsDeleted()
        {
            var demo = new Demo<ListOfUsersDTO>();
            var response = demo.DeleteUser("api/users/2");
            statusCode = response.StatusCode;
            var code = (int)statusCode;
            Assert.AreEqual(204, code);
            
            Reporter.LogReport(Status.Pass, "User is deleted. Status code: " + code);
        }
    }
}

