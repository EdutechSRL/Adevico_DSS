using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.CommunityDiary.Business
{
    public class DiaryItemNotFound : Exception
    {
        public DiaryItemNotFound()
        {
        }

        public DiaryItemNotFound(string message)
            : base(message)
        {
        }

        public DiaryItemNotFound(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
   
}
