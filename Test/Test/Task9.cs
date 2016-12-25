using System;
using System.Globalization;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;


namespace Test
{
    [TestFixture]
    public class Test9
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
        public void TestCountries()
        {
            driver.Url = "https://localhost/litecart/admin/?app=countries&doc=countries";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();

            // part a
            int cellIndex = GetCellIndex("Name");
            var elements = driver.FindElements(By.XPath(string.Format("//tr[@class='row']/td[{0}]", cellIndex.ToString())));
            for (int i = 0; i < elements.Count-1;i++ )
            {
                var country1 = elements[i].GetAttribute("innerText");
                var country2 = elements[i+1].GetAttribute("innerText");                
                Assert.IsTrue(GetResultOfComparing(country1, country2));
            }

                     
            //part b 
            elements = driver.FindElements(By.XPath("//tr[@class='row']"));
            for (int i = 0; i < elements.Count - 1; i++)
            {
                cellIndex = GetCellIndex("Zones");  
                var el = elements[i].FindElements(By.XPath(string.Format("//td[{0}]", cellIndex.ToString())))[i];
                int countOfZones = int.Parse(el.GetAttribute("innerText"));

                if (countOfZones > 0)
                {
                    cellIndex = GetCellIndex("Name");
                    el = elements[i].FindElements(By.XPath(string.Format("//td[{0}]/a", cellIndex.ToString())))[i];
                    string href = el.GetAttribute("href");                    
                    driver.FindElement(By.CssSelector(string.Format("tr.row [href='{0}']", href))).Click();

                     cellIndex = GetCellIndex("Name");                     
                     elements = driver.FindElements(By.CssSelector(string.Format("table#table-zones tr >td:nth-of-type({0})", cellIndex.ToString())));
                     for (int j = 0; j < elements.Count - 1 && elements[j + 1].GetAttribute("innerText")!=""; j++)
                    {
                        var country1 = elements[j].GetAttribute("innerText");                        
                        var country2 = elements[j + 1].GetAttribute("innerText");
                        Assert.IsTrue(GetResultOfComparing(country1, country2));
                    }
                }
            }
            
        }

        [TearDown]
        public void Stop()
        {
            driver.Quit();
            driver = null;
        }

        private int GetCellIndex(string columnName)
        {
            var headers = driver.FindElements(By.CssSelector("table.dataTable .header th"));
            int cellIndex = int.Parse(headers.Where(item => item.GetAttribute("textContent").Equals(columnName)).Select(item => item.GetAttribute("cellIndex")).Single()) + 1;
            return cellIndex;
        }

        private  bool GetResultOfComparing(string country1, string country2)
        {
            int resultOfComparing = String.Compare(country1, 0, country2, 0, 100, new CultureInfo("en-US"), CompareOptions.IgnoreCase);
            if (resultOfComparing < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
