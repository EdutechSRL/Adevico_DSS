using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.FileRepository.Domain
{
    [Serializable]
    public class dtoNodeItem 
    {
        public long Id { get; set; }
        public long IdFolder { get; set; }
        public Int32 IdCommunity { get; set; }
        public String Name { get; set; }
        public String Url { get; set; }
        public String Path { get; set; }
        public String IdentifierPath { get; set; }

        public String TemplateNavigateUrl { get; set; }
        public FolderType FolderType { get; set; }
        public ItemType ItemType { get; set; }
        public NodeType Type { get; set; }
        public Boolean HasCurrent { get; set; }
        public Boolean IsCurrent { get; set; }
        public Boolean IsEmpty { get; set; }
        public Boolean Selected { get; set; }
        public Boolean Selectable { get; set; }
        public List<dtoNodeItem> Fathers { get; set; }
        public dtoNodeItem()
        {
            Fathers = new List<dtoNodeItem>();
        }
        public Boolean DisplayEmpty()
        {
            return (FolderType != Domain.FolderType.standard);
        }
        
    }

    [Serializable]
    public enum NodeType : int
    {
        Root = 0,
        Item = 1,
        OpenItemNode = 2,
        CloseNode = 3,
        OpenChildren = 4,
        CloseChildren = 5,
        NoChildren = 6
    }
}