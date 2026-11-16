import { http } from '@/shared/api/http'
import type { AuthResponse } from '@/shared/auth/AuthContext'
import type { LoginValues, RegistroValues } from './schemas'

export const authApi = {
  login: (datos: LoginValues) => http.post<AuthResponse>('/auth/login', datos).then((r) => r.data),
  registrarProductor: (datos: RegistroValues) =>
    http.post<AuthResponse>('/auth/registro-productor', datos).then((r) => r.data),
}
