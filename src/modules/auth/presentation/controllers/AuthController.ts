
import type { Request, Response } from "express";

import { RegisterProducer } from "../../application/use-cases/RegisterProducer.js";
import { LoginUser } from "../../application/use-cases/LoginUser.js";
import type {AuthenticatedRequest} from "../../../../shared/middlewares/auth.middleware.js";

import {
  registerProducerSchema,
  loginSchema
} from "../validators/auth.schemas.js";

export class AuthController {
  constructor(
    private readonly registerProducer: RegisterProducer,
    private readonly loginUser: LoginUser
  ) {}

  
  // Obtener los datos del usuario autenticado
  me = (req: Request, res: Response): void => {

    const authenticatedRequest = req as AuthenticatedRequest;
    const user = authenticatedRequest.user;

    if (!user) {
      res.status(401).json({
        success: false,
        message: "No se pudo identificar al usuario autenticado."
      });
      return;
    }

    res.status(200).json({
      success: true,
      message: "Información del usuario obtenida correctamente.",
      data: {
        id: user.sub,
        empresaId: user.empresaId,
        rol: user.rol
      }
    });
  };


  // Registrar un productor
  register = async (req: Request, res: Response): Promise<void> => {
    const validation = registerProducerSchema.safeParse(req.body);

    if (!validation.success) {
      res.status(400).json({
        success: false,
        message: "Los datos enviados no son válidos.",
        errors: validation.error.issues.map(issue => ({
          field: issue.path.join("."),
          message: issue.message
        }))
      });
      return;
    }

    try {
      const input = validation.data;

      const result = await this.registerProducer.execute({
        nombreCompleto: input.nombreCompleto,
        email: input.email,
        password: input.password,
        codigoInvitacion: input.codigoInvitacion,
        dni: input.dni,
        telefono: input.telefono,
        ...(input.direccion !== undefined
          ? { direccion: input.direccion }
          : {})
      });

      res.status(201).json({
        success: true,
        message: "Productor registrado correctamente.",
        data: result
      });

    } catch (error) {
      const message = error instanceof Error ? error.message : "";

      if (
        message.includes("correo electrónico ya está registrado") ||
        message.includes("productor con ese DNI")
      ) {
        res.status(409).json({ success: false, message });
        return;
      }

      if (message.includes("código de invitación")) {
        res.status(400).json({ success: false, message });
        return;
      }

      console.error("Error al registrar productor:", error);

      res.status(500).json({
        success: false,
        message: "Ocurrió un error interno al registrar el productor."
      });
    }
  };

  // Iniciar sesión
  login = async (req: Request, res: Response): Promise<void> => {
    const validation = loginSchema.safeParse(req.body);

    if (!validation.success) {
      res.status(400).json({
        success: false,
        message: "Los datos enviados no son válidos.",
        errors: validation.error.issues.map(issue => ({
          field: issue.path.join("."),
          message: issue.message
        }))
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
      const message = error instanceof Error ? error.message : "";

      if (message === "Credenciales inválidas.") {
        res.status(401).json({
          success: false,
          message
        });
        return;
      }

      if (message === "La cuenta está desactivada.") {
        res.status(403).json({
          success: false,
          message
        });
        return;
      }

      console.error("Error al iniciar sesión:", error);

      res.status(500).json({
        success: false,
        message: "Ocurrió un error interno al iniciar sesión."
      });
    }
  };
}
