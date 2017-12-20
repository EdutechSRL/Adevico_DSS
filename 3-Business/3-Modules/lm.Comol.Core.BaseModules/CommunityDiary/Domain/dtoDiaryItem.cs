using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Repository;
using lm.Comol.Core.Events.Domain;

namespace lm.Comol.Core.BaseModules.CommunityDiary.Domain
{
    [Serializable()]
    public class dtoDiaryItem
    {
        public long Id { get; set; }
        public long IdEvent { get; set; }
        public int CommunityId { get; set; }
        public CoreItemPermission Permission {get; set;}
        public CommunityEventItem EventItem { get; set; }
        public int LessonNumber {get; set;}
        public String Description { get; set; }
        public String DescriptionPlain { get; set; }
        public List<dtoAttachmentItem> Attachments { get; set; }
        public List<RepositoryAttachmentUploadActions> UploadActions { get; set; }
        public RepositoryAttachmentUploadActions DefaultUploadAction { get; set; }

        public dtoDiaryItem()
        {
            UploadActions = new List<RepositoryAttachmentUploadActions>();
            DefaultUploadAction = RepositoryAttachmentUploadActions.none;
            Attachments = new List<dtoAttachmentItem>();
        }
        public dtoDiaryItem(CoreItemPermission module, CommunityEventItem item, List<RepositoryAttachmentUploadActions> actions, RepositoryAttachmentUploadActions dAction)
        {
            Id = item.Id;
            EventItem = item;
            CommunityId = item.IdCommunityOwner;
            IdEvent = (item.EventOwner != null  ? item.EventOwner.Id : 0);
            Permission = module;
            UploadActions = actions;
            DefaultUploadAction = dAction;
            Attachments = new List<dtoAttachmentItem>();
        }
        public dtoDiaryItem(int idCommunity, CoreItemPermission module, CommunityEventItem item, Int32 lessonNumber, String description, List<RepositoryAttachmentUploadActions> actions, RepositoryAttachmentUploadActions dAction)
        {
            Id = item.Id;
            CommunityId = idCommunity;
            IdEvent = (item.EventOwner != null ? item.EventOwner.Id : 0);
            EventItem = item;
            Permission = module;
            LessonNumber = lessonNumber;
            if (String.IsNullOrWhiteSpace(description))
                description = "";
            Description = description;
            UploadActions = actions;
            DefaultUploadAction = dAction;
            Attachments = new List<dtoAttachmentItem>();
        }
    }
 }