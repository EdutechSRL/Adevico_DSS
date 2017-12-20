using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTS = iTextSharp.text;

namespace lm.Comol.Core.DomainModel.Helpers.Export
{
    //Contiene HELPER specifici per la conversione in RTF.
    //Al esempio il load di stili specifici o la correzione di determinati TAG HTML
    public class RtfHelper
    {
        public static iTextSharp.text.html.simpleparser.StyleSheet GetStyles()
        {
            iTS.html.simpleparser.StyleSheet styles =
new iTextSharp.text.html.simpleparser.StyleSheet();
                
            styles.LoadTagStyle("ul", "indent", "10");
            styles.LoadTagStyle("ol", "indent", "10");
            styles.LoadTagStyle("br", "font-size", "50%");
            //styles.LoadTagStyle("center", "text-align", "center");
            //styles.LoadTagStyle("p.center", "text-align", "center");
            //styles.LoadTagStyle("p", )

            return styles;
        }

        /// <summary>
        /// Elimina i tag che creano problemi durante la conversione
        /// </summary>
        /// <param name="Html">Stringa contenente il codice HTML</param>
        /// <remarks>
        /// Ad esempio il tag "HR" nella vecchia versione di iTextSharp viene ignorato,
        /// mentre nella nuova versione manda in crash il motore di conversione
        /// e viene quindi eliminato.
        /// </remarks>
        public static String HtmlCheckRtf(String Html)
        {
            
            ////Elimino HR <- Da problemi e rischia di mandare in crash l'output
            //outHtml = System.Text.RegularExpressions.Regex.Replace(outHtml, "</?(h|H)(r|R)( )*/?>", string.Empty);

            ////Il TAG <CENTER> è ignorato. Viene quindi sostituito da <p style="text-align: center">
            //String center_open = "<div style=\"text-align: center\">";
            //outHtml = System.Text.RegularExpressions.Regex.Replace(outHtml, "<(c|C)(e|E)(n|N)(t|T)(e|E)(r|R)( )*>", center_open);
            //String center_close = "</div>";
            //outHtml = System.Text.RegularExpressions.Regex.Replace(outHtml, "</(c|C)(e|E)(n|N)(t|T)(e|E)(r|R)( )*>", center_close);

            System.Text.RegularExpressions.RegexOptions Option = System.Text.RegularExpressions.RegexOptions.IgnoreCase;

            Html = System.Text.RegularExpressions.Regex.Replace(Html, "<img[^>]*>", string.Empty, Option);

            Html = System.Text.RegularExpressions.Regex.Replace(Html, "<table[^>]*>", "<div>", Option);
            Html = System.Text.RegularExpressions.Regex.Replace(Html, "<th[^>]*>", "<div>", Option);
            Html = System.Text.RegularExpressions.Regex.Replace(Html, "<tr[^>]*>", "<div>", Option);
            Html = System.Text.RegularExpressions.Regex.Replace(Html, "<td[^>]*>", "<span>", Option);

            Html = System.Text.RegularExpressions.Regex.Replace(Html, "</table[^>]*>", "</div>", Option);
            Html = System.Text.RegularExpressions.Regex.Replace(Html, "</th[^>]*>", "</div>", Option);
            Html = System.Text.RegularExpressions.Regex.Replace(Html, "</tr[^>]*>", "</div>", Option);
            Html = System.Text.RegularExpressions.Regex.Replace(Html, "</td[^>]*>", "</span>", Option);

            return Html;
            
            //</center>
        }

        public static String RemoveHtmlTag(String Html)
        {
            System.Text.RegularExpressions.RegexOptions Option = System.Text.RegularExpressions.RegexOptions.IgnoreCase;

            Html = System.Text.RegularExpressions.Regex.Replace(Html, "<br[^>]*>", "\r\n", Option);
            Html = System.Text.RegularExpressions.Regex.Replace(Html, "<[^>]*>", String.Empty, Option);

            return System.Web.HttpUtility.HtmlDecode(Html);
        }

        public static void SetAlignment(ref iTS.Cell cell, DocTemplateVers.ElementAlignment align)
        {
            switch (align)
            {
                case DocTemplateVers.ElementAlignment.BottomCenter:
                    cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER; // = iTextSharp5.text.Cell.ALIGN_CENTER;
                    cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_BOTTOM;
                    break;
                case DocTemplateVers.ElementAlignment.BottomLeft:
                    cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_BOTTOM;
                    break;
                case DocTemplateVers.ElementAlignment.BottomRight:
                    cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_BOTTOM;
                    break;

                case DocTemplateVers.ElementAlignment.MiddleCenter:
                    cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    break;
                case DocTemplateVers.ElementAlignment.MiddleLeft:
                    cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    break;
                case DocTemplateVers.ElementAlignment.MiddleRight:
                    cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    break;

                case DocTemplateVers.ElementAlignment.TopCenter:
                    cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_TOP;
                    break;
                case DocTemplateVers.ElementAlignment.TopLeft:
                    cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_TOP;
                    break;
                case DocTemplateVers.ElementAlignment.TopRight:
                    cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_TOP;
                    break;

                default:
                    cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_TOP;
                    break;
            }
        }
    }
}
