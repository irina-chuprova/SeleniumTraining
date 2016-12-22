using System;
using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace Test
{
    [TestFixture]
    public class Test7
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
        public void TestPage()
        {
            driver.Url = "https://localhost/litecart/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();


            List<Menu> mainMenuItems = new List<Menu>();
            var elements = driver.FindElements(By.CssSelector("li#app- a"));                        
            foreach (var element in elements)
            {
                var tmpHref = element.GetAttribute("href");                                
                mainMenuItems.Add(new Menu(tmpHref));                
            }

            foreach (var item in mainMenuItems)
            {
                driver.FindElement(By.CssSelector(string.Format("li#app- [href='{0}']", item.reference))).Click();                
                elements = driver.FindElements(By.CssSelector("li#app- ul.docs li a"));
                foreach (var element in elements)
                {
                    var tmpHref = element.GetAttribute("href");                    
                    item.subMenu.Add(new Menu(tmpHref));    
                }
                foreach (var subItem in item.subMenu)
                {
                    driver.FindElement(By.CssSelector(string.Format("ul#box-apps-menu li#app- ul.docs [href='{0}']", subItem.reference))).Click();
                    ElementIsPresent(By.CssSelector("h1"));
                }
            }
        }

        [TearDown]
        public void Stop()
        {
            driver.Quit();
            driver = null;
        }

        private bool ElementIsPresent(By locator)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                IWebElement element = wait.Until(ExpectedConditions.ElementExists(locator));
                return true;
            }
            catch (TimeoutException ex)
            { 
                return false;
            }
        }

        class Menu
        {
            public string reference;
            public List<Menu> subMenu = new List<Menu>();            

            public Menu(string reference)
            {
                this.reference = reference;
            }
        }
    }
}
