using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data;

namespace LetsDonateStuff.DAL
{
    public abstract class Repository: IDisposable, LetsDonateStuff.DAL.IRepository
    {
        DonationsContext context = new DonationsContext();

        protected DonationsContext Context
        {
            get { return context; }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        protected void Update<T>(DbSet<T> set, T item) where T : class
        {
            if (context.Entry(item).State == EntityState.Detached)
            {
                set.Attach(item);
                context.Entry(item).State = EntityState.Modified;
            }
        }
    }
}