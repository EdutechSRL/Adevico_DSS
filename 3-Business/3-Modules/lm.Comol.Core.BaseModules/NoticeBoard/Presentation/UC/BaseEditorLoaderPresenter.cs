using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.NoticeBoard.Domain;
using lm.Comol.Core.Business;
using lm.Comol.Core.BaseModules.Editor.Business;
using lm.Comol.Core.BaseModules.Editor;

namespace lm.Comol.Core.BaseModules.NoticeBoard.Presentation
{
    public class BaseEditorLoaderPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Init"
        private ServiceEditor _Service;

        //private int ModuleID
        //{
        //    get
        //    {
        //        if (_ModuleID <= 0)
        //        {
        //            _ModuleID = this.Service.ServiceModuleID();
        //        }
        //        return _ModuleID;
        //    }
        //}
        public virtual BaseModuleManager CurrentManager { get; set; }
        protected virtual IViewBaseEditorLoader View
        {
            get { return (IViewBaseEditorLoader)base.View; }
        }
        private ServiceEditor Service
        {
            get
            {
                if (_Service == null)
                    _Service = new ServiceEditor(AppContext);
                return _Service;
            }
        }
        public BaseEditorLoaderPresenter(iApplicationContext oContext):base(oContext){
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public BaseEditorLoaderPresenter(iApplicationContext oContext, IViewBaseEditorLoader view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion

        public void InitView(String configurationPath, NoticeboardMessage message)
        {
            if (IsSessionTimeout())
                return;
            if (!View.isInitialized){
                InitializeView(configurationPath, message);
                View.LoadMessage(message);
            }
        }

        private void InitializeView(String configurationPath, NoticeboardMessage message)
        {
            lm.Comol.Core.BaseModules.Editor.EditorConfiguration config = ServiceEditor.GetConfiguration(configurationPath);
            if (!(config == null || (config.DefaultEditor == null && !config.Settings.Any()))){
                ModuleEditorSettings mSettings = (config.ModuleSettings == null) ? null : config.ModuleSettings.Where(m => m.ModuleCode == ModuleNoticeboard.UniqueID).FirstOrDefault();

                EditorType loadType = (mSettings != null) ?
                                    mSettings.EditorType
                                    :
                                    ((config.Settings.Any() && config.Settings.Where(s => s.EditorType == EditorType.telerik).Any())
                                    ? EditorType.telerik : ((config.DefaultEditor != null) ? config.DefaultEditor.EditorType : EditorType.none));
                EditorSettings rSettings = (config.Settings.Any() ? config.Settings.Where(s => s.EditorType == loadType).FirstOrDefault() : null);

                EditorSettings settings = (rSettings != null) ? rSettings : config.DefaultEditor;
                String fontfamily = (mSettings != null) ? mSettings.FontNames : (settings != null) ? settings.FontNames : config.FontNames;
                if (String.IsNullOrEmpty(fontfamily))
                    fontfamily = config.FontNames;

                String fontSizes = (mSettings != null) ? mSettings.FontSizes : (settings != null) ? settings.FontSizes : config.FontSizes;
                String realFontSizes = (mSettings != null) ? mSettings.RealFontSizes : (settings != null) ? settings.RealFontSizes : config.RealFontSizes;
                Boolean useRealFontSize = (!String.IsNullOrEmpty(realFontSizes)) ? View.UseRealFontSize : (mSettings != null) ? mSettings.UseRealFontSize : (settings != null) ? settings.UseRealFontSize : config.UseRealFontSize;


                FontConfiguration defaultFont = (mSettings != null && mSettings.DefaultFont != null) ? mSettings.DefaultFont : (settings != null && settings.DefaultFont != null) ? settings.DefaultFont : config.DefaultFont;
                FontConfiguration defaultRealFont = (mSettings != null && mSettings.DefaultRealFont != null) ? mSettings.DefaultRealFont : (settings != null && settings.DefaultRealFont != null) ? settings.DefaultRealFont : config.DefaultRealFont;
                List<FontSettings> fontSizeSettings = config.FontSizeSettings;
                List<String> fitems = fontfamily.Split(',').Where(s=> !String.IsNullOrEmpty(s)).ToList();
                

                String dFont = "";
                if (!useRealFontSize && defaultFont!=null && fitems.Select(s => s.ToLower()).Contains(defaultFont.Family.ToLower()))
                    dFont = defaultFont.Family;
                else if (useRealFontSize && defaultRealFont != null && fitems.Select(s => s.ToLower()).Contains(defaultRealFont.Family.ToLower()))
                    dFont = defaultRealFont.Family;
                if (message.StyleSettings != null && !String.IsNullOrEmpty(message.StyleSettings.FontFamily) && fitems.Select(s => s.ToLower()).Contains(message.StyleSettings.FontFamily.ToLower()))
                    dFont = message.StyleSettings.FontFamily;
                View.LoadFontFamily(fitems, dFont);


                List<String> sizeItems = fontSizeSettings.Select(s=> s.Value).ToList();
                
                //fontfamily.Split(',').Where(s=> !String.IsNullOrEmpty(s)).ToList();

                //LoadFontSize

            }
            View.isInitialized = true;
        }

        private Boolean IsSessionTimeout()
        {
            if (UserContext.isAnonymous)
            {
                View.DisplaySessionTimeout();
                return true;
            }
            return false;
        }
    }
}
