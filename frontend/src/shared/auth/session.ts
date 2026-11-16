export type Rol = 'SuperAdmin' | 'AdminEmpresa' | 'Tecnico' | 'Productor'

export interface UsuarioActual {
  id: number
  nombre: string
  email: string
  rol: Rol
  empresaId: number | null
  empresaNombre: string | null
}

export interface Sesion {
  token: string
  expiraEl: number
  usuario: UsuarioActual
}

const CLAVE = 'agrocita.sesion'
export const EVENTO_SESION_EXPIRADA = 'agrocita:sesion-expirada'

function leer(): Sesion | null {
  try {
    const crudo = localStorage.getItem(CLAVE)
    if (!crudo) return null

    const sesion = JSON.parse(crudo) as Sesion
    if (!sesion.token || sesion.expiraEl <= Date.now()) {
      localStorage.removeItem(CLAVE)
      return null
    }

    return sesion
  } catch {
    localStorage.removeItem(CLAVE)
    return null
  }
}

export const session = {
  get: leer,
  token: () => leer()?.token ?? null,
  save(token: string, expiraEnSegundos: number, usuario: UsuarioActual): Sesion {
    const sesion: Sesion = {
      token,
      expiraEl: Date.now() + expiraEnSegundos * 1000,
      usuario,
    }
    localStorage.setItem(CLAVE, JSON.stringify(sesion))
    return sesion
  },
  updateUser(usuario: UsuarioActual) {
    const actual = leer()
    if (actual) localStorage.setItem(CLAVE, JSON.stringify({ ...actual, usuario }))
  },
  clear() {
    localStorage.removeItem(CLAVE)
  },
}
