import type { FarmRepository } from "../../domain/repositories/FarmRepository.js";
import type { CreateFarmDTO } from "../dto/CreateFarmDTO.js";

export class CreateFarm {
  constructor(
    private readonly farmRepository: FarmRepository
  ) {}

  async execute(
    productorId: number,
    input: CreateFarmDTO
  ) {

    // Comprobar si el productor ya tiene una finca con ese nombre.
    const farms =
      await this.farmRepository.findAllByProducerId(productorId);

    const farmExists = farms.some(
      farm =>
        farm.nombre.toLowerCase() === input.nombre.toLowerCase()
    );

    if (farmExists) {
      throw new Error(
        "Ya tienes registrada una finca con ese nombre."
      );
    }

    // Preparar los datos de la finca.
    const farmData = {
      productorId,
      ...input
    };

    // Registrar la finca.
    return await this.farmRepository.create(farmData);
  }
}