
import "dotenv/config";
import { z } from "zod";

const envSchema = z.object({
  PORT: z.coerce.number().int().min(1).max(65535).default(3000),

  DB_HOST: z.string().min(1),
  DB_PORT: z.coerce.number().int().min(1).max(65535),
  DB_NAME: z.string().min(1),
  DB_USER: z.string().min(1),
  DB_PASSWORD: z.string().min(1),
  JWT_SECRET: z.string().min(32)
});

const result = envSchema.safeParse(process.env);

if (!result.success) {
  console.error("Error en la configuración de variables de entorno:");
  console.error(z.treeifyError(result.error));
  throw new Error("Configuración del entorno inválida.");
}

export const env = result.data;
