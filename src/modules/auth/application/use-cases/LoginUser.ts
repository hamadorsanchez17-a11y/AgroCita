
import bcrypt from "bcrypt";

import type { AuthRepository } from "../../domain/repositories/AuthRepository.js";
import type { TokenService } from "../../domain/services/TokenService.js";

import type { LoginDTO } from "../dto/LoginDTO.js";
import type { AuthResponseDTO } from "../dto/AuthResponseDTO.js";

export class LoginUser {
  constructor(
    private readonly authRepository: AuthRepository,
    private readonly tokenService: TokenService
  ) {}

  async execute(input: LoginDTO): Promise<AuthResponseDTO> {

    // 1. Buscar al usuario por su correo electrónico
    const user =
      await this.authRepository.findUserCredentialsByEmail(
        input.email
      );

    // 2. Comprobar que el usuario exista y que la contraseña coincida
    if (
      !user ||
      !(await bcrypt.compare(input.password, user.passwordHash))
    ) {
      throw new Error("Credenciales inválidas.");
    }

    // 3. Comprobar que la cuenta esté activa
    if (!user.activo) {
      throw new Error("La cuenta está desactivada.");
    }

    // 4. Generar el token JWT
    const accessToken = this.tokenService.generate({
      sub: user.id,
      empresaId: user.empresaId,
      rol: user.rol
    });

    // 5. Devolver el token y los datos públicos del usuario
    return {
      accessToken,
      usuario: {
        id: user.id,
        empresaId: user.empresaId,
        nombreCompleto: user.nombreCompleto,
        email: user.email,
        rol: user.rol
      }
    };
  }
}
