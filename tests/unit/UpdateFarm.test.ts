
import test from "node:test";
import assert from "node:assert/strict";

import { UpdateFarm } from "../../src/modules/farms/application/use-cases/UpdateFarm.js";
import { MockFarmRepository } from "../../src/modules/farms/infrastructure/repositories/MockFarmRepository.js";

const createFarm = async (repository: MockFarmRepository, productorId: number, nombre: string) => {
  return repository.create({
    productorId,
    nombre,
    departamento: "Francisco Morazán",
    municipio: "Valle de Ángeles",
    ubicacion: "Carretera hacia San Juancito",
    areaManzanas: 15.5,
    descripcion: "Finca dedicada al cultivo de café."
  });
};

test("Debe actualizar correctamente los datos de una finca", async () => {
  const repository = new MockFarmRepository();
  const updateFarm = new UpdateFarm(repository);

  const farm = await createFarm(repository, 1, "Finca El Paraíso");

  const result = await updateFarm.execute(farm.id, 1, {
    nombre: "Finca El Paraíso Actualizada",
    areaManzanas: 20
  });

  assert.equal(result.nombre, "Finca El Paraíso Actualizada");
  assert.equal(result.areaManzanas, 20);
  assert.equal(result.departamento, "Francisco Morazán");
});

test("Debe rechazar la actualización de una finca inexistente", async () => {
  const repository = new MockFarmRepository();
  const updateFarm = new UpdateFarm(repository);

  await assert.rejects(
    updateFarm.execute(999, 1, { nombre: "Nueva finca" }),
    /No se encontró la finca solicitada/
  );
});

test("Debe rechazar un nombre que ya utiliza otra finca del mismo productor", async () => {
  const repository = new MockFarmRepository();
  const updateFarm = new UpdateFarm(repository);

  const firstFarm = await createFarm(repository, 1, "Finca El Paraíso");
  await createFarm(repository, 1, "Finca Santa Rosa");

  await assert.rejects(
    updateFarm.execute(firstFarm.id, 1, {
      nombre: "Finca Santa Rosa"
    }),
    /Ya tienes registrada otra finca con ese nombre/
  );
});

test("Debe permitir utilizar el nombre de una finca de otro productor", async () => {
  const repository = new MockFarmRepository();
  const updateFarm = new UpdateFarm(repository);

  const firstFarm = await createFarm(repository, 1, "Finca El Paraíso");
  await createFarm(repository, 2, "Finca Santa Rosa");

  const result = await updateFarm.execute(firstFarm.id, 1, {
    nombre: "Finca Santa Rosa"
  });

  assert.equal(result.nombre, "Finca Santa Rosa");
  assert.equal(result.productorId, 1);
});

test("No debe permitir actualizar una finca de otro productor", async () => {
  const repository = new MockFarmRepository();
  const updateFarm = new UpdateFarm(repository);

  const farm = await createFarm(repository, 2, "Finca Santa Rosa");

  await assert.rejects(
    updateFarm.execute(farm.id, 1, {
      nombre: "Finca Modificada"
    }),
    /No se encontró la finca solicitada/
  );
});
