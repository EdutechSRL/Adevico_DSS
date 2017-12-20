using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
    public class dtoSubmittedFile : dtoBaseFile
    {
        public virtual Boolean isReplaced { get; set; }
        public dtoSubmittedFile() : base() {
        }

        public dtoSubmittedFile(SubmittedFile submittedFile)
        {
            Id = submittedFile.Id;
            Item = submittedFile.File;
            Deleted = submittedFile.Deleted;
            ModuleLinkId = submittedFile.Link.Id;
            isReplaced = submittedFile.isReplaced;
            Link = submittedFile.Link;
        }
    }
}
