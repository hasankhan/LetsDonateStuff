using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LinqKit;
using System.Linq.Expressions;

namespace LetsDonateStuff.DAL
{
    public class LinqHelper
    {
        public static IQueryable<T> SearchByKeywords<T>(IQueryable<T> query, string keywords, Func<string, Expression<Func<T, bool>>> conditionSelector)
        {
            if (!String.IsNullOrEmpty(keywords))
            {
                string[] tokens = keywords.Split();
                var predicate = tokens.Aggregate(PredicateBuilder.False<T>(), (p, keyword) =>
                {
                    var condition = conditionSelector(keyword);
                    return p.Or(condition);
                });
                query = query.AsExpandable().Where(predicate);
            }
            return query;
        }
    }
}