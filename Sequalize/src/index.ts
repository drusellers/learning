import { Sequelize, DataTypes } from 'sequelize';

const sequelize = new Sequelize({
   dialect: 'sqlite',
  storage: ':memory:',
});

const User = sequelize.define('user', {
  id: { type: DataTypes.INTEGER, autoIncrement: true, primaryKey: true},
  username: DataTypes.STRING,
  birthday: DataTypes.DATE,
});

const Company = sequelize.define('company', {
  id: { type: DataTypes.INTEGER, autoIncrement: true, primaryKey: true},
  name: DataTypes.STRING,
});

const Membership = sequelize.define('membership', {
  role: DataTypes.STRING,
});

User.hasMany(Membership)
Company.hasMany(Membership)
User.belongsToMany(Company, { through: Membership })
Company.belongsToMany(User, { through: Membership })

await sequelize.sync({ force: true });

const jane = await User.create({
  username: 'janedoe',
  birthday: new Date(1980, 6, 20),
});

const omc = await Company.create({
  name: 'omc',
})

await jane.addCompany(omc, { through: { role: 'admin'}})

// TODO: now read it back out as User & { roles: string[] }

const x = typeof User

console.log(Object.keys(x))
console.log(typeof User)