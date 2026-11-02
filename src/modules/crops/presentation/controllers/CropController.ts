import type { Request, Response } from "express";

import type { AuthenticatedRequest } from "../../../../shared/middlewares/auth.middleware.js";

import { CreateCrop } from "../../application/use-cases/CreateCrop.js";
import { GetCropsByFarm } from "../../application/use-cases/GetCropsByFarm.js";

import { GetMyProducerProfile } from "../../../producers/application/use-cases/GetMyProducerProfile.js";
import { GetFarmById } from "../../../farms/application/use-cases/GetFarmById.js";

import { createCropSchema } from "../validators/CreateCropValidator.js";

export class CropController {
  constructor(
    private readonly createCrop: CreateCrop,
    private readonly getCropsByFarm: GetCropsByFarm,
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
        message: "No tienes permisos para registrar cultivos."
      });
      return;
    }

    const validation = createCropSchema.safeParse(req.body);

    if (!validation.success) {
      res.status(400).json({
        success: false,
        message: "Los datos del cultivo no son válidos.",
        errors: validation.error.issues
      });
      return;
    }

    try {
      const producerProfile = await this.getMyProducerProfile.execute(
        user.sub,
        user.empresaId
      );

      await this.getFarmById.execute(
        validation.data.fincaId,
        producerProfile.id
      );

      const crop = await this.createCrop.execute(
        validation.data.fincaId,
        validation.data.nombre
      );

      res.status(201).json({
        success: true,
        message: "Cultivo registrado correctamente.",
        data: crop
      });

    } catch (error) {
      const message = error instanceof Error
        ? error.message
        : "Ocurrió un error al registrar el cultivo.";

      const statusCode =
        message === "No se encontró la finca solicitada." ||
        message === "No se encontró el perfil del productor."
          ? 404
          : message === "El usuario no pertenece a una empresa."
            ? 403
            : 500;

      res.status(statusCode).json({
        success: false,
        message
      });
    }
  };

  getAllByFarm = async (
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
        message: "No tienes permisos para consultar estos cultivos."
      });
      return;
    }

    const rawFarmId = req.params.farmId;

    if (
      typeof rawFarmId !== "string" ||
      !/^[1-9]\d*$/.test(rawFarmId)
    ) {
      res.status(400).json({
        success: false,
        message: "El identificador de la finca no es válido."
      });
      return;
    }

    try {
      const producerProfile = await this.getMyProducerProfile.execute(
        user.sub,
        user.empresaId
      );

      await this.getFarmById.execute(
        Number(rawFarmId),
        producerProfile.id
      );

      const crops = await this.getCropsByFarm.execute(
        Number(rawFarmId)
      );

      res.status(200).json({
        success: true,
        message: "Cultivos obtenidos correctamente.",
        data: crops
      });

    } catch (error) {
      const message = error instanceof Error
        ? error.message
        : "Ocurrió un error al consultar los cultivos.";

      const statusCode =
        message === "No se encontró la finca solicitada." ||
        message === "No se encontró el perfil del productor."
          ? 404
          : message === "El usuario no pertenece a una empresa."
            ? 403
            : 500;

      res.status(statusCode).json({
        success: false,
        message
      });
    }
  };
}