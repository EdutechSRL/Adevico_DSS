using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using System.Collections;
using NHibernate.Collection.Generic;
using NHibernate.Collection;

namespace lm.Comol.Modules.Standard.ProjectManagement.Domain
{
    public class StoragePmActivityBag : NHibernate.UserTypes.IUserCollectionType
    {
        public object Instantiate(int anticipatedSize)
        {
            return new List<PmActivity>();
        }

        public IPersistentCollection Instantiate(ISessionImplementor session, ICollectionPersister persister)
        {
            return new PersistentStoragePmActivityBag(session);
        }

        public IPersistentCollection Wrap(ISessionImplementor session, object collection)
        {
            return new PersistentStoragePmActivityBag(session, (IList<PmActivity>)collection);
        }

        public IEnumerable GetElements(object collection)
        {
            return (IEnumerable)collection;
        }

        public bool Contains(object collection, object entity)
        {
            return ((IList<PmActivity>)collection).Contains((PmActivity)entity);
        }

        public object IndexOf(object collection, object entity)
        {
            return ((IList<PmActivity>)collection).IndexOf((PmActivity)entity);
        }

        public object ReplaceElements(object original, object target, ICollectionPersister persister, object owner, IDictionary copyCache, ISessionImplementor session)
        {
            var result = (IList<PmActivity>)target;
            result.Clear();

            foreach (var box in (IEnumerable)original)
                result.Add((PmActivity)box);

            return result;
        }
    }

    public class PersistentStoragePmActivityBag : PersistentGenericBag<PmActivity>
    {
        public PersistentStoragePmActivityBag(ISessionImplementor session)
            : base(session)
        {
        }

        public PersistentStoragePmActivityBag(ISessionImplementor session, ICollection<PmActivity> original)
            : base(session, original)
        {
        }

        public override ICollection GetOrphans(object snapshot, string entityName)
        {
            var orphans = base.GetOrphans(snapshot, entityName)
                .Cast<PmActivity>()
                .Where(b => ReferenceEquals(null, b.Parent))
                .ToArray();

            return orphans;
        }
    }
}
