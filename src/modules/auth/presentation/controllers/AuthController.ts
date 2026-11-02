
import type { Request, Response } from "express";

import type { AuthenticatedRequest } from "../../../../shared/middlewares/auth.middleware.js";

import { RegisterProducer } from "../../application/use-cases/RegisterProducer.js";
import { LoginUser } from "../../application/use-cases/LoginUser.js";

import {
  registerProducerSchema,
  loginSchema
} from "../validators/auth.schemas.js";

export class AuthController {
  constructor(
    private readonly registerProducer: RegisterProducer,
    private readonly loginUser: LoginUser
  ) {}

  register = async (
    req: Request,
    res: Response
  ): Promise<void> => {
    const validation = registerProducerSchema.safeParse(req.body);

    if (!validation.success) {
      res.status(400).json({
        success: false,
        message: "Datos de registro inválidos.",
        errors: validation.error.issues
      });
      return;
    }

    try {
      const data = validation.data;

      const result = await this.registerProducer.execute({
        nombreCompleto: data.nombreCompleto,
        email: data.email,
        password: data.password,
        codigoInvitacion: data.codigoInvitacion,
        dni: data.dni,
        telefono: data.telefono,
        ...(data.direccion !== undefined
          ? { direccion: data.direccion }
          : {})
      });

      res.status(201).json({
        success: true,
        message: "Productor registrado correctamente.",
        data: result
      });
    } catch (error) {
      const message = error instanceof Error
        ? error.message
        : "Error al registrar el productor.";

      res.status(400).json({
        success: false,
        message
      });
    }
  };

  login = async (
    req: Request,
    res: Response
  ): Promise<void> => {
    const validation = loginSchema.safeParse(req.body);

    if (!validation.success) {
      res.status(400).json({
        success: false,
        message: "Datos de inicio de sesión inválidos.",
        errors: validation.error.issues
      });
      return;
    }

    try {
      const result = await this.loginUser.execute(validation.data);

      res.status(200).json({
        success: true,
        message: "Inicio de sesión exitoso.",
        data: result
      });
    } catch (error) {
      const message = error instanceof Error
        ? error.message
        : "Error al iniciar sesión.";

      const statusCode = message === "Credenciales inválidas."
        ? 401
        : 403;

      res.status(statusCode).json({
        success: false,
        message
      });
    }
  };

  me = async (
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

    res.status(200).json({
      success: true,
      message: "Usuario autenticado correctamente.",
      data: {
        id: user.sub,
        empresaId: user.empresaId,
        rol: user.rol
      }
    });
  };
}
