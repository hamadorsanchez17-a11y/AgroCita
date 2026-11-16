import axios from 'axios'
import { EVENTO_SESION_EXPIRADA, session } from '@/shared/auth/session'

export const http = axios.create({
  baseURL: import.meta.env.VITE_API_URL ?? 'http://localhost:5071/api',
  timeout: 20000,
  headers: { Accept: 'application/json' },
})

http.interceptors.request.use((config) => {
  const token = session.token()

  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }

  return config
})

http.interceptors.response.use(
  (response) => response,
  (error: unknown) => {
    if (
      axios.isAxiosError(error) &&
      error.response?.status === 401 &&
      error.config?.headers?.Authorization
    ) {
      session.clear()
      window.dispatchEvent(new Event(EVENTO_SESION_EXPIRADA))
    }

    return Promise.reject(error)
  },
)

export interface Paginado<T> {
  items: T[]
  total: number
  pagina: number
  tamano: number
}

export interface Referencia {
  id: number
  nombre: string
}

export function limpiarParams<T extends object>(params: T): Partial<T> {
  return Object.fromEntries(
    Object.entries(params).filter(([, valor]) => valor !== undefined && valor !== null && valor !== ''),
  ) as Partial<T>
}
