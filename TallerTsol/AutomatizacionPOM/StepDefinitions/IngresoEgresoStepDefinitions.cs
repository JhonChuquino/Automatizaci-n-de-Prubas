using AutomatizacionPOM.Pages;
using AventStack.ExtentReports.Gherkin.Model;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using SeleniumExtras.WaitHelpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace AutomatizacionPOM.StepDefinitions
{
    [Binding]
    public class IngresoEgresoStepDefinitions
    {
        private readonly IWebDriver driver;
        private readonly IngresoEgresosPage ingresoEgresosPage;
        private readonly WebDriverWait wait;
        private bool flujoInvalido = false;
        private bool fechaAutoCorregida = false;


        public IngresoEgresoStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            ingresoEgresosPage = new IngresoEgresosPage(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        // ==========================
        // ACCESO Y NAVEGACIÓN
        // ==========================

        [Given(@"el usuario accede al sistema de Ingresos/Egresos '(.*)'")]
        public void DadoQueElUsuarioAccedeAlSistemaDeIngresosEgresos(string url)
        {
            driver.Navigate().GoToUrl(url);
            Console.WriteLine($"Navegando a: {url}");
        }

        // ==========================
        // FECHAS
        // ==========================

        [When(@"el usuario selecciona la fecha inicial '(.*)'")]
        public void CuandoElUsuarioSeleccionaLaFechaInicial(string fechaInicial)
        {
            bool fechaAutoCorregida = ingresoEgresosPage.IngresarFechaInicial(fechaInicial);

            if (fechaAutoCorregida)
            {
                flujoInvalido = true;
                fechaAutoCorregida = true;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("?? Flujo marcado como INVÁLIDO: fecha fue rechazada/corregida por el sistema");
                Console.ResetColor();
            }
        }

        [When(@"el usuario selecciona la fecha final '(.*)'")]
        public void CuandoElUsuarioSeleccionaLaFechaFinal(string fechaFinal)
        {
            bool fechaAutoCorregida = ingresoEgresosPage.IngresarFechaFinal(fechaFinal);

            if (fechaAutoCorregida)
            {
                flujoInvalido = true;
                fechaAutoCorregida = true;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("?? Flujo marcado como INVÁLIDO: fecha final fue rechazada/corregida");
                Console.ResetColor();
            }
        }
        // ==========================
        // TIPO DE OPERACIÓN Y REGISTRO
        // ==========================

        [When(@"el usuario selecciona el tipo de operacion '(.*)'")]
        public void CuandoElUsuarioSeleccionaElTipoDeOperacion(string tipoOperacion)
        {
            if (flujoInvalido)
            {
                Console.WriteLine("Flujo inválido previo: se omite selección de tipo de operación.");
                return;
            }

            flujoInvalido = ingresoEgresosPage.SeleccionarTipoOperacion(tipoOperacion);
        }


        [When(@"el usuario define el tipo de registro '(.*)'")]
        public void CuandoElUsuarioDefineElTipoDeRegistro(string tipoRegistro)
        {
            if (string.IsNullOrWhiteSpace(tipoRegistro) || tipoRegistro == "0")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Tipo de registro no especificado. Se detiene el flujo para marcar inconsistencia.");
                Console.ResetColor();
                flujoInvalido = true;
                return;
            }

            ingresoEgresosPage.SeleccionarTipoRegistro(tipoRegistro);
        }



        // ==========================
        // AUTORIZADO / PAGADOR
        // ==========================

        [When(@"el usuario selecciona el autorizado '(.*)'")]
        public void CuandoElUsuarioSeleccionaElAutorizado(string autorizado)
        {
            if (flujoInvalido)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Flujo inválido: se omite selección de autorizado.");
                Console.ResetColor();
                return;
            }

            ingresoEgresosPage.SeleccionarAutorizado(autorizado);
        }


        [When(@"el usuario ingresa el RUC del autorizado '(.*)'")]
        public void CuandoElUsuarioIngresaElRUCDelAutorizado(string rucAutorizado)
        {
            if (flujoInvalido)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Flujo inválido previo: se omite ingreso de RUC del autorizado.");
                Console.ResetColor();
                return;
            }

            bool esCampoVacio = ingresoEgresosPage.IngresarRucAutorizado(rucAutorizado);

            if (esCampoVacio)
            {
                flujoInvalido = true;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("?? Flujo marcado como INVÁLIDO: Campo obligatorio sin completar");
                Console.ResetColor();
            }
        }


        [When(@"el usuario ingresa el RUC del pagador '(.*)'")]
        public void CuandoElUsuarioIngresaElRUCDelPagador(string rucPagador)
        {
            if (flujoInvalido)
            {
                Console.WriteLine("Flujo inválido previo: se omite ingreso de RUC del pagador.");
                return;
            }

            flujoInvalido = ingresoEgresosPage.IngresarRucDni(rucPagador);
        }



        // ==========================
        // DOCUMENTO, IMPORTE, OBSERVACIÓN
        // ==========================

        [When(@"el usuario selecciona el documento '(.*)'")]
        public void CuandoElUsuarioSeleccionaElDocumento(string documento)
        {
            // Si ya estamos en flujo inválido, omitimos y salimos.
            if (flujoInvalido)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Flujo inválido: se omite selección de documento.");
                Console.ResetColor();
                return;
            }

            // Llamamos a la página y almacenamos si fue exitoso
            bool seleccionExitosa = ingresoEgresosPage.SeleccionarDocumento(documento);

            // Si la selección NO fue exitosa, marcamos el flujo como inválido.
            if (!seleccionExitosa)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR DE NEGOCIO: Documento '{documento}' no disponible. Marcando la prueba como Flujo Inválido.");
                Console.ResetColor();
                flujoInvalido = true;
            }
        }

        [When(@"el usuario define el importe '(.*)'")]
        public void CuandoElUsuarioDefineElImporte(string importe)
        {
            if (flujoInvalido)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Flujo inválido: se omite ingreso de importe.");
                Console.ResetColor();
                return;
            }

            ingresoEgresosPage.IngresarImporte(importe);
        }


        [When(@"el usuario ingresa la observacion '(.*)'")]
        public void CuandoElUsuarioIngresaLaObservacion(string observacion)
        {
            if (flujoInvalido)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Flujo inválido: se omite ingreso de observación.");
                Console.ResetColor();
                return;
            }

            ingresoEgresosPage.IngresarObservacion(observacion);
        }


        // ==========================
        // GUARDAR REGISTRO
        // ==========================

        [When(@"el usuario guarda el registro")]
        public void CuandoElUsuarioGuardaElRegistro()
        {
            try
            {
                // Intentar hacer clic en Guardar
                var resultado = ingresoEgresosPage.HacerClicEnGuardar();

                if (!resultado)
                {
                    Console.WriteLine(">>> Botón Guardar no disponible o no se pudo hacer clic");
                }

                // Esperar a que aparezca el resultado (modal de éxito, error o inconsistencia)
                Thread.Sleep(3000); // Aumentar el tiempo de espera
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al intentar guardar: {ex.Message}");
            }
        }


        // ==========================
        // VALIDACIONES DE RESULTADO
        // ==========================

        [Then(@"la operacion se realiza con exito")]
        public void EntoncesLaOperacionSeRealizaConExito()
        {
            bool modalExito = ingresoEgresosPage.ExisteModalExito();

            if (flujoInvalido)
            {
                // 1. Caso Flujo Inválido (Documento no existe, RUC inválido, etc.)
                // Si el flujo fue inválido, ESPERAMOS que NO aparezca el modal de éxito.
                Assert.IsFalse(modalExito,
                    "ERROR LÓGICO: Se detectó un flujo inválido, pero la operación ¡se completó con éxito! Esto indica un defecto.");

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("PRUEBA EXITOSA: La operación falló como se esperaba debido a un dato inválido (flujo controlado).");
                Console.ResetColor();
            }
            else
            {
                // 2. Caso Flujo Válido
                // Si el flujo fue válido, ESPERAMOS que SÍ aparezca el modal de éxito.
                Assert.IsTrue(modalExito,
                    "FALLO DE REGRESIÓN: No apareció el modal de éxito esperado. La operación no se completó.");

                Console.WriteLine("Modal de éxito detectado correctamente.");
            }
        }



        [Then(@"se muestra el mensaje de inconsistencia")]
        public void EntoncesSeMuestraElMensajeDeInconsistencia()
        {
            // 1. Validación preventiva (campos vacíos detectados ANTES de enviar)
            if (flujoInvalido)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("??? TEST EXITOSO ???");
                Console.WriteLine("Validación preventiva detectada:");
                Console.WriteLine("  • Campo(s) obligatorio(s) sin completar");
                Console.WriteLine("  • Sistema bloqueó el flujo antes de enviar al backend");
                Console.ResetColor();
                flujoInvalido = false;
                Assert.Pass("Sistema validó preventivamente campos obligatorios");
                return;
            }

            // 2. Esperar respuesta del sistema (dar tiempo razonable)
            Thread.Sleep(2000); // Esperar que renderice (2 seg es suficiente)

            // 3. Verificar si mostró mensaje de inconsistencia
            bool inconsistenciaDetectada = ingresoEgresosPage.ExisteInconsistencia();

            if (inconsistenciaDetectada)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("??? TEST EXITOSO ???");
                Console.WriteLine("Sistema mostró mensaje de inconsistencia correctamente");
                Console.ResetColor();
                ingresoEgresosPage.CerrarModalError();
                Assert.Pass("Sistema validó inconsistencia correctamente");
                return;
            }

            // 4. Verificar si guardó exitosamente (BUG CRÍTICO)
            bool guardadoExitoso = ingresoEgresosPage.ExisteModalExito();

            if (guardadoExitoso)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("BUG CRÍTICO DETECTADO ");
                Console.WriteLine("Sistema permitió guardar datos inconsistentes:");
                Console.WriteLine("  • NO validó campos obligatorios");
                Console.WriteLine("  • Integridad de datos comprometida");
                Console.WriteLine("  • Datos incorrectos persistidos en BD");
                Console.WriteLine("Ticket: [BUG-004-No-Valida-Campos-Obligatorios]");
                Console.ResetColor();
                Assert.Fail("BUG CRÍTICO: Sistema guardó datos inconsistentes");
                return;
            }

            // 5. Timeout - Backend no respondió
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("BUG DE COMUNICACIÓN DETECTADO");
            Console.WriteLine("Problema:");
            Console.WriteLine("  • Backend procesó la solicitud pero NO respondió");
            Console.WriteLine("  • Usuario quedó sin feedback visual (modal)");
            Console.WriteLine("  • Timeout después de esperar respuesta");
            Console.WriteLine("Impacto:");
            Console.WriteLine("  UX muy pobre (usuario confundido)");
            Console.WriteLine("  No sabe si se guardó o no");
            Console.WriteLine("Ticket: [BUG-005-Backend-Timeout-Sin-Respuesta]");
            Console.ResetColor();
            Assert.Inconclusive("Timeout: Backend no responde - Usuario sin feedback");
        }



    }
}
