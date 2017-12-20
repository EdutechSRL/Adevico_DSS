using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace lm.Comol.Core.DomainModel.Helpers
{
    public static class TextUtility
    {
        /// <summary> 
        /// Truncate a given text to the given number of characters. 
        /// Also any embedded html is stripped. 
        /// </summary> 
        /// <param name="fullText"></param> 
        /// <param name="numberOfCharacters"></param> 
        /// <returns></returns> 
        public static string TruncateText(string fullText, int numberOfCharacters)
        {
	        string text = null;
	        if (fullText.Length > numberOfCharacters) {
		        int spacePos = fullText.IndexOf(" ", numberOfCharacters);
		        if (spacePos > -1) {
			        text = fullText.Substring(0, spacePos) + "...";
		        } else {
			        text = fullText;
		        }
	        } else {
		        text = fullText;
	        }
	        Regex regexStripHTML = new Regex("<[^>]+>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
	        text = regexStripHTML.Replace(text, " ");
	        return text;
        }

        /// <summary> 
        /// Ensure that the given string has a trailing slash. 
        /// </summary> 
        /// <param name="stringThatNeedsTrailingSlash"></param> 
        /// <returns></returns> 
        public static string EnsureTrailingSlash(string stringThatNeedsTrailingSlash)
        {
	        if (!stringThatNeedsTrailingSlash.EndsWith("/")) {
		        return stringThatNeedsTrailingSlash + "/";
	        } else {
		        return stringThatNeedsTrailingSlash;
	        }
        }

        public static string SanitizeFileName(string fileName, char? replacement = null)
        {
            if (fileName == null) { return null; }
            if (fileName.Length == 0) { return ""; }

            var sb = new StringBuilder();
            var badChars = Path.GetInvalidFileNameChars().ToList();

            foreach (var @char in fileName)
            {
                if (badChars.Contains(@char))
                {
                    if (replacement.HasValue)
                    {
                        sb.Append(replacement.Value);
                    }
                    continue;
                }
                sb.Append(@char);
            }
            return sb.ToString();
        }


    }
}
