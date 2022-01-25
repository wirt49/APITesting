﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter.Configuration;

namespace APIDemo.Helpers
{
    public static class Reporter
    {
        public static ExtentReports extentReports;
        public static ExtentHtmlReporter htmlReporter;
        public static ExtentTest testCase;

        public static void SetupExtentReport(string repornName, string documentTitle, dynamic path)
        {
            htmlReporter = new ExtentHtmlReporter(path);
            htmlReporter.Config.Theme = Theme.Standard;
            htmlReporter.Config.DocumentTitle = documentTitle;
            htmlReporter.Config.ReportName = repornName;

            extentReports = new ExtentReports();
            extentReports.AttachReporter(htmlReporter);
        }

        

        public static void CreateTest(string testName)
        {
            testCase = extentReports.CreateTest(testName);
        }

        public static void LogReport(Status status, string message)
        {
            testCase.Log(status, message);
        }

        public static void FlushReport()
        {
            extentReports.Flush();
        }

        public static void TestStatus(string status)
        {
            if (status.Equals("Pass"))
            {
                testCase.Pass("Test is passed");
            }
            else
            {
                testCase.Fail("Test is failed");
            }
        }
    }
}
