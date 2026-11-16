import { createBrowserRouter, Navigate } from 'react-router-dom'
import type { RouteObject } from 'react-router-dom'
import { AppLayout } from '@/app/AppLayout'
import { NotFoundPage } from '@/app/NotFoundPage'
import { LoginPage } from '@/features/auth/LoginPage'
import { RegistroPage } from '@/features/auth/RegistroPage'
import { rutaInicial, useUsuario } from '@/shared/auth/AuthContext'
import type { Rol } from '@/shared/auth/session'
import { GuestOnly, RequireAuth, RequireRole } from '@/shared/routing/guards'
import { PantallaCargando } from '@/shared/ui/feedback'

function Portada() {
  const usuario = useUsuario()

  return <Navigate to={rutaInicial(usuario.rol)} replace />
}

function soloPara(roles: Rol[], children: RouteObject[]): RouteObject {
  return { element: <RequireRole roles={roles} />, children }
}

export const router = createBrowserRouter([
  {
    hydrateFallbackElement: <PantallaCargando />,
    children: [
      {
        element: <GuestOnly />,
        children: [
          { path: '/login', element: <LoginPage /> },
          { path: '/registro', element: <RegistroPage /> },
        ],
      },
      {
        element: <RequireAuth />,
        children: [
          {
            element: <AppLayout />,
            children: [
              soloPara(['SuperAdmin'], [
                { path: '/empresas', lazy: () => import('@/features/admin/EmpresasPage').then((m) => ({ Component: m.EmpresasPage })) },
                { path: '/planes', lazy: () => import('@/features/admin/PlanesPage').then((m) => ({ Component: m.PlanesPage })) },
              ]),
              soloPara(['Productor', 'AdminEmpresa', 'Tecnico'], [
                { path: '/', lazy: () => import('@/features/inicio/InicioPage').then((m) => ({ Component: m.InicioPage })) },
                { path: '/fincas/:id', lazy: () => import('@/features/fincas/FincaDetallePage').then((m) => ({ Component: m.FincaDetallePage })) },
                { path: '/visitas/:id', lazy: () => import('@/features/visitas/VisitaDetallePage').then((m) => ({ Component: m.VisitaDetallePage })) },
              ]),
              soloPara(['Productor'], [
                { path: '/fincas', lazy: () => import('@/features/fincas/FincasPage').then((m) => ({ Component: m.FincasPage })) },
                { path: '/informes', lazy: () => import('@/features/informes/InformesPage').then((m) => ({ Component: m.InformesPage })) },
                { path: '/perfil', lazy: () => import('@/features/productores/PerfilPage').then((m) => ({ Component: m.PerfilPage })) },
              ]),
              soloPara(['Productor', 'AdminEmpresa'], [
                { path: '/solicitudes', lazy: () => import('@/features/solicitudes/SolicitudesPage').then((m) => ({ Component: m.SolicitudesPage })) },
                {
                  path: '/solicitudes/:id',
                  lazy: () => import('@/features/solicitudes/SolicitudDetallePage').then((m) => ({ Component: m.SolicitudDetallePage })),
                },
              ]),
              soloPara(['AdminEmpresa'], [
                { path: '/agenda', lazy: () => import('@/features/visitas/AgendaPage').then((m) => ({ Component: m.AgendaPage })) },
                { path: '/productores', lazy: () => import('@/features/productores/ProductoresPage').then((m) => ({ Component: m.ProductoresPage })) },
                {
                  path: '/productores/:id',
                  lazy: () => import('@/features/productores/ProductorDetallePage').then((m) => ({ Component: m.ProductorDetallePage })),
                },
                { path: '/tecnicos', lazy: () => import('@/features/tecnicos/TecnicosPage').then((m) => ({ Component: m.TecnicosPage })) },
                { path: '/catalogos', lazy: () => import('@/features/calidad/CatalogosPage').then((m) => ({ Component: m.CatalogosPage })) },
                { path: '/organizacion', lazy: () => import('@/features/organizacion/OrganizacionPage').then((m) => ({ Component: m.OrganizacionPage })) },
              ]),
              soloPara(['Tecnico'], [
                { path: '/visitas', lazy: () => import('@/features/visitas/MisVisitasPage').then((m) => ({ Component: m.MisVisitasPage })) },
              ]),
            ],
          },
          { path: '/inicio', element: <Portada /> },
        ],
      },
      { path: '*', element: <NotFoundPage /> },
    ],
  },
])
