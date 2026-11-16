import { useQuery } from '@tanstack/react-query'
import { useState } from 'react'
import { QueryView } from '@/shared/ui/feedback'
import { Segmented } from '@/shared/ui/layout'
import { calidadApi, calidadKeys } from './api'
import { ListaAplicaciones, ListaInspecciones, ListaRecomendaciones } from './Registros'

type Pestana = 'inspecciones' | 'recomendaciones' | 'aplicaciones'

export function HistorialCultivo({ cultivoId }: { cultivoId: number }) {
  const [pestana, setPestana] = useState<Pestana>('inspecciones')

  const inspecciones = useQuery({
    queryKey: calidadKeys.inspecciones(cultivoId),
    queryFn: () => calidadApi.inspecciones(cultivoId),
    enabled: pestana === 'inspecciones',
  })

  const recomendaciones = useQuery({
    queryKey: calidadKeys.recomendaciones(cultivoId),
    queryFn: () => calidadApi.recomendaciones(cultivoId),
    enabled: pestana === 'recomendaciones',
  })

  const aplicaciones = useQuery({
    queryKey: calidadKeys.aplicaciones(cultivoId),
    queryFn: () => calidadApi.aplicaciones(cultivoId),
    enabled: pestana === 'aplicaciones',
  })

  return (
    <div className="animate-fade border-t border-line-soft bg-paper/50 px-5 py-5 md:px-6">
      <Segmented
        etiqueta="Historial del cultivo"
        valor={pestana}
        alCambiar={setPestana}
        opciones={[
          { valor: 'inspecciones', etiqueta: 'Inspecciones' },
          { valor: 'recomendaciones', etiqueta: 'Recomendaciones' },
          { valor: 'aplicaciones', etiqueta: 'Aplicaciones' },
        ]}
      />
      <div className="mt-3">
        {pestana === 'inspecciones' && (
          <QueryView query={inspecciones} filas={2}>
            {(items) => <ListaInspecciones items={items} />}
          </QueryView>
        )}
        {pestana === 'recomendaciones' && (
          <QueryView query={recomendaciones} filas={2}>
            {(items) => <ListaRecomendaciones items={items} />}
          </QueryView>
        )}
        {pestana === 'aplicaciones' && (
          <QueryView query={aplicaciones} filas={2}>
            {(items) => <ListaAplicaciones items={items} />}
          </QueryView>
        )}
      </div>
    </div>
  )
}
