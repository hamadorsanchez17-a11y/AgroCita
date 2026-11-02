
import type { Request, Response, NextFunction } from "express";
import jwt from "jsonwebtoken";

import { env } from "../config/env.js";

export interface AuthenticatedUser {
  sub: number;
  empresaId: number | null;
  rol: string;
}

export interface AuthenticatedRequest extends Request {
  user?: AuthenticatedUser;
}

export function authenticateToken(
  req: AuthenticatedRequest,
  res: Response,
  next: NextFunction
): void {

  // Obtener el encabezado Authorization
  const authorization = req.headers.authorization;

  // Comprobar que exista y utilice Bearer
  if (!authorization || !authorization.startsWith("Bearer ")) {
    res.status(401).json({
      success: false,
      message: "Se requiere un token de autenticación."
    });
    return;
  }

  // Extraer el token
  const token = authorization.split(" ")[1];

  if (!token) {
    res.status(401).json({
      success: false,
      message: "El token no es válido."
    });
    return;
  }

  try {
    // Verificar la firma y vigencia del JWT
    const decoded = jwt.verify(token, env.JWT_SECRET);

    if (
      typeof decoded === "string" ||
      typeof decoded.sub !== "number" ||
      (decoded.empresaId !== null &&
        typeof decoded.empresaId !== "number") ||
      typeof decoded.rol !== "string"
    ) {
      res.status(401).json({
        success: false,
        message: "El contenido del token no es válido."
      });
      return;
    }

    // Guardar la identidad autenticada en la petición
    req.user = {
      sub: decoded.sub,
      empresaId: decoded.empresaId,
      rol: decoded.rol
    };

    // Continuar con el siguiente middleware o controlador
    next();

  } catch {
    res.status(401).json({
      success: false,
      message: "El token es inválido o ha expirado."
    });
  }
}
