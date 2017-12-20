using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Business;
using lm.Comol.Core.BaseModules.NoticeBoard.Domain;
using System.IO;
using TheArtOfDev;
using TheArtOfDev.HtmlRenderer.WinForms;
namespace lm.Comol.Core.BaseModules.NoticeBoard.Business
{
    public class ServiceNoticeBoard: CoreServices
    {
        private const string UniqueCode = "SRVcommInfo";
        private iApplicationContext _Context;
        #region initClass

            public ServiceNoticeBoard() { }
            public ServiceNoticeBoard(iApplicationContext oContext)
            {
                this.Manager = new BaseModuleManager(oContext.DataContext);
                _Context = oContext;
                this.UC = oContext.UserContext;
            }
            public ServiceNoticeBoard(iDataContext oDC)
            {
                this.Manager = new BaseModuleManager(oDC); 
               
                _Context = new ApplicationContext();
                _Context.DataContext = oDC;
                this.UC = null;
            }
        #endregion

        public int ServiceModuleID()
        {
            return this.Manager.GetModuleID(ModuleNoticeboard.UniqueID);
        }

        public NoticeboardMessage GetMessage(long idMessage) {
            NoticeboardMessage message = null;
            try
            {
                message = Manager.Get<NoticeboardMessage>(idMessage);
            }
            catch (Exception ex) { }

            return message;
        }
        public NoticeboardMessage GetLastMessage(int idCommunity)
        {
            NoticeboardMessage message = null;
            try
            {
                Community owner = (idCommunity>0) ? Manager.GetCommunity(idCommunity) : null;
                message = (from m in Manager.GetIQ<NoticeboardMessage>()
                           where m.Status == Status.Active && ((m.isForPortal && (idCommunity == 0)) || (m.Community == owner && idCommunity > 0))
                           select m).OrderByDescending(m => m.DisplayDate).Skip(0).Take(1).ToList().FirstOrDefault();
                    
            }
            catch (Exception ex) { }

            return message;
        }
        public long GetLastMessageId(int idCommunity)
        {
            long idMessage = 0;
            try
            {
                Community owner = (idCommunity > 0) ? Manager.GetCommunity(idCommunity) : null;
                idMessage = (from m in Manager.GetIQ<NoticeboardMessage>()
                             where m.Status == Status.Active && ((m.isForPortal && (idCommunity == 0)) || (m.Community == owner && idCommunity > 0))
                             select m).OrderByDescending(m => m.DisplayDate).Skip(0).Take(1).ToList().Select(m => m.Id).FirstOrDefault();

            }
            catch (Exception ex) { }

            return idMessage;
        }

        #region "Edit Message"

        public Boolean isNewMessage(long idMessage, String text, String plainText)
        {
            NoticeboardMessage message = null;
            try
            {
                Person p = Manager.GetPerson(UC.CurrentUserID);
                if (p != null && p.TypeID != (int)UserTypeStandard.Guest && p.TypeID != (int)UserTypeStandard.PublicUser)
                {
                    if (idMessage > 0)
                    {
                        message = Manager.Get<NoticeboardMessage>(idMessage);
                        if (message != null)
                        {
                            switch (message.Status)
                            {
                                case Status.Active:
                                    return false; //(message.PlainText != plainText);
                                case Status.Draft:
                                    return false;
                                case Status.Expired:
                                    return true;
                            }
                        }
                    }
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return true;
            }
            return true;
        }


            public NoticeboardMessage SaveMessage(
                long idMessage,
                Boolean advancedEditor,
                Int32 idCommunity, 
                Boolean isForPortal, 
                String renderfolderpath, 
                String url, 
                String defaultHttpUrl, 
                String baseUrlHttp,
                String text, 
                String plainText,
                StyleSettings settings = null)
            {
                NoticeboardMessage message = null;
                try {

                    Person p = Manager.GetPerson(UC.CurrentUserID);

                    if ( p!=null && p.TypeID != (int) UserTypeStandard.Guest && p.TypeID!=(int) UserTypeStandard.PublicUser)
                    {
                        Manager.BeginTransaction();
                        if (idMessage > 0) {
                            message = Manager.Get<NoticeboardMessage>(idMessage);
                            if (message != null)
                            {
                                message.ModifiedBy = p;
                                message.ModifiedOn = DateTime.Now;
                                if (message.Status== Status.Active &&  message.PlainText != plainText)
                                    message.DisplayDate =  message.ModifiedOn.Value;
                            }
                        }
                        else
                        {
                            message = new NoticeboardMessage();
                            message.isDeleted = false;
                            message.Status = Status.Active;
                            message.Owner = p;
                            message.CreatedOn = DateTime.Now;
                            message.CreatedBy = p;
                            message.DisplayDate = message.CreatedOn.Value;
                            message.ModifiedBy = p;
                            message.ModifiedOn = message.CreatedOn;
                            message.isForPortal = isForPortal;
                            message.Community = (isForPortal) ? null : Manager.GetCommunity(idCommunity);
                            message.Status = Status.Active;
                           
                        }

                        if (message != null)
                        {
                            message.Message = text;
                            message.PlainText = plainText;
                            message.CreateByAdvancedEditor = advancedEditor;
                            if (!advancedEditor && settings != null)
                                message.StyleSettings = settings;

                            if (idMessage == 0)
                            {
                                List<NoticeboardMessage> items = (from m in Manager.GetIQ<NoticeboardMessage>() where m.Status == Status.Active && m.isForPortal == message.isForPortal && m.Community == message.Community select m).ToList();
                                foreach (NoticeboardMessage item in items)
                                {
                                    item.Status = Status.Expired;
                                }
                                if (items.Any())
                                    Manager.SaveOrUpdateList(items);
                                Manager.SaveOrUpdate(message);
                                

                            }
                        }

                        Manager.Commit();

                        if (message.Status == Status.Active) { 
                            if (!String.IsNullOrEmpty(url) && url.Contains("{0}"))
                                url = String.Format(url, message.Id);
                            try
                            {
                                Manager.BeginTransaction();
                                RigenerateImage(message, renderfolderpath, url, baseUrlHttp, defaultHttpUrl);
                                Manager.Commit();
                            }
                            catch (Exception ex)
                            {
                                Manager.RollBack();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    message = null;
                    Manager.RollBack();
                }
                return message;
            }


            public NoticeboardMessage SetActiveMessage(long idMessage, String renderfolderpath, String url, String defaultHttpUrl, String baseUrlHttp)
            {
                NoticeboardMessage message = null;
                try {
                    Person p = Manager.GetPerson(UC.CurrentUserID);
                    message = Manager.Get<NoticeboardMessage>(idMessage);
                    if (message != null && (message.isForPortal || (!message.isForPortal && message.Community !=null)  ) && p!=null && p.TypeID != (int) UserTypeStandard.Guest && p.TypeID!=(int) UserTypeStandard.PublicUser)
                    {
                        message = SaveMessage(0, message.CreateByAdvancedEditor, (message.isForPortal) ? 0 : message.Community.Id, message.isForPortal, renderfolderpath, url, defaultHttpUrl, baseUrlHttp, message.Message, message.PlainText, message.StyleSettings);
                        //Manager.BeginTransaction();
                        //List<NoticeboardMessage> items = (from m in Manager.GetIQ<NoticeboardMessage>() where m.Status == Status.Active && m.isForPortal == message.isForPortal && m.Community == message.Community select m).ToList();
                        //foreach (NoticeboardMessage item in items) {
                        //    item.Status = Status.Expired;
                        //}
                        //if (items.Any())
                        //    Manager.SaveOrUpdateList(items);
                        //message.isDeleted = false;
                        //message.Status = Status.Active;
                        //message.ModifiedBy = p;
                        //message.ModifiedOn = DateTime.Now;
                        //message.DisplayDate = message.ModifiedOn.Value;
                        //Manager.Commit();
                        //if (message.Image == Guid.Empty)
                        //{
                        //    try
                        //    {
                        //        Manager.BeginTransaction();
                        //        RigenerateImage(message, renderfolderpath, url, baseUrlHttp, defaultHttpUrl);
                        //        Manager.Commit();
                        //    }
                        //    catch (Exception ex) { 
                            
                        //    }
                        //}
                    }
                }
                catch (Exception ex)
                {
                    message = null;
                    Manager.RollBack();
                }
                return message;
            }
            public NoticeboardMessage AddEmptyMessage(Int32 idCommunity, Boolean isForPortal, String renderfolderpath, String url, String defaultHttpUrl, String baseUrlHttp)
            {
                return SaveMessage(0,true,idCommunity, isForPortal, renderfolderpath, url, defaultHttpUrl,baseUrlHttp,"","");
            }

            public NoticeboardMessage VirtualDeleteMessage(long idMessage, Boolean delete, Boolean activate, System.Guid applicationWorkingId, String renderfolderpath = "", String url="", String defaultHttpUrl = "", String baseUrlHttp = "")
            {
                NoticeboardMessage message = null;
                try {
                    Person p = Manager.GetPerson(UC.CurrentUserID);
                    message = Manager.Get<NoticeboardMessage>(idMessage);
                    if (message != null && p!=null && p.TypeID != (int) UserTypeStandard.Guest && p.TypeID!=(int) UserTypeStandard.PublicUser)
                    {
                        Manager.BeginTransaction();
                        List<NoticeboardMessage> items = null;
                        message.isDeleted = delete;
                        message.ModifiedBy = p;
                        message.ModifiedOn = DateTime.Now;
                        if (message.Status == Status.Active && delete)
                        {
                            items = (from m in Manager.GetIQ<NoticeboardMessage>() where m.Status == Status.Expired && m.isForPortal == message.isForPortal && m.Community == message.Community orderby m.ModifiedOn descending select m).Skip(0).Take(1).ToList();
                            foreach (NoticeboardMessage item in items)
                            {
                                item.Status = Status.Active;
                                item.ModifiedBy = p;
                                item.ModifiedOn = DateTime.Now;
                            }
                            message.Status = Status.VirtualDeleted;
                        }
                        else if (!delete && activate)
                        {
                            items = (from m in Manager.GetIQ<NoticeboardMessage>() where m.Status == Status.Active && m.isForPortal == message.isForPortal && m.Community == message.Community select m).ToList();
                            foreach (NoticeboardMessage item in items)
                            {
                                item.Status = Status.Expired;
                            }
                            message.Status = Status.Active;
                            message.DisplayDate = message.ModifiedOn.Value;
                        }
                        else
                            message.Status = (delete) ? Status.VirtualDeleted : Status.Expired;
                       
                        if (items!= null && items.Any())
                            Manager.SaveOrUpdateList(items);
              
                        Manager.SaveOrUpdate(message);
                        Manager.Commit();
                        if (message.Image == Guid.Empty && message.Status== Status.Active) {
                            try
                            {
                                Manager.BeginTransaction();
                                RigenerateImage(message, renderfolderpath, url, baseUrlHttp, defaultHttpUrl);
                                Manager.Commit();
                            }
                            catch (Exception ex)
                            {
                                Manager.RollBack();
                            }
                        }                      
                    }
                }
                catch (Exception ex)
                {
                    message = null;
                    Manager.RollBack();
                }
                return message;
            }

            public NoticeboardMessage DeleteMessage(long idMessage)
            {
                NoticeboardMessage message = null;
                try {
                    Person p = Manager.GetPerson(UC.CurrentUserID);
                    message = Manager.Get<NoticeboardMessage>(idMessage);
                    if (message != null && p!=null && p.TypeID != (int) UserTypeStandard.Guest && p.TypeID!=(int) UserTypeStandard.PublicUser)
                    {
                        Manager.BeginTransaction();
                        Manager.DeletePhysical(message);
                        Manager.Commit();                  
                    }
                }
                catch (Exception ex)
                {
                    Manager.RollBack();
                }
                return message;
            }
        
            public void RigenerateImage(NoticeboardMessage message, String renderfolderpath,String templateFile, String applicationurl, String preloadCss,String rendertext="",String renderstyle="",  Boolean alsoThumbnail = false, Int32 width =300, Int32 height = 200 )
            {
                System.Guid identifier = Guid.NewGuid();
                try{
                    String html = "";
                    using(StreamReader sw=new StreamReader(templateFile))
                    {
                        html = sw.ReadToEnd();
                    }
                    if (!string.IsNullOrEmpty(html)){
                        html = html.Replace("#renderstyle#",renderstyle );
                        html = html.Replace("#baseurl#",applicationurl );
                        html = html.Replace("#preloadcss#",preloadCss );
                        html = html.Replace("#rendertext#",rendertext );
                    }
                    System.Drawing.Image image = null;
                    if (width>0 && height>0)
                        image = HtmlRender.RenderToImageGdiPlus(html, new System.Drawing.Size(width, height));
                    else
                        image = HtmlRender.RenderToImageGdiPlus(html);
                    if (!lm.Comol.Core.File.Exists.Directory(renderfolderpath))
                        lm.Comol.Core.File.Create.Directory(renderfolderpath);

                    lm.Comol.Core.File.Create.Image(image, renderfolderpath + identifier.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);
                    //image.Save(renderfolderpath + identifier.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);
                }
                catch (Exception ex){
                    identifier = Guid.Empty;
                }
                if (identifier != Guid.Empty) {
                    message.Image = identifier;
                    if (alsoThumbnail)
                    {

                    }
                }
            }

            public void RigenerateImage(NoticeboardMessage message, String renderfolderpath, String url, String baseUrlHttp, String baseUrlHttps )
            {
                System.Guid identifier = Guid.NewGuid();
                try
                {
                    if (url.Contains("https://"))
                        url = url.Replace("https://", "http://");
                    lm.Comol.Core.DomainModel.Helpers.Thumbnailer c = new lm.Comol.Core.DomainModel.Helpers.Thumbnailer();
                    System.Drawing.Bitmap b = c.GetThumbnailFromWeb(url, false, baseUrlHttp, baseUrlHttps);
                    if (b==null)
                        b = c.GetThumbnailFromWeb(url, true, baseUrlHttp, baseUrlHttps);

                    if (b != null)
                    {
                        if (!lm.Comol.Core.File.Exists.Directory(renderfolderpath))
                            lm.Comol.Core.File.Create.Directory(renderfolderpath);

                        lm.Comol.Core.File.Create.Image(b, renderfolderpath + identifier.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);
                    }
                    else
                        identifier = Guid.Empty;
                }
                catch (Exception ex)
                {
                    identifier = Guid.Empty;
                }
                if (identifier != Guid.Empty)
                {
                    message.Image = identifier;
                }
            }
        #endregion


        #region "History"
            public liteHistoryItem GetHistoryItem(long idMessage)
            {
                liteHistoryItem message = null;
                try
                {
                    message = Manager.Get<liteHistoryItem>(idMessage);

                }
                catch (Exception ex) { }

                return message;
            }
            public liteHistoryItem GetLastHistoryItem(int idCommunity)
            {
                liteHistoryItem message = null;
                try
                {
                    message = (from m in Manager.GetIQ<liteHistoryItem>()
                               where m.isDeleted == false && ((m.isForPortal && (idCommunity == 0)) || (m.IdCommunity == idCommunity && idCommunity > 0)) 
                               select m).OrderByDescending(m => m.DisplayDate).Skip(0).Take(1).ToList().FirstOrDefault();
                    
                }
                catch (Exception ex) { }

                return message;
            }
            public Int32 GetHistoryItemsCount(int idCommunity, List<Status> availableStatus)
            {
                Int32 count = 0;
                try
                {
                    count = (from m in Manager.GetIQ<liteHistoryItem>()
                             where availableStatus.Contains(m.Status) && ((m.isForPortal && (idCommunity == 0)) || (m.IdCommunity == idCommunity && !m.isForPortal))
                               select m.Id).Count();
                }
                catch (Exception ex) { }

                return count;
            }

            public List<Status> GetAvailableStatus(ModuleNoticeboard permissions, Boolean forManagment)
            {
                List<Status> items = new List<Status>();
                if ((permissions.Administration || permissions.ViewCurrentMessage))
                {
                    items.Add(Status.Active);
                }
                if ((permissions.Administration || permissions.ViewOldMessage))
                {
                    items.Add(Status.Expired);
                }
                if ((permissions.Administration || permissions.EditMessage)) {
                    items.Add(Status.Draft);
                }
                if (forManagment && (permissions.Administration || permissions.RetrieveOldMessage || permissions.DeleteMessage))
                    items.Add(Status.VirtualDeleted);

                return items;

            }
            public List<dtoHistoryItem> GetHistoryItems(int idCommunity,long idCurrentMessage, Int32 pageIndex, Int32 pageSize,String removedUser, List<Status> availableStatus)
            {
                List<dtoHistoryItem> items = new List<dtoHistoryItem>();
                try
                {
                    List<liteHistoryItem> hItems = (from m in Manager.GetIQ<liteHistoryItem>()
                                                    where availableStatus.Contains(m.Status) && ((m.isForPortal && (idCommunity == 0)) || (m.IdCommunity == idCommunity))
                                                    orderby m.DisplayDate descending 
                                                    select m).Skip(pageIndex * pageSize).Take(pageSize).ToList();

                    items = hItems.Select(i => new dtoHistoryItem(i, removedUser) { Selected= (i.Id == idCurrentMessage)}).ToList();
                    switch(items.Count ){
                        case 1:
                            items[0].DisplayAs = ItemDisplayOrder.first | ItemDisplayOrder.last;
                            break;
                        case 0:
                            break;
                        default:
                            items.FirstOrDefault().DisplayAs = ItemDisplayOrder.first;
                            items.LastOrDefault().DisplayAs= ItemDisplayOrder.last;
                            break;
                    }
                }
                catch (Exception ex) { }

                return items;
            }
            public Int32 GetHistoryPageIndex(int idCommunity, long idMessage, Int32 pageSize, List<Status> availableStatus)
            {
                Int32 pageIndex = 0;
                try
                {
                    if (idMessage > 0) { 
                        List<long> hItems = (from m in Manager.GetIQ<liteHistoryItem>()
                                             where availableStatus.Contains(m.Status) && ((m.isForPortal && (idCommunity == 0)) || (m.IdCommunity == idCommunity))
                                             orderby m.DisplayDate descending
                                                        select m.Id).ToList();
                        Int32 position = hItems.IndexOf(idMessage)+1;
                        if (position > pageSize)
                            pageIndex = (int)Math.Floor((double)(position / pageSize));
                    }
                }
                catch (Exception ex) { }

                return pageIndex;
            }
        #endregion


        public ModuleNoticeboard GetPermissions(int idCommunity)
        {
            ModuleNoticeboard permissions = null;
            try
            {
                if (idCommunity > 0 || idCommunity<0)
                    permissions = new ModuleNoticeboard(Manager.GetModulePermission(UC.CurrentUserID, idCommunity, Manager.GetModuleID(ModuleNoticeboard.UniqueID)));
                else
                    permissions = ModuleNoticeboard.CreatePortalmodule(UC.UserTypeID);
            }
            catch (Exception ex) { }

            return permissions;
        }
    }
}
