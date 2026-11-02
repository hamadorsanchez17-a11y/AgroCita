import type { Request, Response } from "express";

import type { AuthenticatedRequest } from "../../../../shared/middlewares/auth.middleware.js";

import { CreateFarm } from "../../application/use-cases/CreateFarm.js";
import { GetMyFarms } from "../../application/use-cases/GetMyFarms.js";
import { GetMyProducerProfile } from "../../../producers/application/use-cases/GetMyProducerProfile.js";
import { GetFarmById } from "../../application/use-cases/GetFarmById.js";
import { UpdateFarm } from "../../application/use-cases/UpdateFarm.js";

import {
  createFarmSchema,
  updateFarmSchema
} from "../validators/farm.schemas.js";

export class FarmController {
  constructor(
    private readonly createFarm: CreateFarm,
    private readonly getMyProducerProfile: GetMyProducerProfile,
    private readonly getMyFarms: GetMyFarms,
    private readonly getFarmById: GetFarmById,
    private readonly updateFarm: UpdateFarm
  ) {}

  create = async (
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
        message: "No tienes permisos para registrar fincas."
      });
      return;
    }

    const validation = createFarmSchema.safeParse(req.body);

    if (!validation.success) {
      res.status(400).json({
        success: false,
        message: "Los datos de la finca no son válidos.",
        errors: validation.error.issues
      });
      return;
    }

    try {
      const producerProfile =
        await this.getMyProducerProfile.execute(
          user.sub,
          user.empresaId
        );

      const farm = await this.createFarm.execute(
        producerProfile.id,
        validation.data
      );

      res.status(201).json({
        success: true,
        message: "Finca registrada correctamente.",
        data: farm
      });

    } catch (error) {
      const message =
        error instanceof Error
          ? error.message
          : "Ocurrió un error al registrar la finca.";

      const statusCode =
        message === "Ya tienes registrada una finca con ese nombre."
          ? 409
          : message === "No se encontró el perfil del productor." ||
              message === "El usuario no pertenece a una empresa."
            ? 403
            : 500;

      res.status(statusCode).json({
        success: false,
        message
      });
    }
  };

  getAll = async (
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
        message: "No tienes permisos para consultar estas fincas."
      });
      return;
    }

    try {
      const producerProfile =
        await this.getMyProducerProfile.execute(
          user.sub,
          user.empresaId
        );

      const farms = await this.getMyFarms.execute(
        producerProfile.id
      );

      res.status(200).json({
        success: true,
        message: "Fincas obtenidas correctamente.",
        data: farms
      });

    } catch (error) {
      const message =
        error instanceof Error
          ? error.message
          : "Ocurrió un error al consultar las fincas.";

      const statusCode =
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

  getById = async (req: Request, res: Response): Promise<void> => {
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
        message: "No tienes permisos para consultar esta finca."
      });
      return;
    }

    const rawFarmId = req.params.farmId;

    if (typeof rawFarmId !== "string" || !/^[1-9]\d*$/.test(rawFarmId)) {
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

      const farm = await this.getFarmById.execute(
        Number(rawFarmId),
        producerProfile.id
      );

      res.status(200).json({
        success: true,
        message: "Finca obtenida correctamente.",
        data: farm
      });
    } catch (error) {
      const message = error instanceof Error
        ? error.message
        : "Ocurrió un error al consultar la finca.";

      const statusCode =
        message === "No se encontró la finca solicitada."
          ? 404
          : message === "No se encontró el perfil del productor."
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

  update = async (req: Request, res: Response): Promise<void> => {
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
        message: "No tienes permisos para actualizar esta finca."
      });
      return;
    }

    const rawFarmId = req.params.farmId;

    if (typeof rawFarmId !== "string" || !/^[1-9]\d*$/.test(rawFarmId)) {
      res.status(400).json({
        success: false,
        message: "El identificador de la finca no es válido."
      });
      return;
    }

    const validation = updateFarmSchema.safeParse(req.body);

    if (!validation.success) {
      res.status(400).json({
        success: false,
        message: "Los datos de actualización no son válidos.",
        errors: validation.error.issues
      });
      return;
    }

    if (Object.keys(validation.data).length === 0) {
      res.status(400).json({
        success: false,
        message: "Debes proporcionar al menos un campo para actualizar."
      });
      return;
    }

    try {
      const producerProfile = await this.getMyProducerProfile.execute(
        user.sub,
        user.empresaId
      );

      const farm = await this.updateFarm.execute(
        Number(rawFarmId),
        producerProfile.id,
        validation.data
      );

      res.status(200).json({
        success: true,
        message: "Finca actualizada correctamente.",
        data: farm
      });
    } catch (error) {
      const message = error instanceof Error
        ? error.message
        : "Ocurrió un error al actualizar la finca.";

      const statusCode =
        message === "No se encontró la finca solicitada." ||
        message === "No se encontró el perfil del productor."
          ? 404
          : message === "Ya tienes registrada otra finca con ese nombre."
            ? 409
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