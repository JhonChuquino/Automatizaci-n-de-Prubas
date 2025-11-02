using AutomatizacionPOM.Pages;
using OpenQA.Selenium;
using Reqnroll;

namespace AutomatizacionPOM.StepDefinitions
{
    [Binding]
    public class LoginFeatureStepDefinitions
    {
        private readonly IWebDriver _driver;
        private AccessPage _accessPage;

        // Reqnroll inyectará automáticamente el driver creado en Hooks
        public LoginFeatureStepDefinitions(IWebDriver driver)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver), "El WebDriver no puede ser nulo.");
            _accessPage = new AccessPage(_driver);  // Inicializamos _accessPage aquí
        }

        [Given(@"el usuario ingresa al ambiente '(.*)'")]
        public void GivenElUsuarioIngresaAlAmbiente(string ambiente)
        {
            try
            {
                _accessPage.OpenToAplicattion(ambiente);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al acceder al ambiente: {ex.Message}");
                throw;  // Propagar el error si ocurre algo al intentar abrir la aplicación
            }
        }

        [When(@"el usuario inicia sesion con usuario '(.*)' y contrasena '(.*)'")]
        public void WhenElUsuarioIniciaSesionConUsuarioYContrasena(string user, string password)
        {
            try
            {
                _accessPage.LoginToApplication(user, password);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el login: {ex.Message}");
                throw;  // Propagar el error si falla el login
            }
        }

        [When(@"accede al modulo '(.*)'")]
        public void WhenAccedeAlModulo(string modulo)
        {
            try
            {
                _accessPage.enterModulo(modulo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al acceder al módulo {modulo}: {ex.Message}");
                throw;
            }
        }

        [When(@"accede al submodulo '(.*)'")]
        public void WhenAccedeAlSubmodulo(string submodulo)
        {
            try
            {
                _accessPage.enterSubModulo(submodulo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al acceder al submódulo {submodulo}: {ex.Message}");
                throw;
            }
        }
    }
}
