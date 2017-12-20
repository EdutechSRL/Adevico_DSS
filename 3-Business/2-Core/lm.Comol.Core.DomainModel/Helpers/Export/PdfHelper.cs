using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Helpers.Export
{
    //Contiene HELPER specifici per la conversione in PDF.
    //Al esempio il load di stili specifici o la correzione di determinati TAG HTML
    public class PdfHelper
    {
        /// <summary>
        /// Elimina i tag che creano problemi durante la conversione
        /// </summary>
        /// <param name="Html">Stringa contenente il codice HTML</param>
        /// <remarks>
        /// Ad esempio il tag "HR" nella vecchia versione di iTextSharp viene ignorato,
        /// mentre nella nuova versione manda in crash il motore di conversione
        /// e viene quindi eliminato.
        /// 
        /// Il tag IMG può contenere immagini non raggiungibili che mandano in TIMEOUT la conversione che genere quindi eccezione.
        /// Lato nostro non va in timeout e riesce a mandare comunque il documento, che però rischia di essere corrotto a causa dell'eccezione!
        /// </remarks>
        public static String HtmlCheckPDF(String Html)
        {
            System.Text.RegularExpressions.RegexOptions Option = System.Text.RegularExpressions.RegexOptions.IgnoreCase;

            //Elimino HR <- Da problemi e rischia di mandare in crash l'output
            Html = System.Text.RegularExpressions.Regex.Replace(Html, "</?hr( )*/?>", string.Empty, Option);

            //Il TAG <CENTER> è ignorato. Viene quindi sostituito da <p style="text-align: center">
            String center_open = "<div style=\"text-align: center\">";
            String center_close = "</div>";
            Html = System.Text.RegularExpressions.Regex.Replace(Html, "<center( )*>", center_open, Option);
            Html = System.Text.RegularExpressions.Regex.Replace(Html, "</center( )*>", center_close, Option);

            //L'immagine del tag IMG può non essere raggiungibile e/o la richiesta andare in timeout, quindi bloccare l'EXPORT!
            Html = System.Text.RegularExpressions.Regex.Replace(Html, "<img[^>]*>", string.Empty, Option);

            Html = System.Text.RegularExpressions.Regex.Replace(Html, "<br[^>]*>", "<br />", Option);

            Html = System.Text.RegularExpressions.Regex.Replace(Html, "font-size: smaller;", string.Empty, Option);


            String Times = "<span style=\"font-family: Times;\">";

            //Match insensitive delle varianti del Times New roman, come: 
            // Times New Roman, Times New, TIMES_ROMAN, 'Times New   Roman ', '   Times  '
            // Contanto anche eventuali apici ad inizio/fine e spazi in più
            String RegTimesNewRoman = "<span style=\"font-family: *'? *[Tt][Ii][Mm][Ee][Ss](.[Nn][Ee][Ww])?(.[Rr][Oo][Mm][Aa][Nn])? *'?.?;?\">";

            Html = System.Text.RegularExpressions.Regex.Replace(Html, RegTimesNewRoman, Times, Option);
            
            return Html;
            //</center>
        }

        public static String HtmlReplaceBR(String Html)
        {
            return Html.Replace("\r\n", "<br>");
        }

        public static String HtmlReplaceNbspToBR(String Html)
        {
            const string regPSpan = @"(<p.*><span.*>)&nbsp;(<\/span><\/p>)";
            const string replaceString = @"$1<br/>$2";
            System.Text.RegularExpressions.RegexOptions Option = System.Text.RegularExpressions.RegexOptions.IgnoreCase;

            return System.Text.RegularExpressions.Regex.Replace(Html, regPSpan, replaceString, Option);

        }

        public static String RemoveHtmlTag(String Html)
        {
            System.Text.RegularExpressions.RegexOptions Option = System.Text.RegularExpressions.RegexOptions.IgnoreCase;
            Html = System.Text.RegularExpressions.Regex.Replace(Html, "<br[^>]*>", "\r\n", Option);
            Html = System.Text.RegularExpressions.Regex.Replace(Html, "<[^>]*>", String.Empty, Option);
            Html = Html.Replace("&euro", "€");
            return System.Web.HttpUtility.HtmlDecode(Html);
        }
    }
}