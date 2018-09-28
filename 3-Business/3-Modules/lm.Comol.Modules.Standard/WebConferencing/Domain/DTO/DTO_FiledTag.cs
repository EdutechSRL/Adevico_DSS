using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Modules.Standard.WebConferencing.Domain.DTO
{
    [Serializable]
    public class DTO_FiledTag
    {
        public virtual List<String> FullTag { get; set; }
        public DTO_FiledTag(String content, String openTag, String closeTag)
        {
            FullTag = new List<String>();
            String[] startDelims = new string[] { openTag };
            String[] endDelims = new string[] { closeTag };
            List<String> tags = content.Split(startDelims, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string tag in tags.Where(t => t.Contains(closeTag)).ToList())
            {
                FullTag.Add(openTag + tag.Split(endDelims, StringSplitOptions.None).ToList().FirstOrDefault() + closeTag);
            }
        }

        public String ReplaceTag(String content, String fieldValue)
        {
            FullTag.ForEach(t => content = content.Replace(t, fieldValue));
            return content;
        }
        
    }
}
