using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Common
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _dbContext;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Getters

        public virtual TEntity GetById(int id)
        {
            return _dbContext.Set<TEntity>().Find(id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().AsEnumerable();
        }

        public virtual IEnumerable<TEntity> GetByPredicate(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>()
                   .Where(predicate)
                   .AsEnumerable();
        }

        #endregion

        #region CRUD

        public void Add(TEntity entity)
        {
            using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.Set<TEntity>().Add(entity);
                    _dbContext.SaveChanges();

                    transaction.Commit();
                }
                catch (DataException ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    throw ex;
                }
            }
        }

        public void Edit(TEntity entity)
        {
            using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.Entry(entity).State = EntityState.Modified;
                    _dbContext.SaveChanges();

                    transaction.Commit();
                }
                catch (DataException ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    throw ex;
                }
            }
        }

        public void Delete(TEntity entity)
        {
            using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.Set<TEntity>().Remove(entity);
                    _dbContext.SaveChanges();

                    transaction.Commit();
                }
                catch (DataException ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    throw ex;
                }
            }            
        }

        #endregion

        #region Functions

        // funcion 
        public int ExecuteFunction(string function, params SqlParameter[] parameters) 
        {
            try
            { 
                string formattedParameters = FormatSQLParameters(parameters);
                function = function + " " + formattedParameters;

                var result = _dbContext.Database.ExecuteSqlCommand(function, parameters);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // funcion to list 
        public IEnumerable<TEntity> FunctionToList(string function, params SqlParameter[] parameters) 
        {
            try
            {
                string formattedParameters = FormatSQLParameters(parameters);
                function = function + " " + formattedParameters;

                var result = _dbContext.Set<TEntity>().FromSql(function, parameters).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Funcion que da formato a los parametros
        private string FormatSQLParameters(params SqlParameter[] parameters)
        {
            int counter = 1;
            string formattedParameters = "";

            foreach (var item in parameters)
            {
                if (parameters.Length <= counter)
                {
                    formattedParameters = formattedParameters + item.ParameterName.ToString();
                }
                else
                {
                    formattedParameters = formattedParameters + item.ParameterName.ToString() + ",";
                }
                counter++;
            }

            return formattedParameters;
        }

        #endregion






    }

}
