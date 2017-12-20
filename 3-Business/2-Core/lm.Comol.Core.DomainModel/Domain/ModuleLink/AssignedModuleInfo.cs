using System;
namespace lm.Comol.Core.DomainModel
{
    [Serializable()]
    public class AssignedModuleInfo
    {
        public virtual String CodeModule { get; set; }
        public virtual int IdModuleAction { get; set; }
        public virtual int IdModule { get; set; }
      //  public virtual ModuleDefinition Module { get; set; }


        public virtual long IdObjectLong { get; set; }
        public virtual Guid IdObjectGuid { get; set; }
        public virtual long ContentPermission { get; set; }
        public virtual ModuleLink ModuleLink { get; set; }
    }
}