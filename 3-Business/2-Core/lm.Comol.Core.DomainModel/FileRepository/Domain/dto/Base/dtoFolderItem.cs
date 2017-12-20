using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoFolderItem
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public virtual List<dtoFolderItem> Children { get; set; }
        public String TemplateUrl { get; set; }
        public String IdentifierPath { get; set; }
        public FolderType FolderType { get; set; }
        public Boolean IsEmpty { get { return Children==null || !Children.Any();}}
        public Boolean IsCurrent { get; set; }
        public Boolean IsInCurrentPath { get; set; }
        public dtoFolderItem()
        {
            Children = new List<dtoFolderItem>();
        }
    }
}