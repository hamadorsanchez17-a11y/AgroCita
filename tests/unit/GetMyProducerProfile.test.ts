
import test from "node:test";
import assert from "node:assert/strict";

import { GetMyProducerProfile } from "../../src/modules/producers/application/use-cases/GetMyProducerProfile.js";
import { MockProducerRepository } from "../../src/modules/producers/infrastructure/repositories/MockProducerRepository.js";

test("Debe obtener correctamente el perfil del productor", async () => {
  const repository = new MockProducerRepository();
  const getProfile = new GetMyProducerProfile(repository);

  const result = await getProfile.execute(1, 1);

  assert.equal(result.id, 1);
  assert.equal(result.usuarioId, 1);
  assert.equal(result.empresaId, 1);
  assert.equal(result.nombreCompleto, "Juan Pérez");
  assert.equal(result.email, "juan@example.com");
});

test("Debe rechazar un usuario que no pertenece a una empresa", async () => {
  const repository = new MockProducerRepository();
  const getProfile = new GetMyProducerProfile(repository);

  await assert.rejects(
    getProfile.execute(1, null),
    /usuario no pertenece a una empresa/
  );
});

test("Debe rechazar un usuario sin perfil de productor", async () => {
  const repository = new MockProducerRepository();
  const getProfile = new GetMyProducerProfile(repository);

  await assert.rejects(
    getProfile.execute(999, 1),
    /No se encontró el perfil del productor/
  );
});

test("No debe permitir consultar el perfil de otra empresa", async () => {
  const repository = new MockProducerRepository();
  const getProfile = new GetMyProducerProfile(repository);

  await assert.rejects(
    getProfile.execute(1, 2),
    /No se encontró el perfil del productor/
  );
});
