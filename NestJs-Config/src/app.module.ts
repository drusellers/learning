import { Module, OnModuleDestroy, OnModuleInit } from '@nestjs/common'
import { AppController } from './app.controller'
import { AppService } from './app.service'
import { ConfigModule } from '@nestjs/config'
import { LoggerModule } from 'nestjs-pino'
import { v4 } from 'uuid'

@Module({
  imports: [
    LoggerModule.forRoot({
      pinoHttp: {
        genReqId: (request) => request.headers['x-correlation-id'] || v4(),
        serializers: {
          req: (req) => ({
            id: req.id,
            method: req.method,
            url: req.url,
            // user_id: (req.user as Auth0RequestUser)?.user?.id,
          }),
          res: (res) => ({
            statusCode: res.statusCode,
            contentLength: res['content-length'],
          }),
        },
      },
    }),
    ConfigModule.forRoot({
      // order matters - first file wins
      envFilePath: ['.env.local', '.env'],
    }),
  ],
  controllers: [AppController],
  providers: [AppService],
})
export class AppModule implements OnModuleInit, OnModuleDestroy {
  onModuleInit() {
    console.log('onModuleInit')
  }

  onModuleDestroy() {
    console.log('onModuleDestroy')
  }
}
