import { z } from "zod";

export const scheduleVisitSchema = z.object({
  solicitudId: z.number().int().positive({
    message: "El ID de la solicitud debe ser positivo.",
  }),

  tecnicoId: z.number().int().positive({
    message: "Debe seleccionar un técnico válido.",
  }),

  fincaId: z.number().int().positive({
    message: "El ID de la finca debe ser positivo.",
  }),

  fechaProgramada: z.iso.date({
    error: "La fecha debe ser válida.",
  }),

  horaProgramada: z.string().regex(
    /^([01]\d|2[0-3]):[0-5]\d$/,
    "La hora debe tener el formato HH:mm."
  ),
});

export type ScheduleVisitDTO = z.infer<
  typeof scheduleVisitSchema
>;