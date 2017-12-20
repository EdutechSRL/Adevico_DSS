using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Editor
{
    [Serializable()]
    public class EditorToolbar
    {
        public virtual EditorType EditorType { get; set; }
        public virtual ToolbarType ToolbarType { get; set; }
        public virtual String Url { get; set; }
        public virtual String LocalPath { get; set; }

        public EditorToolbar()
        {
        }
    }

    [Serializable()]
    public enum ToolbarType
    {
        bySettings = 0,
        none = 1,
        simple = 2,
        advanced = 3,
        full = 4,
        lite = 5
    }
}