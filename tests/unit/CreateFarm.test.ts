
import test from "node:test";
import assert from "node:assert/strict";

import { CreateFarm } from "../../src/modules/farms/application/use-cases/CreateFarm.js";
import { MockFarmRepository } from "../../src/modules/farms/infrastructure/repositories/MockFarmRepository.js";

const createInput = (overrides = {}) => ({
  nombre: "Finca El Paraíso",
  departamento: "Francisco Morazán",
  municipio: "Valle de Ángeles",
  ubicacion: "Carretera hacia San Juancito",
  areaManzanas: 15.5,
  descripcion: "Finca dedicada al cultivo de café.",
  ...overrides
});

test("Debe registrar correctamente una finca", async () => {
  const repository = new MockFarmRepository();
  const createFarm = new CreateFarm(repository);

  const result = await createFarm.execute(1, createInput());

  assert.equal(result.id, 1);
  assert.equal(result.productorId, 1);
  assert.equal(result.nombre, "Finca El Paraíso");
  assert.equal(result.areaManzanas, 15.5);
});

test("Debe permitir registrar varias fincas para el mismo productor", async () => {
  const repository = new MockFarmRepository();
  const createFarm = new CreateFarm(repository);

  await createFarm.execute(1, createInput());

  const secondFarm = await createFarm.execute(
    1,
    createInput({
      nombre: "Finca Santa Rosa"
    })
  );

  assert.equal(secondFarm.id, 2);
  assert.equal(secondFarm.nombre, "Finca Santa Rosa");
});

test("Debe rechazar una finca con nombre duplicado", async () => {
  const repository = new MockFarmRepository();
  const createFarm = new CreateFarm(repository);

  await createFarm.execute(1, createInput());

  await assert.rejects(
    createFarm.execute(
      1,
      createInput({
        nombre: "finca el paraíso"
      })
    ),
    /Ya tienes registrada una finca con ese nombre/
  );
});

test("Debe permitir que otro productor registre una finca con el mismo nombre", async () => {
  const repository = new MockFarmRepository();
  const createFarm = new CreateFarm(repository);

  await createFarm.execute(1, createInput());

  const result = await createFarm.execute(2, createInput());

  assert.equal(result.productorId, 2);
  assert.equal(result.nombre, "Finca El Paraíso");
});
