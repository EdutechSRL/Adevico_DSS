using System;
using System.Collections.Generic;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Core.BaseModules.Editor
{
    public interface IViewBaseEditor : lm.Comol.Core.DomainModel.Common.iDomainView
    {
        String FontNames { get; set; }
        String FontSizes { get; set; }
        String RealFontSizes { get; set; }
        Boolean UseRealFontSize { get; set; }
        String EditorClientId { get; }
        Int32 EditorIdCommunity { get; set; }
        Int32 EditorIdUser { get; set; }
        Int32 EditorIdLanguage { get; set; }
        Boolean isInitialized {get;set;}
        Boolean IsEnabled { get; set; }
        Boolean RenderAsDiv { get; set; }
        Boolean AllowHTMLTags {get;set;}
        Boolean AllowPreview { get; set; }
        Boolean StripFormattingOnPaste { get; set; }
      //  FontConfiguration DefaultFontConfiguration { get; }
        String Text { get; }
        String DefaultFontName { get; set; }
        String DefaultFontSize { get; set; }
        String DefaultColor { get; set; }
        String DefaultBackground { get; set; }
        //String ContainerCssClass { get; set; }
        String EditorCssClass { get; set; }
        String EnabledTags { get; set; }
        String DisabledTags { get; set; }
        String EditorWidth { get; set; }
        String EditorHeight { get; set; }
        ItemPolicy AutoResizeHeight { get; set; }
        ItemPolicy SetDefaultFont { get; set; }
        Boolean AllowAnonymous { get; set; }

        String OnClientCommandExecuted  { get; set; }
        String OnClientLoadScript  { get; set; }

        long MaxTextLength { get; set; }
        long MaxHtmlLength { get; set; }
        EditorNewLineModes NewLineMode { get; set; }

        void DisplayPreview();
        void DisplayAsLabel();
        /// <summary>
        /// Allow to display "HTMLedit" on UI"
        /// </summary>
        Boolean AllowHtmlEdit { get; set; }
        /// <summary>
        /// Allow HTML edit to all profile types
        /// </summary>
        Boolean AllowHtmlEditToAll { get; set; }
        /// <summary>
        /// Specify profile types which can edit content as HTML: each type is comma delimited
        /// </summary>
        String AllowHtmlEditTo { get; set; }
    }
}


    //Person ImagesPaths() As String()
    //Person ShowAddSmartTag() As Boolean
    //WriteOnly Person ShowAddImage() As Boolean
    //WriteOnly Person ShowAddDocument() As Boolean
    //ReadOnly Person CustomDialogScript() As String
    //ReadOnly Person UserLanguage() As Lingua
    //ReadOnly Person CurrentCommunity() As Community
    //ReadOnly Person CurrentUser() As Person
    //Person FontSizes() As String
    //Person FontNames() As String
    //Person DisabledTags() As String
    //Sub SetAdvancedTools(ByVal oList As List(Of SmartTag))