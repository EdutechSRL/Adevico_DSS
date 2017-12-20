USE [EDU_Ts_Dump]
GO

/****** Object:  UserDefinedFunction [dbo].[NEW_ufn_CP_GetValueForField]    Script Date: 18/10/2017 14:10:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[splitstring](@stringToSplit VARCHAR(MAX))
RETURNS
 @returnList TABLE ([Name] [nvarchar] (500))
AS
BEGIN

 DECLARE @name NVARCHAR(255)
 DECLARE @pos INT

 WHILE CHARINDEX('|', @stringToSplit) > 0
 BEGIN
  SELECT @pos  = CHARINDEX('|', @stringToSplit)  
  SELECT @name = SUBSTRING(@stringToSplit, 1, @pos-1)

  INSERT INTO @returnList 
  SELECT @name

  SELECT @stringToSplit = SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)
 END

 INSERT INTO @returnList
 SELECT @stringToSplit

 RETURN
END


GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[NEW_ufn_CP_GetValueForField]
(
	-- Add the parameters for the function here
	@FielType int,
	@FieldValue ntext
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @FieldValueOUT NVARCHAR(MAX)

	--11	RadioButtonList
	--12	DropDownList

	--13	CheckBoxList

	DECLARE @FieldValueIN NVARCHAR(MAX);
	set @FieldValueIN = convert(NVARCHAR(MAX), @FieldValue);

	if(isnull(@FieldValueIN,'') = '')
		SET @FieldValueOUT = '';

	else if (@FielType = 11 or @FielType = 12)
		begin
			if(ISNUMERIC(@FieldValueIN) > 0)
				begin
					DECLARE @FielvValueId bigint;

					SET @FielvValueId = CONVERT(bigint, @FieldValueIN);

					SELECT @FieldValueOUT = Name FROM dbo.CP_FieldOption WHERE (Id = @FielvValueId)
				end
			ELSE
				begin
					SET @FieldValueOUT = '';
				end
		end
	else if (@FielType = 13)
		begin
			SELECT @FieldValueOUT = COALESCE(@FieldValueOUT + '|', '') + Name 
			FROM dbo.CP_FieldOption	WHERE Id IN(SELECT * FROM dbo.splitstring(@FieldValueIN))
		end
	else
	begin 
		set @FieldValueOUT = @FieldValue;
	end


	return @FieldValueOUT;
END


GO


CREATE VIEW [dbo].[V_Bandi_Full]
AS
SELECT        TOP (100) PERCENT dbo.ORGANIZZAZIONE.ORGN_ragioneSociale AS OrganizzazioneNome, dbo.COMUNITA.CMNT_id AS ComunitàId, dbo.COMUNITA.CMNT_nome AS ComunitaNome, 
						 dbo.CP_CallForPaper.Id AS BandoId, dbo.CP_CallForPaper.Name AS BandoNome, dbo.CP_ENUM_CallType.Name AS BandoTipo, dbo.CP_CallForPaper.IsPublic AS BandoPubblico, 
						 dbo.CP_CallForPaper.IsPortal AS BandoDiPortale, dbo.CP_CallForPaper.AdvacedEvaluation AS BandoAvanzato, dbo.CP_CallForPaper._CreatedOn AS BandoCreatoIl, 
						 dbo.CP_CallForPaper.StartDate AS BandoInizioSottomissioni, dbo.CP_CallForPaper.EndDate AS BandoFineSottomissioni, dbo.CP_CallForPaper.SubmissionClosed AS BandoSottomissioniChiuse, 
						 dbo.CP_CallForPaper.Edition AS BandoEdizione, dbo.CP_ENUM_CallStatus.Name AS BandoStato, dbo.CP_UserSubmission.Id AS SottomissioneId, dbo.CP_ENUM_SubmissionStatus.Name AS SottomissioneStato,
						  dbo.CP_UserSubmission.isComplete AS SottomissioneCompletata, dbo.CP_UserSubmission.isAnonymous AS SottomissinoeAnonima, dbo.CP_UserSubmission._CreatedOn AS SottomissioneCreataIl, 
						 dbo.CP_UserSubmission.SubmittedOn AS SottomissioneSottomessaIl, dbo.CP_UserSubmission.IdPerson AS SottomittoreId, dbo.PERSONA.PRSN_nome AS SottomittoreNome, 
						 dbo.PERSONA.PRSN_cognome AS SottomittoreCognome, dbo.CP_FieldDefinition.Id AS CampoId, dbo.CP_FieldDefinition.Name AS CampoNome, dbo.CP_FieldDefinition.Tags AS CampoTag, 
						 dbo.CP_ENUM_FieldType.Name AS CampoTipo, dbo.NEW_ufn_CP_GetValueForField(dbo.CP_FieldDefinition.Type, dbo.CP_FieldValue.Value) AS CampoValore
FROM            dbo.CP_ENUM_FieldType RIGHT OUTER JOIN
						 dbo.CP_ENUM_CallStatus RIGHT OUTER JOIN
						 dbo.COMUNITA LEFT OUTER JOIN
						 dbo.ORGANIZZAZIONE ON dbo.COMUNITA.CMNT_ORGN_id = dbo.ORGANIZZAZIONE.ORGN_id RIGHT OUTER JOIN
						 dbo.CP_CallForPaper LEFT OUTER JOIN
						 dbo.CP_ENUM_CallType ON dbo.CP_CallForPaper.Type = dbo.CP_ENUM_CallType.Value ON dbo.COMUNITA.CMNT_id = dbo.CP_CallForPaper.IdCommunity RIGHT OUTER JOIN
						 dbo.CP_FieldDefinition RIGHT OUTER JOIN
						 dbo.CP_FieldValue LEFT OUTER JOIN
						 dbo.CP_UserSubmission ON dbo.CP_FieldValue.IdUserSubmission = dbo.CP_UserSubmission.Id ON dbo.CP_FieldDefinition.Id = dbo.CP_FieldValue.IdFieldDefinition LEFT OUTER JOIN
						 dbo.PERSONA ON dbo.CP_UserSubmission.IdPerson = dbo.PERSONA.PRSN_id ON dbo.CP_CallForPaper.Id = dbo.CP_UserSubmission.IdCallForPaper ON 
						 dbo.CP_ENUM_CallStatus.Value = dbo.CP_CallForPaper.Status LEFT OUTER JOIN
						 dbo.CP_ENUM_SubmissionStatus ON dbo.CP_UserSubmission.Status = dbo.CP_ENUM_SubmissionStatus.Value ON dbo.CP_ENUM_FieldType.Value = dbo.CP_FieldDefinition.Type
WHERE        (dbo.PERSONA.PRSN_invisible = 0)

GO

USE [EDU_Ts_Dump]
GO

/****** Object:  View [dbo].[V_NumeroCampiCompilati]    Script Date: 18/10/2017 14:12:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[V_NumeroCampiCompilati]
AS
SELECT        COUNT(dbo.CP_FieldValue.Id) AS Expr1
FROM            dbo.CP_FieldValue LEFT OUTER JOIN
						 dbo.CP_UserSubmission ON dbo.CP_FieldValue.IdUserSubmission = dbo.CP_UserSubmission.Id LEFT OUTER JOIN
						 dbo.PERSONA ON dbo.CP_UserSubmission.IdPerson = dbo.PERSONA.PRSN_id
WHERE        (dbo.PERSONA.PRSN_invisible = 0)

GO