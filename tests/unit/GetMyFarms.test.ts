
import test from "node:test";
import assert from "node:assert/strict";

import { GetMyFarms } from "../../src/modules/farms/application/use-cases/GetMyFarms.js";
import { MockFarmRepository } from "../../src/modules/farms/infrastructure/repositories/MockFarmRepository.js";

const createFarmInput = (nombre: string) => ({
  nombre,
  departamento: "Francisco Morazán",
  municipio: "Valle de Ángeles",
  ubicacion: "Carretera hacia San Juancito",
  areaManzanas: 15.5,
  descripcion: "Finca dedicada al cultivo de café."
});

test("Debe obtener todas las fincas del productor", async () => {
  const repository = new MockFarmRepository();
  const getMyFarms = new GetMyFarms(repository);

  const createFarm = async (productorId: number, nombre: string) => {
    await repository.create({
      productorId,
      ...createFarmInput(nombre)
    });
  };

  await createFarm(1, "Finca El Paraíso");
  await createFarm(1, "Finca Santa Rosa");
  await createFarm(2, "Finca Los Pinos");

  const result = await getMyFarms.execute(1);

  assert.equal(result.length, 2);
  assert.deepEqual(
    result.map(farm => farm.nombre),
    ["Finca El Paraíso", "Finca Santa Rosa"]
  );
});

test("Debe devolver una lista vacía si el productor no tiene fincas", async () => {
  const repository = new MockFarmRepository();
  const getMyFarms = new GetMyFarms(repository);

  const result = await getMyFarms.execute(999);

  assert.deepEqual(result, []);
});

test("No debe devolver las fincas de otros productores", async () => {
  const repository = new MockFarmRepository();
  const getMyFarms = new GetMyFarms(repository);

  await repository.create({
    productorId: 1,
    ...createFarmInput("Finca El Paraíso")
  });

  await repository.create({
    productorId: 2,
    ...createFarmInput("Finca Santa Rosa")
  });

  const result = await getMyFarms.execute(1);

  assert.equal(result.length, 1);
  assert.equal(result[0]?.productorId, 1);
});
