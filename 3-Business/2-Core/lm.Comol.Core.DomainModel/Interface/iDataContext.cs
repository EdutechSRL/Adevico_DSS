using System;
using NHibernate;
using System.Collections.Generic;

namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true)]
	public interface iDataContext : IDisposable
	{

		bool isDirty { get; }

		bool isInTransaction { get; }
		void Add(object item);
		void Delete(object item);
		void Save(object item);

		void SaveOrUpdate(object item);
		void Update<t>(t item);

		void Refresh<t>(t item);
		void BeginTransaction();
		void Commit();

		void Rollback();
		IList<T> GetAll<T>(FetchMode fetchplan = FetchMode.Default);
		IList<T> GetAll<T>(int pageIndex, int pageSize, FetchMode fetchplan = FetchMode.Default);
		int GetCount<T>();
		T GetById<T>(object id);



		// VERIFICARE SE MANTENERE O GENERALIZZARE
		ICriteria AddPaging(ICriteria criteria, int pageIndex, int pageSize);
		IQuery AddPaging(IQuery query, int pageIndex, int pageSize);

		IList<T> GetByCriteria<T>(ICriteria criteria, FetchMode fetchplan = FetchMode.Default);
		IList<T> GetByCriteria<T>(ICriteria criteria, int pageIndex, int pageSize, FetchMode fetchplan = FetchMode.Default);
		T GetByCriteriaUnique<T>(ICriteria criteria, FetchMode fetchplan = FetchMode.Default);
		IList<T> GetByQuery<T>(IQuery query, FetchMode fetchplan = FetchMode.Default);
		IList<T> GetByQuery<T>(IQuery query, int pageIndex, int pageSize, FetchMode fetchplan = FetchMode.Default);

		T GetByQueryUnique<T>(IQuery query, FetchMode fetchplan = FetchMode.Default);
		int GetCount<T>(ICriteria criteria);
		ICriteria CreateCriteria<T>();
		IQuery CreateQuery(string query);
		ISession GetCurrentSession();
	}
}