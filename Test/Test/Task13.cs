using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;

//Сценарий должен состоять из следующих частей:
// 1) открыть страницу какого-нибудь товара
// 2) добавить его в корзину
// 3) подождать, пока счётчик товаров в корзине обновится
// 4) вернуться на главную страницу, повторить предыдущие шаги ещё два раза, чтобы в общей сложности в корзине было 3 единицы товара
// 5) открыть корзину (в правом верхнем углу кликнуть по ссылке Checkout)
// 6) удалить все товары из корзины один за другим, после каждого удаления подождать, пока внизу обновится таблица

namespace Test
{
    class Task13
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
        public void Test13()
        {
            driver.Url = "https://localhost/litecart";
            AddProductToCart("Red Duck");
            CheckCounter("div#cart .quantity", "1");

            driver.Url = "https://localhost/litecart";
            AddProductToCart("Green Duck");
            CheckCounter("div#cart .quantity", "2");

            driver.Url = "https://localhost/litecart";
            AddProductToCart("Blue Duck");
            CheckCounter("div#cart .quantity", "3");

            ClickOnHrefOfElements("div#cart a.link", "Checkout");
            RemoveAllProducts();
        }

        private void RemoveAllProducts()        
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

        [TearDown]
        public void Stop()
        {
            driver.Quit();
            driver = null;
        }

        private void AddProductToCart(string selectedProduct)
        {
            var elements = driver.FindElements(By.CssSelector("div.content li.product"));
            var element = elements.Where(item => item.FindElement(By.CssSelector(".name"))
                                                     .GetAttribute("textContent").Equals(selectedProduct))
                                   .Select(item => item)
                                   .First();
            element.Click();
            IWebElement button = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("[name='add_cart_product']")));
            button.Click();            
        }

        private void CheckCounter(string cssSelector, string count)
        {
            IWebElement element = driver.FindElement(By.CssSelector(cssSelector));
            bool isChanged = wait.Until(ExpectedConditions.TextToBePresentInElement(element, count));            
        }

        private void ClickOnHrefOfElements(string cssSelector, string value)
        {
            var elements = driver.FindElements(By.CssSelector(cssSelector));
            string href = elements.Where(item => item.GetAttribute("textContent").Contains(value))
                            .Select(item => item.GetAttribute("href"))
                            .Single();
            driver.FindElement(By.CssSelector(string.Format("{0}[href='{1}']", cssSelector, href))).Click();

        }

        private void RemoveProduct(IWebElement element)
        {
            element.Click();
            IWebElement button = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("[name='remove_cart_item']")));
            button.Click();
        }

        private int GetCountProductInBasket()
        {
            int actualValue = driver.FindElements(By.CssSelector("td.item")).Count;
            return actualValue;
        }

        private IWebElement GetProductFromBasketByName(string name)
        {
            IWebElement element = driver.FindElements(By.CssSelector("td.item"))
                                        .Where(item => item.GetAttribute("textContent").Equals(name))
                                        .Select(item => item)
                                        .First();
            return element;
        }
    }
}
