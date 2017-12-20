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
        #region "Manage Transfers
            public liteFileTransfer FileTransferGet(long idItem, long idVersion)
            {
                return GetQueryLiteFileTransferItems(i => i.IdItem == idItem && i.IdVersion== idVersion && i.Deleted == BaseStatusDeleted.None).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            public FileTransferMultimedia MultimediaTransferGet(long idItem, long idVersion)
            {
                return (from m in Manager.GetIQ<FileTransferMultimedia>() where m.IdItem== idItem && m.IdVersion==idVersion select m).Skip(0).Take(1).ToList().FirstOrDefault();
            }
            public List<liteFileTransfer> FileTransferGetById(List<long> idFiles)
            {
                if (idFiles == null || !idFiles.Any())
                    return new List<liteFileTransfer>();
                else
                    idFiles = idFiles.Distinct().ToList();
                if (idFiles.Count <= maxItemsForQuery)
                    return FileTransferGetPagedById(idFiles);
                else
                {
                    List<liteFileTransfer> results = new List<liteFileTransfer>();
                    Int32 pageIndex = 0;
                    List<long> idPagedItems = idFiles.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    while (idPagedItems.Any())
                    {
                        results.AddRange(FileTransferGetPagedById(idPagedItems));
                        pageIndex++;
                        idPagedItems = idFiles.Skip(pageIndex * maxItemsForQuery).Take(maxItemsForQuery).ToList();
                    }
                    return results;
                }
            }
            private List<liteFileTransfer> FileTransferGetPagedById(List<long> idFiles)
            {
                return GetQuery<liteFileTransfer>().Where(t => idFiles.Contains(t.IdItem)).ToList();
            }
            private IQueryable<liteFileTransfer> GetQueryLiteFileTransferItems(Expression<Func<liteFileTransfer, bool>> filters)
            {
                return (from q in Manager.GetIQ<liteFileTransfer>() select q).Where(filters);
            }
            public Boolean FileTransferAdd(RepositoryItem item, dtoUploadedItem dto, DateTime? date=null){
                return FileTransferAdd(item,dto, GetValidPerson(UC.CurrentUserID),date);
            }
            public Boolean FileTransferAdd(RepositoryItem item, dtoUploadedItem dto, litePerson person, DateTime? date = null)
            {
                return FileTransferAdd(item, dto.SavedPath, dto.SavedFileName, person, date);
            }

            public Boolean FileTransferAdd(RepositoryItem item,String savedPath, String savedFileName, litePerson person, DateTime? date = null)
            {
                Boolean result = false;
                Boolean isInTransaction = Manager.IsInTransaction();
                try
                {
                    if (person != null)
                    {
                        switch (item.Type)
                        {
                            case ItemType.Multimedia:
                                FileTransferMultimedia multimedia = new FileTransferMultimedia();
                                if (date.HasValue)
                                    multimedia.ModifiedOn = date.Value;
                                multimedia.IdItem = item.Id;
                                multimedia.IdVersion = item.IdVersion;
                                multimedia.UniqueIdItem = item.UniqueId;
                                multimedia.UniqueIdVersion = item.UniqueIdVersion;
                                multimedia.Path = savedPath;
                                multimedia.Filename = savedFileName;
                                multimedia.Repository = item.Repository;
                                if (!isInTransaction)
                                    Manager.BeginTransaction();
                                Manager.SaveOrUpdate(multimedia);
                                if (!isInTransaction)
                                    Manager.Commit();
                                break;
                            case ItemType.ScormPackage:
                                FileTransferScorm scorm = new FileTransferScorm();
                                if (date.HasValue)
                                    scorm.ModifiedOn = date.Value;
                                scorm.IdItem = item.Id;
                                scorm.IdVersion = item.IdVersion;
                                scorm.UniqueIdItem = item.UniqueId;
                                scorm.UniqueIdVersion = item.UniqueIdVersion;
                                scorm.Path = savedPath;
                                scorm.Filename = savedFileName;
                                scorm.Repository = item.Repository;
                                if (!isInTransaction)
                                    Manager.BeginTransaction();
                                Manager.SaveOrUpdate(scorm);
                                if (!isInTransaction)
                                    Manager.Commit();
                                break;
                            case ItemType.VideoStreaming:
                                FileTransferVideoStreaming video = new FileTransferVideoStreaming();
                                if (date.HasValue)
                                    video.ModifiedOn = date.Value;
                                video.IdItem = item.Id;
                                video.IdVersion = item.IdVersion;
                                video.UniqueIdItem = item.UniqueId;
                                video.UniqueIdVersion = item.UniqueIdVersion;
                                video.Path = savedPath;
                                video.Filename = savedFileName;
                                if (!isInTransaction)
                                    Manager.BeginTransaction();
                                Manager.SaveOrUpdate(video);
                                if (!isInTransaction)
                                    Manager.Commit();
                                break;
                            default:
                                return true;
                        }

                    }
                }
                catch (Exception ex)
                {
                    if (!isInTransaction)
                        Manager.RollBack();
                }
                return result;
            }
            public Boolean FileTransferNotifyToTransferService(String istance)
            {
                Boolean result = false;
                lm.Comol.Core.DomainModel.SrvFileTransfer.IFileMQService service=null;
                try
                {
                    service = new lm.Comol.Core.DomainModel.SrvFileTransfer.FileMQServiceClient();
                    if (service!=null){
                        System.ServiceModel.ClientBase<lm.Comol.Core.DomainModel.SrvFileTransfer.IFileMQService> srv = null;
                        try
                        {
                            service.FileTransferNotification(istance);
                            result = true;
                        }
                        catch (Exception ex)
                        {
                            if (service != null)
                            {
                                srv = (System.ServiceModel.ClientBase<lm.Comol.Core.DomainModel.SrvFileTransfer.IFileMQService>)service;
                                srv.Abort();
                                srv = null;
                            }
                            service = null;
                        }
                        finally
                        {
                            if (service != null)
                            {
                                srv = (System.ServiceModel.ClientBase<lm.Comol.Core.DomainModel.SrvFileTransfer.IFileMQService>)service;
                                srv.Close();
                                srv = null;
                            }
                        }
                    } 
                }
                catch (Exception mq)
                {

                }
                return result;
            }
        #endregion
    }
}