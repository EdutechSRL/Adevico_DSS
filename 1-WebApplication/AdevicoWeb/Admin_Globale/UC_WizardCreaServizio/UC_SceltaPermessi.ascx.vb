Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.Comunita

Imports Comunita_OnLine.ModuloEnum

Public Class UC_SceltaPermessi
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
                isDefinito = (Me.HDN_definito.Value = True)
            Catch ex As Exception
                isDefinito = False
            End Try
        End Get
    End Property
    Public ReadOnly Property isInizializzato() As Boolean
        Get
            Try
                isInizializzato = CBool(HDNhasSetup.Value)
            Catch ex As Exception
                isInizializzato = False
            End Try
        End Get
    End Property
    Public ReadOnly Property PermessiSelezionati() As String
        Get
            Try
                Dim i As Integer
                Dim selezionati As String = ","
                For i = Me.CBLpermessi.SelectedIndex To Me.CBLpermessi.Items.Count - 1
                    If Me.CBLpermessi.Items(i).Selected Then
                        selezionati &= Me.CBLpermessi.Items(i).Value & ","
                    End If
                Next
                PermessiSelezionati = selezionati
            Catch ex As Exception
                PermessiSelezionati = ""
            End Try
        End Get
    End Property
    Protected WithEvents HDNhasSetup As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_definito As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_servizioID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_associati As System.Web.UI.HtmlControls.HtmlInputHidden
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
    Public Function Setup_Controllo(ByVal ServizioID As Integer) As ModuloEnum.WizardServizio_Message
        Dim iResponse As WizardServizio_Message = WizardServizio_Message.NessunPermesso


        Dim oServizio As New COL_Servizio
        Dim i, totale As Integer

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        Me.SetupInternazionalizzazione()
        Try
            Me.HDN_servizioID.Value = ServizioID

            oServizio.ID = ServizioID

            Dim items As List(Of PermissionAssociation) = oServizio.ListPermissionForSelection(Session("LinguaID"))
            totale = items.Count

            Me.TBLassociaPermessi.Visible = True
            Me.CBLpermessi.Items.Clear()

            Me.HDN_associati.Value = ""
            If totale > 0 Then
                Me.CBLpermessi.DataSource = items
                Me.CBLpermessi.DataValueField = "Id"
                Me.CBLpermessi.DataTextField = "Name"
                Me.CBLpermessi.DataBind()

                Dim index As Integer = 0
                For Each item As ListItem In CBLpermessi.Items
                    item.Text &= " ( " & items(index).Position & " - " & items(index).ToBinary & ")"
                    index += 1
                Next
                For i = 0 To totale - 1

                    If items(i).IsAssociated Then
                        Me.CBLpermessi.Items(i).Selected = True
                        Me.HDN_associati.Value &= Me.CBLpermessi.Items(i).Value & ","
                    End If
                Next
                If Me.HDN_associati.Value <> "" Then
                    Me.HDN_associati.Value = "," & Me.HDN_associati.Value
                End If
                Me.HDNhasSetup.Value = True
                Return WizardServizio_Message.OperazioneConclusa
            Else
                Me.HDNhasSetup.Value = False
                Return WizardServizio_Message.NessunPermesso
            End If

        Catch ex As Exception

        End Try
        Return iResponse
    End Function

    Public Function HasPermessiSelezionati() As Boolean
        Dim i, totale As Integer

        totale = Me.CBLpermessi.Items.Count
        For i = 0 To totale - 1
            Try
                If Me.CBLpermessi.Items(i).Selected Then
                    Return True
                End If
            Catch ex As Exception

            End Try
        Next
        Return False
    End Function
    Public Function HasPermessi() As Boolean
        Dim totale As Integer = 0
        Try
            totale = Me.CBLpermessi.Items.Count
        Catch ex As Exception

        End Try
        Return (totale > 0)
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

    Public Function Salva_Dati(ByVal ServizioID As Integer) As WizardServizio_Message
        Dim iResponse As WizardServizio_Message = WizardServizio_Message.ErroreGenerico
        Dim oServizio As New COL_Servizio

        Try
            Me.HDN_servizioID.Value = ServizioID
            oServizio.ID = ServizioID
            If Me.HasPermessiSelezionati = False Then
                Return WizardServizio_Message.SelezionaPermesso
            Else
                Dim i, totale, PermessoID As Integer

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

            End If
        Catch ex As Exception

        End Try
        Return iResponse
    End Function
End Class