Imports Comunita_OnLine
Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.CL_persona
Imports lm.Comol.Modules.Chat

Public Class ChatOutPut_Ext
    Inherits System.Web.UI.Page

    Private Messaggi As New WS_Chat.WS_ChatSoapClient
    Dim ArUtBlock() As Integer
    'Dim IdCom As Integer
    Protected oResource As ResourceManager
    Protected WithEvents Output As System.Web.UI.HtmlControls.HtmlControl
    Protected WithEvents Body As System.Web.UI.HtmlControls.HtmlGenericControl

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblout As System.Web.UI.WebControls.Label

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

        'Localizzazione
        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
            Me.SetupInternazionalizzazione()
        End If

        'Impostazone del refresh
        Dim timeref As Integer
        timeref = CInt(Session("RefTime"))
        If timeref < 5 Then
            timeref = 5
        End If

        Me.Response.AppendHeader("Refresh", timeref)

        'Raccolta ID comunita
        Dim IdCom As Integer
        IdCom = CInt(Request.QueryString("IDCom"))
        If IdCom = 0 Then
            Me.SetLblOut(2)
            Exit Sub
        End If

        'Recupero messaggi
        Try
            If Messaggi.GetLvl(Session("objPersona").Id, IdCom) > 0 Then
                Dim strTemp As String
                Dim DSTemp As New DataSet
                Dim Msg As String

                DSTemp = Messaggi.RecuperaMessaggio(Session("objPersona").Id, IdCom) 'Session("ChatLastConnection")

                Dim oDataview As DataView

                'oDataview = DSTemp.Tables("DTTemp").DefaultView
                ''Tentativo...
                'oDataview.Sort = "Tempo DESC"

                If DSTemp.Tables(0).Rows.Count > 0 Then

                    For Each MyRow As DataRow In DSTemp.Tables(0).Rows
                        Dim oTempo As DateTime

                        Msg = MyRow(2)
                        Msg = Me.faccine(Msg)
                        oTempo = MyRow("Tempo")
                        If MyRow("TBold") Then
                            Msg = "<b>" & Msg & "</b>"
                        End If

                        If MyRow(6) Then
                            Msg = "<i>" & Msg & "</i>"
                        End If

                        If MyRow(7) Then
                            Msg = "<u>" & Msg & "</u>"
                        End If

                        Msg = "<font style='COLOR: #" & MyRow(9) & "; BACKGROUND-COLOR=" & MyRow(8) & "'>" & Msg & "</font>"
                        strTemp &= "<b><u>" & MyRow(0) & "</U></b>" & " - [" & MyRow(1).ToShortTimeString() & "] : " & "<br>" & Msg & "<br>" '& strTemp '.ToLongTimeString()
                    Next

                End If

                strTemp = Session("ChatMsg" & IdCom) & strTemp
                Session("ChatMsg" & IdCom) = strTemp
                'Session("ChatMsg" & IdCom).Timeout(5)

                Me.lblout.Text = Session("ChatMsg" & IdCom)
                Me.lblout.DataBind()
            End If
        Catch
            Me.SetLblOut(1)
        End Try

     

    End Sub

    Private Function faccine(ByVal StrMsg As String) As String
        If False Then
            'Vecchio codice che utilizza l'xml...
            Dim DSEmo As New DataSet

            Try
                Dim StrXMLDataPath As String = Request.MapPath(GetPercorsoApplicazione(me.Request) & "/uc/Emoticon.xml") '"Emoticon.xml" 'GetPercorsoApplicazione(me.Request) & "/Emoticon.xml"
                DSEmo.ReadXml(StrXMLDataPath)


                Dim row As DataRow

                Dim Stringa1 As String
                Dim Stringa2 As String


                For Each row In DSEmo.Tables(0).Rows

                    Stringa1 = row.Item("Short")
                    Stringa2 = "<img src='./../images/forum/smile/" & row.Item("Img") & "'></a>"
                    StrMsg = StrMsg.Replace(Stringa1, Stringa2)

                Next

            Catch ex As Exception
                Dim smile(22) As String
                Dim smileimages(22) As String
                smile(1) = ":-)"
                smile(2) = ":D"
                smile(3) = ":-O"
                smile(4) = ":-p"
                smile(5) = ";-)"
                smile(6) = "(H)"
                smile(7) = ":$"
                smile(8) = "|-)"
                smile(9) = ":("
                smile(10) = ";-("
                smile(11) = ":S"
                smile(12) = ":@"
                smile(13) = "(*)"
                smile(14) = "(L)"
                smile(15) = "(U)"
                smile(16) = "(Y)"
                smile(17) = "(N)"
                smile(18) = "(pp)"
                smile(19) = "8-)"
                smile(20) = "(6)"
                smile(21) = "(?)"

                smileimages(1) = "smiley1.gif"
                smileimages(2) = "smiley4.gif"
                smileimages(3) = "smiley3.gif"
                smileimages(4) = "smiley17.gif"
                smileimages(5) = "smiley2.gif"
                smileimages(6) = "smiley16.gif"
                smileimages(7) = "smiley9.gif"
                smileimages(8) = "smiley12.gif"
                smileimages(9) = "smiley6.gif"
                smileimages(10) = "smiley19.gif"
                smileimages(11) = "smiley5.gif"
                smileimages(12) = "smiley7.gif"
                smileimages(13) = "smiley10.gif"
                smileimages(14) = "smiley27.gif"
                smileimages(15) = "smiley28.gif"
                smileimages(16) = "smiley20.gif"
                smileimages(17) = "smiley21.gif"
                smileimages(18) = "smiley32.gif"
                smileimages(19) = "smiley23.gif"
                smileimages(20) = "smiley15.gif"
                smileimages(21) = "smiley25.gif"

                Dim i As Integer
                For i = 1 To 21
                    StrMsg = StrMsg.Replace(smile(i), "<img src=""./../images/forum/smile/" & smileimages(i) & """>")
                Next
            End Try
        Else
            Dim smile(22) As String
            Dim smileimages(22) As String
            smile(1) = ":-)"
            smile(2) = ":D"
            smile(3) = ":-O"
            smile(4) = ":-p"
            smile(5) = ";-)"
            smile(6) = "(H)"
            smile(7) = ":$"
            smile(8) = "|-)"
            smile(9) = ":("
            smile(10) = ";-("
            smile(11) = ":S"
            smile(12) = ":@"
            smile(13) = "(*)"
            smile(14) = "(L)"
            smile(15) = "(U)"
            smile(16) = "(Y)"
            smile(17) = "(N)"
            smile(18) = "(pp)"
            smile(19) = "8-)"
            smile(20) = "(6)"
            smile(21) = "(?)"

            smileimages(1) = "smiley1.gif"
            smileimages(2) = "smiley4.gif"
            smileimages(3) = "smiley3.gif"
            smileimages(4) = "smiley17.gif"
            smileimages(5) = "smiley2.gif"
            smileimages(6) = "smiley16.gif"
            smileimages(7) = "smiley9.gif"
            smileimages(8) = "smiley12.gif"
            smileimages(9) = "smiley6.gif"
            smileimages(10) = "smiley19.gif"
            smileimages(11) = "smiley5.gif"
            smileimages(12) = "smiley7.gif"
            smileimages(13) = "smiley10.gif"
            smileimages(14) = "smiley27.gif"
            smileimages(15) = "smiley28.gif"
            smileimages(16) = "smiley20.gif"
            smileimages(17) = "smiley21.gif"
            smileimages(18) = "smiley32.gif"
            smileimages(19) = "smiley23.gif"
            smileimages(20) = "smiley15.gif"
            smileimages(21) = "smiley25.gif"

            Dim i As Integer
            For i = 1 To 21
                StrMsg = StrMsg.Replace(smile(i), "<img src=""./../images/forum/smile/" & smileimages(i) & """>")
            Next
        End If
        Return StrMsg

    End Function

#Region "Errori e localizzazioni"

    Private Sub SetLblOut(ByVal code As Integer)
        'In caso di errori imposta il messaggio
        Select Case code
            Case 1  'Errori web-server: solitamente l'utante non ha accesso o e' stato escluso
                'Me.lblout.Text = "L'amministratore ha ritenuto opportuno escluderti dalla chat<br>o il servizio non è momentaneamente disponibile<br>Attendere 5 minuti e riprovare."

            Case 2  'Non e' stato ricevuto un ID Comunita: probabile tentativo di accesso non corretto...
                'Me.lblout.Text = "Accesso non corretto al servizio."
                Try
                    Me.lblout.Text = Me.oResource.getValue("Error2")
                Catch ex As Exception
                    Me.lblout.Text = ""
                End Try
        End Select
    End Sub

    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_Chat_Ext"
        oResource.Folder_Level1 = "Chat_Messenger"
        oResource.setCulture()
    End Sub

    Private Sub SetupInternazionalizzazione()
        With oResource
        End With
    End Sub

#End Region
End Class