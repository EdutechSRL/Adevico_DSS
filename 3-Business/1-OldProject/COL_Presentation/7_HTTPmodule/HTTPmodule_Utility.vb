Imports COL_BusinessLogic_v2


Public Class HTTPmodule_Utility
	Inherits HTTPhandlerModuleUtility


	Public Sub New(ByVal context As System.Web.HttpContext)
		MyBase.New(context)
	End Sub
	Public Sub New(ByVal context As System.Web.HttpContext, ByVal oConfig As FileSettings.ConfigType)
		MyBase.New(context, oConfig)
	End Sub

End Class