


''' <summary>
''' Helper per l'invio dei trap SNMP per il servizio Bandi
''' </summary>
Public Class CallTrapHelper
    ''' <summary>
    ''' Invio di un trap
    ''' </summary>
    ''' <param name="ActionCode">Codice azione. Univoco per ogni singola azione di ogni servizio</param>
    ''' <param name="ObjectId">Id oggetto</param>
    ''' <param name="ObjectType">Tipo oggetto</param>
    ''' <param name="SuccessInfo">Informazioni aggiuntive sull'operazione, soprattutto in caso di fallimento dell'operazione.</param>
    Public Shared Sub SendTrap(ByVal ActionCode As lm.Comol.Modules.CallForPapers.Trap.CallTrapId,
                        ByVal ObjectId As Int64,
                        ByVal ObjectType As lm.Comol.Modules.CallForPapers.Trap.CallObjectId,
                        ByVal SuccessInfo As String)

        Dim PageUtility As PresentationLayer.OLDpageUtility = New OLDpageUtility(HttpContext.Current)


        Dim ActionValue As New WsSnmtp.dtoActionValues

        ActionValue.Action = New WsSnmtp.dtoActionData()

        With ActionValue.Action
            .ModuleId = PresentationLayer.TrapModules.SRVCFP
            .ModuleCode = [Enum].GetName(GetType(PresentationLayer.TrapModules), PresentationLayer.TrapModules.SRVCFP)

            .InteractionType = lm.ActionDataContract.InteractionType.UserWithLearningObject

            .ActionCodeId = ActionCode
            .ActionTypeId = [Enum].GetName(GetType(lm.Comol.Modules.CallForPapers.Trap.CallTrapId), ActionCode)

            .SuccessInfo = SuccessInfo

            .ObjectId = ObjectId
            .ObjectType = [Enum].GetName(GetType(lm.Comol.Modules.CallForPapers.Trap.CallObjectId), ObjectType)


            .CommunityId = PageUtility.ComunitaCorrente.Id

            .CommunityIsFederated = False 'CommunityIsFederated

            '.ExtensionData = 
            .GenericInfo = ""
        End With

        ActionValue.User = New WsSnmtp.dtoUserValues

        With ActionValue.User
            .id = PageUtility.CurrentUser.ID
            .login = PageUtility.CurrentUser.Login
            .mail = PageUtility.CurrentUser.Mail
            .name = PageUtility.CurrentUser.Nome
            .surname = PageUtility.CurrentUser.Cognome
            .taxCode = PageUtility.CurrentUser.CodFiscale
            .Ip = PageUtility.ClientIPadress
            .ProxyIp = PageUtility.ProxyIPadress
        End With

        PageUtility.TrapSendGeneric(ActionCode, ActionValue)

    End Sub


End Class
