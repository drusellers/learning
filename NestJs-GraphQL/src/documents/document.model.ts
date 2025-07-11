import { Field, ID, ObjectType } from '@nestjs/graphql';

@ObjectType({ description: 'document ' })
export class Document {
  @Field((_type) => ID)
  id: string;

  @Field()
  title: string;

  @Field({ nullable: true })
  description?: string;

  @Field()
  creationDate: Date;

  @Field((_type) => [String])
  ingredients: string[];
}
