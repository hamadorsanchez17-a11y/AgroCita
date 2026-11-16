import axios from 'axios'
import type { FieldValues, Path, UseFormSetError } from 'react-hook-form'

export interface ApiError {
  status: number | null
  title: string
  message: string
  fieldErrors: Record<string, string[]>
}

interface CuerpoError {
  title?: string
  detail?: string
  message?: string
  errors?: Record<string, string[] | string>
}

const MENSAJES_POR_ESTADO: Record<number, string> = {
  400: 'Los datos enviados no son válidos.',
  401: 'Tu sesión terminó. Inicia sesión de nuevo.',
  403: 'No tienes permiso para realizar esta acción.',
  404: 'No encontramos lo que buscabas.',
  409: 'La operación choca con un registro existente.',
  500: 'El servidor tuvo un problema. Intenta de nuevo en unos minutos.',
}

function normalizarCampo(campo: string): string {
  return campo.replace(/\[(\d+)\]/g, '.$1')
}

export function extractApiError(error: unknown): ApiError {
  if (axios.isAxiosError(error)) {
    if (!error.response) {
      return {
        status: null,
        title: 'Sin conexión',
        message:
          error.code === 'ECONNABORTED'
            ? 'El servidor tardó demasiado en responder.'
            : 'No pudimos conectar con el servidor. Revisa tu conexión.',
        fieldErrors: {},
      }
    }

    const { status } = error.response
    const cuerpo = (typeof error.response.data === 'object' ? error.response.data : null) as CuerpoError | null
    const fieldErrors: Record<string, string[]> = {}

    for (const [campo, mensajes] of Object.entries(cuerpo?.errors ?? {})) {
      fieldErrors[normalizarCampo(campo)] = Array.isArray(mensajes) ? mensajes : [String(mensajes)]
    }

    return {
      status,
      title: cuerpo?.title ?? 'Error',
      message:
        cuerpo?.detail ??
        cuerpo?.message ??
        MENSAJES_POR_ESTADO[status] ??
        'Ocurrió un error inesperado.',
      fieldErrors,
    }
  }

  return {
    status: null,
    title: 'Error',
    message: error instanceof Error ? error.message : 'Ocurrió un error inesperado.',
    fieldErrors: {},
  }
}

export function extractErrorMessage(error: unknown): string {
  const { message, fieldErrors } = extractApiError(error)
  const primero = Object.values(fieldErrors)[0]?.[0]

  return primero ?? message
}

export function applyServerErrors<T extends FieldValues>(
  error: unknown,
  setError: UseFormSetError<T>,
  campos: readonly string[],
): string | null {
  const api = extractApiError(error)
  const sinCampo: string[] = []

  for (const [campo, mensajes] of Object.entries(api.fieldErrors)) {
    const raiz = campo.split('.')[0]

    if (campos.includes(raiz)) {
      setError(campo as Path<T>, { type: 'server', message: mensajes[0] })
    } else {
      sinCampo.push(...mensajes)
    }
  }

  if (sinCampo.length > 0) return sinCampo[0]

  return Object.keys(api.fieldErrors).length > 0 ? null : api.message
}
