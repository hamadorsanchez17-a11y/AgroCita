import { useQuery } from '@tanstack/react-query'
import { Check, Copy, RefreshCw } from 'lucide-react'
import { useState } from 'react'
import { useAccion } from '@/shared/forms/mutations'
import { formato } from '@/shared/lib/format'
import { EstadoBadge } from '@/shared/ui/Badge'
import { Button } from '@/shared/ui/Button'
import { QueryView } from '@/shared/ui/feedback'
import { Card, CardHeader, Contours, Dato, PageHeader } from '@/shared/ui/layout'
import { Modal, ModalActions } from '@/shared/ui/Modal'
import { organizacionApi, organizacionKeys } from './api'

function CodigoInvitacion({ codigo }: { codigo: string }) {
  const [copiado, setCopiado] = useState(false)
  const [confirmando, setConfirmando] = useState(false)

  const regenerar = useAccion({
    mutationFn: organizacionApi.regenerarCodigo,
    invalidar: [organizacionKeys.empresa],
    mensaje: 'Código de invitación renovado.',
    alExito: () => setConfirmando(false),
  })

  const copiar = async () => {
    await navigator.clipboard.writeText(codigo)
    setCopiado(true)
    window.setTimeout(() => setCopiado(false), 1800)
  }

  return (
    <section className="relative overflow-hidden rounded-2xl bg-forest px-6 py-7 text-paper shadow-lift md:px-8">
      <Contours className="pointer-events-none absolute -top-40 -right-32 size-[460px] text-moss/30" />
      <div className="relative">
        <p className="eyebrow !text-ochre">Código de invitación</p>
        <p className="mt-3 font-mono text-[1.75rem] tracking-[0.22em] break-all sm:text-4xl">{codigo}</p>
        <p className="mt-3 max-w-md text-sm text-paper/70">
          Entrégalo a tus productores. Lo escriben al crear su cuenta y quedan vinculados a tu organización.
        </p>
        <div className="mt-5 flex flex-wrap gap-2.5">
          <Button variante="acento" icono={copiado ? <Check className="size-4" /> : <Copy className="size-4" />} onClick={() => void copiar()}>
            {copiado ? 'Copiado' : 'Copiar código'}
          </Button>
          <Button
            variante="fantasma"
            className="!text-paper/80 hover:!bg-paper/10 hover:!text-paper"
            icono={<RefreshCw className="size-4" />}
            onClick={() => setConfirmando(true)}
          >
            Generar uno nuevo
          </Button>
        </div>
      </div>

      <Modal
        abierto={confirmando}
        alCerrar={() => setConfirmando(false)}
        ancho="sm"
        titulo="¿Generar un código nuevo?"
        descripcion="El código actual dejará de funcionar. Los productores ya registrados no se ven afectados."
      >
        <ModalActions>
          <Button variante="fantasma" onClick={() => setConfirmando(false)}>
            Cancelar
          </Button>
          <Button cargando={regenerar.isPending} onClick={() => regenerar.mutate()}>
            Generar código
          </Button>
        </ModalActions>
      </Modal>
    </section>
  )
}

export function OrganizacionPage() {
  const empresa = useQuery({ queryKey: organizacionKeys.empresa, queryFn: organizacionApi.empresa })
  const suscripcion = useQuery({ queryKey: organizacionKeys.suscripcion, queryFn: organizacionApi.suscripcion })

  return (
    <>
      <PageHeader
        seccion="Administración · Cuenta"
        titulo={empresa.data?.nombre ?? 'Organización'}
        descripcion="Los datos de tu organización, su código de invitación y el plan contratado."
      />

      <QueryView query={empresa}>
        {(datos) => (
          <div className="stagger flex flex-col gap-6">
            <CodigoInvitacion codigo={datos.codigoInvitacion} />

            <div className="grid gap-6 lg:grid-cols-2">
              <Card>
                <CardHeader titulo="Datos de la organización" numero="01" />
                <dl className="grid gap-5 px-5 py-5 sm:grid-cols-2 md:px-6">
                  <Dato etiqueta="RTN" mono>
                    {datos.rtn}
                  </Dato>
                  <Dato etiqueta="Teléfono" mono>
                    {datos.telefono}
                  </Dato>
                  <div className="sm:col-span-2">
                    <Dato etiqueta="Correo">{datos.email}</Dato>
                  </div>
                  <div className="sm:col-span-2">
                    <Dato etiqueta="Dirección">{datos.direccion ?? '—'}</Dato>
                  </div>
                </dl>
              </Card>

              <Card>
                <CardHeader titulo="Suscripción" numero="02" />
                <div className="px-5 py-5 md:px-6">
                  <QueryView query={suscripcion} filas={2}>
                    {(plan) => (
                      <>
                        <div className="flex items-start justify-between gap-4">
                          <div>
                            <p className="eyebrow">Plan</p>
                            <p className="mt-1 font-display text-4xl">{plan.plan.nombre}</p>
                          </div>
                          <EstadoBadge estado={plan.estado} />
                        </div>
                        <dl className="mt-6 grid grid-cols-2 gap-5 border-t border-line-soft pt-5">
                          <Dato etiqueta="Desde" mono>
                            {formato.fecha(plan.fechaInicio)}
                          </Dato>
                          <Dato etiqueta="Hasta" mono>
                            {plan.fechaFin ? formato.fecha(plan.fechaFin) : 'Sin vencimiento'}
                          </Dato>
                        </dl>
                        {plan.estado !== 'Activa' && (
                          <p className="mt-5 rounded-xl border border-clay/40 bg-clay-soft/40 px-4 py-3 text-sm text-clay">
                            Tu organización no puede operar hasta regularizar la suscripción. Contacta al equipo de AgroCita.
                          </p>
                        )}
                      </>
                    )}
                  </QueryView>
                </div>
              </Card>
            </div>
          </div>
        )}
      </QueryView>
    </>
  )
}
