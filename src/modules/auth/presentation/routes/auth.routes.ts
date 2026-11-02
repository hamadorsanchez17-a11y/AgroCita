
import { Router } from "express";

import { AuthController } from "../controllers/AuthController.js";

import { RegisterProducer } from "../../application/use-cases/RegisterProducer.js";
import { LoginUser } from "../../application/use-cases/LoginUser.js";

import { MockAuthRepository } from "../../infrastructure/repositories/MockAuthRepository.js";
import { JwtTokenService } from "../../infrastructure/services/JwtTokenService.js";
import { authenticateToken } from "../../../../shared/middlewares/auth.middleware.js";

const authRouter = Router();

// Dependencias compartidas
const authRepository = new MockAuthRepository();
const tokenService = new JwtTokenService();

// Casos de uso
const registerProducer = new RegisterProducer(authRepository);

const loginUser = new LoginUser(
  authRepository,
  tokenService
);

// Controlador
const authController = new AuthController(
  registerProducer,
  loginUser
);

// Endpoints
authRouter.post("/register", authController.register);
authRouter.post("/login", authController.login);
authRouter.get("/me", authenticateToken, authController.me);

export default authRouter;
