import { Injectable, Logger } from '@nestjs/common'
import { ConfigService } from '@nestjs/config'

@Injectable()
export class AppService {
  private readonly logger = new Logger(AppService.name)

  constructor(private readonly configService: ConfigService) {}

  getHello(): string {
    this.logger.log({ a: 1 }, 'hello')
    const name = this.configService.get<string>('NAME') ?? 'World'
    return `Hello ${name}!`
  }
}
