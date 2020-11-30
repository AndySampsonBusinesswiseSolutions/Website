USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[PageRequest_GetByProcessQueueGUIDAndUserId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[PageRequest_GetByProcessQueueGUIDAndUserId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-10-07
-- Description:	Get PageRequest info from [System].[PageRequest] table by Process Queue GUID and User Id
-- =============================================

ALTER PROCEDURE [System].[PageRequest_GetByProcessQueueGUIDAndUserId]
    @ProcessQueueGUID UNIQUEIDENTIFIER,
    @CreatedByUserId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-10-07 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        PageRequestId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        PageId,
        ProcessQueueGUID,
        PageRequestResult
    FROM 
        [System].[PageRequest] 
    WHERE 
        ProcessQueueGUID = @ProcessQueueGUID
        AND CreatedByUserId = @CreatedByUserId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
