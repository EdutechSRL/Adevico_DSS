using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel
{
    public class ItemSaveException : Exception
    {
        public ItemSaveException()
        {
        }

        public ItemSaveException(string message)
            : base(message)
        {
        }

        public ItemSaveException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class UnknownItemException : Exception
    {
        public UnknownItemException()
        {
        }

        public UnknownItemException(string message)
            : base(message)
        {
        }

        public UnknownItemException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
   }
