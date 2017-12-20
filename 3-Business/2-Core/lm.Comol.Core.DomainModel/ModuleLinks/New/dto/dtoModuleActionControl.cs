
using System;
namespace lm.Comol.Core.DomainModel
{
	[CLSCompliant(true), Serializable()]
	public class dtoModuleActionControl
	{
        public string ControlUrl { get; set; }
        public string ExtraData { get; set; }
        public string LinkUrl { get; set; }
        public Boolean isEnabled { get; set; }
		public StandardActionType ControlType { get; set; }
        public Boolean RefreshContainerPage { get; set; }
        public Boolean OnModalPage { get; set; }
        public Boolean SaveLinkStatistics { get; set; }

        public Boolean isPopupUrl;
		public dtoModuleActionControl()
		{
			isPopupUrl = false;
            isEnabled = true;
			ControlType = StandardActionType.Play;
		}
		public dtoModuleActionControl(string pLinkUrl, bool isPopup)
		{
			isPopupUrl = isPopup;
            isEnabled = true;
			LinkUrl = pLinkUrl;
			ControlType = StandardActionType.Play;
		}
		public dtoModuleActionControl(string pLinkUrl, StandardActionType pControlType, bool isPopup)
		{
			isPopupUrl = isPopup;
            isEnabled = true;
			LinkUrl = pLinkUrl;
			ControlType = pControlType;
		}
		public dtoModuleActionControl(string pLinkUrl, bool pEnabled, StandardActionType pControlType, bool isPopup)
		{
			isPopupUrl = isPopup;
            isEnabled = pEnabled;
			LinkUrl = pLinkUrl;
			ControlType = pControlType;
		}
	}
}