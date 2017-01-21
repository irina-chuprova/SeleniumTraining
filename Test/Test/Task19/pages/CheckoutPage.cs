using System;
using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;

namespace Test.Task19
{
    internal class CheckoutPage : Page
    {
        public CheckoutPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
        }

        public void RemoveAllProducts()
        {
            var elements = driver.FindElements(By.CssSelector("td.item"));
            if (elements.Count > 0)
            {
                RemoveProduct(elements[0]);
                wait.Until(ExpectedConditions.StalenessOf(elements[0]));
                RemoveAllProducts();
            }
            else
            {
                return;
            }
        }

        public void RemoveProduct(IWebElement element)
        {
            element.Click();
            IWebElement button = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("[name='remove_cart_item']")));
            button.Click();
        }

        [FindsBy(How = How.CssSelector, Using = "td.item")]
        IList<IWebElement> productRow;
        public int GetCountProductInBasket()
        {
            int actualValue = productRow.Count;
            return actualValue;
        }
    }
}
