using System;
using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;

namespace Test.Task19
{
    internal class ProductPage : Page
    {
        public ProductPage(IWebDriver driver) : base(driver)
        {
            PageFactory.InitElements(driver, this);
        }

        public void AddProduct()
        {
            IWebElement button = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("[name='add_cart_product']")));
            button.Click();
        }
    }
}
