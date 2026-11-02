
import type { Producer } from "../entities/Producer.js";

export interface ProducerRepository {
  findProfileByUserId(
    userId: number,
    empresaId: number
  ): Promise<Producer | null>;
}
