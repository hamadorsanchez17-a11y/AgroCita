import clsx from 'clsx'
import {
  Building2,
  CalendarDays,
  ClipboardList,
  FileText,
  FlaskConical,
  Home,
  Layers,
  LogOut,
  Menu,
  Sprout,
  Tractor,
  UserRound,
  Users,
  X,
} from 'lucide-react'
import type { LucideIcon } from 'lucide-react'
import { AnimatePresence, LayoutGroup, motion } from 'motion/react'
import { Suspense, useEffect, useState } from 'react'
import { NavLink, Outlet, useLocation } from 'react-router-dom'
import { useAuth, useUsuario } from '@/shared/auth/AuthContext'
import type { Rol } from '@/shared/auth/session'
import { formato } from '@/shared/lib/format'
import { Cargando } from '@/shared/ui/feedback'
import { Contours } from '@/shared/ui/layout'
import { Marca } from '@/shared/ui/Marca'

interface Enlace {
  a: string
  texto: string
  icono: LucideIcon
  exacto?: boolean
}

const NAVEGACION: Record<Rol, Enlace[]> = {
  Productor: [
    { a: '/', texto: 'Inicio', icono: Home, exacto: true },
    { a: '/fincas', texto: 'Mis fincas', icono: Tractor },
    { a: '/solicitudes', texto: 'Solicitudes', icono: ClipboardList },
    { a: '/informes', texto: 'Informes', icono: FileText },
    { a: '/perfil', texto: 'Mi perfil', icono: UserRound },
  ],
  AdminEmpresa: [
    { a: '/', texto: 'Inicio', icono: Home, exacto: true },
    { a: '/solicitudes', texto: 'Solicitudes', icono: ClipboardList },
    { a: '/agenda', texto: 'Agenda', icono: CalendarDays },
    { a: '/productores', texto: 'Productores', icono: Users },
    { a: '/tecnicos', texto: 'Técnicos', icono: Sprout },
    { a: '/catalogos', texto: 'Catálogos', icono: FlaskConical },
    { a: '/organizacion', texto: 'Organización', icono: Building2 },
  ],
  Tecnico: [
    { a: '/', texto: 'Inicio', icono: Home, exacto: true },
    { a: '/visitas', texto: 'Mis visitas', icono: CalendarDays },
  ],
  SuperAdmin: [
    { a: '/empresas', texto: 'Organizaciones', icono: Building2 },
    { a: '/planes', texto: 'Planes', icono: Layers },
  ],
}

const NOMBRE_ROL: Record<Rol, string> = {
  Productor: 'Productor',
  AdminEmpresa: 'Administrador',
  Tecnico: 'Técnico',
  SuperAdmin: 'AgroCita',
}

const salida = [0.22, 1, 0.36, 1] as const

function Navegacion({ grupo, alNavegar }: { grupo: string; alNavegar?: () => void }) {
  const usuario = useUsuario()
  const { cerrar } = useAuth()

  return (
    <div className="relative flex h-full flex-col overflow-hidden bg-forest-deep text-paper">
      <div className="reticula pointer-events-none absolute inset-0 opacity-60" aria-hidden />
      <div className="pointer-events-none absolute -top-24 -left-24 size-72 rounded-full bg-ochre/10 blur-3xl" aria-hidden />
      <Contours className="pointer-events-none absolute -right-56 -bottom-44 size-[540px] text-ochre/15" />

      <div className="relative px-6 pt-7 pb-7">
        <Marca claro compacto />
        <div className="mt-6 flex items-center gap-2 rounded-xl border border-paper/10 bg-paper/[0.04] px-3 py-2.5">
          <span className="relative grid size-2 place-items-center" aria-hidden>
            <span className="absolute size-2 animate-ping rounded-full bg-ochre/60" />
            <span className="size-1.5 rounded-full bg-ochre" />
          </span>
          <p className="truncate font-mono text-[10.5px] tracking-[0.12em] text-paper/70 uppercase">
            {usuario.empresaNombre ?? 'Administración SaaS'}
          </p>
        </div>
      </div>

      <p className="relative px-6 pb-2 font-mono text-[10px] tracking-[0.16em] text-paper/40 uppercase">Menú</p>
      <LayoutGroup id={grupo}>
        <nav className="relative flex flex-1 flex-col gap-1 overflow-y-auto px-3" aria-label="Principal">
          {NAVEGACION[usuario.rol].map(({ a, texto, icono: Icono, exacto }, indice) => (
            <motion.div
              key={a}
              initial={{ opacity: 0, x: -12 }}
              animate={{ opacity: 1, x: 0 }}
              transition={{ duration: 0.45, ease: salida, delay: 0.05 + indice * 0.04 }}
            >
              <NavLink
                to={a}
                end={exacto}
                onClick={alNavegar}
                className={({ isActive }) =>
                  clsx(
                    'group relative flex items-center gap-3 rounded-xl px-3.5 py-2.5 text-[14.5px] font-medium transition-colors duration-200',
                    isActive ? 'text-paper' : 'text-paper/60 hover:text-paper',
                  )
                }
              >
                {({ isActive }) => (
                  <>
                    {isActive && (
                      <motion.span
                        layoutId="nav-activo"
                        className="absolute inset-0 rounded-xl border border-paper/10 bg-paper/[0.08] shadow-[inset_0_1px_0_rgb(255_255_255/0.06)]"
                        transition={{ type: 'spring', stiffness: 380, damping: 32 }}
                        aria-hidden
                      >
                        <span className="absolute top-1/2 -left-3 h-5 w-1 -translate-y-1/2 rounded-r-full bg-ochre shadow-[0_0_12px_rgb(197_240_90/0.8)]" />
                      </motion.span>
                    )}
                    <span className="absolute inset-0 rounded-xl bg-paper/[0.04] opacity-0 transition-opacity group-hover:opacity-100" aria-hidden />
                    <Icono
                      className={clsx(
                        'relative size-[18px] transition duration-300 group-hover:scale-110',
                        isActive ? 'text-ochre' : 'text-paper/45 group-hover:text-paper/80',
                      )}
                      aria-hidden
                    />
                    <span className="relative">{texto}</span>
                  </>
                )}
              </NavLink>
            </motion.div>
          ))}
        </nav>
      </LayoutGroup>

      <div className="relative m-3 rounded-2xl border border-paper/10 bg-paper/[0.05] p-3 backdrop-blur">
        <div className="flex items-center gap-3">
          <div className="grid size-10 shrink-0 place-items-center rounded-xl bg-ochre font-display text-[15px] font-semibold text-forest-deep shadow-[0_0_0_3px_rgb(197_240_90/0.15)]">
            {formato.iniciales(usuario.nombre)}
          </div>
          <div className="min-w-0 flex-1">
            <p className="truncate text-sm font-semibold">{usuario.nombre}</p>
            <p className="truncate font-mono text-[10.5px] tracking-wider text-paper/55 uppercase">{NOMBRE_ROL[usuario.rol]}</p>
          </div>
          <button
            type="button"
            onClick={cerrar}
            aria-label="Cerrar sesión"
            title="Cerrar sesión"
            className="grid size-9 shrink-0 cursor-pointer place-items-center rounded-lg text-paper/60 transition hover:bg-paper/10 hover:text-ochre"
          >
            <LogOut className="size-[18px]" />
          </button>
        </div>
      </div>
    </div>
  )
}

export function AppLayout() {
  const [menuAbierto, setMenuAbierto] = useState(false)
  const { pathname } = useLocation()

  useEffect(() => {
    window.scrollTo({ top: 0 })
  }, [pathname])

  return (
    <div className="min-h-dvh lg:pl-[272px]">
      <aside className="fixed inset-y-0 left-0 hidden w-[272px] lg:block">
        <Navegacion grupo="escritorio" />
      </aside>

      <header className="sticky top-0 z-30 flex items-center justify-between border-b border-line/70 bg-paper/80 px-5 py-3 backdrop-blur-xl lg:hidden">
        <Marca compacto />
        <button
          type="button"
          onClick={() => setMenuAbierto(true)}
          aria-label="Abrir menú"
          className="grid size-10 cursor-pointer place-items-center rounded-xl border border-line bg-card text-ink shadow-sheet"
        >
          <Menu className="size-5" />
        </button>
      </header>

      <AnimatePresence>
        {menuAbierto && (
          <div className="fixed inset-0 z-40 lg:hidden">
            <motion.div
              className="absolute inset-0 bg-forest-deep/60 backdrop-blur-sm"
              onClick={() => setMenuAbierto(false)}
              initial={{ opacity: 0 }}
              animate={{ opacity: 1 }}
              exit={{ opacity: 0 }}
              aria-hidden
            />
            <motion.div
              className="absolute inset-y-0 left-0 w-[288px] shadow-lift"
              initial={{ x: '-100%' }}
              animate={{ x: 0 }}
              exit={{ x: '-100%' }}
              transition={{ type: 'spring', stiffness: 360, damping: 36 }}
            >
              <Navegacion grupo="movil" alNavegar={() => setMenuAbierto(false)} />
              <button
                type="button"
                onClick={() => setMenuAbierto(false)}
                aria-label="Cerrar menú"
                className="absolute top-6 right-4 grid size-9 cursor-pointer place-items-center rounded-lg text-paper/70 hover:bg-paper/10"
              >
                <X className="size-5" />
              </button>
            </motion.div>
          </div>
        )}
      </AnimatePresence>

      <main className="mx-auto w-full max-w-[1200px] px-5 py-8 sm:px-8 lg:px-12 lg:py-14">
        <Suspense fallback={<Cargando />}>
          <motion.div
            key={pathname}
            initial={{ opacity: 0, y: 14 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5, ease: salida }}
          >
            <Outlet />
          </motion.div>
        </Suspense>
      </main>
    </div>
  )
}
