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
        private DateTime? fechaInicialGuardada;
        private string rucAutorizadoGuardado;

        public IngresoEgresosPage(IWebDriver driver)
        {
            this.driver = driver ?? throw new ArgumentNullException(nameof(driver));
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20))
            {
                PollingInterval = TimeSpan.FromMilliseconds(500)
            };
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            fechaInicialGuardada = null;
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

        public bool IngresarFechaInicial(string fecha)
        {
            if (fecha == "0") return false;

            try
            {
                var campo = wait.Until(ExpectedConditions.ElementToBeClickable(FechaInicialInput));
                campo.Clear();
                campo.SendKeys(fecha);
                campo.SendKeys(Keys.Tab);

                // Esperar a que el campo procese la fecha
                Thread.Sleep(800);

                // Verificar si la fecha fue auto-corregida
                string valorFinal = campo.GetAttribute("value");

                if (valorFinal != fecha && !string.IsNullOrEmpty(valorFinal))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"?? FECHA AUTO-CORREGIDA POR EL SISTEMA:");
                    Console.WriteLine($"   • Fecha ingresada: {fecha}");
                    Console.WriteLine($"   • Fecha corregida a: {valorFinal}");
                    Console.WriteLine($"   • Esto indica que '{fecha}' es INVÁLIDA");
                    Console.ResetColor();

                    // Intentar parsear la fecha corregida
                    if (DateTime.TryParseExact(valorFinal, "dd/MM/yyyy", null,
                        System.Globalization.DateTimeStyles.None, out DateTime fechaCorregida))
                    {
                        fechaInicialGuardada = fechaCorregida;
                        Console.WriteLine($"   • Sistema continuará con: {fechaCorregida:dd/MM/yyyy}");
                    }

                    return true; // Marcar como flujo inválido (fecha fue rechazada)
                }

                // Si la fecha no cambió, es válida
                if (DateTime.TryParseExact(fecha, "dd/MM/yyyy", null,
                    System.Globalization.DateTimeStyles.None, out DateTime fechaParsed))
                {
                    fechaInicialGuardada = fechaParsed;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"? Fecha inicial aceptada: {fechaInicialGuardada:dd/MM/yyyy}");
                    Console.ResetColor();
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"? Error al ingresar fecha inicial: {ex.Message}");
                Console.ResetColor();
                return true;
            }
        }

        public bool IngresarFechaFinal(string fecha)
        {
            if (fecha == "0") return false;

            try
            {
                var campoFinal = wait.Until(ExpectedConditions.ElementToBeClickable(FechaFinalInput));

                // Validar fecha inicial vs final ANTES de ingresar
                if (fechaInicialGuardada.HasValue &&
                    DateTime.TryParseExact(fecha, "dd/MM/yyyy", null,
                        System.Globalization.DateTimeStyles.None, out DateTime fechaFinal))
                {
                    if (fechaFinal < fechaInicialGuardada.Value)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"?? Fecha final ({fechaFinal:dd/MM/yyyy}) < Fecha inicial ({fechaInicialGuardada:dd/MM/yyyy})");
                        Console.ResetColor();

                        // Verificar si el campo está deshabilitado
                        if (!campoFinal.Enabled)
                        {
                            Console.WriteLine("? Campo deshabilitado - Validación correcta del sistema");
                            return true;
                        }
                    }
                }

                // Ingresar la fecha
                campoFinal.Clear();
                campoFinal.SendKeys(fecha);
                campoFinal.SendKeys(Keys.Tab);

                // Esperar a que el campo procese
                Thread.Sleep(800);

                // Verificar si fue auto-corregida
                string valorFinal = campoFinal.GetAttribute("value");

                if (valorFinal != fecha && !string.IsNullOrEmpty(valorFinal))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"?? FECHA FINAL AUTO-CORREGIDA:");
                    Console.WriteLine($"   • Fecha ingresada: {fecha}");
                    Console.WriteLine($"   • Fecha corregida a: {valorFinal}");
                    Console.ResetColor();

                    return true; // Marcar como flujo inválido
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"? Fecha final aceptada: {fecha}");
                Console.ResetColor();

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Error al ingresar fecha final: {ex.Message}");
                return true;
            }
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

        public bool IngresarRucAutorizado(string ruc)
        {
            if (ruc == "0") return false;

            try
            {
                var campo = wait.Until(ExpectedConditions.ElementToBeClickable(RucAutorizado));
                campo.Clear();
                campo.SendKeys(ruc);
                campo.SendKeys(Keys.Enter);
                Thread.Sleep(1000);

                // ? Guardar el RUC para validación posterior
                rucAutorizadoGuardado = ruc;
                Console.WriteLine($"? RUC del autorizado ingresado: {ruc}");
                return false;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"? Error al ingresar RUC autorizado: {ex.Message}");
                Console.ResetColor();
                return true;
            }
        }

        public bool IngresarRucDni(string ruc)
        {
            if (ruc == "0")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"?? RUC del {(tipoRegistroActual == "Egreso" ? "beneficiario" : "pagador")} marcado como '0' - Campo quedará VACÍO");
                Console.WriteLine("   • Este es un caso de prueba negativo");
                Console.WriteLine("   • Se espera que el sistema valide campo obligatorio");
                Console.ResetColor();
                return true; // flujo inválido intencional
            }

            // ? VALIDACIÓN PREVENTIVA: RUC duplicado (solo informativa, NO detiene el flujo)
            if (!string.IsNullOrEmpty(rucAutorizadoGuardado) && ruc == rucAutorizadoGuardado)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("?? VALIDACIÓN LÓGICA DETECTADA:");
                Console.WriteLine($"   RUC Autorizado: {rucAutorizadoGuardado}");
                Console.WriteLine($"   RUC {(tipoRegistroActual == "Egreso" ? "Beneficiario" : "Pagador")}: {ruc}");
                Console.WriteLine("   ? Ambos RUCs son IGUALES (inconsistencia lógica)");
                Console.WriteLine("   ? Continuando para probar validación del sistema...");
                Console.ResetColor();
                // ?? NO retornamos aquí - dejamos que el sistema valide
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

                Console.WriteLine($"? RUC del {(tipoRegistroActual == "Egreso" ? "beneficiario" : "pagador")} ingresado: {ruc}");
                return false; // flujo válido - continúa normalmente
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"? Error al ingresar RUC/DNI: {ex.Message}");
                Console.ResetColor();
                return true; // error real
            }
        }


        public bool SeleccionarDocumento(string documento)
        {
            if (documento == "0") return true; // Se considera exitoso si no hay selección

            try
            {
                // 1. Clic en la flecha para desplegar el Select2
                var inputSelect2Arrow = wait.Until(ExpectedConditions.ElementToBeClickable(Select2DocumentoArrow));
                inputSelect2Arrow.Click();

                Thread.Sleep(500);

                // 2. Definir el localizador para la opción
                var opcionLocator = By.XPath($"//ul[contains(@id, 'select2') and contains(@id, 'results')]/li[normalize-space(text())='{documento}']");

                // 3. Esperar a que la opción sea clickeable. Si no está, lanza TimeoutException.
                var opcion = wait.Until(ExpectedConditions.ElementToBeClickable(opcionLocator));
                opcion.Click();
                Console.WriteLine($"Documento seleccionado exitosamente: '{documento}'");
                return true; // Éxito en la selección

            }
            catch (WebDriverTimeoutException)
            {
                // 4. Capturar la excepción de tiempo de espera (la opción no existe/cargó)
                Console.WriteLine($"ADVERTENCIA: No se pudo seleccionar el documento '{documento}'. La opción no está disponible o el Select2 falló en cargar.");
                return false; // Fallo en la selección
            }
            catch (Exception ex)
            {
                // 5. Capturar otros posibles errores, como que la flecha no sea clickeable
                Console.WriteLine($"ERROR INESPERADO al seleccionar documento: {ex.Message}");
                return false;
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

        // En IngresoEgresosPage.cs
        public bool HacerClicEnGuardar()
        {
            try
            {
                var btn = wait.Until(ExpectedConditions.ElementToBeClickable(BtnGuardar));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btn);
                btn.Click();
                Console.WriteLine(">>> Registro guardado (clic ejecutado)");
                return true; // ? Retorna true si tuvo éxito
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine(">>> Botón Guardar no disponible (posible inconsistencia).");
                return false; // ? Retorna false si falló
            }
            catch (Exception ex)
            {
                Console.WriteLine($">>> Error al hacer clic en Guardar: {ex.Message}");
                return false; // ? Retorna false para cualquier otro error
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
                Thread.Sleep(2000);
                bool tieneInconsistencia = false;

                // 1. Buscar mensaje de INCONSISTENCIA(S)
                try
                {
                    var mensajeInconsistencia = driver.FindElement(
                        By.XPath("//*[contains(text(),'INCONSISTENCIA') or contains(text(),'Es necesario') or contains(text(),'obligatorio') or contains(text(),'requerido')]"));

                    if (mensajeInconsistencia.Displayed)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"? Mensaje de inconsistencia detectado: {mensajeInconsistencia.Text}");
                        Console.ResetColor();
                        tieneInconsistencia = true;
                    }
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("No se encontró mensaje de INCONSISTENCIA(S)");
                }

                // 2. Buscar modal de error "Ocurrió un Problema"
                try
                {
                    var modalError = driver.FindElement(
                        By.XPath("//*[contains(text(),'Ocurrió un Problema') or contains(text(),'Error') or contains(text(),'No se ha podido')]"));

                    if (modalError.Displayed)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"? Modal de error detectado: {modalError.Text}");
                        Console.ResetColor();
                        tieneInconsistencia = true;
                    }
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("No se encontró modal 'Ocurrió un Problema'");
                }

                // 3. Buscar elementos de error por clase CSS
                try
                {
                    var elementosError = driver.FindElements(By.CssSelector(
                        ".text-danger, .error, .alert-danger, .invalid-feedback, .has-error, [style*='color: red']"));

                    foreach (var elem in elementosError.Where(e => e.Displayed && !string.IsNullOrWhiteSpace(e.Text)))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"? Elemento de error encontrado: {elem.Text}");
                        Console.ResetColor();
                        tieneInconsistencia = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error buscando elementos CSS: {ex.Message}");
                }

                // 4. Verificar si hay campos con borde rojo (validación HTML5)
                try
                {
                    var camposInvalidos = driver.FindElements(By.CssSelector("input:invalid, select:invalid, textarea:invalid"));

                    if (camposInvalidos.Any(c => c.Displayed))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"? Campos inválidos detectados (validación HTML5): {camposInvalidos.Count}");
                        Console.ResetColor();
                        tieneInconsistencia = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error verificando campos HTML5: {ex.Message}");
                }

                // 5. Verificar si el botón GUARDAR está deshabilitado
                try
                {
                    var botonGuardar = driver.FindElement(By.XPath("//button[contains(text(),'GUARDAR')]"));
                    bool estaDeshabilitado = !botonGuardar.Enabled ||
                                            botonGuardar.GetAttribute("disabled") != null ||
                                            botonGuardar.GetAttribute("class").Contains("disabled");

                    if (estaDeshabilitado)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("? Botón GUARDAR está deshabilitado (validación preventiva)");
                        Console.ResetColor();
                        tieneInconsistencia = true;
                    }
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("Botón GUARDAR no encontrado");
                }

                // 6. NUEVO: Verificar si el modal sigue abierto (timeout del backend)
                try
                {
                    var modalAbierto = driver.FindElement(
                        By.XPath("//div[@id='modal-registro-ingreso-egreso-varios' and contains(@style,'display: block')]"));

                    if (modalAbierto.Displayed)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("?? TIMEOUT DETECTADO:");
                        Console.WriteLine("   • Modal aún abierto después de intentar guardar");
                        Console.WriteLine("   • Backend no respondió o validó sin enviar feedback");
                        Console.WriteLine("   • Usuario no recibe confirmación ni error");
                        Console.WriteLine("   • BUG DE UX: Falta comunicación backend ? frontend");
                        Console.ResetColor();

                        tieneInconsistencia = true;
                    }
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("Modal cerrado correctamente");
                }

                return tieneInconsistencia;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error en ExisteInconsistencia: {ex.Message}");
                Console.ResetColor();
                return false;
            }
        }

        // Método para cerrar el modal de error si aparece
        public void CerrarModalError()
        {
            try
            {
                var botonOk = wait.Until(driver =>
                    driver.FindElement(By.XPath("//button[contains(text(),'OK')]")));

                if (botonOk.Displayed)
                {
                    botonOk.Click();
                    Console.WriteLine("Modal de error cerrado");
                    Thread.Sleep(500);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("No hay modal de error para cerrar");
            }
        }

    }
}
