
import test from "node:test";
import assert from "node:assert/strict";

import { LoginUser } from "../../src/modules/auth/application/use-cases/LoginUser.js";
import { MockAuthRepository } from "../../src/modules/auth/infrastructure/repositories/MockAuthRepository.js";

import type { TokenService } from "../../src/modules/auth/domain/services/TokenService.js";

const createTokenService = (): TokenService => ({
  generate: () => "token-de-prueba"
});

const createUser = async () => {
  const repository = new MockAuthRepository();

  await repository.createProducerAccount({
    empresaId: 1,
    nombreCompleto: "Juan Pérez",
    email: "juan@example.com",
    passwordHash: await (await import("bcrypt")).default.hash("Password123", 12),
    dni: "0801199012345",
    telefono: "99998888",
    direccion: "Tegucigalpa"
  });

  return repository;
};

test("Debe iniciar sesión correctamente", async () => {
  const repository = await createUser();
  const tokenService = createTokenService();

  const loginUser = new LoginUser(repository, tokenService);

  const result = await loginUser.execute({
    email: "juan@example.com",
    password: "Password123"
  });

  assert.equal(result.accessToken, "token-de-prueba");
  assert.equal(result.usuario.id, 1);
  assert.equal(result.usuario.email, "juan@example.com");
  assert.equal(result.usuario.rol, "PRODUCTOR");
});

test("Debe rechazar una contraseña incorrecta", async () => {
  const repository = await createUser();
  const tokenService = createTokenService();

  const loginUser = new LoginUser(repository, tokenService);

  await assert.rejects(
    loginUser.execute({
      email: "juan@example.com",
      password: "PasswordIncorrecta"
    }),
    /Credenciales inválidas/
  );
});

test("Debe rechazar un correo electrónico inexistente", async () => {
  const repository = await createUser();
  const tokenService = createTokenService();

  const loginUser = new LoginUser(repository, tokenService);

  await assert.rejects(
    loginUser.execute({
      email: "desconocido@example.com",
      password: "Password123"
    }),
    /Credenciales inválidas/
  );
});
