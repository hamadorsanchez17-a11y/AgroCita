
import type { Request, Response } from "express";

import type { AuthenticatedRequest } from "../../../../shared/middlewares/auth.middleware.js";

import { ScheduleVisit } from "../../application/use-cases/ScheduleVisit.js";
import { GetVisitsByTechnician } from "../../application/use-cases/GetVisitsByTechnician.js";
import { GetVisitsByCompany } from "../../application/use-cases/GetVisitsByCompany.js";
import { UpdateVisit } from "../../application/use-cases/UpdateVisit.js";

import { scheduleVisitSchema } from "../validators/ScheduleVisitValidator.js";
import { updateVisitSchema } from "../validators/UpdateVisitValidator.js";

export class VisitController {
  constructor(
    private readonly scheduleVisit: ScheduleVisit,
    private readonly getVisitsByTechnician: GetVisitsByTechnician,
    private readonly getVisitsByCompany: GetVisitsByCompany,
    private readonly updateVisit: UpdateVisit
  ) {}

  // PROGRAMAR UNA VISITA
  schedule = async (
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

    if (user.empresaId === null) {
      res.status(403).json({
        success: false,
        message: "El usuario no pertenece a una empresa."
      });
      return;
    }

    const validation = scheduleVisitSchema.safeParse(req.body);

    if (!validation.success) {
      res.status(400).json({
        success: false,
        message: "Datos de programación inválidos.",
        errors: validation.error.flatten()
      });
      return;
    }

    try {
      const visit = await this.scheduleVisit.execute({
        empresaId: user.empresaId,
        ...validation.data
      });

      res.status(201).json({
        success: true,
        message: "Visita programada correctamente.",
        data: visit
      });
    } catch (error) {
      res.status(400).json({
        success: false,
        message: error instanceof Error
          ? error.message
          : "Error al programar la visita."
      });
    }
  };

  // CONSULTAR MIS VISITAS
  getMyVisits = async (
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

    if (user.empresaId === null) {
      res.status(403).json({
        success: false,
        message: "El usuario no pertenece a una empresa."
      });
      return;
    }

    try {
      const visits = await this.getVisitsByTechnician.execute(
        user.sub,
        user.empresaId
      );

      res.status(200).json({
        success: true,
        message: "Visitas del técnico obtenidas correctamente.",
        data: visits
      });
    } catch (error) {
      res.status(400).json({
        success: false,
        message: error instanceof Error
          ? error.message
          : "Error al consultar las visitas."
      });
    }
  };

  // CONSULTAR TODAS LAS VISITAS DE LA EMPRESA
  getAllByCompany = async (
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

    if (user.empresaId === null) {
      res.status(403).json({
        success: false,
        message: "El usuario no pertenece a una empresa."
      });
      return;
    }

    try {
      const visits = await this.getVisitsByCompany.execute(
        user.empresaId
      );

      res.status(200).json({
        success: true,
        message: "Visitas de la empresa obtenidas correctamente.",
        data: visits
      });
    } catch (error) {
      res.status(400).json({
        success: false,
        message: error instanceof Error
          ? error.message
          : "Error al consultar las visitas."
      });
    }
  };

  // ACTUALIZAR O REPROGRAMAR UNA VISITA
  update = async (
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

    if (user.empresaId === null) {
      res.status(403).json({
        success: false,
        message: "El usuario no pertenece a una empresa."
      });
      return;
    }

    const id = Number(req.params.id);

    if (!Number.isInteger(id) || id <= 0) {
      res.status(400).json({
        success: false,
        message: "El ID de la visita no es válido."
      });
      return;
    }

    const validation = updateVisitSchema.safeParse(req.body);

    if (!validation.success) {
      res.status(400).json({
        success: false,
        message: "Datos de actualización inválidos.",
        errors: validation.error.flatten()
      });
      return;
    }

    try {
      const data = validation.data;

      const visit = await this.updateVisit.execute({
        id,
        empresaId: user.empresaId,
        ...(data.tecnicoId !== undefined && {
          tecnicoId: data.tecnicoId
        }),
        ...(data.fechaProgramada !== undefined && {
          fechaProgramada: data.fechaProgramada
        }),
        ...(data.horaProgramada !== undefined && {
          horaProgramada: data.horaProgramada
        }),
        ...(data.estado !== undefined && {
          estado: data.estado
        })
      });

      if (!visit) {
        res.status(404).json({
          success: false,
          message: "No se encontró la visita solicitada."
        });
        return;
      }

      res.status(200).json({
        success: true,
        message: "Visita actualizada correctamente.",
        data: visit
      });
    } catch (error) {
      res.status(400).json({
        success: false,
        message: error instanceof Error
          ? error.message
          : "Error al actualizar la visita."
      });
    }
  };
}
