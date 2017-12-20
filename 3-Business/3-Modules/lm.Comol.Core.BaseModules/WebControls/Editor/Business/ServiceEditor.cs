using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.Authentication;
using lm.Comol.Core.Business;
using lm.Comol.Core.PersonalInfo;

namespace lm.Comol.Core.BaseModules.Editor.Business
{
    public class ServiceEditor : CoreServices 
    {
        protected iApplicationContext _Context;

        #region initClass
            public ServiceEditor() :base() { }
            public ServiceEditor(iApplicationContext oContext) :base(oContext.DataContext) {
                _Context = oContext;
                this.UC = oContext.UserContext;
            }
            public ServiceEditor(iDataContext oDC) : base(oDC) { }
        #endregion


        public static EditorConfiguration GetConfiguration(String configurationPath){
            EditorConfiguration config = null;
            try
            {
                config = lm.Comol.Core.DomainModel.Helpers.CacheHelper.Find<EditorConfiguration>("EditorConfiguration");
                if (config==null){
                    lm.Comol.Core.DomainModel.Helpers.XmlRepository<EditorConfiguration> repository = new lm.Comol.Core.DomainModel.Helpers.XmlRepository<EditorConfiguration>();
                    config = repository.Deserialize(configurationPath);
                    lm.Comol.Core.DomainModel.Helpers.CacheHelper.AddToFileCache("EditorConfiguration", config, configurationPath);
                }
            }
            catch (Exception ex) { 
                
            }
            return config;
        }
        public String ImageHandlerPath(String configurationPath)
        {
            EditorConfiguration config = GetConfiguration(configurationPath);
            return (config == null) ? "" : config.ImageHandlerPath;
        }
        
    }
}