using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;
using System.Linq;

namespace Test.Task19
{
    internal class LiteCartPage : Page
    {
        public LiteCartPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
        }
        public void Open()
        {
            driver.Url = "https://localhost/litecart";
        }

        public void SelectProduct(string selectedProduct)
        {
            var elements = driver.FindElements(By.CssSelector("div.content li.product"));
            var element = elements.Where(item => item.FindElement(By.CssSelector(".name"))
                                                     .GetAttribute("textContent").Equals(selectedProduct))
                                   .Select(item => item)
                                   .First();
            element.Click();
        }

        public void CheckCounter(string count)
        {
            string cssSelector = "div#cart .quantity";
            IWebElement element = driver.FindElement(By.CssSelector(cssSelector));
            bool isChanged = wait.Until(ExpectedConditions.TextToBePresentInElement(element, count));
        }

        public void Checkout()
        {
            ClickOnHrefOfElements("div#cart a.link", "Checkout");
        }

        private void ClickOnHrefOfElements(string cssSelector, string value)
        {
            var elements = driver.FindElements(By.CssSelector(cssSelector));
            string href = elements.Where(item => item.GetAttribute("textContent").Contains(value))
                            .Select(item => item.GetAttribute("href"))
                            .Single();
            driver.FindElement(By.CssSelector(string.Format("{0}[href='{1}']", cssSelector, href))).Click();

        }
    }
}
