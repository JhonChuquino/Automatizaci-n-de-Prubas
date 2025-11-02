using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatizacionPOM.Pages.Helpers
{
    public class Utilities
    {
        private IWebDriver driver;

        public Utilities(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void ClickButton(By _path)
        {
            driver.FindElement(_path).Click();
            Thread.Sleep(4000);
        }

        public void EnterText(By _path, string _field)
        {
            driver.FindElement(_path).SendKeys(_field);
            Thread.Sleep(4000);
        }

        public void ClearAndEnterText(By _path, string _field)
        {
            var element = driver.FindElement(_path);
            element.SendKeys(Keys.Control + "a");
            EnterText(_path, _field);
            Thread.Sleep(4000);
        }

        public void Enter(By _path)
        {
            var element = driver.FindElement(_path);
            element.SendKeys(Keys.Enter);
            Thread.Sleep(4000);
        }

        public void SelectOption(By pathComponent, string option)
        {
            Thread.Sleep(4000);
            try
            {
                IWebElement dropdown = driver.FindElement(pathComponent);
                dropdown.Click();

                Thread.Sleep(4000);

                IWebElement optionElement = driver.FindElement(By.XPath($"//li[contains(text(), '{option}')]"));
                optionElement.Click();
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine($"Error: No se encontró la opción '{option}' en el menú desplegable. Detalle: {ex.Message}");
            }
            Thread.Sleep(4000);
        }
    }


}