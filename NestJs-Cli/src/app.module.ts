import { Module, OnModuleDestroy, OnModuleInit } from '@nestjs/common';
import { AppController } from './app.controller';
import { AppService } from './app.service';
import { BasicCommand } from './sample.command';

@Module({
  imports: [],
  controllers: [AppController],
  providers: [AppService, BasicCommand],
})
export class AppModule implements OnModuleInit, OnModuleDestroy {
  onModuleInit() {
    console.log('onModuleInit');
  }

  onModuleDestroy() {
    console.log('onModuleDestroy');
  }
}
