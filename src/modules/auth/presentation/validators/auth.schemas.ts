
import { z } from "zod";

// Validación para el registro de productores
export const registerProducerSchema = z.object({
  nombreCompleto: z
    .string()
    .trim()
    .min(3, "El nombre debe tener al menos 3 caracteres.")
    .max(150, "El nombre no puede superar los 150 caracteres."),

  email: z
    .string()
    .trim()
    .email("El correo electrónico no es válido.")
    .max(120, "El correo no puede superar los 120 caracteres."),

  password: z
    .string()
    .min(8, "La contraseña debe tener al menos 8 caracteres.")
    .max(72, "La contraseña no puede superar los 72 caracteres."),

  codigoInvitacion: z
    .string()
    .trim()
    .length(12, "El código de invitación debe tener 12 caracteres."),

  dni: z
    .string()
    .trim()
    .regex(/^\d{13}$/, "El DNI debe contener exactamente 13 dígitos."),

  telefono: z
    .string()
    .trim()
    .min(8, "El teléfono debe tener al menos 8 caracteres.")
    .max(20, "El teléfono no puede superar los 20 caracteres."),

  direccion: z
    .string()
    .trim()
    .max(250, "La dirección no puede superar los 250 caracteres.")
    .optional()
});

// Validación para el inicio de sesión
export const loginSchema = z.object({
  email: z
    .string()
    .trim()
    .email("El correo electrónico no es válido.")
    .max(120),

  password: z
    .string()
    .min(1, "La contraseña es obligatoria.")
});

// Tipos derivados de los esquemas
export type RegisterProducerDTO = z.infer<
  typeof registerProducerSchema
>;

export type LoginDTO = z.infer<typeof loginSchema>;
