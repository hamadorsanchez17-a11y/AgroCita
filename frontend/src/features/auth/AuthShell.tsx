import { CalendarCheck, CircleCheck, Clock, UserRound } from 'lucide-react'
import { motion, useMotionValue, useSpring, useTransform } from 'motion/react'
import type { MouseEvent, ReactNode } from 'react'
import { Contours } from '@/shared/ui/layout'
import { Marca } from '@/shared/ui/Marca'

interface AuthShellProps {
  children: ReactNode
  frase: ReactNode
  nota: string
}

const salida = [0.22, 1, 0.36, 1] as const

const CULTIVOS = ['Café', 'Maíz', 'Frijol', 'Cacao', 'Plátano', 'Hortalizas', 'Arroz', 'Sorgo', 'Aguacate', 'Palma']

function Marquesina() {
  const fila = [...CULTIVOS, ...CULTIVOS]

  return (
    <div className="relative overflow-hidden [mask-image:linear-gradient(90deg,transparent,black_12%,black_88%,transparent)]" aria-hidden>
      <div className="animate-marquesina flex w-max gap-3">
        {fila.map((cultivo, indice) => (
          <span
            key={`${cultivo}-${indice}`}
            className="flex items-center gap-2 rounded-full border border-paper/10 bg-paper/[0.04] px-3.5 py-1.5 font-mono text-[11px] tracking-[0.1em] text-paper/60 uppercase"
          >
            <span className="size-1 rounded-full bg-ochre/70" />
            {cultivo}
          </span>
        ))}
      </div>
    </div>
  )
}

export function AuthShell({ children, frase, nota }: AuthShellProps) {
  const puntoX = useMotionValue(0)
  const puntoY = useMotionValue(0)
  const suaveX = useSpring(puntoX, { stiffness: 70, damping: 20 })
  const suaveY = useSpring(puntoY, { stiffness: 70, damping: 20 })
  const cercaX = useTransform(suaveX, (valor) => valor * 28)
  const cercaY = useTransform(suaveY, (valor) => valor * 28)
  const lejosX = useTransform(suaveX, (valor) => valor * -14)
  const lejosY = useTransform(suaveY, (valor) => valor * -14)
  const fondoX = useTransform(suaveX, (valor) => valor * -40)
  const fondoY = useTransform(suaveY, (valor) => valor * -40)

  const alMover = (evento: MouseEvent<HTMLElement>) => {
    const caja = evento.currentTarget.getBoundingClientRect()
    puntoX.set((evento.clientX - caja.left) / caja.width - 0.5)
    puntoY.set((evento.clientY - caja.top) / caja.height - 0.5)
  }

  return (
    <div className="grid min-h-dvh lg:grid-cols-[minmax(0,1.1fr)_minmax(0,1fr)]">
      <aside
        onMouseMove={alMover}
        className="relative hidden overflow-hidden bg-forest-deep text-paper lg:flex lg:flex-col lg:justify-between lg:gap-6 lg:p-12 xl:p-14"
      >
        <div className="reticula pointer-events-none absolute inset-0" aria-hidden />
        <div className="grano pointer-events-none absolute inset-0 opacity-70" aria-hidden />
        <motion.div style={{ x: fondoX, y: fondoY }} className="pointer-events-none absolute inset-0" aria-hidden>
          <div className="absolute top-[8%] right-[6%] size-[420px] rounded-full bg-ochre/[0.13] blur-[90px]" />
          <div className="absolute bottom-[-10%] left-[-10%] size-[460px] rounded-full bg-moss/25 blur-[100px]" />
          <Contours animado className="absolute -top-48 -right-60 size-[860px] text-ochre/25" />
          <div className="absolute -bottom-80 -left-72 size-[780px] rotate-45">
            <Contours animado className="size-full text-moss/40" />
          </div>
        </motion.div>

        <motion.div
          className="relative"
          initial={{ opacity: 0, y: -10 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.6, ease: salida }}
        >
          <Marca claro />
        </motion.div>

        <div className="pointer-events-none relative hidden h-[236px] w-[400px] shrink-0 self-end xl:block [@media(max-height:860px)]:hidden" aria-hidden>
          <motion.div style={{ x: cercaX, y: cercaY }} className="absolute top-0 left-0 w-[284px]">
            <motion.div
              className="animate-flota rounded-2xl border border-paper/12 bg-paper/[0.07] p-4 shadow-[0_30px_60px_-20px_rgb(0_0_0/0.6)] backdrop-blur-xl"
              initial={{ opacity: 0, y: 30, rotate: -4 }}
              animate={{ opacity: 1, y: 0, rotate: -3 }}
              transition={{ duration: 1, ease: salida, delay: 0.6 }}
            >
              <div className="flex items-center justify-between">
                <span className="font-mono text-[10px] tracking-[0.14em] text-paper/50 uppercase">Próxima visita</span>
                <span className="flex items-center gap-1.5 rounded-full bg-ochre px-2 py-0.5 font-mono text-[9.5px] font-medium tracking-wider text-forest-deep uppercase">
                  <CalendarCheck className="size-3" />
                  Confirmada
                </span>
              </div>
              <div className="mt-4 flex items-center gap-4">
                <div className="grid size-14 shrink-0 place-items-center rounded-xl bg-paper text-forest-deep">
                  <div className="text-center leading-none">
                    <p className="font-mono text-[9px] tracking-widest text-ochre-deep uppercase">Mié</p>
                    <p className="font-display text-2xl font-semibold">18</p>
                  </div>
                </div>
                <div className="min-w-0">
                  <p className="font-display text-lg leading-tight font-semibold tracking-tight">Finca El Encino</p>
                  <p className="mt-1 flex items-center gap-3 text-[12px] text-paper/65">
                    <span className="flex items-center gap-1 font-mono">
                      <Clock className="size-3 text-ochre" />
                      09:00
                    </span>
                    <span className="flex items-center gap-1">
                      <UserRound className="size-3 text-ochre" />
                      Técnico asignado
                    </span>
                  </p>
                </div>
              </div>
            </motion.div>
          </motion.div>

          <motion.div style={{ x: lejosX, y: lejosY }} className="absolute top-[110px] right-0 w-[220px]">
            <motion.div
              className="animate-flota rounded-2xl border border-paper/12 bg-paper/[0.06] p-4 shadow-[0_30px_60px_-20px_rgb(0_0_0/0.6)] backdrop-blur-xl [animation-delay:-3s]"
              initial={{ opacity: 0, y: 30, rotate: 4 }}
              animate={{ opacity: 1, y: 0, rotate: 2 }}
              transition={{ duration: 1, ease: salida, delay: 0.8 }}
            >
              <p className="font-mono text-[10px] tracking-[0.14em] text-paper/50 uppercase">Calidad del lote</p>
              <div className="mt-2 flex items-end justify-between gap-4">
                <p className="font-display text-4xl leading-none font-semibold tracking-tight">
                  92<span className="text-xl text-ochre">%</span>
                </p>
                <div className="flex h-10 items-end gap-1">
                  {[38, 52, 46, 68, 74, 88, 92].map((alto, indice) => (
                    <motion.span
                      key={indice}
                      className="w-2 rounded-sm bg-ochre/80"
                      initial={{ height: 0 }}
                      animate={{ height: `${alto}%` }}
                      transition={{ duration: 0.8, ease: salida, delay: 1.1 + indice * 0.07 }}
                    />
                  ))}
                </div>
              </div>
            </motion.div>
          </motion.div>

          <motion.div
            className="absolute bottom-0 left-6 inline-flex items-center gap-2 rounded-full border border-paper/12 bg-forest/80 px-3.5 py-2 text-[12px] font-medium shadow-lift backdrop-blur-xl"
            initial={{ opacity: 0, scale: 0.8 }}
            animate={{ opacity: 1, scale: 1 }}
            transition={{ type: 'spring', stiffness: 300, damping: 20, delay: 1.4 }}
          >
            <CircleCheck className="size-4 text-ochre" />
            Informe técnico entregado
          </motion.div>
        </div>

        <div className="relative max-w-[600px]">
          <motion.p
            className="eyebrow flex items-center gap-2 !text-ochre"
            initial={{ opacity: 0, y: 10 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.6, ease: salida, delay: 0.15 }}
          >
            <span className="h-px w-8 bg-ochre" />
            Asistencia técnica agrícola · Honduras
          </motion.p>
          <motion.h1
            className="mt-5 text-[3.5rem] leading-[0.95] font-semibold text-balance xl:text-[4.1rem]"
            initial={{ opacity: 0, y: 40, filter: 'blur(12px)' }}
            animate={{ opacity: 1, y: 0, filter: 'blur(0px)' }}
            transition={{ duration: 1.1, ease: salida, delay: 0.25 }}
          >
            {frase}
          </motion.h1>
          <motion.p
            className="mt-7 max-w-md text-[16px] leading-relaxed text-paper/65"
            initial={{ opacity: 0, y: 14 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8, ease: salida, delay: 0.45 }}
          >
            {nota}
          </motion.p>
        </div>

        <motion.div
          className="relative flex flex-col gap-6"
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          transition={{ duration: 1, delay: 0.7 }}
        >
          <Marquesina />
          <dl className="grid max-w-lg grid-cols-3 gap-6 border-t border-paper/10 pt-5 font-mono text-[10.5px] tracking-[0.12em] text-paper/45 uppercase">
            <div>
              <dt>Latitud</dt>
              <dd className="mt-1 text-[13px] tracking-normal text-paper/90">14.0723° N</dd>
            </div>
            <div>
              <dt>Longitud</dt>
              <dd className="mt-1 text-[13px] tracking-normal text-paper/90">87.1921° O</dd>
            </div>
            <div>
              <dt>Zona</dt>
              <dd className="mt-1 text-[13px] tracking-normal text-paper/90">Honduras</dd>
            </div>
          </dl>
        </motion.div>
      </aside>

      <main className="relative flex flex-col px-6 py-10 sm:px-10 lg:justify-center lg:px-16 xl:px-24">
        <div className="mb-10 lg:hidden">
          <Marca />
        </div>
        <motion.div
          className="relative mx-auto w-full max-w-[440px]"
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.8, ease: salida, delay: 0.2 }}
        >
          {children}
        </motion.div>
      </main>
    </div>
  )
}
