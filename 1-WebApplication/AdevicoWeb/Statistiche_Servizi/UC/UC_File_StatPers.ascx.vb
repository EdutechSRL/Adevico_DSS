Imports lm.Comol.Core.Statistiche

Partial Public Class UC_File_StatPers
    Inherits BaseControlSession
    Implements I_Uc_FilePers

    Private _Presenter As Presenter_Uc_FilePers
    Private _UserID As Integer
    Private _CommunityID As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("StatisticheService", "UC_FILE_Stat")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub

    ''' <summary>
    ''' Richiamato dal suo preesnter
    ''' </summary>
    ''' <param name="oUserList"></param>
    ''' <remarks></remarks>
    Public Sub BindUserList(ByVal oUserList As System.Collections.Generic.IList(Of lm.Comol.Core.Statistiche.FilePersonalStat)) Implements lm.Comol.Core.Statistiche.I_Uc_FilePers.BindUserList
        Me.Dg_StatUserFile.DataSource = oUserList
        Me.Dg_StatUserFile.DataBind()
    End Sub

    ''' <summary>
    ''' Richiamato da MainStatistiche servizi
    ''' </summary>
    ''' <param name="Community_Id"></param>
    ''' <param name="User_Id"></param>
    ''' <remarks></remarks>
    Public Sub Bind(ByVal Community_Id As Integer, ByVal User_Id As Integer) Implements lm.Comol.Core.Statistiche.IpersonalContainer.Bind
        Me.UserID = User_Id
        Me.CommunityID = Community_Id
        Me.Presenter.BindFileStat()
        If IsNothing(Me.Resource) Then
            Me.SetCultureSettings()
        End If
        With Me.Resource
            .setLabel(Me.LBL_NumPlay_t)
        End With
        Me.LBL_UserName.Text = MyBase.UtenteCorrente.Anagrafica
    End Sub

    Public Property CommunityID() As Integer Implements lm.Comol.Core.Statistiche.IpersonalContainer.CommunityID
        Get
            Return _CommunityID
        End Get
        Set(ByVal value As Integer)
            _CommunityID = value
        End Set
    End Property
    Public Property UserID() As Integer Implements lm.Comol.Core.Statistiche.IpersonalContainer.UserID
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property

    Private Property Presenter() As Presenter_Uc_FilePers
        Get
            If IsNothing(Me._Presenter) Then
                Me._Presenter = New Presenter_Uc_FilePers(Me)
            End If
            Return Me._Presenter
        End Get
        Set(ByVal value As Presenter_Uc_FilePers)
            Me._Presenter = value
        End Set
    End Property

    Private Sub Dg_StatUserFile_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles Dg_StatUserFile.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Try
                Resource.setLabel(e.Item.FindControl("Lbl_FileName_Dg"))
            Catch ex As Exception
            End Try
            Try
                Resource.setLabel(e.Item.FindControl("Lbl_Scaricamenti_Dg"))
            Catch ex As Exception
            End Try
            Try
                Resource.setLabel(e.Item.FindControl("Lbl_LastDownload_Dg"))
            Catch ex As Exception
            End Try
            'Lbl_FileName_Dg
            'Lbl_Scaricamenti_Dg
            'Lbl_LastDownload_Dg
        End If

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oFPStat As FilePersonalStat
            Try
                oFPStat = e.Item.DataItem
            Catch ex As Exception
                Exit Sub
            End Try

            Dim Lbl_FileName, Lbl_Scaricamenti, Lbl_LastDownload As Label
            Dim Lnb_GotoFile As LinkButton

            Try
                Lbl_FileName = e.Item.Cells(1).FindControl("Lbl_FileName")
            Catch ex As Exception
            End Try
            If Not IsNothing(Lbl_FileName) Then
                Lbl_FileName.Text = oFPStat.FilePath & oFPStat.FileName
            End If

            'Try
            '    Lnb_GotoFile = e.Item.Cells(1).FindControl("Lnb_GotoFile")
            'Catch ex As Exception
            'End Try
            'If Not IsNothing(Lnb_GotoFile) Then
            '    With Lnb_GotoFile
            '        .CommandName = "GotoUser"
            '        .CommandArgument = oFPStat.FileID
            '        .Enabled = False
            '    End With
            'End If

            Try
                Lbl_Scaricamenti = e.Item.Cells(1).FindControl("Lbl_Scaricamenti")
            Catch ex As Exception
            End Try
            If Not IsNothing(Lbl_Scaricamenti) Then
                Lbl_Scaricamenti.Text = oFPStat.NumDownload
            End If

            Try
                Lbl_LastDownload = e.Item.Cells(1).FindControl("Lbl_LastDownload")
            Catch ex As Exception
            End Try
            If Not IsNothing(Lbl_LastDownload) Then
                Lbl_LastDownload.Text = oFPStat.LastDownload.ToString
            End If

            'Lnb_GotoFile
            'Lbl_FileName

            'Lbl_Scaricamenti
            'Lbl_LastDownload
        End If
    End Sub

    Public WriteOnly Property TotaleDownload() As Object Implements lm.Comol.Core.Statistiche.I_Uc_FilePers.TotaleDownload
        Set(ByVal value As Object)
            Me.LBL_NumPlay.Text = value
        End Set
    End Property
End Class