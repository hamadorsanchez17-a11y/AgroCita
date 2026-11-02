import type { Request, Response } from "express";

import type { AuthenticatedRequest } from "../../../../shared/middlewares/auth.middleware.js";

import { GetMyProducerProfile } from "../../application/use-cases/GetMyProducerProfile.js";

export class ProducerController {
  constructor(
    private readonly getMyProducerProfile: GetMyProducerProfile
  ) {}

  getMyProfile = async (
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
        message: "No tienes permisos para consultar este perfil."
      });
      return;
    }

    try {
      const profile = await this.getMyProducerProfile.execute(
        user.sub,
        user.empresaId
      );

      res.status(200).json({
        success: true,
        message: "Perfil del productor obtenido correctamente.",
        data: profile
      });

    } catch (error) {
      const message =
        error instanceof Error
          ? error.message
          : "Ocurrió un error al consultar el perfil.";

      const statusCode =
        message === "No se encontró el perfil del productor."
          ? 404
          : 403;

      res.status(statusCode).json({
        success: false,
        message
      });
    }
  };
}