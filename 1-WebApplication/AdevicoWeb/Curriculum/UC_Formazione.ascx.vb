Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona

Public Class UC_Formazione
    Inherits System.Web.UI.UserControl
    Protected oResourceFormazione As ResourceManager

#Region "accessori"
    Protected WithEvents HDNprsn_id As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNcreu_id As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNisfr_id As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents PNLinserimento As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLformazione As System.Web.UI.WebControls.Panel
#End Region
#Region "lista"

    Protected WithEvents RPTformazione As System.Web.UI.WebControls.Repeater
    'Protected WithEvents BTNinserisci As System.Web.UI.WebControls.Button
    Protected WithEvents TBRmaterie As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRqualifica As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRlivello As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBinizio_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBfine_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBnome_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBmaterie_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBqualifica_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBclassificazione_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBrendiPubblico_t As System.Web.UI.WebControls.Label
#End Region

#Region "aggiungi"
    Protected WithEvents LBerrore As System.Web.UI.WebControls.Label
    'Protected WithEvents BTNannulla As System.Web.UI.WebControls.Button
    '  Protected WithEvents BTNaggiungi As System.Web.UI.WebControls.Button
    ' Protected WithEvents BTNmodifica As System.Web.UI.WebControls.Button
    Protected WithEvents DDLfine As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLinizio As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TXBnome As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBmaterie As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBqualifica As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBclassificazione As System.Web.UI.WebControls.TextBox
    Protected WithEvents CBXrendiPubblico As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CBXinCorso As System.Web.UI.WebControls.CheckBox
#End Region

    Public Property CurriculumID() As Integer
        Get
            If Me.HDNcreu_id.Value = "" Then
                Me.HDNcreu_id.Value = 0
            End If
            CurriculumID = Me.HDNcreu_id.Value
        End Get
        Set(ByVal Value As Integer)
            Me.HDNcreu_id.Value = Value
        End Set
    End Property

    Public Property PRSN_ID() As Integer
        Get
            If Me.HDNprsn_id.Value = "" Then
                Me.HDNprsn_id.Value = 0
            End If
            PRSN_ID = Me.HDNprsn_id.Value
        End Get
        Set(ByVal Value As Integer)
            Me.HDNprsn_id.Value = Value
        End Set
    End Property

    Public Event MostraModifica(ByVal sender As Object, ByVal e As EventArgs)
    Public Event MostraAggiungi(ByVal sender As Object, ByVal e As EventArgs)

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsNothing(oResourceFormazione) Then
            SetCulture(Session("LinguaCode"))
        End If
        Try
            If Page.IsPostBack = False Then

                Me.SetupInternazionalizzazione()
                RiempiDDLInizio()
                RiempiDDLFine()

                Dim PRSN_id As Integer
                PRSN_id = Me.HDNprsn_id.Value
                Dim oFormazione As New COL_IstruzioneFormazione
                oFormazione.EstraiByPersona(PRSN_id)


                If oFormazione.ID = -1 Then
                    PNLinserimento.Visible = True
                    PNLformazione.Visible = False
                    Viewstate("startFormazione") = "si"
                Else
                    PNLinserimento.Visible = False
                    PNLformazione.Visible = True
                    Bind_Dati()
                End If
                Me.TXBclassificazione.Attributes.Add("onkeypress", "return(LimitText(this,1000));")
                Me.TXBmaterie.Attributes.Add("onkeypress", "return(LimitText(this,1000));")
                Me.TXBnome.Attributes.Add("onkeypress", "return(LimitText(this,1000));")
                Me.TXBqualifica.Attributes.Add("onkeypress", "return(LimitText(this,1000));")

            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub start()
        If Viewstate("startFormazione") <> "si" Then
            Me.PNLinserimento.Visible = False
            Me.PNLformazione.Visible = True
        Else
            RaiseEvent MostraAggiungi(Me, EventArgs.Empty)
        End If

        Me.PulisciCampi()
    End Sub

#Region "Bind_Dati"
    Private Sub Bind_Dati()
        Dim PRSN_id As Integer
        Dim oDataset As DataSet
        Dim i, totale As Integer
        Dim oFormazione As New COL_IstruzioneFormazione
        Try
            PRSN_id = Me.HDNprsn_id.Value
            oDataset = oFormazione.ElencaByPersona(PRSN_id)
            totale = oDataset.Tables(0).Rows.Count
            If totale = 0 Then 'se datagrid vuota
                Me.PNLinserimento.Visible = True
                Me.PNLformazione.Visible = False
                'invio l'evento per mostrare il pulsante inserisci anzichè visualizza aggiungi...
                RaiseEvent MostraAggiungi(Me, EventArgs.Empty)
            Else
                Viewstate("startFormazione") = ""
                oDataset.Tables(0).Columns.Add(New DataColumn("ofine"))
                oDataset.Tables(0).Columns.Add(New DataColumn("oRendiPubblico"))
                For i = 0 To totale - 1
                    Dim oRow As DataRow

                    oRow = oDataset.Tables(0).Rows(i)

                    If oRow.Item("ISFR_esperienzaInCorso") = "1" Then
                        oRow.Item("ofine") = oResourceFormazione.getValue("incorso") '"Esperienza in Corso" 'oResource.
                    Else
                        oRow.Item("ofine") = oRow.Item("ISFR_fine")
                    End If
                    If oRow.Item("ISFR_rendiPubblico") = "1" Then
                        oRow.Item("oRendiPubblico") = "Si" 'oResource.
                    Else
                        oRow.Item("oRendiPubblico") = "No" 'oResource.
                    End If

                    If Not IsDBNull(oRow.Item("ISFR_principaliMaterieAbilita")) Then
                        oRow.Item("ISFR_principaliMaterieAbilita") = Replace(oRow.Item("ISFR_principaliMaterieAbilita"), vbCrLf, "<br>")
                    End If
                    If Not IsDBNull(oRow.Item("ISFR_qualificaConseguita")) Then
                        oRow.Item("ISFR_qualificaConseguita") = Replace(oRow.Item("ISFR_qualificaConseguita"), vbCrLf, "<br>")
                    End If
                    If Not IsDBNull(oRow.Item("ISFR_livelloClassificazioneNazionale")) Then
                        oRow.Item("ISFR_livelloClassificazioneNazionale") = Replace(oRow.Item("ISFR_livelloClassificazioneNazionale"), vbCrLf, "<br>")
                    End If
                Next
                RPTformazione.DataSource = oDataset
                RPTformazione.DataBind()

            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub BindModifica()
        Dim oFormazione As New COL_IstruzioneFormazione
        Try
            oFormazione.ID = HDNisfr_id.Value

            oFormazione.Estrai()
            Me.DDLinizio.SelectedValue = oFormazione.inizio
            Me.DDLfine.SelectedValue = oFormazione.fine
            Me.TXBnome.Text = oFormazione.nomeeTipoIstituto
            Me.TXBmaterie.Text = oFormazione.principaliMaterieAbilita
            Me.TXBqualifica.Text = oFormazione.qualificaConseguita
            Me.TXBclassificazione.Text = oFormazione.livelloClassificazioneNazionale
            Me.CBXrendiPubblico.Checked = oFormazione.RendiPubblico
            Me.CBXinCorso.Checked = oFormazione.EsperienzaInCorso
            If Me.CBXinCorso.Checked = True Then
                Me.DDLfine.Enabled = False
            Else
                Me.DDLfine.Enabled = True
            End If
            'oFormazione.Curriculum_id = Me.HDNcreu_id.Value
        Catch ex As Exception

        End Try

    End Sub
    Private Sub RiempiDDLInizio()
        Dim i As Integer

        Try
            Dim AnnoMin, AnnoMax As Int16
            AnnoMin = 1920
            AnnoMax = Date.Now.Year
            For i = 0 To (AnnoMax - AnnoMin)
                DDLinizio.Items.Add(New ListItem(CStr(AnnoMin + i), CInt(AnnoMin + i)))
            Next
            DDLinizio.SelectedValue = Now.Year
        Catch ex As Exception
            DDLinizio.Items.Add(New ListItem(Now.Year.ToString, Now.Year))
        End Try
    End Sub
    Private Sub RiempiDDLFine()
        Dim i As Integer
        Try
            Dim AnnoMin, AnnoMax As Int16
            AnnoMin = 1920
            AnnoMax = Date.Now.Year
            For i = 0 To (AnnoMax - AnnoMin)
                DDLfine.Items.Add(New ListItem(CStr(AnnoMin + i), CInt(AnnoMin + i)))
            Next
            DDLfine.SelectedValue = Now.Year
        Catch ex As Exception
            DDLfine.Items.Add(New ListItem(Now.Year.ToString, Now.Year))
        End Try
    End Sub
#End Region


#Region "gestione repeater"
    Private Sub RPTformazione_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTformazione.ItemCreated
        If IsNothing(oResourceFormazione) Then
            SetCulture(Session("LinguaCode"))
        End If
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Try
                Try
                    Dim oLinkbuttonM As LinkButton
                    oLinkbuttonM = e.Item.FindControl("LKBmodifica")
                    If IsNothing(oLinkbuttonM) = False Then
                        oResourceFormazione.setLinkButton(oLinkbuttonM, True, True, False, False)
                    End If
                Catch ax As Exception

                End Try
                Try
                    Dim oLinkbuttonE As LinkButton
                    oLinkbuttonE = e.Item.FindControl("LKBelimina")
                    If IsNothing(oLinkbuttonE) = False Then
                        oResourceFormazione.setLinkButton(oLinkbuttonE, True, True, False, True)
                    End If
                Catch ax As Exception

                End Try

                Try
                    Dim olabelInizio As Label
                    olabelInizio = e.Item.FindControl("LBinizio_s")
                    If IsNothing(olabelInizio) = False Then
                        oResourceFormazione.setLabel_To_Value(olabelInizio, "LBinizio_s.text")
                    End If
                Catch ex As Exception

                End Try
                Try
                    Dim olabelFine As Label
                    olabelFine = e.Item.FindControl("LBfine_s")
                    If IsNothing(olabelFine) = False Then
                        oResourceFormazione.setLabel_To_Value(olabelFine, "LBfine_s.text")
                    End If
                Catch ex As Exception

                End Try
                Try
                    Dim olabelNome As Label
                    olabelNome = e.Item.FindControl("LBnome_s")
                    If IsNothing(olabelNome) = False Then
                        oResourceFormazione.setLabel_To_Value(olabelNome, "LBnome_s.text")
                    End If
                Catch ex As Exception

                End Try
                Try
                    Dim olabelMaterie As Label
                    olabelMaterie = e.Item.FindControl("LBmaterie_s")
                    If IsNothing(olabelMaterie) = False Then
                        oResourceFormazione.setLabel_To_Value(olabelMaterie, "LBmaterie_s.text")
                    End If
                Catch ex As Exception

                End Try
                Try
                    Dim olabelQualifica As Label
                    olabelQualifica = e.Item.FindControl("LBqualifica_s")
                    If IsNothing(olabelQualifica) = False Then
                        oResourceFormazione.setLabel_To_Value(olabelQualifica, "LBqualifica_s.text")
                    End If
                Catch ex As Exception

                End Try
                Try
                    Dim olabelClassificazione As Label
                    olabelClassificazione = e.Item.FindControl("LBclassificazione_s")
                    If IsNothing(olabelClassificazione) = False Then
                        oResourceFormazione.setLabel_To_Value(olabelClassificazione, "LBclassificazione_s.text")
                    End If
                Catch ex As Exception

                End Try
                Try
                    Dim olabelRendiPubblico As Label
                    olabelRendiPubblico = e.Item.FindControl("LBrendiPubblico_s")
                    If IsNothing(olabelRendiPubblico) = False Then
                        oResourceFormazione.setLabel_To_Value(olabelRendiPubblico, "LBrendiPubblico_s.text")
                    End If
                Catch ex As Exception

                End Try
                'oResourceFormazione.setLabel(CType(RPTformazione.FindControl("LBinizio_s"), Label))
                'oResourceFormazione.setLabel(CType(RPTformazione.FindControl("LBfine_s"), Label))
                'oResourceFormazione.setLabel(CType(RPTformazione.FindControl("LBnome_s"), Label))
                'oResourceFormazione.setLabel(CType(RPTformazione.FindControl("LBmaterie_s"), Label))
                'oResourceFormazione.setLabel(CType(RPTformazione.FindControl("LBqualifica_s"), Label))
                'oResourceFormazione.setLabel(CType(RPTformazione.FindControl("LBclassificazione_s"), Label))
                'oResourceFormazione.setLabel(CType(RPTformazione.FindControl("LBrendiPubblico_s"), Label))
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub RPTformazione_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTformazione.ItemCommand
        Dim ISFR_id As Integer
        Select Case e.CommandName
            Case "modifica"
                ISFR_id = e.CommandArgument
                Me.PNLinserimento.Visible = True
                Me.PNLformazione.Visible = False
                RaiseEvent MostraModifica(Me, EventArgs.Empty)
                'Me.BTNmodifica.Visible = True
                ' Me.BTNaggiungi.Visible = False
                HDNisfr_id.Value = ISFR_id
                Me.BindModifica()
            Case "elimina"
                Try
                    ISFR_id = e.CommandArgument
                    Dim oFormazione As New COL_IstruzioneFormazione
                    oFormazione.Elimina(ISFR_id)
                    Me.Bind_Dati()
                Catch ex As Exception

                End Try



        End Select




    End Sub
#End Region

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResourceFormazione = New ResourceManager

        oResourceFormazione.UserLanguages = Code
        oResourceFormazione.ResourcesName = "pg_UC_Formazione"
        oResourceFormazione.Folder_Level1 = "Curriculum"
        oResourceFormazione.Folder_Level2 = "UC"
        oResourceFormazione.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResourceFormazione
            .setLabel(LBinizio_t)
            .setLabel(LBfine_t)
            .setCheckBox(CBXinCorso)
            .setLabel(LBnome_t)
            .setLabel(LBmaterie_t)
            .setLabel(LBqualifica_t)
            .setLabel(LBclassificazione_t)
            .setLabel(LBrendiPubblico_t)

        End With
    End Sub

#End Region


#Region "aggiungi/modifica"
    Public Sub AggiungiFormazione()
        Dim oFormazione As New COL_IstruzioneFormazione
        Try
            If Me.DDLinizio.SelectedValue > Me.DDLfine.SelectedValue Then
                'Me.LBerrore.Text = "La data di fine è precedente la data di inizio"
                oResourceFormazione.setLabel_To_Value(LBerrore, "errore")
            Else
                Me.LBerrore.Text = ""
                With oFormazione
                    .inizio = Me.DDLinizio.SelectedValue
                    .fine = Me.DDLfine.SelectedValue
                    .nomeeTipoIstituto = Me.TXBnome.Text
                    .principaliMaterieAbilita = Me.TXBmaterie.Text
                    .qualificaConseguita = Me.TXBqualifica.Text
                    .livelloClassificazioneNazionale = Me.TXBclassificazione.Text
                    .RendiPubblico = Me.CBXrendiPubblico.Checked
                    .Curriculum_id = Me.HDNcreu_id.Value
                    .EsperienzaInCorso = Me.CBXinCorso.Checked
                End With
                If Me.HDNcreu_id.Value <> "" Then
                    oFormazione.Aggiungi()

                    Me.PNLinserimento.Visible = False
                    Me.PNLformazione.Visible = True
                    Me.Bind_Dati()
                    PulisciCampi()
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub InserisciFormazione()
        Me.PNLformazione.Visible = False
        Me.PNLinserimento.Visible = True
        Me.PulisciCampi()
    End Sub

    Public Sub AnnullaFormazione()
        Me.PNLformazione.Visible = True
        Me.PNLinserimento.Visible = False
        Me.Bind_Dati()
    End Sub
    Private Sub PulisciCampi()
        Me.DDLinizio.SelectedValue = Date.Now.Year
        Me.DDLfine.SelectedValue = Date.Now.Year
        Me.TXBnome.Text = ""
        Me.TXBmaterie.Text = ""
        Me.TXBqualifica.Text = ""
        Me.TXBclassificazione.Text = ""
        Me.CBXrendiPubblico.Checked = True
        ' Me.BTNaggiungi.Visible = True
        ' Me.BTNmodifica.Visible = False
        Me.LBerrore.Text = ""
        Me.CBXinCorso.Checked = False
    End Sub

    Public Sub ModificaFormazione()

        Dim oFormazione As New COL_IstruzioneFormazione
        Try
            If Me.DDLinizio.SelectedValue > Me.DDLfine.SelectedValue Then
                '  Me.LBerrore.Text = "La data di fine è precedente la data di inizio"
                oResourceFormazione.setLabel_To_Value(LBerrore, "errore")
            Else
                With oFormazione
                    .inizio = Me.DDLinizio.SelectedValue
                    .fine = Me.DDLfine.SelectedValue
                    .nomeeTipoIstituto = Me.TXBnome.Text
                    .principaliMaterieAbilita = Me.TXBmaterie.Text
                    .qualificaConseguita = Me.TXBqualifica.Text
                    .livelloClassificazioneNazionale = Me.TXBclassificazione.Text
                    .RendiPubblico = Me.CBXrendiPubblico.Checked
                    .Curriculum_id = Me.HDNcreu_id.Value
                    .ID = HDNisfr_id.Value
                    .EsperienzaInCorso = Me.CBXinCorso.Checked
                End With
                If Me.HDNisfr_id.Value <> "" Then
                    oFormazione.Modifica()

                    Me.PNLinserimento.Visible = False
                    Me.PNLformazione.Visible = True
                    Me.Bind_Dati()
                    PulisciCampi()
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

    Private Sub RPTformazione_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTformazione.ItemDataBound
        ' Dim oCell As New TableCell
        ' Dim oWebControl As WebControl
        '  oCell = CType(e.Item.Cell(0), TableCell)

        Dim oTableRow As New TableRow
        Try

            oTableRow = e.Item.FindControl("TBRmaterie")
            If IsDBNull(e.Item.DataItem("ISFR_principaliMaterieAbilita")) Then
                oTableRow.Visible = False
            ElseIf e.Item.DataItem("ISFR_principaliMaterieAbilita") = "" Then
                oTableRow.Visible = False
            Else
                oTableRow.Visible = True
            End If
        Catch ex As Exception

        End Try


        Dim oTableRow2 As New TableRow
        Try

            oTableRow2 = e.Item.FindControl("TBRqualifica")
            If IsDBNull(e.Item.DataItem("ISFR_qualificaConseguita")) Then
                oTableRow2.Visible = False
            ElseIf e.Item.DataItem("ISFR_qualificaConseguita") = "" Then
                oTableRow2.Visible = False
            Else
                oTableRow2.Visible = True

            End If
        Catch ex As Exception

        End Try

        Dim oTableRow3 As New TableRow
        Try

            oTableRow3 = e.Item.FindControl("TBRlivello")
            If IsDBNull(e.Item.DataItem("ISFR_livelloClassificazioneNazionale")) Then
                oTableRow3.Visible = False
            ElseIf e.Item.DataItem("ISFR_livelloClassificazioneNazionale") = "" Then
                oTableRow3.Visible = False
            Else
                oTableRow3.Visible = True
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CBXinCorso_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBXinCorso.CheckedChanged
        If Me.CBXinCorso.Checked = True Then
            Me.DDLfine.Enabled = False
        Else
            Me.DDLfine.Enabled = True
        End If
    End Sub

    
End Class
