using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ANGenericRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        IDataContextFactory _dataContextFactory;
        public Repository(IDataContextFactory dataContextFactory)
        {
            _dataContextFactory = dataContextFactory;
        }//End Repository Constructor

        public OperationResult AddItem(T entity)
        {
            OperationResult opResult = OperationResult.GetInstance();
            try
            {
                GetTable.InsertOnSubmit(entity);
                
                this._dataContextFactory.SaveAll();
                opResult.ObjectId = PrimaryKeyName;
            }
            catch (Exception ex)
            {
                opResult.IsFailed = true;
                opResult.ErrorMessage = ex.Message + ex.StackTrace;
            }
            return opResult;
        }

        public OperationResult UpdateItem(T entity)
        {
            OperationResult opResult = OperationResult.GetInstance();
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("object should not be null");
                }
                ChangeSet changes = _dataContextFactory.Context.GetChangeSet();
                _dataContextFactory.SaveAll();
                changes = null;
            }
            catch (Exception ex)
            {
                opResult.IsFailed = true;
                opResult.ErrorMessage = ex.Message + ex.StackTrace;
            }
            return opResult;
        }

        public OperationResult DeleteItem(T entity)
        {
            OperationResult opResult = OperationResult.GetInstance();
            try
            {
                _dataContextFactory.Context.GetTable<T>().DeleteOnSubmit(entity);
                _dataContextFactory.SaveAll();
            }
            catch (Exception ex)
            {
                opResult.IsFailed = true;
                opResult.ErrorMessage = ex.Message + ex.StackTrace;
            }
            return opResult;
        }//End Method

        #region Get List

        public List<T> GetAll()
        {
            return GetTable.ToList();
        }

        public List<T> GetAll(int currPage, int pageSize)
        {
            return GetTable.Skip((currPage - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<T> GetAllWithOrderByAsc(Func<T, object> orderByAscField, int currPage, int pageSize)
        {
            return GetTable.OrderBy(orderByAscField).Skip((currPage - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<T> GetAllWithOrderByDesc(Func<T, object> orderByDescField, int currPage, int pageSize)
        {
            return GetTable.OrderByDescending(orderByDescField).Skip((currPage - 1) * pageSize).Take(pageSize).ToList();
        }
        #endregion


        #region Get List By Query

        public List<T> GetItemListByQuery(Func<T, bool> exp)
        {
            return GetTable.Where(exp).ToList();
        }

        public List<T> GetItemListByQuery(Func<T, bool> exp, int currPage, int pageSize)
        {
            return GetTable.Where(exp).Skip((currPage - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<T> GetItemListByQueryWithOrderByAsc(Func<T, bool> exp, Func<T, object> orderByAscField, int currPage, int pageSize)
        {
            return GetTable.Where(exp).OrderBy(orderByAscField).Skip((currPage - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<T> GetItemListByQueryWithOrderByDesc(Func<T, bool> exp, Func<T, object> orderByDescField, int currPage, int pageSize)
        {
            return GetTable.Where(exp).OrderByDescending(orderByDescField).Skip((currPage - 1) * pageSize).Take(pageSize).ToList();
        }


        #endregion


        public T GetSingle(Func<T, bool> exp)
        {
            return GetTable.FirstOrDefault(exp);
        }

        #region Properties

        private string PrimaryKeyName
        {
            get { return TableMetadata.RowType.IdentityMembers[0].Name; }
        }

        private System.Data.Linq.Table<T> GetTable
        {
            get { return _dataContextFactory.Context.GetTable<T>(); }
        }

        private System.Data.Linq.Mapping.MetaTable TableMetadata
        {
            get { return _dataContextFactory.Context.Mapping.GetTable(typeof(T)); }
        }

        private System.Data.Linq.Mapping.MetaType ClassMetadata
        {
            get { return _dataContextFactory.Context.Mapping.GetMetaType(typeof(T)); }
        }

        #endregion


        public int GetItemCountByQuery(Func<T, bool> exp)
        {
            return GetTable.Count(exp);
        }


        public int GetItemCount()
        {
            return GetTable.Count();
        }
    }//End Class

}//End Namespace
