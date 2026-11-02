import { z } from "zod";

export const createFarmSchema = z.object({
  nombre: z
    .string()
    .trim()
    .min(2, "El nombre debe tener al menos 2 caracteres.")
    .max(120, "El nombre no puede superar los 120 caracteres."),

  departamento: z
    .string()
    .trim()
    .min(2, "El departamento es obligatorio.")
    .max(100),

  municipio: z
    .string()
    .trim()
    .min(2, "El municipio es obligatorio.")
    .max(100),

  ubicacion: z
    .string()
    .trim()
    .min(3, "La ubicación debe tener al menos 3 caracteres.")
    .max(250),

  areaManzanas: z
    .number()
    .positive("El área debe ser mayor que cero."),

  descripcion: z
    .string()
    .trim()
    .max(500, "La descripción no puede superar los 500 caracteres.")
    .optional()
});

export type CreateFarmInput = z.infer<typeof createFarmSchema>;
export const updateFarmSchema = createFarmSchema.partial();

export type UpdateFarmInput = z.infer<typeof updateFarmSchema>;