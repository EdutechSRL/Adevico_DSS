Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel

Namespace lm.Comol.Modules.Base.Presentation
	Public Interface IviewBaseFileDownload
		Inherits lm.Comol.Core.DomainModel.Common.iDomainView

        ReadOnly Property BaseFileID() As System.Guid
		Property FileName() As String
		Sub DownloadFile(ByVal FileName As String, ByVal oBasefile As lm.Comol.Core.DomainModel.BaseFile)
		Sub WriteFile(ByVal content() As Byte, ByVal FileName As String, ByVal ContentType As String)
	End Interface
End Namespace