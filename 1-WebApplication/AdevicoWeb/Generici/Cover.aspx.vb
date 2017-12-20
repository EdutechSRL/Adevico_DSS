Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.FileLayer
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.Comol.Entities.Configuration
Imports lm.ActionDataContract
Imports System.Reflection
Imports lm.Comol.Core.File

Partial Public Class Cover
    Inherits PageBase
    Implements IviewCover

    Private _presenter As PresenterCover
    Private _Servizio As Services_Cover
    Private _ObjectPath As ObjectFilePath

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
    Public ReadOnly Property Presenter() As PresenterCover
        Get
            If IsNothing(_presenter) Then
                _presenter = New PresenterCover(Me)
            End If
            Presenter = _presenter
        End Get
    End Property

    Public ReadOnly Property PaginaDefault() As String Implements IviewCover.PaginaDefault
        Get
            Return "Comunita/Comunita.aspx"
        End Get
    End Property

    Private ReadOnly Property DestinationPath() As String Implements IviewCover.DestinationPath
        Get
            Dim Path As String = ""
            If IsNothing(_ObjectPath) Then
                _ObjectPath = Me.ObjectPath(Me.SystemSettings.File.Cover)
            End If
            Path = _ObjectPath.Drive & Me.ComunitaLavoroID & "\"
            Return Path
        End Get
    End Property
    Private ReadOnly Property VirtualDestinationPath() As String
        Get
            Dim Path As String = ""
            If IsNothing(_ObjectPath) Then
                _ObjectPath = Me.ObjectPath(Me.SystemSettings.File.Cover)
            End If

            Path = _ObjectPath.Virtual & Me.ComunitaLavoroID & "/"
            Path = Replace(Path, "//", "/")
            Return Path
        End Get
    End Property
    Private Property CoverComunita() As COL_BusinessLogic_v2.COL_Cover Implements IviewCover.CoverComunita
        Get
            If Me.MLVcontenuto.GetActiveView Is Me.VIWedit Then
                CoverComunita = Me.CTRLgestioneCover.CoverComunita
            Else
                Try
                    CoverComunita = DirectCast(Me.ViewState("Cover"), COL_BusinessLogic_v2.COL_Cover)
                Catch ex As Exception
                    CoverComunita = Nothing
                End Try
            End If
        End Get
        Set(ByVal value As COL_BusinessLogic_v2.COL_Cover)
            Me.ViewState("Cover") = value
            If IsNothing(value) Then
                Me.MLVcontenuto.SetActiveView(Me.VIWnonDefinita)
            Else
                Bind_DatiCover()
            End If
        End Set
    End Property
    Private Property SkipCover() As Boolean Implements IviewCover.SkipCover
        Get
            SkipCover = Me.CBXskip.Checked
        End Get
        Set(ByVal value As Boolean)
            Me.CBXskip.Checked = value
        End Set
    End Property
    Public Property VisibilitaSkip() As Boolean Implements IviewCover.VisibilitaSkip
        Get
            VisibilitaSkip = Me.CBXskip.Visible
        End Get
        Set(ByVal value As Boolean)
            Me.CBXskip.Visible = value
        End Set
    End Property
    Private ReadOnly Property Servizio() As COL_BusinessLogic_v2.UCServices.Services_Cover Implements IviewCover.Servizio
        Get
            If IsNothing(_Servizio) Then
                If Me.isModalitaAmministrazione Then 'And Me.isUtenteAnonimo 
                    _Servizio = New Services_Cover(COL_Comunita.GetPermessiForServizioByCode(Main.TipoRuoloStandard.AdminComunità, Me.AmministrazioneComunitaID, Services_Cover.Codex))
                Else
                    _Servizio = MyBase.ElencoServizi.Find(Services_Cover.Codex)
                    If IsNothing(_Servizio) Then
                        _Servizio = New Services_Cover("00000000000000000000000000000000")
                    End If
                End If
            End If
            Servizio = _Servizio
        End Get
    End Property

    Private ReadOnly Property PaginaServizio() As ServicePage Implements IviewCover.PaginaServizio
        Get
            Return Me.CTRLgestioneCover.PaginaServizio
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(_presenter) Then
            _presenter = New PresenterCover(Me)
        End If
    End Sub

    Public Overrides Sub BindDati()
        _presenter = New PresenterCover(Me)
        If (Me.IsPostBack = False) Then
            _presenter.Init()
            Me.LNBgestione.Visible = Me.Servizio.Admin OrElse Me.Servizio.Management
        End If
    End Sub

    Public Overrides Sub BindNoPermessi()
        Me.MLVcontenuto.SetActiveView(Me.VIWpermessi)
        Me.PageUtility.AddAction(Services_Cover.ActionType.NoPermission, Nothing, InteractionType.UserWithLearningObject)
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return Me.Servizio.Read OrElse Me.Servizio.Admin OrElse Me.Servizio.Management
    End Function

    Public Overrides Sub RegistraAccessoPagina()
        Me.PageUtility.AddAction(Services_Cover.ActionType.Show, Nothing, InteractionType.UserWithLearningObject)

        'Me.AddAction(Services_Cover.ActionType.Show, Nothing, lm.ActionDataContract.InteractionType.UserWithLearningObject)
    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Cover", "Generici")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(Me.LBNopermessi)
            .setLabel(Me.LBnonDefinita)
            .setLabel(Me.LBnoattivata)
            '.setLabel(Me.LBtitoloServizio)
            Me.Master.ServiceTitle = .getValue("LBtitoloServizio.text")
            .setLinkButton(Me.LNBvisualizza, True, True)
            .setLinkButton(Me.LNBgestione, True, True)
            .setLinkButton(Me.LNBsalvaImpostazioni, True, True)
            .setCheckBox(Me.CBXskip)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

#Region "Visualizzazione Cover"
    Private Sub Bind_CoverTitolo()
        Me.DVtitoloCover.Style("display") = "block"
        With Me.CoverComunita
            Me.LBTitolo.Text = .Titolo
            Me.LNBtitolo.Text = .Titolo
            If .FontFace1 <> "" Then
                Me.LBTitolo.Font.Name = .FontFace1
                Me.LNBtitolo.Font.Name = .FontFace1
            Else
                Me.LBTitolo.Font.Name = "Verdana,Arial,Tahoma,Helvetica,sans-serif"
                Me.LNBtitolo.Font.Name = "Verdana,Arial,Tahoma,Helvetica,sans-serif"
            End If
            If .FontSize1 <> 0 Then
                Me.LBTitolo.Font.Size = System.Web.UI.WebControls.FontUnit.Point(.FontSize1)
                Me.LNBtitolo.Font.Size = System.Web.UI.WebControls.FontUnit.Point(.FontSize1)
            Else
                Me.LBTitolo.Font.Size = System.Web.UI.WebControls.FontUnit.Point(16)
                Me.LNBtitolo.Font.Size = System.Web.UI.WebControls.FontUnit.Point(16)
            End If

            Me.LBTitolo.Font.Bold = .Bold1
            Me.LBTitolo.Font.Italic = .Italic1
            Me.LNBtitolo.Font.Bold = .Bold1
            Me.LNBtitolo.Font.Italic = .Italic1

            If .FontColor1 <> "" Then
                Me.LBTitolo.ForeColor = System.Drawing.Color.FromName(.FontColor1)
                Me.LNBtitolo.ForeColor = System.Drawing.Color.FromName(.FontColor1)
            Else
                Me.LBTitolo.ForeColor = System.Drawing.Color.Black
                Me.LNBtitolo.ForeColor = System.Drawing.Color.Black
            End If
            Me.LNBtitolo.Font.Underline = True
            Me.LNBtitolo.Attributes.Add("onmouseout", "status='';return true;")
            Me.LNBtitolo.Attributes.Add("onmouseclick", "status='';return true;")
            Me.LNBtitolo.Attributes.Add("onmouseover", "status='';return true;")
        End With
    End Sub
    Private Sub Bind_AnnoAccademico()
        Me.DVannoAccademico.Style("display") = "block"
        With Me.CoverComunita
            Me.LBannoAccademico.Text = .Anno

            If .FontFace2 <> "" Then
                Me.LBannoAccademico.Font.Name = .FontFace2
            Else
                Me.LBannoAccademico.Font.Name = "Verdana,Arial,Tahoma,Helvetica,sans-serif"
            End If
            If .FontSize2 <> 0 Then
                Me.LBannoAccademico.Font.Size = System.Web.UI.WebControls.FontUnit.Point(.FontSize2)
            Else
                Me.LBannoAccademico.Font.Size = System.Web.UI.WebControls.FontUnit.Point(12)
            End If
            If .FontColor2 <> "" Then
                Me.LBannoAccademico.ForeColor = System.Drawing.Color.FromName(.FontColor2)
            Else
                Me.LBannoAccademico.ForeColor = System.Drawing.Color.Black
            End If
            Me.LBannoAccademico.Font.Bold = .Bold2
            Me.LBannoAccademico.Font.Italic = .Italic2
        End With
    End Sub
    Private Sub Bind_Commenti()
        Me.DVcommentiCover.Style("display") = "block"
        With Me.CoverComunita
            Me.LBcommenti.Text = .Commenti

            If .FontFace3 <> "" Then
                Me.LBcommenti.Font.Name = .FontFace3
            Else
                Me.LBcommenti.Font.Name = "Verdana,Arial,Tahoma,Helvetica,sans-serif"
            End If
            If .FontSize3 <> 0 Then
                Me.LBcommenti.Font.Size = System.Web.UI.WebControls.FontUnit.Point(.FontSize3)
            Else
                Me.LBcommenti.Font.Size = System.Web.UI.WebControls.FontUnit.Point(10)
            End If
            If .FontColor3 <> "" Then
                Me.LBcommenti.ForeColor = System.Drawing.Color.FromName(.FontColor3)
            Else
                Me.LBcommenti.ForeColor = System.Drawing.Color.Black
            End If
            Me.LBcommenti.Font.Bold = .Bold3
            Me.LBcommenti.Font.Italic = .Italic3
        End With
    End Sub
    Private Sub Bind_Immagine()
        Me.DVimmagineCover.Style("display") = "block"
        With Me.CoverComunita
            Me.IMBcover.ImageUrl = VirtualDestinationPath & .Immagine
            'Dim oFile As New COL_File
            Dim altezza, larghezza As Integer
            ContentOf.ImageSize(Me.DestinationPath & .Immagine, altezza, larghezza)
            If altezza > 300 Then
                Me.IMBcover.Height = System.Web.UI.WebControls.Unit.Pixel(300)
                'osservo se così ridimensionata è ancora più larga di 700 se si taglio!
                Dim proporzione As Double = altezza / 300
                Dim larghezzaNew As Double = larghezza / proporzione
                If larghezzaNew > 700 Then
                    Me.IMBcover.Width = System.Web.UI.WebControls.Unit.Pixel(700)
                Else
                    Me.IMBcover.Width = System.Web.UI.WebControls.Unit.Pixel(larghezzaNew)
                End If
            Else
                Me.IMBcover.Height = System.Web.UI.WebControls.Unit.Pixel(altezza)
                If larghezza > 700 Then
                    Me.IMBcover.Width = System.Web.UI.WebControls.Unit.Pixel(700)
                    'osservo se così ridimensionata è ancora più alta di 300 se si taglio!
                    Dim proporzione As Double = larghezza / 700
                    Dim altezzaNew As Double = altezzaNew / proporzione
                    If altezzaNew > 300 Then
                        Me.IMBcover.Height = System.Web.UI.WebControls.Unit.Pixel(300)
                    Else
                        Me.IMBcover.Height = System.Web.UI.WebControls.Unit.Pixel(altezzaNew)
                    End If
                Else
                    Me.IMBcover.Width = System.Web.UI.WebControls.Unit.Pixel(larghezza)
                End If
            End If

            If .Didascalia <> "" Then
                Me.IMBcover.ToolTip = .Didascalia
            End If

        End With
    End Sub

    Private Sub Bind_DatiCover()
        Dim oCover As COL_Cover

        oCover = Me.CoverComunita
        If oCover.isAttivata Then
            Me.MLVcontenuto.SetActiveView(Me.VIWcover)

            If oCover.Titolo = String.Empty Or oCover.Titolo = "" Then
                Me.DVtitoloCover.Style("display") = "none"
            Else
                Me.Bind_CoverTitolo()
            End If
            If oCover.Anno = String.Empty Or oCover.Anno = "" Then
                Me.DVannoAccademico.Style("display") = "none"
            Else
                Me.Bind_AnnoAccademico()
            End If
            If oCover.Commenti = String.Empty Or oCover.Commenti = "" Then
                Me.DVcommentiCover.Style("display") = "none"
            Else
                Me.Bind_Commenti()
            End If
            If oCover.Immagine = String.Empty Or oCover.Immagine = "" Then
                Me.DVimmagineCover.Style("display") = "none"
            ElseIf Me.CoverComunita.ImageExist(Me.DestinationPath) Then
                Me.Bind_Immagine()
            Else
                Me.DVimmagineCover.Style("display") = "none"
            End If
        Else
            Me.MLVcontenuto.SetActiveView(Me.VIWnonAttivata)
        End If

    End Sub

#End Region

    Private Sub LNBgestione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBgestione.Click
        Me._presenter.Edit()
    End Sub

    Public Sub ShowManagement(ByVal oCover As COL_BusinessLogic_v2.COL_Cover) Implements IviewCover.ShowManagement
        Me.MLVcontenuto.SetActiveView(Me.VIWedit)
        Me.CTRLgestioneCover.CoverComunita = oCover
        Me.LNBgestione.Visible = False
        Me.LNBsalvaImpostazioni.Visible = True
        Me.LNBvisualizza.Visible = True
    End Sub


    Private Sub LNBsalvaImpostazioni_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBsalvaImpostazioni.Click
        If Page.IsValid Then
            Me._presenter.Save()
            Me.PageUtility.AddAction(Services_Cover.ActionType.Edit, Nothing, InteractionType.UserWithLearningObject)
        End If
    End Sub

    Public Sub ShowCover(ByVal oCover As COL_BusinessLogic_v2.COL_Cover) Implements IviewCover.ShowCover
        Me.MLVcontenuto.SetActiveView(Me.VIWcover)
        Me.CoverComunita = oCover
        Me.PageUtility.AddAction(Services_Cover.ActionType.Show, Me.PageUtility.CreateObjectsList(Services_Cover.ObjectType.Cover, oCover.Id), InteractionType.UserWithLearningObject)
    End Sub

    Public Sub SetEditImage(ByVal Immagine As String) Implements IviewCover.SetEditImage
        Me.CTRLgestioneCover.SetEditImage(Immagine)
    End Sub

    Public Sub DeleteImageFile(ByVal Immagine As String) Implements IviewCover.DeleteImageFile
        Delete.File_FM(Me.DestinationPath & Immagine)
    End Sub

    Public Function SaveImageFile(Optional ByVal ImmagineCorrente As String = "") As String Implements IviewCover.SaveImageFile
        Dim NomeImmagine As String = ""
        If Me.CTRLgestioneCover.PostedFile.FileName <> String.Empty Then
            Dim PrefissoData As String = Now.ToString
            Dim NomeFile As String = ""

            Dim Estensione As String = ""

            PrefissoData = PrefissoData.Replace(" ", "-")
            PrefissoData = PrefissoData.Replace("/", "-")
            PrefissoData = PrefissoData.Replace(".", "_")
            PrefissoData = PrefissoData.Replace(":", "_")
            NomeFile = Me.CTRLgestioneCover.PostedFile.FileName
            Estensione = Right(NomeFile, Len(NomeFile) - NomeFile.LastIndexOf("."))


            Create.Directory(DestinationPath)
            If Create.UploadFile(Me.CTRLgestioneCover.PostedFile, Me.DestinationPath & PrefissoData & Estensione) Then
                Dim oFile As New COL_File
                If oFile.ResizeLogo(DestinationPath & PrefissoData & Estensione, Me.DestinationPath & PrefissoData & "_mini" & Estensione, 100, 125) < 1 Then
                    Me.ShowErrorEditing(IviewCover.CoverError.ImageNotResized)
                Else
                    NomeImmagine = PrefissoData & Estensione
                End If
                Me.CTRLgestioneCover.SetEditImage(NomeImmagine)
            Else
                Me.ShowErrorEditing(IviewCover.CoverError.ImageNotUploaded)
                NomeImmagine = ""
            End If
        End If
        Return NomeImmagine
    End Function

    Public Sub ShowErrorEditing(ByVal Errore As IviewCover.CoverError) Implements IviewCover.ShowErrorEditing
        If Errore = IviewCover.CoverError.None Then
            Dim alertMSG As String = Me.Resource.getValue("Errore." & CType(Errore, IviewCover.CoverError))
            If alertMSG <> "" Then
                Me.ShowAlertError(alertMSG)
            End If
        ElseIf Errore = IviewCover.CoverError.NotAdded Or Errore = IviewCover.CoverError.ImageNotChanged Then
            Dim alertMSG As String = Me.Resource.getValue("Errore." & CType(Errore, IviewCover.CoverError))
            If alertMSG <> "" Then
                Me.ShowAlertError(alertMSG)
            End If
        Else
            Me.CTRLgestioneCover.ShowErrorEditing(True, Me.Resource.getValue("Errore." & CType(Errore, IviewCover.CoverError)))
        End If
    End Sub


    Private Sub LNBvisualizza_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBvisualizza.Click
        Me.MLVcontenuto.SetActiveView(Me.VIWcover)
        Me.LNBgestione.Visible = Me.Servizio.Admin Or Me.Servizio.Management
        Me.LNBsalvaImpostazioni.Visible = False
        Me.LNBvisualizza.Visible = False
        Me._presenter.Show()
    End Sub

    Private Sub IMBcover_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBcover.Click
        Me.RedirectToUrl(Me._presenter.GetDefaultPage)
    End Sub

    Private Sub CBXskip_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBXskip.CheckedChanged
        Me._presenter.SetSkipCover(Me.CBXskip.Checked)
    End Sub


    Private Sub LNBtitolo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBtitolo.Click
        Me.RedirectToUrl(Me._presenter.GetDefaultPage)
    End Sub

    Public ReadOnly Property CurrentModuleID() As Integer
        Get
            Return PageUtility.CurrentModule.ID
        End Get
    End Property

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_Cover.Codex)
    End Sub

    Public Sub SendAddNotification(ByVal CoverID As Integer) Implements PresentationLayer.IviewCover.SendAddNotification
        Dim oServiceNotification As New CoverNotificationUtility(Me.PageUtility)

        oServiceNotification.NotifyAdd(Me.ComunitaLavoroID, CoverID)
    End Sub

    Public Sub SendEditNotification(ByVal CoverID As Integer) Implements PresentationLayer.IviewCover.SendEditNotification
        Dim oServiceNotification As New CoverNotificationUtility(Me.PageUtility)
        oServiceNotification.NotifyEdit(Me.ComunitaLavoroID, CoverID)
    End Sub

End Class