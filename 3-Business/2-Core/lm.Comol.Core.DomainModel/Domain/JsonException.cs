using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Domain
{
    public class JsonException : Exception
    {
        public JsonException()
        {

        }

        public JsonException(object error)
            : base(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(error))
        {

        }
    }

    [Serializable]
    public class JsonError<T> where  T : class 
    {
        public T ReturnObject { get; set; }
        /// <summary>
        /// Error level
        /// </summary>
        public String Level { get; set; }
        /// <summary>
        /// Error Translated
        /// </summary>
        public String Message { get; set; }
        public Boolean CloseDialog { get; set; }
        public Boolean MessageDialog { get; set; }
        public Boolean PageReload { get; set; }
    }
}