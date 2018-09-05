using CreditAppBMG.DAL.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CreditAppBMG.DAL.Repositories
{
    public abstract class GenericRepository<TEntity> where TEntity : class
    {
        internal DatabaseContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository()
        {
            this.context = new DatabaseContext();
            this.dbSet = context.Set<TEntity>();
        }


        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual TEntity Insert(TEntity entity)
        {
            TEntity insertedEntity = dbSet.Add(entity).Entity;
            return insertedEntity;
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (this.context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            this.context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void Save(TEntity entityToSave)
        {
            // if (context.Entry(entityToSave).Property("Id").CurrentValue )
        }

        public virtual void Save()
        {
            this.context.SaveChanges();

        }

        //public IEnumerable<U> ExecuteProcedure<U>(string procedureName, params SqlParameter[] parameters)
        //{
        //    var sqlParameters = new List<object>();
        //    if (parameters != null && parameters.Any())
        //    {
        //        procedureName += " " + string.Join(
        //            ",",
        //            parameters.Select(x => string.Format("{0} = {0}", x.ParameterName)));
        //        sqlParameters.AddRange(parameters);
        //    }
        //    return this.context.Database.SqlQuery<U>(procedureName, sqlParameters.ToArray()).ToList();

        //}

        //public List<Dictionary<string, object>> ExecuteProcedure(string procedureName, params object[] parameters)
        //{

        //    var result = new List<Dictionary<string, object>>();
        //    this.context.Database.Connection.Open();
        //    using (var cmd = context.Database.Connection.CreateCommand())
        //    {
        //        cmd.CommandText = procedureName;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandTimeout = 60;
        //        foreach (var param in parameters)
        //        {
        //            cmd.Parameters.Add(param);
        //        }
        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                var row = new Dictionary<string, object>();
        //                for (var i = 0; i < reader.FieldCount; i++)
        //                {
        //                    row[reader.GetName(i)] = reader[i];
        //                }
        //                result.Add(row);
        //            }
        //        }
        //    }

        //    return result;

        //}

        public virtual bool AlreadyExists(string propertyOrField, string value)
        {
            ParameterExpression param = Expression.Parameter(typeof(TEntity));
            Expression boby = Expression.Equal(Expression.PropertyOrField(param, propertyOrField),
                  Expression.Constant(value, typeof(string)));
            Expression<Func<TEntity, bool>> filter = Expression.Lambda<Func<TEntity, bool>>(boby, param);

            var entities = Get(filter);

            if (entities != null && entities.Count() > 0)
                return true;
            else
                return false;
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate = null)
        {

            var set = context.Set<TEntity>();
            if (predicate != null)
            {
                return set.Where(predicate).ToList();
            }
            return set.ToList();

        }


    }
}
