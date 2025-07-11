import { Args, Query, Resolver } from '@nestjs/graphql';
import { Document } from './document.model';
import { DocumentArgs } from './document.args';

@Resolver((of) => Document)
export class DocumentResolver {
  constructor() {}

  @Query((returns) => Document)
  recipe(@Args('id') id: string): Promise<Document> {
    return Promise.resolve(new Document());
  }

  @Query((returns) => [Document])
  recipes(@Args() documentArgs: DocumentArgs): Promise<Document[]> {
    return Promise.resolve([new Document()]);
  }
}
