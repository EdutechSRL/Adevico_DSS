using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;
using lm.Comol.Core.DomainModel.Helpers.Export;

namespace lm.Comol.Core.DomainModel.DocTemplate.Temporary_Helper
{
    public abstract class OLD_iTextSharpHelper
    {
        /// <summary>
        /// Genera ed invia via HTTP.response un PDF
        /// </summary>
        /// <param name="FileName">Nome del file in output</param>
        /// <param name="Response">HTTP.Response</param>
        /// <param name="PageSettings">Impostazioni PDF (dimensione pagina, etc...) SE NULL usa valori di default</param>
        /// <param name="Header">Se != NULL imposta l'header a seconda di questo parametro</param>
        /// <param name="Footer">Se != NULL imposta l'header a seconda di questo parametro</param>
        /// <remarks>
        /// Da queste verranno derivate altre 2 funzioni:
        ///     1. SOLO con PageSetting (no render di Hader e Footer)
        ///     2. Con Template (render di settings, Header, Footer e conversione dei TAG).
        /// PS: la conversione dei tag può essere implementata anche restituendo semplicemente l'oggetto in input,
        /// in questo caso non verrà effettuata alcuna conversione.
        /// </remarks>
        public void HTMLSendPDF(
            String FileName,
            System.Web.HttpResponse Response,
            lm.Comol.Core.DomainModel.Helpers.Export.PageSettings PageSettings,
            DocTemplate.TemplateHeaderFooter Header,
            DocTemplate.TemplateHeaderFooter Footer)
        {
            Response.Clear();

            
            Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ".pdf");
            Response.ContentType = "application/pdf";
            
            //text.Document doc = InitDocument(PageSettings);
            //text.pdf.PdfWriter pdfWriter = text.pdf.PdfWriter.GetInstance(doc, Response.OutputStream());
            
            //ORA, SE si vogliono le intestazioni sulla prima pagina,
            //Quanto segue VA PRIMA di doc.Open

            


            ////SE mi vengono passati HEADER e/o FOOTER,
            ////li gestisco alla solita maniera, altrimenti si dovrà arrangiare chi implementa la compilazione dell'aggeggio.
            if (Header != null || Footer == null)
            {
                if (PageSettings.HasHeaderOnFirstPage == false)
                {
                    //doc.Open();
                }
                ////Rivedere la "traduzione" in C#
                //MyPageEventHandler ev = New MyPageEventHandler(Me.CTRL_Title.HTML);
                //pdfWriter.PageEvent = ev;
                if (PageSettings.HasHeaderOnFirstPage == true)
                {
                    //doc.Open();
                }
            }
            
            //Conversione dei tag (Nella funzione con Template)
            //TagConversion()

            ////Compilazione documento
            //RenderDocument(doc, Response.OutputStream);


            //Se il doc non è stato chiuso precedentemente, lo chiudo!
            //if (doc.IsOpen) { doc.Close(); }

            Response.End();
        }

        ///// <summary>
        ///// Restituisce un nuovo Document con preimpostati i vari parametri:
        ///// Dimensione pagina, sfondo, margini, etc...
        ///// </summary>
        ///// <param name="PageSettings">Le varie impostazioni</param>
        ///// <returns>Un nuovo documento correttamente impostato.</returns>
        //private text.Document InitDocument(DocTemplate.PageSettings PageSettings)
        //{
        //    if(PageSettings == null)
        //    { PageSettings = GetDefaultSettings(); }

        //    //Impostare i vari parametri di DOC


        //    return new text.Document();
        //}


        /// <summary>
        /// Eventualmente inserire il tutto nel costruttore/i di DocTemplate.PageSettings()
        /// </summary>
        /// <returns></returns>
        public static PageSettings GetDefaultSettings()
        {
            PageSettings Settings = new PageSettings();
            
            Settings.Author = "";
            Settings.Creator = "";
            Settings.Keywords = "";
            Settings.Producer = "";
            Settings.Subject = "";
            Settings.Title = "";

            //Se Alpha = 0 => no background!
            Settings.BackgroundAlpha = 0;
            Settings.BackgroundBlue = 0;
            Settings.BackgroundGreen = 0;
            Settings.BackgroundRed = 0;

            //Se image path = "" || image non esiste => no sfondo 
            Settings.BackgroundImagePath = "";
            Settings.BackGroundImageFormat = BackgrounImageFormat.Center;
            
            Settings.Height = 0; //<- Recuperare da "A4" verticale!!!!
            Settings.Width = 0;  //<- Recuperare da "A4" verticale!!!!

            Settings.MarginBottom = 0;
            Settings.MarginLeft = 0;
            Settings.MarginRight = 0;
            Settings.MarginTop = 0;

            //Se ShowPageNumber == false => no numeri pagina
            Settings.ShowPageNumber = false;
            Settings.PageNumberAlignment = ElementAlignment.BottomCenter;

            return Settings;
        }

        ///// <summary>
        ///// Funzione che DEVE essere implementata per la creazione vera e propria del documento.
        ///// </summary>
        ///// <param name="doc"></param>
        ///// <param name="stream"></param>
        ///// <remarks>
        ///// SE non viene passato Header e/o Footer, necessario aprire il documento.
        ///// QUESTO per permettere di gestire eventi di pagina anche sulla prima pagina.
        ///// </remarks>
        //abstract public void RenderDocument(ref text.Document doc, ref System.IO.Stream stream)
        //{
        //    if(!doc.IsOpen) { doc.Open();}
        //}

        /// <summary>
        /// Dovrà implementare autonomamente la conversione dei tag:
        ///     1. Generici
        ///     2. di Servizio
        /// </summary>
        /// <param name="Content">
        ///     L'oggetto content che rappresenta il contenuto. Verranno semplicemente sostituiti vari tag
        ///     con gli opportuni contenuti
        /// </param>
        /// <param name="ServicesContents">
        ///     L'elenco dei servizi associati a tale Template.
        ///     Tali servizi dovranno in qualche modo fornire i metodi per la sostituizione dei tag con gli opportuni contenuti.
        /// </param>
        /// <returns></returns>
        //abstract public TextElement TagConversion(TextElement Content, IList<ServiceContent> ServicesContents)
        //{
        //    return Content;
        //}
    }
}
