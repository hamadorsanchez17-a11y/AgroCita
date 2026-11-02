
import { Router } from "express";

import { VisitController } from "../controllers/VisitController.js";

import { ScheduleVisit } from "../../application/use-cases/ScheduleVisit.js";
import { GetVisitsByTechnician } from "../../application/use-cases/GetVisitsByTechnician.js";
import { GetVisitsByCompany } from "../../application/use-cases/GetVisitsByCompany.js";
import { UpdateVisit } from "../../application/use-cases/UpdateVisit.js";

import { MockVisitRepository } from "../../infrastructure/repositories/MockVisitRepository.js";

import { assistanceRequestRepository } from "../../../assistance-requests/infrastructure/repositories/assistanceRequestRepositoryInstance.js";

import { authenticateToken } from "../../../../shared/middlewares/auth.middleware.js";

const visitRouter = Router();

const visitRepository = new MockVisitRepository();

const scheduleVisit = new ScheduleVisit(
  visitRepository,
  assistanceRequestRepository
);

const getVisitsByTechnician = new GetVisitsByTechnician(
  visitRepository
);

const getVisitsByCompany = new GetVisitsByCompany(
  visitRepository
);

const updateVisit = new UpdateVisit(
  visitRepository
);

const visitController = new VisitController(
  scheduleVisit,
  getVisitsByTechnician,
  getVisitsByCompany,
  updateVisit
);

// Programar una visita
visitRouter.post(
  "/",
  authenticateToken,
  visitController.schedule
);

// Consultar visitas del técnico
visitRouter.get(
  "/my-visits",
  authenticateToken,
  visitController.getMyVisits
);

// Consultar visitas de la empresa
visitRouter.get(
  "/",
  authenticateToken,
  visitController.getAllByCompany
);

// Actualizar o reprogramar una visita
visitRouter.patch(
  "/:id",
  authenticateToken,
  visitController.update
);

export default visitRouter;
