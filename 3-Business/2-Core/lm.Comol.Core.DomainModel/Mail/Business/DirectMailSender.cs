using System.Net.Mail;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

namespace lm.Comol.Core.DomainModel
{
	public class DirectMailSender
	{

		#region "Private"
		//Private _Sender As MailAddress
		private MailAddress _ReplyTo;
		private bool _SendCopyToSender;
		private string _Pre_DefaultSubject;
		private MailAddress _RealSenderMailAdress;
		private string _ServerSMTP;
			#endregion

        private SmtpClient _SMTP;
		#region "Proprietà"
	        public MailAddress SenderAddress {
			    get { return _ReplyTo; }
		    }
		    public MailAddress ReplyTo {
                get { return _ReplyTo; }
		    }
            private Int32 HostPort { get; set; }
            private String HostName { get; set; }
            private String AccountName { get; set; }
            private String AccountPassword { get; set; }
            private Boolean EnableSsl { get; set; }
            private Boolean EnabledAuthentication { get; set; }
            public MailAddressCollection AddressTo { get; set; }
            public MailAddressCollection AddressCC { get; set; }
            public MailAddressCollection AddressCCN { get; set; }
            public ArrayList Attachments { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
            public bool NotifyMailRead { get; set; }
            private SmtpClient SMTP {
                get
                {
                    if ((_SMTP == null))
                    {
                        SmtpClient smtpClient = new SmtpClient(HostName, HostPort);
                        smtpClient.EnableSsl = EnableSsl;
                        if (EnabledAuthentication && !String.IsNullOrEmpty(AccountName))
                            smtpClient.Credentials = new System.Net.NetworkCredential(AccountName, AccountPassword);
                        _SMTP = smtpClient;
                    }
                    return _SMTP;
                }
            }
		#endregion


        private DirectMailSender(string host, Int32 hostPort, Boolean enableSsl, Boolean enabledAuthentication, String username, String password)
		{
			this.AddressTo = new MailAddressCollection();
			this.AddressCC = new MailAddressCollection();
			this.AddressCCN = new MailAddressCollection();
			this.Attachments = new ArrayList();
			this._Pre_DefaultSubject = "";
			this.NotifyMailRead = false;
            HostName = host;
            HostPort = hostPort;
            EnableSsl = enableSsl;
            EnabledAuthentication = enabledAuthentication;
            if (enabledAuthentication && String.IsNullOrEmpty(username))
                EnabledAuthentication = false;
            else {
                AccountName = username;
                AccountPassword = password;
            }
		}

        public DirectMailSender(string host, Int32 hostPort, Boolean enableSsl, Boolean enabledAuthentication, String username, String password, string SubjectPrefix, MailAddress mailSenderAccount)
            : this(host, hostPort, enableSsl, enabledAuthentication, username, password)
		{
			this._RealSenderMailAdress = mailSenderAccount;
			this._Pre_DefaultSubject = SubjectPrefix;
			//                Me._Settings = oSettings
			//                Me._ReplyTo = oSettings.

		}
        public DirectMailSender(string host, Int32 hostPort, Boolean enableSsl, Boolean enabledAuthentication, String username, String password, string SubjectPrefix, MailAddress mailSenderAccount, MailAddress RealSenderAccount)
            : this(host, hostPort, enableSsl, enabledAuthentication, username, password)
		{
			this._RealSenderMailAdress = RealSenderAccount;
			this._Pre_DefaultSubject = SubjectPrefix;
			this._ReplyTo = mailSenderAccount;
		}

		//Public Sub InviaMail()
		//    Dim oMailMessage As New MailMessage
		//    Dim oSmtp As New System.Net.Mail.SmtpClient

		//    If Me._NotificaRicezione Then
		//        Try
		//            If Me._ReplyTo.DisplayName <> "" Then
		//                oMailMessage.Headers.Add("Disposition-Notification-To", Me._ReplyTo.DisplayName & " <" & Me._ReplyTo.Address & ">")
		//            Else
		//                oMailMessage.Headers.Add("Disposition-Notification-To", "<" & Me._ReplyTo.Address & ">")
		//            End If
		//        Catch ex As Exception

		//        End Try
		//    End If
		//    oSmtp = New SmtpClient(Me._Settings.ServerSMTP)
		//    oMailMessage.From = Me._RealSenderMailAdress
		//    oMailMessage.ReplyTo = Me._ReplyTo
		//    If InStr(Me._Oggetto, Me._PreSystemSubject) <= 0 Then
		//        oMailMessage.Subject = Me._PreSystemSubject & " " & Me._Oggetto
		//    Else
		//        oMailMessage.Subject = Me._Oggetto
		//    End If
		//    oMailMessage.Body = Me._Body
		//    oMailMessage.IsBodyHtml = False
		//    If Not IsNothing(Me._Attach) Then
		//        If Me._Attach.Count > 0 Then
		//            Dim file As String
		//            For Each file In Me._Attach
		//                Dim oAttach As Attachment = New Attachment(file)

		//                oMailMessage.Attachments.Add(oAttach)
		//            Next
		//        End If
		//    End If
		//    Try
		//        Dim oMailAddress As MailAddress

		//        If Me._A.Count > 0 Then
		//            For Each oMailAddress In Me._A
		//                oMailMessage.To.Add(oMailAddress)
		//            Next
		//        End If
		//        If Me._CC.Count > 0 Then
		//            For Each oMailAddress In Me._CC
		//                oMailMessage.CC.Add(oMailAddress)
		//            Next
		//        End If
		//        If Me._CCN.Count > 0 Then
		//            For Each oMailAddress In Me._CCN
		//                oMailMessage.Bcc.Add(oMailAddress)
		//            Next
		//        End If

		//        oSmtp.Send(oMailMessage)
		//        _Errore = Errori_Db.None

		//    Catch ex As Exception
		//        'Dim errore As String
		//        'errore = "The following exception occurred: " + ex.ToString()
		//        'errore = errore & vbCrLf

		//        'While Not (ex.InnerException Is Nothing)
		//        '    errore = errore & "--------------------------------" & vbCrLf
		//        '    errore = errore & "The following InnerException reported: " + ex.InnerException.ToString() & vbCrLf
		//        '    ex = ex.InnerException
		//        'End While

		//        _Errore = Errori_Db.System
		//    End Try

		//    If Me._HasCopiaMittente And _Errore = Errori_Db.None Then
		//        Me.InviaCopiaMittente()
		//    End If
		//End Sub

        private MailMessage SendInternalMail(int MaxRecipients, bool asHtml)
		{
			MailMessage oMailMessage = new MailMessage();
			if (this.NotifyMailRead) {
				try {
					if (!string.IsNullOrEmpty(this._ReplyTo.DisplayName)) {
						oMailMessage.Headers.Add("Disposition-Notification-To", this._ReplyTo.DisplayName + " <" + this._ReplyTo.Address + ">");
					} else {
						oMailMessage.Headers.Add("Disposition-Notification-To", "<" + this._ReplyTo.Address + ">");
					}

				} catch (Exception ex) {
				}
			}

			oMailMessage.From = this._RealSenderMailAdress;
			oMailMessage.ReplyTo = this._ReplyTo;
			if (this.Subject.StartsWith(this._Pre_DefaultSubject))
			    oMailMessage.Subject = this.Subject;	
			else 
                oMailMessage.Subject = this._Pre_DefaultSubject + " " + this.Subject;

            oMailMessage.Body = (asHtml ? this.Body.Replace("\r\n","<br>") : this.Body);
			oMailMessage.IsBodyHtml = asHtml;
			if ((this.Attachments != null)) {
				if (this.Attachments.Count > 0) {
					foreach (string file in this.Attachments) {
						Attachment oAttach = new Attachment(file);

						oMailMessage.Attachments.Add(oAttach);
					}
				}
			}
            try
            {
                List<dtoRecipients> oList = (from MailAddress a in AddressTo
                                             select new dtoRecipients
                                             {
                                                 Type = dtoRecipients.TypeRecipeints.AddressTo,
                                                 Mail = a
                                             }).ToList();
                List<dtoRecipients> oListCC = (from MailAddress cc in AddressCC
                                               select new dtoRecipients
                                               {
                                                   Type = dtoRecipients.TypeRecipeints.AddressCC,
                                                   Mail = cc
                                               }).ToList();
                List<dtoRecipients> oListCCN = (from MailAddress ccn in AddressCCN
                                                select new dtoRecipients
                                                {
                                                    Type = dtoRecipients.TypeRecipeints.AddressCCN,
                                                    Mail = ccn
                                                }).ToList();
                if (oListCC.Count > 0)
                {
                    oList.AddRange(oListCC);
                }
                if (oListCCN.Count > 0)
                {
                    oList.AddRange(oListCCN);
                }

                int Total = oList.Count;
                int PageNumber = 0;
                if (oList.Count / MaxRecipients < 1)
                {
                    PageNumber = 0;
                }
                else
                {
                    PageNumber = (oList.Count % MaxRecipients == 0 ? oList.Count / MaxRecipients - 1 : (oList.Count / MaxRecipients));
                }
                for (int i = 0; i <= PageNumber; i++)
                {
                    List<dtoRecipients> oTemp = oList.Skip(MaxRecipients * i).Take(MaxRecipients).ToList();

                    if (oTemp.Count > 0)
                    {
                        oMailMessage.To.Clear();
                        oMailMessage.CC.Clear();
                        oMailMessage.Bcc.Clear();
                        foreach (MailAddress oMailAddress in (from dtoRecipients m in oTemp where m.Type == dtoRecipients.TypeRecipeints.AddressTo select m.Mail).ToList())
                        {
                            oMailMessage.To.Add(oMailAddress);
                        }
                        foreach (MailAddress oMailAddress in (from dtoRecipients m in oTemp where m.Type == dtoRecipients.TypeRecipeints.AddressCC select m.Mail).ToList())
                        {
                            oMailMessage.CC.Add(oMailAddress);
                        }
                        foreach (MailAddress oMailAddress in (from dtoRecipients m in oTemp where m.Type == dtoRecipients.TypeRecipeints.AddressCCN select m.Mail).ToList())
                        {
                            oMailMessage.Bcc.Add(oMailAddress);
                        }

                        this.SMTP.Send(oMailMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                oMailMessage = null;
            }
            finally
            {
                //per il dispose di tutto il smtp client, dobbiamo passare ad .NET 4.0
                foreach (Attachment attachment in oMailMessage.Attachments)
                {
                    attachment.Dispose();
                }
                oMailMessage.Attachments.Dispose();
                oMailMessage = null;
            }
			return oMailMessage; //ha ancora senso?
		}

		public void SendMail(string ToAdresses)
		{
            SendInternalMail(ToAdresses, false);
		}
        //Added Tia
        public void SendMail(string ToAdresses,bool isHtml )
        {
            SendInternalMail(ToAdresses, isHtml);
        }

        private void SendInternalMail(string ToAdresses, bool isHtml)
        {
            if (ToAdresses.Contains(";"))
            {
                foreach (String address in ToAdresses.Split(';'))
                {
                    if (!String.IsNullOrEmpty(address))
                        this.AddressCCN.Add(new MailAddress(address));
                }
            }
            else if (ToAdresses.Contains(","))
            {
                foreach (String address in ToAdresses.Split(','))
                {
                    if (!String.IsNullOrEmpty(address))
                        this.AddressCCN.Add(new MailAddress(address));
                }
            }
            else
            {
                this.AddressTo.Add(new MailAddress(ToAdresses));
            }

            SendInternalMail(int.MaxValue - 1, isHtml);
        }
        //

		public void SendMail(int MaxRecipients, bool asHtml)
		{
			SendInternalMail(MaxRecipients, asHtml);
		}
		public void SendMailWithCopyTosender(string SubjectForSenderCopy, int MaxRecipients, bool asHtml)
		{
			MailMessage oMailMessage = SendInternalMail(MaxRecipients, asHtml);
			if ((oMailMessage != null)) {
				MailMessage oSenderMessage = new MailMessage();

				oSenderMessage.IsBodyHtml = asHtml;
				oSenderMessage.From = this._RealSenderMailAdress;
				oSenderMessage.ReplyTo = this.ReplyTo;
				oSenderMessage.To.Add(this.ReplyTo);
                if (Subject.StartsWith(_Pre_DefaultSubject))
                    oSenderMessage.Subject = " " + SubjectForSenderCopy + this.Subject;
                else
                    oSenderMessage.Subject = this._Pre_DefaultSubject + " " + SubjectForSenderCopy + this.Subject;

                string stringa = "\r\n";
			//	MailAddress oMailAddress = default(MailAddress);

				if (this.AddressTo.Count > 0) {
					stringa += "To:";
					foreach (MailAddress oMailAddress in this.AddressTo) {
						stringa += oMailAddress.DisplayName + "; ";
						// & " <" & oMailAddress.Address & ">" & "; "
					}
                    stringa += "\r\n";
				}
				if (this.AddressCC.Count > 0) {
					stringa += "Cc:";
					foreach (MailAddress oMailAddress in this.AddressCC) {
						stringa += oMailAddress.DisplayName + "; ";
						// & " <" & oMailAddress.Address & ">" & "; "
					}
                    stringa += "\r\n";
				}
				if (this.AddressCCN.Count > 0) {
					stringa += "Ccn:";
					foreach (MailAddress oMailAddress in this.AddressCCN) {
						stringa += oMailAddress.DisplayName + "; ";
						// & " <" & oMailAddress.Address & ">" & "; "
					}
                    stringa += "\r\n";
				}

				stringa += "Body:" + this.Body;
				oSenderMessage.Body = stringa;
				oSenderMessage.IsBodyHtml = false;
				if (this.Attachments.Count != 0) {
                    foreach (string file in this.Attachments)
                    {
						Attachment oAttach = new Attachment(file);
						oSenderMessage.Attachments.Add(oAttach);
					}
				}
				try {
					this.SMTP.Send(oSenderMessage);

				} catch (Exception ex) {
				}
			}
		}


		//        Public Sub InviaMailHTML()
		//            Dim oMailMessage As New MailMessage

		//            If Me._NotificaRicezione Then
		//                If Me._ReplyTo.DisplayName <> "" Then
		//                    oMailMessage.Headers.Add("Disposition-Notification-To", Me._ReplyTo.DisplayName & " <" & Me._ReplyTo.Address & ">")
		//                Else
		//                    oMailMessage.Headers.Add("Disposition-Notification-To", "<" & Me._ReplyTo.Address & ">")
		//                End If
		//            End If

		//            oMailMessage.From = Me._RealSenderMailAdress
		//            oMailMessage.ReplyTo = Me._ReplyTo
		//            If InStr(Me._Oggetto, Me._PreSystemSubject) <= 0 Then
		//                oMailMessage.Subject = Me._PreSystemSubject & " " & Me._Oggetto
		//            Else
		//                oMailMessage.Subject = Me._Oggetto
		//            End If

		//            Dim oMailAddress As MailAddress
		//            If Me._A.Count > 0 Then
		//                For Each oMailAddress In Me._A
		//                    oMailMessage.To.Add(oMailAddress)
		//                Next
		//            End If
		//            If Me._CC.Count > 0 Then
		//                For Each oMailAddress In Me._CC
		//                    oMailMessage.CC.Add(oMailAddress)
		//                Next
		//            End If
		//            If Me._CCN.Count > 0 Then
		//                For Each oMailAddress In Me._CCN
		//                    oMailMessage.Bcc.Add(oMailAddress)
		//                Next
		//            End If

		//            oMailMessage.Body = Me._Body
		//            oMailMessage.Body = Replace(oMailMessage.Body, vbCrLf, "<br>")
		//            oMailMessage.IsBodyHtml = True
		//            If Not IsNothing(Me._Attach) Then
		//                If Me._Attach.Count > 0 Then
		//                    Dim file As String
		//                    For Each file In Me._Attach
		//                        Dim oAttach As Attachment = New Attachment(file)

		//                        oMailMessage.Attachments.Add(oAttach)
		//                    Next
		//                End If
		//            End If
		//            Try
		//                Dim oSmtp As New System.Net.Mail.SmtpClient
		//                oSmtp = New SmtpClient(Me._Settings.ServerSMTP)
		//                oSmtp.Send(oMailMessage)
		//                _Errore = Errori_Db.None

		//            Catch ex As Exception
		//                _Errore = Errori_Db.System
		//            End Try

		//            If Me._HasCopiaMittente And _Errore = Errori_Db.None Then
		//                Me.InviaCopiaMittenteHTML()
		//            End If
		//        End Sub

		//        Private Sub InviaCopiaMittenteHTML()
		//            Dim oMailMessage As New MailMessage

		//            oMailMessage.From = Me._RealSenderMailAdress
		//            oMailMessage.ReplyTo = Me._ReplyTo
		//            oMailMessage.To.Add(Me._ReplyTo)
		//            If InStr(Me._Oggetto, Me._PreSystemSubject) <= 0 Then
		//                oMailMessage.Subject = Me._PreSystemSubject & " " & Me._Settings.SubjectForSenderCopy & Me._Oggetto
		//            Else
		//                oMailMessage.Subject = " " & Me._Settings.SubjectForSenderCopy & Me._Oggetto
		//            End If

		//            Dim stringa As String = "E-mail spedita " & vbCrLf
		//            Dim oMailAddress As MailAddress
		//            If Me._A.Count > 0 Then
		//                stringa += "a:"
		//                For Each oMailAddress In Me._A
		//                    stringa += oMailAddress.DisplayName & " <" & oMailAddress.Address & ">" & "; "
		//                Next
		//                stringa += vbCrLf
		//            End If
		//            If Me._CC.Count > 0 Then
		//                stringa += "Cc:"
		//                For Each oMailAddress In Me._CC
		//                    stringa += oMailAddress.DisplayName & " <" & oMailAddress.Address & ">" & "; "
		//                Next
		//                stringa += vbCrLf
		//            End If
		//            If Me._CCN.Count > 0 Then
		//                stringa += "Ccn:"
		//                For Each oMailAddress In Me._CCN
		//                    stringa += oMailAddress.DisplayName & " <" & oMailAddress.Address & ">" & "; "
		//                Next
		//                stringa += vbCrLf
		//            End If
		//            stringa += "Testo:" & Me.Body
		//            oMailMessage.Body = stringa
		//            oMailMessage.IsBodyHtml = True
		//            If Not IsNothing(Me._Attach) Then
		//                If Me._Attach.Count <> 0 Then
		//                    Dim file As String
		//                    For Each file In Me._Attach
		//                        Dim oAttach As Attachment = New Attachment(file)

		//                        oMailMessage.Attachments.Add(oAttach)
		//                    Next
		//                End If
		//            End If

		//            Try
		//                Dim oSmtp As New System.Net.Mail.SmtpClient
		//                oSmtp = New SmtpClient(Me._Settings.ServerSMTP)
		//                oSmtp.Send(oMailMessage)
		//            Catch ex As Exception

		//            End Try

		//        End Sub

		//        Private Class dtoRecipients
		//            Public Mail As MailAddress
		//            Public Type As TypeRecipeints

		//            Public Enum TypeRecipeints
		//                A = 1
		//                CC = 2
		//                CCN = 3
		//            End Enum
		//        End Class

		private class dtoRecipients
		{
			public MailAddress Mail;

			public TypeRecipeints Type;
			public enum TypeRecipeints
			{
				AddressTo = 1,
				AddressCC = 2,
				AddressCCN = 3
			}
		}
	}
}