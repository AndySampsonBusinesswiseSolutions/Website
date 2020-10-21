USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[DataUploadValidationError_GetByDataUploadValidationErrorGUID]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[DataUploadValidationError_GetByDataUploadValidationErrorGUID] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-03
-- Description:	Get DataUploadValidationError info from [Customer].[DataUploadValidationError] table by GUID
-- =============================================

ALTER PROCEDURE [Customer].[DataUploadValidationError_GetByDataUploadValidationErrorGUID]
    @DataUploadValidationErrorGUID UNIQUEIDENTIFIER,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-03 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        DataUploadValidationErrorId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        DataUploadValidationErrorGUID
    FROM 
        [Customer].[DataUploadValidationError] 
    WHERE 
        DataUploadValidationErrorGUID = @DataUploadValidationErrorGUID
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
