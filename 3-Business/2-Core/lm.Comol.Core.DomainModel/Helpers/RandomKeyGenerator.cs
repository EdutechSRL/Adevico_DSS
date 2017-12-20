using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Text;
using System.Security.Cryptography;

namespace lm.Comol.Core.DomainModel.Helpers
{
    public enum CharacterType
    {

        UpperCase,

        LowerCase,

        Number,

        Special

    }



    public static class RandomKeyGenerator
    {

        private static readonly char[] _Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private static readonly char[] _Numbers = "1234567890".ToCharArray();
        private static readonly char[] _Symbols = "!@#$%^&*.?".ToCharArray();
        private static readonly Random _random = new Random();

        public static string GenerateRandomKey(int minimumLength, int maximumLength,

                                              bool allowCharacters, bool allowNumbers, bool allowSymbols)
        {

            string[] _CharacterTypes;

            _CharacterTypes = getCharacterTypes(allowCharacters, allowNumbers, allowSymbols);

            StringBuilder randomKey = new StringBuilder(maximumLength);
            int currentRandomKeyLength = _random.Next(maximumLength);

            if (currentRandomKeyLength < minimumLength)
            {

                currentRandomKeyLength = minimumLength;

            }

            //Generate the randomKey

            for (int i = 0; i < currentRandomKeyLength; i++)
            {

                randomKey.Append(getCharacter(_CharacterTypes));

            }

            return randomKey.ToString();

        }



        public static string GenerateRandomKey()
        {

            return GenerateRandomKey(10, 10, true, true, true);

        }

        // Getting character types allowed in the key //(UpperCase,LowerCase,Number,Special)

        //Parameters

        //Whether to allow characters

        //Whether to allow numbers

        //Whether to allow symbols

        //Return type as string array.

        private static string[] getCharacterTypes(bool allowCharacters, bool allowNumbers, bool allowSymbols)
        {

            ArrayList alCharacterTypes = new ArrayList();

            foreach (string characterType in Enum.GetNames(typeof(CharacterType)))
            {

                CharacterType currentType =

                  (CharacterType)Enum.Parse(typeof(CharacterType),

                  characterType, false);

                bool addType = false;

                switch (currentType)
                {

                    case CharacterType.LowerCase:

                        addType = allowCharacters;

                        break;

                    case CharacterType.Number:

                        addType = allowNumbers;

                        break;

                    case CharacterType.Special:

                        addType = allowSymbols;

                        break;

                    case CharacterType.UpperCase:

                        addType = allowCharacters;

                        break;

                }

                if (addType)
                {

                    alCharacterTypes.Add(characterType);

                }

            }

            return (string[])alCharacterTypes.ToArray(typeof(string));

        }





        // Getting character type randomly from the array of character types

        //Parameter is Array of allowed character types.

        // One of the types as string



        private static string getCharacter(string[] characterTypes)
        {

            string characterType =

                         characterTypes[_random.Next(characterTypes.Length)];

            CharacterType typeToGet =

               (CharacterType)Enum.Parse(typeof(CharacterType), characterType, false);

            switch (typeToGet)
            {

                case CharacterType.LowerCase:

                    return _Letters[_random.Next(_Letters.Length)].ToString().ToLower();

                case CharacterType.UpperCase:

                    return _Letters[_random.Next(_Letters.Length)].ToString().ToUpper();

                case CharacterType.Number:

                    return _Numbers[_random.Next(_Numbers.Length)].ToString();

                case CharacterType.Special:

                    return _Symbols[_random.Next(_Symbols.Length)].ToString();

            }

            return null;

        }

    }
}