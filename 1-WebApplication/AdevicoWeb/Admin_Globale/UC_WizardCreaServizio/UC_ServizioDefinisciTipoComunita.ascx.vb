
Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita

Imports COL_BusinessLogic_v2.CL_permessi

Public Class UC_ServizioDefinisciTipoComunita
    Inherits System.Web.UI.UserControl
    Private oResource As ResourceManager

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
    Public ReadOnly Property isDefinito() As Boolean
        Get
            Try
                Dim i, totale As Integer

                If Me.RBLsceltaTipodefinizione.SelectedValue = 0 Then
                    For i = 0 To Me.RPTgenericoTipoComunita.Items.Count - 1
                        Dim oRow As RepeaterItem
                        Dim oAssociato, oAttivato As CheckBox

                        Try
                            oRow = Me.RPTgenericoTipoComunita.Items(i)
                            oAssociato = oRow.FindControl("CBXgenericoTipoComunitaAssociato")
                            oAttivato = oRow.FindControl("CBXgenericoTipoComunitaAttiva")
                            If oAssociato.Checked Or oAttivato.Checked Then
                                isDefinito = True
                                Exit For
                            End If
                        Catch ex As Exception

                        End Try
                    Next
                Else
                    For i = 0 To Me.RPTtipoComunitaMultiplo.Items.Count - 1
                        Dim oRow As RepeaterItem
                        Dim oRPTorganizzazione As Repeater

                        oRow = Me.RPTtipoComunitaMultiplo.Items(i)
                        oRPTorganizzazione = oRow.FindControl("RPTorganizzazione")
                        If Not IsNothing(oRPTorganizzazione) Then
                            Dim j As Integer
                            Dim oAssociato, oAttivato As CheckBox

                            Try
                                For j = 0 To oRPTorganizzazione.Items.Count - 1
                                    Dim oRowSotto As RepeaterItem
                                    oRowSotto = oRPTorganizzazione.Items(j)
                                    oAssociato = oRowSotto.FindControl("CBXtipoComunitaAssociato")
                                    oAttivato = oRowSotto.FindControl("CBXtipoComunitaAttiva")

                                    If oAssociato.Checked Or oAttivato.Checked Then
                                        isDefinito = True
                                        Exit For
                                    End If
                                Next

                            Catch ex As Exception

                            End Try
                        End If
                    Next
                End If
            Catch ex As Exception
                isDefinito = False
            End Try
        End Get
    End Property
    Public ReadOnly Property ListaTipiComunitaSelezionati() As Object(,)
        Get
            Try
                Dim i, totale As Integer
                Dim oArrayLista(,) As Object

                If Me.RBLsceltaTipodefinizione.SelectedValue = 0 Then
                    ReDim Preserve oArrayLista(1, 0)
                    oArrayLista(0, 0) = "-1"
                    oArrayLista(1, 0) = ","
                    For i = 0 To Me.RPTgenericoTipoComunita.Items.Count - 1
                        Dim oRow As RepeaterItem
                        Dim oAssociato, oAttivato As CheckBox
                        Dim oLabel As Label
                        Try
                            oRow = Me.RPTgenericoTipoComunita.Items(i)
                            oAssociato = oRow.FindControl("CBXgenericoTipoComunitaAssociato")
                            oAttivato = oRow.FindControl("CBXgenericoTipoComunitaAttiva")
                            oLabel = oRow.FindControl("LBgenericotipoComunitaID")
                            If oAssociato.Checked Then
                                If InStr(oArrayLista(1, 0), "," & oLabel.Text & ",") <= 0 Then
                                    oArrayLista(1, 0) &= oLabel.Text & ","
                                End If
                            End If
                        Catch ex As Exception

                        End Try
                    Next
                Else
                    Dim indiceOrganizzazioni As Integer = 0

                    For i = 0 To Me.RPTtipoComunitaMultiplo.Items.Count - 1
                        Dim oRow As RepeaterItem
                        Dim oRPTorganizzazione As Repeater
                        Dim oLBtipoComunitaID, oLBOrganizzazioneID As Label
                        Dim tipoComunitaID, OrganizzazioneID As String

                        oRow = Me.RPTtipoComunitaMultiplo.Items(i)
                        oRPTorganizzazione = oRow.FindControl("RPTorganizzazione")
                        oLBtipoComunitaID = oRow.FindControl("LBtipoComunitaID")

                        tipoComunitaID = oLBtipoComunitaID.Text
                        If Not IsNothing(oRPTorganizzazione) And tipoComunitaID <> "" Then
                            Dim j, IndiceCorrente As Integer
                            Dim oAssociato, oAttivato As CheckBox



                            Try
                                For j = 0 To oRPTorganizzazione.Items.Count - 1
                                    Dim oRowSotto As RepeaterItem
                                    oRowSotto = oRPTorganizzazione.Items(j)
                                    oAssociato = oRowSotto.FindControl("CBXtipoComunitaAssociato")
                                    oAttivato = oRowSotto.FindControl("CBXtipoComunitaAttiva")
                                    oLBOrganizzazioneID = oRowSotto.FindControl("LBorganizzazioneID")
                                    OrganizzazioneID = oLBOrganizzazioneID.Text

                                    If (oAssociato.Checked Or oAttivato.Checked) And OrganizzazioneID <> "" Then
                                        Try
                                            If UBound(oArrayLista, 1) = 0 Then
                                                ReDim Preserve oArrayLista(1, indiceOrganizzazioni)
                                                oArrayLista(0, 0) = OrganizzazioneID
                                                oArrayLista(1, 0) = "," & tipoComunitaID & ","
                                            Else
                                                Dim ii As Integer
                                                IndiceCorrente = -1
                                                For ii = 0 To UBound(oArrayLista, 2) - 1
                                                    If oArrayLista(0, ii) = OrganizzazioneID Then
                                                        IndiceCorrente = ii
                                                        Exit For
                                                    End If
                                                Next
                                                If IndiceCorrente < 0 Then
                                                    indiceOrganizzazioni += 1
                                                    ReDim Preserve oArrayLista(1, indiceOrganizzazioni)
                                                    oArrayLista(0, IndiceCorrente) = OrganizzazioneID
                                                    oArrayLista(1, IndiceCorrente) = "," & tipoComunitaID & ","
                                                Else
                                                    oArrayLista(1, IndiceCorrente) &= tipoComunitaID & ","
                                                End If

                                            End If

                                        Catch ex As Exception

                                        End Try
                                    End If
                                Next
                            Catch ex As Exception

                            End Try
                            indiceOrganizzazioni += 1
                        End If
                    Next
                End If
                Return oArrayLista
            Catch ex As Exception
                ListaTipiComunitaSelezionati = Nothing
            End Try
        End Get
    End Property

    Public ReadOnly Property ListaTipiComunitaStandard() As Object(,)
        Get
            Try
                Dim i, totale As Integer
                Dim oArrayLista(,) As Object

                ReDim Preserve oArrayLista(1, 0)
                oArrayLista(0, 0) = "-1"
                oArrayLista(1, 0) = ","
                For i = 0 To Me.RPTgenericoTipoComunita.Items.Count - 1
                    Dim oRow As RepeaterItem
                    Dim oAssociato, oAttivato As CheckBox
                    Dim oLabel As Label
                    Try
                        oRow = Me.RPTgenericoTipoComunita.Items(i)
                        oAssociato = oRow.FindControl("CBXgenericoTipoComunitaAssociato")
                        oAttivato = oRow.FindControl("CBXgenericoTipoComunitaAttiva")
                        oLabel = oRow.FindControl("LBgenericotipoComunitaID")
                        If oAssociato.Checked Then
                            If InStr(oArrayLista(1, 0), "," & oLabel.Text & ",") <= 0 Then
                                oArrayLista(1, 0) &= oLabel.Text & ","
                            End If
                        End If
                    Catch ex As Exception

                    End Try
                Next
                Return oArrayLista
            Catch ex As Exception
                ListaTipiComunitaStandard = Nothing
            End Try
        End Get
    End Property
    Public ReadOnly Property ListaTipiComunitaMultipli() As Object(,)
        Get
            Try
                Dim i, totale As Integer
                Dim oArrayLista(,) As Object
                Dim indiceOrganizzazioni As Integer = 0

                For i = 0 To Me.RPTtipoComunitaMultiplo.Items.Count - 1
                    Dim oRow As RepeaterItem
                    Dim oRPTorganizzazione As Repeater
                    Dim oLBtipoComunitaID, oLBOrganizzazioneID As Label
                    Dim tipoComunitaID, OrganizzazioneID As String

                    oRow = Me.RPTtipoComunitaMultiplo.Items(i)
                    oRPTorganizzazione = oRow.FindControl("RPTorganizzazione")
                    oLBtipoComunitaID = oRow.FindControl("LBtipoComunitaID")

                    tipoComunitaID = oLBtipoComunitaID.Text
                    If Not IsNothing(oRPTorganizzazione) And tipoComunitaID <> "" Then
                        Dim j, IndiceCorrente As Integer
                        Dim oAssociato, oAttivato As CheckBox

                        Try
                            For j = 0 To oRPTorganizzazione.Items.Count - 1
                                Dim oRowSotto As RepeaterItem
                                oRowSotto = oRPTorganizzazione.Items(j)
                                oAssociato = oRowSotto.FindControl("CBXtipoComunitaAssociato")
                                oAttivato = oRowSotto.FindControl("CBXtipoComunitaAttiva")
                                oLBOrganizzazioneID = oRowSotto.FindControl("LBorganizzazioneID")
                                OrganizzazioneID = oLBOrganizzazioneID.Text

                                If (oAssociato.Checked Or oAttivato.Checked) And OrganizzazioneID <> "" Then
                                    Try
                                        If UBound(oArrayLista, 1) = 0 Then
                                            ReDim Preserve oArrayLista(1, indiceOrganizzazioni)
                                            oArrayLista(0, 0) = OrganizzazioneID
                                            oArrayLista(1, 0) = "," & tipoComunitaID & ","
                                        Else
                                            Dim ii As Integer
                                            IndiceCorrente = -1
                                            For ii = 0 To UBound(oArrayLista, 2) - 1
                                                If oArrayLista(0, ii) = OrganizzazioneID Then
                                                    IndiceCorrente = ii
                                                    Exit For
                                                End If
                                            Next
                                            If IndiceCorrente < 0 Then
                                                indiceOrganizzazioni += 1
                                                ReDim Preserve oArrayLista(1, indiceOrganizzazioni)
                                                oArrayLista(0, IndiceCorrente) = OrganizzazioneID
                                                oArrayLista(1, IndiceCorrente) = "," & tipoComunitaID & ","
                                            Else
                                                oArrayLista(1, IndiceCorrente) &= tipoComunitaID & ","
                                            End If

                                        End If

                                    Catch ex As Exception

                                    End Try
                                End If
                            Next
                        Catch ex As Exception

                        End Try
                        indiceOrganizzazioni += 1
                    End If
                Next

                Return oArrayLista
            Catch ex As Exception
                ListaTipiComunitaMultipli = Nothing
            End Try
        End Get
    End Property
    Public ReadOnly Property isInizializzato() As Boolean
        Get
            Try
                isInizializzato = (HDNhasSetupMultiplo.Value = True) Or (HDNhasSetupGenerali.Value = True)
            Catch ex As Exception
                isInizializzato = False
            End Try
        End Get
    End Property
    Public ReadOnly Property isDefinizionegenerale() As Boolean
        Get
            Try
                isDefinizionegenerale = (Me.RBLsceltaTipodefinizione.SelectedValue = 0)
            Catch ex As Exception
                isDefinizionegenerale = False
            End Try
        End Get
    End Property
    Private Function HasTipiComunitaAssociati() As Boolean
        Dim i, totale As Integer

        Try
            If Me.RBLsceltaTipodefinizione.SelectedValue = 0 Then
                totale = Me.RPTgenericoTipoComunita.Items.Count() - 1

                For i = 0 To totale
                    Dim oAssociato, oAttivato As System.Web.UI.WebControls.CheckBox
                    oAssociato = Me.RPTgenericoTipoComunita.Items(i).FindControl("CBXgenericoTipoComunitaAssociato")
                    oAttivato = Me.RPTgenericoTipoComunita.Items(i).FindControl("CBXgenericoTipoComunitaAttiva")
                    If oAssociato.Checked Or oAttivato.Checked Then
                        Return True
                    End If
                Next
            Else
                totale = Me.RPTtipoComunitaMultiplo.Items.Count() - 1

                For i = 0 To totale
                    Dim oRPTorganizzazione As Repeater
                    Dim j, totaleJ As Integer

                    oRPTorganizzazione = Me.RPTtipoComunitaMultiplo.Items(i).FindControl("RPTorganizzazione")
                    totaleJ = oRPTorganizzazione.Items.Count() - 1

                    For j = 0 To totaleJ
                        Dim oAssociato, oAttivato As System.Web.UI.WebControls.CheckBox
                        oAssociato = oRPTorganizzazione.Items(j).FindControl("CBXtipoComunitaAssociato")
                        oAttivato = oRPTorganizzazione.Items(j).FindControl("CBXtipoComunitaAttiva")

                        If oAssociato.Checked Or oAttivato.Checked Then
                            Return True
                        End If
                    Next
                Next
            End If
        Catch ex As Exception

        End Try
        Return False
    End Function

    Protected WithEvents HDNhasSetupGenerali As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNhasSetupMultiplo As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_definito As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_servizioID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents RPTtipoComunitaMultiplo As System.Web.UI.WebControls.Repeater
    Protected WithEvents LBsceltaTipodefinizione_t As System.Web.UI.WebControls.Label
    Protected WithEvents RBLsceltaTipodefinizione As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents RPTgenericoTipoComunita As System.Web.UI.WebControls.Repeater
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
        oResource.ResourcesName = "pg_UC_DatiServizio"
        oResource.Folder_Level1 = "Admin_Globale"
        oResource.Folder_Level2 = "UC"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(Me.LBsceltaTipodefinizione_t)
            .setRadioButtonList(Me.RBLsceltaTipodefinizione, 0)
            .setRadioButtonList(Me.RBLsceltaTipodefinizione, 1)
        End With
    End Sub
#End Region

    Public Function Setup_Controllo(ByVal ServizioID As Integer) As WizardServizio_Message
        Dim iResponse As WizardServizio_Message = WizardServizio_Message.ServizioNonTrovato

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        Me.SetupInternazionalizzazione()
        Try
            If ServizioID = 0 Then
                Me.RBLsceltaTipodefinizione.Enabled = False
            Else
                Me.RBLsceltaTipodefinizione.Enabled = True
            End If
            Me.RBLsceltaTipodefinizione.SelectedIndex = 0
            Me.HDN_servizioID.Value = ServizioID
            Me.HDNhasSetupGenerali.Value = False
            Me.HDNhasSetupMultiplo.Value = False

            Me.Bind_dati(ServizioID, True)
            If Me.RPTtipoComunitaMultiplo.Items.Count = 0 And Me.RBLsceltaTipodefinizione.SelectedIndex = 1 Then
                Return WizardServizio_Message.NessunTipoComunita
            ElseIf Me.RPTgenericoTipoComunita.Items.Count = 0 And Me.RBLsceltaTipodefinizione.SelectedIndex = 0 Then
                Return WizardServizio_Message.NessunTipoComunita
            Else
                Return WizardServizio_Message.OperazioneConclusa
            End If
        Catch ex As Exception
            Return WizardServizio_Message.NessunTipoComunita
        End Try
        Return iResponse
    End Function
    Private Sub Bind_dati(ByVal ServizioID As Integer, Optional ByVal GeneraForInit As Boolean = False)
        If ServizioID > 0 And GeneraForInit Then
            If Me.RBLsceltaTipodefinizione.SelectedValue = 0 Then
                Bind_DefinizioniSpecifiche()
            Else
                Bind_DefinizioniGenerali()
            End If
            Bind_DefinizioniSpecifiche()
        End If
        If Me.RBLsceltaTipodefinizione.SelectedValue <> 0 Then
            Bind_DefinizioniSpecifiche()
            Me.RPTgenericoTipoComunita.Visible = False
            Me.RPTtipoComunitaMultiplo.Visible = True
        Else
            Me.RPTgenericoTipoComunita.Visible = True
            Me.RPTtipoComunitaMultiplo.Visible = False
            Bind_DefinizioniGenerali()
        End If
    End Sub
    Private Sub Bind_DefinizioniSpecifiche()
        Dim oDataset As DataSet
        Dim oTipoComunita As New COL_Tipo_Comunita

        Try
            oDataset = oTipoComunita.Elenca(Session("LinguaID")) ' oServizio.TipiComunitaDefiniti(Me.DDLorganizzazione.SelectedValue, Session("LinguaID"))

            Me.RPTtipoComunitaMultiplo.DataSource = oDataset
            Me.RPTtipoComunitaMultiplo.DataBind()
            Me.HDNhasSetupMultiplo.Value = True
        Catch ex As Exception
            Me.HDNhasSetupMultiplo.Value = False
        End Try
    End Sub
    Private Sub Bind_DefinizioniGenerali()
        Dim oDataset As DataSet
        Dim oServizio As New COL_Servizio
        Dim i, totale As Integer
        oServizio.ID = ServizioID

        Try
            oDataset = oServizio.TipiComunitaDefiniti(-1, Session("LinguaID"))
            oDataset.Tables(0).Columns.Add(New DataColumn("oCheckAssociato"))
            oDataset.Tables(0).Columns.Add(New DataColumn("oCheckDefault"))

            totale = oDataset.Tables(0).Rows.Count - 1
            For i = 0 To totale
                Dim oRow As DataRow
                oRow = oDataset.Tables(0).Rows(i)

                If Me.HDNhasSetupGenerali.Value = False And Me.HDN_servizioID.Value = 0 Then
                    oRow.Item("oCheckDefault") = True
                    oRow.Item("oCheckAssociato") = True
                Else
                    Dim isDefault, isAssociato As Boolean
                    isDefault = CBool(oRow.Item("Attivato"))
                    isAssociato = CBool(oRow.Item("Associato"))
                    oRow.Item("oCheckDefault") = CBool(oRow.Item("Attivato"))
                    oRow.Item("oCheckAssociato") = CBool(oRow.Item("Associato"))
                End If
                If oRow.Item("totale") > 0 Then
                    oRow.Item("TPCM_descrizione") &= " (<b>" & oRow.Item("totale") & "</b>)"
                End If

                If CBool(oRow.Item("Associato")) Then
                    Me.HDN_definito.Value = True
                End If
            Next

            Me.RPTgenericoTipoComunita.DataSource = oDataset
            Me.RPTgenericoTipoComunita.DataBind()
            Me.HDNhasSetupGenerali.Value = True
        Catch ex As Exception
            Me.HDNhasSetupGenerali.Value = False
        End Try
    End Sub

    Private Sub RPTtipoComunitaMultiplo_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTtipoComunitaMultiplo.ItemDataBound
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If e.Item.ItemType = ListItemType.Header Then
            Try
                Dim oRPTorganizzazioneHEADER As Repeater

                oRPTorganizzazioneHEADER = e.Item.FindControl("RPTorganizzazioneHEADER")

                Dim oDataset As DataSet
                Dim oOrganizzazione As New COL_Organizzazione
                Dim totale, i As Integer

                If IsNothing(oRPTorganizzazioneHEADER) = False Then
                    AddHandler oRPTorganizzazioneHEADER.ItemCreated, AddressOf RPTorganizzazioneHEADER_ItemCreated
                End If
                oDataset = oOrganizzazione.ElencaByIstituzione(Session("ISTT_ID"))
                oDataset.Tables(0).Columns.Add(New DataColumn("CssClass"))
                totale = oDataset.Tables(0).Rows.Count - 1
                For i = 0 To totale
                    Dim oRow As DataRow
                    oRow = oDataset.Tables(0).Rows(i)
                    If i Mod 2 = 0 Then
                        oRow.Item("CssClass") = "ROW_Alternate_Small"
                    Else
                        oRow.Item("CssClass") = "ROW_Normal_Small"
                    End If
                Next
                oRPTorganizzazioneHEADER.DataSource = oDataset
                oRPTorganizzazioneHEADER.DataBind()
            Catch ex As Exception

            End Try
        ElseIf e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Try
                Dim oRPTorganizzazione As Repeater
                Dim oLBtipoComunitaID As Label
                Dim oDataset As DataSet
                Dim oTipoComunita As New COL_Tipo_Comunita
                Dim totale, i As Integer

                oRPTorganizzazione = e.Item.FindControl("RPTorganizzazione")
                oLBtipoComunitaID = e.Item.FindControl("LBtipoComunitaID")
                If IsNothing(oRPTorganizzazione) = False Then
                    AddHandler oRPTorganizzazione.ItemCreated, AddressOf RPTorganizzazione_ItemCreated
                End If
                oTipoComunita.ID = oLBtipoComunitaID.Text
                oDataset = oTipoComunita.ElencaDefinizioneServizi(Me.HDN_servizioID.Value)

                oDataset.Tables(0).Columns.Add(New DataColumn("oCheckAssociato"))
                oDataset.Tables(0).Columns.Add(New DataColumn("oCheckDefault"))
                oDataset.Tables(0).Columns.Add(New DataColumn("CssClass"))
                totale = oDataset.Tables(0).Rows.Count - 1
                For i = 0 To totale
                    Dim oRow As DataRow
                    oRow = oDataset.Tables(0).Rows(i)

                    If Me.HDNhasSetupMultiplo.Value = False And Me.HDN_servizioID.Value = 0 Then
                        oRow.Item("oCheckDefault") = True
                        oRow.Item("oCheckAssociato") = True
                    Else
                        oRow.Item("oCheckDefault") = CBool(oRow.Item("Attivato"))
                        oRow.Item("oCheckAssociato") = CBool(oRow.Item("Associato"))
                    End If

                    If i Mod 2 = 0 Then
                        oRow.Item("CssClass") = "ROW_Alternate_Small"
                    Else
                        oRow.Item("CssClass") = "ROW_Normal_Small"
                    End If
                Next
                oRPTorganizzazione.DataSource = oDataset
                oRPTorganizzazione.DataBind()
            Catch ex As Exception

            End Try
        End If

    End Sub
    Private Sub RPTorganizzazione_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Try
                oResource.setCheckBox(e.Item.FindControl("CBXtipoComunitaAssociato"))
            Catch ex As Exception

            End Try
            Try
                oResource.setCheckBox(e.Item.FindControl("CBXtipoComunitaAttiva"))
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub RPTorganizzazioneHEADER_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If e.Item.ItemType = ListItemType.Header Then

        End If
    End Sub

    Public Function SalvaDati(ByVal ServizioID As Integer) As WizardServizio_Message
        Me.HDN_servizioID.Value = ServizioID
        If Me.HasTipiComunitaAssociati Then
            Return AssociaTipiComunita()
        Else
            Return WizardServizio_Message.SelezionaTipoComunita
        End If
    End Function

    Private Function AssociaTipiComunita() As WizardServizio_Message
        Dim i, totale, LKST_id As Integer
        Dim oServizio As New COL_Servizio
        Try
            Dim ListaAssociati As String = ","
            Dim ListaAttivati As String = ","

            oServizio.ID = Me.HDN_servizioID.Value

            If Me.RBLsceltaTipodefinizione.SelectedValue = 0 Then
                totale = Me.RPTgenericoTipoComunita.Items.Count() - 1

                For i = 0 To totale
                    Dim ocheckAssociato As System.Web.UI.WebControls.CheckBox
                    Dim ocheckAttivato As System.Web.UI.WebControls.CheckBox
                    Dim oLabelID As Label

                    Try
                        oLabelID = Me.RPTgenericoTipoComunita.Items(i).FindControl("LBgenericotipoComunitaID")
                        ocheckAttivato = Me.RPTgenericoTipoComunita.Items(i).FindControl("CBXgenericoTipoComunitaAttiva")
                        ocheckAssociato = Me.RPTgenericoTipoComunita.Items(i).FindControl("CBXgenericoTipoComunitaAssociato")

                        If ocheckAssociato.Checked Then
                            ListaAssociati &= oLabelID.Text & ","
                        ElseIf ocheckAttivato.Checked Then
                            ListaAssociati &= oLabelID.Text & ","
                        End If
                        If ocheckAttivato.Checked Then
                            ListaAttivati &= oLabelID.Text & ","
                        End If
                    Catch exAbilitato As Exception

                    End Try
                Next

                If ListaAssociati = "," Then
                    ListaAssociati = ""
                End If
                If ListaAttivati = "," Then
                    ListaAttivati = ""
                End If
                oServizio.GeneraTipiComunitaImpostazioniDefault(ListaAssociati, ListaAttivati)
                If oServizio.Errore = Errori_Db.None Then
                    Me.HDN_definito.Value = True
                    Return WizardServizio_Message.TipiComunitaAssociati
                Else
                    Return WizardServizio_Message.ErroreTipiComunita
                End If
            Else
                ' MANCA PER TUTTE QUANTE LE ALTRE !!
                totale = Me.RPTtipoComunitaMultiplo.Items.Count() - 1
                Dim oHashAttivate As Hashtable
                Dim oHashAssociate As Hashtable
                Dim oArrayOrganizzazione As Integer()
                Dim indiceOrganizzazione As Integer = 0

                For i = 0 To totale
                    Dim oLabelID As Label
                    Dim OrganizzazioneID, TipoComunitaID As Integer
                    Dim oRPTorganizzazione As Repeater
                    Try
                        oRPTorganizzazione = Me.RPTtipoComunitaMultiplo.Items(i).FindControl("RPTorganizzazione")
                        oLabelID = Me.RPTtipoComunitaMultiplo.Items(i).FindControl("LBtipoComunitaID")
                        TipoComunitaID = oLabelID.Text

                        If Not IsNothing(oRPTorganizzazione) Then
                            Dim j As Integer

                            For j = 0 To oRPTorganizzazione.Items.Count() - 1
                                Dim ocheckAssociato As System.Web.UI.WebControls.CheckBox
                                Dim ocheckAttivato As System.Web.UI.WebControls.CheckBox
                                Dim oLabelOrgnID As Label
                                Dim Lista As String

                                ocheckAttivato = oRPTorganizzazione.Items(j).FindControl("CBXtipoComunitaAttiva")
                                ocheckAssociato = oRPTorganizzazione.Items(j).FindControl("CBXtipoComunitaAssociato")
                                oLabelOrgnID = oRPTorganizzazione.Items(j).FindControl("LBorganizzazioneID")
                                OrganizzazioneID = oLabelOrgnID.Text

                                If Not oHashAttivate.ContainsKey(CInt(OrganizzazioneID)) Then
                                    ReDim oArrayOrganizzazione(indiceOrganizzazione)
                                    oArrayOrganizzazione(indiceOrganizzazione) = OrganizzazioneID
                                    indiceOrganizzazione += 1
                                    oHashAttivate.Add(OrganizzazioneID, ",")
                                    oHashAssociate.Add(OrganizzazioneID, ",")
                                End If
                                If ocheckAttivato.Checked Then
                                    oHashAttivate.Item(CInt(OrganizzazioneID)) = CStr(oHashAttivate.Item(CInt(OrganizzazioneID))) & TipoComunitaID & ","
                                End If
                                If ocheckAttivato.Checked Or ocheckAssociato.Checked Then
                                    oHashAssociate.Item(CInt(OrganizzazioneID)) = CStr(oHashAssociate.Item(CInt(OrganizzazioneID))) & TipoComunitaID & ","
                                End If
                            Next
                        End If
                    Catch exAbilitato As Exception

                    End Try
                Next

                Dim iResponse As WizardServizio_Message = ModuloEnum.WizardServizio_Message.TipiComunitaAssociati
                If indiceOrganizzazione = 0 Then
                    iResponse = ModuloEnum.WizardServizio_Message.SelezionaTipoComunita
                Else
                    For i = 0 To indiceOrganizzazione - 1
                        ListaAttivati = CStr(oHashAttivate.Item(CInt(oArrayOrganizzazione(i))))
                        ListaAssociati = CStr(oHashAssociate.Item(CInt(oArrayOrganizzazione(i))))

                        If ListaAssociati = "," Then
                            ListaAssociati = ""
                        End If
                        If ListaAttivati = "," Then
                            ListaAttivati = ""
                        End If
                        oServizio.DefinisciTipiComunitaDisponibili(CInt(oArrayOrganizzazione(i)), ListaAssociati, False)
                        If oServizio.Errore = Errori_Db.None Then
                            Me.HDN_definito.Value = True
                            oServizio.DefinisciTipoComunitaAttivate(CInt(oArrayOrganizzazione(i)), ListaAttivati, False)
                        Else
                            iResponse = WizardServizio_Message.ErroreTipiComunita
                        End If
                    Next
                End If

                Return iResponse
            End If
        Catch ex As Exception
            Return WizardServizio_Message.ErroreTipiComunita
        End Try
    End Function

    Private Sub RBLsceltaTipodefinizione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLsceltaTipodefinizione.SelectedIndexChanged
        Dim ServizioID As Integer = 0

        Try
            ServizioID = Me.HDN_servizioID.Value
        Catch ex As Exception

        End Try
        Me.Bind_dati(ServizioID)
        If ServizioID > 0 Then
            Dim isDefault As Boolean

            isDefault = (Me.RBLsceltaTipodefinizione.SelectedValue = 0)
            RaiseEvent AggiornaMenuServizi(isDefault, True)
        End If
    End Sub
    Private Sub RPTgenericoTipoComunita_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTgenericoTipoComunita.ItemDataBound
        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Try
                oResource.setCheckBox(e.Item.FindControl("CBXgenericoTipoComunitaAssociato"))
            Catch ex As Exception

            End Try
            Try
                oResource.setCheckBox(e.Item.FindControl("CBXgenericoTipoComunitaAttiva"))
            Catch ex As Exception

            End Try
        End If

    End Sub



    Public Function ModificaDati(Optional ByVal Replica As Boolean = False, Optional ByVal DefaultValue As Boolean = False) As WizardServizio_Message
        Dim oServizio As New COL_Servizio
        Dim iResponse As WizardServizio_Message = ModuloEnum.WizardServizio_Message.SelezionaTipoComunita

        If Me.HasTipiComunitaAssociati Then
            If DefaultValue Then
                iResponse = SalvaImpostazioniDefault(Replica)
                If Me.RBLsceltaTipodefinizione.SelectedValue <> 0 And Not Replica Then
                    Me.HDNhasSetupMultiplo.Value = False
                    Me.Bind_DefinizioniSpecifiche()
                End If
            Else
                iResponse = SalvaImpostazioniOrganizzazioni()
            End If
        Else
            Return WizardServizio_Message.SelezionaTipoComunita
        End If
        Return iResponse
    End Function

    Private Function SalvaImpostazioniDefault(ByVal Replica As Boolean) As WizardServizio_Message
        Dim i, totale, LKST_id As Integer
        Dim iResponse As WizardServizio_Message = ModuloEnum.WizardServizio_Message.ErroreGenerico
        Dim oServizio As New COL_Servizio
        Try
            Dim ListaAssociati As String = ","
            Dim ListaAttivati As String = ","

            oServizio.ID = Me.HDN_servizioID.Value
            totale = Me.RPTgenericoTipoComunita.Items.Count() - 1

            For i = 0 To totale
                Dim ocheckAssociato As System.Web.UI.WebControls.CheckBox
                Dim ocheckAttivato As System.Web.UI.WebControls.CheckBox
                Dim oLabelID As Label

                Try
                    oLabelID = Me.RPTgenericoTipoComunita.Items(i).FindControl("LBgenericotipoComunitaID")
                    ocheckAttivato = Me.RPTgenericoTipoComunita.Items(i).FindControl("CBXgenericoTipoComunitaAttiva")
                    ocheckAssociato = Me.RPTgenericoTipoComunita.Items(i).FindControl("CBXgenericoTipoComunitaAssociato")

                    oServizio.ImpostaTipoComunitaToDefault(CInt(oLabelID.Text), ocheckAssociato.Checked, ocheckAttivato.Checked, Replica)
                    If oServizio.Errore = Errori_Db.None And iResponse <> WizardServizio_Message.ErroreTipiComunita Then
                        Me.HDN_definito.Value = True
                        iResponse = WizardServizio_Message.TipiComunitaAssociati
                    Else
                        iResponse = WizardServizio_Message.ErroreTipiComunita
                    End If
                Catch exAbilitato As Exception

                End Try
            Next
            Return iResponse
        Catch ex As Exception
            Return WizardServizio_Message.ErroreTipiComunita
        End Try
    End Function
    Private Function SalvaImpostazioniOrganizzazioni() As WizardServizio_Message
        Dim i, totale, LKST_id As Integer
        Dim iResponse As WizardServizio_Message = ModuloEnum.WizardServizio_Message.ErroreGenerico
        Dim oServizio As New COL_Servizio

        Try
            oServizio.ID = Me.HDN_servizioID.Value
            totale = Me.RPTtipoComunitaMultiplo.Items.Count() - 1

            For i = 0 To totale
                Dim oLabelID As Label
                Dim OrganizzazioneID, TipoComunitaID As Integer
                Dim oRPTorganizzazione As Repeater
                Try
                    oRPTorganizzazione = Me.RPTtipoComunitaMultiplo.Items(i).FindControl("RPTorganizzazione")
                    oLabelID = Me.RPTtipoComunitaMultiplo.Items(i).FindControl("LBtipoComunitaID")
                    TipoComunitaID = oLabelID.Text

                    If Not IsNothing(oRPTorganizzazione) Then
                        Dim j As Integer

                        For j = 0 To oRPTorganizzazione.Items.Count() - 1
                            Dim ocheckAssociato As System.Web.UI.WebControls.CheckBox
                            Dim ocheckAttivato As System.Web.UI.WebControls.CheckBox
                            Dim oLabelOrgnID As Label
                            Dim Lista As String

                            ocheckAttivato = oRPTorganizzazione.Items(j).FindControl("CBXtipoComunitaAttiva")
                            ocheckAssociato = oRPTorganizzazione.Items(j).FindControl("CBXtipoComunitaAssociato")
                            oLabelOrgnID = oRPTorganizzazione.Items(j).FindControl("LBorganizzazioneID")
                            OrganizzazioneID = oLabelOrgnID.Text

                            oServizio.ImpostaTipoComunita(OrganizzazioneID, TipoComunitaID, ocheckAttivato.Checked Or ocheckAssociato.Checked, ocheckAttivato.Checked)
                            If oServizio.Errore = Errori_Db.None And iResponse <> WizardServizio_Message.ErroreTipiComunita Then
                                Me.HDN_definito.Value = True
                                iResponse = WizardServizio_Message.TipiComunitaAssociati
                            Else
                                iResponse = WizardServizio_Message.ErroreTipiComunita
                            End If
                        Next
                    End If
                Catch exAbilitato As Exception

                End Try
            Next
            Return iResponse
        Catch ex As Exception
            Return WizardServizio_Message.ErroreTipiComunita
        End Try
    End Function
End Class