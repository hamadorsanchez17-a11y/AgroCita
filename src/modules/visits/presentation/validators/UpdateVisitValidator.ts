import { z } from "zod";

export const updateVisitSchema = z.object({
  tecnicoId: z.number().int().positive().optional(),

  fechaProgramada: z.iso.date({
    error: "La fecha debe ser válida."
  }).optional(),

  horaProgramada: z.string()
    .regex(
      /^([01]\d|2[0-3]):[0-5]\d$/,
      "La hora debe tener el formato HH:mm."
    )
    .optional(),

  estado: z.enum([
    "PENDIENTE",
    "CONFIRMADA",
    "EN_PROCESO",
    "COMPLETADA",
    "CANCELADA"
  ]).optional()

}).refine(
  (data) =>
    Object.values(data).some((value) => value !== undefined),
  {
    message: "Debe proporcionar al menos un campo para actualizar."
  }
);

export type UpdateVisitDTO = z.infer<typeof updateVisitSchema>;