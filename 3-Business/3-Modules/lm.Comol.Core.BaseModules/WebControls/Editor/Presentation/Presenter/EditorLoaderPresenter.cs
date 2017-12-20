using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
using lm.Comol.Core.BaseModules.Repository.Business;
using lm.Comol.Core.Business;
using lm.Comol.Core.BaseModules.Editor.Business;
namespace lm.Comol.Core.BaseModules.Editor
{
    public class EditorLoaderPresenter : lm.Comol.Core.DomainModel.Common.DomainPresenter
    {
        #region "Init"
        private int _ModuleID;
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
        protected virtual IViewEditorLoader View
        {
            get { return (IViewEditorLoader)base.View; }
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
        public EditorLoaderPresenter(iApplicationContext oContext):base(oContext){
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        public EditorLoaderPresenter(iApplicationContext oContext, IViewEditorLoader view)
            : base(oContext, view)
        {
            this.CurrentManager = new BaseModuleManager(oContext);
        }
        #endregion

        public void InitView(String configurationPath, String moduleCode, EditorType selectedType, Boolean allowAnonymous)
        {
            if (!allowAnonymous && UserContext.isAnonymous)
                View.CurrentType = EditorType.none;
            else {
                EditorConfiguration config = ServiceEditor.GetConfiguration(configurationPath);
                if (config == null || (config.DefaultEditor == null && !config.Settings.Any()))
                {
                    View.CurrentType = EditorType.none;
                }
                else {
                    ModuleEditorSettings mSettings = (config.ModuleSettings==null) ? null : config.ModuleSettings.Where(m => m.ModuleCode == moduleCode).FirstOrDefault();

                    EditorType loadType = (mSettings !=null ) ?
                                        mSettings.EditorType
                                        : 
                                        ((config.Settings.Any() && config.Settings.Where(s=>s.EditorType== selectedType).Any()) 
                                        ? selectedType : ((config.DefaultEditor !=null) ? config.DefaultEditor.EditorType:  EditorType.none));
                    View.CurrentType = loadType;
                    EditorSettings rSettings = (config.Settings.Any() ? config.Settings.Where(s=> s.EditorType==loadType).FirstOrDefault(): null);
                    EditorInitializer eInitializer = GetInitializer(loadType, config,(rSettings != null) ? rSettings: config.DefaultEditor,mSettings);

                    View.LoadEditor(loadType, eInitializer);
                    View.isInitialized = true;
                }
            }
        }

        private EditorInitializer GetInitializer(EditorType eType, EditorConfiguration config, EditorSettings settings, ModuleEditorSettings mSettings)
        {
            EditorInitializer item = new EditorInitializer();
            //item.SelectedToolbarType = View.Toolbar;
            item.Toolbar = GetToolbar(eType, config, settings, mSettings);
            item.CssFiles = config.CssFiles;
            #region "HTML edit"
            item.AllowEditHTML = AllowEditHtml(settings, mSettings);
                
            #endregion
            #region "Fonts"
            String names = View.FontNames;
                String sizes = View.FontSizes;
                String realFontSizes = View.RealFontSizes;
                item.FontNames = (View.AllAvailableFontnames) ? config.FontNames : (!String.IsNullOrEmpty(names)) ? names : (mSettings != null) ? mSettings.FontNames : (settings != null) ? settings.FontNames : config.FontNames;
                item.FontSizes = (!String.IsNullOrEmpty(sizes)) ? sizes : (mSettings != null) ? mSettings.FontSizes : (settings != null) ? settings.FontSizes : config.FontSizes;
                item.RealFontSizes = (!String.IsNullOrEmpty(realFontSizes)) ? realFontSizes : (mSettings != null) ? mSettings.RealFontSizes : (settings != null) ? settings.RealFontSizes : config.RealFontSizes;
                item.UseRealFontSize = (!String.IsNullOrEmpty(realFontSizes)) ? View.UseRealFontSize : (mSettings != null) ? mSettings.UseRealFontSize : (settings != null) ? settings.UseRealFontSize : config.UseRealFontSize;

              //  FontConfiguration vConfiguration = View.DefaultFontConfiguration;
            //new FontConfiguration();
            //    vConfiguration.Background = View.DefaultBackground;
            //    vConfiguration.Color = View.DefaultColor;
            //    vConfiguration.Family = View.DefaultFontName;
            //    vConfiguration.Size = View.DefaultFontSize;
            //    vConfiguration.IsRealFontSize = View.UseRealFontSize;
                //    (mSettings != null) ? mSettings. : (settings != null) ? settings.FontFamily : config.DefaultEditor.ToolbarType;
                item.DefaultFont = (mSettings != null && mSettings.DefaultFont !=null) ? mSettings.DefaultFont : (settings !=null && settings.DefaultFont !=null) ? settings.DefaultFont:  config.DefaultFont;
                item.DefaultRealFont = (mSettings != null && mSettings.DefaultRealFont != null) ? mSettings.DefaultRealFont : (settings != null && settings.DefaultRealFont != null) ? settings.DefaultRealFont : config.DefaultRealFont;
                item.FontSizeSettings = config.FontSizeSettings;
            #endregion  
            #region "Other Settings"
                ItemPolicy sDefaultFonts = View.SetDefaultFont;
                ItemPolicy aPolicy = View.AutoResizeHeight;

                item.SetDefaultFont = (sDefaultFonts != ItemPolicy.byconfiguration) ? sDefaultFonts : (mSettings != null) ? mSettings.SetDefaultFont : (settings != null) ? settings.SetDefaultFont : ItemPolicy.notallowed;
                item.AutoResizeHeight = (aPolicy != ItemPolicy.byconfiguration) ? aPolicy : (mSettings != null) ? mSettings.AutoResizeHeight : (settings != null) ? settings.AutoResizeHeight :  ItemPolicy.notallowed;
                if (View.DisabledTags =="-")
                    item.DisabledTags = (mSettings != null) ? mSettings.DisabledTags : (settings != null) ? settings.DisabledTags : "";
                else
                    item.DisabledTags = View.DisabledTags;

                if (View.EnabledTags == "-")
                    item.EnabledTags = (mSettings != null) ? mSettings.EnabledTags : (settings != null) ? settings.EnabledTags : "";
                else
                    item.EnabledTags = View.EnabledTags;

                item.Width = (!String.IsNullOrEmpty(View.EditorWidth)) ? View.EditorWidth : (mSettings != null) ? mSettings.Width : (settings != null) ? settings.Width : "";
                item.Height = (!String.IsNullOrEmpty(View.EditorHeight)) ? View.EditorHeight : (mSettings != null) ? mSettings.Height : (settings != null) ? settings.Height : "";
            #endregion  

            #region "Link To"
                String vItemsToLink = View.ItemsToLink;
                List<String> tags = item.EnabledTags.Split(',').ToList();
                item.ItemsToLink = config.AvailableItemsToLink.Where(a => tags.Contains(a.Tag)).ToList();
            #endregion
            #region "Smartags"
                String vSmartags = View.SmartTags;
            #endregion
                item.ToolsPath = (settings != null) ? settings.ToolsPath : (config.DefaultEditor == null) ? "" : config.DefaultEditor.ToolsPath;
            item.DefaultCssFilesPath = config.DefaultCssFilesPath;
            if (mSettings != null)
            {
                item.AllowMultipleFontFamily = mSettings.AllowMultipleFontFamily;
                item.AllowMultipleFontSize = mSettings.AllowMultipleFontSize;
                if (mSettings.OvverideCssFileSettings)
                    item.CssFiles = mSettings.CssFiles;
            }
            return item;
        }

        private EditorToolbar GetToolbar(EditorType eType, EditorConfiguration config, EditorSettings settings, ModuleEditorSettings mSettings) {
            ToolbarType tType = View.Toolbar;
            if (tType == ToolbarType.bySettings)
            {
                tType = (mSettings != null) ? mSettings.ToolbarType : (settings != null) ? settings.ToolbarType : config.DefaultEditor.ToolbarType;
                switch (eType)
                {
                    case EditorType.none:
                    case EditorType.textarea:
                        tType = ToolbarType.none;
                        break;
                    case EditorType.lite:
                        tType = (tType == ToolbarType.full || tType == ToolbarType.advanced) ? ToolbarType.simple : tType;
                        break;
                }
            }
            View.Toolbar = tType;
            return config.AvailableToolbars.Where(t=>t.ToolbarType==tType && t.EditorType==eType ).FirstOrDefault();
        }

        private Boolean AllowEditHtml(EditorSettings settings, ModuleEditorSettings mSettings)
        {
            Boolean allow = false;
            Boolean allowEditHTML = View.AllowHtmlEdit;
            Boolean editToAll = View.AllowHtmlEditToAll;
            String allowTo = View.AllowHtmlEditTo;
            allow = AllowEditHtml(allowEditHTML, editToAll, (!editToAll && !String.IsNullOrEmpty(allowTo)) ? allowTo.Split(',').ToList().Where(t => t.All(c => char.IsDigit(c))).Select(t => Int32.Parse(t)).ToList() : new List<Int32>());
            if (!allow && String.IsNullOrEmpty(allowTo)) {
                if (mSettings!=null)
                    allow = AllowEditHtml(mSettings.AllowHtmlEdit, mSettings.AllowHtmlEditToAll, mSettings.AllowedHtmlEditProfileTypes);
                if (!allow && (mSettings==null || !mSettings.AllowHtmlEdit))
                    allow = AllowEditHtml(settings.AllowHtmlEdit, settings.AllowHtmlEditToAll, settings.AllowedHtmlEditProfileTypes);
            }
            return allow;
        }
        private Boolean AllowEditHtml(Boolean allowEditHTML, Boolean editToAll, List<Int32> types)
        {
            return allowEditHTML && (editToAll || (!editToAll && types.Any() && types.Contains(UserContext.UserTypeID)));
        }
    }
}
