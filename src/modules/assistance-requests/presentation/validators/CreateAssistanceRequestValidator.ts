import { z } from "zod";

export const createAssistanceRequestSchema = z.object({
  fincaId: z.number().int().positive({
    message: "El ID de la finca debe ser positivo.",
  }),

  cultivoIds: z
    .array(z.number().int().positive())
    .min(1, "Debe seleccionar al menos un cultivo.")
    .refine(
      (ids) => new Set(ids).size === ids.length,
      "No puede seleccionar cultivos repetidos."
    ),

  fechaDeseada: z.iso.date({
    error: "La fecha deseada debe ser válida.",
  }),

  motivo: z
    .string()
    .trim()
    .min(5, "El motivo debe tener al menos 5 caracteres.")
    .max(500, "El motivo no puede superar los 500 caracteres."),
});

export type CreateAssistanceRequestDTO = z.infer<
  typeof createAssistanceRequestSchema
>;