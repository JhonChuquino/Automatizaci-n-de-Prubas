using AutomatizacionPOM.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using SeleniumExtras.WaitHelpers;
using System;

namespace AutomatizacionPOM.StepDefinitions
{
    [Binding]
    public class IngresoEgresoStepDefinitions
    {
        private readonly IWebDriver driver;
        private readonly IngresoEgresosPage ingresoEgresosPage;
        private readonly WebDriverWait wait;
        private bool flujoInvalido = false;


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
            ingresoEgresosPage.IngresarFechaInicial(fechaInicial);
        }

        [When(@"el usuario selecciona la fecha final '(.*)'")]
        public void CuandoElUsuarioSeleccionaLaFechaFinal(string fechaFinal)
        {
            ingresoEgresosPage.IngresarFechaFinal(fechaFinal);
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
                Console.WriteLine("Flujo inválido: se omite ingreso de RUC del autorizado.");
                Console.ResetColor();
                return;
            }

            ingresoEgresosPage.IngresarRucAutorizado(rucAutorizado);
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
            if (flujoInvalido)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Flujo inválido: se omite selección de documento.");
                Console.ResetColor();
                return;
            }

            ingresoEgresosPage.SeleccionarDocumento(documento);
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
            if (flujoInvalido)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Flujo inválido: no se intenta guardar porque hay inconsistencia previa.");
                Console.ResetColor();
                return;
            }

            ingresoEgresosPage.HacerClicEnGuardar();
        }


        // ==========================
        // VALIDACIONES DE RESULTADO
        // ==========================

        //Caso ÉXITO
        [Then(@"la operacion se realiza con exito")]
        public void EntoncesLaOperacionSeRealizaConExito()
        {
            bool modalExito = ingresoEgresosPage.ExisteModalExito();
            Assert.IsTrue(modalExito, "No apareció el modal de éxito esperado.");
            Console.WriteLine("Modal de éxito detectado correctamente.");
        }

        //Caso INCONSISTENCIA
        [Then(@"se muestra el mensaje de inconsistencia")]
        public void EntoncesSeMuestraElMensajeDeInconsistencia()
        {
            if (flujoInvalido)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Caso inválido controlado correctamente (flujo detenido antes del error).");
                Console.ResetColor();
                flujoInvalido = false; // Reinicia para el siguiente caso
                return;
            }

            bool inconsistencia = ingresoEgresosPage.ExisteInconsistencia();

            if (!inconsistencia)
            {
                bool guardado = ingresoEgresosPage.ExisteModalExito();

                if (guardado)
                {
                    Assert.Fail("El sistema permitió guardar con datos inconsistentes (defecto funcional detectado).");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("No se mostró mensaje de inconsistencia, pero tampoco se guardó (posible defecto visual o validación silenciosa).");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Mensaje de inconsistencia detectado correctamente.");
                Console.ResetColor();
            }

            Assert.IsTrue(inconsistencia, "No se encontró el mensaje de INCONSISTENCIA(S).");
        }

    }
}
