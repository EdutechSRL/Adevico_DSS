Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel.Helpers
Imports System
Imports System.Linq
Public Class RolePermissionManagement
    Inherits PageBase

    Private ReadOnly Property PreloadedIdModule() As Integer
        Get
            Dim ServiceID As Integer = -1

            If IsNumeric(Me.Request.QueryString("idModule")) Then
                ServiceID = Me.Request.QueryString("idModule")
            End If
            Return ServiceID
        End Get
    End Property
    Private ReadOnly Property PreloadedIdOrganization() As Integer
        Get
            Dim IdOrganization As Integer = -1

            If IsNumeric(Me.Request.QueryString("IdOrganization")) Then
                IdOrganization = Me.Request.QueryString("IdOrganization")
            End If
            Return IdOrganization
        End Get
    End Property
    Private ReadOnly Property PreloadedIdRole() As Integer
        Get
            Dim idRole As Integer = -1

            If IsNumeric(Me.Request.QueryString("idRole")) Then
                idRole = Me.Request.QueryString("idRole")
            End If
            Return idRole
        End Get
    End Property
    Private Property IdRole() As Integer
        Get
            Return ViewStateOrDefault("IdRole", CInt(0))
        End Get
        Set(value As Integer)
            ViewState("IdRole") = value
        End Set
    End Property
    Private Property StartupPermission() As Boolean
        Get
            Return ViewStateOrDefault("StartupPermission", False)
        End Get
        Set(value As Boolean)
            ViewState("StartupPermission") = value
        End Set
    End Property
    Private _CurrentManager As ManagerPermission
    Public ReadOnly Property CurrentManager() As ManagerPermission
        Get
            If IsNothing(_CurrentManager) Then
                _CurrentManager = New ManagerPermission(Me.PageUtility.CurrentContext, Me.SystemSettings.DBconnectionSettings.GetConnection(DBconnectionSettings.DBsetting.COMOL, ConnectionType.SQL).Name)
            End If
            Return _CurrentManager
        End Get
    End Property
    Private ReadOnly Property CurrentIdModule() As Integer
        Get
            If Me.DDLmodules.Items.Count = 0 Then
                Return -1
            Else
                Return CInt(Me.DDLmodules.SelectedValue)
            End If
        End Get
    End Property
    Private ReadOnly Property CurrentIdOrganization() As Integer
        Get
            If Me.DDLorganization.Items.Count = 0 Then
                Return -999
            Else
                Return CInt(Me.DDLorganization.SelectedValue)
            End If
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub


#Region "default"
    Public Overrides Sub BindNoPermessi()
        Me.Master.ShowNoPermission = True
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Dim PersonTypeID As Integer = Me.TipoPersonaID
        Return (PersonTypeID = Main.TipoPersonaStandard.AdminSecondario OrElse PersonTypeID = Main.TipoPersonaStandard.SysAdmin)
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()

    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
#End Region


    Public Overrides Sub BindDati()
        Me.Master.ServiceTitle = "Role Permission Management"
        Me.Master.ShowNoPermission = False
        Me.IdRole = Me.PreloadedIdRole
        Me.Bind_Organizzazioni()
        Me.BTNsave.Visible = False
        Me.BTNsaveForAll.Visible = False
        Me.BTBdefaultValue.Visible = False
        If Me.DDLorganization.Items.Count > 0 Then
            Me.Bind_Modules()
            If Me.DDLmodules.Items.Count > 0 Then
                If Not IsNothing(DDLmodules.Items.FindByValue(PreloadedIdModule)) Then
                    Me.DDLmodules.SelectedValue = PreloadedIdModule
                End If
                LoadRolePermissions(Me.DDLmodules.SelectedValue, Me.DDLorganization.SelectedValue, True)
                Me.BTNsave.Visible = True

                If Me.DDLorganization.SelectedValue < 0 Then
                    Me.BTNsaveForAll.Visible = True
                Else
                    Me.BTBdefaultValue.Visible = True
                End If
            End If
            Dim roles As List(Of Role) = COL_TipoRuolo.List(Me.LinguaID)
            If (roles.Where(Function(r) r.ID = IdRole).Any()) Then
                Me.Master.ServiceTitle = "Role """ & roles.Where(Function(r) r.ID = IdRole).Select(Function(r) r.Name).FirstOrDefault() & """ Permission Management"
            End If
            Me.DDLorganization.Enabled = (Me.DDLmodules.Items.Count > 0)
        Else
            Me.RPTroleOrganizationPermission.Visible = False
        End If
        HYPbackToManagementBottom.NavigateUrl = BaseUrl & "Admin_globale/adminG_ManagementRuoli.aspx"
        HYPbackToManagementBottom.Visible = True
    End Sub
    Private Sub Bind_Organizzazioni()
        Dim oLista As List(Of COL_Organizzazione) = COL_BusinessLogic_v2.Comunita.COL_Organizzazione.LazyElenca("", "", Session("ISTT_ID"))

        If oLista.Count > 0 Then
            Me.DDLorganization.DataTextField = "RagioneSociale"
            Me.DDLorganization.DataValueField = "Id"
            Me.DDLorganization.DataSource = oLista
            Me.DDLorganization.DataBind()
            Me.DDLorganization.Items.Insert(0, New ListItem("< Default >", -1))
            Me.DDLorganization.Enabled = True
        Else

            Me.DDLorganization.Items.Add(New ListItem("< Default >", -1))
            Me.DDLorganization.Enabled = False
        End If
    End Sub
    'Private Sub Bind_CommunityTypes()
    '    Dim oList As List(Of COL_Tipo_Comunita) = COL_BusinessLogic_v2.Comunita.COL_Tipo_Comunita.PlainLista(Me.LinguaID, True)
    '    Dim items As List(Of Integer) = CurrentManager.GetAvailableCommunityTypeByRole(Me.IdRole)
    '    If oList.Count > 0 Then
    '        Me.DDLcommunityType.DataTextField = "Descrizione"
    '        Me.DDLcommunityType.DataValueField = "ID"
    '        Me.DDLcommunityType.DataSource = (From t In oList Where items.Contains(t.ID)).ToList()
    '        Me.DDLcommunityType.DataBind()
    '        Me.DDLcommunityType.Enabled = True
    '    Else
    '        Me.DDLcommunityType.Items.Add(New ListItem("< None >", -1))
    '        Me.DDLcommunityType.Enabled = False
    '    End If
    'End Sub
    Private Sub Bind_Modules()
        Try
            Dim modules As List(Of PlainService) = ManagerService.ListSystemTranslated(PageUtility.LinguaID, True)
            Dim items As List(Of Integer) = CurrentManager.GetAvailableModulesByOrganizations(IdRole, Me.DDLorganization.SelectedValue)

            Me.DDLmodules.DataSource = modules.Where(Function(m) items.Contains(m.ID)).ToList()
            Me.DDLmodules.DataTextField = "Name"
            Me.DDLmodules.DataValueField = "Id"
            Me.DDLmodules.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub DDLorganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLorganization.SelectedIndexChanged
        Dim SelectedModuleID As Integer = Me.DDLmodules.SelectedValue
        Me.BTNsave.Visible = False
        Me.BTNsaveForAll.Visible = False
        Me.BTBdefaultValue.Visible = False
        If Me.CurrentIdModule > 0 Then
            Me.BTNsave.Visible = True
            If Me.CurrentIdOrganization < 0 Then
                Me.BTNsaveForAll.Visible = True
            Else
                Me.BTBdefaultValue.Visible = True
            End If
            If Not IsNothing(Me.DDLmodules.Items.FindByValue(SelectedModuleID)) Then
                Me.DDLmodules.SelectedValue = SelectedModuleID
            End If
            Me.LoadRolePermissions(Me.DDLmodules.SelectedValue, Me.DDLorganization.SelectedValue, True)
        End If
    End Sub
    Private Sub DDLmodules_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLmodules.SelectedIndexChanged
        Dim SelectedModuleID As Integer = Me.DDLmodules.SelectedValue

        Me.BTNsave.Visible = False
        If Me.CurrentIdModule > 0 Then
            Me.BTNsave.Visible = True
            If Me.DDLorganization.SelectedValue < 0 Then
                Me.BTNsaveForAll.Visible = True
            Else
                Me.BTBdefaultValue.Visible = True
            End If
            If Not IsNothing(Me.DDLmodules.Items.FindByValue(SelectedModuleID)) Then
                Me.DDLmodules.SelectedValue = SelectedModuleID
            End If
            Me.LoadRolePermissions(Me.DDLmodules.SelectedValue, Me.DDLorganization.SelectedValue, True)
        End If
    End Sub

    Private Sub LoadRolePermissions(ByVal idModule As Integer, ByVal idOrganization As Integer, Optional ByVal start As Boolean = False)
        StartupPermission = start
        Dim types As List(Of COL_Tipo_Comunita) = COL_BusinessLogic_v2.Comunita.COL_Tipo_Comunita.PlainLista(Me.LinguaID, True)
        Dim permissions As List(Of Permessi) = COL_Servizio.ListPermessiServizio(idModule, Me.LinguaID)
        Dim items As New List(Of dtoDisplayPermission)
        Dim dto As New dtoDisplayPermission()

        Dim rows As List(Of dtoTemplateRolePermission) = Me.CurrentManager.GetPermissionByRole(IdRole, idModule, idOrganization)
        dto.Columns = permissions

        Dim idCommunityType As Integer = -5
        Dim positions As List(Of Integer) = permissions.Select(Function(p) p.Posizione).ToList()
        For Each row As dtoTemplateRolePermission In rows
            idCommunityType = row.IdCommunityType
            row.Update(types.Where(Function(t) t.ID = idCommunityType).Select(Function(t) t.Descrizione).FirstOrDefault(), positions)
        Next
        dto.Rows = rows.OrderBy(Function(r) r.CommunityTypeName).ToList()
        items.Add(dto)
        Me.RPTroleOrganizationPermission.DataSource = items
        Me.RPTroleOrganizationPermission.DataBind()

        StartupPermission = False
    End Sub


    Private Sub Bind_PermessiRuoliServizi(ByVal ModuleID As Integer, ByVal OrganizationID As Integer, ByVal TypeID As Integer, Optional ByVal start As Boolean = False)
        Dim oServizio As New COL_Servizio
        Dim oDataset As New DataSet
        Dim oDatasetRuoli As New DataSet

        oServizio.ID = ModuleID
        Try
            Dim i, j, totalePermessi, totale, PRMS_Posizione As Integer
            Dim ARRpermServizio As Integer() = Nothing
            Dim oTBrow As New TableRow
            Dim oStringaPermessi As String
            oDataset = oServizio.ElencaPermessiAssociatiByLingua(Me.LinguaID)
            totalePermessi = oDataset.Tables(0).Rows.Count - 1

            'Caricamento riga con nome permessi !

            Dim oTableCell As New TableCell
            oTableCell.Text = "&nbsp;"
            oTableCell.BackColor = System.Drawing.Color.Lavender
            oTBrow.Cells.Add(oTableCell)
            For i = 0 To totalePermessi
                Dim oRow As DataRow
                Dim oLinkColonna As New HtmlControls.HtmlInputButton
                oTableCell = New TableCell
                oTableCell.HorizontalAlign = HorizontalAlign.Center

                oRow = oDataset.Tables(0).Rows(i)
                If IsDBNull(oRow.Item("Nome")) Then
                    Try
                        oLinkColonna.Value = oRow.Item("NomeDefault")
                    Catch ex As Exception
                        oLinkColonna.Value = "--"
                    End Try
                Else
                    oLinkColonna.Value = oRow.Item("Nome")
                End If

                oTableCell = New TableCell
                oLinkColonna.Attributes.Add("class", "Header_Repeater10")
                oLinkColonna.Attributes.Add("onclick", "SelezioneColonna('CB_" & oRow.Item("PRMS_Posizione") & "');return false;")
                oTableCell.Controls.Add(oLinkColonna)
                oTableCell.CssClass = "Header_Repeater10"
                If i Mod 2 = 0 Then
                    oTableCell.BackColor = System.Drawing.Color.White
                Else
                    oTableCell.BackColor = System.Drawing.Color.LightYellow
                End If
                oTBrow.Cells.Add(oTableCell)
                ReDim Preserve ARRpermServizio(i)
                ARRpermServizio(i) = oRow.Item("PRMS_Posizione")
            Next
            oTableCell = New TableCell
            oTBrow.Cells.Add(oTableCell)
            '   Me.TBLpermessiRuoli.Rows.Add(oTBrow)


            oDatasetRuoli = oServizio.ElencaRuoliPermessiByTipoComunita(OrganizationID, TypeID)
            totale = oDatasetRuoli.Tables(0).Rows.Count - 1

            If start Then
                Me.HIDcheckbox.Value = ""
            End If
            Dim uniqueID As String
            For i = 0 To totale
                oTableCell = New TableCell
                Dim oRow As DataRow
                Dim ElencoControlli As String = ","
                Dim TotaleSelezionati As Integer = 0
                oTBrow = New TableRow

                oRow = oDatasetRuoli.Tables(0).Rows(i)

                oStringaPermessi = oRow.Item("LPRS_valore")

                Dim oLabel As New Label
                oLabel.ID = "LB" & i & "_" & (oRow.Item("TPRL_id"))
                oLabel.Text = oRow.Item("TPRL_nome")
                oTableCell.Controls.Add(oLabel)
                oTableCell.BackColor = System.Drawing.Color.Lavender
                oTBrow.Cells.Add(oTableCell)

                For j = 0 To UBound(ARRpermServizio)
                    oTableCell = New TableCell
                    oTableCell.HorizontalAlign = HorizontalAlign.Center

                    If j Mod 2 = 0 Then
                        oTableCell.BackColor = System.Drawing.Color.White
                    Else
                        oTableCell.BackColor = System.Drawing.Color.LightYellow
                    End If
                    Dim oCheckbox As New HtmlControls.HtmlInputCheckBox
                    PRMS_Posizione = ARRpermServizio(j)
                    uniqueID = PRMS_Posizione & "_" & oRow.Item("TPRL_id")
                    oCheckbox.ID = "CB_" & uniqueID
                    oCheckbox.Value = uniqueID
                    If start Then
                        If oStringaPermessi.Substring(PRMS_Posizione, 1) = 1 Then
                            oCheckbox.Checked = True
                            TotaleSelezionati += 1
                            If Me.HIDcheckbox.Value = "" Then
                                Me.HIDcheckbox.Value = "," & uniqueID & ","
                            Else
                                Me.HIDcheckbox.Value = Me.HIDcheckbox.Value & uniqueID & ","
                            End If
                        Else
                            oCheckbox.Checked = False
                        End If
                    Else
                        If InStr(Me.HIDcheckbox.Value, "," & uniqueID & ",") > 0 Then
                            oCheckbox.Checked = True
                            TotaleSelezionati += 1
                        Else
                            oCheckbox.Checked = False
                        End If
                    End If

                    oCheckbox.Attributes.Add("onclick", "SelectFromNameAndAssocia('" & Me.ClientID & ":" & oCheckbox.ClientID & "','" & uniqueID & "');return true;")

                    oTableCell.Controls.Add(oCheckbox)

                    oTBrow.Cells.Add(oTableCell)
                    ElencoControlli &= Me.Master.ClientID & Me.IdSeparator.ToString & "CPHservice" & Me.IdSeparator.ToString & "CB_" & uniqueID & ","
                Next

                Dim oLink As New HtmlControls.HtmlInputButton
                oTableCell = New TableCell
                oLink.Value = "Sel / Desel"
                oLink.Attributes.Add("onclick", "SelezioneRiga('" & ElencoControlli & "');return false;")
                oLink.Attributes.Add("class", "Command_Repeater10")
                oTableCell.Controls.Add(oLink)
                oTBrow.Cells.Add(oTableCell)

                '        Me.TBLpermessiRuoli.Rows.Add(oTBrow)
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BTNsave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNsave.Click
        If CurrentIdModule > 0 Then
            LoadRolePermissions(CurrentIdModule, CurrentIdOrganization, False)
            '  Me.Bind_PermessiRuoliServizi(Me.DDLmodules.SelectedValue, Me.DDLorganization.SelectedValue, Me.DDLcommunityType.SelectedValue, False)
            Me.CurrentManager.SaveRoleTemplate(IdRole, CurrentIdOrganization, CurrentIdModule, Me.GetRolePermission)
        End If
    End Sub


    Private Sub BTBdefaultValue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTBdefaultValue.Click
        If CurrentIdModule > 0 Then
            LoadRolePermissions(CurrentIdModule, -1, True)
            Me.CurrentManager.SaveRoleTemplate(IdRole, CurrentIdOrganization, CurrentIdModule, Me.GetRolePermission)
            '        Dim oTipoComunita As New COL_Tipo_Comunita
            '        oTipoComunita.ID = Me.DDLcommunityType.SelectedValue
            '        oTipoComunita.DefinisciPermessiRuoliDefault(Me.DDLorganization.SelectedValue, Me.DDLmodules.SelectedValue)
            '        Me.TBLpermessiRuoli.Rows.Clear()
            '        Me.Bind_PermessiRuoliServizi(Me.DDLmodules.SelectedValue, Me.DDLorganization.SelectedValue, Me.DDLcommunityType.SelectedValue, True)

        End If

    End Sub
    Private Sub BTNsaveForAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNsaveForAll.Click
        If CurrentIdModule > 0 Then
            LoadRolePermissions(CurrentIdModule, CurrentIdOrganization, False)
            Me.CurrentManager.ApplyRoleTemplateToAll(IdRole, CurrentIdOrganization, CurrentIdModule, Me.GetRolePermission)
        End If
    End Sub
    Private Sub BTNreplaceCommunityValues_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNreplaceCommunityValues.Click
        '  Me.Bind_PermessiRuoliServizi(Me.DDLmodules.SelectedValue, Me.DDLorganization.SelectedValue, Me.DDLcommunityType.SelectedValue, False)

        If CurrentIdModule > 0 AndAlso CurrentIdOrganization >= -1 Then
            LoadRolePermissions(CurrentIdModule, CurrentIdOrganization, False)
            Me.CurrentManager.ApplyToCommunitiesByRole(IdRole, CurrentIdOrganization, CurrentIdModule, Me.GetRolePermission())

            CacheHelper.PurgeCacheItems(lm.Comol.Modules.Standard.Menu.Domain.CacheKeys.RenderAllCommunity)
            CacheHelper.PurgeCacheItems(CachePolicy.Permessi)
            CacheHelper.PurgeCacheItems(CachePolicy.PermessiServizio(Me.DDLmodules.SelectedValue))
            Dim Code As String = Me.CurrentManager.GetModuleCode(CurrentIdModule)
            If Code <> "" Then
                CacheHelper.PurgeCacheItems(CachePolicy.PermessiServizioUtente(Code))
            End If

            '   CacheHelper.PurgeCacheItems(CachePolicyCommunity.)
            'ObjectBase.PurgeCacheItems(CachePolicy.MenuComunita)
        End If

    End Sub

    Protected Sub RPTtypes_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
            Dim dto As dtoTemplateRolePermission = e.Item.DataItem

            Dim controls As String = ","
            For Each permission As dtoPermission In dto.Positions
                controls &= "CB_" & permission.IdPosition & "_" & permission.IdOwner & ","
            Next

            Dim oButton As Button = e.Item.FindControl("BTNsetAll")
            oButton.OnClientClick = "SelezioneRiga('" & controls & "');return false;"
        End If
    End Sub
    Protected Sub RPTpermissionName_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
            Dim dto As Permessi = e.Item.DataItem
            Dim oButton As Button = e.Item.FindControl("BTNpermission")
            oButton.OnClientClick = "SelezioneColonna('CB_" & dto.Posizione & "');return false;"

        End If
    End Sub
    Protected Sub RPTpermissionValue_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.AlternatingItem OrElse e.Item.ItemType = ListItemType.Item Then
            Dim dto As dtoPermission = e.Item.DataItem
            Dim place As PlaceHolder = e.Item.FindControl("PLHpermission")
            Dim oCheckbox As New HtmlInputCheckBox
            Dim idCheck As String = "CB_" & dto.IdPosition & "_" & dto.IdOwner
            oCheckbox.ID = idCheck
            oCheckbox.Checked = dto.Selected
            oCheckbox.Value = idCheck
            If StartupPermission AndAlso dto.Selected Then
                If String.IsNullOrEmpty(Me.HIDcheckbox.Value) Then
                    Me.HIDcheckbox.Value = "," & idCheck & ","
                Else
                    Me.HIDcheckbox.Value = Me.HIDcheckbox.Value & idCheck & ","
                End If
            ElseIf Not StartupPermission Then
                If Not String.IsNullOrEmpty(Me.HIDcheckbox.Value) AndAlso Me.HIDcheckbox.Value.Contains("," & idCheck & ",") Then
                    oCheckbox.Checked = True
                Else
                    oCheckbox.Checked = False
                End If
            End If
            oCheckbox.Attributes.Add("onclick", "SelectFromNameAndAssocia('" & idCheck & "');return true;")
            place.Controls.Add(oCheckbox)
        End If
    End Sub
    Private Function GetRolePermission() As List(Of dtoTemplateRolePermission)
        Dim items As New List(Of dtoTemplateRolePermission)


        If Not Me.DDLmodules.Items.Count = 0 Then
            Dim container As Repeater = RPTroleOrganizationPermission.Items(0).FindControl("RPTtypes")

            For Each row As RepeaterItem In container.Items
                Dim idCommunityType As Integer = CInt(DirectCast(row.FindControl("LTidCommunityType"), Literal).Text)
                Dim item As New dtoTemplateRolePermission
                item.IdCommunityType = idCommunityType

                Dim repeater As Repeater = row.FindControl("RPTpermissionValue")
                Dim permission As Long = 0
                For Each subRow As RepeaterItem In repeater.Items
                    Dim place As PlaceHolder = subRow.FindControl("PLHpermission")

                    Dim oCheckbox As HtmlControls.HtmlInputCheckBox = place.Controls(0)
                    If oCheckbox.Checked Then
                        permission = permission Or (2 ^ CInt(DirectCast(subRow.FindControl("LTposition"), Literal).Text))
                    End If
                Next
                item.Permission = permission
                items.Add(item)
            Next
        End If
        Return items
    End Function

    Protected Function GetBackground(ByVal type As ListItemType) As String
        If type = ListItemType.Item Then
            Return "ColumnItem"
        Else
            Return "ColumnAlternateItem"

        End If
    End Function

End Class

<Serializable()> Public Class dtoDisplayPermission
    Public Columns As List(Of Permessi)
    Public Rows As List(Of dtoTemplateRolePermission)
    Public Sub New()
        Columns = New List(Of Permessi)
        Rows = New List(Of dtoTemplateRolePermission)
    End Sub


End Class