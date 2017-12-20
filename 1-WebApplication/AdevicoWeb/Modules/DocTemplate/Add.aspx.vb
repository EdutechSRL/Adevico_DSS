Imports lm.Comol.Core.BaseModules.DocTemplate

Imports TemplateVers = lm.Comol.Core.DomainModel.DocTemplateVers

Public Class AddDocTemplate
    Inherits PageBase
    Implements Presentation.IViewDocTemplateAdd

#Region "Context"
    Private _Presenter As Presentation.DocTemplateAddPresenter
    Private ReadOnly Property CurrentPresenter() As Presentation.DocTemplateAddPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New Presentation.DocTemplateAddPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property

#End Region

#Region "Implements"
    Public ReadOnly Property HasServices As Boolean Implements Presentation.IViewDocTemplateAdd.HasServices
        Get
            If (Me.RBLservices.SelectedValue <> "0") Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedTemplateId As Long Implements Presentation.IViewDocTemplateAdd.PreloadedTemplateId
        Get
            If IsNumeric(Request.QueryString("idTemplate")) Then
                Return CLng(Request.QueryString("idTemplate"))
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property PreloadedVersionId As Long Implements Presentation.IViewDocTemplateAdd.PreloadedVersionId
        Get
            If IsNumeric(Request.QueryString("idVersion")) Then
                Return CLng(Request.QueryString("idVersion"))
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property SelectedServicesId As System.Collections.Generic.IList(Of Long) Implements Presentation.IViewDocTemplateAdd.SelectedServicesId
        Get
            Dim Ids As IList(Of Int64) = New List(Of Int64)

            If (Me.RBLservices.SelectedValue = "1") Then
                For Each li As ListItem In Me.CBLservices.Items
                    If (li.Selected) Then
                        Dim Id As Int64 = 0
                        Try
                            Id = System.Convert.ToInt64(li.Value)
                        Catch ex As Exception

                        End Try

                        If (Id > 0) Then
                            Ids.Add(Id)
                        End If
                    End If
                Next
            End If

            Return Ids

        End Get
    End Property
    Public ReadOnly Property TemplateType As lm.Comol.Core.DomainModel.DocTemplateVers.TemplateType Implements Presentation.IViewDocTemplateAdd.TemplateType
        Get
            Select Case Me.RBLtype.SelectedValue
                Case "0"
                    Return lm.Comol.Core.DomainModel.DocTemplateVers.TemplateType.Standard
                Case "1"
                    Return lm.Comol.Core.DomainModel.DocTemplateVers.TemplateType.System
                Case "2"
                    Return lm.Comol.Core.DomainModel.DocTemplateVers.TemplateType.Skin
                Case Else
                    Return lm.Comol.Core.DomainModel.DocTemplateVers.TemplateType.Standard
            End Select
        End Get
    End Property
    Public ReadOnly Property TemplateName As String Implements Presentation.IViewDocTemplateAdd.TemplateName
        Get
            Return Me.TXBname.Text
        End Get
    End Property
    Public Property CurrentPermission As lm.Comol.Core.DomainModel.DocTemplateVers.ModuleDocTemplate Implements Presentation.IViewDocTemplateAdd.CurrentPermission
        Get
        End Get
        Set(value As lm.Comol.Core.DomainModel.DocTemplateVers.ModuleDocTemplate)
        End Set
    End Property
    Public ReadOnly Property PreloadBackUrl As String Implements Presentation.IViewDocTemplateAdd.PreloadBackUrl
        Get
            Return Request.QueryString("BackUrl")
        End Get
    End Property

#End Region

#Region "Internal"
    Private Property TranslatedModules As List(Of PlainService)
        Get
            Return ViewStateOrDefault("TranslatedModules", ManagerService.ListSystemTranslated(PageUtility.LinguaID))
        End Get
        Set(value As List(Of PlainService))
            ViewState("TranslatedModules") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Master.ShowDocType = True
    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Me.CurrentPresenter.InitView()
    End Sub
    Public Overrides Sub BindNoPermessi()
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function
    Public Overrides Sub RegistraAccessoPagina()
    End Sub
    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_templateAdd", "Modules", "DocTemplates")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLiteral(LTtitle_t)

            .setLiteral(LTtype_t)
            .setLiteral(LTservices_t)

            .setLabel(LBLname_t)
            .setLabel(LBLtypeDescription_t)
            .setLabel(LBLservicesDescription_t)

            .setLinkButton(LKBcrea, True, True, False, False)
            .setHyperLink(Me.HYPundo, True, True, False, False)
        End With

        Me.SetListItem(RBLtype.ID.ToString, 0, RBLtype.Items(0))
        Me.SetListItem(RBLtype.ID.ToString, 1, RBLtype.Items(1))
        Me.SetListItem(RBLtype.ID.ToString, 2, RBLtype.Items(2))

        Me.SetListItem(RBLservices.ID.ToString, 0, RBLservices.Items(0))
        Me.SetListItem(RBLservices.ID.ToString, 1, RBLservices.Items(1))
    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)
    End Sub
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get

        End Get
    End Property
#End Region

#Region "Implements"
    Public Sub BindView(dtoAddTemplate As lm.Comol.Core.DomainModel.DocTemplateVers.Domain.DTO.Management.DTO_AddTemplate) Implements Presentation.IViewDocTemplateAdd.BindView

        If Not IsNothing(dtoAddTemplate) Then

            Me.TXBname.Text = dtoAddTemplate.Name

            Me.RBLtype.SelectedValue = dtoAddTemplate.Type

            Me.RBLtype.Items(0).Enabled = dtoAddTemplate.IsActiveDefault
            Me.RBLtype.Items(1).Enabled = dtoAddTemplate.IsActiveSystem
            Me.RBLtype.Items(2).Enabled = dtoAddTemplate.IsActiveSkin

            Me.RBLservices.SelectedValue = "0"
            Me.CBLservices.Items.Clear()


            '=--> !!! NOTA !!! <--=
            ' Le seguenti funzioni ANDRANNO OTTIMIZZATE con l'aggiunta di ulteriori servizi associabili ai DocTempalte!!!
            ' Questo vale ANCHE per UC\uc_EditBody.ascx.vb!!!

            'Lista di tutti i moduli internazionalizzati
            Dim modules As List(Of PlainService) = TranslatedModules

            'Servizio Edupath
            Dim serv As PlainService = modules.Where(Function(m) m.Code = "SRVEDUP").FirstOrDefault()
            Dim AvServices As New List(Of TemplateVers.Domain.DTO.Management.DTO_AddServices)()
            Dim srvItm As New TemplateVers.Domain.DTO.Management.DTO_AddServices()
            srvItm.ServiceCode = serv.Code
            srvItm.ServiceName = serv.Name
            srvItm.ServicesId = serv.ID

            'Controllo se l'Edupath è stato selezionato nel tempalte sorgente (preselezione)
            Dim PF As Integer = 0

            If Not IsNothing(dtoAddTemplate.Services) AndAlso dtoAddTemplate.Services.Count > 0 Then
                PF = (From itm As TemplateVers.Domain.DTO.Management.DTO_AddServices In dtoAddTemplate.Services Where (itm.ServiceCode = "SRVEDUP") Select itm.ServicesId).Count()
            End If

            If (PF > 0) Then
                srvItm.Selected = True
            End If

            AvServices.Add(srvItm)


            'Servizio Bandi (ToDo)
            serv = modules.Where(Function(m) m.Code = "SRVCFP").FirstOrDefault()

            If (CurrentPresenter.ServiceIsEnable(serv.ID)) Then
                'Dim AvServices As New List(Of TemplateVers.Domain.DTO.Management.DTO_AddServices)()
                srvItm = New TemplateVers.Domain.DTO.Management.DTO_AddServices()
                srvItm.ServiceCode = serv.Code
                srvItm.ServiceName = serv.Name
                srvItm.ServicesId = serv.ID

                'Controllo se i bandi sono stati selezionati nel tempalte sorgente (preselezione)
                Dim bnd As Integer = 0

                If Not IsNothing(dtoAddTemplate.Services) AndAlso dtoAddTemplate.Services.Count > 0 Then
                    bnd = (From itm As TemplateVers.Domain.DTO.Management.DTO_AddServices In dtoAddTemplate.Services Where (itm.ServiceCode = "SRVCFP") Select itm.ServicesId).Count()
                End If

                If (bnd > 0) Then
                    srvItm.Selected = True
                End If

                AvServices.Add(srvItm)
            End If
            

            ' Bind UL
            If Not IsNothing(AvServices) AndAlso AvServices.Count() > 0 Then
                For Each dto As TemplateVers.Domain.DTO.Management.DTO_AddServices In AvServices
                    Dim li As New ListItem
                    li.Text = dto.ServiceName
                    li.Value = dto.ServicesId
                    li.Selected = dto.Selected
                    li.Attributes.Add("class", "item")

                    If (dto.Selected) Then
                        Me.RBLservices.SelectedValue = "1"
                    End If

                    Me.CBLservices.Items.Add(li)
                Next
            Else
                Me.CBLservices.Visible = False
                Me.RBLservices.Items(1).Enabled = False
            End If

            If (Me.RBLservices.SelectedValue = "0") Then
                Me.CBLservices.Enabled = False
            Else
                Me.CBLservices.Enabled = True
            End If

        End If
    End Sub
    Public Sub GoToEdit(TemplateId As Long) Implements Presentation.IViewDocTemplateAdd.GoToEdit
        Response.Redirect("./list.aspx")
    End Sub
    Public Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements Presentation.IViewDocTemplateAdd.DisplayNoPermission
    End Sub
    Public Sub DisplaySessionTimeout() Implements Presentation.IViewDocTemplateAdd.DisplaySessionTimeout
    End Sub
    Public Sub SendUserAction(idCommunity As Integer, idModule As Integer, action As lm.Comol.Core.DomainModel.DocTemplateVers.ModuleDocTemplate.ActionType) Implements Presentation.IViewDocTemplateAdd.SendUserAction
    End Sub
    Public Sub SendUserAction(idCommunity As Integer, idModule As Integer, idTemplate As Long, action As lm.Comol.Core.DomainModel.DocTemplateVers.ModuleDocTemplate.ActionType) Implements Presentation.IViewDocTemplateAdd.SendUserAction
    End Sub
    Public Sub ShowError(AddError As lm.Comol.Core.BaseModules.DocTemplate.Presentation.AddError) Implements Presentation.IViewDocTemplateAdd.ShowError
        Select Case AddError
            Case Presentation.AddError.invalidName

            Case Presentation.AddError.genericError

        End Select
    End Sub
    Public Sub SetBackUrls(BackUrl As String, ListUrl As String) Implements Presentation.IViewDocTemplateAdd.SetBackUrls

        Me.HYPundo.Visible = False

        If Not String.IsNullOrEmpty(BackUrl) Then
            Me.HYPundo.NavigateUrl = BaseUrl & BackUrl
            Me.HYPundo.Visible = True
        ElseIf Not String.IsNullOrEmpty(ListUrl) Then
            Me.HYPundo.NavigateUrl = BaseUrl & ListUrl
            Me.HYPundo.Visible = True
        End If
    End Sub
    Public Sub GoToList(Url As String) Implements Presentation.IViewDocTemplateAdd.GoToList
        Response.Redirect(Url)
    End Sub

#End Region

#Region "Internal"
    Private Sub SetListItem(ByVal Pre As String, ByVal value As Integer, ByRef item As ListItem)
        Dim text As String = "<dl>" & vbCrLf
        text &= "<dt>" & Resource.getValue(Pre & ".Term." & value.ToString()) & "</dt>" & vbCrLf
        text &= "<dd>" & Resource.getValue(Pre & ".Definition." & value.ToString()) & "</dd>" & vbCrLf
        text &= "</dl>"

        item.Text = text
    End Sub
#End Region

#Region "Handler"
    Private Sub LKBcrea_Click(sender As Object, e As System.EventArgs) Handles LKBcrea.Click
        Me.CurrentPresenter.AddTemplate()
    End Sub
    Private Sub RBLservices_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RBLservices.SelectedIndexChanged
        If (Me.RBLservices.SelectedValue = "0") Then
            Me.CBLservices.Enabled = False
        Else
            Me.CBLservices.Enabled = True
        End If
    End Sub

#End Region

    Public ReadOnly Property TemplateBasePath As String Implements lm.Comol.Core.BaseModules.DocTemplate.Presentation.IViewDocTemplateAdd.TemplateBasePath
        Get
            Return (MyBase.SystemSettings.DocTemplateSettings.BasePath).Replace("/", "\").Replace("\\", "\")
        End Get
    End Property

    Public ReadOnly Property TemplateTempPath As String Implements lm.Comol.Core.BaseModules.DocTemplate.Presentation.IViewDocTemplateAdd.TemplateTempPath
        Get
            If Not MyBase.SystemSettings.DocTemplateSettings.BasePath.EndsWith("\") Then
                MyBase.SystemSettings.DocTemplateSettings.BasePath &= "\"
            End If

            Return (MyBase.SystemSettings.DocTemplateSettings.BasePath & TemplateTempPath & MyBase.SystemSettings.DocTemplateSettings.BaseTemporaryFolder).Replace("/", "\").Replace("\\", "\")
        End Get
    End Property
End Class