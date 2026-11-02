import { Router } from "express";
import { CropController } from "../controllers/CropController.js";
import { CreateCrop } from "../../application/use-cases/CreateCrop.js";
import { GetCropsByFarm } from "../../application/use-cases/GetCropsByFarm.js";
import { cropRepository } from "../../infrastructure/repositories/cropRepositoryInstance.js";
import { GetMyProducerProfile } from "../../../producers/application/use-cases/GetMyProducerProfile.js";
import { MockProducerRepository } from "../../../producers/infrastructure/repositories/MockProducerRepository.js";
import { GetFarmById } from "../../../farms/application/use-cases/GetFarmById.js";
import { farmRepository } from "../../../farms/infrastructure/repositories/farmRepositoryInstance.js";
import { authenticateToken } from "../../../../shared/middlewares/auth.middleware.js";

const router = Router();


const producerRepository = new MockProducerRepository();

const createCrop = new CreateCrop(cropRepository);
const getCropsByFarm = new GetCropsByFarm(cropRepository);

const getMyProducerProfile = new GetMyProducerProfile(producerRepository);
const getFarmById = new GetFarmById(farmRepository);

const cropController = new CropController(
  createCrop,
  getCropsByFarm,
  getMyProducerProfile,
  getFarmById
);

router.post("/", authenticateToken, cropController.create);
router.get("/farms/:farmId", authenticateToken, cropController.getAllByFarm);

export default router;