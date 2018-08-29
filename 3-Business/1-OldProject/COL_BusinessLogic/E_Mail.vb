Imports System.Net.Mail
Imports COL_BusinessLogic_v2.Localizzazione

Public Class COL_E_Mail

#Region "Private"
	Private _IDmittente As Integer
	'Private _Sender As MailAddress
	Private _ReplyTo As MailAddress
	Private _A As MailAddressCollection
	Private _CC As MailAddressCollection
	Private _CCN As MailAddressCollection
	Private _Oggetto As String
	Private _Body As String
	Private _NotificaRicezione As Boolean
	Private _HasCopiaMittente As Boolean
	Private _PreSystemSubject As String
	Private _Attach As ArrayList
	Private _Errore As Errori_Db
	Private _Settings As MailLocalized
	Private _RealSenderMailAdress As MailAddress

#End Region

#Region "Proprietà"
	Public Property Mittente() As MailAddress
		Get
			Mittente = _ReplyTo
		End Get
		Set(ByVal Value As MailAddress)
			_ReplyTo = Value
			If Me._Settings.SendMailByReply Then
				_RealSenderMailAdress = Me._Settings.RealMailSenderAccount
			Else
				_RealSenderMailAdress = _ReplyTo
			End If
		End Set
	End Property
	Public ReadOnly Property ReplyTo() As MailAddress
		Get
			ReplyTo = _ReplyTo
		End Get
	End Property

	Public Property IndirizziTO() As MailAddressCollection
		Get
			IndirizziTO = _A
		End Get
		Set(ByVal Value As MailAddressCollection)
			_A = Value
		End Set
	End Property
	Public Property IndirizziCC() As MailAddressCollection
		Get
			IndirizziCc = _CC
		End Get
		Set(ByVal Value As MailAddressCollection)
			_CC = Value
		End Set
	End Property
	Public Property IndirizziCCN() As MailAddressCollection
		Get
			IndirizziCcn = _CCN
		End Get
		Set(ByVal Value As MailAddressCollection)
			_CCN = Value
		End Set
	End Property
	Public Property Attachment() As ArrayList
		Get
			Attachment = _Attach
		End Get
		Set(ByVal Value As ArrayList)
			_Attach = Value
		End Set
	End Property
	Public Property Oggetto() As String
		Get
			Oggetto = _Oggetto
		End Get
		Set(ByVal Value As String)
			_Oggetto = Value
		End Set
	End Property
	Public Property Body() As String
		Get
			Body = _Body
		End Get
		Set(ByVal Value As String)
			_Body = Value
		End Set
	End Property
	Public Property NotificaRicezione() As Boolean
		Get
			NotificaRicezione = _NotificaRicezione
		End Get
		Set(ByVal Value As Boolean)
			_NotificaRicezione = Value
		End Set
	End Property
	Public Property HasCopiaMittente() As Boolean
		Get
			HasCopiaMittente = _HasCopiaMittente
		End Get
		Set(ByVal Value As Boolean)
			_HasCopiaMittente = Value
		End Set
	End Property
	Public ReadOnly Property Errore() As Errori_Db
		Get
			Errore = _Errore
		End Get
	End Property
#End Region

	Public Sub New()
		Me._A = New MailAddressCollection
		Me._CC = New MailAddressCollection
		Me._CCN = New MailAddressCollection
	End Sub

	Public Sub New(ByVal oSettings As maillocalized)
		Try
			Me._PreSystemSubject = oSettings.SubjectPrefix
			Me._A = New MailAddressCollection
			Me._CC = New MailAddressCollection
			Me._CCN = New MailAddressCollection
			Me._Attach = New ArrayList
			Me._Settings = oSettings
			Me._ReplyTo = oSettings.RealMailSenderAccount
		Catch ex As Exception
			Me._PreSystemSubject = ""
		End Try
		Me._NotificaRicezione = False
	End Sub


	Public Sub InviaMail()
		Dim oMailMessage As New MailMessage
		Dim oSmtp As New System.Net.Mail.SmtpClient

		If Me._NotificaRicezione Then
			Try
				If Me._ReplyTo.DisplayName <> "" Then
					oMailMessage.Headers.Add("Disposition-Notification-To", Me._ReplyTo.DisplayName & " <" & Me._ReplyTo.Address & ">")
				Else
					oMailMessage.Headers.Add("Disposition-Notification-To", "<" & Me._ReplyTo.Address & ">")
				End If
			Catch ex As Exception

			End Try
		End If
        oSmtp = New SmtpClient(Me._Settings.ServerSMTP, Me._Settings.HostPort)
        oSmtp.EnableSsl = Me._Settings.UseSsl
        If (Me._Settings.AuthenticationEnabled) AndAlso Not String.IsNullOrEmpty(Me._Settings.CredentialsUsername) Then
            oSmtp.Credentials = New System.Net.NetworkCredential(Me._Settings.CredentialsUsername, Me._Settings.CredentialsPassword)
        End If
		oMailMessage.From = Me._RealSenderMailAdress
		oMailMessage.ReplyTo = Me._ReplyTo
		If InStr(Me._Oggetto, Me._PreSystemSubject) <= 0 Then
			oMailMessage.Subject = Me._PreSystemSubject & " " & Me._Oggetto
		Else
			oMailMessage.Subject = Me._Oggetto
		End If
        oMailMessage.Body = Me._Body
		oMailMessage.IsBodyHtml = False
		If Not IsNothing(Me._Attach) Then
			If Me._Attach.Count > 0 Then
				Dim file As String
				For Each file In Me._Attach
					Dim oAttach As Attachment = New Attachment(file)

					oMailMessage.Attachments.Add(oAttach)
				Next
			End If
		End If
        Try
            Dim oMailAddress As MailAddress

            If Me._A.Count > 0 Then
                For Each oMailAddress In Me._A
                    oMailMessage.To.Add(oMailAddress)
                Next
            End If
            If Me._CC.Count > 0 Then
                For Each oMailAddress In Me._CC
                    oMailMessage.CC.Add(oMailAddress)
                Next
            End If
            If Me._CCN.Count > 0 Then
                For Each oMailAddress In Me._CCN
                    oMailMessage.Bcc.Add(oMailAddress)
                Next
            End If

            oSmtp.Send(oMailMessage)
            _Errore = Errori_Db.None

        Catch ex As Exception
            'Dim errore As String
            'errore = "The following exception occurred: " + ex.ToString()
            'errore = errore & vbCrLf

            'While Not (ex.InnerException Is Nothing)
            '    errore = errore & "--------------------------------" & vbCrLf
            '    errore = errore & "The following InnerException reported: " + ex.InnerException.ToString() & vbCrLf
            '    ex = ex.InnerException
            'End While

            _Errore = Errori_Db.System
        End Try

		If Me._HasCopiaMittente And _Errore = Errori_Db.None Then
			Me.InviaCopiaMittente()
		End If
    End Sub
    Public Sub SendMailWithRecipientsLimit(ByVal MaxRecipients As Integer)
        Dim oMailMessage As New MailMessage
        Dim oSmtp As New System.Net.Mail.SmtpClient

        If Me._NotificaRicezione Then
            Try
                If Me._ReplyTo.DisplayName <> "" Then
                    oMailMessage.Headers.Add("Disposition-Notification-To", Me._ReplyTo.DisplayName & " <" & Me._ReplyTo.Address & ">")
                Else
                    oMailMessage.Headers.Add("Disposition-Notification-To", "<" & Me._ReplyTo.Address & ">")
                End If
            Catch ex As Exception

            End Try
        End If
        oSmtp = New SmtpClient(Me._Settings.ServerSMTP, Me._Settings.HostPort)
        oSmtp.EnableSsl = Me._Settings.UseSsl
        If (Me._Settings.AuthenticationEnabled) AndAlso Not String.IsNullOrEmpty(Me._Settings.CredentialsUsername) Then
            oSmtp.Credentials = New System.Net.NetworkCredential(Me._Settings.CredentialsUsername, Me._Settings.CredentialsPassword)
        End If
          

        oMailMessage.From = Me._RealSenderMailAdress
        oMailMessage.ReplyTo = Me._ReplyTo
        If InStr(Me._Oggetto, Me._PreSystemSubject) <= 0 Then
            oMailMessage.Subject = Me._PreSystemSubject & " " & Me._Oggetto
        Else
            oMailMessage.Subject = Me._Oggetto
        End If
        oMailMessage.Body = Me._Body
        oMailMessage.IsBodyHtml = False
        If Not IsNothing(Me._Attach) Then
            If Me._Attach.Count > 0 Then
                Dim file As String
                For Each file In Me._Attach
                    Dim oAttach As Attachment = New Attachment(file)

                    oMailMessage.Attachments.Add(oAttach)
                Next
            End If
        End If
        Try
            Dim oList As List(Of dtoRecipients) = (From a In _A Select New dtoRecipients() With {.Type = dtoRecipients.TypeRecipeints.A, .Mail = a}).ToList
            Dim oListCC As List(Of dtoRecipients) = (From cc In _CC Select New dtoRecipients() With {.Type = dtoRecipients.TypeRecipeints.CC, .Mail = cc}).ToList
            Dim oListCCN As List(Of dtoRecipients) = (From ccn In _CCN Select New dtoRecipients() With {.Type = dtoRecipients.TypeRecipeints.CCN, .Mail = ccn}).ToList
            If oListCC.Count > 0 Then
                oList.AddRange(oListCC)
            End If
            If oListCCN.Count > 0 Then
                oList.AddRange(oListCCN)
            End If

            Dim Total As Integer = oList.Count
            Dim PageNumber As Integer
            If oList.Count / MaxRecipients < 1 Then
                PageNumber = 0
            Else
                PageNumber = IIf(oList.Count Mod MaxRecipients = 0, oList.Count / MaxRecipients - 1, (oList.Count / MaxRecipients))
            End If
            For i As Integer = 0 To PageNumber
                Dim oTemp As List(Of dtoRecipients) = oList.Skip(MaxRecipients * i).Take(MaxRecipients).ToList

                If oTemp.Count > 0 Then
                    oMailMessage.To.Clear()
                    oMailMessage.CC.Clear()
                    oMailMessage.Bcc.Clear()
                    For Each oMailAddress In (From m In oTemp Where m.Type = dtoRecipients.TypeRecipeints.A Select m.Mail).ToList
                        oMailMessage.To.Add(oMailAddress)
                    Next
                    For Each oMailAddress In (From m In oTemp Where m.Type = dtoRecipients.TypeRecipeints.CC Select m.Mail).ToList
                        oMailMessage.CC.Add(oMailAddress)
                    Next
                    For Each oMailAddress In (From m In oTemp Where m.Type = dtoRecipients.TypeRecipeints.CCN Select m.Mail).ToList
                        oMailMessage.Bcc.Add(oMailAddress)
                    Next

                    oSmtp.Send(oMailMessage)
                End If
            Next
            _Errore = Errori_Db.None

        Catch ex As Exception
            'Dim errore As String
            'errore = "The following exception occurred: " + ex.ToString()
            'errore = errore & vbCrLf

            'While Not (ex.InnerException Is Nothing)
            '    errore = errore & "--------------------------------" & vbCrLf
            '    errore = errore & "The following InnerException reported: " + ex.InnerException.ToString() & vbCrLf
            '    ex = ex.InnerException
            'End While

            _Errore = Errori_Db.System
        End Try

        If Me._HasCopiaMittente And _Errore = Errori_Db.None Then
            Me.InviaCopiaMittente()
        End If
    End Sub
	Private Sub InviaCopiaMittente()
		Dim oMailMessage As New MailMessage


		oMailMessage.From = Me._RealSenderMailAdress
		oMailMessage.ReplyTo = Me._ReplyTo
		oMailMessage.To.Add(Me._ReplyTo)
		If InStr(Me._Oggetto, Me._PreSystemSubject) <= 0 Then
			oMailMessage.Subject = Me._PreSystemSubject & " " & Me._Settings.SubjectForSenderCopy & Me._Oggetto
		Else
			oMailMessage.Subject = " " & Me._Settings.SubjectForSenderCopy & Me._Oggetto
		End If


		Dim stringa As String = "E-mail spedita " & vbCrLf
		Dim oMailAddress As MailAddress

		If Me._A.Count > 0 Then
			stringa += "a:"
			For Each oMailAddress In Me._A
                stringa += oMailAddress.DisplayName & "; " ' & " <" & oMailAddress.Address & ">" & "; "
			Next
			stringa += vbCrLf
		End If
		If Me._CC.Count > 0 Then
			stringa += "Cc:"
			For Each oMailAddress In Me._CC
                stringa += oMailAddress.DisplayName & "; " ' & " <" & oMailAddress.Address & ">" & "; "
			Next
			stringa += vbCrLf
		End If
		If Me._CCN.Count > 0 Then
			stringa += "Ccn:"
			For Each oMailAddress In Me._CCN
                stringa += oMailAddress.DisplayName & "; " ' & " <" & oMailAddress.Address & ">" & "; "
			Next
			stringa += vbCrLf
		End If

		stringa += "Testo:" & Me.Body
		oMailMessage.Body = stringa
		oMailMessage.IsBodyHtml = False
		If Me._Attach.Count <> 0 Then
			Dim file As String
			For Each file In Me._Attach
				Dim oAttach As Attachment = New Attachment(file)

				oMailMessage.Attachments.Add(oAttach)
			Next
		End If

		Try
			Dim oSmtp As New System.Net.Mail.SmtpClient
            oSmtp = New SmtpClient(Me._Settings.ServerSMTP, Me._Settings.HostPort)
            oSmtp.EnableSsl = Me._Settings.UseSsl
            If (Me._Settings.AuthenticationEnabled) AndAlso Not String.IsNullOrEmpty(Me._Settings.CredentialsUsername) Then
                oSmtp.Credentials = New System.Net.NetworkCredential(Me._Settings.CredentialsUsername, Me._Settings.CredentialsPassword)
            End If
			oSmtp.Send(oMailMessage)
		Catch ex As Exception

		End Try
	End Sub
	Public Sub InviaMailHTML()
		Dim oMailMessage As New MailMessage

		If Me._NotificaRicezione Then
			If Me._ReplyTo.DisplayName <> "" Then
				oMailMessage.Headers.Add("Disposition-Notification-To", Me._ReplyTo.DisplayName & " <" & Me._ReplyTo.Address & ">")
			Else
				oMailMessage.Headers.Add("Disposition-Notification-To", "<" & Me._ReplyTo.Address & ">")
			End If
		End If

		oMailMessage.From = Me._RealSenderMailAdress
		oMailMessage.ReplyTo = Me._ReplyTo
		If InStr(Me._Oggetto, Me._PreSystemSubject) <= 0 Then
			oMailMessage.Subject = Me._PreSystemSubject & " " & Me._Oggetto
		Else
			oMailMessage.Subject = Me._Oggetto
		End If

		Dim oMailAddress As MailAddress
		If Me._A.Count > 0 Then
			For Each oMailAddress In Me._A
				oMailMessage.To.Add(oMailAddress)
			Next
		End If
		If Me._CC.Count > 0 Then
			For Each oMailAddress In Me._CC
				oMailMessage.CC.Add(oMailAddress)
			Next
		End If
		If Me._CCN.Count > 0 Then
			For Each oMailAddress In Me._CCN
				oMailMessage.Bcc.Add(oMailAddress)
			Next
		End If

		oMailMessage.Body = Me._Body
		oMailMessage.Body = Replace(oMailMessage.Body, vbCrLf, "<br>")
		oMailMessage.IsBodyHtml = True
		If Not IsNothing(Me._Attach) Then
			If Me._Attach.Count > 0 Then
				Dim file As String
				For Each file In Me._Attach
					Dim oAttach As Attachment = New Attachment(file)

					oMailMessage.Attachments.Add(oAttach)
				Next
			End If
		End If
		Try
			Dim oSmtp As New System.Net.Mail.SmtpClient
            oSmtp = New SmtpClient(Me._Settings.ServerSMTP, Me._Settings.HostPort)
            oSmtp.EnableSsl = Me._Settings.UseSsl
            If (Me._Settings.AuthenticationEnabled) AndAlso Not String.IsNullOrEmpty(Me._Settings.CredentialsUsername) Then
                oSmtp.Credentials = New System.Net.NetworkCredential(Me._Settings.CredentialsUsername, Me._Settings.CredentialsPassword)
            End If
			oSmtp.Send(oMailMessage)
			_Errore = Errori_Db.None

		Catch ex As Exception
			_Errore = Errori_Db.System
		End Try

		If Me._HasCopiaMittente And _Errore = Errori_Db.None Then
			Me.InviaCopiaMittenteHTML()
		End If
	End Sub

	Private Sub InviaCopiaMittenteHTML()
		Dim oMailMessage As New MailMessage

		oMailMessage.From = Me._RealSenderMailAdress
		oMailMessage.ReplyTo = Me._ReplyTo
		oMailMessage.To.Add(Me._ReplyTo)
		If InStr(Me._Oggetto, Me._PreSystemSubject) <= 0 Then
			oMailMessage.Subject = Me._PreSystemSubject & " " & Me._Settings.SubjectForSenderCopy & Me._Oggetto
		Else
			oMailMessage.Subject = " " & Me._Settings.SubjectForSenderCopy & Me._Oggetto
		End If

		Dim stringa As String = "E-mail spedita " & vbCrLf
		Dim oMailAddress As MailAddress
		If Me._A.Count > 0 Then
			stringa += "a:"
			For Each oMailAddress In Me._A
				stringa += oMailAddress.DisplayName & " <" & oMailAddress.Address & ">" & "; "
			Next
			stringa += vbCrLf
		End If
		If Me._CC.Count > 0 Then
			stringa += "Cc:"
			For Each oMailAddress In Me._CC
				stringa += oMailAddress.DisplayName & " <" & oMailAddress.Address & ">" & "; "
			Next
			stringa += vbCrLf
		End If
		If Me._CCN.Count > 0 Then
			stringa += "Ccn:"
			For Each oMailAddress In Me._CCN
				stringa += oMailAddress.DisplayName & " <" & oMailAddress.Address & ">" & "; "
			Next
			stringa += vbCrLf
		End If
		stringa += "Testo:" & Me.Body
		oMailMessage.Body = stringa
		oMailMessage.IsBodyHtml = True
		If Not IsNothing(Me._Attach) Then
			If Me._Attach.Count <> 0 Then
				Dim file As String
				For Each file In Me._Attach
					Dim oAttach As Attachment = New Attachment(file)

					oMailMessage.Attachments.Add(oAttach)
				Next
			End If
		End If

		Try
			Dim oSmtp As New System.Net.Mail.SmtpClient
            oSmtp = New SmtpClient(Me._Settings.ServerSMTP, Me._Settings.HostPort)
            oSmtp.EnableSsl = Me._Settings.UseSsl
            If (Me._Settings.AuthenticationEnabled) AndAlso Not String.IsNullOrEmpty(Me._Settings.CredentialsUsername) Then
                oSmtp.Credentials = New System.Net.NetworkCredential(Me._Settings.CredentialsUsername, Me._Settings.CredentialsPassword)
            End If
			oSmtp.Send(oMailMessage)
		Catch ex As Exception

		End Try

    End Sub

    Private Class dtoRecipients
        Public Mail As MailAddress
        Public Type As TypeRecipeints

        Public Enum TypeRecipeints
            A = 1
            CC = 2
            CCN = 3
        End Enum
    End Class
End Class