using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Security.Cryptography;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain
{
    /// <summary>
    /// Classe Helper per generazioni codici utente e stanza.
    /// Crea due stringhe di poche lettere casuali che antepone e pospone all'ID in forma esadecimale:
    /// ccc##ccc
    /// c => lettera casuale
    /// # => Id in esadecimale
    /// </summary>
    public class CodeHelper
    {
        /// <summary>
        /// Generate a Code
        /// </summary>
        /// <param name="Id">Id - For unique generation.</param>
        /// <returns>
        ///     Code:
        ///         RND + Id.ToHEX + RND
        ///     RND => 2 or 3 random chr - SOLO lettere
        /// </returns>
        public static String GenerateCode(Int64 Id)
        {

            String BaseID = Id.ToString("X");

            int AddLenght = (7 - BaseID.Length) / 2;

            if (AddLenght < 2) AddLenght = 2;

            BaseID = RandomString(AddLenght) + BaseID + RandomString(AddLenght);

            return BaseID;
        }

        /// <summary>
        /// Generate a random string with specific size
        /// </summary>
        /// <param name="size">Output string size in char</param>
        /// <returns>A random String</returns>
        public static string RandomString(int size)
        {
            

            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                Int32 Aciichar = Convert.ToInt32(Math.Floor(58 * random.NextDouble() + 65)); //27 (26)
                
                if (Aciichar > 90 && Aciichar < 97)
                    Aciichar += 7;

                ch = Convert.ToChar(Aciichar);
                builder.Append(ch);
            }

            return builder.ToString();
        }

        private static Random _random;
        private static Random random
        {
            get
            {
                if(_random == null)
                    _random = new Random((int)DateTime.Now.Ticks);

                return _random;
            }
        }
    }
}
