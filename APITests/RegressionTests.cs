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
        public void VerifyListOfUsersByRestSharp()
        {
            var demo = new Demo<ListOfUsersDTO>();
            var user = demo.GetUsers("api/users?page=2");
            Assert.AreEqual(2, user.Page);
            Reporter.LogReport(Status.Fail, "Page number does not match");
            Assert.AreEqual("Michael", user.Data[0].first_name);
            Reporter.LogReport(Status.Fail, "User first name does not match");

        }

        [TestMethod]
        public void GetByHTTP()
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
        public void CreateByHTTP()
        {
            string html;

            var reqObject = new UsersRequestObject();
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

            var apiResponse = JsonConvert.DeserializeObject<UsersResponseObject>(html);
            Assert.IsTrue(apiResponse.name.Equals("morpheus") && apiResponse.job.Equals("leader"));
        }

        [TestMethod]
        public void CreateByRestSharp()
        {
            string requestBody = @"{""name"": ""morph"",
                                    ""job"": ""lid""}";
            var demo = new Demo<CreateUserDTO>();
            var user = demo.CreateUser("api/users", requestBody);
            
            Assert.AreEqual("morph", user.name);
        }
    }
}

