
Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona


Public Class AdminG_RigeneraAlberoUtenti
    Inherits System.Web.UI.Page

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
        If Page.IsPostBack = False Then
            Dim oPersona As New COL_Persona
            Dim oDataset As New DataSet
			Dim oUtility As New OLDpageUtility(Me.Context)

            Dim i, totale As Integer

            Try
                oDataset = oPersona.Elenca()
                totale = oDataset.Tables(0).Rows.Count - 1
                For i = 0 To totale


                    Dim oTreeComunita As New COL_TreeComunita
                    Dim PersonaID As Integer
                    Try

                        PersonaID = oDataset.Tables(0).Rows(i).Item("PRSN_ID")

						oTreeComunita.Directory = Server.MapPath(oUtility.ApplicationUrlBase & "profili/") & PersonaID & "\"
                        oTreeComunita.Nome = PersonaID & ".xml"
                        If Not oTreeComunita.Exist() Then
							oTreeComunita.AggiornaInfo(PersonaID, oDataset.Tables(0).Rows(i).Item("PRSN_LNGU_ID"), oUtility.SystemSettings.Login.DaysToUpdateProfile, False)
                        End If
                    Catch ex As Exception

                    End Try
                Next
            Catch ex As Exception

            End Try
           
        End If
    End Sub

End Class
