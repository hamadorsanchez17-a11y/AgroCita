
import app from "./app.js";
import { env } from "./shared/config/env.js";
import { pool } from "./shared/database/pool.js";

async function startServer(): Promise<void> {
  try {
    // Comprobar conexión con PostgreSQL
    await pool.query("SELECT 1");

    console.log("Conexión con PostgreSQL establecida.");

    // Iniciar servidor HTTP
    app.listen(env.PORT, () => {
      console.log(`AgroCita API ejecutándose en http://localhost:${env.PORT}`);
    });
  } catch (error) {
    console.error("Error al iniciar AgroCita:", error);

    await pool.end();
    process.exit(1);
  }
}

void startServer();
