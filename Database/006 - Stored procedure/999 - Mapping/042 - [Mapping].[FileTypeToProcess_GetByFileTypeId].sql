USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[FileTypeToProcess_GetByFileTypeId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[FileTypeToProcess_GetByFileTypeId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-08
-- Description:	Get FileTypeToProcess info from [Mapping].[FileTypeToProcess] table by File Type Id
-- =============================================

ALTER PROCEDURE [Mapping].[FileTypeToProcess_GetByFileTypeId]
    @FileTypeId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-08 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        FileTypeToProcessId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        FileTypeId,
        ProcessId
    FROM 
        [Mapping].[FileTypeToProcess] 
    WHERE 
        FileTypeId = @FileTypeId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
