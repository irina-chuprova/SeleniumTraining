using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;




namespace Test
{
    class Task11
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
        public void Test11()
        {
            string email = TestCommon.GenerateRandomString(5) + "@test.test";
            string password = "qqq111!";

            driver.Url = "https://localhost/litecart/en/";
            var elements = driver.FindElements(By.CssSelector("#box-account-login a"));
            string link=elements.Where(item => item.GetAttribute("textContent").Equals("New customers click here")).Select(item => item.GetAttribute("href")).Single();
            driver.FindElement(By.CssSelector(string.Format("#box-account-login a[href='{0}']", link))).Click();
            
            EnterText("company", "test company");
            EnterText("address1", "test address1");
            EnterText("address2", "test address2");
            EnterText("firstname", "test firstname");
            EnterText("lastname", "test lastname");
            EnterText("postcode", "98778");
            EnterText("city", "Toronto");
            SelectValueOfElement("[name='country_code']", "Italy");
            EnterText("email", email);
            EnterText("phone", "+39119119191");
            EnterText("password", password);
            EnterText("confirmed_password", password);


            driver.FindElement(By.CssSelector("[name='create_account']")).Click();

            Logout();

            EnterTextByCssSelector("#box-account-login input[name='email']", email);
            EnterTextByCssSelector("#box-account-login input[name='password']", password);
            
            driver.FindElement(By.CssSelector(".button-set [name='login']")).Click();
            Logout();
    }
        [TearDown]
        public void Stop()
        {
            driver.Quit();
            driver = null;
        }

        public void SelectValueOfElement(string cssSelector, string value)
        {
            IWebElement country = driver.FindElement(By.CssSelector(cssSelector));
            SelectElement c = new SelectElement(country);
            c.SelectByText(value);
        }
        public void EnterText(string elementName, string value)
        {
            IWebElement element =driver.FindElement(By.CssSelector(string.Format("[name='{0}']", elementName)));
            element.Clear();
            element.SendKeys(value);            
        }

        public void EnterTextByCssSelector(string cssSeector, string value)
        {
            IWebElement element = driver.FindElement(By.CssSelector(cssSeector));
            element.Clear();
            element.SendKeys(value);
        }

        private void Logout()
        {
            var elements = driver.FindElements(By.CssSelector("#box-account a"));
            string link = elements.Where(item => item.GetAttribute("textContent").Equals("Logout")).Select(item => item.GetAttribute("href")).Single();
            driver.FindElement(By.CssSelector(string.Format("#box-account a[href='{0}']", link))).Click();
        }

    }
}
