using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Telerik.Documents.SpreadsheetStreaming;


namespace lm.Comol.Modules.CallForPapers.AdvEconomic.dto
{
    /// <summary>
    /// Impostazioni esportazione tabelle economiche
    /// </summary>
    public class dtoEcoTableExportSettings
    {   
        /// <summary>
        /// Larghezza colonne default
        /// </summary>
        public double[] ColumnWidths = { 15.00, 20.00, 20.00, 15.00, 20.00, 20.00, 50.00 };
        /// <summary>
        /// Larghezza colonne aggiuntive
        /// </summary>
        public double ColumnAddWidth = 25.00;
        /// <summary>
        /// Altezza riga intestazione
        /// </summary>
        public int HeaderRowHeight = 22;
        /// <summary>
        /// Altezza riga dati
        /// </summary>
        public int RowHeight = 18;

        /// <summary>
        /// Intestazioni colonna (internazionalizzare)
        /// </summary>
        public string[] HeadStrings = { "Quantità", "Prezzo unitario", "Totale richiesto", "Approvato", "Quantità ammessa", "Spesa ammessa", "Commenti" };

        /// <summary>
        /// Traduzione valori binari
        /// </summary>
        public string[] BoolValue = { "No", "Sì" };
        /// <summary>
        /// Formato celle per l'header
        /// </summary>
        public SpreadCellFormat HeaderFormat
        {
            get
            {
                if(_headerFormat == null)
                {

                }
                _headerFormat = new SpreadCellFormat();
                //Font
                _headerFormat.FontFamily = new SpreadThemableFontFamily("Segoe UI");
                _headerFormat.FontSize = 11;
                //Sfondo
                _headerFormat.Fill = SpreadPatternFill.CreateSolidFill(new SpreadColor(227, 230, 234));//new SpreadColor(51, 153, 51));
                //Colore testo
                _headerFormat.ForeColor = new SpreadThemableColor(new SpreadColor(0, 0, 0));
                //bold
                _headerFormat.IsBold = true;
                //Allineamento orizzontale
                _headerFormat.HorizontalAlignment = SpreadHorizontalAlignment.Center;
                //Allineamento verticale
                _headerFormat.VerticalAlignment = SpreadVerticalAlignment.Center;

                return _headerFormat;
            }
            set
            {
                _headerFormat = value;
            }
        }

        private SpreadCellFormat _headerFormat { get; set; }
        private SpreadCellFormat _normalFormat { get; set; }
        /// <summary>
        /// Formato standard celle dati
        /// </summary>
        public SpreadCellFormat NormalFormat
        {
            get
            {           
                if (_normalFormat == null)
                {

                    _normalFormat = new SpreadCellFormat();
                    //Font
                    _normalFormat.FontSize = 11;
                    //Allineamenti
                    _normalFormat.VerticalAlignment = SpreadVerticalAlignment.Center;
                    _normalFormat.HorizontalAlignment = SpreadHorizontalAlignment.Center;
                    //Colore testo
                    _normalFormat.ForeColor = new SpreadThemableColor(new SpreadColor(0, 0, 0));
                }

                return _normalFormat;
            }
            set
            {
                _normalFormat = value;
            }
        }

        

        /// <summary>
        /// Formato celle con virgola (2 decimali)
        /// </summary>
        public SpreadCellFormat DoubleFormat
        {
            get
            {
                SpreadCellFormat format = NormalFormat;
                format.NumberFormat = "#,##0.00";

                return format;
            }
        }

        /// <summary>
        /// Formato celle economiche (2 decimali con simbolo Euro)
        /// </summary>
        public SpreadCellFormat DoubleEcoFormat
        {
            get
            {
                SpreadCellFormat format = NormalFormat;
                format.NumberFormat = "#,##0.00 \\€";

                return format;
            }
        }

        /// <summary>
        /// Formato celle economiche (2 decimali con simbolo Euro)
        /// </summary>
        public static SpreadCellFormat InvalidCellFormat(SpreadCellFormat format, bool isValid)
        {
            
            if (isValid)
                format.ForeColor = new SpreadThemableColor(new SpreadColor(0,0,0));
            else
                format.ForeColor = new SpreadThemableColor(new SpreadColor(128, 128, 128));

            return format;           
        }
    }
}
