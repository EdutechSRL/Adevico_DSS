Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Curriculum_Europeo


Public Class UC_infoFormazione
    Inherits System.Web.UI.UserControl
    Protected oResourceFormazione As ResourceManager

#Region "accessori"
    Protected WithEvents HDNprsn_id As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents HDNcreu_id As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents HDNisfr_id As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents PNLformazione As System.Web.UI.WebControls.Panel
#End Region
#Region "lista"

    Protected WithEvents RPTformazione As System.Web.UI.WebControls.Repeater
    'Protected WithEvents BTNinserisci As System.Web.UI.WebControls.Button
    Protected WithEvents TBRmaterie As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRqualifica As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRlivello As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBinizio_s As System.Web.UI.WebControls.Label
    Protected WithEvents LBfine_s As System.Web.UI.WebControls.Label
    Protected WithEvents LBnome_s As System.Web.UI.WebControls.Label
    Protected WithEvents LBmaterie_s As System.Web.UI.WebControls.Label
    Protected WithEvents LBqualifica_s As System.Web.UI.WebControls.Label
    Protected WithEvents LBclassificazione_s As System.Web.UI.WebControls.Label
#End Region

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
        Try
            If IsNothing(oResourceFormazione) Then
                SetCulture(Session("LinguaCode"))
            End If
            If Page.IsPostBack = False Then
                Me.SetupInternazionalizzazione()
            End If

        Catch ex As Exception

        End Try
    End Sub
    Public Function start() As Boolean
        If IsNothing(oResourceFormazione) Then
            SetCulture(Session("LinguaCode"))
            Me.SetupInternazionalizzazione()
        End If

        Dim PRSN_id As Integer
        Dim oFormazione As New COL_IstruzioneFormazione
        'Dim oCurriculum As New COL_CurriculumEuropeo

        PRSN_id = Me.HDNprsn_id.Value
        oFormazione.EstraiByPersona(PRSN_id)
        '  oCurriculum.EstraiByPersona(PRSN_id)
        ' Me.HDNcreu_id.Value = oCurriculum.ID
        If oFormazione.ID = -1 Then
            PNLformazione.Visible = False
            Return False
        Else
            PNLformazione.Visible = True
            Bind_Dati()
            Return True
        End If
    End Function


#Region "Bind_Dati"
    Private Sub Bind_Dati()
        Dim PRSN_id As Integer
        Dim oDataset As DataSet
        Dim i, totale As Integer
        Dim oFormazione As New COL_IstruzioneFormazione
        Try
            PRSN_id = Me.HDNprsn_id.Value
            oDataset = oFormazione.ElencaByPersona(PRSN_id, Main.FiltroVisibilità.Pubblici)
            totale = oDataset.Tables(0).Rows.Count

            If totale = 0 Then 'se datagrid vuota
                Me.PNLformazione.Visible = False
            Else
                Viewstate("startFormazione") = ""
                oDataset.Tables(0).Columns.Add(New DataColumn("ofine"))
                oDataset.Tables(0).Columns.Add(New DataColumn("oRendiPubblico"))
                For i = 0 To totale - 1
                    Dim oRow As DataRow

                    oRow = oDataset.Tables(0).Rows(i)

                    If oRow.Item("ISFR_esperienzaInCorso") = "1" Then
                        oRow.Item("ofine") = oResourceFormazione.getValue("incorso") '"Esperienza in Corso" '.
                    Else
                        oRow.Item("ofine") = oRow.Item("ISFR_fine")
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
#End Region


#Region "gestione repeater"
    Private Sub RPTformazione_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTformazione.ItemCreated
        If IsNothing(oResourceFormazione) Then
            SetCulture(Session("LinguaCode"))
        End If
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Try


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
            Catch ex As Exception

            End Try
        End If
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

        End With
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

End Class
