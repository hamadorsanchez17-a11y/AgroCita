
import test from "node:test";
import assert from "node:assert/strict";

import { RegisterProducer } from "../../src/modules/auth/application/use-cases/RegisterProducer.js";
import { MockAuthRepository } from "../../src/modules/auth/infrastructure/repositories/MockAuthRepository.js";

const createInput = (overrides = {}) => ({
  nombreCompleto: "Juan Pérez",
  email: "juan@example.com",
  password: "Password123",
  codigoInvitacion: "AGROTEST0001",
  dni: "0801199012345",
  telefono: "99998888",
  direccion: "Tegucigalpa",
  ...overrides
});

test("Debe registrar correctamente a un productor", async () => {
  const repository = new MockAuthRepository();
  const registerProducer = new RegisterProducer(repository);

  const result = await registerProducer.execute(createInput());

  assert.deepEqual(result, {
    userId: 1,
    empresaId: 1
  });
});

test("Debe rechazar un código de invitación inexistente", async () => {
  const repository = new MockAuthRepository();
  const registerProducer = new RegisterProducer(repository);

  await assert.rejects(
    registerProducer.execute(
      createInput({ codigoInvitacion: "CODIGO999999" })
    ),
    /código de invitación no es válido/
  );
});

test("Debe rechazar el registro en una empresa inactiva", async () => {
  const repository = new MockAuthRepository();
  const registerProducer = new RegisterProducer(repository);

  await assert.rejects(
    registerProducer.execute(
      createInput({ codigoInvitacion: "AGROTEST0002" })
    ),
    /empresa está inactiva/
  );
});

test("Debe rechazar un correo electrónico duplicado", async () => {
  const repository = new MockAuthRepository();
  const registerProducer = new RegisterProducer(repository);

  await registerProducer.execute(createInput());

  await assert.rejects(
    registerProducer.execute(
      createInput({
        email: "juan@example.com",
        dni: "0801199012346"
      })
    ),
    /correo electrónico ya está registrado/
  );
});

test("Debe rechazar un DNI duplicado dentro de la misma empresa", async () => {
  const repository = new MockAuthRepository();
  const registerProducer = new RegisterProducer(repository);

  await registerProducer.execute(createInput());

  await assert.rejects(
    registerProducer.execute(
      createInput({
        email: "otro@example.com",
        dni: "0801199012345"
      })
    ),
    /productor con ese DNI/
  );
});
