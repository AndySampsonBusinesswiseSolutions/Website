USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ProcessQueue_GetByProcessQueueGUIDAndAPIId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[ProcessQueue_GetByProcessQueueGUIDAndAPIId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Get Process Queue info from [System].[ProcessQueue] table by ProcessQueueGUID and APIId
-- =============================================

ALTER PROCEDURE [System].[ProcessQueue_GetByProcessQueueGUIDAndAPIId]
    @ProcessQueueGUID UNIQUEIDENTIFIER,
    @APIId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-02 -> Andrew Sampson -> Initial development of script
    -- 2020-06-17 -> Andrew Sampson -> Adjusted stored procedure name to correctly identify which GUID is being used
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
        ProcessQueueGUID,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        APIID,
        HasError
    FROM 
        [System].[ProcessQueue] 
    WHERE 
        ProcessQueueGUID = @ProcessQueueGUID
        AND APIId = @APIId
END
GO
