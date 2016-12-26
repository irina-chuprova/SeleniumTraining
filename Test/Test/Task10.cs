using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace Test
{
    class Task10
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Start()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void Test10()
        {
            int numberOfElement =0;
            driver.Url = "https://localhost/litecart/en/";
            string link = driver.FindElements(By.CssSelector("div#box-campaigns li .link"))[numberOfElement].GetAttribute("href");
            string expectedTitle = driver.FindElements(By.CssSelector("div#box-campaigns li .link"))[numberOfElement].GetAttribute("title");

            var element = driver.FindElements(By.CssSelector("div#box-campaigns li .regular-price"))[numberOfElement];
            string expectedRegularPrice = element.GetAttribute("textContent");
            string expectedColorOfRegularPrice = element.GetCssValue("color");

            element = driver.FindElements(By.CssSelector("div#box-campaigns li .campaign-price"))[numberOfElement];
            string expectedCampaignPrice = element.GetAttribute("textContent");
            string expectedColorOfCampaignPrice = element.GetCssValue("color");

            driver.FindElement(By.CssSelector(string.Format("div#box-campaigns li [href='{0}']",link))).Click();

            string actualTitle = driver.FindElement(By.CssSelector("div#box-product h1.title")).GetAttribute("textContent");

            element =driver.FindElement(By.CssSelector("div#box-product .regular-price"));
            string actualRegularPrice = element.GetAttribute("textContent");
            string actualColorOfRegularPrice = element.GetCssValue("color");
            
            element =driver.FindElement(By.CssSelector("div#box-product .campaign-price"));
            string actualcampaignPrice = element.GetAttribute("textContent");
            string actualColorOfCampaignPrice = element.GetCssValue("color");

            Assert.AreEqual(expectedTitle, actualTitle, "Title of selected product is not correct.");
            Assert.AreEqual(expectedRegularPrice, actualRegularPrice, "Regular price of selected product is not correct.");            
            Assert.AreEqual(expectedCampaignPrice, actualcampaignPrice, "Campaign price of selected product is not correct.");

            List<string> greyColors = new List<string>{"rgba(119, 119, 119, 1)", "rgba(102, 102, 102, 1)"};
            Assert.Contains(expectedColorOfRegularPrice, greyColors);
            Assert.Contains(actualColorOfRegularPrice, greyColors);

            List<string> redColors = new List<string> { "rgba(204, 0, 0, 1)", "rgba(204, 0, 0, 1)" };
            Assert.Contains(expectedColorOfCampaignPrice, redColors);
            Assert.Contains(actualColorOfCampaignPrice, redColors);
        }

        [TearDown]
        public void Stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
