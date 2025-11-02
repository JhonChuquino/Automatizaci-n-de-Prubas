Feature: LoginFeature

A short summary of the feature

@InicioSesion
Scenario: Inicio de sesion exitoso
    Given el usuario ingresa al ambiente 'http://161.132.67.82:31097/'
    When el usuario inicia sesion con usuario 'admin@plazafer.com' y contrasena 'calidad'
