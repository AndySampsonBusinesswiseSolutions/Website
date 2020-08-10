USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[DataUploadValidationErrorMessage_GetByDataUploadValidationErrorEntityId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[DataUploadValidationErrorMessage_GetByDataUploadValidationErrorEntityId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-10
-- Description:	Get DataUploadValidationErrorMessage info from [Customer].[DataUploadValidationErrorMessage] table by DataUploadValidationErrorEntityId
-- =============================================

ALTER PROCEDURE [Customer].[DataUploadValidationErrorMessage_GetByDataUploadValidationErrorEntityId]
    @DataUploadValidationErrorEntityId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-10 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        DataUploadValidationErrorMessageId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        DataUploadValidationErrorEntityId,
        DataUploadValidationErrorMessageDescription
    FROM 
        [Customer].[DataUploadValidationErrorMessage] 
    WHERE 
        DataUploadValidationErrorEntityId = @DataUploadValidationErrorEntityId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
