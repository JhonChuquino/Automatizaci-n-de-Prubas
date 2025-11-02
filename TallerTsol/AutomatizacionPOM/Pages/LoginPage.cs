using AutomatizacionPOM.Pages.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace AutomatizacionPOM.Pages
{
    public class AccessPage
    {
        private readonly IWebDriver _driver;
        private readonly Utilities _utilities;

        // Elementos para login
        private readonly By usernameField = By.XPath("//input[@id='Email']");
        private readonly By passwordField = By.XPath("//input[@id='Password']");
        private readonly By loginButton = By.XPath("//button[normalize-space()='Iniciar']");
        private readonly By acceptButton = By.XPath("//button[contains(text(),'Aceptar')]");
        private readonly By logo = By.XPath("//img[@id='ImagenLogo']");

        // Módulos
        private readonly By TesoreriaField = By.XPath("//span[normalize-space()='Tesorería y Finanzas']");
        private readonly By IngresoField = By.XPath("//a[normalize-space()='Ingresos/Egresos']");

        public AccessPage(IWebDriver driver)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver), "El WebDriver no puede ser nulo.");
            _utilities = new Utilities(driver);
        }

        // Métodos

        public void OpenToAplicattion(string url)
        {
            _driver.Navigate().GoToUrl(url);
            WaitForPageToLoad();
        }

        public void LoginToApplication(string _username, string _password)
        {
            _utilities.EnterText(usernameField, _username);
            WaitForPageToLoad();

            _utilities.EnterText(passwordField, _password);
            WaitForPageToLoad();

            _utilities.ClickButton(loginButton);
            WaitForPageToLoad();

            _utilities.ClickButton(acceptButton);
            WaitForPageToLoad();

            // Verificación del login
            var successElement = _driver.FindElement(logo);
            Assert.IsNotNull(successElement, "No se encontró el logo después del login.");
        }

        public void enterModulo(string _modulo)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));

            try
            {
                switch (_modulo)
                {
                    case "Tesoreria y Finanzas":
                        // Esperar hasta que el elemento esté visible
                        var tesoreriaElemento = wait.Until(d => d.FindElement(TesoreriaField));
                        Console.WriteLine($"Elemento encontrado: {tesoreriaElemento.Text}"); // Mensaje de depuración
                        tesoreriaElemento.Click();
                        break;
                    default:
                        throw new ArgumentException($"El módulo '{_modulo}' no es válido.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al acceder al módulo: {ex.Message}");
                throw;
            }

            WaitForPageToLoad();
        }



        public void enterSubModulo(string _submodulo)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));

            try
            {
                if (_submodulo == "Ingresos/Egresos")
                {
                    // Esperar a que el enlace sea visible
                    var submodulo = wait.Until(ExpectedConditions.ElementIsVisible(IngresoField));

                    // Scroll suave al centro
                    ((IJavaScriptExecutor)_driver).ExecuteScript(
                        "arguments[0].scrollIntoView({block: 'center'});", submodulo);

                    // Esperar a que sea clickable
                    var clickable = wait.Until(ExpectedConditions.ElementToBeClickable(IngresoField));

                    // Click con fallback a JS
                    try { clickable.Click(); }
                    catch { ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", clickable); }
                }
                else
                {
                    throw new ArgumentException($"Submódulo no soportado: {_submodulo}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Submódulo: {ex.Message}");
                throw;
            }

            WaitForPageToLoad();
        }

        // Espera explícita para asegurar que la página se haya cargado
        private void WaitForPageToLoad()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
        }
    }
}
