using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Task19
{    
    public class TestBase
    {
        public Application app {get;set;}
        [SetUp]
        public void Start()
        {
            app = new Application();
        }
    }
}
