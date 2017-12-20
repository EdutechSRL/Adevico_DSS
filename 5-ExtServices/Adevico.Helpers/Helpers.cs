using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Adevico.StatExporter.Helpers
{
    public static class Helpers
    {
        /// <summary>
        /// Crea una stringa "timestamp", utilizzabile come nome file della data corrente,
        /// nel formato indicato 
        /// </summary>
        /// <param name="format">Formato Output del timestamp</param>
        /// <returns></returns>
        public static string FileTimeStampGet(string format)
        {
            string timeStamp;

            try
            {
                timeStamp = DateTime.Now.ToString(format);
            }
            catch (Exception)
            {
                timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            }

            if (timeStamp.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            }

            return timeStamp;
        }

        /// <summary>
        /// Verifica che una terna di valori anno, mese, giorno corrispondano ad una data.
        /// </summary>
        /// <param name="year">Anno</param>
        /// <param name="month">Mese</param>
        /// <param name="day">Giorno</param>
        /// <remarks>
        /// SE non è una data valida, i valori vengono impostati a 0.
        /// </remarks>
        public static void DateTimeCheck(ref int year, ref int month, ref int day)
        {
            //Check sulle date
            day = (day < 0) ? 0 : day;
            month = (month < 0) ? 0 : month;
            year = (year < 0) ? 0 : year;

            if (year <= 0)
            {
                year = 0;
                month = 0;
                day = 0;
            }
            if (month <= 0 || month > 12)
            {
                month = 0;
                day = 0;
            }
            if (day <= 0 || day > 31)
            {
                day = 0;
            }

            try
            {
                DateTime dt = new DateTime(year, month, day);

            }
            catch (Exception)
            {
                day = 0;
                month = 0;
                year = 0;
            }
        }
    }
}
