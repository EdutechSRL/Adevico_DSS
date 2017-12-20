using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.NoticeBoard.Domain
{
    [Serializable]
    public class MessageStyle : lm.Comol.Core.DomainModel.DomainObject<long>
    {
        public virtual String Face {get;set;}
        public virtual Int32 Size {get;set;}
        public virtual String Color {get;set;}
        public virtual String Align {get;set;}
        public virtual String BackGroundColor {get;set;}

        public MessageStyle()
        {
            Color="";
            Align = "";
            Face="Verdana";
            BackGroundColor="FFFFFF";

        }
    }
}