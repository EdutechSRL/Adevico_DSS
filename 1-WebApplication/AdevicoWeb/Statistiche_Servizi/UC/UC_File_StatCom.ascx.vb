Imports lm.Comol.Core.Statistiche

Partial Public Class UC_File_StatCom
    Inherits BaseControlSession
    Implements I_Uc_FileCom

    Private _CommunityID As Integer
    Private _Presenter As lm.Comol.Core.Statistiche.Presenter_Uc_FileCom
    Public Property Presenter() As lm.Comol.Core.Statistiche.Presenter_Uc_FileCom
        Get
            If IsNothing(Me._Presenter) Then
                Me._Presenter = New lm.Comol.Core.Statistiche.Presenter_Uc_FileCom(Me)
            End If
            Return Me._Presenter
        End Get
        Set(ByVal value As lm.Comol.Core.Statistiche.Presenter_Uc_FileCom)
            Me._Presenter = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub Bind(ByVal Community_Id As Integer) Implements lm.Comol.Core.Statistiche.IcommunityContainer.Bind
        Me._CommunityID = Community_Id
        Me.PGgrid_UtentiCom.Pager = Me.Pager

        If IsNothing(Me.Pager) Then
            'Me.PGgrid_UtentiCom.Pager = Me.Pager
            Me.Presenter.BindUserList(Me.CommunityID, Me.DDLtipoRicerca.SelectedValue, Me.TXBvalore.Text)
        End If


    End Sub

    Public Property CommunityID() As Integer Implements lm.Comol.Core.Statistiche.IcommunityContainer.CommunityID
        Get
            Return Me._CommunityID
        End Get
        Set(ByVal value As Integer)
            Me._CommunityID = value
        End Set
    End Property

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()

    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub

#Region "Lista Utenti"

    Public Sub BindListUtenti(ByVal oList As System.Collections.Generic.List(Of lm.Comol.Core.Statistiche.StatFileUsr)) Implements lm.Comol.Core.Statistiche.I_Uc_FileCom.BindListUtenti
        Me.DG_UtentiCom.DataSource = oList
        Me.DG_UtentiCom.DataBind()
    End Sub

    Private Sub DG_UtentiCom_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DG_UtentiCom.ItemCommand
        Select Case e.CommandName
            Case "GotoUser"
                Dim UserID As Integer
                Try
                    UserID = e.CommandArgument
                Catch ex As Exception
                End Try
                'Me.Presenter........
        End Select
    End Sub

    Private Sub DG_UtentiCom_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DG_UtentiCom.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            'INTERNAZIONALIZZAZIONE...
            'LBL_DG_Cognome_h
            'LBL_DG_Ruolo_h
            'LBL_DG_TotDown_h
            'LBL_DG_KbDown_h
            'LBL_DG_LastDown_h
            'LBL_DG_Nome_h
        ElseIf e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim LKB_DG_Nome, LKB_DG_Cognome As LinkButton
            Dim LBL_DG_Nome, LBL_DG_Cognome, LBL_DG_Ruolo, LBL_DG_TotDown, LBL_DG_KbDown, LBL_DG_LastDown As Label

            Dim oFUStat As lm.Comol.Core.Statistiche.StatFileUsr
            Try
                oFUStat = e.Item.DataItem
            Catch ex As Exception
                Exit Sub
            End Try

            Try
                LBL_DG_Nome = e.Item.Cells(1).FindControl("LBL_DG_Nome")
            Catch ex As Exception
            End Try
            If Not IsNothing(LBL_DG_Nome) Then
                LBL_DG_Nome.Text = oFUStat.Nome
            End If

            Try
                LBL_DG_Cognome = e.Item.Cells(1).FindControl("LBL_DG_Cognome")
            Catch ex As Exception
            End Try
            If Not IsNothing(LBL_DG_Cognome) Then
                LBL_DG_Cognome.Text = oFUStat.Cognome
            End If

            Try
                LBL_DG_Ruolo = e.Item.Cells(1).FindControl("LBL_DG_Ruolo")
            Catch ex As Exception
            End Try
            If Not IsNothing(LBL_DG_Ruolo) Then
                LBL_DG_Ruolo.Text = oFUStat.Ruolo
            End If

            Try
                LBL_DG_TotDown = e.Item.Cells(1).FindControl("LBL_DG_TotDown")
            Catch ex As Exception
            End Try
            If Not IsNothing(LBL_DG_TotDown) Then
                LBL_DG_TotDown.Text = oFUStat.NumDown.ToString
            End If

            Try
                LBL_DG_KbDown = e.Item.Cells(1).FindControl("LBL_DG_KbDown")
            Catch ex As Exception
            End Try
            If Not IsNothing(LBL_DG_KbDown) Then
                LBL_DG_KbDown.Text = oFUStat.KbDown.ToString
            End If

            Try
                LBL_DG_LastDown = e.Item.Cells(1).FindControl("LBL_DG_LastDown")
            Catch ex As Exception
            End Try
            If Not IsNothing(LBL_DG_LastDown) Then
                If oFUStat.LastDown = New DateTime() Then
                    LBL_DG_LastDown.Text = ""
                Else
                    LBL_DG_LastDown.Text = oFUStat.LastDown.ToString
                End If
            End If

            Try
                LKB_DG_Nome = e.Item.Cells(1).FindControl("LKB_DG_Nome")
            Catch ex As Exception
            End Try
            If Not IsNothing(LKB_DG_Nome) Then
                With LKB_DG_Nome
                    .CommandName = "GotoUser"
                    .CommandArgument = oFUStat.Id
                    .Enabled = True
                End With
            End If

            Try
                LKB_DG_Cognome = e.Item.Cells(1).FindControl("LKB_DG_Cognome")
            Catch ex As Exception
            End Try
            If Not IsNothing(LKB_DG_Cognome) Then
                With LKB_DG_Cognome
                    .CommandName = "GotoUser"
                    .CommandArgument = oFUStat.Id
                    .Enabled = True
                End With
            End If
        End If

    End Sub

    Private Sub LNBchiudiFiltro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBchiudiFiltro.Click
        Me.TBRapriFiltro.Visible = True
        Me.TBRchiudiFiltro.Visible = False
        Me.TBRfiltri.Visible = False
    End Sub
    Private Sub LNBapriFiltro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBapriFiltro.Click
        Me.TBRapriFiltro.Visible = False
        Me.TBRchiudiFiltro.Visible = True
        Me.TBRfiltri.Visible = True
    End Sub
#End Region

    Public Property OrderDirection() As Boolean Implements lm.Comol.Core.Statistiche.I_Uc_FileCom.OrderDirection
        Get
            If IsNothing(Me.ViewState("OrderDir")) Then
                Me.ViewState("OrderDir") = True
            End If
            Return Me.ViewState("OrderDir")
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("OrderDir") = value
        End Set
    End Property

    Public Property OrderField() As lm.Comol.Core.Statistiche.Presenter_Uc_FileCom.OrderFields Implements lm.Comol.Core.Statistiche.I_Uc_FileCom.OrderField
        Get
            If IsNothing(Me.ViewState("OrderField")) Then
                Me.ViewState("OrderField") = lm.Comol.Core.Statistiche.Presenter_Uc_FileCom.OrderFields.Name
            End If
            Return Me.ViewState("OrderField")
        End Get
        Set(ByVal value As lm.Comol.Core.Statistiche.Presenter_Uc_FileCom.OrderFields)
            Me.ViewState("OrderField") = value
        End Set
    End Property

    Public Property PageSize() As Integer Implements lm.Comol.Core.Statistiche.I_Uc_FileCom.PageSize
        Get
            Return CInt(Me.DDLNumeroRecord.SelectedValue)
        End Get
        Set(ByVal value As Integer)
            Me.DDLNumeroRecord.SelectedValue = value
        End Set
    End Property

    Public Property RecordTotali() As Integer Implements lm.Comol.Core.Statistiche.I_Uc_FileCom.RecordTotali
        Get
            If IsNothing(Me.ViewState("RecordTotali")) Then
                Me.ViewState("RecordTotali") = 0
            End If
            Return Me.ViewState("RecordTotali")
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("RecordTotali") = value
        End Set
    End Property

    Public Property Pager() As lm.Comol.Core.DomainModel.PagerBase Implements lm.Comol.Core.Statistiche.I_Uc_FileCom.Pager
        Get
            If TypeOf Me.ViewState("Pager") Is lm.Comol.Core.DomainModel.PagerBase Then
                Return Me.ViewState("Pager")
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As lm.Comol.Core.DomainModel.PagerBase)
            Me.ViewState("Pager") = value
            Me.PGgrid_UtentiCom.Pager = value
            Me.PGgrid_UtentiCom.Visible = Not value.Count = 0 AndAlso (value.Count + 1 > value.PageSize)
        End Set
    End Property

    Private Sub PGgrid_UtentiCom_OnPageSelected() Handles PGgrid_UtentiCom.OnPageSelected
        Me.Pager = Me.PGgrid_UtentiCom.Pager
        Me.Presenter.BindUserList(Me.CommunityID, Me.DDLtipoRicerca.SelectedValue, Me.TXBvalore.Text)
    End Sub

    Private Sub Lkb_Cerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Lkb_Cerca.Click
        Me.Pager = Me.PGgrid_UtentiCom.Pager
        Me.Presenter.BindUserList(Me.CommunityID, Me.DDLtipoRicerca.SelectedValue, Me.TXBvalore.Text)
    End Sub

    Private Sub DDLNumeroRecord_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLNumeroRecord.SelectedIndexChanged
        Me.Pager = Me.PGgrid_UtentiCom.Pager
        Me.Presenter.BindUserList(Me.CommunityID, Me.DDLtipoRicerca.SelectedValue, Me.TXBvalore.Text)
    End Sub
End Class