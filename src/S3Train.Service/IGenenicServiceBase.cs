using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using S3Train.Domain;
using X.PagedList;

namespace S3Train
{
    public interface IGenenicServiceBase<T> where T : EntityBase
    {
        IQueryable<T> Query();
        IList<T> GetAll();
        IList<T> GetAll(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);
        IEnumerable<T> Gets(Expression<Func<T, bool>> predicate);
        IEnumerable<T> Gets(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);
        IPagedList<T> GetAllPaged(int? pageIndex, int pageSize = 20, Expression<Func<T, bool>> where = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
        T GetById(string id);
        T Get(Expression<Func<T, bool>> predicate);
        T Get(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);
        T Insert(T item);
        void Insert(List<T> items);
        T Update(T item);
        T Remove(T item);
        void Remove(Expression<Func<T, bool>> predicate);

        IPagedList<T> GetAllPaged(IQueryable<T> list, int? pageIndex, int pageSize = 20, Expression<Func<T,
            bool>> where = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
        T Get(IQueryable<T> list, Expression<Func<T, bool>> predicate);


    }
}