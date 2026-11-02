import type { Request, Response } from "express";
import type { AuthenticatedRequest } from "../../../../shared/middlewares/auth.middleware.js";

import { CreateAssistanceRequest } from "../../application/use-cases/CreateAssistanceRequest.js";
import { GetMyAssistanceRequests } from "../../application/use-cases/GetMyAssistanceRequests.js";
import { GetMyProducerProfile } from "../../../producers/application/use-cases/GetMyProducerProfile.js";
import { GetFarmById } from "../../../farms/application/use-cases/GetFarmById.js";

import { createAssistanceRequestSchema } from "../validators/CreateAssistanceRequestValidator.js";

export class AssistanceRequestController {
  constructor(
    private readonly createAssistanceRequest: CreateAssistanceRequest,
    private readonly getMyAssistanceRequests: GetMyAssistanceRequests,
    private readonly getMyProducerProfile: GetMyProducerProfile,
    private readonly getFarmById: GetFarmById
  ) {}

  create = async (req: Request, res: Response): Promise<void> => {
    const authenticatedRequest = req as AuthenticatedRequest;
    const user = authenticatedRequest.user;

    if (!user) {
      res.status(401).json({
        success: false,
        message: "Se requiere autenticación."
      });
      return;
    }

    if (user.rol !== "PRODUCTOR") {
      res.status(403).json({
        success: false,
        message: "Solo los productores pueden crear solicitudes."
      });
      return;
    }

    const validation = createAssistanceRequestSchema.safeParse(req.body);

    if (!validation.success) {
      res.status(400).json({
        success: false,
        message: "Los datos de la solicitud no son válidos.",
        errors: validation.error.issues
      });
      return;
    }

    try {
      const profile = await this.getMyProducerProfile.execute(
        user.sub,
        user.empresaId
      );

      await this.getFarmById.execute(
        validation.data.fincaId,
        profile.id
      );

      const request = await this.createAssistanceRequest.execute({
        empresaId: profile.empresaId,
        productorId: profile.id,
        fincaId: validation.data.fincaId,
        cultivoIds: validation.data.cultivoIds,
        fechaDeseada: validation.data.fechaDeseada,
        motivo: validation.data.motivo
      });

      res.status(201).json({
        success: true,
        message: "Solicitud de asistencia técnica registrada correctamente.",
        data: request
      });
    } catch (error) {
      const message = error instanceof Error
        ? error.message
        : "Ocurrió un error al registrar la solicitud.";

      const statusCode =
        message === "No se encontró la finca solicitada." ||
        message === "No se encontró el perfil del productor."
          ? 404
          : message === "El usuario no pertenece a una empresa."
            ? 403
            : 400;

      res.status(statusCode).json({
        success: false,
        message
      });
    }
  };

  getMyRequests = async (
    req: Request,
    res: Response
  ): Promise<void> => {
    const authenticatedRequest = req as AuthenticatedRequest;
    const user = authenticatedRequest.user;

    if (!user) {
      res.status(401).json({
        success: false,
        message: "Se requiere autenticación."
      });
      return;
    }

    if (user.rol !== "PRODUCTOR") {
      res.status(403).json({
        success: false,
        message: "No tienes permisos para consultar estas solicitudes."
      });
      return;
    }

    try {
      const profile = await this.getMyProducerProfile.execute(
        user.sub,
        user.empresaId
      );

      const requests = await this.getMyAssistanceRequests.execute(
        profile.id,
        profile.empresaId
      );

      res.status(200).json({
        success: true,
        message: "Solicitudes obtenidas correctamente.",
        data: requests
      });
    } catch (error) {
      const message = error instanceof Error
        ? error.message
        : "Ocurrió un error al consultar las solicitudes.";

      res.status(400).json({
        success: false,
        message
      });
    }
  };
}