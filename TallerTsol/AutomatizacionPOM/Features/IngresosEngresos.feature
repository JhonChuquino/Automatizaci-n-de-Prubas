Feature: Ingresos y Egresos

  Background:
    Given el usuario accede al sistema de Ingresos/Egresos 'http://161.132.67.82:31097/'
    When el usuario inicia sesion con usuario 'admin@plazafer.com' y contrasena 'calidad'
    And accede al modulo 'Tesoreria y Finanzas'
    And accede al submodulo 'Ingresos/Egresos'

  @RegistrarIngresoEgreso
  Scenario Outline: Registro de Ingresos y Egresos
    When el usuario selecciona la fecha inicial '<fecha_inicial>'
    And el usuario selecciona la fecha final '<fecha_final>'
    And el usuario selecciona el tipo de operacion '<tipo_operacion>'
    And el usuario define el tipo de registro '<tipo_registro>'
    And el usuario selecciona el autorizado '<autorizado>'
    And el usuario ingresa el RUC del autorizado '<ruc_autorizado>'
    And el usuario ingresa el RUC del pagador '<ruc_pagador>'
    And el usuario selecciona el documento '<documento>'
    And el usuario define el importe '<importe>'
    And el usuario ingresa la observacion '<observacion>'
    And el usuario guarda el registro
    Then <resultado>

  Examples:
    | caso | fecha_inicial | fecha_final | tipo_operacion | tipo_registro | autorizado | ruc_autorizado | ruc_pagador | documento | importe | observacion | resultado |
    | 1 | 01/11/2025 | 01/11/2025 | Cobros | Ingreso | Empleado | 60587924  | 76184356 | NOTA DE INGRESO | 120 | Prueba automatizada | la operacion se realiza con exito |
    | 2 | 01/11/2025 | 01/11/2025 | Pagos | Egreso | Proveedor | 75521712 | 76184356 | NOTA DE EGRESO | 0 | Prueba automatizada | se muestra el mensaje de inconsistencia |
    | 3 | 01/11/2025 | 01/11/2025 | Pagos | Egreso | Proveedor | 75521712 | 76184356 | 0 | 100 | Prueba automatizada | se muestra el mensaje de inconsistencia |
    | 4 | 01/11/2025 | 01/11/2025 | Cobros | Ingreso | Empleado | 75521712 | 76184356 | NOTA DE INGRESO | 120 | Prueba automatizada | la operacion se realiza con exito |
    | 5 | 02/11/2025 | 02/11/2025 | Pagos | Egreso | Proveedor | 75521712 | 76184356 | NOTA DE EGRESO | 150 | Registro valido | la operacion se realiza con exito |
    | 6 | 04/11/2025 | 04/11/2025 | Pagos | Egreso | Proveedor | 76184356 | 75521712 | NOTA DE EGRESO | 250 | Datos completos | la operacion se realiza con exito |
    | 7 | 05/11/2025 | 05/11/2025 | Cobros | Ingreso | Empleado | 75521712 | 76184356 | NOTA DE INGRESO | 80 | Validacion correcta | la operacion se realiza con exito |
    | 8 | 06/11/2025 | 06/11/2025 | Pagos | Egreso | Proveedor | 75521712 | 76184356 | NOTA DE EGRESO | 200 | Flujo normal | la operacion se realiza con exito |
    | 9 | 08/11/2025 | 08/11/2025 | Pagos | Egreso | Proveedor | 75521712 | 76184356 | NOTA DE EGRESO | 300 | Registro exitoso | la operacion se realiza con exito |
    | 10 | 09/11/2025 | 09/11/2025 | Cobros | Ingreso | Empleado | 76184356 | 75521712 | NOTA DE INGRESO | 60 | Prueba flujo normal | la operacion se realiza con exito |
    | 11 | 10/11/2025 | 10/11/2025 | Pagos | Egreso | Proveedor | 75521712 | 76184356 | NOTA DE EGRESO | 500 | Verificacion correcta | la operacion se realiza con exito |
    | 12 | 01/11/2025 | 01/11/2025 | Pagos | Egreso | Proveedor | 75521712 | 76184356 | NOTA DE EGRESO | 0 | Sin importe | se muestra el mensaje de inconsistencia |
    | 13 | 01/11/2025 | 01/11/2025 | Pagos | Egreso | 0 | 0 | 76184356 | NOTA DE EGRESO | 100 | Falta autorizado | se muestra el mensaje de inconsistencia |
    | 14 | 01/11/2025 | 01/11/2025 | Pagos | Egreso | Proveedor | 75521712 | 0 | NOTA DE EGRESO | 100 | Falta pagador | se muestra el mensaje de inconsistencia |
    | 15 | 01/11/2025 | 01/11/2025 | Pagos | Egreso | Proveedor | 75521712 | 76184356 | 0 | 100 | Sin documento | se muestra el mensaje de inconsistencia |
    | 16 | 01/11/2025 | 02/11/2025 | 0 | Egreso | Proveedor | 75521712 | 76184356 | NOTA DE EGRESO | 100 | Tipo de operacion faltante | se muestra el mensaje de inconsistencia |
    | 17 | 01/11/2025 | 01/11/2025 | Cobros | 0 | Empleado | 75521712 | 76184356 | NOTA DE INGRESO | 100 | Sin tipo registro | se muestra el mensaje de inconsistencia |
    | 18 | 01/11/2025 | 01/11/2025 | Pagos | Egreso | Proveedor | 0 | 0 | 0 | 0 | Todo vacio | se muestra el mensaje de inconsistencia |
    | 19 | 01/11/2025 | 01/11/2025 | Cobros | Ingreso | 0 | 0 | 76184356 | 0 | 0 | Sin datos | se muestra el mensaje de inconsistencia |
    | 20 | 02/11/2025 | 01/11/2025 | Cobros | Ingreso | Empleado | 75521712 | 76184356 | NOTA DE INGRESO | 150 | Fecha inicial mayor | se muestra el mensaje de inconsistencia |
    | 21 | 01/11/2025 | 01/11/2025 | Pagos | Egreso | 0 | 0 | 0 | 0 | 0 | Caso extremo | se muestra el mensaje de inconsistencia |
    | 22 | 12/11/2025 | 12/11/2025 | Cobros | Ingreso | Cliente | 12345678 | 75521712 | NOTA DE INGRESO | 95 | Registro de cliente | la operacion se realiza con exito |
    | 23 | 13/11/2025 | 13/11/2025 | Pagos | Egreso | Cliente | 12345678 | 76184356 | NOTA DE EGRESO | 30 | Pago a cliente | la operacion se realiza con exito |
    | 24 | 14/11/2025 | 14/11/2025 | Cobros | Ingreso | Empleado | 75521712 | 0 | NOTA DE INGRESO | 45 | RUC Pagador faltante | se muestra el mensaje de inconsistencia |
    | 25 | 15/11/2025 | 15/11/2025 | Pagos | Egreso | Proveedor | 0 | 76184356 | NOTA DE EGRESO | 70 | RUC Autorizado faltante | se muestra el mensaje de inconsistencia |
    | 26 | 16/11/2025 | 16/11/2025 | Cobros | Egreso | Cliente | 12345678 | 75521712 | NOTA DE EGRESO | 10 | Tipo de registro incorrecto | se muestra el mensaje de inconsistencia |
    | 27 | 17/11/2025 | 17/11/2025 | Pagos | Ingreso | Proveedor | 75521712 | 76184356 | NOTA DE INGRESO | 15 | Tipo de registro incorrecto | se muestra el mensaje de inconsistencia |
    | 28 | 18/11/2025 | 18/11/2025 | Cobros | Ingreso | Cliente | 12345678 | 76184356 | NOTA DE INGRESO | 15000 | Importe grande | la operacion se realiza con exito |
    | 29 | 19/11/2025 | 19/11/2025 | Pagos | Egreso | Empleado | 75521712 | 76184356 | NOTA DE EGRESO | 1 | Importe mínimo | la operacion se realiza con exito |
    | 30 | 20/11/2025 | 20/11/2025 | Cobros | Ingreso | Cliente | 0 | 0 | NOTA DE INGRESO | 100 | Solo tipo y importe | se muestra el mensaje de inconsistencia |
    | 31 | 21/11/2025 | 21/11/2025 | Cobros | Ingreso | Empleado | 123 | 76184356 | NOTA DE INGRESO | 100 | RUC autorizado invalido | se muestra el mensaje de inconsistencia |
    | 32 | 22/11/2025 | 22/11/2025 | Pagos | Egreso | Proveedor | 75521712 | 999 | NOTA DE EGRESO | 100 | RUC pagador inválido | se muestra el mensaje de inconsistencia |
    | 33 | 23/11/2025 | 23/11/2025 | Cobros | Ingreso | Cliente | ABCDEFGH | 76184356 | NOTA DE INGRESO | 100 | RUC con letras | se muestra el mensaje de inconsistencia |
    | 34 | 24/11/2025 | 24/11/2025 | Pagos | Egreso | Proveedor | 75521712 | 76184356 | NOTA DE EGRESO | -100 | Importe negativo | se muestra el mensaje de inconsistencia |
    | 35 | 25/11/2025 | 25/11/2025 | Cobros | Ingreso | Empleado | 75521712 | 76184356 | NOTA DE INGRESO | 0.5 | Importe decimal | la operacion se realiza con exito |
    | 36 | 26/11/2025 | 26/11/2025 | Pagos | Egreso | Proveedor | 75521712 | 76184356 | NOTA DE EGRESO | 999999999 | Importe muy grande | se muestra el mensaje de inconsistencia |
    | 37 | 01/01/2025 | 01/01/2025 | Cobros | Ingreso | Empleado | 75521712 | 76184356 | NOTA DE INGRESO | 100 | Fecha inicio de año | la operacion se realiza con exito |
    | 38 | 31/12/2025 | 31/12/2025 | Pagos | Egreso | Proveedor | 75521712 | 76184356 | NOTA DE EGRESO | 100 | Fecha fin de año | la operacion se realiza con exito |
    | 39 | 29/02/2024 | 29/02/2024 | Cobros | Ingreso | Cliente | 12345678 | 76184356 | NOTA DE INGRESO | 100 | Año bisiesto | la operacion se realiza con exito |
    | 40 | 01/11/2024 | 01/11/2025 | Pagos | Egreso | Proveedor | 75521712 | 76184356 | NOTA DE EGRESO | 100 | Rango de un año | se muestra el mensaje de inconsistencia |
    | 41 | 27/11/2025 | 27/11/2025 | Cobros | Ingreso | Empleado | 75521712 | 76184356 | NOTA DE INGRESO | 100 | Documento FACTURA | la operacion se realiza con exito |
    | 42 | 28/11/2025 | 28/11/2025 | Pagos | Egreso | Proveedor | 75521712 | 76184356 | BOLETA | 100 | Documento BOLETA | la operacion se realiza con exito |
    | 43 | 29/11/2025 | 29/11/2025 | Cobros | Ingreso | Cliente | 12345678 | 76184356 | RECIBO | 100 | Documento RECIBO | la operacion se realiza con exito |
    | 44 | 0 | 0 | Cobros | Ingreso | Empleado | 75521712 | 76184356 | NOTA DE INGRESO | 100 | Sin fechas | se muestra el mensaje de inconsistencia |
    | 45 | 01/11/2025 | 0 | Pagos | Egreso | Proveedor | 75521712 | 76184356 | NOTA DE EGRESO | 100 | Sin fecha final | se muestra el mensaje de inconsistencia |
    | 46 | 0 | 01/11/2025 | Cobros | Ingreso | Cliente | 12345678 | 76184356 | NOTA DE INGRESO | 100 | Sin fecha inicial | se muestra el mensaje de inconsistencia |
    | 47 | 30/11/2025 | 30/11/2025 | Pagos | Egreso | Proveedor | 75521712 | 76184356 | NOTA DE EGRESO | 100  | Sin observación | la operacion se realiza con exito |
    | 48 | 01/12/2025 | 01/12/2025 | Cobros | Ingreso | Empleado | 75521712 | 76184356 | NOTA DE INGRESO | 100 | Observación muy larga | se muestra el mensaje de inconsistencia |
    | 49 | 02/12/2025 | 02/12/2025 | Pagos | Egreso | Proveedor | 75521712 | 75521712 | NOTA DE EGRESO | 100 | Mismo RUC autorizado y pagador | se muestra el mensaje de inconsistencia |
    | 50 | 03/12/2025 | 03/12/2025 | Pagos | Egreso | Empleado | 75521712 | 0 | NOTA DE EGRESO | 100 | Egreso sin beneficiario | se muestra el mensaje de inconsistencia |
    | 51 | 04/12/2025 | 04/12/2025 | Pagos | Egreso | Cliente | 0 | 76184356 | NOTA DE EGRESO | 100 | Egreso sin autorizado | se muestra el mensaje de inconsistencia |
    | 52 | 05/12/2025 | 05/12/2025 | Cobros | Ingreso | Empleado | 75521712 | 76184356 | NOTA DE INGRESO | 100 | Observación con <script>alert('XSS')</script> | la operacion se realiza con exito |
    | 53 | 06/12/2025 | 06/12/2025 | Pagos | Egreso | Proveedor | 75521712 | 76184356 | NOTA DE EGRESO | 100 | Observación con ' OR '1'='1 | la operacion se realiza con exito |
    | 54 | 07/12/2025 | 07/12/2025 | Cobros | Ingreso | Empleado | SPACE75521712SPACE | 76184356 | NOTA DE INGRESO | 100 | RUC con espacios | se muestra el mensaje de inconsistencia |
    | 55 | 08/12/2025 | 08/12/2025 | Pagos | Egreso | Proveedor | 75521712 | SPACE76184356SPACE | NOTA DE EGRESO | 100 | RUC pagador con espacios | se muestra el mensaje de inconsistencia |
    | 56 | 09/12/2025 | 09/12/2025 | Cobros | Ingreso | Cliente | 12345678 | 76184356 | NOTA DE INGRESO | 100.00 | Importe con decimales | la operacion se realiza con exito |
    | 57 | 10/12/2025 | 10/12/2025 | Pagos | Egreso | Proveedor | 75521712 | 76184356 | NOTA DE EGRESO | 100,50 | Importe con coma | se muestra el mensaje de inconsistencia |
    | 58 | 11/12/2025 | 11/12/2025 | Cobros | Ingreso | Empleado | 75521712 | 76184356 | NOTA DE INGRESO | ABC | Importe no numérico | se muestra el mensaje de inconsistencia |
    | 59 | 32/01/2025 | 32/01/2025 | Cobros | Ingreso | Empleado | 75521712 | 76184356 | NOTA DE INGRESO | 100 | Día inválido | se muestra el mensaje de inconsistencia |
    | 60 | 01/13/2025 | 01/13/2025 | Pagos | Egreso | Proveedor | 75521712 | 76184356 | NOTA DE EGRESO | 100 | Mes inválido | se muestra el mensaje de inconsistencia |
    | 61 | 29/02/2025 | 29/02/2025 | Cobros | Ingreso | Cliente | 12345678 | 76184356 | NOTA DE INGRESO | 100 | Fecha inexistente | se muestra el mensaje de inconsistencia |
    | 62 | 12/12/2025 | 12/12/2025 | Cobros | Ingreso | Empleado | 1234567 | 76184356 | NOTA DE INGRESO | 100 | RUC 7 digitos DNI | se muestra el mensaje de inconsistencia |
    | 63 | 13/12/2025 | 13/12/2025 | Pagos | Egreso | Proveedor | 75521712 | 12345678 | NOTA DE EGRESO | 100 | RUC 8 dígitos DNI | la operacion se realiza con exito |
    | 64 | 14/12/2025 | 14/12/2025 | Cobros | Ingreso | Cliente | 7552171299 | 76184356 | NOTA DE INGRESO | 100 | RUC 10 dígitos | se muestra el mensaje de inconsistencia |
    | 65 | 15/12/2025 | 15/12/2025 | Pagos | Egreso | Proveedor | 755217129999 | 76184356 | NOTA DE EGRESO | 100 | RUC 12 dígitos | se muestra el mensaje de inconsistencia |
    | 66 | 16/12/2025 | 16/12/2025 | Cobros | Ingreso | Empleado | 75521712 | 76184356 | NOTA DE INGRESO | 100 | Test de performance | la operacion se realiza con exito |
    | 67 | 17/12/2025 | 17/12/2025 | Cobros | Ingreso | Empleado | 75521712 | 76184356 | NOTA DE INGRESO | 0.01 | Importe mínimo decimal | la operacion se realiza con exito |
    | 68 | 18/12/2025 | 18/12/2025 | Pagos | Egreso | Proveedor | 75521712 | 76184356 | NOTA DE EGRESO | 50.99 | Importe con centavos | la operacion se realiza con exito |
    | 69 | 19/12/2025 | 19/12/2025 | Cobros | Ingreso | Cliente | 12345678 | 76184356 | NOTA DE INGRESO | 1000000 | Importe un millón | la operacion se realiza con exito |
    | 70 | 20/12/2025 | 20/12/2025 | Pagos | Egreso | Proveedor | 20123456789 | 76184356 | NOTA DE EGRESO | 100 | RUC 11 digitos valido | la operacion se realiza con exito |
    | 71 | 05/11/2025    | 06/11/2025  | Cobros         | Ingreso       | VARIOS       | 00000000       | 00000000     | COBRO VARIOS     | 100     | Cobro varios efectivo     | la operacion se realiza con exito |
    | 72 | 05/11/2025    | 06/11/2025  | Cobros         | Ingreso       | VARIOS       | 75521712       | 76184356     | COBRO VARIOS     | 100     | Cobro varios con RUC      | se muestra el mensaje de inconsistencia |
    | 73 | 05/11/2025    | 06/11/2025  | Cobros         | Ingreso       | Empleado     | 75521712       | 00000000     | COBRO VARIOS     | 100     | Empleado con VARIOS       | se muestra el mensaje de inconsistencia |
    | 74 | 05/11/2025    | 06/11/2025  | Pagos          | Egreso        | Proveedor    | 75521712       | 76184356     | NOTA DE EGRESO   | 100     | Medio GIROS               | la operacion se realiza con exito |
    | 75 | 05/11/2025    | 06/11/2025  | Cobros         | Ingreso       | Cliente      | 12345678       | 76184356     | FACTURA          | 1000    | Cliente con factura       | la operacion se realiza con exito |
    | 76 | 05/11/2025    | 06/11/2025  | Cobros         | Ingreso       | Cliente      | 12345678       | 76184356     | NOTA DE EGRESO   | 100     | Documento de egreso en ingreso | se muestra el mensaje de inconsistencia |
    | 77 | 05/11/2025    | 06/11/2025  | Pagos          | Egreso        | Cliente      | 12345678       | 76184356     | NOTA DE INGRESO  | 100     | Documento de ingreso en egreso | se muestra el mensaje de inconsistencia |
    | 78 | 01/11/2025    | 30/11/2025  | Cobros         | Ingreso       | Empleado     | 75521712       | 76184356     | NOTA DE INGRESO  | 100     | Rango 30 días             | la operacion se realiza con exito |
    | 79 | 01/01/2025    | 31/12/2025  | Pagos          | Egreso        | Proveedor    | 75521712       | 76184356     | NOTA DE EGRESO   | 100     | Rango 1 año               | se muestra el mensaje de inconsistencia |
    | 80 | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Empleado     | 75521712       | 75521712     | NOTA DE INGRESO  | 100     | Mismo RUC autorizado/pagador en ingreso | la operacion se realiza con exito |
    | 81 | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | Empleado     | 75521712       | 75521712     | NOTA DE EGRESO   | 100     | Mismo RUC en egreso       | se muestra el mensaje de inconsistencia |
    | 82 | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Cliente      | 00000000       | 76184356     | RECIBO           | 50      | Cliente con RUC 00000000  | se muestra el mensaje de inconsistencia |
    | 83 | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | Proveedor    | 75521712       | 76184356     | RECIBO           | 100     | Recibo en egreso          | se muestra el mensaje de inconsistencia |
    | 84 | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Empleado     | 75521712       | 76184356     | NOTA DE INGRESO  | 100     | Observación vacía         | la operacion se realiza con exito |
    | 85 | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Empleado     | 75521712       | 76184356     | NOTA DE INGRESO  | 100     | Observación 500 chars     | se muestra el mensaje de inconsistencia |
    | 86 | 05/11/2025    | 06/11/2025  | 0              | 0             | 0            | 0              | 0            | 0                | 0       | Todos los campos vacíos   | se muestra el mensaje de inconsistencia |
    | 87 | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Empleado     | 75521712       | 76184356     | FACTURA          | 100     | Factura en ingreso        | se muestra el mensaje de inconsistencia |
    | 88 | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | Proveedor    | 75521712       | 76184356     | BOLETA           | 100     | Boleta en egreso          | la operacion se realiza con exito |
    | 89 | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Proveedor    | 20123456789    | 76184356     | NOTA DE INGRESO  | 100     | Proveedor como autorizado en ingreso | se muestra el mensaje de inconsistencia |
    | 90 | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | Cliente      | 12345678       | 76184356     | NOTA DE EGRESO   | 100     | Cliente como autorizado en egreso | se muestra el mensaje de inconsistencia |
    | 91   | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | VARIOS       | 00000000       | 00000000     | COBRO VARIOS     | 250     | Cobro varios sin RUC                     | la operacion se realiza con exito              |
    | 92   | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | VARIOS       | 75521712       | 00000000     | COBRO VARIOS     | 100     | VARIOS con RUC autorizado                | se muestra el mensaje de inconsistencia        |
    | 93   | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | VARIOS       | 00000000       | 00000000     | EGRESO VARIOS    | 180     | Egreso varios sin beneficiario           | la operacion se realiza con exito              |
    | 94   | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | VARIOS       | 00000000       | 76184356     | EGRESO VARIOS    | 100     | VARIOS con RUC pagador                   | se muestra el mensaje de inconsistencia        |
    | 95   | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Cliente      | 12345678       | 76184356     | RECIBO           | 75      | Recibo por cliente                       | la operacion se realiza con exito              |
    | 96   | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Proveedor    | 20123456789    | 76184356     | RECIBO           | 100     | Recibo por proveedor                     | se muestra el mensaje de inconsistencia        |
    | 97   | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | Proveedor    | 20123456789    | 75521712     | CHEQUE           | 500     | Egreso con cheque                        | la operacion se realiza con exito              |
    | 98   | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Empleado     | 75521712       | 76184356     | CHEQUE           | 100     | Cheque en ingreso                        | se muestra el mensaje de inconsistencia        |
    | 99   | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | Proveedor    | 75521712       | 76184356     | TRANSFERENCIA    | 300     | Egreso por transferencia                 | la operacion se realiza con exito              |
    | 100  | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Cliente      | 12345678       | 76184356     | TRANSFERENCIA    | 100     | Transferencia en ingreso                 | se muestra el mensaje de inconsistencia        |
    | 101  | 01/11/2025    | 15/11/2025  | Cobros         | Ingreso       | Empleado     | 75521712       | 76184356     | NOTA DE INGRESO  | 100     | Rango 15 días                            | la operacion se realiza con exito              |
    | 102  | 01/10/2025    | 30/11/2025  | Pagos          | Egreso        | Proveedor    | 75521712       | 76184356     | NOTA DE EGRESO   | 100     | Rango 61 días                            | se muestra el mensaje de inconsistencia        |
    | 103  | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Empleado     | 75521712       | 76184356     | NOTA DE INGRESO  | 999999.99 | Importe máximo permitido               | la operacion se realiza con exito              |
    | 104  | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | Proveedor    | 75521712       | 76184356     | NOTA DE EGRESO   | 1000000 | Importe excede límite                    | se muestra el mensaje de inconsistencia        |
    | 105  | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Empleado     | 75521712       | 76184356     | NOTA DE INGRESO  | 100     | Observación con 250 caracteres           | la operacion se realiza con exito              |
    | 106  | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Empleado     | 75521712       | 76184356     | NOTA DE INGRESO  | 100     | Observación con 501 caracteres           | se muestra el mensaje de inconsistencia        |
    | 107  | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Empleado     | 75521712       | 76184356     | NOTA DE INGRESO  | 100     | Observación con &amp;                    | la operacion se realiza con exito              |
    | 108  | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | Proveedor    | 75521712       | 76184356     | NOTA DE EGRESO   | 100     | Observación con &lt;                     | la operacion se realiza con exito              |
    | 109  | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Cliente      | 12345678       | 12345678     | NOTA DE INGRESO  | 100     | Cliente paga a sí mismo                  | se muestra el mensaje de inconsistencia        |
    | 110  | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | Cliente      | 12345678       | 12345678     | NOTA DE EGRESO   | 100     | Cliente recibe de sí mismo               | se muestra el mensaje de inconsistencia        |
    | 111  | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Empleado     | 75521712       | 75521712     | COBRO VARIOS     | 100     | Empleado con COBRO VARIOS                | se muestra el mensaje de inconsistencia        |
    | 112  | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | Proveedor    | 20123456789    | 20123456789  | EGRESO VARIOS    | 100     | Proveedor con EGRESO VARIOS              | se muestra el mensaje de inconsistencia        |
    | 113  | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | VARIOS       | 00000000       | 00000000     | RECIBO           | 100     | VARIOS con RECIBO                        | se muestra el mensaje de inconsistencia        |
    | 114  | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | VARIOS       | 00000000       | 00000000     | BOLETA           | 100     | VARIOS con BOLETA                        | se muestra el mensaje de inconsistencia        |
    | 115  | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Empleado     | 75521712       | 76184356     | FACTURA          | 1000    | Factura en ingreso (inválido)            | se muestra el mensaje de inconsistencia        |
    | 116  | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | Proveedor    | 75521712       | 76184356     | RECIBO           | 100     | Recibo en egreso (inválido)              | se muestra el mensaje de inconsistencia        |
    | 117  | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Cliente      | 12345678       | 76184356     | BOLETA           | 100     | Boleta en ingreso (inválido)             | se muestra el mensaje de inconsistencia        |
    | 118  | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | Empleado     | 75521712       | 76184356     | FACTURA          | 100     | Factura en egreso (inválido)             | se muestra el mensaje de inconsistencia        |
    | 119  | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Empleado     | 75521712       | 76184356     | NOTA DE INGRESO  | 0.001   | Importe 3 decimales                      | se muestra el mensaje de inconsistencia        |
    | 120  | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | Proveedor    | 75521712       | 76184356     | NOTA DE EGRESO   | 0.99    | Importe con 2 decimales                  | la operacion se realiza con exito              |
    | 121  | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Empleado     | 75521712       | 76184356     | NOTA DE INGRESO  | 100     | RUC autorizado con guion: 7552-1712      | se muestra el mensaje de inconsistencia        |
    | 122  | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | Proveedor    | 75521712       | 76184356     | NOTA DE EGRESO   | 100     | RUC pagador con punto: 7618.4356         | se muestra el mensaje de inconsistencia        |
    | 123  | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | VARIOS       | 00000000       | 00000000     | COBRO VARIOS     | 0       | Cobro varios con importe 0               | se muestra el mensaje de inconsistencia        |
    | 124  | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | VARIOS       | 00000000       | 00000000     | EGRESO VARIOS    | 0       | Egreso varios con importe 0              | se muestra el mensaje de inconsistencia        |
    | 125  | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Empleado     | 75521712       | 76184356     | NOTA DE INGRESO  | 100     | Documento duplicado en día               | se muestra el mensaje de inconsistencia        |
    | 126  | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | Proveedor    | 75521712       | 76184356     | NOTA DE EGRESO   | 100     | Nro documento ya usado                   | se muestra el mensaje de inconsistencia        |
    | 127  | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Empleado     | 75521712       | 76184356     | NOTA DE INGRESO  | 100     | Ingreso con hora 00:00                   | la operacion se realiza con exito              |
    | 128  | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | Proveedor    | 75521712       | 76184356     | NOTA DE EGRESO   | 100     | Egreso con hora 23:59                    | la operacion se realiza con exito              |
    | 129  | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Empleado     | 75521712       | 76184356     | NOTA DE INGRESO  | 100     | Registro desde botón INGRESO directo     | la operacion se realiza con exito              |
    | 130  | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | Proveedor    | 75521712       | 76184356     | NOTA DE EGRESO   | 100     | Registro desde botón EGRESO directo      | la operacion se realiza con exito              |
    | 131  | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Empleado     | 75521712       | 76184356     | NOTA DE INGRESO  | 100     | Cajero: ADMIN (válido)                   | la operacion se realiza con exito              |
    | 132  | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | Proveedor    | 75521712       | 76184356     | NOTA DE EGRESO   | 100     | Cajero vacío                             | se muestra el mensaje de inconsistencia        |
    | 133  | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Cliente      | 12345678       | 76184356     | NOTA DE INGRESO  | 100     | Cliente como pagador (inválido)          | se muestra el mensaje de inconsistencia        |
    | 134  | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | Empleado     | 75521712       | 76184356     | NOTA DE EGRESO   | 100     | Empleado como beneficiario (inválido)    | se muestra el mensaje de inconsistencia        |
    | 135  | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | VARIOS       | 00000000       | 00000000     | COBRO VARIOS     | 100     | Medio EFECTIVO (por defecto)             | la operacion se realiza con exito              |
    | 136  | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | Proveedor    | 75521712       | 76184356     | NOTA DE EGRESO   | 100     | Medio GIROS sin banco                    | se muestra el mensaje de inconsistencia        |
    | 137  | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Empleado     | 75521712       | 76184356     | NOTA DE INGRESO  | 100     | Registro con paginación activa           | la operacion se realiza con exito              |
    | 138  | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | Proveedor    | 75521712       | 76184356     | NOTA DE EGRESO   | 100     | Filtro activo no afecta registro         | la operacion se realiza con exito              |
    | 139  | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Empleado     | 75521712       | 76184356     | NOTA DE INGRESO  | 100     | Registro con 65 filas en tabla           | la operacion se realiza con exito              |
    | 140  | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | Proveedor    | 75521712       | 76184356     | NOTA DE EGRESO   | 100     | Registro tras limpiar filtros            | la operacion se realiza con exito              |
    | 141  | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Empleado     | 75521712       | 76184356     | NOTA DE INGRESO  | 100     | Ingreso con documento NIO2-9999          | la operacion se realiza con exito              |
    | 142  | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | Proveedor    | 75521712       | 76184356     | NOTA DE EGRESO   | 100     | Egreso con documento NIO2-9999           | se muestra el mensaje de inconsistencia        |
    | 143  | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | VARIOS       | 00000000       | 00000000     | COBRO VARIOS     | 100     | COBRO VARIOS + EFECTIVO                  | la operacion se realiza con exito              |
    | 144  | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | VARIOS       | 00000000       | 00000000     | EGRESO VARIOS    | 100     | EGRESO VARIOS + EFECTIVO                 | la operacion se realiza con exito              |
    | 145  | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Empleado     | 75521712       | 76184356     | NOTA DE INGRESO  | 100     | Observación con emoji                    | se muestra el mensaje de inconsistencia        |
    | 146  | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | Proveedor    | 75521712       | 76184356     | NOTA DE EGRESO   | 100     | Observación con salto de línea           | la operacion se realiza con exito              |
    | 147  | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Cliente      | 12345678       | 76184356     | RECIBO           | 100     | Cliente con RECIBO válido                | la operacion se realiza con exito              |
    | 148  | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | Proveedor    | 20123456789    | 75521712     | BOLETA           | 100     | Proveedor con BOLETA válida              | la operacion se realiza con exito              |
    | 149  | 05/11/2025    | 05/11/2025  | Cobros         | Ingreso       | Empleado     | 75521712       | 76184356     | NOTA DE INGRESO  | 100     | Registro con RUC autorizado = pagador    | la operacion se realiza con exito              |
    | 150  | 05/11/2025    | 05/11/2025  | Pagos          | Egreso        | Proveedor    | 75521712       | 75521712     | NOTA DE EGRESO   | 100     | Proveedor paga a sí mismo                | se muestra el mensaje de inconsistencia        |