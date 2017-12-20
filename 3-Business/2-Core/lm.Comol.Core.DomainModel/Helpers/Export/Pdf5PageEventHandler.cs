using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using lm.Comol.Core.DomainModel.DocTemplate;
using TemplVers = lm.Comol.Core.DomainModel.DocTemplateVers;
using System.IO;
using iTextSharp.text;
using Document = iTextSharp5.text.Document;
using IElement = iTextSharp5.text.IElement;

namespace lm.Comol.Core.DomainModel.Helpers.Export
{
    public class Pdf5PageEventHandler : iTextSharp5.text.pdf.PdfPageEventHelper
    {
        #region Variabili interne
        //private Boolean _HeaderOnFirstPage;

        private static int _imgFract = 5;

        private float _yPosF;
        private float _xPosF;

        private float _yPosH;
        private float _xPosH;

        private float _BottomMargin = 0; //Diventerà READONLY!!!
        private float _TopMargin = 0; //Diventerà READONLY!!!

        private Dictionary<Int64, Int32> LastPageElementRender { get; set; }
        private int LastHFrenderedPage { get; set; }
        private int CurrentPage { get; set; }

        private int HFPageMask { get; set; }
        private string HFPageRange { get; set; }

        private IList<DocTemplateVers.Domain.DTO.ServiceExport.DTO_Signature> SignaturesPOS { get; set; }
        private IList<DocTemplateVers.Domain.DTO.ServiceExport.DTO_Signature> SignaturesStatic { get; set; }
        //private iTextSharp5.text.pdf.PdfPTable SignatureStatic { get; set; }

        //NOTA SVILUPPO:
        // Al momento è COSì! per agevolare il debug.
        // Successivamente verrà ottimizzato con l'aggiunta di dictionary o classi apposite che contengono i dati a seguire.

        float MaxWidth = 0;


        private int HeadCols = 0;
        private int FootCols = 0;

        private iTextSharp5.text.pdf.PdfPCell HeadLeft { get; set; }
        private iTextSharp5.text.pdf.PdfPCell HeadCenter { get; set; }
        private iTextSharp5.text.pdf.PdfPCell HeadRight { get; set; }

        private iTextSharp5.text.pdf.PdfPCell FootLeft { get; set; }
        private iTextSharp5.text.pdf.PdfPCell FootCenter { get; set; }
        private iTextSharp5.text.pdf.PdfPCell FootRight { get; set; }

        private TemplVers.Domain.DTO.ServiceExport.DTO_ElementText HeadLeftTxt { get; set; }
        private TemplVers.Domain.DTO.ServiceExport.DTO_ElementText HeadCenterTxt { get; set; }
        private TemplVers.Domain.DTO.ServiceExport.DTO_ElementText HeadRightTxt { get; set; }

        private TemplVers.Domain.DTO.ServiceExport.DTO_ElementText FootLeftTxt { get; set; }
        private TemplVers.Domain.DTO.ServiceExport.DTO_ElementText FootCenterTxt { get; set; }
        private TemplVers.Domain.DTO.ServiceExport.DTO_ElementText FootRightTxt { get; set; }

        private Boolean HLtag = true;
        private Boolean HCtag = true;
        private Boolean HRtag = true;

        private Boolean FLtag = true;
        private Boolean FCtag = true;
        private Boolean FRtag = true;

        private string BGpath;
        private TemplVers.BackgrounImagePosition BGformat;
        private float PGwidth;
        private float PGHeight;

        private float PGwidth_full;
        private float PGHeight_full;

        protected iTextSharp5.text.pdf.PdfTemplate total;

        private String Watermark = "";

        #endregion


        #region Proprietà

        /// <summary>
        /// Alla creazione viene passato per riferimento il bordo inferiore della pagina.
        /// Questo bordo viene usato come bordo tra il FOOTER e la pagina,
        /// ma il bordo effettivo della pagina, quello fino a cui arriva il contenuto,
        /// và aggiornato.
        /// In questo senso il MarginBottom, passato per riferimento, viene aggiornato automaticamente,
        /// ma può essere recuperato tramite questa funzione.
        /// </summary>
        public float BottomMargin
        {
            get
            {
                return _BottomMargin;
            }
        }
        /// <summary>
        /// Alla creazione viene passato per riferimento il bordo superiore della pagina.
        /// Questo bordo viene usato come bordo tra l'HEADER e la pagina,
        /// ma il bordo effettivo della pagina, quello da cui parte il contenuto,
        /// và aggiornato.
        /// In questo senso il MarginTop, passato per riferimento, viene aggiornato automaticamente,
        /// ma può essere recuperato tramite questa funzione.
        /// </summary>
        public float TopMargin
        {
            get
            {
                return _TopMargin;
            }
        }



        #endregion

        #region Costruttori

        public Pdf5PageEventHandler(
            TemplVers.Domain.DTO.ServiceExport.DTO_HeaderFooter header, 
            TemplVers.Domain.DTO.ServiceExport.DTO_HeaderFooter footer, 
            ref DocTemplateVers.Domain.DTO.ServiceExport.DTO_Settings settings, 
            IList<DocTemplateVers.Domain.DTO.ServiceExport.DTO_Signature> Signatures,
            string waterMark = "")
            : base()
        {
            this.CurrentPage = 1000;
            this.Watermark = waterMark;

            LastPageElementRender = new Dictionary<long, int>();

            HFPageMask = settings.PagePlacingMask;
            HFPageRange = settings.PagePlacingRange;


            if (!string.IsNullOrEmpty(settings.BackgroundImagePath))
            {
                BGpath = settings.BackgroundImagePath;
                BGformat = settings.BackGroundImageFormat;
            }

            PGwidth = settings.Width - (settings.MarginLeft + settings.MarginRight);
            PGHeight = settings.Height - (settings.MarginTop + settings.MarginBottom);

            PGwidth_full = settings.Width;
            PGHeight_full = settings.Height;
            //int Cols;

            iTextSharp5.text.pdf.PdfPTable _HeaderPdfTable;
            iTextSharp5.text.pdf.PdfPTable _FooterPdfTable;

            //Implement HEADER
            //iTextSharp5.text.pdf.PdfPCell cell;// = new iTextSharp5.text.pdf.PdfPCell();

            HeadCols = ExportBaseHelper.ColumnCount(header);

            if (HeadCols > 0)
            {
                MaxWidth = PGwidth / HeadCols;

                //if (_HeaderPdfTable == null)
                //{
                _HeaderPdfTable = new iTextSharp5.text.pdf.PdfPTable(HeadCols);
                _HeaderPdfTable.TotalWidth = PGwidth;

                if (header.Left != null)
                {
                    HeadLeft = RenderElement(header.Left, ref HLtag);
                    _HeaderPdfTable.AddCell(HeadLeft);
                }
                else
                {
                    HeadLeft = null;
                    HeadLeftTxt = null;
                    HLtag = false;
                }

                if (header.Center != null)
                {
                    HeadCenter = RenderElement(header.Center, ref HCtag);
                    _HeaderPdfTable.AddCell(HeadCenter);

                }

                if (header.Right != null)
                {
                    HeadRight = RenderElement(header.Right, ref HRtag);
                    _HeaderPdfTable.AddCell(HeadRight);
                }


                _HeaderPdfTable.CalculateHeights();

                float TableHeight = _HeaderPdfTable.TotalHeight;

                _yPosH = settings.Height - (settings.MarginTop);
                _xPosH = settings.MarginLeft;

                _TopMargin = settings.MarginTop * 2 + TableHeight;
                //}
            }
            else
            {
                _TopMargin = settings.MarginTop;
            }

            //implement footer table
            FootCols = ExportBaseHelper.ColumnCount(footer);
            if (FootCols > 0)
            {
                float MaxWidth = PGwidth / FootCols;

                //if (_FooterPdfTable == null)
                //{
                _FooterPdfTable = new iTextSharp5.text.pdf.PdfPTable(FootCols);
                _FooterPdfTable.TotalWidth = PGwidth;

                if (footer.Left != null)
                {
                    FootLeft = new iTextSharp5.text.pdf.PdfPCell(RenderElement(footer.Left, ref FLtag));
                    FootLeft.Border = 0;
                    _FooterPdfTable.AddCell(FootLeft);
                }

                if (footer.Center != null)
                {
                    FootCenter = new iTextSharp5.text.pdf.PdfPCell(RenderElement(footer.Center, ref FCtag));
                    FootCenter.Border = 0;
                    _FooterPdfTable.AddCell(FootCenter);
                }

                if (footer.Right != null)
                {
                    FootRight = new iTextSharp5.text.pdf.PdfPCell(RenderElement(footer.Right, ref FRtag));
                    FootRight.Border = 0;
                    _FooterPdfTable.AddCell(FootRight);
                }

                //_FooterPdfTable.CalculateHeightsFast();
                _FooterPdfTable.CalculateHeights();

                float TableHeight = _FooterPdfTable.TotalHeight;

                _yPosF = TableHeight + settings.MarginBottom;
                _xPosF = settings.MarginLeft;

                _BottomMargin = settings.MarginBottom * 2 + TableHeight;
                //}
            }
            else
            {
                _BottomMargin = settings.MarginBottom;
            }

            settings.MarginBottom = this.BottomMargin;
            settings.MarginTop = this.TopMargin;

            ParamReset(header, footer);




            //SIGNATURES

            if (Signatures != null && Signatures.Count() > 0)
            {
                this.SignaturesPOS = (from DocTemplateVers.Domain.DTO.ServiceExport.DTO_Signature Sgn in Signatures
                                      where Sgn.HasPDFPositioning == true
                                      select Sgn).ToList();
                this.SignaturesStatic = (from DocTemplateVers.Domain.DTO.ServiceExport.DTO_Signature Sgn in Signatures
                                         where Sgn.HasPDFPositioning == false
                                         select Sgn).ToList();
            }
            else
            {
                this.SignaturesPOS = null;
            }



        }

        private void ParamReset(
            TemplVers.Domain.DTO.ServiceExport.DTO_HeaderFooter header,
            TemplVers.Domain.DTO.ServiceExport.DTO_HeaderFooter footer)
        {
            //Reset Parametri inutili

            //Header
            if (HLtag)
            {
                try
                {
                    HeadLeft = null;
                    HeadLeftTxt = (TemplVers.Domain.DTO.ServiceExport.DTO_ElementText)header.Left;
                }
                catch { }
            }

            if (HCtag)
            {
                try
                {
                    HeadCenter = null;
                    HeadCenterTxt = (TemplVers.Domain.DTO.ServiceExport.DTO_ElementText)header.Center;
                }
                catch { }
            }

            if (HRtag)
            {
                try
                {
                    HeadRight = null;
                    HeadRightTxt = (TemplVers.Domain.DTO.ServiceExport.DTO_ElementText)header.Right;
                }
                catch { }
            }

            //Footer
            if (FLtag)
            {
                try
                {
                    FootLeft = null;
                    FootLeftTxt = (TemplVers.Domain.DTO.ServiceExport.DTO_ElementText)footer.Left;
                }
                catch { }
            }

            if (FCtag)
            {
                try
                {
                    FootCenter = null;
                    FootCenterTxt = (TemplVers.Domain.DTO.ServiceExport.DTO_ElementText)footer.Center;
                }
                catch { }
            }

            if (FRtag)
            {
                try
                {
                    FootRight = null;
                    FootRightTxt = (TemplVers.Domain.DTO.ServiceExport.DTO_ElementText)footer.Right;
                }
                catch { }
            }
        }

        ///// <summary>
        ///// Crea le tabelle necessarie e modifica i parametri forniti se necessario
        ///// </summary>
        ///// <param name="header">docTemplate Header</param>
        ///// <param name="footer">docTemplate Footer</param>
        ///// <param name="marginLeft">Margine sinistro</param>
        ///// <param name="marginRight">Margine destro</param>
        ///// <param name="marginTop">Margine superiore</param>
        ///// <param name="marginBottom">Margine Inferiore</param>
        ///// <param name="pageWidth">Larghezza pagina</param>
        ///// <param name="pageHeight">Altezza pagina</param>
        ///// <remarks>I paramatri possono essere modificati. Si consiglia quindi di passare settings. per le varie impostazioni, in particolare TopMargin e BottomMargin e SOLO SUCCESSIVAMENTE inizializzare la pagina con i relativi SETTINGS.</remarks>
        //public Pdf5PageEventHandler(
        //    TemplVers.Domain.DTO.ServiceExport.DTO_HeaderFooter header,
        //    TemplVers.Domain.DTO.ServiceExport.DTO_HeaderFooter footer,
        //    float marginLeft, float marginRight,
        //    ref float marginTop, ref float marginBottom,
        //    float pageWidth, float pageHeight,
        //    int HeaderFooterPagePlacingMask, string HeaderFooterPagePlacingRange)
        //    : base()
        //{
        //    //_HeaderOnFirstPage = HeaderOnfirstPage;
        //    LastPageElementRender = new Dictionary<long, int>();

        //    HFPageMask = HeaderFooterPagePlacingMask;
        //    HFPageRange = HeaderFooterPagePlacingRange;

        //    BGpath = "";
        //    BGformat = TemplVers.BackgrounImagePosition.Center;

        //    PGwidth = pageWidth - (marginLeft + marginRight);
        //    PGHeight = pageHeight - (marginTop + marginBottom);


        //    int Cols;

        //    //Implement Footer
        //    Cols = ExportBaseHelper.ColumnCount(footer);

        //    iTextSharp5.text.pdf.PdfPCell cell;// = new iTextSharp5.text.pdf.PdfPCell();


        //    if (Cols > 0)
        //    {

        //        float MaxWidth = PGwidth / Cols;

        //        //implement footer table
        //        if (_FooterPdfTable == null)
        //        {
        //            _FooterPdfTable = new iTextSharp5.text.pdf.PdfPTable(Cols);
        //            _FooterPdfTable.TotalWidth = PGwidth;

        //            if (footer.Left != null)
        //            {
        //                cell = new iTextSharp5.text.pdf.PdfPCell(RenderElement(footer.Left, MaxWidth));
        //                cell.Border = 0;
        //                _FooterPdfTable.AddCell(cell);
        //            }

        //            if (footer.Center != null)
        //            {
        //                cell = new iTextSharp5.text.pdf.PdfPCell(RenderElement(footer.Center, MaxWidth));
        //                cell.Border = 0;
        //                _FooterPdfTable.AddCell(cell);
        //            }

        //            if (footer.Right != null)
        //            {
        //                cell = new iTextSharp5.text.pdf.PdfPCell(RenderElement(footer.Right, MaxWidth));
        //                cell.Border = 0;
        //                _FooterPdfTable.AddCell(cell);
        //            }

        //            //_FooterPdfTable.CalculateHeightsFast();
        //            _FooterPdfTable.CalculateHeights();

        //            float TableHeight = _FooterPdfTable.TotalHeight;

        //            _yPosF = TableHeight + marginBottom;
        //            _xPosF = marginLeft;

        //            _BottomMargin = marginBottom * 2 + TableHeight;
        //        }
        //    }

        //    Cols = ExportBaseHelper.ColumnCount(header);

        //    if (Cols > 0)
        //    {
        //        float MaxWidth = PGwidth / Cols;

        //        if (_HeaderPdfTable == null)
        //        {
        //            _HeaderPdfTable = new iTextSharp5.text.pdf.PdfPTable(Cols);
        //            _HeaderPdfTable.TotalWidth = PGwidth;

        //            if (header.Left != null)
        //            { _HeaderPdfTable.AddCell(RenderElement(header.Left, MaxWidth)); }

        //            if (header.Center != null)
        //            { _HeaderPdfTable.AddCell(RenderElement(header.Center, MaxWidth)); }

        //            if (header.Right != null)
        //            { _HeaderPdfTable.AddCell(RenderElement(header.Right, MaxWidth)); }


        //            _HeaderPdfTable.CalculateHeights();

        //            float TableHeight = _HeaderPdfTable.TotalHeight;

        //            _yPosH = pageHeight - (marginTop);
        //            _xPosH = marginLeft;

        //            _TopMargin = marginTop * 2 + TableHeight;
        //        }
        //    }

        //    marginBottom = this.BottomMargin;
        //    marginTop = this.TopMargin;
        //}
        #endregion

        #region Override eventi
        public override void OnOpenDocument(iTextSharp5.text.pdf.PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);

            _defaultBottom = document.BottomMargin;
            _defaultTop = document.TopMargin;
            
        }

        float _defaultTop = 0;
        float _defaultBottom = 0;

        public override void OnStartPage(iTextSharp5.text.pdf.PdfWriter writer, Document document)
        {
            ////Imposto QUI per TUTTI!!!
            this.CurrentPage = document.PageNumber;


            base.OnStartPage(writer, document);

            ////Blocco margini (dovrebbe andare PRIMA di OnStartPage di base!
            //if ((this.HFPageMask == (int)PageMaskingInclude.none) 
            //    || ((this.HFPageMask & (int)PageMaskingInclude.All) > 0))
            //{
            //    if (!PageHelper.IsCorrectPage(CurrentPage, this.HFPageMask, this.HFPageRange))
            //    {
            //        try
            //        {
            //            document.SetMargins(document.LeftMargin, document.RightMargin, _defaultTop, _defaultBottom);
            //        }
            //        catch (Exception)
            //        {
            //        }
            //    }
            //    else
            //    {
            //        iTextSharp5.text.pdf.PdfPTable headerPdfTable = this.GetHeaderTable();
            //        iTextSharp5.text.pdf.PdfPTable footerPdfTable = this.GetFooterTable();

            //        document.SetMargins(document.LeftMargin, document.RightMargin, _defaultTop - headerPdfTable.TotalHeight, _defaultBottom - footerPdfTable.TotalHeight);
            //    }
            //}
            ////END Blocco margini
            

            try
            {
                // BackGround
                if (!String.IsNullOrEmpty(BGpath))
                {
                    iTextSharp5.text.Image bg = iTextSharp5.text.Image.GetInstance(BGpath);

                    switch (BGformat)
                    {
                        case TemplVers.BackgrounImagePosition.Center:
                            float cntLeft = (PGwidth_full - bg.Width) / 2;
                            float cntBot = (PGHeight_full - bg.Height) / 2;
                            bg.SetAbsolutePosition(cntLeft, cntBot);
                            document.Add(bg);
                            break;

                        case TemplVers.BackgrounImagePosition.Stretch:
                            bg.ScaleAbsolute(PGwidth_full, PGHeight_full); //A4
                            bg.SetAbsolutePosition(0, 0);
                            document.Add(bg);
                            break;

                        case TemplVers.BackgrounImagePosition.Tiled:
                            if (bg.Width > PGwidth_full || bg.Height > PGHeight_full)
                            {
                                bg.ScaleToFit(PGwidth_full, PGHeight_full);
                            }

                            int Htile = ((int)PGwidth_full / (int)bg.Width) + 1;
                            int Vtile = ((int)PGHeight_full / (int)bg.Height) + 1;

                            //float PosB = PGHeight;
                            //float PosL = 0;

                            for (int h = 0; h <= Htile; h++)
                            {
                                for (int v = 0; v <= Vtile; v++)
                                {
                                    bg.SetAbsolutePosition(bg.Width * h, PGHeight_full - (bg.Height * v));
                                    document.Add(bg);
                                }
                            }

                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                string error = ex.ToString();
            }


        }


        /// <summary>
        /// Triggered just before starting a new page.
        /// This is the best place to add a header, a footer, a watermark, and so on.
        /// </summary>
        /// <param name="writer">DPFWriter</param>
        /// <param name="document">document</param>
        public override void OnEndPage(iTextSharp5.text.pdf.PdfWriter writer, iTextSharp5.text.Document document)
        {
            if (PageHelper.IsCorrectPage(CurrentPage, this.HFPageMask, this.HFPageRange))
            {
                iTextSharp5.text.pdf.PdfContentByte cb = writer.DirectContent;

                //NECESSARIO,
                //altrimenti il numero di pagina MOSTRATO non viene aggiornato...
                this.LastHFrenderedPage = CurrentPage;

                //Header
                iTextSharp5.text.pdf.PdfPTable _HeaderPdfTable = this.GetHeaderTable();

                if (_HeaderPdfTable != null)
                {
                    WriteTable(_HeaderPdfTable, cb, _xPosH, _yPosH);
                    //_HeaderPdfTable.WriteSelectedRows(0, -1, _xPosH, _yPosH, cb);
                }

                //Footer
                iTextSharp5.text.pdf.PdfPTable _FooterPdfTable = this.GetFooterTable();
                if (_FooterPdfTable != null)
                {
                    WriteTable(_FooterPdfTable, cb, _xPosF, _yPosF);
                    //_FooterPdfTable.WriteSelectedRows(0, -1, _xPosF, _yPosF, cb);
                }
            }

            //Signatures
            if (this.SignaturesPOS != null)
            {
                this.RenderSignaturePOS(document, writer, false);
            }

            RenderWaterMark(document, writer);

            base.OnEndPage(writer, document);
        }





        private void WriteTable(iTextSharp5.text.pdf.PdfPTable table, iTextSharp5.text.pdf.PdfContentByte cb, float _xPos, float _yPos)
        {
            table.WriteSelectedRows(0, -1, _xPos, _yPos, cb);
        }


        /// <summary>
        /// Prima della chiusura del documento
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="document"></param>
        /// <remarks>
        /// NON è possibile aggiungere NULLA alla pagina,
        /// MA QUI! ci va la gestione del numero TOTALE di pagine...
        /// </remarks>
        public override void OnCloseDocument(iTextSharp5.text.pdf.PdfWriter writer, Document document)
        {
            //total.BeginText();
            //total.ShowText(this.CurrentPage.ToString());
            //total.EndText();

            //total.setFontAndSize(helv, 12);
            //total.setTextMatrix(0, 0);

            base.OnCloseDocument(writer, document);
        }
        #endregion

        public void LastPageEnd(iTextSharp5.text.pdf.PdfWriter writer, Document document)
        {
            if (this.SignaturesStatic != null && this.SignaturesStatic.Count > 0)
            {
                iTextSharp5.text.pdf.PdfPTable StSgnTbl = this.GetStaticSignatureTable(document);
                if (StSgnTbl != null)
                    document.Add(StSgnTbl);
            }

            if (PageHelper.HasLast(this.HFPageMask) && CurrentPage != this.LastHFrenderedPage)
            {
                iTextSharp5.text.pdf.PdfContentByte cb = writer.DirectContent;

                this.LastHFrenderedPage = CurrentPage;


                //Header
                iTextSharp5.text.pdf.PdfPTable _HeaderPdfTable = this.GetHeaderTable();
                if (_HeaderPdfTable != null)
                {
                    _HeaderPdfTable.WriteSelectedRows(0, -1, _xPosH, _yPosH, cb);
                }

                //Footer
                iTextSharp5.text.pdf.PdfPTable _FooterPdfTable = this.GetFooterTable();
                if (_FooterPdfTable != null)
                {
                    _FooterPdfTable.WriteSelectedRows(0, -1, _xPosF, _yPosF, cb);
                }

                //Signatures
                if (this.SignaturesPOS != null && this.SignaturesPOS.Count() > 0)
                {
                    this.RenderSignaturePOS(document, writer, true);
                }
            }
        }


        #region Render Elementi

        private void RenderWaterMark(Document doc, iTextSharp5.text.pdf.PdfWriter writer)
        {
            //string waterMark = "Draft";

            if (!String.IsNullOrEmpty(this.Watermark))
            {
                try
                {
                    float fontSize = 80;
                    float xPosition = 300;
                    float yPosition = 400;
                    float angle = 45;

                    //iTextSharp5.text.pdf.PdfContentByte under = writer.DirectContentUnder;            SOTTO TUTTO
                    iTextSharp5.text.pdf.PdfContentByte under = writer.DirectContent;                 //Content corrente

                    //Acquisisco un 'livello' sopra al contenuto del pdf
                    //PdfContentByte content = st.GetOverContent(1);
                    

                    //PdfContentByte under = writer.DirectContentUnder;
                    iTextSharp5.text.pdf.BaseFont baseFont =
                        iTextSharp5.text.pdf.BaseFont.CreateFont(
                            iTextSharp5.text.pdf.BaseFont.HELVETICA,
                            iTextSharp5.text.pdf.BaseFont.WINANSI,
                            iTextSharp5.text.pdf.BaseFont.EMBEDDED);

                    

                    iTextSharp5.text.pdf.PdfGState trasnparency = new iTextSharp5.text.pdf.PdfGState();
                    trasnparency.FillOpacity = 0.3f;

                    under.BeginText();
                    under.SetColorFill(iTextSharp5.text.pdf.CMYKColor.LIGHT_GRAY);

                    under.SetGState(trasnparency);

                    under.SetFontAndSize(baseFont, fontSize);
                    under.ShowTextAligned(
                            iTextSharp5.text.pdf.PdfContentByte.ALIGN_CENTER,
                            this.Watermark,
                            xPosition,
                            yPosition,
                            angle);
                    under.EndText();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                }

            }
        }


        /// <summary>
        /// Crea e posiziona le firme nella pagina corrente del documento.
        /// </summary>
        /// <param name="doc">Il documento</param>
        /// <param name="writer">Il pdf-writer</param>
        /// <remarks> CONTROLLARE IL POSIZIONAMENTO!!!</remarks>
        private void RenderSignaturePOS(Document doc, iTextSharp5.text.pdf.PdfWriter writer, Boolean RenderLast)
        {
            if (this.SignaturesPOS != null && this.SignaturesPOS.Count() > 0)
            {

                //IList<iTextSharp5.text.pdf.PdfPCell> cellsLeft = new List<iTextSharp5.text.pdf.PdfPCell>();
                //IList<iTextSharp5.text.pdf.PdfPCell> cellsCenter = new List<iTextSharp5.text.pdf.PdfPCell>();
                //IList<iTextSharp5.text.pdf.PdfPCell> cellsRight = new List<iTextSharp5.text.pdf.PdfPCell>();
                IList<iTextSharp5.text.pdf.PdfPCell> cellsPos = new List<iTextSharp5.text.pdf.PdfPCell>();

                foreach (DocTemplateVers.Domain.DTO.ServiceExport.DTO_Signature sgn in this.SignaturesPOS)
                {
                    //int CurrentPage = doc.PageNumber;

                    if ((PageHelper.IsCorrectPage(this.CurrentPage, sgn.PagePlacingMask, sgn.PagePlacingRange)) || (RenderLast && PageHelper.HasLast(sgn.PagePlacingMask)))
                    {
                        ///render

                        iTextSharp5.text.pdf.PdfPCell cell = GetSignatureCell(doc, sgn);

                        if (sgn.HasPDFPositioning)
                        {
                            //Position Tabel with cell
                            iTextSharp5.text.pdf.PdfPTable posTable = new iTextSharp5.text.pdf.PdfPTable(1);
                            posTable.TotalWidth = doc.PageSize.Width - sgn.PosLeft;

                            posTable.AddCell(cell);

                            iTextSharp5.text.pdf.PdfContentByte cb = writer.DirectContent;

                            posTable.WriteSelectedRows(0, -1, sgn.PosLeft, sgn.PosBottom, cb);
                        }

                    }

                }
            }
        }

        // -----------------------------------------------------------------------------------------------------------------------------

        private iTextSharp5.text.pdf.PdfPTable GetStaticSignatureTable(Document doc)
        {
            if (this.SignaturesStatic == null || this.SignaturesStatic.Count() <= 0)
            {
                return null;
            }

            IList<iTextSharp5.text.pdf.PdfPCell> cellsLeft = new List<iTextSharp5.text.pdf.PdfPCell>();
            IList<iTextSharp5.text.pdf.PdfPCell> cellsCenter = new List<iTextSharp5.text.pdf.PdfPCell>();
            IList<iTextSharp5.text.pdf.PdfPCell> cellsRight = new List<iTextSharp5.text.pdf.PdfPCell>();


            iTextSharp5.text.pdf.PdfPTable SignStatTbl = new iTextSharp5.text.pdf.PdfPTable(3);

            int cellLeft = 0;
            int cellCenter = 0;
            int cellRight = 0;


            foreach (lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Signature sgn in this.SignaturesStatic)
            {
                iTextSharp5.text.pdf.PdfPCell cell = GetSignatureCell(doc, sgn);

                switch (sgn.Position)
                {
                    case lm.Comol.Core.DomainModel.DocTemplateVers.SignaturePosition.left:
                        cellsLeft.Add(cell);
                        cellLeft++;
                        break;
                    case lm.Comol.Core.DomainModel.DocTemplateVers.SignaturePosition.center:
                        cellsCenter.Add(cell);
                        cellCenter++;
                        break;
                    case lm.Comol.Core.DomainModel.DocTemplateVers.SignaturePosition.right:
                        cellsRight.Add(cell);
                        cellRight++;
                        break;
                }

                iTextSharp5.text.pdf.PdfPCell leftCell = new iTextSharp5.text.pdf.PdfPCell();
                leftCell.Border = 0;

                //Boolean AddTable = false;

                if (cellLeft > 0)
                {
                    iTextSharp5.text.pdf.PdfPTable leftTable = new iTextSharp5.text.pdf.PdfPTable(1);
                    foreach (iTextSharp5.text.pdf.PdfPCell Lcell in cellsLeft)
                    {
                        leftTable.AddCell(Lcell);
                    }
                    leftCell.AddElement(leftTable);
                }

                SignStatTbl.AddCell(leftCell);

                iTextSharp5.text.pdf.PdfPCell centerCell = new iTextSharp5.text.pdf.PdfPCell();
                centerCell.Border = 0;
                if (cellCenter > 0)
                {
                    iTextSharp5.text.pdf.PdfPTable centerTable = new iTextSharp5.text.pdf.PdfPTable(1);
                    foreach (iTextSharp5.text.pdf.PdfPCell Ccell in cellsCenter)
                    {
                        centerTable.AddCell(Ccell);
                    }
                    centerCell.AddElement(centerTable);
                }
                SignStatTbl.AddCell(centerCell);

                iTextSharp5.text.pdf.PdfPCell rightCell = new iTextSharp5.text.pdf.PdfPCell();
                rightCell.Border = 0;
                if (cellRight > 0)
                {
                    iTextSharp5.text.pdf.PdfPTable rightTable = new iTextSharp5.text.pdf.PdfPTable(1);
                    foreach (iTextSharp5.text.pdf.PdfPCell Rcell in cellsRight)
                    {
                        rightTable.AddCell(Rcell);
                    }
                    rightCell.AddElement(rightTable);
                }
                SignStatTbl.AddCell(rightCell);
            }

            return SignStatTbl;
        }



        private iTextSharp5.text.pdf.PdfPCell GetSignatureCell(
            Document doc,
            lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport.DTO_Signature sgn
            )
        {
            iTextSharp5.text.pdf.PdfPCell cell = new iTextSharp5.text.pdf.PdfPCell();

            if (!String.IsNullOrEmpty(sgn.Text))
            {
                String Text = TagReplacer.ReplaceAll(sgn.Text, this.CurrentPage);

                if (sgn.IsHTML)
                {
                    cell = HtmlToCell(Text);

                    //List<iTextSharp5.text.IElement> AL_Content = iTextSharp5.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(lm.Comol.Core.DomainModel.Helpers.Export.PdfHelper.HtmlCheckPDF(Text)), null);
                    //foreach (iTextSharp5.text.IElement element in AL_Content)
                    //{
                    //    cell.AddElement(element);
                    //}
                }
                else
                    cell.AddElement(new iTextSharp5.text.Paragraph(Text));
            }

            if (sgn.HasImage)
            {
                iTextSharp5.text.Image img;
                try  //Lasciare
                {
                    img = iTextSharp5.text.Image.GetInstance(sgn.Path);

                    //if (sgn.Width > doc.PageSize.Width - (doc.LeftMargin + doc.RightMargin))
                    //    sgn.Width = (short)doc.PageSize.Width;

                    //if (sgn.Height > doc.PageSize.Height - (doc.TopMargin + doc.BottomMargin))
                    //    sgn.Height = (short)doc.PageSize.Height;

                    if (sgn.Width > 0 && sgn.Height > 0)
                        //img.ScaleToFit(sgn.Width, sgn.Height);
                        img.ScaleAbsolute(sgn.Width, sgn.Height);
                    else if (sgn.Width > 0)
                        img.ScaleAbsoluteWidth(sgn.Width);
                    else if (sgn.Height > 0)
                        img.ScaleAbsoluteHeight(sgn.Height);

                    //if (img.Width > MaxWidth)
                    //    img.ScaleAbsoluteWidth(MaxWidth);
                    cell.AddElement(img);
                }
                catch (Exception ex)
                {
                    string Exception = ex.ToString();
                    //cell = null;
                }
            }

            cell.Border = 0;
            return cell;
        }
















        private iTextSharp5.text.pdf.PdfPTable GetHeaderTable()
        {
            //Boolean HasTag = false;

            if (HeadCols <= 0)
                return null;

            iTextSharp5.text.pdf.PdfPTable HeadTable = new iTextSharp5.text.pdf.PdfPTable(HeadCols);
            HeadTable.TotalWidth = PGwidth;

            if (!HLtag && HeadLeft != null)
                HeadTable.AddCell(HeadLeft);
            else if (HLtag && HeadLeftTxt != null)
            {
                iTextSharp5.text.pdf.PdfPCell cell = RenderElement(HeadLeftTxt, ref HLtag);
            }

            if (!HCtag && HeadCenter != null)
                HeadTable.AddCell(HeadCenter);
            else if (HCtag && HeadCenterTxt != null)
            {
                HeadTable.AddCell(RenderElement(HeadCenterTxt, ref HCtag));
            }

            if (!HRtag && HeadRight != null)
                HeadTable.AddCell(HeadRight);
            else if (HRtag && HeadRightTxt != null)
            {
                HeadTable.AddCell(RenderElement(HeadRightTxt, ref HRtag));
            }

            return HeadTable;
        }

        private iTextSharp5.text.pdf.PdfPTable GetFooterTable()
        {
            Boolean HasTag = false;

            if (FootCols <= 0)
                return null;

            iTextSharp5.text.pdf.PdfPTable FootTable = new iTextSharp5.text.pdf.PdfPTable(FootCols);
            FootTable.TotalWidth = PGwidth;

            if (!FLtag && FootLeft != null)
                FootTable.AddCell(FootLeft);
            else if (FLtag && FootLeftTxt != null)
            {
                FootTable.AddCell(RenderElement(FootLeftTxt, ref FLtag));
            }

            if (!FCtag && FootCenter != null)
                FootTable.AddCell(FootCenter);
            else if (FCtag && FootCenterTxt != null)
            {
                FootTable.AddCell(RenderElement(FootCenterTxt, ref FCtag));
            }

            if (!FRtag && FootRight != null)
                FootTable.AddCell(FootRight);
            else if (FRtag && FootRightTxt != null)
            {
                FootTable.AddCell(RenderElement(FootRightTxt, ref FRtag));
            }

            return FootTable;
        }

        /// <summary>
        /// Smista la chiamata sulle relative funzioni.
        /// Pare che non possa essere fatto diversamente...
        /// </summary>
        /// <param name="Element">L'elemento da renderizzare</param>
        /// <returns></returns>
        private iTextSharp5.text.pdf.PdfPCell RenderElement(
            TemplVers.Domain.DTO.ServiceExport.DTO_Element Element,
            ref Boolean HasTag)
        {
            //if (Element != null)
            //{
            if (Element.GetType() == typeof(TemplVers.Domain.DTO.ServiceExport.DTO_ElementText))
            {
                return RenderElement((TemplVers.Domain.DTO.ServiceExport.DTO_ElementText)Element, ref HasTag);
            }
            else if (Element.GetType() == typeof(TemplVers.Domain.DTO.ServiceExport.DTO_ElementImage))
            {
                HasTag = false;
                return RenderElement((TemplVers.Domain.DTO.ServiceExport.DTO_ElementImage)Element);
            }
            else if (Element.GetType() == typeof(TemplVers.Domain.DTO.ServiceExport.DTO_ElementImageMulti))
            {
                HasTag = false;
                return RenderElement((TemplVers.Domain.DTO.ServiceExport.DTO_ElementImageMulti)Element);
            }
            //}

            return null;
        }

        /// <summary>
        /// Renderizza un elemento di tipo testuale
        /// </summary>
        /// <param name="TxtElement">L'elemento testuale da renderizzare</param>
        /// <returns>Una PdfCell con il conuto impostato correttamente.</returns>
        private iTextSharp5.text.pdf.PdfPCell RenderElement(
            TemplVers.Domain.DTO.ServiceExport.DTO_ElementText TxtElement,
            ref Boolean HasTag)
        {
            iTextSharp5.text.pdf.PdfPCell cell;// = new iTextSharp5.text.Cell();

            HasTag = TagReplacer.HasTag(TxtElement.Text);

            String Text = "";
            if (HasTag)
                Text = TagReplacer.ReplaceAll(TxtElement.Text, this.CurrentPage);
            else
                Text = TxtElement.Text;



            //total = writer.DirectContent.CreateTemplate()






            if (TxtElement.IsHTML)
            {
                cell = HtmlToCell(Text);
                SetAlignment(ref cell, TxtElement.Alignment);

                //                cell = new iTextSharp5.text.pdf.PdfPCell();
                //                //ExportBaseHelper.HtmlCheck(TxtElement.Text);

                //                //System.Collections.ArrayList AL_Content = 
                //                List<iTextSharp5.text.IElement> AL_Content =
                //iTextSharp5.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(PdfHelper.HtmlCheckPDF(Text)), null);

                //                foreach (iTextSharp5.text.IElement element in AL_Content)
                //                {
                //                    cell.AddElement(element);
                //                }

            }
            else
            {
                iTextSharp5.text.Paragraph textPar = new iTextSharp5.text.Paragraph(Text);
                SetAlignment(ref textPar, TxtElement.Alignment);

                cell = new iTextSharp5.text.pdf.PdfPCell(textPar);

                SetAlignment(ref cell, TxtElement.Alignment);

            }

            cell.Border = 0;
            cell.Padding = 0;
            return cell;
        }

        /// <summary>
        /// Renderizza un elemento di tipo Immagine
        /// </summary>
        /// <param name="ImgElement">L'elemento di tipo immagine</param>
        /// <returns>Una PdfCell con il conuto impostato correttamente.</returns>
        private iTextSharp5.text.pdf.PdfPCell RenderElement(TemplVers.Domain.DTO.ServiceExport.DTO_ElementImage ImgElement)
        {
            iTextSharp5.text.pdf.PdfPCell cell = new iTextSharp5.text.pdf.PdfPCell();


            iTextSharp5.text.Image img;
            try
            {
                img = iTextSharp5.text.Image.GetInstance(ImgElement.Path);

                float ImageRatio = img.Width / img.Height;

                float imgWidth = ImgElement.Width;
                float imgHeight = ImgElement.Height;

                if (imgWidth > MaxWidth)
                    imgWidth = MaxWidth;

                if (imgHeight > PGHeight / _imgFract)
                    imgHeight = PGHeight / _imgFract;


                if (imgHeight > 0 && imgWidth > 0)
                    img.ScaleAbsolute(imgWidth, imgHeight);
                else if (imgWidth > 0)
                    img.ScaleToFit(imgWidth, imgWidth / ImageRatio);
                //img.ScaleAbsoluteWidth(ImgElement.Width);
                else if (imgHeight > 0)
                    img.ScaleToFit(imgHeight * ImageRatio, imgHeight);
                //img.ScaleAbsoluteHeight(ImgElement.Height);
                else
                {
                    //img.ScaleToFit(MaxWidth, PGHeight / _imgFract);
                }


                //else if (img.Width > MaxWidth || img.Height > PGHeight / 3)
                //{
                //    img.ScaleToFit(MaxWidth, PGHeight / 2);
                //}

                //if (img.Width > )
                //    img.ScaleAbsoluteWidth(MaxWidth);
                SetAlignment(ref img, ImgElement.Alignment);

                cell = new iTextSharp5.text.pdf.PdfPCell(img);  //Proviamo se COSì effettivamente SCALA l'immagine correttamente.
                //cell.AddElement(img); 

            }
            catch (Exception ex)
            {
                string Exception = ex.ToString();

                //cell = null;
            }

            cell.Border = 0;
            cell.Padding = 0;
            SetAlignment(ref cell, ImgElement.Alignment);

            return cell;
            //return new iTextSharp5.text.pdf.PdfPCell(new iTextSharp5.text.Paragraph("NOT IMPLEMENT!"));
        }

        /// <summary>
        /// Renderizza un elemento di tipo MultiImage (SOLO per Footer Skin)
        /// </summary>
        /// <param name="MultiImgElement">Elemento MultiImage da renderizzare.</param>
        /// <returns>Una PdfCell con il conuto impostato correttamente.</returns>
        private iTextSharp5.text.pdf.PdfPCell RenderElement(TemplVers.Domain.DTO.ServiceExport.DTO_ElementImageMulti MultiImgElement)
        {
            //MaxWidth = 100;


            //cell.BackgroundColor = BaseColor.ORANGE;

            //cell.Width = MaxWidth;
            iTextSharp5.text.pdf.PdfPCell cell = new iTextSharp5.text.pdf.PdfPCell();


            if (MultiImgElement != null && MultiImgElement.ImgElements != null)
            {
                int Innercol = MultiImgElement.ImgElements.Count();

                if (Innercol > 0)
                {
                    iTextSharp5.text.pdf.PdfPTable InnerTable = new iTextSharp5.text.pdf.PdfPTable(Innercol);
                    InnerTable.DefaultCell.UseBorderPadding = false;
                    InnerTable.DefaultCell.Padding = 0;
                    InnerTable.DefaultCell.Border = 0;

                    InnerTable.TotalWidth = MaxWidth;


                    float ElementWidth = (MaxWidth) / Innercol;
                    float ElementHeight = PGHeight / 3;

                    foreach (TemplVers.Domain.DTO.ServiceExport.DTO_ElementImage ImgElement in MultiImgElement.ImgElements)
                    {
                        //iTextSharp5.text.pdf.PdfPCell innercell = new iTextSharp5.text.pdf.PdfPCell();

                        //innercell.Border = 0;
                        //innercell.Padding = 0;
                        //innercell.Width = MaxWidth;
                        //innercell.BackgroundColor = BaseColor.GREEN;

                        try
                        {
                            iTextSharp5.text.Image img = iTextSharp5.text.Image.GetInstance(ImgElement.Path);


                            if (ImgElement.Width != 0 && ImgElement.Width < ElementWidth)
                                ElementWidth = ImgElement.Width;

                            img.ScaleToFit(ElementWidth, ElementHeight);

                            //if (ImgElement.Width > 0 && ImgElement.Height > 0)
                            //    img.ScaleAbsolute(ImgElement.Width, ImgElement.Height);
                            //else if (ImgElement.Width > 0)
                            //    img.ScaleAbsoluteWidth(ImgElement.Width);
                            //else if (ImgElement.Height > 0)
                            //    img.ScaleAbsoluteHeight(ImgElement.Height);
                            //else if (img.Width > ElementWidth || img.Height > )
                            //    img.ScaleToFit(ElementWidth, PGHeight / 2);

                            //inner

                            //innercell.AddElement(img);

                            //SetAlignment(ref innercell, MultiImgElement.Alignment); //inner

                            //InnerTable.AddCell(innercell);
                            iTextSharp5.text.pdf.PdfPCell innercell = new iTextSharp5.text.pdf.PdfPCell(img, false);
                            innercell.Padding = 0;
                            innercell.BorderWidth = 0;
                            InnerTable.AddCell(innercell);

                        }
                        catch (Exception ex)
                        {
                            //cell = null;
                        }
                        //}
                    }

                    cell.AddElement(InnerTable);
                }
            }

            SetAlignment(ref cell, MultiImgElement.Alignment);

            cell.Border = 0;
            cell.Padding = 0;

            return cell;
            //return new iTextSharp5.text.pdf.PdfPCell(new iTextSharp5.text.Paragraph("NOT IMPLEMENT!"));
        }

        //
        private static void SetAlignment(ref iTextSharp5.text.Image img, TemplVers.ElementAlignment Alignment)
        {
            switch (Alignment)
            {
                case TemplVers.ElementAlignment.BottomCenter:
                    img.Alignment = iTextSharp5.text.Element.ALIGN_CENTER; // = iTextSharp5.text.Cell.ALIGN_CENTER;
                    break;
                case TemplVers.ElementAlignment.BottomLeft:
                    img.Alignment = iTextSharp5.text.Element.ALIGN_LEFT;
                    break;
                case TemplVers.ElementAlignment.BottomRight:
                    img.Alignment = iTextSharp5.text.Element.ALIGN_RIGHT;
                    break;

                case TemplVers.ElementAlignment.MiddleCenter:
                    img.Alignment = iTextSharp5.text.Element.ALIGN_CENTER;
                    break;
                case TemplVers.ElementAlignment.MiddleLeft:
                    img.Alignment = iTextSharp5.text.Element.ALIGN_LEFT;
                    break;
                case TemplVers.ElementAlignment.MiddleRight:
                    img.Alignment = iTextSharp5.text.Element.ALIGN_RIGHT;
                    break;

                case TemplVers.ElementAlignment.TopCenter:
                    img.Alignment = iTextSharp5.text.Element.ALIGN_CENTER;
                    break;
                case TemplVers.ElementAlignment.TopLeft:
                    img.Alignment = iTextSharp5.text.Element.ALIGN_LEFT;
                    break;
                case TemplVers.ElementAlignment.TopRight:
                    img.Alignment = iTextSharp5.text.Element.ALIGN_RIGHT;
                    break;

                default:
                    img.Alignment = iTextSharp5.text.Element.ALIGN_LEFT;
                    break;
            }
        }


        private static void SetAlignment(ref iTextSharp5.text.Paragraph par, TemplVers.ElementAlignment Alignment)
        {
            switch (Alignment)
            {
                case TemplVers.ElementAlignment.BottomCenter:
                    par.Alignment = iTextSharp5.text.Element.ALIGN_CENTER; // = iTextSharp5.text.Cell.ALIGN_CENTER;
                    break;
                case TemplVers.ElementAlignment.BottomLeft:
                    par.Alignment = iTextSharp5.text.Element.ALIGN_LEFT;
                    break;
                case TemplVers.ElementAlignment.BottomRight:
                    par.Alignment = iTextSharp5.text.Element.ALIGN_RIGHT;
                    break;

                case TemplVers.ElementAlignment.MiddleCenter:
                    par.Alignment = iTextSharp5.text.Element.ALIGN_CENTER;
                    break;
                case TemplVers.ElementAlignment.MiddleLeft:
                    par.Alignment = iTextSharp5.text.Element.ALIGN_LEFT;
                    break;
                case TemplVers.ElementAlignment.MiddleRight:
                    par.Alignment = iTextSharp5.text.Element.ALIGN_RIGHT;
                    break;

                case TemplVers.ElementAlignment.TopCenter:
                    par.Alignment = iTextSharp5.text.Element.ALIGN_CENTER;
                    break;
                case TemplVers.ElementAlignment.TopLeft:
                    par.Alignment = iTextSharp5.text.Element.ALIGN_LEFT;
                    break;
                case TemplVers.ElementAlignment.TopRight:
                    par.Alignment = iTextSharp5.text.Element.ALIGN_RIGHT;
                    break;

                default:
                    par.Alignment = iTextSharp5.text.Element.ALIGN_LEFT;
                    break;
            }
        }



        private static void SetAlignment(ref iTextSharp5.text.pdf.PdfPCell cell, TemplVers.ElementAlignment Alignment)
        {
            switch (Alignment)
            {
                case TemplVers.ElementAlignment.BottomCenter:
                    cell.HorizontalAlignment = iTextSharp5.text.Element.ALIGN_CENTER; // = iTextSharp5.text.Cell.ALIGN_CENTER;
                    cell.VerticalAlignment = iTextSharp5.text.Element.ALIGN_BOTTOM;
                    break;
                case TemplVers.ElementAlignment.BottomLeft:
                    cell.HorizontalAlignment = iTextSharp5.text.Element.ALIGN_LEFT;
                    cell.VerticalAlignment = iTextSharp5.text.Element.ALIGN_BOTTOM;
                    break;
                case TemplVers.ElementAlignment.BottomRight:
                    cell.HorizontalAlignment = iTextSharp5.text.Element.ALIGN_RIGHT;
                    cell.VerticalAlignment = iTextSharp5.text.Element.ALIGN_BOTTOM;
                    break;

                case TemplVers.ElementAlignment.MiddleCenter:
                    cell.HorizontalAlignment = iTextSharp5.text.Element.ALIGN_CENTER;
                    cell.VerticalAlignment = iTextSharp5.text.Element.ALIGN_MIDDLE;
                    break;
                case TemplVers.ElementAlignment.MiddleLeft:
                    cell.HorizontalAlignment = iTextSharp5.text.Element.ALIGN_LEFT;
                    cell.VerticalAlignment = iTextSharp5.text.Element.ALIGN_MIDDLE;
                    break;
                case TemplVers.ElementAlignment.MiddleRight:
                    cell.HorizontalAlignment = iTextSharp5.text.Element.ALIGN_RIGHT;
                    cell.VerticalAlignment = iTextSharp5.text.Element.ALIGN_MIDDLE;
                    break;

                case TemplVers.ElementAlignment.TopCenter:
                    cell.HorizontalAlignment = iTextSharp5.text.Element.ALIGN_CENTER;
                    cell.VerticalAlignment = iTextSharp5.text.Element.ALIGN_TOP;
                    break;
                case TemplVers.ElementAlignment.TopLeft:
                    cell.HorizontalAlignment = iTextSharp5.text.Element.ALIGN_LEFT;
                    cell.VerticalAlignment = iTextSharp5.text.Element.ALIGN_TOP;
                    break;
                case TemplVers.ElementAlignment.TopRight:
                    cell.HorizontalAlignment = iTextSharp5.text.Element.ALIGN_RIGHT;
                    cell.VerticalAlignment = iTextSharp5.text.Element.ALIGN_TOP;
                    break;

                default:
                    cell.HorizontalAlignment = iTextSharp5.text.Element.ALIGN_LEFT;
                    cell.VerticalAlignment = iTextSharp5.text.Element.ALIGN_TOP;
                    break;
            }
        }
        #endregion

        public static iTextSharp5.text.pdf.PdfPCell HtmlToCell(String Html)
        {
            iTextSharp5.text.pdf.PdfPCell cell = new iTextSharp5.text.pdf.PdfPCell();

            List<IElement> AL_Content = new List<IElement>();
            Boolean Error = false;

            Html = lm.Comol.Core.DomainModel.Helpers.Export.PdfHelper.HtmlCheckPDF(Html);

            try
            {
                AL_Content = iTextSharp5.text.html.simpleparser.HTMLWorker.ParseToList(new System.IO.StringReader(Html), null);
            }
            catch (Exception ex)
            {
                Error = true;
            }

            if (Error)
            {
                Error = true;

                Html = lm.Comol.Core.DomainModel.Helpers.Export.PdfHelper.RemoveHtmlTag(Html);

                try
                {
                    AL_Content = iTextSharp5.text.html.simpleparser.HTMLWorker.ParseToList(new System.IO.StringReader(Html), null);
                    Error = false;
                }
                catch (Exception ex)
                {
                    cell = new iTextSharp5.text.pdf.PdfPCell(
                        new iTextSharp5.text.Paragraph(lm.Comol.Core.DomainModel.Helpers.Export.PdfHelper.RemoveHtmlTag(Html))
                    );
                }
            }

            if (!Error)
            {
                foreach (iTextSharp5.text.IElement element in AL_Content)
                {
                    cell.AddElement(element);
                }
            }

            return cell;
        }
    }

}