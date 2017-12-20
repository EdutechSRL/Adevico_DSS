Imports COL_BusinessLogic_v2.UCServices

Public Interface IviewQuestionario
    Inherits IviewQuestionarioBase

    Sub ImpostazioneDatiBase()
	Sub ImpostazioneDatiComunitaByQuestionario()
	Sub CaricaPersona(ByVal PersonaID As Integer, ByVal ComunitaID As Integer)
	Sub CaricaAnonimo(ByVal PersonaID As Integer)
	Sub RegistraAccessoPagina()
	'Sub CambiaImpostazioniLingua(ByVal LinguaID As Integer, ByVal LinguaCode As String)
    Function HasPermessi() As Boolean
    Sub SetControlliByPermessi()

	Sub BindNoPermessi()


    Property isAnonymousCompiler() As Boolean
    Property LinguaQuestionario() As Integer
    Property LinguaDefaultQuestionario() As Integer
    Sub LoadQuestionnaireByUrl()
End Interface