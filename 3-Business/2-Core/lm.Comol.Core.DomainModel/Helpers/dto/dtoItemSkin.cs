using System;
namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public class dtoItemSkin
    {
        public virtual long IdSkin {get;set;}
        public virtual int IdOrganization {get;set;}
        public virtual int IdCommunity {get;set;}
        public virtual Boolean IsForPortal {get;set;}
        public virtual Boolean ForObject { get; set; }
    }
}