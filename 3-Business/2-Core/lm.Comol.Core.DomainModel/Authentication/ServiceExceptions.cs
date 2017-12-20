using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.Authentication
{
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException()
        {
        }

        public InvalidPasswordException(string message)
            : base(message)
        {
        }

        public InvalidPasswordException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class SamePasswordException : Exception
    {
        public SamePasswordException()
        {
        }

        public SamePasswordException(string message)
            : base(message)
        {
        }

        public SamePasswordException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class ProfilerException : Exception
    {
        public virtual ProfilerError Error {get;set;}
        public ProfilerException()
        {
        }

        public ProfilerException(string message)
            : base(message)
        {
        }

        public ProfilerException(string message, Exception inner)
            : base(message, inner)
        {
        }
        public ProfilerException(ProfilerError error, Exception inner)
            : base(error.ToString() , inner)
        {
            Error = error;
        }
    }
   }
