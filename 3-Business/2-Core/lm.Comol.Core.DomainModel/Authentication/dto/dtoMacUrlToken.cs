using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class dtoMacUrlToken
    {
        public virtual List<dtoMacUrlUserAttribute> Attributes { get; set; }
        public virtual dtoMacUrlTokenEvaluation Evaluation { get; set; }
        public virtual String UniqueIdentifyer { get { return Attributes.Where(a => a.isIdentifier).Select(a => a.QueryValue).FirstOrDefault(); } }
        public virtual String InternalMac { get; set; }
        public dtoMacUrlToken()
        {
            Attributes = new List<dtoMacUrlUserAttribute>();
            Evaluation = new dtoMacUrlTokenEvaluation() { Result = UrlProviderResult.NotEvaluatedToken };
        }

        public void SetException(Exception ex)
        {
            if (ex != null)
            {
                Evaluation.ExceptionString = "Message=" + ex.Message + "\n\r";
                if (ex.InnerException != null)
                    Evaluation.ExceptionString += "InnerException=" + ex.InnerException.ToString() + "\n\r";
            }
        }
    }

    [Serializable]
    public class dtoMacUrlTokenEvaluation
    {
        public virtual UrlProviderResult Result { get; set; }
        public virtual String ExpiredMessage { get; set; }
        public virtual String ExceptionString { get; set; }
        public virtual String Mac { get; set; }
       // public virtual String TokenException { get; set; }
    }
}