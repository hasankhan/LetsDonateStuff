using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LetsDonateStuff.DAL;

namespace LetsDonateStuff.Services
{
    public abstract class Service<TRepository> where TRepository:IRepository
    {
        Func<TRepository> repositoryFactory;

        protected Service(Func<TRepository> repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
        }

        protected T GetItem<T>(Func<TRepository, T> action)
        {
            using (var repository = repositoryFactory())
                return action(repository);
        }

        protected void UpdateItem(Action<TRepository> action)
        {
            using (var repository = repositoryFactory())
            {
                action(repository);
                repository.Save();
            }
        }

        protected T GetAndUpdate<T>(Func<TRepository, T> action)
        {
            T result = default(T);
            UpdateItem(repository =>
            {
                result = action(repository);
            });
            return result;
        }
    }
}