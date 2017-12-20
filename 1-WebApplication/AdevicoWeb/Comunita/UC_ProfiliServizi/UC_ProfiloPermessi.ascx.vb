Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita

Imports COL_BusinessLogic_v2.CL_permessi

Public Class UC_ProfiloPermessi
    Inherits System.Web.UI.UserControl

    Private oResource As ResourceManager

    Public ReadOnly Property ProfiloID() As Integer
        Get
            Try
                ProfiloID = Me.HDN_profiloID.Value
            Catch ex As Exception
                Me.HDN_profiloID.Value = 0
                ProfiloID = 0
            End Try
        End Get
    End Property
    Public ReadOnly Property isDefinito() As Boolean
        Get
            Try
                isDefinito = (Me.HDN_definito.Value = True)
            Catch ex As Exception
                isDefinito = False
            End Try
        End Get
    End Property

    Protected WithEvents HDN_definito As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HIDcheckbox As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_profiloID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_TPCM_ID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents LBserviziPermessi_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLserviziPermessi As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TBLpermessiRuoli As System.Web.UI.WebControls.Table
    Public Event AggiornaMenuServizi(ByVal AttivaDefault As Boolean, ByVal Abilitato As Boolean)


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
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
    End Sub

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal Code As String)
        Me.oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_UC_DatiProfilo"
        oResource.Folder_Level1 = "Comunita"
        oResource.Folder_Level2 = "UC_ProfiliServizi"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(Me.LBserviziPermessi_t)
        End With
    End Sub
#End Region

#Region "Bind Dati"
    Public Function Setup_Controllo(ByVal ProfiloID As Integer) As WizardProfilo_Message
        Dim iResponse As WizardProfilo_Message = WizardProfilo_Message.ProfiloNonTrovato

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        Me.SetupInternazionalizzazione()

        Try

            Me.HDN_profiloID.Value = ProfiloID
            Me.HDN_TPCM_ID.Value = -1
            If ProfiloID = 0 Then
                Return iResponse
            Else
                Dim oProfilo As New COL_ProfiloServizio
                oProfilo.Id = ProfiloID
                oProfilo.Estrai()
                If oProfilo.Errore = Errori_Db.None Then
                    Me.HDN_TPCM_ID.Value = oProfilo.TipoComunitaID
                    Me.Bind_Servizi(Me.HDN_TPCM_ID.Value)
                    Try
                        If Me.DDLserviziPermessi.Items.Count > 0 Then
                            Me.DDLserviziPermessi.SelectedIndex = 0
                            Me.Bind_PermessiRuoliServizi(Me.DDLserviziPermessi.SelectedValue, Me.HDN_TPCM_ID.Value, True)
                            RaiseEvent AggiornaMenuServizi(True, True)
                            iResponse = WizardProfilo_Message.OperazioneConclusa
                        Else
                            Me.TBLpermessiRuoli.Rows.Clear()
                            RaiseEvent AggiornaMenuServizi(False, False)
                        End If
                    Catch ex As Exception

                    End Try
                End If
            End If
        Catch ex As Exception
            Return WizardProfilo_Message.NessunServizio
        End Try
        Return iResponse
    End Function

    Private Sub Bind_Servizi(ByVal TipoComunitaID As Integer)
        Dim oProfilo As New COL_ProfiloServizio
        Dim oDataset As DataSet

        Try
            oProfilo.Id = Me.HDN_profiloID.Value
            oProfilo.TipoComunitaID = TipoComunitaID

            oDataset = oProfilo.ElencaServiziAssociati(Session("LinguaID"))
            Me.DDLserviziPermessi.DataSource = oDataset
            Me.DDLserviziPermessi.DataTextField = "SRVZ_Nome"
            Me.DDLserviziPermessi.DataValueField = "SRVZ_ID"
            Me.DDLserviziPermessi.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub DDLserviziPermessi_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLserviziPermessi.SelectedIndexChanged
        If Me.DDLserviziPermessi.Items.Count > 0 Then
            Me.Bind_PermessiRuoliServizi(Me.DDLserviziPermessi.SelectedValue, Me.HDN_TPCM_ID.Value, True)
            RaiseEvent AggiornaMenuServizi(True, True)
        Else
            RaiseEvent AggiornaMenuServizi(True, False)
        End If
    End Sub

    Private Sub Bind_PermessiRuoliServizi(ByVal ServizioID As Integer, ByVal TipoComunitaID As Integer, Optional ByVal start As Boolean = False)
        Dim oServizio As New COL_Servizio
        Dim oDataset As New DataSet
        Dim oDatasetRuoli As New DataSet

        oServizio.ID = ServizioID
        Try
            Dim i, j, totalePermessi, totale, PRMS_Posizione As Integer
            Dim ARRpermServizio As Integer() = Nothing
            Dim oTBrow As New TableRow
            Dim oStringaPermessi As String
            oDataset = oServizio.ElencaPermessiAssociatiByLingua(Session("LinguaID"))
            totalePermessi = oDataset.Tables(0).Rows.Count - 1

            'Caricamento riga con nome permessi !

            Dim oTableCell As New TableCell
            oTableCell.Text = "&nbsp;"
            oTableCell.BackColor = System.Drawing.Color.Lavender
            oTBrow.Cells.Add(oTableCell)
            For i = 0 To totalePermessi
                Dim oRow As DataRow
                Dim oLabel As New Label
                oTableCell = New TableCell
                oTableCell.HorizontalAlign = HorizontalAlign.Center

                oRow = oDataset.Tables(0).Rows(i)

                Try
                    If IsDBNull(oRow.Item("Nome")) Then
                        oRow.Item("Nome") = oRow.Item("NomeDefault")
                    End If
                Catch ex As Exception

                End Try

                Dim showDescrizione As Boolean = False
                Try
                    If IsDBNull(oRow.Item("Descrizione")) Then
                        oRow.Item("Descrizione") = oRow.Item("DescrizioneDefault")
                    End If
                    If oRow.Item("Descrizione") <> "" Then
                        oRow.Item("Descrizione") = Replace(oRow.Item("Descrizione"), "'", "\'")
                        showDescrizione = True
                    End If
                Catch ex As Exception

                End Try
                'oTableCell.Text = oRow.Item("Nome")
                oLabel.Text = oRow.Item("Nome")
                If showDescrizione Then
                    oLabel.Attributes.Add("onmouseover", "ChangeState(event,'layer1','visible','" & oRow.Item("Descrizione") & "')")
                    oLabel.Attributes.Add("onmouseout", "ChangeState(event,'layer1','hidden','')")
                End If

                oTableCell.Controls.Add(oLabel)
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
            Me.TBLpermessiRuoli.Rows.Add(oTBrow)


            Dim oProfilo As New COL_ProfiloServizio
            oProfilo.Id = ProfiloID
            oDatasetRuoli = oProfilo.ElencaPermessi(ServizioID, Me.HDN_TPCM_ID.Value, Session("LinguaID"))
            totale = oDatasetRuoli.Tables(0).Rows.Count - 1

            If start Then
                Me.HIDcheckbox.Value = ""
            End If
            Dim uniqueID As String
            For i = 0 To totale
                oTableCell = New TableCell
                Dim oRow As DataRow
                oTBrow = New TableRow

                oRow = oDatasetRuoli.Tables(0).Rows(i)

                Try
                    If IsDBNull(oRow.Item("Permessi")) Then
                        oRow.Item("Permessi") = oRow.Item("PermessiDefault")
                    End If
                Catch ex As Exception
                    oRow.Item("Permessi") = "00000000000000000000000000000000"
                End Try
                oStringaPermessi = oRow.Item("Permessi")

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
                        Else
                            oCheckbox.Checked = False
                        End If
                    End If
                    oCheckbox.Attributes.Add("onclick", "SelectFromNameAndAssocia('" & Me.ClientID & ":" & oCheckbox.ClientID & "','" & uniqueID & "');return true;")
                    oTableCell.Controls.Add(oCheckbox)
                    oTBrow.Cells.Add(oTableCell)
                Next
                Me.TBLpermessiRuoli.Rows.Add(oTBrow)
            Next
        Catch ex As Exception

        End Try
    End Sub
#End Region

    Public Function SalvaDati(Optional ByVal Replica As Boolean = False) As WizardProfilo_Message ', Optional ByVal DefaultValue As Boolean = False)
        Dim oProfilo As New COL_ProfiloServizio
        Dim iResponse As WizardProfilo_Message = WizardProfilo_Message.ErroreGenerico

        Try
            If Me.HDN_profiloID.Value > -1 Then
                oProfilo.Id = Me.HDN_profiloID.Value
                oProfilo.Estrai()
                If oProfilo.Errore <> Errori_Db.None Then
                    Return WizardProfilo_Message.ErroreGenerico
                End If
            Else
                Return WizardProfilo_Message.ErroreGenerico
            End If
        Catch ex As Exception
            Return WizardProfilo_Message.ErroreGenerico
        End Try
        Me.Bind_PermessiRuoliServizi(Me.DDLserviziPermessi.SelectedValue, oProfilo.TipoComunitaID, False)
        'If DefaultValue Then
        '    If Me.DDLserviziPermessi.Items.Count = 0 Then
        '        iResponse = WizardProfilo_Message.NessunServizio
        '    Else
        '        '    Dim oTipoComunita As New COL_Tipo_Comunita
        '        '    oTipoComunita.ID = Me.DDLtipocomunita.SelectedValue
        '        '    oTipoComunita.DefinisciPermessiRuoliDefault(Me.DDLorganizzazionePermessi.SelectedValue, Me.HDN_servizioID.Value)
        '        '    Me.TBLpermessiRuoli.Rows.Clear()
        '        '    Me.Bind_PermessiRuoliServizi(Me.HDN_servizioID.Value, Me.DDLorganizzazionePermessi.SelectedValue, Me.DDLtipocomunita.SelectedValue, True)
        '        '    Return WizardProfilo_Message.PermessiAssociati
        '    End If
        'Else
        iResponse = Me.AssociaPermessiRuoli(Replica)
        'End If
        Return iResponse
    End Function

    Private Function AssociaPermessiRuoli(ByVal Replica As Boolean) As WizardProfilo_Message
        Dim oProfilo As New COL_ProfiloServizio
        Dim iResponse As WizardProfilo_Message = WizardProfilo_Message.ErroreGenerico

        oProfilo.Id = Me.HDN_profiloID.Value
        oProfilo.Estrai()
        If oProfilo.Errore <> Errori_Db.None Then
            Return WizardProfilo_Message.ProfiloNonTrovato
        Else
            Try
                Dim i, j, TPRL_ID, totaleV, totaleO, Posizione, Associati, Totali As Integer
                Dim oStringaPermessi, nome() As String
                Dim Permessi() As Char
                Dim oTableCell As TableCell

                totaleV = Me.TBLpermessiRuoli.Rows.Count - 1
                totaleO = Me.TBLpermessiRuoli.Rows(0).Cells.Count - 1
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
                        oProfilo.DefinisciPermessiRuoli(Me.DDLserviziPermessi.SelectedValue, TPRL_ID, oStringaPermessi, Replica)
                        If oProfilo.Errore = Errori_Db.None Then
                            Associati += 1
                        End If
                        Totali += 1
                    End If
                Next
                If Totali > 0 Then
                    If Totali = Associati Then
                        iResponse = WizardProfilo_Message.PermessiDefiniti
                    ElseIf Associati > 0 Then
                        iResponse = WizardProfilo_Message.PermessiAssociatiParziali
                    Else
                        iResponse = WizardProfilo_Message.NONModificato
                    End If
                End If
            Catch ex As Exception

            End Try
        End If
        Return iResponse
    End Function

End Class