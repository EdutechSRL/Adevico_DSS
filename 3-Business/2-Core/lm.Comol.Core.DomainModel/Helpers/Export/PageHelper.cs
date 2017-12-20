using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Helpers.Export
{
    public static class PageHelper
    {
        /// <summary>
        /// Verifica se la pagina corrente è la prima
        /// </summary>
        /// <param name="PageNumber">Pagina corrente</param>
        /// <returns>TRUE se è vera</returns>
        public static Boolean IsFirst(Int32 PageNumber)
        {
            if (PageNumber == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Verifica se la pagina corrente è pari.
        /// </summary>
        /// <param name="PageNumber">Pagina corrente</param>
        /// <returns>TRUE se è pari, FALSE se è dispari</returns>
        public static Boolean IsEven(Int32 PageNumber)
        {
            if (PageNumber%2 == 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Verifica se la pagina corrente è l'ultima
        /// </summary>
        /// <param name="PageNumber">Pagina corrente</param>
        /// <param name="TotalPage">Ultima pagina. Se = 0 restituisce FALSE.</param>
        /// <returns>True se è l'ultima pagina.</returns>
        /// <remarks>Se TotalPage == 0, restituisce FALSE. Infatti iTextSharp non può conoscere il numero di pagine finchè il documento non viene chiuso, rendendo di fatto impossibile tale controllo fino alla chiusura del documento. Questo vale SOLO per il PDF, l'RTF ha logiche ancora diverse e meno modificabili...</remarks>
        //public static Boolean IsLast(Int32 PageNumber, Int32 TotalPage)
        //{
        //    if (TotalPage == 0)
        //        return false;
        //    if (PageNumber == TotalPage)
        //        return true;
        //    else
        //        return false;
        //}

        public static Boolean HasLast(int MaskValue)
        {
            if ((MaskValue & (int)PageMaskingInclude.Last) > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Verifica se la pagina corrente è nel range specificato
        /// </summary>
        /// <param name="PageNumber">Pagina corrente</param>
        /// <param name="Range">Range</param>
        /// <returns>TRUE se è nel range</returns>
        /// <remarks>
        /// Range è una stringa fatta nel seguente modo:
        /// 1-3, 6
        /// Sono più gruppi separati da virgola. Ogni gruppo rappresenta:
        /// una pagina singola (valore intero)
        /// un range di pagina (due valori interi separati da un trattino "-")
        /// NOTA: nel caso fossero inserite lettere o formati non appropriati, il sistema può restituire FALSE!
        /// --- Inserire regular expression a livello di interfaccia per il controllo della corettezza dei dati!
        /// --- Gli spazi inseriti sono ignorati.
        /// </remarks>
        public static Boolean IsInRange(Int32 PageNumber, String Range)
        {
            String[] subRanges = Range.Replace(" ", "").Split(',');

            foreach (string sbr in subRanges)
            {
                if (sbr.Contains('-'))
                {
                    String[] Values = sbr.Split('-');
                    try
                    {
                        Int32 min = System.Convert.ToInt32(Values[0]);
                        Int32 max = System.Convert.ToInt32(Values[1]);

                        if (min <= max)
                        {
                            if (min <= PageNumber && max >= PageNumber)
                                return true;
                        }
                        else
                        {
                            //return false;
                            //throw new format exception!
                        }
                    }
                    catch
                    {
                        return false;
                    }
                } else {
                    try
                    {
                        Int32 Page = System.Convert.ToInt32(sbr);
                        if (PageNumber == Page)
                            return true;
                    }
                    catch
                    {
                        //return false;
                        //throw new format exception!
                    }
                }
            }

            return false;
            
        }


        /// <summary>
        /// Controlla se la pagina corrente è interna ai valori specificati
        /// </summary>
        /// <param name="PageNumber">Numero pagina corrente</param>
        /// <param name="MaskValue">Valore che indica quali parametri applicare: 0-63</param>
        /// <param name="TotalPage">numero totale di pagine</param>
        /// <param name="Range">Stringa formata da sottoblocchi seprati da virgola. Ogni sottoblocco può essere formato da due interi separati da trattino (range di pagine, es: 3-5) oppure da una singola pagina.</param>
        /// <returns>
        /// True se la pagina è nell'intervallo specificato.
        /// </returns>
        public static Boolean IsCorrectPage(Int32 PageNumber, int MaskValue, String Range)
        {
            //return true;

            if (MaskValue == (int)PageMaskingInclude.none)
                return false;
            if ((MaskValue & (int)PageMaskingInclude.All) > 0)
                return true;
            if (((MaskValue & (int)PageMaskingInclude.Even) > 0) && (IsEven(PageNumber)))
                return true;
            if (((MaskValue & (int)PageMaskingInclude.Odd) > 0) && (!IsEven(PageNumber)))
                return true;
            if (((MaskValue & (int)PageMaskingInclude.First) > 0) && (IsFirst(PageNumber)))
                return true;
            //if (((MaskValue & (int)PageMaskingInclude.Last) > 0) && (IsLast(PageNumber, TotalPage)))
            //    return true;
            if (((MaskValue & (int)PageMaskingInclude.Range) > 0) && (IsInRange(PageNumber, Range)))
                return true;

            return false;
        }
    }

    public enum PageMaskingInclude
    {
        none = 0,
        All = 1,
        Even = 2,
        Odd = 4,
        First = 8,
        Last = 16,
        Range = 32
    }
}
