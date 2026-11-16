const fechaLarga = new Intl.DateTimeFormat('es-HN', { day: 'numeric', month: 'long', year: 'numeric' })
const fechaCorta = new Intl.DateTimeFormat('es-HN', { day: '2-digit', month: 'short', year: 'numeric' })
const hora = new Intl.DateTimeFormat('es-HN', { hour: 'numeric', minute: '2-digit', hour12: true })
const mesCorto = new Intl.DateTimeFormat('es-HN', { month: 'short' })
const diaSemana = new Intl.DateTimeFormat('es-HN', { weekday: 'long' })
const moneda = new Intl.NumberFormat('es-HN', { style: 'currency', currency: 'HNL', maximumFractionDigits: 0 })

function aFecha(valor: string): Date {
  if (/^\d{4}-\d{2}-\d{2}$/.test(valor)) {
    const [anio, mes, dia] = valor.split('-').map(Number)
    return new Date(anio, mes - 1, dia)
  }

  return new Date(valor)
}

export const formato = {
  fecha: (valor?: string | null) => (valor ? fechaCorta.format(aFecha(valor)) : '—'),
  fechaLarga: (valor?: string | null) => (valor ? fechaLarga.format(aFecha(valor)) : '—'),
  hora: (valor?: string | null) => (valor ? hora.format(aFecha(valor)) : '—'),
  fechaHora: (valor?: string | null) =>
    valor ? `${fechaCorta.format(aFecha(valor))} · ${hora.format(aFecha(valor))}` : '—',
  dia: (valor: string) => String(aFecha(valor).getDate()).padStart(2, '0'),
  mes: (valor: string) => mesCorto.format(aFecha(valor)).replace('.', ''),
  anio: (valor: string) => aFecha(valor).getFullYear(),
  diaSemana: (valor: string) => diaSemana.format(aFecha(valor)),
  moneda: (valor: number) => moneda.format(valor),
  area: (valor?: number | null) => (valor == null ? '—' : `${valor} mz`),
  iniciales: (nombre: string) =>
    nombre
      .split(' ')
      .filter(Boolean)
      .slice(0, 2)
      .map((parte) => parte[0]?.toUpperCase())
      .join(''),
}

export function hoyISO(): string {
  const ahora = new Date()
  const local = new Date(ahora.getTime() - ahora.getTimezoneOffset() * 60000)
  return local.toISOString().slice(0, 10)
}

export function sumarDias(fecha: string, dias: number): string {
  const base = aFecha(fecha)
  base.setDate(base.getDate() + dias)
  const local = new Date(base.getTime() - base.getTimezoneOffset() * 60000)
  return local.toISOString().slice(0, 10)
}

export function aISOLocal(valor: string): string {
  const fecha = new Date(valor)
  const local = new Date(fecha.getTime() - fecha.getTimezoneOffset() * 60000)
  return local.toISOString().slice(0, 16)
}

export function claveDia(valor: string): string {
  return aISOLocal(valor).slice(0, 10)
}
