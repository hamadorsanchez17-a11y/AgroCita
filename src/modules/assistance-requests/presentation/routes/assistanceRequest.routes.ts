
import { Router } from "express";

import { AssistanceRequestController } from "../controllers/AssistanceRequestController.js";

import { CreateAssistanceRequest } from "../../application/use-cases/CreateAssistanceRequest.js";
import { GetMyAssistanceRequests } from "../../application/use-cases/GetMyAssistanceRequests.js";

import { assistanceRequestRepository } from "../../infrastructure/repositories/assistanceRequestRepositoryInstance.js";

import { GetMyProducerProfile } from "../../../producers/application/use-cases/GetMyProducerProfile.js";
import { MockProducerRepository } from "../../../producers/infrastructure/repositories/MockProducerRepository.js";

import { GetFarmById } from "../../../farms/application/use-cases/GetFarmById.js";
import { farmRepository } from "../../../farms/infrastructure/repositories/farmRepositoryInstance.js";

import { cropRepository } from "../../../crops/infrastructure/repositories/cropRepositoryInstance.js";

import { authenticateToken } from "../../../../shared/middlewares/auth.middleware.js";

const assistanceRequestRouter = Router();

// Repositorios
const producerRepository = new MockProducerRepository();

// Casos de uso
const createAssistanceRequest = new CreateAssistanceRequest(
  assistanceRequestRepository,
  cropRepository
);

const getMyAssistanceRequests = new GetMyAssistanceRequests(
  assistanceRequestRepository
);

const getMyProducerProfile = new GetMyProducerProfile(
  producerRepository
);

const getFarmById = new GetFarmById(farmRepository);

// Controlador
const assistanceRequestController = new AssistanceRequestController(
  createAssistanceRequest,
  getMyAssistanceRequests,
  getMyProducerProfile,
  getFarmById
);

// Rutas protegidas
assistanceRequestRouter.post(
  "/",
  authenticateToken,
  assistanceRequestController.create
);

assistanceRequestRouter.get(
  "/my-requests",
  authenticateToken,
  assistanceRequestController.getMyRequests
);

export default assistanceRequestRouter;
