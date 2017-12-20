Imports lm.Comol.UI.Presentation

Partial Public Class UC_GenericUploadFile
	Inherits System.Web.UI.UserControl
	Implements lm.Comol.Modules.Base.Presentation.IviewBaseFileUpload

#Region "ControlProperty"
	Private _CurrentManager As lm.Comol.Modules.Base.BusinessLogic.ManagerFiles
	Private _NotUploadedFile As IList(Of lm.Comol.Core.DomainModel.iBaseFile)
	Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

	Public Property InitialFileInputsCount() As Integer
		Get
			Return RDUfiles.InitialFileInputsCount
		End Get
		Set(ByVal value As Integer)
			If value > 0 Then
				RDUfiles.InitialFileInputsCount = value
			Else
				RDUfiles.InitialFileInputsCount = 1
			End If
			If RDUfiles.InitialFileInputsCount > Me.RDUfiles.MaxFileInputsCount Then
				Me.RDUfiles.MaxFileInputsCount = RDUfiles.InitialFileInputsCount
			End If
		End Set
	End Property
	Public Property MaxFileInputsCount() As Integer
		Get
			Return RDUfiles.MaxFileInputsCount
		End Get
		Set(ByVal value As Integer)
            If Not value > 0 Then
                value = 1
            End If
            RDUfiles.MaxFileInputsCount = value
			If RDUfiles.InitialFileInputsCount > Me.RDUfiles.MaxFileInputsCount Then
				Me.RDUfiles.InitialFileInputsCount = RDUfiles.MaxFileInputsCount
            End If
            If value = 1 Then
                Me.RDUfiles.ControlObjectsVisibility = Telerik.Web.UI.ControlObjectsVisibility.None
            Else
                Me.RDUfiles.ControlObjectsVisibility = Telerik.Web.UI.ControlObjectsVisibility.AddButton Or Telerik.Web.UI.ControlObjectsVisibility.RemoveButtons
            End If
		End Set
	End Property
	Public WriteOnly Property Skin() As String
		Set(ByVal value As String)
			If value <> "" Then
				RDUfiles.Skin = value
				RPApersonalFiles.Skin = value
			End If
		End Set
	End Property
	Public WriteOnly Property ReadOnlyFileInputs() As Boolean
		Set(ByVal value As Boolean)
			RDUfiles.ReadOnlyFileInputs = value
		End Set
	End Property
	Private Property CurrentManager() As lm.Comol.Modules.Base.BusinessLogic.ManagerFiles
		Get
			If IsNothing(_CurrentManager) Then
				_CurrentManager = New lm.Comol.Modules.Base.BusinessLogic.ManagerFiles(Me.CurrentContext)
			End If
			Return _CurrentManager
		End Get
		Set(ByVal value As lm.Comol.Modules.Base.BusinessLogic.ManagerFiles)
			_CurrentManager = value
		End Set
	End Property
	Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
		Get
			If IsNothing(_CurrentContext) Then
				_CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
			End If
			Return _CurrentContext
		End Get
	End Property
	Public Property NotUploadedFile() As IList(Of lm.Comol.Core.DomainModel.iBaseFile)
		Get
			If IsNothing(_NotUploadedFile) Then
				_NotUploadedFile = New List(Of lm.Comol.Core.DomainModel.iBaseFile)
			End If
			Return _NotUploadedFile
		End Get
		Set(ByVal value As IList(Of lm.Comol.Core.DomainModel.iBaseFile))
			_NotUploadedFile = value
		End Set
	End Property

	Public ReadOnly Property NotSubmitProgressAreaScript() As String
		Get
			Return "getRadProgressManager().startProgressPolling();return true;"
		End Get
	End Property
#End Region

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Me.Page.IsPostBack = False Then
			'Me.RPApersonalFiles
		End If
	End Sub

	Private Sub RPMmanagerUserFile_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles RPMpersonalFiles.DataBinding
		Dim oProgress As Telerik.Web.UI.RadProgressContext = Telerik.Web.UI.RadProgressContext.Current
		If Not IsNothing(oProgress) Then
			oProgress.SecondaryTotal = ""
			oProgress.SecondaryValue = ""
			oProgress.SecondaryPercent = ""
		End If
	End Sub

    Public Function AddToUserRepository(ByVal DestinationPath As String) As IList(Of lm.Comol.Core.DomainModel.BaseFile)
        Dim oList As IList(Of lm.Comol.Core.DomainModel.BaseFile) = New List(Of lm.Comol.Core.DomainModel.BaseFile)


        For Each oTelerikFile As Telerik.Web.UI.UploadedFile In Me.RDUfiles.UploadedFiles
            Dim oBaseFile As lm.Comol.Core.DomainModel.BaseFile = CreateFromTelerik(oTelerikFile)

            If IsNothing(oBaseFile) Then
                NotUploadedFile.Add(oBaseFile)
            Else
                Dim oSavedFile As lm.Comol.Core.DomainModel.BaseFile = Me.CurrentManager.Save(Me.CurrentContext.UserContext.CurrentUser.Id, Me.CurrentContext.UserContext.CurrentUser.Id, oBaseFile)
                If Not IsNothing(oSavedFile) Then
                    oTelerikFile.SaveAs(DestinationPath & oSavedFile.FileSystemName)
                    oList.Add(oSavedFile)
                Else
                    NotUploadedFile.Add(oBaseFile)
                End If
            End If
        Next
        Return oList
    End Function

	Private Function CreateFromTelerik(ByVal oTelerikFile As Telerik.Web.UI.UploadedFile) As lm.Comol.Core.DomainModel.iBaseFile
		Dim oBaseFile As New lm.Comol.Core.DomainModel.BaseFile

		oBaseFile.ContentType = oTelerikFile.ContentType
		oBaseFile.Description = ""
		oBaseFile.Name = oTelerikFile.GetNameWithoutExtension
		oBaseFile.Extension = oTelerikFile.GetExtension
		oBaseFile.Size = oTelerikFile.ContentLength
		Return oBaseFile
	End Function
End Class