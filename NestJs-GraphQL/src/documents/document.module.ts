import { Module } from '@nestjs/common';
import { DocumentResolver } from './document.resolver';

@Module({
  imports: [],
  controllers: [],
  providers: [DocumentResolver],
})
export class DocumentModule {}
