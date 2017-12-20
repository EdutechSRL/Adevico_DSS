using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTS = iTextSharp5.text;


namespace lm.Comol.Core.DomainModel.DocTemplateVers.Helpers
{
    /// <summary>
    /// Measure converter, based on iTextSharp (72px/inch)
    /// </summary>
    [Serializable]
    public static class Measure
    {
        /// <summary>
        /// Convert mm to Px (72px/inch)
        /// </summary>
        /// <param name="mm">value in mm</param>
        /// <returns>value in Px</returns>
        public static Single mm_To_Px(Single mm)
        {
            return (float) System.Math.Round(mm / 25.4f * 72, 0);
        }
        /// <summary>
        /// Convert mm to cm
        /// </summary>
        /// <param name="mm">value in mm</param>
        /// <returns>value in cm</returns>
        public static Single mm_To_cm(Single mm)
        {
            return (float) System.Math.Round(mm / 10, 1);
        }
        /// <summary>
        /// Convert mm to Inch
        /// </summary>
        /// <param name="mm">value in mm</param>
        /// <returns>value in inch</returns>
        public static Single mm_To_Inch(Single mm)
        {
            return (float) System.Math.Round(mm / 25.4f, 2);
        }


        /// <summary>
        /// Convert cm to Px (72px/inch)
        /// </summary>
        /// <param name="mm">value in cm</param>
        /// <returns>value in Px</returns>
        public static Single cm_To_Px(Single cm)
        {
            return (float) System.Math.Round(cm / 2.54f * 72, 0);
        }
        /// <summary>
        /// Convert cm to mm
        /// </summary>
        /// <param name="mm">value in cm</param>
        /// <returns>value in mm</returns>
        public static Single cm_To_mm(Single cm)
        {
            return (float) System.Math.Round(cm * 10, 0);
        }
        /// <summary>
        /// Convert cm to inch
        /// </summary>
        /// <param name="mm">value in cm</param>
        /// <returns>value in inch</returns>
        public static Single cm_To_Inch(Single cm)
        {
            return (float) System.Math.Round(cm / 2.54f, 2);
        }


        /// <summary>
        /// Convert Px to cm (72px/inch)
        /// </summary>
        /// <param name="mm">value in px</param>
        /// <returns>value in cm</returns>
        public static Single Px_To_cm(Single Px)
        {
            return (float) System.Math.Round(Px / 72 * 2.54f, 1);
        }
        /// <summary>
        /// Convert Px to mm (72px/inch)
        /// </summary>
        /// <param name="mm">value in px</param>
        /// <returns>value in mm</returns>
        public static Single Px_To_mm(Single Px)
        {
            return (float) System.Math.Round(Px / 72 * 25.4f, 0);
        }
        /// <summary>
        /// Convert Px to inch (72px/inch)
        /// </summary>
        /// <param name="mm">value in px</param>
        /// <returns>value in inch</returns>
        public static Single Px_To_Inch(Single Px)
        {
            return (float) System.Math.Round(Px / 72, 2);
        }

        /// <summary>
        /// Convert inch to cm
        /// </summary>
        /// <param name="mm">value in inch</param>
        /// <returns>value in cm</returns>
        public static Single Inch_To_cm(Single Inch)
        {
            return (float) System.Math.Round(Inch * 2.54f, 1);
        }
        /// <summary>
        /// Convert inch to mm
        /// </summary>
        /// <param name="mm">value in inch</param>
        /// <returns>value in mm</returns>
        public static Single Inch_To_mm(Single Inch)
        {
            return (float) System.Math.Round(Inch * 25.4f, 0);
        }
        /// <summary>
        /// Convert inch to px (72px/inch)
        /// </summary>
        /// <param name="mm">value in inch</param>
        /// <returns>value in px</returns>
        public static Single Inch_To_Px(Single Inch)
        {
            return (float) System.Math.Round(Inch * 72, 0);
        }

        /// <summary>
        /// Get Pagesize from specific format
        /// </summary>
        /// <param name="mm">value in inch</param>
        /// <returns>value in cm</returns>
        public static PageSizeValue GetSize(PageSize Size, String Measure)
        {
            PageSizeValue OutSize = new PageSizeValue();
            OutSize.Width = 0;
            OutSize.Height = 0;

            iTS.Rectangle Rect = null;

            switch (Size)
            {
                case PageSize._11X17:
                    Rect = iTS.PageSize._11X17;
                    break;
                case PageSize._11X17_L:
                    Rect = iTS.PageSize._11X17.Rotate();
                    break;
                case PageSize.A0:
                    Rect = iTS.PageSize.A0;
                    break;
                case PageSize.A0_L:
                    Rect = iTS.PageSize.A0.Rotate();
                    break;
                case PageSize.A1:
                    Rect = iTS.PageSize.A1;
                    break;
                case PageSize.A1_L:
                    Rect = iTS.PageSize.A1.Rotate();
                    break;
                case PageSize.A2:
                    Rect = iTS.PageSize.A2;
                    break;
                case PageSize.A2_L:
                    Rect = iTS.PageSize.A2.Rotate();
                    break;
                case PageSize.A3:
                    Rect = iTS.PageSize.A3;
                    break;
                case PageSize.A3_L:
                    Rect = iTS.PageSize.A3.Rotate();
                    break;
                case PageSize.A4:
                    Rect = iTS.PageSize.A4;
                    break;
                case PageSize.A4_L:
                    Rect = iTS.PageSize.A4.Rotate();
                    break;
                case PageSize.A5:
                    Rect = iTS.PageSize.A5;
                    break;
                case PageSize.A5_L:
                    Rect = iTS.PageSize.A5.Rotate();
                    break;
                case PageSize.A6:
                    Rect = iTS.PageSize.A6;
                    break;
                case PageSize.A6_L:
                    Rect = iTS.PageSize.A6.Rotate();
                    break;
                case PageSize.A7:
                    Rect = iTS.PageSize.A7;
                    break;
                case PageSize.A7_L:
                    Rect = iTS.PageSize.A7.Rotate();
                    break;
                case PageSize.A8:
                    Rect = iTS.PageSize.A8;
                    break;
                case PageSize.A8_L:
                    Rect = iTS.PageSize.A8.Rotate();
                    break;
                case PageSize.A9:
                    Rect = iTS.PageSize.A9;
                    break;
                case PageSize.A9_L:
                    Rect = iTS.PageSize.A9.Rotate();
                    break;
                case PageSize.A10:
                    Rect = iTS.PageSize.A10;
                    break;
                case PageSize.A10_L:
                    Rect = iTS.PageSize.A10.Rotate();
                    break;
                case PageSize.B0:
                    Rect = iTS.PageSize.B0;
                    break;
                case PageSize.B0_L:
                    Rect = iTS.PageSize.B0.Rotate();
                    break;
                case PageSize.B1:
                    Rect = iTS.PageSize.B1;
                    break;
                case PageSize.B1_L:
                    Rect = iTS.PageSize.B1.Rotate();
                    break;
                case PageSize.B2:
                    Rect = iTS.PageSize.B2;
                    break;
                case PageSize.B2_L:
                    Rect = iTS.PageSize.B2.Rotate();
                    break;
                case PageSize.B3:
                    Rect = iTS.PageSize.B3;
                    break;
                case PageSize.B3_L:
                    Rect = iTS.PageSize.B3.Rotate();
                    break;
                case PageSize.B4:
                    Rect = iTS.PageSize.B4;
                    break;
                case PageSize.B4_L:
                    Rect = iTS.PageSize.B4.Rotate();
                    break;
                case PageSize.B5:
                    Rect = iTS.PageSize.B5;
                    break;
                case PageSize.B5_L:
                    Rect = iTS.PageSize.B5.Rotate();
                    break;
                case PageSize.B6:
                    Rect = iTS.PageSize.B6;
                    break;
                case PageSize.B6_L:
                    Rect = iTS.PageSize.B6.Rotate();
                    break;
                case PageSize.B7:
                    Rect = iTS.PageSize.B7;
                    break;
                case PageSize.B7_L:
                    Rect = iTS.PageSize.B7.Rotate();
                    break;
                case PageSize.B8:
                    Rect = iTS.PageSize.B8;
                    break;
                case PageSize.B8_L:
                    Rect = iTS.PageSize.B8.Rotate();
                    break;
                case PageSize.B9:
                    Rect = iTS.PageSize.B9;
                    break;
                case PageSize.B9_L:
                    Rect = iTS.PageSize.B9.Rotate();
                    break;
                case PageSize.B10:
                    Rect = iTS.PageSize.B10;
                    break;
                case PageSize.B10_L:
                    Rect = iTS.PageSize.B10.Rotate();
                    break;
                case PageSize.HALFLETTER:
                    Rect = iTS.PageSize.HALFLETTER;
                    break;
                case PageSize.HALFLETTER_L:
                    Rect = iTS.PageSize.HALFLETTER.Rotate();
                    break;
                case PageSize.LEGAL:
                    Rect = iTS.PageSize.LEGAL;
                    break;
                case PageSize.LEGAL_L:
                    Rect = iTS.PageSize.LEGAL.Rotate();
                    break;
                case PageSize.LETTER:
                    Rect = iTS.PageSize.LETTER;
                    break;
                case PageSize.LETTER_L:
                    Rect = iTS.PageSize.LETTER.Rotate();
                    break;
                case PageSize.NOTE:
                    Rect = iTS.PageSize.NOTE;
                    break;
                case PageSize.NOTE_L:
                    Rect = iTS.PageSize.NOTE.Rotate();
                    break;
                case PageSize.POSTCARD:
                    Rect = iTS.PageSize.POSTCARD;
                    break;
                case PageSize.POSTCARD_L:
                    Rect = iTS.PageSize.POSTCARD.Rotate();
                    break;
                case PageSize.TABLOID:
                    Rect = iTS.PageSize.TABLOID;
                    break;
                case PageSize.TABLOID_L:
                    Rect = iTS.PageSize.TABLOID.Rotate();
                    break;
            }
            
            switch (Measure.ToLower())
            {
                case "mm":
                    OutSize.Height = Px_To_mm(Rect.Height);
                    OutSize.Width = Px_To_mm(Rect.Width);
                    break;
                case "cm":
                    OutSize.Height = Px_To_cm(Rect.Height);
                    OutSize.Width = Px_To_cm(Rect.Width);
                    break;
                case "inch":
                    OutSize.Height = Px_To_Inch(Rect.Height);
                    OutSize.Width = Px_To_Inch(Rect.Width);
                    break;
                default:
                    OutSize.Height = (float) System.Math.Round(Rect.Height, 0);
                    OutSize.Width = (float)System.Math.Round(Rect.Width, 0);
                    break;
            }
            
            return OutSize;
        }
    }


    [Serializable]
    public class PageSizeValue
    {
        public Single Width { get; set; }
        public Single Height { get; set; }
    }


}
