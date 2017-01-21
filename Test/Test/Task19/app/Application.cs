using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Task19
{
    public class Application
    {
        private IWebDriver driver;

        private LiteCartPage liteCartPage;
        private ProductPage productPage;
        private CheckoutPage checkoutPage;

        private WebDriverWait wait;

        public Application()
        {
            driver = new ChromeDriver();
            liteCartPage = new LiteCartPage(driver);
            productPage = new ProductPage(driver);
            checkoutPage = new CheckoutPage(driver);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void Quit()
        {
            driver.Quit();
            driver = null;
        }

        public void AddProductToCart(string selectedProduct, string count)
        {
            liteCartPage.Open();
            liteCartPage.SelectProduct(selectedProduct);
            productPage.AddProduct();
            liteCartPage.CheckCounter(count);
        }

        public void RemoveAllProductFromBlanket()
        {
            liteCartPage.Checkout();
            checkoutPage.RemoveAllProducts();
        }

        public int GetCountOfProductsInBlanket()
        {
            return checkoutPage.GetCountProductInBasket();
        }
    }
}
