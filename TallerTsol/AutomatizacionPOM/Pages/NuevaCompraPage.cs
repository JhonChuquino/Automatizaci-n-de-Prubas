using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumExtras.WaitHelpers;


namespace AutomatizacionPOM.Pages
{
    internal class NuevaCompraPage
    {
        private readonly IWebDriver driver;

        public NuevaCompraPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        // ==========================
        // ELEMENTOS PRINCIPALES
        // ==========================

        public IWebElement InputProveedor => driver.FindElement(By.XPath("//input[contains(@placeholder,'Proveedor') or @type='text'][1]"));
        public IWebElement InputFecha => driver.FindElement(By.XPath("//input[contains(@placeholder,'dd/mm/aaaa') or @type='date']"));
        public IWebElement SelectDocumento => driver.FindElement(By.XPath("//div[@class='col-md-4']//label[normalize-space(text())='DOCUMENTO']/following-sibling::select[@class='select2 form-control tipoDocumento']/following-sibling::span//span[@class='select2-selection__rendered']"));

        public IWebElement InputSerie => driver.FindElement(By.XPath("//input[contains(@placeholder,'Serie') or contains(@name,'serie')]"));
        public IWebElement InputNumeroDocumento => driver.FindElement(By.XPath("//input[contains(@placeholder,'Número') or contains(@name,'numeroDocumento')]"));

        // ==========================
        // ENTREGA / ALMACÉN
        // ==========================
        public IWebElement RadioEntregaInmediata => driver.FindElement(By.XPath("//label[contains(.,'INMEDIATA')]/preceding-sibling::input"));
        public IWebElement RadioEntregaDiferida => driver.FindElement(By.XPath("//label[contains(.,'DIFERIDA')]/preceding-sibling::input"));

        public IWebElement RadioAlmacenUno => driver.FindElement(By.XPath("//label[contains(.,'UNO')]/preceding-sibling::input"));
        public IWebElement RadioAlmacenVarios => driver.FindElement(By.XPath("//label[contains(.,'VARIOS')]/preceding-sibling::input"));
        public IWebElement SelectRol => driver.FindElement(By.XPath("//select[contains(@name,'rol') or contains(@formcontrolname,'rol')]"));
        public IWebElement SelectAlmacen => driver.FindElement(By.XPath("//select[contains(@name,'almacen') or contains(@formcontrolname,'almacen')]"));

        // ==========================
        // PAGO
        // ==========================
        public IWebElement RadioPagoCO => driver.FindElement(By.XPath("//input[@type='radio' and @value='CO']"));
        public IWebElement RadioPagoCR => driver.FindElement(By.XPath("//input[@type='radio' and @value='CR']"));
        public IWebElement RadioPagoCC => driver.FindElement(By.XPath("//input[@type='radio' and @value='CC']"));

        public IWebElement BtnDeposito => driver.FindElement(By.XPath("//button[contains(.,'DEPCU')]"));
        public IWebElement BtnTransferencia => driver.FindElement(By.XPath("//button[contains(.,'TRANFON')]"));
        public IWebElement BtnTarjetaDebito => driver.FindElement(By.XPath("//button[contains(.,'TDEB')]"));
        public IWebElement BtnTarjetaCredito => driver.FindElement(By.XPath("//button[contains(.,'TCRE')]"));
        public IWebElement BtnEfectivo => driver.FindElement(By.XPath("//button[contains(.,'EF')]"));

        public IWebElement InputCuentaProveedor => driver.FindElement(By.XPath("//input[contains(@placeholder,'cuenta') or contains(@name,'cuentaProveedor')]"));
        public IWebElement InputOperacion => driver.FindElement(By.XPath("//textarea[contains(@placeholder,'Operación') or contains(@name,'informacion')]"));

        // ==========================
        // TIPO DE COMPRA
        // ==========================
        public IWebElement RadioExonerada => driver.FindElement(By.XPath("//label[contains(.,'EXONERADAS')]/preceding-sibling::input"));
        public IWebElement RadioGravadasG => driver.FindElement(By.XPath("//label[contains(.,'G') and not(contains(.,'NG'))]/preceding-sibling::input"));
        public IWebElement RadioGravadasNG => driver.FindElement(By.XPath("//label[contains(.,'NG') and not(contains(.,'Y NG'))]/preceding-sibling::input"));
        public IWebElement RadioGravadasGYNG => driver.FindElement(By.XPath("//label[contains(.,'G Y NG')]/preceding-sibling::input"));

        // ==========================
        // PRODUCTO
        // ==========================
        public IWebElement InputCodigoProducto => driver.FindElement(By.XPath("//input[contains(@placeholder,'Código barra')]"));
        public IWebElement InputConcepto => driver.FindElement(By.XPath("//input[contains(@placeholder,'Concepto')]"));
        public IWebElement InputCantidad => driver.FindElement(By.XPath("//input[contains(@name,'cantidad') or contains(@formcontrolname,'cantidad')]"));
        public IWebElement InputImporte => driver.FindElement(By.XPath("//input[contains(@name,'importe') or contains(@formcontrolname,'importe')]"));
        public IWebElement BtnAgregarProducto => driver.FindElement(By.XPath("//button[contains(@class,'btn') and contains(.,'+')]"));

        // ==========================
        // MENSAJES DE ERROR
        // ==========================
        public IWebElement MsgInconsistencia => driver.FindElement(By.XPath("//div[contains(.,'INCONSISTENCIA') or contains(@class,'alert')]"));
        public IWebElement MsgCompletarCampos => driver.FindElement(By.XPath("//div[contains(text(),'completar') or contains(text(),'necesario')]"));

        // ==========================
        // BOTONES
        // ==========================
        public IWebElement BtnGuardarCompra => driver.FindElement(By.XPath("//button[contains(.,'GUARDAR COMPRA') or contains(.,'Guardar Compra')]"));

        // ==========================
        // MÉTODOS DE ACCIÓN
        // ==========================

        public void SeleccionarProveedor(string proveedor)
        {
            var input = InputProveedor;
            input.Clear();
            input.SendKeys(proveedor);
            input.SendKeys(Keys.Tab); // simula salida para activar validaciones
            Console.WriteLine($"Proveedor '{proveedor}' ingresado correctamente.");
        }

        public void SeleccionarDocumento(string tipoDocumento)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60)); // Aumentamos el tiempo de espera
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            try
            {
                // Clic en el contenedor select2 para abrir el dropdown
                Console.WriteLine("Intentando abrir el selector de documento...");
                var contenedor = wait.Until(d =>
                {
                    var e = d.FindElement(By.XPath("//div[@class='col-md-4']//label[normalize-space(text())='DOCUMENTO']/following-sibling::select[@class='select2 form-control tipoDocumento']/following-sibling::span//span[@class='select2-selection select2-selection--single']"));
                    return (e.Displayed && e.Enabled) ? e : null;
                });

                // Hacemos clic en el contenedor para abrir el dropdown
                contenedor.Click();
                wait.Until(d => d.FindElements(By.CssSelector("span.select2-container--open")).Count > 0); // Esperar que el dropdown se abra
                Console.WriteLine("Dropdown abierto.");

                // Obtener todas las opciones del dropdown
                var opciones = wait.Until(d =>
                {
                    var lista = d.FindElements(By.XPath("//ul[@class='select2-results__options']//li[contains(@class, 'select2-results__option')]"));
                    return lista.Count > 0 ? lista : null;
                });

                // Buscar la opción que contiene el texto deseado (tipo de documento)
                var opcion = opciones.FirstOrDefault(o =>
                    o.Text.Trim().Equals(tipoDocumento, StringComparison.OrdinalIgnoreCase)); // Usamos Equals para una comparación más precisa

                if (opcion == null)
                {
                    Console.WriteLine($"No se encontró la opción '{tipoDocumento}'. Opciones disponibles:");
                    foreach (var o in opciones) Console.WriteLine($" - {o.Text}");
                    throw new Exception("Opción no encontrada en el dropdown.");
                }

                // Hacer scroll hacia la opción y hacer clic en ella
                js.ExecuteScript("arguments[0].scrollIntoView(true);", opcion);  // Scroll hacia la opción seleccionada
                js.ExecuteScript("arguments[0].click();", opcion);  // Hacer clic en la opción seleccionada

                Console.WriteLine($"Documento '{tipoDocumento}' seleccionado correctamente.");

                // Esperar que el dropdown se cierre después de seleccionar
                wait.Until(d => !d.FindElements(By.CssSelector("span.select2-container--open")).Any()); // Esperar que el dropdown se cierre
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al seleccionar documento '{tipoDocumento}': {ex.Message}");
                throw;
            }
        }



        public void SeleccionarTipoPago(string tipoPago)
        {
            switch (tipoPago.ToUpper())
            {
                case "CO":
                    RadioPagoCO.Click(); break;
                case "CR":
                    RadioPagoCR.Click(); break;
                case "CC":
                    RadioPagoCC.Click(); break;
            }
        }

        public void SeleccionarTipoCompra(string tipoCompra)
        {
            if (tipoCompra.ToLower().Contains("exoneradas")) RadioExonerada.Click();
            else if (tipoCompra.Contains("G Y NG")) RadioGravadasGYNG.Click();
            else if (tipoCompra.Contains("G") && !tipoCompra.Contains("NG")) RadioGravadasG.Click();
            else if (tipoCompra.Contains("NG")) RadioGravadasNG.Click();
        }

        public void HacerClickGuardar()
        {
            BtnGuardarCompra.Click();
        }

        public bool CompraExitosa()
        {
            return !MsgInconsistencia.Displayed;
        }
    }
}
