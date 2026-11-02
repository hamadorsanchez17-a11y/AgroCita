import express from "express";
import cors from "cors";

import authRoutes from "./modules/auth/presentation/routes/auth.routes.js";
import producerRoutes from "./modules/producers/presentation/routes/producer.routes.js";
import farmRoutes from "./modules/farms/presentation/routes/farm.routes.js";
import cropsRoutes from "./modules/crops/presentation/routes/crop.routes.js";
import assistanceRequestRoutes from "./modules/assistance-requests/presentation/routes/assistanceRequest.routes.js";
import visitRoutes from "./modules/visits/presentation/routes/visit.routes.js";

const app = express();

// Middlewares globales
app.use(cors());
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

// Sección de rutas
app.use("/api/auth", authRoutes);
app.use("/api/producers", producerRoutes);
app.use("/api/farms", farmRoutes);
app.use("/api/crops", cropsRoutes);
app.use("/api/assistance-requests", assistanceRequestRoutes);
app.use("/api/visits", visitRoutes);

// Ruta inicial para comprobar que la API responde
app.get("/api/health", (_req, res) => {
  res.status(200).json({
    success: true,
    message: "AgroCita API está funcionando correctamente."
  });
});

export default app;