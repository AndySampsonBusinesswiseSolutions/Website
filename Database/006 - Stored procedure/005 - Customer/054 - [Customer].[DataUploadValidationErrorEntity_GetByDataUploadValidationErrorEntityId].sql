USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[DataUploadValidationErrorEntity_GetByDataUploadValidationErrorEntityId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[DataUploadValidationErrorEntity_GetByDataUploadValidationErrorEntityId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-10
-- Description:	Get DataUploadValidationErrorEntity info from [Customer].[DataUploadValidationErrorEntity] table by DataUploadValidationErrorEntityId
-- =============================================

ALTER PROCEDURE [Customer].[DataUploadValidationErrorEntity_GetByDataUploadValidationErrorEntityId]
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
        DataUploadValidationErrorEntityId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        DataUploadValidationErrorRowId,
        DataUploadValidationErrorEntityAttributeId
    FROM 
        [Customer].[DataUploadValidationErrorEntity] 
    WHERE 
        DataUploadValidationErrorEntityId = @DataUploadValidationErrorEntityId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
