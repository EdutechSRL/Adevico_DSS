Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Modules.Base.BusinessLogic
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Core.DomainModel.Helpers

Partial Public Class PortalPermissionManagement
    Inherits PageBase


    'Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext

    'Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
    '    Get
    '        If IsNothing(_CurrentContext) Then
    '            _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
    '        End If
    '        Return _CurrentContext
    '    End Get
    'End Property

    Private ReadOnly Property PreloadedServiceID() As Integer
        Get
            Dim ServiceID As Integer = -1

            If IsNumeric(Me.Request.QueryString("IdModule")) Then
                ServiceID = Me.Request.QueryString("IdModule")
            End If
            Return ServiceID
        End Get
    End Property
    Private ReadOnly Property PreloadedCommunityTypeID() As Integer
        Get
            Dim CommunityTypeID As Integer = -1

            If IsNumeric(Me.Request.QueryString("CommunityTypeID")) Then
                CommunityTypeID = Me.Request.QueryString("CommunityTypeID")
            End If
            Return CommunityTypeID
        End Get
    End Property
    Private ReadOnly Property PreloadedOrganizationID() As Integer
        Get
            Dim OrganizationID As Integer = -1

            If IsNumeric(Me.Request.QueryString("OrganizationID")) Then
                OrganizationID = Me.Request.QueryString("OrganizationID")
            End If
            Return OrganizationID
        End Get
    End Property

    Private _CurrentManager As ManagerPermission
    Public ReadOnly Property CurrentManager() As ManagerPermission
        Get
            If IsNothing(_CurrentManager) Then
                _CurrentManager = New ManagerPermission(Me.CurrentContext, Me.SystemSettings.DBconnectionSettings.GetConnection(DBconnectionSettings.DBsetting.COMOL, ConnectionType.SQL).Name)
            End If
            Return _CurrentManager
        End Get
    End Property
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
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
        Me.Master.ServiceTitle = "Advanced Permission Management"
        Me.Master.ShowNoPermission = False
        Me.Bind_CommunityTypes()
        Me.BTNsave.Visible = False
        Me.BTNsaveForAll.Visible = False
        Me.BTBdefaultValue.Visible = False
        If Me.DDLcommunityType.Items.Count > 0 Then
            Me.DDLcommunityType.SelectedValue = Me.PreloadedCommunityTypeID
            Me.Bind_Organizzazioni()
            If Me.DDLorganization.Items.Count > 0 Then
                Me.DDLorganization.SelectedValue = Me.PreloadedOrganizationID

                Me.Bind_Modules()
                If Me.DDLmodules.Items.Count > 0 Then
                    Me.Bind_PermessiRuoliServizi(Me.DDLmodules.SelectedValue, Me.DDLorganization.SelectedValue, Me.DDLcommunityType.SelectedValue, True)
                    Me.BTNsave.Visible = True

                    If Me.DDLorganization.SelectedValue < 0 Then
                        Me.BTNsaveForAll.Visible = True
                    Else
                        Me.BTBdefaultValue.Visible = True
                    End If
                End If
            Else
                Me.TBLpermessiRuoli.Rows.Clear()
            End If
        Else
            Me.TBLpermessiRuoli.Rows.Clear()
        End If
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
    Private Sub Bind_CommunityTypes()
        Dim oList As List(Of COL_Tipo_Comunita) = COL_BusinessLogic_v2.Comunita.COL_Tipo_Comunita.PlainLista(Me.LinguaID, True)

        If oList.Count > 0 Then
            Me.DDLcommunityType.DataTextField = "Descrizione"
            Me.DDLcommunityType.DataValueField = "ID"
            Me.DDLcommunityType.DataSource = oList
            Me.DDLcommunityType.DataBind()
            Me.DDLcommunityType.Enabled = True
        Else
            Me.DDLcommunityType.Items.Add(New ListItem("< None >", -1))
            Me.DDLcommunityType.Enabled = False
        End If
    End Sub
    Private Sub Bind_Modules()
        Dim oTipoComunita As New COL_Tipo_Comunita
        Dim oDataset As DataSet

        Try
            oTipoComunita.ID = Me.DDLcommunityType.SelectedValue
            Me.DDLmodules.DataSource = oTipoComunita.OrganizationServices(Me.DDLorganization.SelectedValue, Me.LinguaID)
            Me.DDLmodules.DataTextField = "Nome"
            Me.DDLmodules.DataValueField = "Id"
            Me.DDLmodules.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub DDLcommunityType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLcommunityType.SelectedIndexChanged
        Dim SelectedModuleID As Integer = Me.DDLmodules.SelectedValue
        Me.Bind_Modules()

        Me.BTNsave.Visible = False
        Me.BTNsaveForAll.Visible = False
        Me.BTBdefaultValue.Visible = False
        If Me.DDLmodules.Items.Count > 0 Then
            Me.BTNsave.Visible = True
            If Me.DDLorganization.SelectedValue < 0 Then
                Me.BTNsaveForAll.Visible = True
            Else
                Me.BTBdefaultValue.Visible = True
            End If
            If Not IsNothing(Me.DDLmodules.Items.FindByValue(SelectedModuleID)) Then
                Me.DDLmodules.SelectedValue = SelectedModuleID
            End If
            Me.Bind_PermessiRuoliServizi(Me.DDLmodules.SelectedValue, Me.DDLorganization.SelectedValue, Me.DDLcommunityType.SelectedValue, True)
        End If
    End Sub

    Private Sub DDLorganization_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLorganization.SelectedIndexChanged
        Dim SelectedModuleID As Integer = Me.DDLmodules.SelectedValue
        Me.BTNsave.Visible = False
        Me.BTNsaveForAll.Visible = False
        Me.BTBdefaultValue.Visible = False
        If Me.DDLmodules.Items.Count > 0 Then
            Me.BTNsave.Visible = True
            If Me.DDLorganization.SelectedValue < 0 Then
                Me.BTNsaveForAll.Visible = True
            Else
                Me.BTBdefaultValue.Visible = True
            End If
            If Not IsNothing(Me.DDLmodules.Items.FindByValue(SelectedModuleID)) Then
                Me.DDLmodules.SelectedValue = SelectedModuleID
            End If
            Me.Bind_PermessiRuoliServizi(Me.DDLmodules.SelectedValue, Me.DDLorganization.SelectedValue, Me.DDLcommunityType.SelectedValue, True)
        End If
    End Sub

    Private Sub DDLmodules_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLmodules.SelectedIndexChanged
        Dim SelectedModuleID As Integer = Me.DDLmodules.SelectedValue

        Me.BTNsave.Visible = False
        Me.BTNsaveForAll.Visible = False
        Me.BTBdefaultValue.Visible = False
        If Me.DDLmodules.Items.Count > 0 Then
            Me.BTNsave.Visible = True
            If Me.DDLorganization.SelectedValue < 0 Then
                Me.BTNsaveForAll.Visible = True
            Else
                Me.BTBdefaultValue.Visible = True
            End If
            If Not IsNothing(Me.DDLmodules.Items.FindByValue(SelectedModuleID)) Then
                Me.DDLmodules.SelectedValue = SelectedModuleID
            End If
            Me.Bind_PermessiRuoliServizi(Me.DDLmodules.SelectedValue, Me.DDLorganization.SelectedValue, Me.DDLcommunityType.SelectedValue, True)
        End If
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
            Me.TBLpermessiRuoli.Rows.Add(oTBrow)


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

                Me.TBLpermessiRuoli.Rows.Add(oTBrow)
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BTNsave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNsave.Click
        If Me.DDLmodules.Items.Count > 0 Then
            Me.Bind_PermessiRuoliServizi(Me.DDLmodules.SelectedValue, Me.DDLorganization.SelectedValue, Me.DDLcommunityType.SelectedValue, False)
            Me.CurrentManager.SaveTemplate(Me.DDLcommunityType.SelectedValue, Me.DDLorganization.SelectedValue, Me.DDLmodules.SelectedValue, Me.GetRolePermission)
        End If
    End Sub

    Private Sub BTNsaveForAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNsaveForAll.Click
        If Me.DDLmodules.Items.Count > 0 Then
            Me.Bind_PermessiRuoliServizi(Me.DDLmodules.SelectedValue, Me.DDLorganization.SelectedValue, Me.DDLcommunityType.SelectedValue, False)
            Me.CurrentManager.ApplyTemplateToAll(Me.DDLcommunityType.SelectedValue, Me.DDLorganization.SelectedValue, Me.DDLmodules.SelectedValue, Me.GetRolePermission)
        End If
    End Sub


    Private Sub AssociaPermessiRuoli(ByVal Replica As Boolean)
        Dim oTipoComunita As New COL_Tipo_Comunita
        oTipoComunita.ID = Me.DDLcommunityType.SelectedValue


        If Not Me.DDLmodules.Items.Count = 0 Then
            Try
                Dim i, j, TPRL_ID, totaleV, totaleO, Posizione, Associati, Totali As Integer
                Dim oStringaPermessi, nome() As String
                Dim Permessi() As Char
                Dim oTableCell As TableCell

                totaleV = Me.TBLpermessiRuoli.Rows.Count - 1
                totaleO = Me.TBLpermessiRuoli.Rows(0).Cells.Count - 2
                TPRL_ID = 0
                For i = 1 To totaleV
                    oStringaPermessi = "00000000000000000000000000000000"
                    Permessi = oStringaPermessi.ToCharArray
                    For j = 1 To totaleO
                        Dim oCheckbox As New HtmlControls.HtmlInputCheckBox
                        oTableCell = Me.TBLpermessiRuoli.Rows(i).Cells(j)
                        oCheckbox = oTableCell.Controls(0)
                        nome = oCheckbox.ID.Split("_")
                        TPRL_ID = nome(2)
                        Posizione = nome(1)

                        If oCheckbox.Checked Then

                            oStringaPermessi = oStringaPermessi.Remove(Posizione, 1)
                            If Posizione > 0 Then
                                oStringaPermessi = oStringaPermessi.Insert(Posizione, 1)
                            Else
                                oStringaPermessi = oStringaPermessi.Insert(0, 1)
                            End If
                        Else
                            oStringaPermessi = oStringaPermessi.Remove(Posizione, 1)
                            If Posizione > 0 Then
                                oStringaPermessi = oStringaPermessi.Insert(Posizione, 0)
                            Else
                                oStringaPermessi = oStringaPermessi.Insert(0, 0)
                            End If
                        End If
                    Next
                    If TPRL_ID <> 0 Then
                        oTipoComunita.DefinisciPermessiRuoli(Me.DDLorganization.SelectedValue, Me.DDLmodules.SelectedValue, TPRL_ID, oStringaPermessi, Replica)
                        If oTipoComunita.ErroreDB = Errori_Db.None Then
                            Associati += 1
                        End If
                        Totali += 1
                    End If
                Next
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub BTBdefaultValue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTBdefaultValue.Click
        If Me.DDLmodules.Items.Count > 0 Then
            Dim oTipoComunita As New COL_Tipo_Comunita
            oTipoComunita.ID = Me.DDLcommunityType.SelectedValue
            oTipoComunita.DefinisciPermessiRuoliDefault(Me.DDLorganization.SelectedValue, Me.DDLmodules.SelectedValue)
            Me.TBLpermessiRuoli.Rows.Clear()
            Me.Bind_PermessiRuoliServizi(Me.DDLmodules.SelectedValue, Me.DDLorganization.SelectedValue, Me.DDLcommunityType.SelectedValue, True)
        End If

    End Sub

    Private Sub BTNreplaceCommunityValues_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNreplaceCommunityValues.Click
        Me.Bind_PermessiRuoliServizi(Me.DDLmodules.SelectedValue, Me.DDLorganization.SelectedValue, Me.DDLcommunityType.SelectedValue, False)

        If Not Me.DDLmodules.Items.Count = 0 AndAlso Not Me.DDLcommunityType.Items.Count = 0 Then
            Dim CommunityTypeID As Integer = Me.DDLcommunityType.SelectedValue
            Dim ModuleID As Integer = Me.DDLmodules.SelectedValue
            Me.CurrentManager.ApplyToCommunities(CommunityTypeID, Me.DDLorganization.SelectedValue, ModuleID, Me.GetRolePermission())

            CacheHelper.PurgeCacheItems(lm.Comol.Modules.Standard.Menu.Domain.CacheKeys.RenderAllCommunity)
            CacheHelper.PurgeCacheItems(CachePolicy.Permessi)
            CacheHelper.PurgeCacheItems(CachePolicy.PermessiServizio(Me.DDLmodules.SelectedValue))
            Dim Code As String = Me.CurrentManager.GetModuleCode(ModuleID)
            If Code <> "" Then
                CacheHelper.PurgeCacheItems(CachePolicy.PermessiServizioUtente(Code))
            End If




            '   CacheHelper.PurgeCacheItems(CachePolicyCommunity.)
            'ObjectBase.PurgeCacheItems(CachePolicy.MenuComunita)
        End If

    End Sub


    Private Function GetRolePermission() As List(Of dtoTemplateModulePermission)
        Dim oList As New List(Of dtoTemplateModulePermission)


        If Not Me.DDLmodules.Items.Count = 0 Then
            Dim Permissions As Long = 0

            Try
                Dim i, j, RoleCount, PermissionCount, Posizione, Associati, Totali As Integer
                Dim oTableCell As TableCell

                RoleCount = Me.TBLpermessiRuoli.Rows.Count - 1
                PermissionCount = Me.TBLpermessiRuoli.Rows(0).Cells.Count - 2
                Dim RoleID As Integer = 0
                Dim IDtoSplit As String()
                For i = 1 To RoleCount
                    Permissions = 0
                    For j = 1 To PermissionCount
                        Dim oCheckbox As New HtmlControls.HtmlInputCheckBox
                        oTableCell = Me.TBLpermessiRuoli.Rows(i).Cells(j)
                        oCheckbox = oTableCell.Controls(0)
                        IDtoSplit = oCheckbox.ID.Split("_")
                        RoleID = IDtoSplit(2)
                        Posizione = IDtoSplit(1)

                        If oCheckbox.Checked Then
                            Permissions = Permissions Or (2 ^ Posizione)
                        End If
                    Next
                    If RoleID <> 0 Then
                        oList.Add(New dtoTemplateModulePermission With {.RoleId = RoleID, .Permission = Permissions})
                    End If
                    RoleID = 0
                Next
            Catch ex As Exception

            End Try
        End If
        Return oList
    End Function
End Class