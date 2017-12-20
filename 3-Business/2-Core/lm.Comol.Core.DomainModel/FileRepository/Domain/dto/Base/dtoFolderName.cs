using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoFolderName
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public String OriginalName { get; set; }
        public Boolean AllowUpload { get; set; }
        public Boolean IsVisible { get; set; }
        public Boolean IsNameModified { get { return !String.IsNullOrWhiteSpace(OriginalName); } }
        public Boolean IsValid { get { return ! String.IsNullOrWhiteSpace(Name) && !System.IO.Path.GetInvalidPathChars().Any(c=> Name.Contains(c));}}
        public dtoFolderName()
        {
        }
    }
}