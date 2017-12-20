using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;

namespace lm.Comol.Modules.CallForPapers.Domain
{
    [Serializable()]
    [CLSCompliant(true)]
    public class CallPrintSettings :
        DomainBaseObjectLiteMetaInfo<long>
    {
        public CallPrintSettings()
        {
            TemplateId = 0;
            VersionId = 0;
            CallId = 0;
            UnselectFields = ShowUnselect.HideAll;
            Layout = PageLayout.LeftRight;
            ShowMandatory = false;
            SectionTitle = new CallPrintFontSets { FontName = "Arial", Size = 16, Variant = FontVariant.Bold };
            SectionDescription = new CallPrintFontSets { FontName = "Arial", Size = 14, Variant = FontVariant.Italic };
            FieldTitle = new CallPrintFontSets { FontName = "Arial", Size = 12, Variant = FontVariant.Bold };
            FieldDescription = new CallPrintFontSets { FontName = "Arial", Size = 12, Variant = FontVariant.Italic };
            FieldContent = new CallPrintFontSets { FontName = "Arial", Size = 12, Variant = FontVariant.None };
        }

        public virtual Int64 TemplateId { get; set; }
        public virtual Int64 VersionId { get; set; }
        public virtual Int64 CallId { get; set; }

        public virtual ShowUnselect UnselectFields { get; set; }

        public virtual PageLayout Layout { get; set; }

        public virtual bool ShowMandatory { get; set; }

        public virtual CallPrintFontSets SectionTitle { get; set; }
        public virtual CallPrintFontSets SectionDescription { get; set; }
        public virtual CallPrintFontSets FieldTitle { get; set; }
        public virtual CallPrintFontSets FieldDescription { get; set; }
        public virtual CallPrintFontSets FieldContent { get; set; }

        public virtual bool AllowPrintDraft { get; set; }
        public virtual string DraftWaterMark { get; set; }
    }

    [Serializable]
    [CLSCompliant(true)]
    public class CallPrintFontSets
    {
        public virtual string FontName { get; set; }
        public virtual Int16 Size { get; set; }
        public virtual FontVariant Variant { get; set; }

    }

    [Flags]
    public enum FontVariant : byte
    {
        None = 0,
        Bold = 1,
        Italic = 2,
        Underline = 4
    }

    public enum ShowUnselect : byte
    {
        HideAll = 0,
        ShowAll = 1
    }

    public enum PageLayout : byte
    {
        Standard = 0,
        LeftRight = 1,
        LeftRight1090 = 19,
        LeftRight2080 = 28,
        LeftRight3070 = 37,
        LeftRight4060 = 46,
        LeftRight5050 = 55,
        LeftRight6040 = 64,
        LeftRight7030 = 73,
        LeftRight8020 = 82,
        LeftRight9010 = 91

    }


}
