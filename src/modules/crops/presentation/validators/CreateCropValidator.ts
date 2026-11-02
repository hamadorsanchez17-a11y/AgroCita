import { z } from "zod";

export const createCropSchema = z.object({
  fincaId: z.number().int().positive({
    message: "El ID de la finca debe ser un número positivo",
  }),

  nombre: z
    .string()
    .min(2, "El nombre del cultivo debe tener al menos 2 caracteres")
    .max(100, "El nombre del cultivo no puede superar los 100 caracteres")
    .trim(),
});

export type CreateCropDTO = z.infer<typeof createCropSchema>;