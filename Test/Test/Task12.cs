using System;
using System.IO;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    class Task12
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
        public void Test12()
        {
            Login();
            ClickOnHrefOfElements("li#app- a", "Catalog");
            ClickOnHrefOfElements("td#content a", "Add New Product");
            var elements = driver.FindElements(By.CssSelector(string.Format("[name='{0}']", "status")));
            elements[0].Click();

            // General
            EnterText("name[en]", "Blue Toy");
            EnterText("code", "test name");

            UncheckAllItems("[name='categories[]']");            
            CheckItem("[name='categories[]']", "data-name", "Rubber Ducks");

             SelectValueOfElement("[name='default_category_id']", "Rubber Ducks");

            UncheckAllItems("[name='product_groups[]']");
            CheckItem("[name='product_groups[]']", "value", "1-3");// Unisex

            EnterText("quantity", "150,33");

            SelectValueOfElement("[name='quantity_unit_id']", "pcs");
            SelectValueOfElement("[name='delivery_status_id']", "3-5 days");
            SelectValueOfElement("[name='sold_out_status_id']", "Temporary sold out");

            LoadPicture();
            EnterTextByMask("date_valid_from", "01.01.2015");
            EnterTextByMask("date_valid_to", "01.01.2025");


            // Information
            var tab = driver.FindElement(By.CssSelector("[href='#tab-information']"));
            tab.Click();
            SelectValueOfElement("select[name='manufacturer_id']", "ACME Corp.");
            EnterText("keywords", "test keywords");
            EnterText("short_description[en]", "test short_description");
            EnterTextByCssSelector(".trumbowyg-editor", "test information about product");
            EnterText("head_title[en]", "test head_title");
            EnterText("meta_description[en]", "test meta_description");

            // Information
            tab = driver.FindElement(By.CssSelector("[href='#tab-prices']"));
            tab.Click();
            EnterText("purchase_price", "25,25");
            SelectValueOfElement("select[name='purchase_price_currency_code']", "Euros");

            EnterText("prices[USD]", "25");
            EnterText("gross_prices[USD]", "2");

            EnterText("prices[EUR]", "35");
            EnterText("gross_prices[EUR]", "2");
            
            driver.FindElement(By.Name("save")).Click();
         
            Assert.IsTrue(ListContainsNewProduct("Blue Toy"));
        }
        [TearDown]
        public void Stop()
        {
            driver.Quit();
            driver = null;
        }

        private void Login()
        {
            driver.Url = "https://localhost/litecart/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
        }

        private void ClickOnHrefOfElements(string cssSelector, string value)
        {
            var elements = driver.FindElements(By.CssSelector(cssSelector));
            string href = elements.Where(item => item.GetAttribute("textContent").Contains(value))
                            .Select(item => item.GetAttribute("href"))
                            .Single();
            driver.FindElement(By.CssSelector(string.Format("{0}[href='{1}']", cssSelector, href))).Click();

        }

        private void EnterText(string elementName, string value)
        {
            IWebElement element = driver.FindElement(By.CssSelector(string.Format("[name='{0}']", elementName)));
            element.Clear();
            element.SendKeys(value);
        }

        public void EnterTextByCssSelector(string cssSeector, string value)
        {
            IWebElement element = driver.FindElement(By.CssSelector(cssSeector));
            element.Clear();
            element.SendKeys(value);
        }

        private void EnterTextByMask(string elementName, string value)
        {
            IWebElement element = driver.FindElement(By.CssSelector(string.Format("[name='{0}']", elementName)));
            element.SendKeys(Keys.Home + value);
        }

        private void UncheckAllItems(string cssSelector)
        {
            var elements = driver.FindElements(By.CssSelector(cssSelector));
            foreach (var element in elements)
            {
                if (element.GetAttribute("checked") == "true")
                {
                    element.Click();
                }
            }
        }

        private void CheckItem(string cssSelector, string attribute, string value)
        {
            var elements = driver.FindElements(By.CssSelector(cssSelector));
            var a = elements[0].GetAttribute(attribute);
            var element = elements.Where(item => item.GetAttribute(attribute).Equals(value))
                                  .Select(item => item)
                                  .Single();
            element.Click();
        }

        private void SelectValueOfElement(string cssSelector, string value)
        {
            IWebElement element = driver.FindElement(By.CssSelector(cssSelector));
            SelectElement se = new SelectElement(element);
            se.SelectByText(value);
        }

        private void LoadPicture()
        {
            var txtFileUpload = driver.FindElement(By.CssSelector("[type='file']"));            
            var sourceFile = AppDomain.CurrentDomain.BaseDirectory+@"\testData\test_image.jpg";
            txtFileUpload.SendKeys(sourceFile);
        }

        private bool ListContainsNewProduct(string expectedValue)
        {
            var rows = driver.FindElements(By.CssSelector("tr.row a"));
            foreach (var row in rows)
            {
                string actualValue = row.GetAttribute("textContent");
                if (actualValue.Equals(expectedValue))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
