using lm.Comol.Core.DomainModel;
using lm.Comol.Core.DomainModel.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Tag.Domain
{
    [Serializable]
    public class dtoTagSelectItem : lm.Comol.Core.DomainModel.DomainBaseObject<long>
    {
        public virtual String Name { get; set; }
        public virtual Boolean IsDefault { get; set; }
        public virtual Boolean IsSystem { get; set; }
        public virtual lm.Comol.Core.Dashboard.Domain.AvailableStatus Status { get; set; }
        public virtual Boolean IsSelected { get; set; }
        public virtual Boolean CanBeSelected { get { return Status == Dashboard.Domain.AvailableStatus.Available; } }
        public virtual Boolean CanBeUnselected { get { return (!IsSelected && Status == Dashboard.Domain.AvailableStatus.Available) || (Status != Dashboard.Domain.AvailableStatus.Unavailable && IsSelected); } }
        public virtual TagType Type { get; set; }
        public virtual Int32 IdModule { get; set; }
        public virtual String ModuleCode { get; set; }
        public dtoTagSelectItem()
        {
        }

        public dtoTagSelectItem(liteTagItem tag, Int32 idLanguage, Int32 dLanguage, List<long> idTags)
        {
            Id = tag.Id;
            Deleted = tag.Deleted;
            IsDefault = tag.IsDefault;
            IsSystem = tag.IsSystem;
            Status = tag.Status;
            Name = tag.GetTitle(idLanguage, dLanguage);
            Type = tag.Type;
            IdModule = tag.IdModule;
            ModuleCode = tag.ModuleCode;
            IsSelected = idTags!= null && idTags.Contains(tag.Id);
        }

        public dtoTagSelectItem Copy(Boolean isSelected)
        {
            dtoTagSelectItem dto = new dtoTagSelectItem();
            dto.Id = Id;
            dto.Deleted = Deleted;
            dto.Name = Name;
            dto.IsDefault = IsDefault;
            dto.IsSystem = IsSystem;
            dto.Status = Status;
            dto.IsSelected = isSelected;
            dto.Type = Type;
            dto.IdModule = IdModule;
            dto.ModuleCode = ModuleCode;
            return dto;
        }

    }
}