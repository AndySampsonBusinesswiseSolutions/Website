USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ProcessQueue_GetByProcessQueueGUID]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[ProcessQueue_GetByProcessQueueGUID] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-23
-- Description:	Get Process Queue info from [System].[ProcessQueue] table by ProcessQueueGUID
-- =============================================

ALTER PROCEDURE [System].[ProcessQueue_GetByProcessQueueGUID]
    @ProcessQueueGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-23 -> Andrew Sampson -> Initial development of script
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
        HasError,
		ErrorMessage
    FROM 
        [System].[ProcessQueue] 
    WHERE 
        ProcessQueueGUID = @ProcessQueueGUID
END
GO
