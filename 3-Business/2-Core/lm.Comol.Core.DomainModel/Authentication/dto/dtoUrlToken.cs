using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class dtoUrlToken
    {
        public virtual String Identifier { get; set; }
        public virtual String Value { get; set; }
        public virtual String DecriptedValue { get; set; }
        public virtual dtoUrlTokenEvaluation Evaluation { get; set; }

        public dtoUrlToken() {
            Evaluation = new dtoUrlTokenEvaluation() { Result = UrlProviderResult.NotEvaluatedToken };
        }

        public void SetException(Exception ex){
            if (ex != null)
            {
                Evaluation.ExceptionString = "Message=" + ex.Message + "\n\r";
                if (ex.InnerException != null)
                    Evaluation.ExceptionString += "InnerException=" + ex.InnerException.ToString() + "\n\r";
            }
        }
    }
  
   [Serializable]
    public class dtoUrlTokenEvaluation
    {
        public virtual UrlProviderResult Result { get; set; }
        public virtual String FullDecriptedValue { get; set; }
        public virtual String ExpiredMessage { get; set; }
        public virtual String ExceptionString { get; set; }
        public virtual String TokenException { get; set; }
    }
}