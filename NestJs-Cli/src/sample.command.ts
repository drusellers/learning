import { Command, CommandRunner } from 'nest-commander';

@Command({ name: 'basic', description: 'A parameter parse' })
export class BasicCommand extends CommandRunner {
  async run(): Promise<void> {
    console.log('hello from Basic');
    return Promise.resolve();
  }
}
