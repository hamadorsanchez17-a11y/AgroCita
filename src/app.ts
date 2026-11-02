
import express from "express";
import cors from "cors";
import authRoutes from "./modules/auth/presentation/routes/auth.routes.js";
import producerRoutes from "./modules/producers/presentation/routes/producer.routes.js";
import farmRoutes from "./modules/farms/presentation/routes/farm.routes.js";

const app = express();

// Middlewares globales
app.use(cors());
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

//Sección de rutas
app.use("/api/auth", authRoutes);
app.use("/api/producers", producerRoutes);
app.use("/api/farms", farmRoutes);

// Ruta inicial para comprobar que la API responde
app.get("/api/health", (_req, res) => {
  res.status(200).json({
    success: true,
    message: "AgroCita API está funcionando correctamente."
  });
});

export default app;
