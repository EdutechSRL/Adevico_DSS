Imports System.Collections
Imports System.Collections.Generic
Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices

Public Class ServiziCorrenti
	'Inherits List(Of MyServices)
    Inherits Hashtable


	Public Sub Add(ByVal ServizioID As Integer, ByVal ServiceCode As String, ByVal Permission As String)
		Dim oServicesElement As New ServiceElement(ServizioID, ServiceCode)
		Select Case ServiceCode
			Case UCServices.Services_AmministraComunita.Codex
				oServicesElement.ServizioConcreto = New Services_AmministraComunita(Permission)
			Case UCServices.Services_AmministrazioneGlobale.Codex
				oServicesElement.ServizioConcreto = New Services_AmministrazioneGlobale(Permission)
            Case UCServices.Services_Bacheca.Codex
                oServicesElement.ServizioConcreto = New Services_Bacheca(Permission)
            Case UCServices.Services_CHAT.Codex
                oServicesElement.ServizioConcreto = New Services_CHAT(Permission)
            Case UCServices.Services_Cover.Codex
                oServicesElement.ServizioConcreto = New Services_Cover(Permission)
			Case UCServices.Services_DiarioLezioni.Codex
				oServicesElement.ServizioConcreto = New Services_DiarioLezioni(Permission)
			Case UCServices.Services_ElencaComunita.Codex
				oServicesElement.ServizioConcreto = New Services_ElencaComunita(Permission)
			Case UCServices.Services_Eventi.Codex
				oServicesElement.ServizioConcreto = New Services_Eventi(Permission)
			Case UCServices.Services_File.Codex
				oServicesElement.ServizioConcreto = New Services_File(Permission)
			Case UCServices.Services_Forum.Codex
				oServicesElement.ServizioConcreto = New Services_Forum(Permission)
			Case UCServices.Services_Gallery.Codex
				oServicesElement.ServizioConcreto = New Services_Gallery(Permission)
			Case UCServices.Services_GestioneIscritti.Codex
				oServicesElement.ServizioConcreto = New Services_GestioneIscritti(Permission)
			Case UCServices.Services_IscrizioneComunita.Codex
				oServicesElement.ServizioConcreto = New Services_IscrizioneComunita(Permission)
			Case UCServices.Services_Listaiscritti.Codex
				oServicesElement.ServizioConcreto = New Services_Listaiscritti(Permission)
			Case UCServices.Services_Mail.Codex
				oServicesElement.ServizioConcreto = New Services_Mail(Permission)
			Case UCServices.Services_ManagementSottoIscritti.Codex
				oServicesElement.ServizioConcreto = New Services_ManagementSottoIscritti(Permission)
			Case UCServices.Services_PostIt.Codex
				oServicesElement.ServizioConcreto = New Services_PostIt(Permission)
            Case UCServices.Services_RaccoltaLink.Codex
                oServicesElement.ServizioConcreto = New Services_RaccoltaLink(Permission)
            Case UCServices.Services_Statistiche.Codex
                oServicesElement.ServizioConcreto = New Services_Statistiche(Permission)
            Case UCServices.Services_Wiki.Codex
                oServicesElement.ServizioConcreto = New Services_Wiki(Permission)
            Case UCServices.Services_Questionario.Codex
				oServicesElement.ServizioConcreto = New Services_Questionario(Permission)
            Case UCServices.Services_WorkBook.Codex
                oServicesElement.ServizioConcreto = New Services_WorkBook(Permission)
            Case UCServices.Services_UserAccessReports.Codex
                oServicesElement.ServizioConcreto = New Services_UserAccessReports(Permission)
        End Select
        If Not MyBase.ContainsKey(ServiceCode) Then
            MyBase.Add(ServiceCode, oServicesElement)
        End If
    End Sub

	Public Overloads Function Find(ByVal ServiceCode As String) As MyServices
		Dim oServicesElement As ServiceElement

		Try
			oServicesElement = DirectCast(MyBase.Item(ServiceCode), ServiceElement)
			Return oServicesElement.ServizioConcreto
		Catch ex As Exception
			oServicesElement = Nothing
		End Try
		Return Nothing
	End Function

End Class