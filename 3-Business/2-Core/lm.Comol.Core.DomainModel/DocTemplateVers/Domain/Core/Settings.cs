using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.DocTemplateVers
{
    /// <summary>
    /// Settings base per i documenti PDF/RTF
    /// </summary>
    /// <remarks>
    /// MISURE:
    /// lm.Comol.Core.DomainModel.DocTemplateVers.Helpers.Measure
    /// 
    /// Contiene le funzioni per convertire le misure utilizzate: iTextSharp utilizza PX!
    /// Contiene inoltre una funzione che restituisce le misure di pagina a partire dal formato.
    /// 
    /// Da valutare QUALI unità di misura e QUALI dimensioni utilizzare, es:
    /// Unità: mm, cm, inch
    /// Dimensioni: A4, A4L, A3, A3L, Letter...
    /// </remarks>
    [Serializable]
    public class Settings : lm.Comol.Core.DomainModel.DomainBaseObjectMetaInfo<Int64>
    {
        /// <summary>
        /// Sottoversione dell'elemento per il varsioning
        /// </summary>
        public virtual Int64 SubVersion { get; set; }
        /// <summary>
        /// La versione del Template a cui appartiene
        /// </summary>
        public virtual TemplateVersion TemplateVersion { get; set; }
        /// <summary>
        /// Dovrebbe essercene SOLO uno Attivo, gli altri sono versioni precedenti...
        /// </summary>
        public virtual Boolean IsActive { get; set; }

        //Dati PDF
        /// <summary>
        /// Pdf Title
        /// </summary>
        public virtual String Title { get; set; }
        /// <summary>
        /// Pdf Subject
        /// </summary>
        public virtual String Subject { get; set; }
        /// <summary>
        /// Pdf Author
        /// </summary>
        public virtual String Author { get; set; }
        /// <summary>
        /// Pdf Creator
        /// </summary>
        public virtual String Creator { get; set; }
        /// <summary>
        /// Pdf Producer
        /// </summary>
        public virtual String Producer { get; set; }
        /// <summary>
        /// Pdf Keywords
        /// </summary>
        public virtual String Keywords { get; set; }

        /// <summary>
        /// Page Size (Format)
        /// </summary>
        /// <remarks>
        /// Internamente il formato della pagina viene convertito in Width/Heigh ad esclusione del caso in cui PageSize = Custom
        /// </remarks>
        public virtual PageSize Size { get; set; }
        /// <summary>
        /// Page Width
        /// </summary>
        public virtual Single Width { get; set; }
        /// <summary>
        /// Page Hieght
        /// </summary>
        public virtual Single Height { get; set; }

        //Margini
        //Aggiungere eventualmente in creazione la possibilità di impostare un margine x tutti...
        /// <summary>
        /// Right margin in px between content and pageborder
        /// </summary>
        public virtual Single MarginRight { get; set; }
        /// <summary>
        /// Left margin in px between content and pageborder
        /// </summary>
        public virtual Single MarginLeft { get; set; }
        /// <summary>
        /// Top  margin in px between content and pageborder
        /// </summary>
        public virtual Single MarginTop { get; set; }
        /// <summary>
        /// Bottom  margin in px between content and pageborder
        /// </summary>
        public virtual Single MarginBottom { get; set; }

        /// <summary>
        /// Background Image Pagh - PDF only!
        /// </summary>
        /// <remarks>
        /// Immagine sfondo (da valutare: SOLO PDF?!) 
        /// Sarà necessario creare un'immagine in una tabella con posizione assuluta (vedi footer)
        /// aggiunta al doc quando viene generato l'Header
        /// </remarks>
        public virtual String BackgroundImagePath { get; set; }
        /// <summary>
        /// Backgound image placing
        /// </summary>
        public virtual BackgrounImagePosition BackGroundImageFormat { get; set; }

        //Colore sfondo
        //si crea un rettangolo delle dimensioni della pagina con i colori indicati.
        //Eventuali HELPER di conversione da stringa RBG a int...
        //http://stackoverflow.com/questions/1926601/itextsharp-how-do-i-set-the-background-color-of-the-document
        /// <summary>
        /// Red value for Background color
        /// </summary>
        public virtual Int16 BackgroundRed { get; set; }
        /// <summary>
        /// Green value for Background color
        /// </summary>
        public virtual Int16 BackgroundGreen { get; set; }
        /// <summary>
        /// Blue  value for Background color
        /// </summary>
        public virtual Int16 BackgroundBlue { get; set; }
        /// <summary>
        /// Alpha chanel value for Background color
        /// </summary>
        public virtual Int16 BackgroundAlpha { get; set; }

        /// <summary>
        /// lm.Comol.Core.DomainModel.Helpers.Export.PageMaskingInclude
        /// </summary>
        /// <remarks>
        /// Il valore è la somma dei seguenti valori:
        /// none = 0,
        /// All = 1,
        /// Even = 2,
        /// Odd = 4,
        /// First = 8,
        /// Last = 16,
        ////Range = 32
        /// </remarks>
        public virtual Int16 PagePlacingMask { get; set; }
        /// <summary>
        /// Stringa che indica in quali pagine posizionare Header e Footer
        /// </summary>
        /// <example>
        /// 1, 3-6 indica la pagine 1, 3, 4, 5, 6.
        /// </example>
        public virtual String PagePlacingRange { get; set; }


        public virtual Settings Copy(
            TemplateVersion TemplateVersion, Boolean IsActive, int Version
            ,Person Person, String ipAddrees, String IpProxyAddress)
        {
            Settings NewSettings = new Settings();
            NewSettings.CreateMetaInfo(Person, ipAddrees, IpProxyAddress);

            NewSettings.TemplateVersion = TemplateVersion;
            NewSettings.SubVersion = SubVersion;
            NewSettings.IsActive = IsActive;

            NewSettings.Title = this.Title;
            NewSettings.Subject = this.Subject;
            NewSettings.Author = this.Author;
            NewSettings.Creator = this.Creator;
            NewSettings.Producer = this.Producer;
            NewSettings.Keywords = this.Keywords;

            //NewSettings.HasHeaderOnFirstPage = this.HasHeaderOnFirstPage;

            NewSettings.Size = this.Size;//db
            NewSettings.Width = this.Width;
            NewSettings.Height = this.Height;

            NewSettings.MarginRight = this.MarginRight;
            NewSettings.MarginLeft = this.MarginLeft;
            NewSettings.MarginTop = this.MarginTop;
            NewSettings.MarginBottom = this.MarginBottom;

            NewSettings.BackgroundImagePath = this.BackgroundImagePath;
            NewSettings.BackGroundImageFormat = this.BackGroundImageFormat;//db

            NewSettings.BackgroundRed = this.BackgroundRed;
            NewSettings.BackgroundGreen = this.BackgroundGreen;
            NewSettings.BackgroundBlue = this.BackgroundBlue;
            NewSettings.BackgroundAlpha = this.BackgroundAlpha;

            //NewSettings.ShowPageNumber = this.ShowPageNumber;
            //NewSettings.PageNumberAlignment = this.PageNumberAlignment;

            NewSettings.PagePlacingMask = this.PagePlacingMask;
            NewSettings.PagePlacingRange = this.PagePlacingRange;

            return NewSettings;
        }
    }
}
