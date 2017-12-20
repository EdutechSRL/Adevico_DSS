Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Curriculum_Europeo



Public Class UC_infoConoscenzaLingua
    Inherits System.Web.UI.UserControl
    Protected oResourceLingua As ResourceManager

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


#Region "accessori"
    Protected WithEvents HDNprsn_id As System.Web.UI.HtmlControls.HtmlInputHidden
    'Protected WithEvents HDNcreu_id As System.Web.UI.HtmlControls.HtmlInputHidden
    ' Protected WithEvents HDNcnln_id As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents PNLlingua As System.Web.UI.WebControls.Panel
#End Region

#Region "lista"

    Protected WithEvents RPTlingua As System.Web.UI.WebControls.Repeater
    Protected WithEvents LBnome_s As System.Web.UI.WebControls.Label
    Protected WithEvents LBlettura_s As System.Web.UI.WebControls.Label
    Protected WithEvents LBscrittura_s As System.Web.UI.WebControls.Label
    Protected WithEvents LBespressioneOrale_s As System.Web.UI.WebControls.Label

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


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsNothing(oResourceLingua) Then
            SetCulture(Session("LinguaCode"))
        End If
        Try
            If Page.IsPostBack = False Then
                Me.SetupInternazionalizzazione()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Function start() As Boolean
        Dim PRSN_id As Integer
        Dim oConoscenzaLingua As New COL_ConoscenzaLingua
        If IsNothing(oResourceLingua) Then
            SetCulture(Session("LinguaCode"))
            Me.SetupInternazionalizzazione()
        End If
        PRSN_id = Me.HDNprsn_id.Value
        oConoscenzaLingua.EstraiByPersona(PRSN_id)
        If oConoscenzaLingua.ID = -1 Then
            PNLlingua.Visible = False
            Return False
        Else
            PNLlingua.Visible = True
            Bind_Dati()
            Return True
        End If
    End Function

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResourceLingua = New ResourceManager

        oResourceLingua.UserLanguages = Code
        oResourceLingua.ResourcesName = "pg_UC_ConoscenzaLingua"
        oResourceLingua.Folder_Level1 = "Curriculum"
        oResourceLingua.Folder_Level2 = "UC"
        oResourceLingua.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResourceLingua

        End With
    End Sub

#End Region

#Region "bind"
    Private Sub Bind_Dati()
        Dim PRSN_id As Integer
        Dim oDataset As DataSet
        Dim i, totale As Integer
        Dim oConoscenzaLingua As New COL_ConoscenzaLingua
        Try
            PRSN_id = Me.HDNprsn_id.Value
            oDataset = oConoscenzaLingua.ElencaByPersona(PRSN_id, Main.FiltroVisibilità.Pubblici)
            totale = oDataset.Tables(0).Rows.Count
            If totale = 0 Then 'se datagrid vuota
                Me.PNLlingua.Visible = False
            Else
                Viewstate("startLingua") = ""
                oDataset.Tables(0).Columns.Add(New DataColumn("oLettura"))
                oDataset.Tables(0).Columns.Add(New DataColumn("oScrittura"))
                oDataset.Tables(0).Columns.Add(New DataColumn("oEspressioneOrale"))
                For i = 0 To totale - 1
                    Dim oRow As DataRow

                    oRow = oDataset.Tables(0).Rows(i)


                    If oRow.Item("CNLN_lettura") = 1 Then
                        oRow.Item("oLettura") = oResourceLingua.getValue("Nessuna") '"Nessuna" 'oResource.
                    ElseIf oRow.Item("CNLN_lettura") = 2 Then
                        oRow.Item("oLettura") = oResourceLingua.getValue("Limitata") '"Limitata"
                    ElseIf oRow.Item("CNLN_lettura") = 3 Then
                        oRow.Item("oLettura") = oResourceLingua.getValue("Discreta") '"Discreta"
                    ElseIf oRow.Item("CNLN_lettura") = 4 Then
                        oRow.Item("oLettura") = oResourceLingua.getValue("Buona") '"Buona"
                    ElseIf oRow.Item("CNLN_lettura") = 5 Then
                        oRow.Item("oLettura") = oResourceLingua.getValue("Ottima") '"Ottima"
                    ElseIf oRow.Item("CNLN_lettura") = 6 Then
                        oRow.Item("oLettura") = oResourceLingua.getValue("Madrelingua") '"Madrelingua"
                    End If

                    If oRow.Item("CNLN_scrittura") = 1 Then
                        oRow.Item("oScrittura") = oResourceLingua.getValue("Nessuna") '"Nessuna" 'oResource.
                    ElseIf oRow.Item("CNLN_scrittura") = 2 Then
                        oRow.Item("oScrittura") = oResourceLingua.getValue("Limitata") '"Limitata"
                    ElseIf oRow.Item("CNLN_scrittura") = 3 Then
                        oRow.Item("oScrittura") = oResourceLingua.getValue("Discreta") '"Discreta"
                    ElseIf oRow.Item("CNLN_scrittura") = 4 Then
                        oRow.Item("oScrittura") = oResourceLingua.getValue("Buona")  '"Buona"
                    ElseIf oRow.Item("CNLN_scrittura") = 5 Then
                        oRow.Item("oScrittura") = oResourceLingua.getValue("Ottima")  '"Ottima"
                    ElseIf oRow.Item("CNLN_scrittura") = 6 Then
                        oRow.Item("oScrittura") = oResourceLingua.getValue("Madrelingua") '"Madrelingua"
                    End If

                    If oRow.Item("CNLN_espressioneOrale") = 1 Then
                        oRow.Item("oEspressioneOrale") = oResourceLingua.getValue("Nessuna") '"Nessuna" 'oResource.
                    ElseIf oRow.Item("CNLN_espressioneOrale") = 2 Then
                        oRow.Item("oEspressioneOrale") = oResourceLingua.getValue("Limitata") '"Limitata"
                    ElseIf oRow.Item("CNLN_espressioneOrale") = 3 Then
                        oRow.Item("oEspressioneOrale") = oResourceLingua.getValue("Discreta") '"Discreta"
                    ElseIf oRow.Item("CNLN_espressioneOrale") = 4 Then
                        oRow.Item("oEspressioneOrale") = oResourceLingua.getValue("Buona")  '"Buona"
                    ElseIf oRow.Item("CNLN_espressioneOrale") = 5 Then
                        oRow.Item("oEspressioneOrale") = oResourceLingua.getValue("Ottima")  '"Ottima"
                    ElseIf oRow.Item("CNLN_espressioneOrale") = 6 Then
                        oRow.Item("oEspressioneOrale") = oResourceLingua.getValue("Madrelingua") '"Madrelingua"
                    End If
                Next
                Me.RPTlingua.DataSource = oDataset
                Me.RPTlingua.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "gestione repeater"
    Private Sub RPTlingua_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTlingua.ItemCreated
        If IsNothing(oResourceLingua) Then
            SetCulture(Session("LinguaCode"))
        End If
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Try


                Try
                    Dim olabel As Label
                    olabel = e.Item.FindControl("LBnome_s")
                    If IsNothing(olabel) = False Then
                        oResourceLingua.setLabel_To_Value(olabel, "LBnome_s.text")
                    End If
                Catch ex As Exception

                End Try
                Try
                    Dim olabel As Label
                    olabel = e.Item.FindControl("LBlettura_s")
                    If IsNothing(olabel) = False Then
                        oResourceLingua.setLabel_To_Value(olabel, "LBlettura_s.text")
                    End If
                Catch ex As Exception

                End Try
                Try
                    Dim olabel As Label
                    olabel = e.Item.FindControl("LBscrittura_s")
                    If IsNothing(olabel) = False Then
                        oResourceLingua.setLabel_To_Value(olabel, "LBscrittura_s.text")
                    End If
                Catch ex As Exception

                End Try
                Try
                    Dim olabel As Label
                    olabel = e.Item.FindControl("LBespressioneOrale_s")
                    If IsNothing(olabel) = False Then
                        oResourceLingua.setLabel_To_Value(olabel, "LBespressioneOrale_s.text")
                    End If
                Catch ex As Exception

                End Try
            Catch ex As Exception

            End Try
        End If
    End Sub

#End Region

End Class