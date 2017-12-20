using System;
namespace lm.Comol.Core.ModuleLinks
{
	[CLSCompliant(true), Serializable()]
	public class dtoPlaceHolder
	{
        public PlaceHolderType Type { get; set; }
        public String CssClass { get; set; }
		public String Text { get; set; }
        
		public dtoPlaceHolder()
		{
            Type = PlaceHolderType.none;
            Text = "";
            CssClass = "";
		}
        public dtoPlaceHolder(String css, String text)
            : this()
		{
            CssClass = css;
            Text = text;
		}
        public dtoPlaceHolder(String css, String text,PlaceHolderType type)
            : this()
        {
            Type = type;
            CssClass = css;
            Text = text;
        }
	}
}