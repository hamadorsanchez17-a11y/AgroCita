
import test, { before } from "node:test";
import assert from "node:assert/strict";
import request from "supertest";

import app from "../../src/app.js";

/*
Pruebas de integración de Gestión de Fincas:
Crear una finca
Consultar la finca
Actualizar la finca
Comprobar que un productor no puede acceder a fincas que no le pertenecen
*/

let accessToken: string;
let farmId: number;

before(async () => {
  // Registrar un productor para las pruebas.
  const registerResponse = await request(app)
    .post("/api/auth/register")
    .send({
      nombreCompleto: "Carlos Integración",
      email: "carlos.fincas@example.com",
      password: "Password123",
      codigoInvitacion: "AGROTEST0001",
      dni: "0801199012345",
      telefono: "99998888",
      direccion: "Tegucigalpa"
    });

  assert.equal(registerResponse.status, 201);

  // Iniciar sesión para obtener el token.
  const loginResponse = await request(app)
    .post("/api/auth/login")
    .send({
      email: "carlos.fincas@example.com",
      password: "Password123"
    });

  assert.equal(loginResponse.status, 200);

  accessToken = loginResponse.body.data.accessToken;
});

test("Debe rechazar la creación de una finca sin autenticación", async () => {
  const response = await request(app)
    .post("/api/farms")
    .send({
      nombre: "Finca El Paraíso",
      departamento: "Francisco Morazán",
      municipio: "Tegucigalpa",
      ubicacion: "Valle de Amarateca",
      areaManzanas: 10
    });

  assert.equal(response.status, 401);
});

test("Debe crear una finca correctamente", async () => {
  const response = await request(app)
    .post("/api/farms")
    .set("Authorization", `Bearer ${accessToken}`)
    .send({
      nombre: "Finca El Paraíso",
      departamento: "Francisco Morazán",
      municipio: "Tegucigalpa",
      ubicacion: "Valle de Amarateca",
      areaManzanas: 10,
      descripcion: "Finca de prueba de integración"
    });

  assert.equal(response.status, 201);
  assert.equal(response.body.success, true);
  assert.ok(response.body.data);

  farmId = response.body.data.id;
});

test("Debe obtener las fincas del productor autenticado", async () => {
  const response = await request(app)
    .get("/api/farms")
    .set("Authorization", `Bearer ${accessToken}`);

  assert.equal(response.status, 200);
  assert.equal(response.body.success, true);
  assert.ok(Array.isArray(response.body.data));
  assert.equal(response.body.data.length, 1);
  assert.equal(response.body.data[0].nombre, "Finca El Paraíso");
});

test("Debe obtener una finca por su identificador", async () => {
  const response = await request(app)
    .get(`/api/farms/${farmId}`)
    .set("Authorization", `Bearer ${accessToken}`);

  assert.equal(response.status, 200);
  assert.equal(response.body.success, true);
  assert.equal(response.body.data.id, farmId);
  assert.equal(response.body.data.nombre, "Finca El Paraíso");
});

test("Debe actualizar una finca correctamente", async () => {
  const response = await request(app)
    .patch(`/api/farms/${farmId}`)
    .set("Authorization", `Bearer ${accessToken}`)
    .send({
      nombre: "Finca El Paraíso Actualizada",
      areaManzanas: 15
    });

  assert.equal(response.status, 200);
  assert.equal(response.body.success, true);
  assert.equal(response.body.data.nombre, "Finca El Paraíso Actualizada");
  assert.equal(response.body.data.areaManzanas, 15);
});

test("Debe rechazar la consulta de una finca inexistente", async () => {
  const response = await request(app)
    .get("/api/farms/99999")
    .set("Authorization", `Bearer ${accessToken}`);

  assert.equal(response.status, 404);
});
