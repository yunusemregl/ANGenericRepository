using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ANGenericRepository
{
    public interface IRepository<T> where T : class
    {
        OperationResult AddItem(T item);

        OperationResult UpdateItem(T item);

        OperationResult DeleteItem(T item);

        List<T> GetAll();

        List<T> GetAll(int currPage, int pageSize);
        List<T> GetAllWithOrderByDesc(Func<T, object> orderByDescField, int currPage, int pageSize);
        List<T> GetAllWithOrderByAsc(Func<T, object> orderByAscField, int currPage, int pageSize);

        List<T> GetItemListByQuery(Func<T, bool> exp, int currPage, int pageSize);
        List<T> GetItemListByQueryWithOrderByDesc(Func<T, bool> exp, Func<T, object> orderByDescField, int currPage, int pageSize);
        List<T> GetItemListByQueryWithOrderByAsc(Func<T, bool> exp, Func<T, object> orderByAscField, int currPage, int pageSize);

        List<T> GetItemListByQuery(Func<T, bool> exp);
        T GetSingle(Func<T, bool> exp);

        int GetItemCountByQuery(Func<T, bool> exp);

        int GetItemCount();

    }
}
