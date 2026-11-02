
import test, { before } from "node:test";
import assert from "node:assert/strict";
import request from "supertest";

import app from "../../src/app.js";

//Prueba de integración HTTP para los productores
/*
Autenticación y obtención del token al iniciar sesión
Consultar el perfil del productor siempre y cuando esté registrado
Creación y consulta de fincas
Actualización de los datos de las fincas
Restricciones de acceso entre productores
*/ 

let accessToken: string;

before(async () => {
  // Registrar un productor para obtener credenciales válidas.
  const registerResponse = await request(app)
    .post("/api/auth/register")
    .send({
      nombreCompleto: "Carlos Integración",
      email: "carlos.integracion@example.com",
      password: "Password123",
      codigoInvitacion: "AGROTEST0001",
      dni: "0801199012345",
      telefono: "99998888",
      direccion: "Tegucigalpa"
    });

  assert.equal(registerResponse.status, 201);

  // Iniciar sesión para obtener el JWT.
  const loginResponse = await request(app)
    .post("/api/auth/login")
    .send({
      email: "carlos.integracion@example.com",
      password: "Password123"
    });

  assert.equal(loginResponse.status, 200);

  accessToken = loginResponse.body.data.accessToken;
});

test("Debe comprobar que la API está funcionando", async () => {
  const response = await request(app).get("/api/health");

  assert.equal(response.status, 200);
  assert.equal(response.body.success, true);
});

test("Debe rechazar la consulta del perfil sin autenticación", async () => {
  const response = await request(app)
    .get("/api/producers/me");

  assert.equal(response.status, 401);
});

test("Debe obtener el perfil del productor autenticado", async () => {
  const response = await request(app)
    .get("/api/producers/me")
    .set("Authorization", `Bearer ${accessToken}`);

  assert.equal(response.status, 200);
  assert.equal(response.body.success, true);
  assert.ok(response.body.data);
  assert.equal(response.body.data.empresaId, 1);
});
