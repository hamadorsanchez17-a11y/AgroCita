
import jwt from "jsonwebtoken";

import { env } from "../../../../shared/config/env.js";

import type {
  TokenPayload,
  TokenService
} from "../../domain/services/TokenService.js";

export class JwtTokenService implements TokenService {
  generate(payload: TokenPayload): string {
    return jwt.sign(payload, env.JWT_SECRET, {
      expiresIn: "1h"
    });
  }
}
