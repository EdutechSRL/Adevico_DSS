Imports lm.Comol.Core.BaseModules.FileStatistics.Presentation
'Imports lm.Comol.Modules.ScormStat

Public Class UC_DisplayFileInfo
    Inherits BaseControl
    Implements IViewDisplayDetailfile

    Private _presenter As DisplayDetailFilePresenter

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("UC_DisplayFileDetail", "Modules", "FileStatistics")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setLabel(Lbl_DetFile_t)
            .setLabel(Lbl_ComService_t)
            .setLabel(Lbl_Path_t)
            .setLabel(Lbl_Size_t)
            .setLabel(Lbl_UbloadBy_t)
            .setLabel(Lbl_TotalDownload_t)
        End With
    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get

        End Get
    End Property

    Public Sub InitiView(
                        ByVal UserId As Int32,
                        ByVal FileId As Int64,
                        ByVal CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext,
                        ByVal Perm As Integer)
        If IsNothing(_presenter) Then
            Me._presenter = New DisplayDetailFilePresenter(CurrentContext, Me)
        End If

        'OnlyForTest, poi me lo dovrò recuperare da dove serve...
        'Dim Servdict As Dictionary(Of String, String) = 

        Me._presenter.BindView(UserId, FileId, Getdictionary(), Perm)
    End Sub

    Private Function Getdictionary() As IDictionary(Of String, String)

        Return COL_BusinessLogic_v2.Comol.Manager.ManagerService.ListSystemTranslated(PageUtility.LinguaID) _
            .Distinct() _
            .ToDictionary(Function(ps) ps.Code, Function(ps) ps.Name)


        'Dim Servdict As New Dictionary(Of String, String)
        'Servdict.Add("SRVMATER", "Materiale")
        'Servdict.Add("SRVMAIL", "Invio Mail")
        'Servdict.Add("SRVLEZ", "Diario Lezione")
        'Servdict.Add("SRVFORUM", "Forum")
        'Servdict.Add("SRVCHAT", "Chat")
        'Servdict.Add("SRVPOSTIT", "Memo")
        'Servdict.Add("SRVTESI", "Tesi")
        'Servdict.Add("SRVADMSMS", "Amministrazione SMS")
        'Servdict.Add("SRVSMS", "SMS")
        'Servdict.Add("SRVBACH", "Bacheca")
        'Servdict.Add("SRVEVENTI", "Eventi")
        'Servdict.Add("SRVADMCMNT", "Amministrazione Comunità")
        'Servdict.Add("SRVADMGLB", "Amministrazione Globale")
        'Servdict.Add("SRVCMMS", "Commissioni Tesi")
        'Servdict.Add("SRV_VISTESI", "Visualizzazione Tesi")
        'Servdict.Add("SRVISCRITTI", "Gestione Iscritti")
        'Servdict.Add("SRVCOORD", "Coordinamento Comunità")
        'Servdict.Add("SRVGALLERY", "Servizio Gallery")
        'Servdict.Add("SRVLINK", "Raccolta Link")
        'Servdict.Add("SRVSTAT", "Servizio Statistiche")
        'Servdict.Add("SRVmngmISCRITTI", "Management Esteso Iscritti")
        'Servdict.Add("SRVCRSUNI", "Corso universitario")
        'Servdict.Add("SRV_ARGMTESI", "Argomenti Tesi")
        'Servdict.Add("SRV_PRGTTESI", "Progetti Tesi")
        'Servdict.Add("SRV_PRPTTESI", "Gestione Proposte Tesi")
        'Servdict.Add("SRVLSTISCR", "Lista iscritti")
        'Servdict.Add("SRVCOVER", "Cover")
        'Servdict.Add("SRVELNC_CMNT", "Servizio Elenca sotto comunità")
        'Servdict.Add("SRVISCR_CMNT", "Servizio Iscrizione")
        'Servdict.Add("SRVWIKI", "Wiki")
        'Servdict.Add("SRVQUST", "Servizio Questionari")
        'Servdict.Add("SRVBLOG", "Servizio Blog")
        'Servdict.Add("SRVLBEL", "Quaderni attività")
        'Servdict.Add("SRVESSE3", "Registro Esse3")
        'Servdict.Add("SRVCRCLA", "Curriculum")
        'Servdict.Add("SRVDIMDIM", "Web Meeting")
        'Servdict.Add("SRVOLUSR", "Utenti On Line")
        'Servdict.Add("SRVUSAGER", "Registro presenze")
        'Servdict.Add("SRVCMNTNEWS", "Novità di comunità")
        'Servdict.Add("SRVTASK", "TaskList")
        'Servdict.Add("SRVFAQ", "Faq")
        'Servdict.Add("SRVEDUP", "Percorso Formativo")
        'Servdict.Add("SRVCFP", "Call For Papers")
        'Return Servdict
    End Function

    Public Sub BindData(ByVal FileDetail As lm.Comol.Core.BaseModules.FileStatistics.Domain.dtoFileDetail, ByVal Perm As Integer) Implements lm.Comol.Core.BaseModules.FileStatistics.Presentation.IViewDisplayDetailfile.BindData

        Lbl_FileName.Text = FileDetail.FileName
        Lbl_ComService.Text = FileDetail.ComService
        Lbl_Path.Text = FileDetail.Path
        Lbl_Size.Text = Str((FileDetail.Size / 1024))
        Lbl_UbloadBy.Text = FileDetail.LoadedBy
        Lbl_UploadedOn.Text = FileDetail.LoadedOn.ToString()
        Lbl_TotalDownload.Text = FileDetail.Downloads.ToString()

        If Perm > 1 Then
            Me.RptDownSummary.DataSource = FileDetail.DownDetails
            Me.RptDownSummary.DataBind()
            Me.RptDownSummary.Visible = True
        Else
            Me.RptDownSummary.Visible = False
        End If
        
    End Sub

    Public Sub InitializeNoPermission(ByVal idCommunity As Integer) Implements lm.Comol.Core.BaseModules.FileStatistics.Presentation.IViewDisplayDetailfile.InitializeNoPermission

    End Sub

    Private Sub RptDownSummary_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RptDownSummary.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim DuserInfo As lm.Comol.Core.BaseModules.FileStatistics.Domain.dtoUserDownInfo = e.Item.DataItem

            Dim oLabel As Label

            oLabel = e.Item.FindControl("Lbl_PersonName")
            oLabel.Text = DuserInfo.downBy

            oLabel = e.Item.FindControl("Lbl_NumDown")

            Dim Downcount As Integer = 0
            If Not IsNothing(DuserInfo.downOnList) Then
                Downcount = DuserInfo.downOnList.Count()
            End If


            If (Downcount > 0) Then
                oLabel.Text = DuserInfo.downOnList.Count.ToString()

                Dim oRepeater As Repeater = e.Item.FindControl("RptDownDetail")
                oRepeater.DataSource = DuserInfo.downOnList
                AddHandler oRepeater.ItemDataBound, AddressOf RptDownDetail_ItemDataBound
                oRepeater.DataBind()
            Else
                oLabel.Text = "0"
            End If


            '
            'Da qui... 
            '1. Inserire i 2 dati e fare il bind del ripeter...
            '2. Internazionalizzare il tutto
            '3. Rivedere grafica
            '4. Eliminare il numdown
            '5. Vedere come/dove recuperare l'internazionalizzazione dei servizi...

        End If
    End Sub

    Private Sub RptDownDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) '.DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim oDownInfo As lm.Comol.Core.BaseModules.FileStatistics.Domain.dtoDownInfo = e.Item.DataItem

            Dim oLabel As Label

            oLabel = e.Item.FindControl("Lbl_DD_Date")
            oLabel.Text = oDownInfo.downDate.ToString

            Resource.setLabel(e.Item.FindControl("Lbl_DD_Service_t"))

            oLabel = e.Item.FindControl("Lbl_DD_Service")
            oLabel.Text = oDownInfo.downService

        End If
    End Sub

End Class