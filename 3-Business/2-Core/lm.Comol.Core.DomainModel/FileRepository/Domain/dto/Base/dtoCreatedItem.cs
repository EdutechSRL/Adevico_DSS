using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoCreatedItem
    {
       public virtual dtoUploadedItem ToAdd { get; set; }
       public virtual RepositoryItem Added { get; set; }
       public virtual Boolean IsValid { get { return (ToAdd != null && ToAdd.IsValid); } }
       public virtual Boolean IsRenamed { get { return (ToAdd != null && ToAdd.IsRenamed); } }
       public virtual Boolean IsAdded { get { return (Added != null && Added.Id>0); } }
       public virtual ItemAvailability Availability { get { return (Added != null) ? Added.Availability : ItemAvailability.notuploaded; } }
       public virtual ItemStatus Status { get { return (Added != null) ? Added.Status : ItemStatus.Draft ; } }
       public virtual ItemUploadError Error { get; set; }
       public dtoCreatedItem() {
           Error = ItemUploadError.None;
       }
    }
}