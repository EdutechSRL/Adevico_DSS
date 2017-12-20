using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Event
{
    public class EventItemFileNotLinked : Exception
    {
        public EventItemFileNotLinked()
        {
        }

        public EventItemFileNotLinked(string message)
            : base(message)
        {
        }

        public EventItemFileNotLinked(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class EventItemInternalFileNotLinked : Exception
    {
        public EventItemInternalFileNotLinked()
        {
        }

        public EventItemInternalFileNotLinked(string message)
            : base(message)
        {
        }

        public EventItemInternalFileNotLinked(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class EventItemCommunityFileNotLinked : Exception
    {
        public EventItemCommunityFileNotLinked()
        {
        }

        public EventItemCommunityFileNotLinked(string message)
            : base(message)
        {
        }

        public EventItemCommunityFileNotLinked(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
