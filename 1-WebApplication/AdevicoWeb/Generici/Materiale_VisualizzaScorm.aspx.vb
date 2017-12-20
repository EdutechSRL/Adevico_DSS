Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.Comol.Materiale.Scorm
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comol.Configurations

Partial Public Class Materiale_VisualizzaScorm
	Inherits PageBase
	Implements IviewVisualizzaScorm




	Public Overrides ReadOnly Property AlwaysBind() As Boolean
		Get
			Return False
		End Get
	End Property
	Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
		Get
			Return True
		End Get
	End Property

#Region "Proprietà private"
	Private _presenter As PresenterVisualizzaScorm
	Private _ScormConfig As ConfigurationPath
	Private _File As ConfigurationPath
	Private _Servizio As Services_File
#End Region

#Region "Proprietà pubbliche"
	Public ReadOnly Property InvalidImage() As String Implements IviewVisualizzaScorm.InvalidImage
		Get
			Return ""
		End Get
	End Property
	Public ReadOnly Property ItemImage() As String Implements IviewVisualizzaScorm.ItemImage
		Get
			Return Me.BaseUrl & "images/scorm/hiddenitemleaf.gif"
		End Get
	End Property
	Public ReadOnly Property ItemsImage() As String Implements IviewVisualizzaScorm.ItemsImage
		Get
			Return Me.BaseUrl & "images/scorm/menu_folder_closed.gif"
		End Get
	End Property
	Public ReadOnly Property MetaDataImage() As String Implements IviewVisualizzaScorm.MetaDataImage
		Get
			Return ""
		End Get
	End Property
	Public ReadOnly Property OrganizationsImage() As String Implements IviewVisualizzaScorm.OrganizationsImage
		Get
			Return Me.BaseUrl & "images/scorm/orgs.gif"
		End Get
	End Property
	Public ReadOnly Property PackageImage() As String Implements IviewVisualizzaScorm.PackageImage
		Get
			Return Me.BaseUrl & "images/scorm/book-open.gif"
		End Get
	End Property

	Private ReadOnly Property FileConfig() As ConfigurationPath Implements IviewVisualizzaScorm.FileConfig
		Get
			If IsNothing(_File) Then
				_File = Me.SystemSettings.File.Materiale
			End If
			FileConfig = _File
		End Get
	End Property
	Private ReadOnly Property ScormConfig() As ConfigurationPath Implements IviewVisualizzaScorm.ScormConfig
		Get
			If IsNothing(_ScormConfig) Then
				_ScormConfig = Me.SystemSettings.File.Scorm
			End If
			ScormConfig = _ScormConfig
		End Get
	End Property
	Public ReadOnly Property Servizio() As COL_BusinessLogic_v2.UCServices.Services_File Implements IviewVisualizzaScorm.Servizio
		Get
			If IsNothing(_Servizio) Then
				If Me.isModalitaAmministrazione Then 'And Me.isUtenteAnonimo 
					_Servizio = New Services_File(COL_Comunita.GetPermessiForServizioByCode(Main.TipoRuoloStandard.AdminComunità, Me.AmministrazioneComunitaID, Services_File.Codex))
				Else
					_Servizio = MyBase.ElencoServizi.Find(Services_File.Codex)
					If IsNothing(_Servizio) Then
						_Servizio = New Services_File("00000000000000000000000000000000")
					End If
				End If
			End If
			Servizio = _Servizio
		End Get
	End Property
	Public ReadOnly Property Presenter() As PresenterVisualizzaScorm Implements IviewVisualizzaScorm.Presenter
		Get
			If IsNothing(_presenter) Then
				_presenter = New PresenterVisualizzaScorm(Me)
			End If
			Presenter = _presenter
		End Get
	End Property

	Public ReadOnly Property VirtualPath___() As String Implements IviewVisualizzaScorm.VirtualPath
		Get
			If Me.ViewState("VirtualPath") = "" Then
				Me.ViewState("VirtualPath") = Me.ObjectPath(Me.ScormConfig).Virtual & Me.ComunitaLavoroID & "/" & ScormUniqueID & "/"
			End If
			Return Me.ViewState("VirtualPath")
		End Get
	End Property
	Public ReadOnly Property DrivePath() As String Implements IviewVisualizzaScorm.DrivePath
		Get
			If Me.ViewState("DrivePath") = "" Then
				Me.ViewState("DrivePath") = Me.ObjectPath(Me.ScormConfig).Drive & Me.ComunitaLavoroID & "\" & ScormUniqueID & "\"
				Me.ViewState("DrivePath") = Replace(Me.ViewState("DrivePath"), "\/", "\")
			End If
			Return Me.ViewState("DrivePath")
		End Get
	End Property

	Public ReadOnly Property ScormUniqueID() As Long Implements IviewVisualizzaScorm.ScormUniqueID
		Get
			Try
				If Not IsNumeric(Me.ViewState("ScormUniqueID")) Then
					Dim IDfile As String
					IDfile = Me.EncryptedQueryString("ScormUniqueID", SecretKeyUtil.EncType.Altro)
					If Not IsNumeric(IDfile) Then
						Me.ViewState("ScormUniqueID") = CLng(0)
					Else
						Me.ViewState("ScormUniqueID") = CLng(IDfile)
					End If
				End If
			Catch ex As Exception
				Me.ViewState("ScormUniqueID") = CLng(0)
			End Try
			ScormUniqueID = DirectCast(Me.ViewState("ScormUniqueID"), Long)
		End Get
	End Property

	Public WriteOnly Property PackageTree() As TreeNode Implements IviewVisualizzaScorm.ScormPackage
		Set(ByVal oLista As TreeNode)
			If IsNothing(oLista) Then
				Me.TRVpackageScorm.DataSource = Nothing
				Me.TRVpackageScorm.DataBind()
			Else
				Me.TRVpackageScorm.Nodes.Add(oLista)
			End If
		End Set
	End Property
	Public Property ScormPackageBasePath() As String Implements IviewVisualizzaScorm.ScormPackageBasePath
		Get
			Return Me.ViewState("ScormPackageBasePath")
		End Get
		Set(ByVal value As String)
			Me.ViewState("ScormPackageBasePath") = value
		End Set
	End Property

	Public WriteOnly Property PackageDownLoadUrl() As String Implements IviewVisualizzaScorm.PackageDownLoadUrl
		Set(ByVal QueryString As String)
			Dim UrlDownLoad As String = ""

			If String.IsNullOrEmpty(QueryString) Then
				Me.LNBdownload.Attributes("onclick") = "return false;"
			Else
				UrlDownLoad = Me.EncryptedUrl("MaterialeScorm.download", QueryString, SecretKeyUtil.EncType.Altro)
				Me.LNBdownload.Attributes("onclick") = "javascript:window.open('" & UrlDownLoad & "');return false;"
			End If
		End Set
	End Property

	Public Property ActivateFromUrl() As String Implements IviewVisualizzaScorm.ActivateFromUrl
		Get
			Return Me.ViewState("ActivateFromUrl")
		End Get
		Set(ByVal value As String)
			Me.ViewState("ActivateFromUrl") = value
		End Set
	End Property
#End Region

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
	End Sub

	Public Overrides Sub BindDati()
		Me.Presenter.Init()
		If IsNothing(Me.Request.UrlReferrer) Then
			Me.ActivateFromUrl = ""
		Else
			Me.ActivateFromUrl = Me.Request.UrlReferrer.AbsoluteUri
		End If
	End Sub

	Public Overrides Sub BindNoPermessi()
		Me.Resource.setLabel(Me.LBNopermessi)
		Me.LNBdownload.Visible = False
		Me.MLVcontenuto.SetActiveView(Me.VIWpermessi)
	End Sub

	Public Overrides Function HasPermessi() As Boolean
		Me.LNBdownload.Visible = True
		Return Me.Servizio.Admin Or Me.Servizio.Moderate Or Me.Servizio.Read Or Me.Servizio.Upload
	End Function

	Public Overrides Sub RegistraAccessoPagina()
		'	Me.RegistraEvento("MATERIALE", , "IX", "VISUALIZZA_SCORM")
	End Sub

	Public Overrides Sub SetCultureSettings()
		MyBase.SetCulture("pg_Materiale_VisualizzaScorm", "Generici")
	End Sub

	Public Overrides Sub SetInternazionalizzazione()
		With Me.Resource
			.setLabel(Me.LBtitoloServizio)
			.setLabel(Me.LBNopermessi)
			.setLinkButton(Me.LNBindietro, True, True)
			.setLinkButton(Me.LNBdownload, True, True)
		End With
	End Sub

	Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String) Implements IviewVisualizzaScorm.ShowErrorMessager
		Me.MLVcontenuto.SetActiveView(Me.VIWpermessi)
		Me.LBNopermessi.Text = errorMessage
	End Sub

	Protected Sub TRVpackageScorm_SelectedNodeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TRVpackageScorm.SelectedNodeChanged
		Me.Presenter.ShowResources(Me.TRVpackageScorm.SelectedNode.Value)
	End Sub

	Private Sub DTLmateriale_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DTLmateriale.ItemCreated
		If e.Item.ItemType = ListItemType.Header Then
			Dim oLabel As Label = e.Item.FindControl("LBtitoloElencoMateriale")

			If Not IsNothing(oLabel) Then
				Me.Resource.setLabel(oLabel)
			End If
		End If
	End Sub

	Private Sub DTLmateriale_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles DTLmateriale.ItemDataBound
		Dim oResourceFile As SCORM_ResourceFile

		Try
			oResourceFile = DirectCast(e.Item.DataItem, SCORM_ResourceFile)
			If Not IsNothing(oResourceFile) Then
				Dim oNomeFile As String = ""
				Dim oLBnomeFile, oLBdimensione As Label
				Dim oHYPfile As HyperLink

				oLBnomeFile = e.Item.FindControl("LBnomeFile")
				oHYPfile = e.Item.FindControl("HYPfile")
				oLBdimensione = e.Item.FindControl("LBdimensione")

				Dim quote As String = """"

				oNomeFile = "<img src=" & quote & oResourceFile.Extension & Me.SystemSettings.Extension.ExtensionToShow & quote & " alt=" & quote & quote & ">&nbsp;" & oResourceFile.FileNameAndExtension
				oLBnomeFile.Text = oNomeFile
				oHYPfile.Text = oNomeFile
				oHYPfile.NavigateUrl = Me.BaseUrl & Me.ScormConfig.RewritePath & Me.ComunitaCorrenteID & "\" & Me.ScormUniqueID & "\" & oResourceFile.Url
				oHYPfile.Attributes.Add("onmouseover", "window.status='';return true;")
				If Me.Servizio.Admin Or Me.Servizio.Moderate Or Me.Servizio.Read Then
					oHYPfile.Visible = True
					oLBnomeFile.Visible = False
				Else
					oHYPfile.Visible = False
					oLBnomeFile.Visible = True
				End If
			End If
		Catch ex As Exception

		End Try
	End Sub

	Public Sub ShowResourceList(ByVal iListaFile As System.Collections.Generic.List(Of COL_BusinessLogic_v2.Comol.Materiale.Scorm.SCORM_ResourceFile)) Implements IviewVisualizzaScorm.ShowResourceList
		Me.DTLmateriale.DataSource = iListaFile
		Me.DTLmateriale.DataBind()
	End Sub

	Private Sub LNBindietro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBindietro.Click
		If Me.ActivateFromUrl <> "" Then
			Response.Redirect(Me.ActivateFromUrl)
		End If
	End Sub
End Class