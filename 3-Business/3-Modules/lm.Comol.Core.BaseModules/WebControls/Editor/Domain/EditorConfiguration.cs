using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.BaseModules.Editor
{
    [Serializable()]
    public class EditorConfiguration
    {
        private Boolean useRealFontSize;
        public virtual String DefaultCssFilesPath { get; set; }
        public virtual String ImageHandlerPath { get; set; }
        public virtual FontConfiguration DefaultFont { get; set; }
        public virtual FontConfiguration DefaultRealFont { get; set; }
        public virtual String FontNames { get; set; }
        public virtual String FontSizes { get; set; }
        public virtual String RealFontSizes { get; set; }
        public virtual Boolean UseRealFontSize
        {
            get
            {
                return (
                        (useRealFontSize && !String.IsNullOrEmpty(RealFontSizes))
                        ||
                        (!useRealFontSize && !String.IsNullOrEmpty(RealFontSizes) && String.IsNullOrEmpty(FontSizes))
                        );
            }
            set { useRealFontSize = value; }
        }
        public virtual List<EditorCssFile> CssFiles { get; set; }
        public virtual List<lm.Comol.Core.DomainModel.Helpers.Tags.SmartTag> AvailableSmartags { get; set; }
        public virtual List<AdvancedLinkItem> AvailableItemsToLink { get; set; }
        public virtual List<EditorToolbar> AvailableToolbars { get; set; }


        public virtual EditorSettings DefaultEditor { get; set; }
        public virtual List<EditorSettings> Settings { get; set; }
        public virtual List<ModuleEditorSettings> ModuleSettings { get; set; }
        public virtual List<FontSettings> FontSizeSettings { get; set; }

        
        public EditorConfiguration() {
            Settings = new List<EditorSettings>();
            ModuleSettings = new List<ModuleEditorSettings>();
            AvailableSmartags = new List<lm.Comol.Core.DomainModel.Helpers.Tags.SmartTag>();
            AvailableItemsToLink = new List<AdvancedLinkItem>();
            AvailableToolbars = new List<EditorToolbar>();
            FontSizeSettings = new List<FontSettings>();
            CssFiles = new List<EditorCssFile>();
        }

    }
    [Serializable()]
    public class EditorCssFile
    {
        public virtual String FileName { get; set; }
        public virtual Boolean isFullAddress { get; set; }
        public EditorCssFile()
        {
            isFullAddress = false;
        }
    }
    [Serializable()]
    public class BaseEditorSettings
    {
        private Boolean useRealFontSize;
        public virtual String EnabledTags { get; set; }
        public virtual String DisabledTags { get; set; }
        public virtual String Width { get; set; }
        public virtual String Height { get; set; }
        public virtual ItemPolicy AutoResizeHeight { get; set; }
        public virtual ItemPolicy SetDefaultFont { get; set; }
        public virtual String FontNames { get; set; }
        public virtual String FontSizes { get; set; }
        public virtual String RealFontSizes { get; set; }
        public virtual FontConfiguration DefaultFont { get; set; }
        public virtual FontConfiguration DefaultRealFont { get; set; }
        public virtual Boolean UseRealFontSize { 
            get
            { 
                return (
                        (useRealFontSize && !String.IsNullOrEmpty(RealFontSizes))
                        ||
                        (!useRealFontSize && !String.IsNullOrEmpty(RealFontSizes) && String.IsNullOrEmpty(FontSizes))
                        ); 
            } 
            set { useRealFontSize = value; } 
        }
        public virtual ToolbarType ToolbarType { get; set; }
        public virtual EditorType EditorType { get; set; }
        public virtual List<EditorCssFile> CssFiles { get; set; }
        /// <summary>
        /// Allow to display "HTMLedit" on UI"
        /// </summary>
        public virtual Boolean AllowHtmlEdit { get; set; }
        
        /// <summary>
        /// Allow HTML edit to all profile types
        /// </summary>
        public virtual Boolean AllowHtmlEditToAll { get; set; }
        /// <summary>
        /// Set profile types which can edit text as HTML
        /// </summary>
        public virtual List<Int32> AllowedHtmlEditProfileTypes { get; set; }
        //public virtual List<lm.Comol.Core.DomainModel.Helpers.Tags.SmartTag> Smartags { get; set; }
        //public virtual List<InternalTools> InternalTools { get; set; }
        public BaseEditorSettings()
        {
            ToolbarType = Editor.ToolbarType.none;
            CssFiles = new List<EditorCssFile>();
            //InternalTools = new List<InternalTools>();
            //Smartags = new List<lm.Comol.Core.DomainModel.Helpers.Tags.SmartTag>();
            AutoResizeHeight = ItemPolicy.notallowed;
            AllowedHtmlEditProfileTypes = new List<Int32>();
        }
    }
    [Serializable()]
    public class EditorSettings : BaseEditorSettings
    {
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual String ToolsPath { get; set; }
        
        public EditorSettings() : base()
        {
        }
    }
    [Serializable()]
    public class ModuleEditorSettings : BaseEditorSettings
    {
        public virtual Boolean OvverideCssFileSettings { get; set; }
        public virtual Boolean AllowOvveride { get; set; }
        public virtual Boolean AllowMultipleFontFamily { get; set; }
        public virtual Boolean AllowMultipleFontSize { get; set; }
        public virtual String ModuleCode { get; set; }
        public ModuleEditorSettings()
            : base()
        {

        }
    }
    [Serializable()]
    public class FontConfiguration
    {
        public virtual String Family { get; set; }
        public virtual String Size { get; set; }
        public virtual String Background { get; set; }
        public virtual String Color { get; set; }
        public virtual Boolean IsRealFontSize { get; set; }
        public FontConfiguration()
        {

        }
    }

    [Serializable()]
    public class FontSettings
    {
        public virtual String Value { get; set; }
        public virtual List<FontSize> RenderSize { get; set; }

        public FontSettings() {
            RenderSize = new List<FontSize>();
        }

        public virtual String GetRenderFont(FontSizeType type)
        {
            return (RenderSize.Any() && RenderSize.Where(f => f.Type == type).Any()) ? RenderSize.Where(f => f.Type == type).FirstOrDefault().ToString() : "";
        }
    }

    [Serializable()]
    public class FontSize
    {
        public virtual String DisplayName { get; set; }
        public virtual String DisplaySize { get; set; }
        public virtual String Size { get; set; }
        public virtual FontSizeType Type { get; set; }
        
        //public virtual String ToValue() { 
        //    return UserSize + TypeToString();
        //}
        public virtual String ToString()
        {
            switch (Type)
            {
                case FontSizeType.em:
                    return Size + "em";
                case FontSizeType.pixel:
                    return Size + "px";
                case FontSizeType.point:
                    return Size + "pt";
                default:
                    return Size + "px";
            }
        }
    }
    [Serializable()]
    public enum FontSizeType
    {
        pixel = 0,
        point= 1,
        em = 2
    }


    [Serializable()]
    public class EditorInitializer
    {
        private Boolean _UseRealFontSize;
        public virtual String EnabledTags { get; set; }
        public virtual String DisabledTags { get; set; }
        public virtual String Width { get; set; }
        public virtual String Height { get; set; }
        public virtual ItemPolicy AutoResizeHeight { get; set; }
        public virtual ItemPolicy SetDefaultFont { get; set; }
        public virtual String ToolsPath { get; set; }
        public virtual String DefaultCssFilesPath { get; set; }
        public virtual EditorToolbar Toolbar { get; set; }
        public virtual String FontNames { get; set; }
        public virtual String FontSizes { get; set; }
        public virtual String RealFontSizes { get; set; }
        public virtual Boolean UseRealFontSize
        {
            get
            {
                return (
                        (_UseRealFontSize && !String.IsNullOrEmpty(RealFontSizes))
                        ||
                        (!_UseRealFontSize && !String.IsNullOrEmpty(RealFontSizes) && String.IsNullOrEmpty(FontSizes))
                        );
            }
            set { _UseRealFontSize = value; }
        }
       
        public virtual List<EditorCssFile> CssFiles { get; set; }
        public virtual Boolean AllowMultipleFontFamily { get; set; }
        public virtual Boolean AllowMultipleFontSize { get; set; }
        public virtual FontConfiguration DefaultFont { get; set; }
        public virtual FontConfiguration DefaultRealFont { get; set; }
        public virtual List<FontSettings> FontSizeSettings { get; set; }
        public virtual List<lm.Comol.Core.DomainModel.Helpers.Tags.SmartTag> Smartags { get; set; }
        public virtual List<AdvancedLinkItem> ItemsToLink { get; set; }
        public virtual Boolean AllowEditHTML { get; set; }
        public EditorInitializer()
        {
            Smartags = new List<lm.Comol.Core.DomainModel.Helpers.Tags.SmartTag>();
            ItemsToLink = new List<AdvancedLinkItem>();
            CssFiles = new List<EditorCssFile>();
            AutoResizeHeight = ItemPolicy.notallowed;
        }

        public virtual String GetRenderFont(FontSizeType type, String fValue)
        {
            return (FontSizeSettings.Any() && FontSizeSettings.Where(f => f.Value == fValue).Any()) ? FontSizeSettings.Where(f => f.Value == fValue).FirstOrDefault().GetRenderFont(type) : "";
        }
    }

    // Points 	Pixels 	Ems 	Percent
    //6pt 	8px 	0.5em 	50%
    //7pt 	9px 	0.55em 	55%
    //7.5pt 	10px 	0.625em 	62.5%
    //8pt 	11px 	0.7em 	70%
    //9pt 	12px 	0.75em 	75%
    //10pt 	13px 	0.8em 	80%
    //10.5pt 	14px 	0.875em 	87.5%
    //11pt 	15px 	0.95em 	95%
    //12pt 	16px 	1em 	100%
    //13pt 	17px 	1.05em 	105%
    //13.5pt 	18px 	1.125em 	112.5%
    //14pt 	19px 	1.2em 	120%
    //14.5pt 	20px 	1.25em 	125%
    //15pt 	21px 	1.3em 	130%
    //16pt 	22px 	1.4em 	140%
    //17pt 	23px 	1.45em 	145%
    //18pt 	24px 	1.5em 	150%
    //20pt 	26px 	1.6em 	160%
    //22pt 	29px 	1.8em 	180%
    //24pt 	32px 	2em 	200%
    //26pt 	35px 	2.2em 	220%
    //27pt 	36px 	2.25em 	225%
    //28pt 	37px 	2.3em 	230%
    //29pt 	38px 	2.35em 	235%
    //30pt 	40px 	2.45em 	245%
    //32pt 	42px 	2.55em 	255%
    //34pt 	45px 	2.75em 	275%
    //36pt 	48px 	3em 	300%
}