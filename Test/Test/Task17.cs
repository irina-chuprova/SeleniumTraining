using System;
using System.IO;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.Events;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Imaging;

//[x] Задание 17. Проверьте отсутствие сообщений в логе браузера
//Сделайте сценарий, который проверяет, не появляются ли в логе браузера сообщения при открытии страниц в учебном приложении, а именно -- страниц товаров в каталоге в административной панели.
//Сценарий должен состоять из следующих частей:
//1) зайти в админку
//2) открыть каталог, категорию, которая содержит товары (страница http://localhost/litecart/admin/?app=catalog&doc=catalog&category_id=1)
//3) последовательно открывать страницы товаров и проверять, не появляются ли в логе браузера сообщения (любого уровня)

namespace Test
{
    class Task17
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
        public void Test17()
        {
            AdminPage.Login(driver, "admin", "admin");
            AdminPage.ClickOnHrefOfElements(driver, "li#app- a", "Catalog");
            AdminPage.ClickOnHrefOfElements(driver, ".dataTable a", "Rubber Ducks");
            AdminPage.ClickOnHrefOfElements(driver, ".dataTable a", "Subcategory");
            var objects = driver.FindElements(By.CssSelector(".dataTable a"));
            List<string> hrefs = new List<string>();


            foreach (var obj in objects)
            {
                var item = obj.GetAttribute("textContent");
                var href = obj.GetAttribute("href");
                if (item != "" && href.Contains("product"))
                {
                    hrefs.Add(href);
                    
                }
            }
            int i = 0;
            foreach (var href in hrefs)
            {
                
                driver.FindElement(By.CssSelector(string.Format(".dataTable a[href='{0}']", href))).Click();
                var logs = driver.Manage().Logs.GetLog("browser");               
                foreach (var log in logs)
                {
                    if (log.Message.Contains("Error"))
                    {
                        Console.WriteLine(log);
                        ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile("screen" + i + ".png", ImageFormat.Png);

                    }
                }
                driver.FindElement(By.CssSelector(".button-set [value=Cancel]")).Click();
                i++;
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
