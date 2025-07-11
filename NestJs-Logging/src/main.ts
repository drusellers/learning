import { NestFactory } from '@nestjs/core'
import { AppModule } from './app.module'
import { Logger, LoggerErrorInterceptor } from 'nestjs-pino'

async function bootstrap() {
  const app = await NestFactory.create(AppModule, { bufferLogs: true })
  app.useLogger(app.get(Logger))
  app.flushLogs()
  process.on('unhandledRejection', function (err) {
    app.get(Logger).log(err, 'Unhandled Rejection')
  })
  app.useGlobalInterceptors(new LoggerErrorInterceptor())
  await app.listen(process.env.PORT ?? 3000)
}
bootstrap().then(console.error)
