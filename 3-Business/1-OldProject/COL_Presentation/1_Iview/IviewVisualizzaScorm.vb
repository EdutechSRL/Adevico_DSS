Imports System.Collections.Generic

Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comol.Materiale.Scorm

Public Interface IviewVisualizzaScorm
	Inherits IviewBase

	ReadOnly Property Servizio() As Services_File
	ReadOnly Property ScormConfig() As ConfigurationPath
	ReadOnly Property FileConfig() As ConfigurationPath
	ReadOnly Property Presenter() As PresenterVisualizzaScorm

	ReadOnly Property DrivePath() As String
	ReadOnly Property VirtualPath() As String
	ReadOnly Property ScormUniqueID() As Long
	WriteOnly Property ScormPackage() As TreeNode
	Property ScormPackageBasePath() As String
	Sub ShowErrorMessager(ByVal message As String)
	Sub ShowResourceList(ByVal iListaFile As List(Of SCORM_ResourceFile))


	ReadOnly Property OrganizationsImage() As String
	ReadOnly Property PackageImage() As String
	ReadOnly Property ItemsImage() As String
	ReadOnly Property ItemImage() As String
	ReadOnly Property InvalidImage() As String
	ReadOnly Property MetaDataImage() As String
	WriteOnly Property PackageDownLoadUrl() As String
	Property ActivateFromUrl() As String
End Interface