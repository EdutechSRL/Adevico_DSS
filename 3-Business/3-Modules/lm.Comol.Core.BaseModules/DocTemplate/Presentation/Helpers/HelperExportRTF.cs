using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using iTS = iTextSharp.text;
using System.IO;

using TemplVers_Export = lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.ServiceExport;

namespace lm.Comol.Core.BaseModules.DocTemplate.Helpers
{
    public class HelperExportRTF : lm.Comol.Core.DomainModel.Helpers.Export.ExportRtfBaseHelper
    {
        private TemplVers_Export.DTO_Template _template { get; set; }

        public Boolean ExportToRtf(
    TemplVers_Export.DTO_Template Template,
    String clientFileName, System.Web.HttpResponse webResponse, System.Web.HttpCookie cookie)
        {
            if (Template == null)
            {
                return false;
            }

            _template = Template;


            //Render effettivo...
            try
            {
                base.WebExport(clientFileName, _template.Settings, webResponse, cookie);
            }
            catch (Exception ex)
            {
                string err = ex.ToString(); //DEBUG!
                return false;
            }

            return true;
        }

        protected override void RenderDocument(iTS.Document doc, iTS.rtf.RtfWriter2 rtfWriter)
        {
            //iTS.Table contentTable = new iTS.Table(1);
            ////Me lo mette come percentualeee!!!
            ////contentTable.Width = 100; // *(_template.Settings.Width - (_template.Settings.MarginLeft + _template.Settings.MarginRight)) / _template.Settings.Width;

            //iTS.Cell body = RenderBody();

            //contentTable.AddCell(body);




            iTS.Table contentTable = new iTS.Table(1);

            contentTable.AddCell(RenderBody());

            try // In taluni casi la conversione manda in crash qui, con mille casini a seguire...
            {
                doc.Add(contentTable);
            }
            catch { }
            

            if (_template.Signatures != null && _template.Signatures.Count() > 0)
            {
                RenderSignature(doc);
            }
        }

        protected override void RenderErrorDocument(iTS.Document doc)
        {
            iTS.Table contentTable = new iTextSharp.text.Table(1);
            contentTable.Width = _template.Settings.Width - (_template.Settings.MarginLeft + _template.Settings.MarginRight);
            contentTable.AddCell(RenderBody());
            doc.Add(contentTable);
            if (_template.Signatures != null && _template.Signatures.Count() > 0)
            {
                RenderSignature(doc);
            }

        }

        protected override TemplVers_Export.DTO_HeaderFooter GetHeader()
        {
            return _template.Header;
        }

        protected override TemplVers_Export.DTO_HeaderFooter GetFooter()
        {
            return _template.Footer;
        }

        private iTS.Cell RenderBody()
        {
            iTS.Cell cell;// = new iTextSharp5.text.Cell();

            if (_template.Body.IsHTML)
            {
                cell = HtmlToCell(_template.Body.Text);
            }
            else
            {
                cell = new iTS.Cell(
                    new iTS.Paragraph(_template.Body.Text)
                    );
            }

            if (cell != null)
            {
                lm.Comol.Core.DomainModel.Helpers.Export.RtfHelper.SetAlignment(ref cell, _template.Body.Alignment); // TxtElement.Alignment);
                cell.Border = 0;
            }


            return cell;
        }

        private void RenderSignature(iTS.Document doc)
        {
            IList<iTS.Cell> cellsLeft = new List<iTS.Cell>();
            IList<iTS.Cell> cellsCenter = new List<iTS.Cell>();
            IList<iTS.Cell> cellsRight = new List<iTS.Cell>();
            
            int cellLeft = 0;
            int cellCenter = 0;
            int cellRight = 0;

            foreach (TemplVers_Export.DTO_Signature sgn in _template.Signatures)
            {
                iTS.Cell cell = new iTS.Cell();
                

                if (sgn.IsHTML)
                {
                    cell = HtmlToCell(sgn.Text);
//                    iTS.html.simpleparser.StyleSheet styles =
//new iTS.html.simpleparser.StyleSheet();

//                    styles.LoadTagStyle("ul", "indent", "10");
//                    styles.LoadTagStyle("ol", "indent", "10");

                    //System.Collections.ArrayList AL_Content = iTS.html.simpleparser.HTMLWorker.ParseToList(new StringReader(sgn.Text), lm.Comol.Core.DomainModel.Helpers.Export.RtfHelper.GetStyles());


                    //for (int j = 0; j < AL_Content.Count; j++)
                    //{
                    //    cell.AddElement(CheckconvertedElement((iTS.IElement)AL_Content[j]));
                    //}
                }
                else
                {
                    cell.AddElement(new iTS.Paragraph(sgn.Text));
                }

                if (sgn.HasImage)
                {
                    iTS.Image img;
                    try
                    {
                        img = iTS.Image.GetInstance(sgn.Path);

                        if (sgn.Width > 0 && sgn.Height > 0)
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

                switch (sgn.Position)
                {
                    case DomainModel.DocTemplateVers.SignaturePosition.left: // SignaturePosition.left:
                        cellsLeft.Add(cell);
                        cellLeft++;
                        break;
                    case DomainModel.DocTemplateVers.SignaturePosition.center: // SignaturePosition.center:
                        cellsCenter.Add(cell);
                        cellCenter++;
                        break;
                    default:
                        cellsRight.Add(cell);
                        cellRight++;
                        break;
                }
            }
              
            iTS.Table signTable = new iTS.Table(3);
            iTS.Cell VoidCell = new iTS.Cell(" ");
            VoidCell.Border = 0;

            int MaxRow = (cellLeft > cellRight) ? cellLeft : cellRight;
            MaxRow = (MaxRow > cellCenter) ? MaxRow : cellCenter;


            if (MaxRow > 0)
            {
                for (int r = 0; r < MaxRow; r++)
                {
                    if (r < cellLeft)
                    {
                        signTable.AddCell(cellsLeft[r]);
                    }
                    else
                    {
                        signTable.AddCell(VoidCell);
                    }

                    if (r < cellCenter)
                    {
                        signTable.AddCell(cellsCenter[r]);
                    }
                    else
                    {
                        signTable.AddCell(VoidCell);
                    }

                    if (r < cellRight)
                    {
                        signTable.AddCell(cellsRight[r]);
                    }
                    else
                    {
                        signTable.AddCell(VoidCell);
                    }
                }

                try
                {
                    doc.Add(signTable);
                }
                catch { }
                
            }
            
        }

        
    }


}
