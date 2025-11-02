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
    | 1 | 01/11/2025 | 01/11/2025 | Cobros | Ingreso | Empleado | 75521712 | 76184356 | NOTA DE INGRESO | 120 | Prueba automatizada | la operacion se realiza con exito |
    | 2 | 01/11/2025 | 01/11/2025 | Pagos | Egreso | Proveedor | 75521712 | 76184356 | NOTA DE EGRESO | 0 | Prueba automatizada | se muestra el mensaje de inconsistencia |
    | 3 | 01/11/2025 | 01/11/2025 | Pagos | Egreso | Proveedor | 75521712 | 76184356 | 0 | 100 | Prueba automatizada | se muestra el mensaje de inconsistencia |
    | 4 | 01/11/2025 | 01/11/2025 | Cobros | Ingreso | Empleado | 75521712 | 76184356 | NOTA DE INGRESO | 120 | Prueba automatizada | la operacion se realiza con exito |
    | 5 | 02/11/2025 | 02/11/2025 | Pagos | Egreso | Proveedor | 75521712 | 76184356 | NOTA DE EGRESO | 150 | Registro valido | la operacion se realiza con exito |
    | 6 | 04/11/2025 | 04/11/2025 | Pagos | Egreso | Proveedor | 76184356 | 75521712 | NOTA DE EGRESO | 250 | Datos completos | la operacion se realiza con exito |
    | 7 | 05/11/2025 | 05/11/2025 | Cobros | Ingreso | Empleado | 75521712 | 76184356 | NOTA DE INGRESO | 80 | Validacion correcta | la operacion se realiza con exito |
    | 8 | 06/11/2025 | 06/11/2025 | Pagos | Egreso | Proveedor | 75521712 | 76184356 | NOTA DE EGRESO | 200 | Flujo normal | la operacion se realiza con exito |
    | 9| 08/11/2025 | 08/11/2025 | Pagos | Egreso | Proveedor | 75521712 | 76184356 | NOTA DE EGRESO | 300 | Registro exitoso | la operacion se realiza con exito |
    | 10| 09/11/2025 | 09/11/2025 | Cobros | Ingreso | Empleado | 76184356 | 75521712 | NOTA DE INGRESO | 60 | Prueba flujo normal | la operacion se realiza con exito |
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
