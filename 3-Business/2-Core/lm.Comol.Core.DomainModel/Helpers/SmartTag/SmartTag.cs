using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace lm.Comol.Core.DomainModel.Helpers.Tags
{
    [Serializable(), XmlInclude(typeof(LatexSmartTag))]
    public class SmartTag
    {
        public String DisplayWidth { get; set; }
        public String DisplayHeight { get; set; }
        public String Name { get; set; }
        public String Tag { get; set; }
        public String Html { get; set; }
        public List<ReplaceIt> ReplaceList { get; set; }

        public String RegularEx { get; set; }
        public Boolean NeedEncoding { get; set; }
        public static String OpenTag
        {
            get { return "[{0}]"; }
        }
        public static String CloseTag
        {
            get { return "[/{0}]"; }
        }
        public String Image { get; set; }
        public SmartTag()
        {
        }
        public SmartTag(String name, String tag, String html, List<ReplaceIt> items = null, String regex = "", Boolean encoding = false, String image = "")
        {
            this.Name = name;
            this.Tag = tag;
            this.Html = html;
            this.NeedEncoding = encoding;
            if (items != null)
                ReplaceList = items;

            this.RegularEx = regex;
            this.Image = image;
        }
        public virtual String TagIt(string code)
        {
            if (NeedEncoding)
                code = System.Web.HttpUtility.UrlEncode(code);
            if (ReplaceList != null)
            {
                foreach (ReplaceIt replacer in ReplaceList)
                {
                    code = (replacer.Regex) ? Regex.Replace(code, replacer.Original, replacer.Replaced) : code.Replace(replacer.Original, replacer.Replaced);
                }
            }
            try
            {
                return (NeedEncoding) ? String.Format(Html, code, System.Web.HttpUtility.UrlDecode(code)): String.Format(Html, code);
            }
            catch (Exception ex)
            {
                return (NeedEncoding) ? Html.Replace("{0}", code).Replace("{1}", System.Web.HttpUtility.UrlDecode(code)) :Html.Replace("{0}", code);
            }
        }

        //Public Function DeTagIt(ByVal code As String) As String
        //    Return code
        //End Function

        //Public Function Comment() As String
        //    Return ""
        //End Function
        public override String ToString()
        {
            return "[" + Tag + "]";
        }
    }

    [Serializable()]
    public class YoutubeSmartTag : SmartTag
    {

    //Public Sub New(Optional ByVal name As String = "YouTube", Optional ByVal tag As String = "youtube", Optional ByVal width As Integer = 425, Optional ByVal height As Integer = 355)
    //	MyBase.New(name, tag, "<object width=" + CStr(width) + " height=" + CStr(height) + "><param name=""movie"" value=""{0}&hl=en""></param><param name=""wmode"" value=""transparent""></param><embed src=""{0}&hl=en"" type=""application/x-shockwave-flash"" wmode=""transparent"" width=" + CStr(width) + " height=" + CStr(height) + "></embed></object>")
    //	Me.ReplaceList = New ArrayList
    //	Me.ReplaceList.Add(New ReplaceIt("watch?v=", "v/"))
    //End Sub

        public YoutubeSmartTag(String name = "YouTube", String tag = "youtube", int width = 425, int height = 355, List<ReplaceIt> items = null, String iImage = "")
            : base(name, tag, "<object width=" + Convert.ToString(width) + " height=" + Convert.ToString(height) + "><param name=\"movie\" value=\"{0}&hl=en\"></param><param name=\"wmode\" value=\"opaque\"></param><embed src=\"{0}&hl=en\" type=\"application/x-shockwave-flash\" wmode=\"opaque\" width=" + Convert.ToString(width) + " height=" + Convert.ToString(height) + "></embed></object>", items, "", false, iImage)
	    {
            //ReplaceList = items;
	    }
	    public override String TagIt(String code)
	    {
		    return base.TagIt(code);
	    }
    }

    [Serializable()]
    public class LatexSmartTag : SmartTag
    {
        public Int32 Dpi { get; set; }
        public String RenderUrl { get; set; }
        public String RenderPopupUrl { get; set; }
        //public LatexSmartTag(Strng address, String name = "Latex", String tag = "latex", String address = "http://localhost/Comunita_online/SmartTagsRender/latexImage.aspx", int dpi = 160, string addressPopUp = "http://localhost/Comunita_online/SmartTagsRender/LatexPopUp.aspx", string iImage = "") : base(name, tag, "<a href=\"\" onclick=\"javascript:return LatexPopup(this,'" + addressPopUp + "');\" class='latexhref' title='{0}'><img class='lateximg' src='" + address + "?{0}\\dpi{" + Convert.ToString(dpi) + "}' alt='{1}' title='{1}'/></a>", , , true, iImage)

        public LatexSmartTag()
        {
        }

        public String InlineRender()
        {
            return "<a href=\"\" onclick=\"javascript:return LatexPopup(this,'" + RenderPopupUrl + "');\" class='latexhref' title='{0}'><img class='lateximg' src='" + RenderUrl + "?{0}\\dpi{" + Convert.ToString((Dpi <= 0) ? "160" : Dpi.ToString()) + "}' alt='{1}' title='{1}'/></a>";
        }
        //public LatexSmartTag(String address, String addressPopUp, String name = "Latex", String tag = "latex", int dpi = 160, String iImage = "")
        //    : base(name, tag, "<a href=\"\" onclick=\"javascript:return LatexPopup(this,'" + addressPopUp + "');\" class='latexhref' title='{0}'><img class='lateximg' src='" + address + "?{0}\\dpi{" + Convert.ToString(dpi) + "}' alt='{1}' title='{1}'/></a>", null, "", true, iImage)
        //{
        //}
    }

    #region "Removed"
    
    

    //[Serializable(), CLSCompliant(true)]
    //public class SlideShareSmartTag : SmartTag
    //{

    //    public SlideShareSmartTag(String name = "SlideShare", String tag = "slideshare", int width = 425, int height = 355, List<ReplaceIt> items = null, String iImage = "")
    //        : base(name, tag, "<object style=\"margin:0px\" width=" + Convert.ToString(width) + " height=" + Convert.ToString(height) + "><param name=\"movie\" value=\"http://static.slideshare.net/swf/ssplayer2.swf?doc={0}\"/><param name=\"allowFullScreen\" value=\"true\"/><param name=\"allowScriptAccess\" value=\"always\"/><embed src=\"http://static.slideshare.net/swf/ssplayer2.swf?doc={0}\" type=\"application/x-shockwave-flash\" allowscriptaccess=\"always\" allowfullscreen=\"true\" width=" + Convert.ToString(width) + " height=" + Convert.ToString(height) + "></embed></object>", items, "", false, iImage)
    //    {
    //        //Me.ReplaceList = New ArrayList
    //        //Me.ReplaceList.Add(New ReplaceIt("\[slideshare id=.*&doc=", "", True))
    //        //Me.ReplaceList.Add(New ReplaceIt("&w=.*\]", "", True))
    //    }
    //}

    //[Serializable(), CLSCompliant(true)]
    //public class DocStocSmartTag : SmartTag
    //{
    //    public DocStocSmartTag(String name = "DocStoc", String tag = "docstoc", int width = 425, int height = 355, string iImage = "") : base(name, tag, "<iframe width=" + Convert.ToString(width) + " height=" + Convert.ToString(height) + " src=\"http://www.docstoc.com/docs/document-preview.aspx?doc_id={0}\"/>",null,"",false,iImage)
    //    {
    //    }
    //}


    #endregion

    //[Serializable()]
    //public class WellKnownSmartTag
    //{
    //    private SmartTags _SmartTags = new SmartTags();
    //    public WellKnownSmartTag()
    //    {
    //        //Dim youtube As New YoutubeSmartTag()
    //        LatexSmartTag latex = new LatexSmartTag();

    //        //Dim dmlist As IList = New ArrayList
    //        //dmlist.Add(New ReplaceIt("/video", "/swf"))
    //        //dmlist.Add(New ReplaceIt("/it/featured", ""))
    //        //dmlist.Add(New ReplaceIt("_.*", "", True))
    //        //Dim dailymotion As New SmartTag("DailyMotion", "dailymotion", "<object width=""420"" height=""339""><param name=""movie"" value=""{0}"" /><param name=""allowFullScreen"" value=""true"" /><param name=""allowScriptAccess"" value=""always"" /><embed src=""{0}"" type=""application/x-shockwave-flash"" width=""420"" height=""339"" allowFullScreen=""true"" allowScriptAccess=""always""></embed></object>", dmlist)

    //        //Dim bold As New SmartTag("bold", "b", "<b>{0}</b>")

    //        //Dim slideshare As New SlideShareSmartTag()

    //        //Dim docstoc As New DocStocSmartTag()

    //        //   smts.add(youtube)
    //        _SmartTags.Add(latex);
    //        //  smts.add(dailymotion)
    //        //  smts.add(slideshare)
    //        //  smts.add(docstoc)
    //        //  smts.add(bold)
    //    }

    //    public String TagAll(string html)
    //    {
    //        return _SmartTags.TagAll(html);
    //    }
    //}
}