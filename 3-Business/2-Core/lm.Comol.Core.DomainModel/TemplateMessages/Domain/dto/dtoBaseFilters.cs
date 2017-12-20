using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
 
namespace lm.Comol.Core.TemplateMessages.Domain
{
    [Serializable]
    public class dtoBaseFilters
    {
        public virtual Int32 PageIndex { get; set; }
        public virtual Int32 PageSize { get; set; }
        public virtual TemplateOrder OrderBy { get; set; }
        public virtual Boolean Ascending { get; set; }
        public virtual TemplateStatus Status { get; set; }
        public virtual String SearchForName { get; set; }
        public virtual TemplateType TemplateType { get; set; }
        //public virtual TemplateLoaderType LoaderType { get; set; }

        public virtual TemplateDisplay TemplateDisplay { get; set; }
        
        public virtual String ModuleCode { get; set; }
        public virtual Dictionary<TemplateStatus, String> TranslationsStatus { get; set; }
        public virtual Dictionary<TemplateType, String> TranslationsType { get; set; }
        public dtoBaseFilters()
        {
            TranslationsStatus = new Dictionary<TemplateStatus, String>();
            TranslationsType = new Dictionary<TemplateType, String>();
            //LoaderType = TemplateLoaderType.None;
        }

        //public void SetLoadingType(TemplateType t) {
        //    switch (t) { 
        //        case Domain.TemplateType.User:
        //            LoaderType = TemplateLoaderType.User;
        //            break;
        //        case Domain.TemplateType.System:
        //            LoaderType = TemplateLoaderType.System;
        //            break;
        //        case Domain.TemplateType.Module:
        //            LoaderType = TemplateLoaderType.Module;
        //            break;
        //    }
        //}
    }  
    
    [Serializable()]
    public enum TemplateOrder
    {
        None = 0,
        ByType = 1,
        ByStatus = 2,
        ByUser = 5,
        ByDate = 6,
        ByName = 7
    }

    [Serializable()]
    public enum TemplateDisplay
    {
        None = 0,
        OnlyVisible = 1,
        Deleted = 2,
        All = 4
    }

    //[Serializable()]
    //public enum TemplateDisplayType
    //{
    //    None = 0,
    //    System = 1,
    //    Module = 2,
    //    User = 3
    //}
}