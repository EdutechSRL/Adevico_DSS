Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.Comunita

Imports Comunita_OnLine.ModuloEnum

Public Class UC_PermessiServizio
    Inherits System.Web.UI.UserControl
    Private oResource As ResourceManager

  
    Public Enum Azione
        AssociaPermessi = 0
        Translate = 1
    End Enum
    Public ReadOnly Property ServizioID() As Integer
        Get
            Try
                ServizioID = Me.HDN_servizioID.Value
            Catch ex As Exception
                Me.HDN_servizioID.Value = 0
                ServizioID = 0
            End Try
        End Get
    End Property
    Public ReadOnly Property AzioneCorrente() As Azione
        Get
            Try
                AzioneCorrente = CType(Me.HDN_azione.Value, Azione)
            Catch ex As Exception

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
    Protected WithEvents HDN_servizioID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_azione As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_associati As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents DGpermessi As System.Web.UI.WebControls.DataGrid
    Protected WithEvents TBLdati As System.Web.UI.WebControls.Table
    Protected WithEvents TBLassociaPermessi As System.Web.UI.WebControls.Table
    Protected WithEvents LBinfoAssocia_t As System.Web.UI.WebControls.Label
    Protected WithEvents CBLpermessi As System.Web.UI.WebControls.CheckBoxList
    Protected WithEvents LBpermessi_t As System.Web.UI.WebControls.Label

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
        oResource.ResourcesName = "pg_UC_DatiServizio"
        oResource.Folder_Level1 = "Admin_Globale"
        oResource.Folder_Level2 = "UC"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(Me.LBinfoAssocia_t)
            .setLabel(Me.LBpermessi_t)
        End With
    End Sub
#End Region

#Region "Bind_Dati"
    Public Function Setup_Controllo(ByVal ServizioID As Integer, ByVal oAzione As Azione) As ModuloEnum.WizardServizio_Message
        Dim iResponse As WizardServizio_Message = WizardServizio_Message.NessunPermesso
        Dim oDataset As New DataSet

        Dim oServizio As New COL_Servizio
        Dim i, totale As Integer

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        Me.SetupInternazionalizzazione()
        Try
            Me.HDN_servizioID.Value = ServizioID
            Me.HDN_azione.Value = CType(oAzione, Azione)

            oServizio.ID = ServizioID
            Me.TBLassociaPermessi.Visible = False
            Me.TBLdati.Visible = False

            oDataset = oServizio.ElencaPermessiForDefinizione(Session("LinguaID"))
            totale = oDataset.Tables(0).Rows.Count

            If oAzione = Azione.AssociaPermessi Then
                Me.TBLassociaPermessi.Visible = True
                Me.CBLpermessi.Items.Clear()

                If totale > 0 Then
                    Me.CBLpermessi.DataSource = oDataset
                    Me.CBLpermessi.DataValueField = "PRMS_ID"
                    Me.CBLpermessi.DataTextField = "PRMS_nome"
                    Me.CBLpermessi.DataBind()

                    For i = 0 To totale - 1
                        If oDataset.Tables(0).Rows(i).Item("Associato") = 1 Then
                            Me.CBLpermessi.Items(i).Selected = True
                            Me.HDN_associati.Value &= Me.CBLpermessi.Items(i).Value & ","
                        End If
                    Next

                    If Me.HDN_associati.Value <> "" Then
                        Me.HDN_associati.Value = "," & Me.HDN_associati.Value
                    End If
                    Return WizardServizio_Message.OperazioneConclusa
                Else
                    Return WizardServizio_Message.NessunPermesso
                End If
            Else
                Me.TBLdati.Visible = True
                If totale > 0 Then
                    Dim oDataview As DataView
                    oDataview = oDataset.Tables(0).DefaultView
                    oDataview.RowFilter = "Associato=1"
                    oDataview.Sort = "PRMS_nome"
                    Me.DGpermessi.DataSource = oDataview
                    Me.DGpermessi.DataBind()
                    Me.DGpermessi.Visible = True
                    Return WizardServizio_Message.OperazioneConclusa
                Else
                    Me.DGpermessi.Visible = False
                    Return WizardServizio_Message.NessunPermesso
                End If
            End If
        Catch ex As Exception

        End Try
        Return iResponse
    End Function

    Public Function HasPermessiSelezionati() As Boolean
        Dim i, totale As Integer

        If Me.HDN_azione.Value = Azione.Translate Then
            totale = Me.DGpermessi.Items.Count
        Else
            totale = Me.CBLpermessi.Items.Count
        End If
        If Me.HDN_azione.Value = Azione.Translate Then
            If Me.DGpermessi.Items.Count > 0 Then
                Return True
            End If
        Else
            For i = 0 To totale - 1
                Try
                    If Me.CBLpermessi.Items(i).Selected Then
                        Return True
                    End If
                Catch ex As Exception

                End Try
            Next
        End If

        Return False
    End Function

    Private Function HasLinguaDefault(ByVal oRPTnome As Repeater) As Boolean
        Dim LinguaID, i, totale As Integer
        Dim Nome, Descrizione As String
        Dim NomeDefault, DescrizioneDefault As String

        totale = oRPTnome.Items.Count
        Try
            Dim oText As TextBox

            Try
                oText = oRPTnome.Items(0).FindControl("TXBtermine")
                If oText.Text = "" Then
                    Return False
                Else
                    If oText.Text.Trim = "" Then
                        Return False
                    Else
                        Return True
                    End If
                End If
            Catch ex As Exception
                Return False
            End Try
        Catch ex As Exception

        End Try
        Return True
    End Function

#End Region

    Private Sub RPTnome_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If e.Item.ItemType = ListItemType.Header Then
            Try
                oResource.setLabel(e.Item.FindControl("LBlinguaNome_t"))
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub RPTdescrizione_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If e.Item.ItemType = ListItemType.Header Then
            Try
                oResource.setLabel(e.Item.FindControl("LBlinguaDescrizione_t"))
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub DGpermessi_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DGpermessi.ItemDataBound
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        If e.Item.ItemType = ListItemType.Pager Then
            Dim oCell As TableCell
            Dim n As Integer
            oCell = CType(e.Item.Controls(0), TableCell)

            n = oCell.ColumnSpan
            ' Aggiungo riga con descrizione:

            'Try
            '    Dim oRow As TableRow
            '    Dim oTableCell As New TableCell
            '    Dim num As Integer = 0
            '    oRow = oCell.Parent()

            '    oTableCell.Controls.Add(Me.CreaLegenda)
            '    oTableCell.ColumnSpan = 2
            '    oTableCell.HorizontalAlign = HorizontalAlign.Left
            '    oCell.ColumnSpan = 1
            '    oRow.Cells.AddAt(0, oTableCell)
            'Catch ex As Exception

            'End Try


            For n = 0 To oCell.Controls.Count - 1 Step 2
                Dim szLnk As String
                szLnk = "System.Web.UI.WebControls.DataGridLinkButton"
                Dim oWebControl As WebControl

                oWebControl = oCell.Controls(n)

                If (oWebControl.GetType().ToString() = szLnk) Then
                    oWebControl.CssClass = "ROW_PagerLink_Small"
                End If
                Try
                    Dim oLabel As Label
                    oLabel = oWebControl
                    oLabel.Text = oLabel.Text
                    oLabel.CssClass = "ROW_PagerSpan_Small"
                Catch ex As Exception
                    Dim oLinkbutton As LinkButton
                    oLinkbutton = oWebControl
                    oLinkbutton.CssClass = "ROW_PagerLink_Small"
                    oResource.setPageDatagrid(Me.DGpermessi, oLinkbutton)
                End Try
            Next
        End If

        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim cssLink As String = "ROW_ItemLink_Small"
            Dim cssRiga As String = "ROW_TD_Small"

            Try
                If CBool(e.Item.DataItem("TPRL_noDelete")) = True Then
                    e.Item.CssClass = "ROW_Disattivate_Small"
                    cssLink = "ROW_ItemLinkDisattivate_Small"
                    cssRiga = "ROW_ItemDisattivate_Small"
                ElseIf CBool(e.Item.DataItem("associato")) = True Then
                    e.Item.CssClass = "ROW_Disabilitate_Small"
                ElseIf e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.CssClass = "ROW_Alternate_Small"
                Else
                    e.Item.CssClass = "ROW_Normal_Small"
                End If
            Catch ex As Exception
                If e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.CssClass = "ROW_Alternate_Small"
                Else
                    e.Item.CssClass = "ROW_Normal_Small"
                End If
            End Try


            Try
                Dim oLBpermesso As Label

                oLBpermesso = e.Item.Cells(1).FindControl("LBpermesso")
                If IsNothing(oLBpermesso) = False Then
                    oLBpermesso.CssClass = cssRiga
                    oLBpermesso.Text = e.Item.DataItem("PRMS_Nome")

                End If
            Catch ex As Exception

            End Try
            Try
                Dim oLBprms_ID As Label

                oLBprms_ID = e.Item.Cells(1).FindControl("LBprms_ID")
                If IsNothing(oLBprms_ID) = False Then
                    oLBprms_ID.CssClass = cssRiga
                    oLBprms_ID.Text = e.Item.DataItem("PRMS_ID")

                End If
            Catch ex As Exception

            End Try

            'Try
            '    Dim oCBXselezionato As CheckBox

            '    oCBXselezionato = e.Item.Cells(1).FindControl("CBXselezionato")
            '    If IsNothing(oCBXselezionato) = False Then
            '        oCBXselezionato.CssClass = cssRiga
            '        oCBXselezionato.Checked = (e.Item.DataItem("Associato") = 1)
            '    End If
            'Catch ex As Exception

            'End Try
            Try
                Dim oRPTnome, oRPTdescrizione As Repeater
                Dim oDataset As DataSet
                Dim oServizio As New COL_Servizio
                oRPTnome = e.Item.Cells(1).FindControl("RPTnome")
                oRPTdescrizione = e.Item.Cells(1).FindControl("RPTdescrizione")
                If IsNothing(oRPTnome) = False Then
                    AddHandler oRPTnome.ItemCreated, AddressOf RPTnome_ItemCreated
                End If
                If IsNothing(oRPTdescrizione) = False Then
                    AddHandler oRPTdescrizione.ItemCreated, AddressOf RPTdescrizione_ItemCreated
                End If
                Dim i, totale As Integer
                oServizio.ID = Me.HDN_servizioID.Value
                oDataset = oServizio.ElencaDefinizioniLinguePermessi(e.Item.DataItem("PRMS_ID"))
                For i = 0 To oDataset.Tables(0).Rows.Count - 1
                    If IsDBNull(oDataset.Tables(0).Rows(i).Item("Nome")) Then
                        oDataset.Tables(0).Rows(i).Item("Nome") = ""
                    Else
                        Me.HDN_definito.Value = True
                    End If
                    If IsDBNull(oDataset.Tables(0).Rows(i).Item("Descrizione")) Then
                        oDataset.Tables(0).Rows(i).Item("Descrizione") = ""
                    Else
                        If oDataset.Tables(0).Rows(i).Item("Descrizione") = "" Then

                        End If
                    End If
                    Try
                        If oDataset.Tables(0).Rows(i).Item("LNGU_Default") = True Then
                            oDataset.Tables(0).Rows(i).Item("LNGU_nome") &= "(<b>d</b>)"
                        End If
                    Catch ex As Exception

                    End Try
                Next
                oRPTnome.DataSource = oDataset
                oRPTnome.DataBind()
                oRPTdescrizione.DataSource = oDataset
                oRPTdescrizione.DataBind()
            Catch ex As Exception

            End Try
        End If
    End Sub


    Public Function Salva_Dati() As WizardServizio_Message
        Dim iResponse As WizardServizio_Message = WizardServizio_Message.ErroreGenerico
        Dim oServizio As New COL_Servizio

        Try
            oServizio.ID = Me.HDN_servizioID.Value
            If Me.HasPermessiSelezionati = False Then
                Return WizardServizio_Message.SelezionaPermesso
            Else
                Dim i, totale, PermessoID As Integer
                If Me.HDN_azione.Value = Azione.AssociaPermessi Then
                    totale = Me.CBLpermessi.Items.Count
                    If totale = 0 Then
                        Return WizardServizio_Message.NessunPermesso
                    Else
                        iResponse = WizardServizio_Message.OperazioneConclusa
                        For i = 0 To totale - 1
                            Dim oListitem As ListItem
                            oListitem = Me.CBLpermessi.Items(i)

                            If InStr(Me.HDN_associati.Value, "," & oListitem.Value & ",") > 0 Then
                                If Not oListitem.Selected Then
                                    oServizio.DisassociaPermesso(PermessoID)
                                    If oServizio.Errore <> Errori_Db.None Then
                                        iResponse = WizardServizio_Message.ErroreAssociazionePermessi
                                    End If
                                End If
                            Else
                                If oListitem.Selected Then
                                    oServizio.AssociaPermesso(oListitem.Value, oListitem.Text, "")
                                    If oServizio.Errore <> Errori_Db.None Then
                                        iResponse = WizardServizio_Message.ErroreAssociazionePermessi
                                    End If
                                End If
                            End If
                        Next
                    End If
                Else
                    Dim noDefault As Boolean = False
                    totale = Me.DGpermessi.Items.Count
                    For i = 0 To totale - 1
                        Dim oRow As DataGridItem
                        Dim oLBprms_ID As Label
                        oRow = Me.DGpermessi.Items(i)
                        oLBprms_ID = oRow.FindControl("LBpermesso")

                        oLBprms_ID = oRow.FindControl("LBprms_ID")
                        Try
                            PermessoID = oLBprms_ID.Text
                            If PermessoID <= 0 Then
                                PermessoID = 0
                            End If
                        Catch ex As Exception
                            PermessoID = 0
                        End Try
                        If PermessoID > 0 Then
                            'If oSelezionato.Checked Then
                            Dim oRPTnome, oRPTdescrizione As Repeater
                            oRPTnome = oRow.FindControl("RPTnome")
                            oRPTdescrizione = oRow.FindControl("RPTdescrizione")

                            If HasLinguaDefault(oRPTnome) Then
                                iResponse = Me.Salva_DefinizioniLingue(PermessoID, oRPTnome, oRPTdescrizione)
                            Else
                                noDefault = True
                            End If
                            If noDefault Then
                                iResponse = WizardServizio_Message.DefinireLinguaDefault
                            Else
                                Me.HDN_definito.Value = True
                            End If
                        Else
                            Return WizardServizio_Message.ErroreGenerico
                        End If
                    Next
                End If
            End If
        Catch ex As Exception

        End Try
        Return iResponse
    End Function


    Private Function Salva_DefinizioniLingue(ByVal PermessoID As Integer, ByVal oRPTnome As Repeater, ByVal oRPTdescrizione As Repeater) As WizardServizio_Message
        Dim LinguaID, i, totale As Integer
        Dim Nome, Descrizione As String
        Dim NomeDefault, DescrizioneDefault As String
        Dim oServizio As New COL_Servizio

        oServizio.ID = Me.HDN_servizioID.Value

        totale = oRPTnome.Items.Count
        Try
            If totale > 0 Then
                For i = 0 To totale - 1
                    Dim oLabel As Label
                    Dim oTextNome As TextBox
                    Dim oTextDescrizione As TextBox


                    Try
                        oLabel = oRPTnome.Items(i).FindControl("LBlinguaID")
                        LinguaID = oLabel.Text
                    Catch ex As Exception
                        LinguaID = 0
                    End Try
                    If LinguaID > 0 Then
                        Try
                            oTextNome = oRPTnome.Items(i).FindControl("TXBtermine")
                            Nome = oTextNome.Text
                        Catch ex As Exception
                            Nome = ""
                        End Try

                        Try
                            oLabel = oRPTnome.Items(i).FindControl("LBdefault")
                            If CBool(oLabel.Text) Then
                                NomeDefault = Nome
                            End If
                        Catch ex As Exception

                        End Try
                        If Nome = "" Then
                            Nome = NomeDefault
                            Try
                                oTextNome.Text = Nome
                            Catch ex As Exception

                            End Try
                        End If

                        Try
                            oTextDescrizione = oRPTdescrizione.Items(i).FindControl("TXBtermine2")
                            Descrizione = oTextDescrizione.Text
                        Catch ex As Exception
                            Descrizione = ""
                        End Try
                        Try
                            oLabel = oRPTdescrizione.Items(i).FindControl("LBdefault2")
                            If CBool(oLabel.Text) Then
                                DescrizioneDefault = Descrizione
                            End If
                        Catch ex As Exception

                        End Try
                        If Descrizione = "" Then
                            Descrizione = DescrizioneDefault
                            Try
                                oTextDescrizione.Text = Descrizione
                            Catch ex As Exception

                            End Try
                        End If
                        If NomeDefault <> "" Then
                            oServizio.TranslatePermessoAssociato(LinguaID, PermessoID, Nome, Descrizione)
                        End If
                    End If
                Next
                Return WizardServizio_Message.OperazioneConclusa
            Else
                Return WizardServizio_Message.ErroreAssociazioneLingue
            End If
        Catch ex As Exception
            Return WizardServizio_Message.ErroreAssociazioneLingue
        End Try
    End Function
End Class