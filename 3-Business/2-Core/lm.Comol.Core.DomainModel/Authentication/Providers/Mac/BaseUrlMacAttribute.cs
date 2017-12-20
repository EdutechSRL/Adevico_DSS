using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication.Helpers;

namespace lm.Comol.Core.Authentication
{
    [Serializable]
    public class BaseUrlMacAttribute : DomainBaseObject<long>
    {
        public virtual String Name { get; set; }
        public virtual String QueryStringName { get; set; }
        public virtual String Description { get; set; }
        public virtual UrlMacAttributeType Type { get; set; }
        public virtual AuthenticationProvider Provider { get; set; }
   
        public BaseUrlMacAttribute()
        {

        }
        public BaseUrlMacAttribute(String name, UrlMacAttributeType type)
        {
            Name = name;
            Type = type;
        }
        public BaseUrlMacAttribute(String name, String queryName, UrlMacAttributeType type)
        {
            Name = name;
            QueryStringName = queryName;
            Type = type;
        }
    }


    [Serializable]
    public class ApplicationAttribute : BaseUrlMacAttribute
    {
        public virtual String Value { get; set; }
        public ApplicationAttribute()
        {
            Type= UrlMacAttributeType.applicationId;
        }
        public ApplicationAttribute(String name)
        {
            Name = name;
            Type= UrlMacAttributeType.applicationId;
        }
        public ApplicationAttribute(String name, String queryName)
        {
            Name = name;
            QueryStringName = queryName;
            Type = UrlMacAttributeType.applicationId;
        }
    }
    [Serializable]
    public class FunctionAttribute : BaseUrlMacAttribute
    {
        public virtual String Value { get; set; }
        public FunctionAttribute()
        {
            Type = UrlMacAttributeType.functionId;
        }
        public FunctionAttribute(String name)
        {
            Name = name;
            Type = UrlMacAttributeType.functionId;
        }
        public FunctionAttribute(String name, String queryName)
        {
            Name = name;
            QueryStringName = queryName;
            Type = UrlMacAttributeType.functionId;
        }
    } 
}