Feature: NuevaCompra
  Como usuario del sistema
  Quiero registrar una nueva compra
  Para asegurar que las reglas obligatorias del formulario se cumplan correctamente

  //Background:
    Given el usuario ingresa al ambiente 'http://161.132.67.82:31097/'
    When el usuario inicia sesion con usuario 'admin@plazafer.com' y contrasena 'calidad'
    And accede al modulo 'Compra'
    And accede al submodulo 'Nueva Compra'

  @RegistrarNuevaCompra
  Scenario Outline: Registro de nueva compra
    When el usuario selecciona el proveedor "<proveedor>"
    And define la fecha de registro "<fecha>"
    And selecciona el tipo de documento "<documento>"
    And define la serie "<serie>"
    And define el numero de documento "<numero_doc>"
    And selecciona el tipo de entrega "<entrega>"
    And selecciona el tipo de pago "<tipo_pago>"
    And selecciona el tipo de compra "<tipo_compra>"
    And registra el producto "<producto>"
    And define la cantidad "<cantidad_producto>"
    And define el importe total "<importe_total>"
    And presiona el boton "Guardar Compra"
    Then <resultado>

    Examples:
      | caso | proveedor | fecha | documento | serie | numero_doc | entrega | tipo_pago | tipo_compra | producto | cantidad_producto | importe_total | resultado |
      | 1 | 0000000 | 31/10/2025 | Factura electronica | 001 | 00001234 | Inmediata | CO | Exoneradas IGV | 88008-1 | 2 | 20.00 | la compra se genera correctamente |
      | 2 | 0000001 | 03/10/2025 | Factura electronica | 002 | 00001234 | Diferida | CO | Exoneradas IGV | 88008-1 | 2 | 20.00 | la compra se genera correctamente |