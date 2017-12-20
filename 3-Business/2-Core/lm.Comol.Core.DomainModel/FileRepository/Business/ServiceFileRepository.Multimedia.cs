using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using lm.Comol.Core.FileRepository.Domain;
using System.Linq.Expressions;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.FileRepository.Business
{
    public partial class ServiceFileRepository
    {
        public List<dtoMultimediaFileObject> MultimediaGetItems(long idItem, long idVersion)
        {
            List<dtoMultimediaFileObject> items = null;
            try {
                items = (from i in Manager.GetIQ<MultimediaFileObject>()
                         where i.IdItem == idItem && i.IdVersion == idVersion
                         select i).ToList().Select(i => new dtoMultimediaFileObject(i)).OrderBy(i=> (i.Folders.Any() ? 0:1)).OrderBy(i=>i.Fullname).ToList(); 
            }
            catch (Exception ex)
            {

            }
            return items;
        }
        public dtoMultimediaFileObject MultimediaGetItem(long idDocument)
        {
            dtoMultimediaFileObject item = null;
            try
            {
                MultimediaFileObject obj = Manager.Get<MultimediaFileObject>(idDocument);
                if (obj != null)
                    item = new dtoMultimediaFileObject(obj);
            }
            catch (Exception ex)
            {

            }
            return item;
        }
        public MultimediaFileObject MultimediaSetDefaultItem(long idItem, long idVersion, long idDocument)
        {
            MultimediaFileObject document = null;
            try
            {
                RepositoryItem item = Manager.Get<RepositoryItem> (idItem);
                RepositoryItemVersion version = Manager.Get<RepositoryItemVersion> (idVersion);
                List<MultimediaFileObject> cDefaults = (from f in Manager.GetIQ<MultimediaFileObject>() where f.Id != idDocument && f.IdVersion == idVersion && f.IsDefaultDocument select f).ToList();
                document = Manager.Get<MultimediaFileObject>(idDocument);
                if (document != null && !document.IsDefaultDocument )
                {
                    DateTime date = DateTime.Now;
                    Manager.BeginTransaction();
                    FileTransferMultimedia mTransfer = MultimediaTransferGet(idItem, idVersion);
                    if (mTransfer != null)
                    {
                        mTransfer.DefaultDocument = document;
                        mTransfer.DefaultDocumentPath = document.Fullname;
                        Manager.SaveOrUpdate(mTransfer);
                    }
                    document.IsDefaultDocument = true;
                    foreach (MultimediaFileObject d in cDefaults)
                    {
                        d.IsDefaultDocument = false;
                    }
                    if (cDefaults.Any())
                        Manager.SaveOrUpdateList(cDefaults);
                    version.UpdateMetaInfo(UC.CurrentUserID, UC.IpAddress, UC.ProxyIpAddress, date);
                    if (item.IdVersion == version.Id)
                    {
                        item.UpdateMetaInfo(UC.CurrentUserID, UC.IpAddress, UC.ProxyIpAddress, date);
                        Manager.SaveOrUpdate(item);
                    }
                    Manager.SaveOrUpdate(version);
                    Manager.SaveOrUpdate(document);
                    Manager.Commit();
                }
            }
            catch (Exception ex)
            {
                Manager.RollBack();
            }
            return document;
        }
    }
}