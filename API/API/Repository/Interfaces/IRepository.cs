using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Interfaces
{
    public interface IRepository<Entity, Key> where Entity : class
    {
            IEnumerable<Entity> Get();
            Entity Get(Key key);
            int Insert(Entity e);
            int Update(Entity e, Key key);
            int Delete(Key key);
        
    }
}
