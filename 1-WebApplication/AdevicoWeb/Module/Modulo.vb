Imports COL_BusinessLogic_v2.UCServices
Module ModuloGenerale
    Public Function Permessi(ByVal Codice As String, ByVal oPage As System.Web.UI.Page) As String 'dal nome del servizio ricavo i permessi (credenziali) che l'utente ha,
        Dim i, dimensione As Integer
        Dim ArrPermessi(,) As String = oPage.Session("ArrPermessi")
        Dim PermessoUtente As String ' stringa di 32 bit contenente il valore dei permessi con cui l'utente entra nella pagina
        Dim nomeLink As String
        ' ArrPermessi(i, 0) = oRow.Item("SRVZ_Codice") 'CODICE servizio
        ' ArrPermessi(i, 1) = oRow.Item("SRVZ_ID") 'id servizio
        '  ArrPermessi(i, 2) = oRow.Item("LKSC_Permessi") 'valore servizio


        dimensione = UBound(ArrPermessi, 1) 'recupero il numero di servizi che l'utente si porta dietro nell'array
        For i = 0 To dimensione ' li scorro...

            If ArrPermessi(i, 0) = Codice Then
                PermessoUtente = ArrPermessi(i, 2) '... e tiro su il valore del permesso
                Exit For
            End If
        Next
        Return PermessoUtente
    End Function

    Public Function GetPermessiForService(ByVal Codice As String, ByVal oPage As System.Web.UI.Page) As String 'dal nome del servizio ricavo i permessi (credenziali) che l'utente ha,
        Dim i, dimensione As Integer
        Dim ArrPermessi(,) As String = oPage.Session("ArrPermessi")
        Dim PermessoUtente As String ' stringa di 32 bit contenente il valore dei permessi con cui l'utente entra nella pagina
        Dim nomeLink As String
        ' ArrPermessi(i, 0) = oRow.Item("SRVZ_Codice") 'CODICE servizio
        ' ArrPermessi(i, 1) = oRow.Item("SRVZ_ID") 'id servizio
        '  ArrPermessi(i, 2) = oRow.Item("LKSC_Permessi") 'valore servizio

        dimensione = UBound(ArrPermessi, 1) 'recupero il numero di servizi che l'utente si porta dietro nell'array
        PermessoUtente = "00000000000000000000000000000000"
        For i = 0 To dimensione ' li scorro...

            If ArrPermessi(i, 0) = Codice Then
                PermessoUtente = ArrPermessi(i, 2) '... e tiro su il valore del permesso
                Exit For
            End If
        Next
        Return PermessoUtente
    End Function

    '  Public Function CanRedirectToDefaultPage(ByVal Codice As String, ByVal CMNT_ID As Integer, ByVal PRSN_ID As Integer) As Boolean
    '      Dim Redirigi As Boolean = False
    '      Dim oComunita As COL_BusinessLogic_v2.Comunita.COL_Comunita

    '      If Codice = Services_Bacheca.Codex Then

    '          Try
    '		Dim oServizio As New Services_Bacheca(oComunita.GetPermessiForServizioByPersona(PRSN_ID, CMNT_ID, Codice))
    '              If oServizio.Admin Or oServizio.Read Or oServizio.Write Then
    '                  Redirigi = True
    '              End If
    '          Catch ex As Exception

    '          End Try
    '      ElseIf Codice = Services_CHAT.Codex Then
    '          Dim oServizio As New Services_CHAT
    '          Try
    '              oServizio.PermessiAssociati = oComunita.GetPermessiForServizioByPersona(PRSN_ID, CMNT_ID, Codice)
    '              If oServizio.Admin Or oServizio.Read Or oServizio.Write Then
    '                  Redirigi = True
    '              End If
    '          Catch ex As Exception

    '          End Try
    '      ElseIf Codice = Services_DiarioLezioni.Codex Then
    '          Dim oServizio As New Services_DiarioLezioni
    '          Try
    '              oServizio.PermessiAssociati = oComunita.GetPermessiForServizioByPersona(PRSN_ID, CMNT_ID, Codice)
    '              If oServizio.Admin Or oServizio.Read Or oServizio.Change Or oServizio.Upload Then
    '                  Redirigi = True
    '              End If
    '          Catch ex As Exception

    '          End Try
    '      ElseIf Codice = Services_Eventi.Codex Then
    '          Dim oServizio As New Services_Eventi
    '          Try
    '              oServizio.PermessiAssociati = oComunita.GetPermessiForServizioByPersona(PRSN_ID, CMNT_ID, Codice)
    '              If oServizio.AdminService Or oServizio.AddEvents Or oServizio.ChangeEvents Or oServizio.ReadEvents Then
    '                  Redirigi = True
    '              End If
    '          Catch ex As Exception

    '          End Try
    '      ElseIf Codice = Services_File.Codex Then
    '          Dim oServizio As New Services_File
    '          Try
    '              oServizio.PermessiAssociati = oComunita.GetPermessiForServizioByPersona(PRSN_ID, CMNT_ID, Codice)
    '              If oServizio.Admin Or oServizio.Moderate Or oServizio.Read Or oServizio.Upload Or oServizio.Change Then
    '                  Redirigi = True
    '              End If
    '          Catch ex As Exception

    '          End Try
    '      ElseIf Codice = Services_Forum.Codex Then
    '          Dim oServizio As New Services_Forum
    '          Try
    '              oServizio.PermessiAssociati = oComunita.GetPermessiForServizioByPersona(PRSN_ID, CMNT_ID, Codice)
    '              If oServizio.AccessoForum Or oServizio.GestioneForum Then
    '                  Redirigi = True
    '              End If
    '          Catch ex As Exception

    '          End Try
    '      ElseIf Codice = Services_Gallery.Codex Then
    '          Dim oServizio As New Services_Gallery
    '          Try
    '              oServizio.PermessiAssociati = oComunita.GetPermessiForServizioByPersona(PRSN_ID, CMNT_ID, Codice)
    '              If oServizio.Admin Or oServizio.Management Or oServizio.List Then
    '                  Redirigi = True
    '              End If
    '          Catch ex As Exception

    '          End Try
    '      ElseIf Codice = Services_RaccoltaLink.Codex Then
    '          Dim oServizio As New Services_RaccoltaLink
    '          Try
    '              oServizio.PermessiAssociati = oComunita.GetPermessiForServizioByPersona(PRSN_ID, CMNT_ID, Codice)
    '              If oServizio.Admin Or oServizio.Moderate Or oServizio.List Or oServizio.AddLink Then
    '                  Redirigi = True
    '              End If
    '          Catch ex As Exception

    '          End Try
    '      ElseIf Codice = Services_Statistiche.Codex Then
    '          Dim oServizio As New Services_Statistiche
    '          Try
    '              oServizio.PermessiAssociati = oComunita.GetPermessiForServizioByPersona(PRSN_ID, CMNT_ID, Codice)
    '              If oServizio.Admin Or oServizio.List Or oServizio.Management Then
    '                  Redirigi = True
    '              End If
    '          Catch ex As Exception

    '          End Try
    '      ElseIf Codice = Services_Listaiscritti.Codex Then
    '          Dim oServizio As New Services_Listaiscritti
    '          Try
    '              oServizio.PermessiAssociati = oComunita.GetPermessiForServizioByPersona(PRSN_ID, CMNT_ID, Codice)
    '              If oServizio.Management Or oServizio.List Or oServizio.Admin Then
    '                  Redirigi = True
    '              End If
    '          Catch ex As Exception

    '          End Try
    '      ElseIf Codice = Services_Cover.Codex Then
    '          Dim oServizio As New Services_Cover
    '          Try
    '              Dim oRuoloComunita As New COL_BusinessLogic_v2.Comunita.COL_RuoloPersonaComunita
    '              If Not oRuoloComunita.isSkipCover(CMNT_ID, PRSN_ID) Then
    '                  oServizio.PermessiAssociati = oComunita.GetPermessiForServizioByPersona(PRSN_ID, CMNT_ID, Codice)
    '                  If oServizio.Management Or oServizio.Read Or oServizio.Admin Then
    '                      Redirigi = True
    '                  End If
    '              End If
    '          Catch ex As Exception

    '	End Try
    'ElseIf Codice = Services_Wiki.Codex Then
    '	Dim oServizio As New Services_Wiki
    '	Try
    '		Dim oRuoloComunita As New COL_BusinessLogic_v2.Comunita.COL_RuoloPersonaComunita
    '		If Not oRuoloComunita.isSkipCover(CMNT_ID, PRSN_ID) Then
    '			oServizio.PermessiAssociati = oComunita.GetPermessiForServizioByPersona(PRSN_ID, CMNT_ID, Codice)
    '			If oServizio.Admin Or oServizio.Lettura Or oServizio.GestioneWiki Then
    '				Redirigi = True
    '			End If
    '		End If
    '	Catch ex As Exception

    '	End Try
    'End If
    '      Return Redirigi
    '  End Function

    'Public Function PlainRedirectToDefaultPage(ByVal CMNT_id As Integer, ByVal PRSN_ID As Integer) As String
    '	Dim urlDefault, Codice As String
    '	Dim DefaultPageID As Integer
    '	Dim hasDefaultPage As Boolean = False
    '	Dim urlRedirect As String = "Comunita/comunita.aspx"
    '	Dim oComunita As COL_BusinessLogic_v2.Comunita.COL_Comunita

    '	Try
    '		hasDefaultPage = oComunita.GetDefaultPage(CMNT_id, urlDefault, Codice, DefaultPageID)
    '		If hasDefaultPage AndAlso urlDefault <> "" Then
    '               Dim Redirigi As Boolean = False
    '               Redirigi = CanRedirectToDefaultPage(Codice, CMNT_id, PRSN_ID)
    '			If Redirigi Then
    '				urlDefault = Replace(urlDefault, "./", "")
    '				urlRedirect = urlDefault
    '			End If
    '		End If
    '	Catch ex As Exception

    '	End Try

    '	Return urlRedirect
    'End Function
    '   Public Function RedirectToDefaultPage(ByVal CMNT_id As Integer, ByVal PRSN_ID As Integer) As String
    '       Dim urlDefault, Codice As String
    '       Dim DefaultPageID As Integer
    '       Dim hasDefaultPage As Boolean = False
    '       Dim urlRedirect As String
    '       Dim oComunita As COL_BusinessLogic_v2.Comunita.COL_Comunita

    '       hasDefaultPage = oComunita.GetDefaultPage(CMNT_id, urlDefault, Codice, DefaultPageID)
    '       If Not hasDefaultPage Or urlDefault = "" Then
    '           urlRedirect = "./Comunita/comunita.aspx"
    '       Else
    '           Dim Redirigi As Boolean = False
    '           Redirigi = CanRedirectToDefaultPage(Codice, CMNT_id, PRSN_ID)
    '           If Redirigi Then
    '               urlRedirect = urlDefault
    '           Else
    '               urlRedirect = "./Comunita/comunita.aspx"
    '           End If
    '       End If

    '       Return urlRedirect
    '   End Function

    Public Function CalcolaAnnoAccademicoTesi() As Integer
        Dim Anno, Mese, AnnoAccademico As Integer
        Dim oData As DateTime
        oData = Now
        Mese = oData.Month
        Anno = oData.Year

        If Mese > 1 And Mese <= 3 Then
            AnnoAccademico = Anno - 2
        Else
            AnnoAccademico = Anno - 1
        End If
        Return AnnoAccademico

    End Function

	Public Function GetResourceConfig(ByVal LinguaCode As String) As ResourceManager
		Dim oResourceConfig = New ResourceManager

		If LinguaCode = "" Then
			LinguaCode = "it-IT"
		End If
		oResourceConfig.UserLanguages = LinguaCode
		oResourceConfig.ResourcesName = System.Configuration.ConfigurationSettings.AppSettings("configFile")
		oResourceConfig.Folder_Level1 = "Root"

		oResourceConfig.setCulture()
		Return oResourceConfig
	End Function

    Public Function GetPercorsoApplicazione(ByVal oRequest As HttpRequest) As String
        If oRequest.ApplicationPath = "/" Then
            Return ""
        Else
            Return oRequest.ApplicationPath
        End If
        Return ""
    End Function
End Module

Public Module ModuloEnum
    Public Enum WizardServizio_Message
        ErroreGenerico = 0
        Creato = 1
        Modificato = 2
        PermessiAssociati = 3
        TipiComunitaAssociati = 4
        PermessiRuoliAssociati = 5
        PermessiRuoliAssociatiParziali = 7
        OperazioneConclusa = 6
        NomeMancante = -1
        NONModificato = -2
        NONinserito = -3
        SelezionaPermesso = -4
        SelezionaTipoComunita = -5
        NessunPermesso = -6
        NessunTipoComunita = -7
        PermessiAssociatiParziali = -8
        ServizioNonTrovato = -9
        ErroreAssociazioneLingue = -10
        ErroreTipiComunita = -11
        ErroreAssociazionePermessi = -12
        DefinireLinguaDefault = -13
    End Enum
    Public Enum WizardProfilo_Message
        ErroreGenerico = 0
        Creato = 1
        Modificato = 2
        RuoliAssociati = 3
        ServiziAssociati = 4
        PermessiDefiniti = 5
        OperazioneConclusa = 6
        NomeMancante = -1
        NONModificato = -2
        NONinserito = -3
        SelezionaRuolo = -4
        SelezionaServizio = -5
        NessunPermesso = -6
        NessunServizio = -7
        PermessiAssociatiParziali = -8
        ProfiloNonTrovato = -9
        TipoComunitanonTrovato = -10
        TipoRuoloNonTrovato = -11
        ErroreAssociazionePermessi = -12
    End Enum
    Public Enum WizardComunita_Message
        NessunPadre = -10
        ServizioDefaultNonDefinito = -9
        PadriNonAssociati = -8
        ErroreServizi = -7
        ResponsabileNonAssociato = -6
        NessunaComunita = -5
        NessunServizio = -4
        NessunAltroPadre = -3
        NesunaOperazione = -2
        DatiMancanti = -1
        ComunitaNonCreata = 0
        ComunitaCreata = 1
        ComunitaAppenaCreata = 2
        ResponsabileAssociato = 3
        PadriAssociati = 4
        ServiziDefault = 5
        ServiziAttivati = 6
        ComunitaModificata = 7
    End Enum
End Module