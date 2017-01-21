using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;
using Test.Task19;


//Сценарий должен состоять из следующих частей:
// 1) открыть страницу какого-нибудь товара
// 2) добавить его в корзину
// 3) подождать, пока счётчик товаров в корзине обновится
// 4) вернуться на главную страницу, повторить предыдущие шаги ещё два раза, чтобы в общей сложности в корзине было 3 единицы товара
// 5) открыть корзину (в правом верхнем углу кликнуть по ссылке Checkout)
// 6) удалить все товары из корзины один за другим, после каждого удаления подождать, пока внизу обновится таблица

namespace Test
{
    class Task19_1 : TestBase
    {
        
        [Test]                
        public void AddAndRemoveProducts()
        {
            Product prod = new Product("Red Duck");
            app.AddProductToCart(prod.name, "1");


            prod = new Product("Green Duck");            
            app.AddProductToCart(prod.name, "2");

            prod = new Product("Blue Duck");            
            app.AddProductToCart(prod.name, "3");

            
            app.RemoveAllProductFromBlanket();
            
            Assert.AreEqual(0, app.GetCountOfProductsInBlanket());
            app.Quit();
        }       
    }
}
