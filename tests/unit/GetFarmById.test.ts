
import test from "node:test";
import assert from "node:assert/strict";

import { GetFarmById } from "../../src/modules/farms/application/use-cases/GetFarmById.js";
import { MockFarmRepository } from "../../src/modules/farms/infrastructure/repositories/MockFarmRepository.js";

test("Debe obtener correctamente una finca del productor", async () => {
  const repository = new MockFarmRepository();
  const getFarmById = new GetFarmById(repository);

  const createdFarm = await repository.create({
    productorId: 1,
    nombre: "Finca El Paraíso",
    departamento: "Francisco Morazán",
    municipio: "Valle de Ángeles",
    ubicacion: "Carretera hacia San Juancito",
    areaManzanas: 15.5,
    descripcion: "Finca dedicada al cultivo de café."
  });

  const result = await getFarmById.execute(createdFarm.id, 1);

  assert.equal(result.id, createdFarm.id);
  assert.equal(result.nombre, "Finca El Paraíso");
  assert.equal(result.productorId, 1);
});

test("Debe rechazar una finca inexistente", async () => {
  const repository = new MockFarmRepository();
  const getFarmById = new GetFarmById(repository);

  await assert.rejects(
    getFarmById.execute(999, 1),
    /No se encontró la finca solicitada/
  );
});

test("No debe permitir consultar una finca de otro productor", async () => {
  const repository = new MockFarmRepository();
  const getFarmById = new GetFarmById(repository);

  const farm = await repository.create({
    productorId: 2,
    nombre: "Finca Santa Rosa",
    departamento: "Francisco Morazán",
    municipio: "Valle de Ángeles",
    ubicacion: "Zona rural",
    areaManzanas: 10,
    descripcion: "Cultivo de café."
  });

  await assert.rejects(
    getFarmById.execute(farm.id, 1),
    /No se encontró la finca solicitada/
  );
});
