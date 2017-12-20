namespace lm.Comol.Core.DomainModel.Common
{
	public class CoreRootObject
	{
        //public static string GenericDisplayActionControl()
        //{
        //    return "Modules/Common/UC/UC_ModuleToModuleDisplayAction.ascx";
        //}
        public static string GenericNewDisplayActionControl()
		{
			return "Modules/Common/UC/UC_ExternalModuleDisplayAction.ascx";
		}
		public static string ImageExportToPdf()
		{
			return "images/Grid/exportPDF.png";
		}
		public static string ImageExportToRTF()
		{
			return "images/Grid/exportRTF.png";
		}
	}
}