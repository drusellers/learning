import { Table, Column, Model, BelongsToMany, DataType, Sequelize, ForeignKey, HasMany, HasOne, BelongsTo } from 'sequelize-typescript'

@Table
export class User extends Model {    
    @Column(DataType.STRING)
    declare username: string

    @Column(DataType.DATE)
    declare birthday: Date

    @BelongsToMany(()=> Company, () => Membership)
    declare companies: Array<Company & { Membership: Membership }>
}

@Table
export class Company extends Model {
    
    @Column(DataType.STRING)
    declare name: string

    @BelongsToMany(()=> User, {
        through: () => Membership
    })
    declare users: User[]
}

@Table
export class Role extends Model {
  @Column({
    type: DataType.STRING,
    allowNull: false,
  })
  declare name: string
}

@Table
export class Membership extends Model {
    
    @ForeignKey(() => User)
    declare userId: User

    @ForeignKey(() => Company)
    declare companyId: Company

    @HasOne(() => Role)
    declare roles: Role[]
}

const sequelize = new Sequelize({
  database: 'some_db',
  dialect: 'sqlite',
  storage: ':memory:',
  models: [User, Company, Membership], // or [Player, Team],
});


await sequelize.sync({ force: true });

const dru = await User.create({
     username: 'drusellers',
      birthday: new Date()
    })

const omc = await Company.create({
    name : 'omc'
})

const admin = await Role.create({
    name: 'Admin'
})

const ceo = await Membership.create({
    userId: dru.id,
    companyId: omc.id,
    role: admin
})


const f = await User.findOne({ 
    where: { 
        username: 'drusellers',

     },
     include: [Company]
})

console.log(JSON.stringify(f?.toJSON(), null, 2))
/*
{
  id: 1,
  username: 'drusellers',
  birthday: 2025-06-28T14:32:09.872Z,
  createdAt: 2025-06-28T14:32:09.872Z,
  updatedAt: 2025-06-28T14:32:09.872Z,
  companies: [
    {
      id: 1,
      name: 'omc',
      createdAt: 2025-06-28T14:32:09.874Z,
      updatedAt: 2025-06-28T14:32:09.874Z,
      Membership: [Object]
    }
  ]
}
*/