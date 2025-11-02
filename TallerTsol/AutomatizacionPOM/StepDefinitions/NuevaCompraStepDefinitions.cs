using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll;
using AutomatizacionPOM.Pages;
using NUnit.Framework;

namespace AutomatizacionPOM.StepDefinitions
{
    [Binding]
    public class NuevaCompraStepDefinitions
    {
        private readonly IWebDriver driver;
        private NuevaCompraPage nuevaCompraPage;

        public NuevaCompraStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            nuevaCompraPage = new NuevaCompraPage(driver);
        }


        [Given(@"el usuario está en la página de nueva compra")]
        public void GivenElUsuarioEstaEnNuevaCompra()
        {
            nuevaCompraPage = new NuevaCompraPage(driver);
        }

        [When(@"el usuario selecciona el proveedor ""(.*)""")]
        public void WhenElUsuarioSeleccionaElProveedor(string proveedor)
        {
            nuevaCompraPage.SeleccionarProveedor(proveedor);
        }


        [When(@"define la fecha de registro ""(.*)""")]
        public void WhenDefineLaFechaDeRegistro(string fecha)
        {
            if (string.IsNullOrWhiteSpace(fecha) || fecha.Contains("(vacio)"))
                return;

            try
            {
                var inputFecha = driver.FindElement(By.Id("fechaRegistro"));

                // Espera hasta que el campo sea visible e interactuable
                var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(d => inputFecha.Displayed && inputFecha.Enabled);

                try
                {
                    // Primer intento: escritura manual normal
                    inputFecha.Click();
                    inputFecha.Clear();
                    inputFecha.SendKeys(fecha);
                    inputFecha.SendKeys(OpenQA.Selenium.Keys.Tab);
                    Console.WriteLine($"Fecha '{fecha}' ingresada manualmente en fechaRegistro.");
                }
                catch (ElementNotInteractableException)
                {
                    // Respaldo: escritura con JavaScript si falla la interacción
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    js.ExecuteScript(@"
                arguments[0].value = arguments[1];
                arguments[0].dispatchEvent(new Event('input', { bubbles: true }));
                arguments[0].dispatchEvent(new Event('change', { bubbles: true }));
            ", inputFecha, fecha);
                    Console.WriteLine($"Fecha '{fecha}' establecida por JavaScript en fechaRegistro.");
                }

                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al definir la fecha: " + ex.Message);
                throw;
            }
        }


        [When(@"selecciona el tipo de documento ""(.*)""")]
        public void WhenSeleccionaElTipoDeDocumento(string documento)
        {
            if (!string.IsNullOrWhiteSpace(documento) && !documento.Contains("(vacío)"))
                nuevaCompraPage.SeleccionarDocumento(documento);
        }

        [When(@"define la serie ""(.*)""")]
        public void WhenDefineLaSerie(string serie)
        {
            if (!string.IsNullOrWhiteSpace(serie) && !serie.Contains("(vacío)"))
                nuevaCompraPage.InputSerie.SendKeys(serie);
        }

        [When(@"define el numero de documento ""(.*)""")]
        public void WhenDefineElNumeroDeDocumento(string numeroDoc)
        {
            if (!string.IsNullOrWhiteSpace(numeroDoc) && !numeroDoc.Contains("(vacío)"))
                nuevaCompraPage.InputNumeroDocumento.SendKeys(numeroDoc);
        }

        [When(@"selecciona el tipo de entrega ""(.*)""")]
        public void WhenSeleccionaElTipoDeEntrega(string entrega)
        {
            if (entrega.ToLower().Contains("inmediata"))
                nuevaCompraPage.RadioEntregaInmediata.Click();
            else if (entrega.ToLower().Contains("diferida"))
                nuevaCompraPage.RadioEntregaDiferida.Click();
        }

        [When(@"selecciona el tipo de pago ""(.*)""")]
        public void WhenSeleccionaElTipoDePago(string tipoPago)
        {
            if (!string.IsNullOrWhiteSpace(tipoPago) && !tipoPago.Contains("(vacío)"))
                nuevaCompraPage.SeleccionarTipoPago(tipoPago);
        }

        [When(@"selecciona el tipo de compra ""(.*)""")]
        public void WhenSeleccionaElTipoDeCompra(string tipoCompra)
        {
            if (!string.IsNullOrWhiteSpace(tipoCompra) && !tipoCompra.Contains("(vacío)"))
                nuevaCompraPage.SeleccionarTipoCompra(tipoCompra);
        }

        [When(@"registra el producto ""(.*)""")]
        public void WhenRegistraElProducto(string producto)
        {
            if (!string.IsNullOrWhiteSpace(producto) && !producto.Contains("(vacío)"))
                nuevaCompraPage.InputCodigoProducto.SendKeys(producto);
        }

        [When(@"define la cantidad ""(.*)""")]
        public void WhenDefineLaCantidad(string cantidad)
        {
            if (!string.IsNullOrWhiteSpace(cantidad) && cantidad != "0")
                nuevaCompraPage.InputCantidad.SendKeys(cantidad);
        }

        [When(@"define el importe total ""(.*)""")]
        public void WhenDefineElImporteTotal(string importe)
        {
            if (!string.IsNullOrWhiteSpace(importe) && importe != "0.00" && importe != "0")
                nuevaCompraPage.InputImporte.SendKeys(importe);
        }

        [When(@"presiona el boton ""Guardar Compra""")]
        public void WhenPresionaElBotonGuardarCompra()
        {
            nuevaCompraPage.HacerClickGuardar();
        }


        // ============================
        // VALIDACIÓN DEL RESULTADO
        // ============================

        [Then(@"el sistema muestra el mensaje de compra ""(.*)""")]
        public void ThenResultado(string resultado)
        {
            if (resultado.Contains("se genera correctamente"))
            {
                Assert.IsTrue(nuevaCompraPage.CompraExitosa(),
                    "Se esperaba una compra exitosa, pero se detectaron inconsistencias.");
            }
            else if (resultado.Contains("inconsistencia"))
            {
                Assert.IsTrue(nuevaCompraPage.MsgInconsistencia.Displayed,
                    "No se mostró mensaje de inconsistencia esperado.");
            }
        }

    }
}
