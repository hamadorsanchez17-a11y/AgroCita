import { Router } from "express";

import { FarmController } from "../controllers/FarmController.js";

import { CreateFarm } from "../../application/use-cases/CreateFarm.js";
import { GetMyFarms } from "../../application/use-cases/GetMyFarms.js";

import { MockFarmRepository } from "../../infrastructure/repositories/MockFarmRepository.js";

import { GetMyProducerProfile } from "../../../producers/application/use-cases/GetMyProducerProfile.js";
import { MockProducerRepository } from "../../../producers/infrastructure/repositories/MockProducerRepository.js";
import { GetFarmById } from "../../application/use-cases/GetFarmById.js";
import { UpdateFarm } from "../../application/use-cases/UpdateFarm.js";

import { authenticateToken } from "../../../../shared/middlewares/auth.middleware.js";

const farmRouter = Router();

// Repositorios
const farmRepository = new MockFarmRepository();
const producerRepository = new MockProducerRepository();

// Casos de uso
const createFarm = new CreateFarm(farmRepository);
const getMyFarms = new GetMyFarms(farmRepository);
const getFarmById = new GetFarmById(farmRepository);
const updateFarm = new UpdateFarm(farmRepository);

const getMyProducerProfile = new GetMyProducerProfile(
  producerRepository
);

// Controlador
const farmController = new FarmController(
  createFarm,
  getMyProducerProfile,
  getMyFarms,
  getFarmById,
  updateFarm
);

// Rutas protegidas
farmRouter.post(
  "/",
  authenticateToken,
  farmController.create
);

farmRouter.get(
  "/",
  authenticateToken,
  farmController.getAll
);

farmRouter.get("/:farmId", authenticateToken, farmController.getById);
farmRouter.patch("/:farmId", authenticateToken, farmController.update);

export default farmRouter;