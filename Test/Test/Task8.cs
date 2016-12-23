using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace Test
{
    [TestFixture]
    public class Test8
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
        public void TestStickers()
        {
            driver.Url = "https://localhost/litecart";
            var elements = driver.FindElements(By.CssSelector("div.content li.product"));         
            foreach(var element in elements)
            {
                var countOfSticker = element.FindElements(By.CssSelector("div.sticker")).Count;
                Assert.AreEqual(1, countOfSticker);
            }
        }

        [TearDown]
        public void Stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
