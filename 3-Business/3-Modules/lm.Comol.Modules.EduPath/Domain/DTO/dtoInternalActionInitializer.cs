using System;
namespace lm.Comol.Modules.EduPath.Domain
{
	[CLSCompliant(true), Serializable()]
	public class dtoInternalActionInitializer : lm.Comol.Core.ModuleLinks.dtoModuleDisplayActionInitializer
	{
        public long IdPath { get; set; }
        public long IdUnit { get; set; }
        public long IdActivity { get; set; }
        public Int32 IdPathCommunity { get; set; }
        public String CookieName { get; set; }
        public dtoSubActivity SubActivity { get; set; }
   	}
}