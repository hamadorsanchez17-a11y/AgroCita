import { Router } from "express";

import { ProducerController } from "../controllers/ProducerController.js";

import { GetMyProducerProfile } from "../../application/use-cases/GetMyProducerProfile.js";

import { MockProducerRepository } from "../../infrastructure/repositories/MockProducerRepository.js";

import { authenticateToken } from "../../../../shared/middlewares/auth.middleware.js";

const producerRouter = Router();

// Instancias de las dependencias
const producerRepository = new MockProducerRepository();

const getMyProducerProfile = new GetMyProducerProfile(
  producerRepository
);

const producerController = new ProducerController(
  getMyProducerProfile
);

// Endpoint para consultar el perfil del productor
producerRouter.get(
  "/me",
  authenticateToken,
  producerController.getMyProfile
);

export default producerRouter;