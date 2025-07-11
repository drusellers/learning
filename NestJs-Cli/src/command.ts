import { AppModule } from './app.module';
import { CommandFactory } from 'nest-commander';
import { ConsoleLogger } from '@nestjs/common';

async function bootstrap() {
  await CommandFactory.run(AppModule, {
    logger: new ConsoleLogger(),
    errorHandler: (err) => {
      console.error(err);
      process.exit(1); // this could also be a 0 depending on how you want to handle the exit code
    },
  });
}

bootstrap().catch(console.dir);
