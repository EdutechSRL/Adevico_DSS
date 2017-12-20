using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.NoticeBoard.Domain
{
    [Serializable]
    public class StyleSettings 
    {
        public virtual String FontFamily { get; set; }
        public virtual Int32 FontSize { get; set; }
        public virtual String FontColor { get; set; }
        public virtual String TextAlign { get; set; }
        public virtual String BackgroundColor { get; set; }

        public StyleSettings()
        {
            FontColor = "";
            TextAlign = "";
            FontFamily = "";
            BackgroundColor="FFFFFF";
        }
    }
}