Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.Comunita

Imports Comunita_OnLine.ModuloEnum

Public Class UC_AdminServizioTraduzionePermessi
    Inherits System.Web.UI.UserControl
    Private oResource As ResourceManager

    Dim oArrayTranslate(,) As Object

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
    'Public ReadOnly Property isDefinito() As Boolean
    '    Get
    '        Try
    '            isDefinito = (Me.HDN_definito.Value = True)
    '        Catch ex As Exception
    '            isDefinito = False
    '        End Try
    '    End Get
    'End Property
    Public ReadOnly Property isInizializzato() As Boolean
        Get
            Try
                isInizializzato = HDNhasSetup.Value
            Catch ex As Exception
                isInizializzato = False
            End Try
        End Get
    End Property
    Public ReadOnly Property isDefaultLanguageDefinito() As Boolean
        Get
            Try
                Dim i, totale, definito As Integer
                totale = Me.RPTpermessi.Items.Count
                If totale = 0 Then
                    isDefaultLanguageDefinito = False
                Else
                    For i = 0 To totale - 1
                        Dim oRow As RepeaterItem
                        Dim oRPTnome As Repeater

                        oRow = Me.RPTpermessi.Items(i)
                        oRPTnome = oRow.FindControl("RPTnome")
                        If isPermessoDefinitoDefault(oRPTnome) Then
                            definito += 1
                        End If
                    Next
                    If definito = totale Then
                        isDefaultLanguageDefinito = True
                    Else
                        isDefaultLanguageDefinito = False
                    End If
                End If
            Catch ex As Exception
                isDefaultLanguageDefinito = False
            End Try
        End Get
    End Property

    Public ReadOnly Property TraduzionePermessiDefault() As Hashtable
        Get
            Try
                Dim oLista As New Hashtable
                Dim i, totale, PermessoID As Integer

                totale = Me.RPTpermessi.Items.Count
                For i = 0 To totale - 1
                    Dim oRow As RepeaterItem
                    Dim oLBprms_ID As Label
                    oRow = Me.RPTpermessi.Items(i)

                    oLBprms_ID = oRow.FindControl("LBprms_ID")
                    Try
                        PermessoID = CInt(oLBprms_ID.Text)
                        If PermessoID > 0 Then
                            Dim oRPTnome, oRPTdescrizione As Repeater
                            oRPTnome = oRow.FindControl("RPTnome")

                            oLista.Add(PermessoID, Me.GetDefinizioneLinguaDefault(PermessoID, oRPTnome))
                        End If
                    Catch ex As Exception

                    End Try
                Next
                TraduzionePermessiDefault = oLista
            Catch ex As Exception
                TraduzionePermessiDefault = Nothing
            End Try
        End Get
    End Property
    Protected WithEvents HDNhasSetup As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_definito As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_servizioID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_associati As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents TBLdati As System.Web.UI.WebControls.Table
    Protected WithEvents RPTpermessi As System.Web.UI.WebControls.Repeater

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
#End Region

#Region "Bind_Dati"


    Public Function Update_Controllo(ByVal ServizioID As Integer, ByVal PermessiSelezionati As String) As ModuloEnum.WizardServizio_Message
        Dim iResponse As WizardServizio_Message = WizardServizio_Message.NessunPermesso
        Dim oDataset As New DataSet

        Dim oServizio As New COL_Servizio
        Dim i, totale As Integer

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        Try
            Me.HDN_servizioID.Value = ServizioID

            oServizio.ID = ServizioID
            Me.TBLdati.Visible = False

            oDataset = oServizio.ElencaPermessiForDefinizione(Session("LinguaID"))
            totale = oDataset.Tables(0).Rows.Count

            Me.TBLdati.Visible = True
            If totale > 0 Then
                Dim oDataview As DataView

                If Not oDataset.Tables(0).Columns.Contains("CssClass") Then
                    oDataset.Tables(0).Columns.Add(New DataColumn("CssClass"))
                End If
              
                oDataview = oDataset.Tables(0).DefaultView


                Me.RidefinisciLingue(PermessiSelezionati)
                PermessiSelezionati = Mid(PermessiSelezionati, 2, PermessiSelezionati.Length - 2)
                PermessiSelezionati = Replace(PermessiSelezionati, ",", " OR PRMS_ID=")
                oDataview.RowFilter = "PRMS_ID=" & PermessiSelezionati
                oDataview.Sort = "PRMS_nome"

                For i = 0 To oDataview.Count - 1
                    If i Mod 2 = 0 Then
                        oDataview.Item(i).Item("CssClass") = "ROWrepeater_Normal_Small"
                    Else
                        oDataview.Item(i).Item("CssClass") = "ROWrepeater_Alternate_Small"
                    End If
                Next
                Me.RPTpermessi.DataSource = oDataview
                Me.RPTpermessi.DataBind()
                Me.RPTpermessi.Visible = True
                Me.HDNhasSetup.Value = True
                Return WizardServizio_Message.OperazioneConclusa
            Else
                Me.HDNhasSetup.Value = False
                Me.RPTpermessi.Visible = False
                Return WizardServizio_Message.NessunPermesso
            End If
        Catch ex As Exception

        End Try
        Return iResponse
    End Function

    Public Function Setup_Controllo(ByVal ServizioID As Integer, ByVal PermessiSelezionati As String) As ModuloEnum.WizardServizio_Message
        Dim iResponse As WizardServizio_Message = WizardServizio_Message.NessunPermesso
        Dim oDataset As New DataSet

        Dim oServizio As New COL_Servizio
        Dim i, totale As Integer

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        Try
            Me.HDN_servizioID.Value = ServizioID

            oServizio.ID = ServizioID
            Me.TBLdati.Visible = False

            oDataset = oServizio.ElencaPermessiForDefinizione(Session("LinguaID"))
            totale = oDataset.Tables(0).Rows.Count

            Me.TBLdati.Visible = True
            If totale > 0 Then
                Dim oDataview As DataView

                oDataset.Tables(0).Columns.Add(New DataColumn("cssClass"))
                oDataview = oDataset.Tables(0).DefaultView
                If PermessiSelezionati <> "" Then
                    PermessiSelezionati = Mid(PermessiSelezionati, 2, PermessiSelezionati.Length - 2)
                    PermessiSelezionati = Replace(PermessiSelezionati, ",", " OR PRMS_ID=")
                    oDataview.RowFilter = "PRMS_ID=" & PermessiSelezionati

                    oDataview.Sort = "PRMS_nome"

                    For i = 0 To oDataview.Count - 1
                        If i Mod 2 = 0 Then
                            oDataview.Item(i).Item("CssClass") = "ROWrepeater_Normal_Small"
                        Else
                            oDataview.Item(i).Item("CssClass") = "ROWrepeater_Alternate_Small"
                        End If
                    Next
                    Me.RPTpermessi.DataSource = oDataview
                    Me.RPTpermessi.DataBind()
                    Me.RPTpermessi.Visible = True
                    Me.HDNhasSetup.Value = True
                    Return WizardServizio_Message.OperazioneConclusa
                Else
                    Return WizardServizio_Message.NessunPermesso
                End If
            Else
                Me.HDNhasSetup.Value = False
                Me.RPTpermessi.Visible = False
                Return WizardServizio_Message.NessunPermesso
            End If
        Catch ex As Exception

        End Try
        Return iResponse
    End Function

    Public Function HasPermessi() As Boolean
        If Me.RPTpermessi.Items.Count > 0 Then
            Return True
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

    Private Sub RPTpermessi_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTpermessi.ItemDataBound
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        If (e.Item.ItemType = ListItemType.Item) OrElse (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim cssLink As String = "ROW_ItemLink_Small"
            Dim cssRiga As String = "ROW_TD_Small"

            'Try
            '    If e.Item.ItemType = ListItemType.AlternatingItem Then
            '        e.Item.DataItem("CssClass") = "ROWrepeater_Alternate_Small"
            '    Else
            '        e.Item.DataItem("CssClass") = "ROWrepeater_Normal_Small"
            '    End If

            'Catch ex As Exception

            'End Try

            Try
                Dim oLBpermesso As Label

                oLBpermesso = e.Item.FindControl("LBpermesso")
                If IsNothing(oLBpermesso) = False Then
                    oLBpermesso.CssClass = cssRiga
                    oLBpermesso.Text = e.Item.DataItem("PRMS_Nome")

                End If
            Catch ex As Exception

            End Try
            Try
                Dim oLBprms_ID As Label

                oLBprms_ID = e.Item.FindControl("LBprms_ID")
                If IsNothing(oLBprms_ID) = False Then
                    oLBprms_ID.CssClass = cssRiga
                    oLBprms_ID.Text = e.Item.DataItem("PRMS_ID")

                End If
            Catch ex As Exception

            End Try
            Try
                Dim oRPTnome, oRPTdescrizione As Repeater
                Dim oDataset As DataSet
                Dim oServizio As New COL_Servizio
                oRPTnome = e.Item.FindControl("RPTnome")
                oRPTdescrizione = e.Item.FindControl("RPTdescrizione")
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
                    Dim oRow As DataRow
                    oRow = oDataset.Tables(0).Rows(i)
                    If IsDBNull(oRow.Item("Nome")) Then
                        oRow.Item("Nome") = ""
                    Else
                        Me.HDN_definito.Value = True
                    End If

                    If IsDBNull(oRow.Item("Descrizione")) Then
                        oRow.Item("Descrizione") = ""
                    Else
                        If oRow.Item("Descrizione") = "" Then

                        End If
                    End If
                    If Not IsNothing(oArrayTranslate) Then
                        Try
                            Dim j As Integer
                            Dim found As Boolean = False
                            For j = 0 To UBound(oArrayTranslate, 2)
                                If oArrayTranslate(0, j) = e.Item.DataItem("PRMS_ID") Then
                                    Dim oArrayLingua(,) As String
                                    Try
                                        Dim pos, totaleLingua As Integer
                                        oArrayLingua = oArrayTranslate(1, j)

                                        totaleLingua = UBound(oArrayLingua, 2)
                                        For pos = 0 To totaleLingua
                                            If oArrayLingua(0, pos) = oRow.Item("LNGU_ID") Then
                                                oRow.Item("Descrizione") = oArrayLingua(2, pos)
                                                oRow.Item("Nome") = oArrayLingua(1, pos)
                                                found = True
                                                Exit For
                                            End If
                                        Next
                                    Catch exLingua As Exception
                                        Exit For
                                    End Try
                                End If
                                If found Then
                                    Exit For
                                End If
                            Next
                        Catch ex As Exception

                        End Try
                    End If
                    Try
                        If oRow.Item("LNGU_Default") = True Then
                            oRow.Item("LNGU_nome") &= "(<b>d</b>)"
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


    Public Function Salva_Dati(ByVal ServizioID As Integer) As WizardServizio_Message
        Dim iResponse As WizardServizio_Message = WizardServizio_Message.ErroreGenerico
        Dim oServizio As New COL_Servizio

        Try
            Me.HDN_servizioID.Value = ServizioID
            oServizio.ID = Me.HDN_servizioID.Value
            If Me.HasPermessi = False Then
                Return WizardServizio_Message.SelezionaPermesso
            Else
                Dim i, totale, PermessoID As Integer
                Dim noDefault As Boolean = False
                totale = Me.RPTpermessi.Items.Count
                For i = 0 To totale - 1
                    Dim oRow As RepeaterItem
                    Dim oLBprms_ID As Label
                    oRow = Me.RPTpermessi.Items(i)
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

    Private Sub RidefinisciLingue(ByRef Permessi As String)
        Try
            Dim i, totale, PermessoID, IndicePermesso As Integer
            Dim noDefault As Boolean = False
            totale = Me.RPTpermessi.Items.Count
            For i = 0 To totale - 1
                Dim oRow As RepeaterItem
                Dim oLBprms_ID As Label
                oRow = Me.RPTpermessi.Items(i)
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
                If PermessoID > 0 And InStr(Permessi, "," & PermessoID & ",") > 0 Then
                    Dim oRPTnome, oRPTdescrizione As Repeater
                    oRPTnome = oRow.FindControl("RPTnome")
                    oRPTdescrizione = oRow.FindControl("RPTdescrizione")

                    ReDim Preserve oArrayTranslate(1, IndicePermesso)
                    oArrayTranslate(0, IndicePermesso) = PermessoID

                    Me.AggiornaDefinizioniLingue(oArrayTranslate, IndicePermesso, oRPTnome, oRPTdescrizione)

                    IndicePermesso += 1
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub
    Private Sub AggiornaDefinizioniLingue(ByVal oArray(,) As Object, ByVal IndicePermesso As Integer, ByVal oRPTnome As Repeater, ByVal oRPTdescrizione As Repeater)
        Dim LinguaID, i, totale As Integer
        Dim Nome, Descrizione As String
        Dim NomeDefault, DescrizioneDefault As String

        totale = oRPTnome.Items.Count
        Try
            If totale > 0 Then
                Dim oArrayLingua(,) As String
                Dim indiceLingua As Integer = 0
                For i = 0 To totale - 1
                    Dim oLabel As Label
                    Dim oTextNome As TextBox
                    Dim oTextDescrizione As TextBox

                    ReDim Preserve oArrayLingua(2, indiceLingua)
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
                        oArrayLingua(0, indiceLingua) = LinguaID
                        oArrayLingua(1, indiceLingua) = Nome
                        oArrayLingua(2, indiceLingua) = Descrizione
                        indiceLingua += 1
                    End If
                Next
                oArray(1, IndicePermesso) = oArrayLingua
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Function isPermessoDefinitoDefault(ByVal oRPTnome As Repeater) As Boolean
        Dim LinguaID, i, totale As Integer
        Dim Nome, Descrizione As String
        Dim NomeDefault, DescrizioneDefault As String

        totale = oRPTnome.Items.Count
        Try
            If totale > 0 Then
                Dim oArrayLingua(,) As String

                For i = 0 To totale - 1
                    Dim oLabel As Label
                    Dim oTextNome As TextBox

                    ReDim Preserve oArrayLingua(1, i)
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
                            If CBool(oLabel.Text) And Nome <> "" Then
                                Return True
                            End If
                        Catch ex As Exception

                        End Try
                    End If
                Next
            End If
        Catch ex As Exception

        End Try
        Return False
    End Function

    Private Function GetDefinizioneLinguaDefault(ByVal PermessoID As Integer, ByVal oRPTnome As Repeater) As String
        Dim LinguaID, i, totale As Integer
        Dim Nome As String = ""
        Dim Descrizione As String = ""
        Dim NomeDefault As String = ""
        Dim iResponse As String = ""

        totale = oRPTnome.Items.Count
        Try
            If totale > 0 Then
                Dim HasDefault As Boolean = False
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
                                HasDefault = True
                            End If
                        Catch ex As Exception

                        End Try
                        iResponse = Nome 
                        If HasDefault Then
                            Exit For
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
        End Try
        Return iResponse
    End Function

End Class