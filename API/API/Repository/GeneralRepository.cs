using API.Context;
using API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository
{
    public class GeneralRepository<Context, Entity, Key> : IRepository<Entity, Key>
        where Context : MyContext
        where Entity : class
    {
        private readonly MyContext myContext;
        private readonly DbSet<Entity> entities;

        public GeneralRepository(MyContext myContext)
        {
            this.myContext = myContext;
            entities = myContext.Set<Entity>();
        }

        public int Delete(Key key)
        {
            var find = entities.Find(key);
            entities.Remove(find);
            var delete = myContext.SaveChanges();
            return delete;
        }

        public IEnumerable<Entity> Get()
        {
            return entities.ToList();
        }

        public Entity Get(Key key)
        {
            return entities.Find(key);
        }

        public int Insert(Entity e)
        {
            entities.Add(e);
            var insert = myContext.SaveChanges();
            return insert;
        }

        public int Update(Entity e, Key key)
        {
            myContext.Entry(e).State = EntityState.Modified;
            myContext.Update(e);
            return myContext.SaveChanges();
        }
    }
}
