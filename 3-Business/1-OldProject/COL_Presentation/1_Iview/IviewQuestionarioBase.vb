Imports COL_BusinessLogic_v2.UCServices

Public Interface IviewQuestionarioBase
	Inherits IViewCommon


	'Questionario
	ReadOnly Property Invito() As COL_Questionario.UtenteInvitato
	Property GruppoQuestionariID() As Integer
	Property GruppoDefaultID() As Integer
	Property GruppoCorrente() As COL_Questionario.QuestionarioGruppo
	Property QuestionarioCorrente() As COL_Questionario.Questionario
	Property DomandaCorrente() As COL_Questionario.Domanda
	Property PaginaCorrenteID() As Integer
	Property LibreriaCorrente() As COL_Questionario.Questionario
	ReadOnly Property Servizio() As Services_Questionario

End Interface
