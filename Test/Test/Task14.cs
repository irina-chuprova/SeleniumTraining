using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

///Задание 14. Проверьте, что ссылки открываются в новом окне
//Сделайте сценарий, который проверяет, что ссылки на странице редактирования страны открываются в новом окне.
//Сценарий должен состоять из следующих частей:
//1) зайти в админку
//2) открыть пункт меню Countries (или страницу http://localhost/litecart/admin/?app=countries&doc=countries)
//3) открыть на редактирование какую-нибудь страну или начать создание новой
//4) возле некоторых полей есть ссылки с иконкой в виде квадратика со стрелкой -- они ведут на внешние страницы и открываются в новом окне, именно это и нужно проверить.
//Конечно, можно просто убедиться в том, что у ссылки есть атрибут target="_blank". Но в этом упражнении требуется именно кликнуть по ссылке, чтобы она открылась в новом окне, потом переключиться в новое окно, закрыть его, вернуться обратно, и повторить эти действия для всех таких ссылок.
//Не забудьте, что новое окно открывается не мгновенно, поэтому требуется ожидание открытия окна.
//Можно оформить сценарий либо как тест, либо как отдельный исполняемый файл.

namespace Test
{
    class Task14
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
        public void Test14()
        {
            AdminPage.Login(driver, "admin", "admin");
            AdminPage.ClickOnHrefOfElements(driver, "li#app- a", "Countries");
            AdminPage.ClickOnHrefOfElements(driver, "td#content a", "Add New Country");

            var elements = driver.FindElements(By.CssSelector("i.fa.fa-external-link"));
            string mainWindow = driver.CurrentWindowHandle;
            foreach (var element in elements)
            {                
                SwitchToNewWindow(element);
                CloseWindowAndSwitchToMainWindow(mainWindow);
            }
        }
        public void SwitchToNewWindow(IWebElement element)
        {
            
            ICollection<string> oldWindows = driver.WindowHandles;
            element.Click();
            WaitNewWindow(oldWindows, 25, 0);
            ICollection<string> allWindows = driver.WindowHandles;
            string newWindow="";
            foreach (var wind in allWindows)
            {
                if (!oldWindows.Contains(wind))
                {
                    newWindow = wind;
                }
            }       
            //wait.Until(ExpectedConditions.Equals(oldWindows, driver.WindowHandles));
            //String newWindow = wait.Until(ThereIsWindowOtherThan(oldWindows));
            driver.SwitchTo().Window(newWindow);                   
        }

        public void CloseWindowAndSwitchToMainWindow(string mainWindow)
        {
            driver.Close();
            driver.SwitchTo().Window(mainWindow);
        }

        [TearDown]
        public void Stop()
        {
            driver.Quit();
            driver = null;
        }

        private void WaitNewWindow(ICollection<string> oldWindows, int countAttempt, int currentAttempt)
        {
            currentAttempt = currentAttempt + 1;
            ICollection<string> allWindows = driver.WindowHandles;
            if (allWindows.Count > oldWindows.Count)
            {
                return;
            }
            else if (countAttempt < currentAttempt)
            {
                WaitNewWindow(oldWindows, countAttempt, currentAttempt);
            }
        }
    }
}
