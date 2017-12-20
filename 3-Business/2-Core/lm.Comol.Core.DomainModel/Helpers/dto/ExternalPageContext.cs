using System;
namespace lm.Comol.Core.DomainModel.Helpers
{
    [Serializable]
    public class ExternalPageContext
    {
        public virtual dtoItemSkin Skin { get; set; }
        public virtual ModuleObject Source {get;set;}
        public virtual Boolean ShowDocType {get;set;}
        public virtual String CssClass {get;set;}
        public virtual String Title { get; set; }

        public ExternalPageContext() {
            Title = "";
            ShowDocType=false;
            CssClass ="";
            Skin = new dtoItemSkin() { IsForPortal = true };
            Source = new ModuleObject();
        }
    }
}
