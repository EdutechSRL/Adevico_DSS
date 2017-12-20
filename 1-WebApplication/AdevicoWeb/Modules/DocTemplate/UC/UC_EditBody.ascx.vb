Imports lm.Comol.Core.BaseModules.DocTemplate
Imports TemplateVers = lm.Comol.Core.DomainModel.DocTemplateVers
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.ActionDataContract
Imports lm.Comol.Core.BaseModules.DocTemplate.Presentation
Imports lm.Comol.Core.DomainModel.DocTemplate

Public Class UC_EditBody
    Inherits BaseControl

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
        End Get
    End Property
#End Region

#Region "Internal"
    Public Event DeletePrevBody(ByVal Id As Int64)
    Public Event DeletePrevBodies(ByVal Ids As IList(Of Int64))
    Public Event RecoverPrevbody(ByVal Id As Int64)

    Public WriteOnly Property Code As String
        Set(value As String)
            LBLcode.Text = value
        End Set
    End Property
    Public Property CurrentText As String
        Get
            Return Me.CTRLvisualEditorText.HTML
        End Get
        Set(value As String)
            Me.CTRLvisualEditorText.HTML = value
        End Set
    End Property
    Public Property IsHtml As Boolean
        Get
            Return True
        End Get
        Set(value As Boolean)
        End Set
    End Property
    Public ReadOnly Property EditorClientId As String
        Get
            Return Me.CTRLvisualEditorText.EditorClientId
        End Get
    End Property
    Public ReadOnly Property TagOnKeyUpScript As String
        Get
            Return HTMLonKeyUpScript
        End Get
    End Property
    Public Property HTMLonKeyUpScript As String
        Get
            Return ViewStateOrDefault("HTMLonKeyUpScript", "")
        End Get
        Set(value As String)
            Me.ViewState("HTMLonKeyUpScript") = value
        End Set
    End Property
    Public WriteOnly Property TelerikOnClientCommandExecuted As String
        Set(value As String)
            Me.CTRLvisualEditorText.OnClientCommandExecuted = value
        End Set
    End Property
    Public WriteOnly Property TelerikClientLoadScript As String
        Set(value As String)
            Me.CTRLvisualEditorText.OnClientLoadScript = value
        End Set
    End Property
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

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_templateEdit", "Modules", "DocTemplates")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        Me.Resource.setLabel(Me.LBLbodyText_t)
        Me.Resource.setLiteral(Me.LITtag_t)
        Me.Resource.setLiteral(Me.LITrevision_t)
        Me.Resource.setLabel(LBLcode_t)
    End Sub
#End Region

#Region "Internal"
    Public Sub LoadAvailableService(usedItems As IList(Of TemplateVers.Domain.DTO.Management.DTO_EditItem(Of TemplateVers.ServiceContent)))

        Dim modules As List(Of PlainService) = TranslatedModules

        Dim items As New List(Of dtoModulePlaceHolder)
        Dim common As New dtoModulePlaceHolder
        common.IdModule = 0
        common.ModuleCode = ""
        common.Name = Resource.getValue("Service.Common")
        common.Attributes = (From p In lm.Comol.Core.DomainModel.Helpers.TemplateCommonPlaceHolders.PlaceHolders()
                             Select New TranslatedItem(Of String) With {
                                 .Id = lm.Comol.Core.DomainModel.Helpers.TemplateCommonPlaceHolders.GetPlaceHolder(p.Key),
                                 .Translation = p.Key.ToString()}).ToList()

            '=--> !!! NOTA !!! <--=
            ' Le seguenti funzioni ANDRANNO OTTIMIZZATE con l'aggiunta di ulteriori servizi associabili ai DocTempalte!!!
            ' Questo vale ANCHE per UC\uc_EditBody.ascx.vb!!!

            items.Add(common)
            Dim PF As Integer = 0
            If (Not IsNothing(usedItems) AndAlso usedItems.Count() > 0) Then
                Try
                    PF = (From itm In usedItems Where (itm.Data.ModuleCode = "SRVEDUP") Select itm).Count()
                Catch ex As Exception

                End Try
            End If


            If (PF > 0) Then
                ' AGGIUNGO PERCORSO FORMATIVO 
                Dim modulePlaceHolders As dtoModulePlaceHolder
                Dim serv As PlainService = modules.Where(Function(m) m.Code = "SRVEDUP").FirstOrDefault()
                If Not IsNothing(serv) Then
                    modulePlaceHolders = New dtoModulePlaceHolder
                    modulePlaceHolders.IdModule = serv.ID
                    modulePlaceHolders.Name = serv.Name
                    modulePlaceHolders.ModuleCode = serv.Code
                    modulePlaceHolders.Attributes = (From p In lm.Comol.Modules.EduPath.TemplateEduPathPlaceHolders.PlaceHolders() Select New TranslatedItem(Of String) With
                        {
                            .Id = lm.Comol.Modules.EduPath.TemplateEduPathPlaceHolders.GetPlaceHolder(p.Key),
                            .Translation = p.Key.ToString()
                        }
                        ).ToList()
                    items.Add(modulePlaceHolders)
                End If
            End If

            '=--> !!! NOTA !!! <--=
            ' Le seguenti funzioni ANDRANNO OTTIMIZZATE con l'aggiunta di ulteriori servizi associabili ai DocTempalte!!!
            ' Questo vale ANCHE per UC\uc_EditBody.ascx.vb!!!

            Dim CfP As Integer = 0
            If (Not IsNothing(usedItems) AndAlso usedItems.Count() > 0) Then
                Try
                    CfP = (From itm In usedItems Where (itm.Data.ModuleCode = "SRVCFP") Select itm).Count()
                Catch ex As Exception

                End Try
            End If


            If (CfP > 0) Then
                ' AGGIUNGO Bandi
                Dim modulePlaceHolders As dtoModulePlaceHolder
                Dim serv As PlainService = modules.Where(Function(m) m.Code = "SRVCFP").FirstOrDefault()
                If Not IsNothing(serv) Then
                    modulePlaceHolders = New dtoModulePlaceHolder
                    modulePlaceHolders.IdModule = serv.ID
                    modulePlaceHolders.Name = serv.Name
                    modulePlaceHolders.ModuleCode = serv.Code
                    modulePlaceHolders.Attributes =
                        (From p In lm.Comol.Modules.CallForPapers.Business.TemplateCallForPeaperPlaceHolders.PlaceHolders()
                            Select New TranslatedItem(Of String) With
                            {.Id = lm.Comol.Modules.CallForPapers.Business.TemplateCallForPeaperPlaceHolders.GetPlaceHolder(p.Key),
                                .Translation = p.Key.ToString()}).ToList()
                    items.Add(modulePlaceHolders)
                End If
            End If

            Me.RPTmodules.DataSource = items
            Me.RPTmodules.DataBind()

    End Sub
    Public Function GetCurrentModules() As System.Collections.Generic.List(Of TemplateVers.ModuleContent)
        Return Nothing
    End Function
    Public Sub BindPrevVersion(ByVal PrevVersions As IList(Of TemplateVers.Domain.DTO.Management.DTO_EditPreviousVersion))

        If Not IsNothing(PrevVersions) AndAlso PrevVersions.Count > 0 Then
            Me.UCprevVersion.BindList(PrevVersions)
            Me.PNLsubVersion.Visible = True
        Else
            Me.PNLsubVersion.Visible = False
        End If

    End Sub
#End Region

#Region "Handler"
    Private Sub UCprevVersion_DeleteItem(Id As Long) Handles UCprevVersion.DeleteItem
        RaiseEvent DeletePrevBody(Id)
    End Sub
    Private Sub UCprevVersion_DeleteItems(Ids As System.Collections.Generic.IList(Of Long)) Handles UCprevVersion.DeleteItems
        RaiseEvent DeletePrevBodies(Ids)
    End Sub
    Private Sub UCprevVersion_RecoverItem(Id As Long) Handles UCprevVersion.RecoverItem
        RaiseEvent RecoverPrevbody(Id)
    End Sub
    Public Sub RPTplaceHolder_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
            Dim item As TranslatedItem(Of String) = DirectCast(e.Item.DataItem, TranslatedItem(Of String))
            Dim oButton As Button = e.Item.FindControl("BTNattribute")

            oButton.Text = item.Translation
            oButton.CommandArgument = item.Id.ToString
            oButton.CssClass = "addTextTelerik"
            'If MandatoryAttributes.Select(Function(a) a.Id).Contains(item.Id) Then
            '    oButton.CssClass &= " required"
            'End If

            oButton.Attributes.Add("rel", item.Id.ToString)
        End If
    End Sub
#End Region

End Class