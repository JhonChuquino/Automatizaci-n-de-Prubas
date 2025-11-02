using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;

namespace AutomatizacionPOM.Pages
{
    public class IngresoEgresosPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;
        private string tipoRegistroActual;

        public IngresoEgresosPage(IWebDriver driver)
        {
            this.driver = driver ?? throw new ArgumentNullException(nameof(driver));
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20))
            {
                PollingInterval = TimeSpan.FromMilliseconds(500)
            };
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
        }

        // ==========================
        // LOCALIZADORES
        // ==========================
        public static readonly By FechaInicialInput = By.XPath("//label[normalize-space()='Fecha Inicial']/following-sibling::div//input");
        public static readonly By FechaFinalInput = By.XPath("//label[normalize-space()='Fecha Final']/following-sibling::div//input");
        public static readonly By RadioCobros = By.XPath("//label[normalize-space()='Cobros'] | //span[normalize-space()='Cobros']");
        public static readonly By RadioPagos = By.XPath("//label[normalize-space()='Pagos'] | //span[normalize-space()='Pagos']");
        public static readonly By RadioIngreso = By.XPath("//button[normalize-space()='INGRESO']");
        public static readonly By RadioEgreso = By.XPath("//button[normalize-space()='EGRESO']");
        public static readonly By RadioEmpleado = By.XPath("//input[@name='cuenta' and @value='1']/following-sibling::label");
        public static readonly By RadioCliente = By.XPath("//input[@name='cuenta' and @value='2']/following-sibling::label");
        public static readonly By RadioProveedor = By.XPath("//input[@name='cuenta' and @value='3']/following-sibling::label");
        public static readonly By Modal = By.XPath("//div[@id='modal-registro-ingreso-egreso-varios']");
        public static readonly By RucAutorizado = By.XPath("//label[normalize-space()='AUTORIZADO POR']/following-sibling::input[1]");
        public static readonly By RucPagador = By.XPath("//div[@id='modal-registro-ingreso-egreso-varios']//label[normalize-space()='PAGADOR']/following::input[@placeholder='DNI/RUC']");
        public static readonly By Select2DocumentoArrow = By.XPath("//label[normalize-space()='DOCUMENTO']/following::span[@role='presentation'][1]/b");
        public static readonly By InputImporte = By.XPath("//div[@id='modal-registro-ingreso-egreso-varios']//input[contains(@ng-model,'Importe')]");
        public static readonly By InputObservacion = By.XPath("//label[normalize-space()='OBSERVACIÓN']/following-sibling::textarea");
        public static readonly By BtnGuardar = By.XPath("//div[@id='modal-registro-ingreso-egreso-varios']//button[normalize-space()='GUARDAR']");
        private By TituloInconsistencia => By.XPath("//h4[normalize-space()='INCONSISTENCIA(S):']");
        private By MensajesInconsistencia => By.XPath("//h4[normalize-space()='INCONSISTENCIA(S):']/following-sibling::table//td");

        // ==========================
        // MÉTODOS DE ACCIÓN
        // ==========================

        public void IngresarFechaInicial(string fecha)
        {
            if (fecha == "0") return;
            var campo = wait.Until(ExpectedConditions.ElementToBeClickable(FechaInicialInput));
            campo.Clear();
            campo.SendKeys(fecha);
            campo.SendKeys(Keys.Tab);
        }

        public void IngresarFechaFinal(string fecha)
        {
            if (fecha == "0") return;
            var campo = wait.Until(ExpectedConditions.ElementToBeClickable(FechaFinalInput));
            campo.Clear();
            campo.SendKeys(fecha);
            campo.SendKeys(Keys.Tab);
        }

        public bool SeleccionarTipoOperacion(string tipo)
        {
            if (string.IsNullOrWhiteSpace(tipo) || tipo == "0")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Tipo de operación no especificado. Se detiene el flujo para marcar inconsistencia.");
                Console.ResetColor();
                return true; // ? flujo inválido detectado
            }

            try
            {
                By radio = tipo switch
                {
                    "Cobros" => RadioCobros,
                    "Pagos" => RadioPagos,
                    _ => throw new ArgumentException($"Tipo no soportado: {tipo}")
                };

                var elemento = wait.Until(ExpectedConditions.ElementToBeClickable(radio));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", elemento);
                Console.WriteLine($"Tipo de operación seleccionado: {tipo}");
            }
            catch (WebDriverTimeoutException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"No se pudo seleccionar el tipo de operación '{tipo}' (elemento no visible).");
                Console.ResetColor();
                return true; // también cuenta como flujo inválido
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al seleccionar el tipo de operación: {ex.Message}");
                return true;
            }

            return false; // flujo válido
        }


        public void SeleccionarTipoRegistro(string tipo)
        {
            // Validar entrada vacía o inválida
            if (string.IsNullOrWhiteSpace(tipo) || tipo == "0")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No se seleccionó un tipo de registro válido. Se marcará inconsistencia sin continuar el flujo.");
                Console.ResetColor();
                return; // No continúa, evita timeouts
            }

            tipoRegistroActual = tipo;

            try
            {
                // Selecciona el radio correspondiente
                By locator = tipo == "Ingreso" ? RadioIngreso : RadioEgreso;
                var boton = wait.Until(ExpectedConditions.ElementToBeClickable(locator));
                boton.Click();

                // Espera que el modal se abra correctamente
                wait.Until(ExpectedConditions.ElementIsVisible(Modal));

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Modal de registro abierto correctamente para tipo: {tipo}");
                Console.ResetColor();
            }
            catch (WebDriverTimeoutException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"No se pudo abrir el modal para el tipo de registro '{tipo}'. Posible error de interfaz.");
                Console.ResetColor();
            }
        }


        public void SeleccionarAutorizado(string tipo)
        {
            if (tipo == "0") return;

            By radio = tipo switch
            {
                "Empleado" => RadioEmpleado,
                "Cliente" => RadioCliente,
                "Proveedor" => RadioProveedor,
                _ => throw new ArgumentException($"Tipo de autorizado '{tipo}' no reconocido.")
            };

            var elem = wait.Until(ExpectedConditions.ElementToBeClickable(radio));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", elem);
            Console.WriteLine($"Tipo de autorizado seleccionado: {tipo}");

            if (tipoRegistroActual == "Egreso")
            {
                var RucBeneficiario = By.XPath("//div[@id='modal-registro-ingreso-egreso-varios']//label[normalize-space()='BENEFICIARIO']/following::input[@placeholder='DNI/RUC']");
                wait.Until(ExpectedConditions.ElementToBeClickable(RucBeneficiario));
            }
            else
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(RucPagador));
            }
        }

        public void IngresarRucAutorizado(string ruc)
        {
            if (ruc == "0") return;

            var campo = wait.Until(ExpectedConditions.ElementToBeClickable(RucAutorizado));
            campo.Clear();
            campo.SendKeys(ruc);
            campo.SendKeys(Keys.Enter);
            Thread.Sleep(1000);
        }

        public bool IngresarRucDni(string ruc)
        {
            if (ruc == "0")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("RUC del pagador no especificado. Se detiene el flujo para marcar inconsistencia.");
                Console.ResetColor();
                return true; // flujo inválido
            }

            try
            {
                By campoLocator = tipoRegistroActual == "Egreso"
                    ? By.XPath("//div[@id='modal-registro-ingreso-egreso-varios']//label[normalize-space()='BENEFICIARIO']/following::input[@placeholder='DNI/RUC']")
                    : RucPagador;

                var campo = wait.Until(ExpectedConditions.ElementToBeClickable(campoLocator));
                campo.Clear();
                campo.SendKeys(ruc);
                campo.SendKeys(Keys.Enter);
                Thread.Sleep(1000);
                return false; // flujo válido
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al ingresar RUC/DNI: {ex.Message}");
                return true; // consideramos flujo inválido
            }
        }


        public void SeleccionarDocumento(string documento)
        {
            if (documento == "0") return;

            try
            {
                var inputSelect2Arrow = wait.Until(ExpectedConditions.ElementToBeClickable(Select2DocumentoArrow));
                inputSelect2Arrow.Click();

                Thread.Sleep(500);
                var opcionLocator = By.XPath($"//ul[contains(@id, 'select2') and contains(@id, 'results')]/li[normalize-space(text())='{documento}']");
                var opcion = wait.Until(ExpectedConditions.ElementToBeClickable(opcionLocator));
                opcion.Click();
            }
            catch
            {
                Console.WriteLine($"No se pudo seleccionar el documento '{documento}' (Select2 no disponible).");
            }
        }

        public void IngresarImporte(string importe)
        {
            var campo = wait.Until(ExpectedConditions.ElementToBeClickable(InputImporte));
            campo.Clear();
            campo.SendKeys(importe);
            campo.SendKeys(Keys.Tab);
        }

        public void IngresarObservacion(string observacion)
        {
            if (observacion == "0") observacion = "";

            var campo = wait.Until(ExpectedConditions.ElementToBeClickable(InputObservacion));
            campo.Clear();
            campo.SendKeys(observacion);
            campo.SendKeys(Keys.Tab);
            Console.WriteLine($"Observación ingresada: '{observacion}'");
        }

        public void HacerClicEnGuardar()
        {
            try
            {
                var btn = wait.Until(ExpectedConditions.ElementToBeClickable(BtnGuardar));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btn);
                btn.Click();
                Console.WriteLine(">>> Registro guardado (clic ejecutado)");
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine(">>> Botón Guardar no disponible (posible inconsistencia).");
            }
        }

        // ==========================
        // VALIDACIONES
        // ==========================

        public bool ExisteModalExito()
        {
            try
            {
                // Usamos 25 segundos para la espera crítica.
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(25));

                // 1. Localizar el título 'Correcto' dentro del modal. 
                //    (Clase 'swal-modal' garantiza que es la ventana correcta)
                var modalTitulo = wait.Until(ExpectedConditions.ElementIsVisible(
                    By.XPath("//div[@class='swal-modal']//div[@class='swal-title' and normalize-space()='Correcto']")
                ));

                // 2. Localizar el botón 'OK' y esperar a que sea clicable.
                var btnOk = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//div[@class='swal-modal']//button[@class='swal-button swal-button--confirm' and normalize-space()='OK']")
                ));

                bool visible = modalTitulo.Displayed && btnOk.Displayed;

                if (visible)
                {
                    Console.WriteLine("Modal de éxito detectado: Título 'Correcto' y botón 'OK' visibles.");
                    btnOk.Click();
                    Console.WriteLine("Boton OK presionado correctamente.");
                }

                return visible;
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($"No apareció el modal de éxito dentro del tiempo esperado (25s).");
                // Devuelve false para que el Assert en el Step Definition falle con el mensaje correcto.
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al verificar el modal de éxito: " + ex.Message);
                return false;
            }
        }



        public bool ExisteInconsistencia()
        {
            try
            {
                Thread.Sleep(1500); // espera que Angular renderice la tabla
                var titulos = driver.FindElements(TituloInconsistencia);
                var mensajes = driver.FindElements(MensajesInconsistencia);

                if (titulos.Count > 0 && mensajes.Count > 0)
                {
                    Console.WriteLine("Inconsistencias detectadas:");
                    foreach (var msg in mensajes)
                        Console.WriteLine("   - " + msg.Text);
                    return true;
                }

                var btnGuardar = driver.FindElement(BtnGuardar);
                if (!btnGuardar.Enabled)
                {
                    Console.WriteLine("Botón GUARDAR deshabilitado — inconsistencia detectada sin mensajes visibles.");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al verificar inconsistencias: " + ex.Message);
                return false;
            }
        }

    }
}
