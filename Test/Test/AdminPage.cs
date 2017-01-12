using System;
using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    class AdminPage
    {
        public static void Login(IWebDriver driver, string username, string password)
        {
            driver.Url = "https://localhost/litecart/admin/";
            driver.FindElement(By.Name("username")).SendKeys(username);
            driver.FindElement(By.Name("password")).SendKeys(password);
            driver.FindElement(By.Name("login")).Click();
        }

        public static void ClickOnHrefOfElements(IWebDriver driver, string cssSelector, string value)
        {
            var elements = driver.FindElements(By.CssSelector(cssSelector));
            string href = elements.Where(item => item.GetAttribute("textContent").Contains(value))
                            .Select(item => item.GetAttribute("href"))
                            .Single();
            driver.FindElement(By.CssSelector(string.Format("{0}[href='{1}']", cssSelector, href))).Click();

        }

        public static void GetAllExternalLinks(IWebDriver driver)
        {
            var elements = driver.FindElements(By.CssSelector("i.fa.fa-external-link"));           
            
        }
    }
}
