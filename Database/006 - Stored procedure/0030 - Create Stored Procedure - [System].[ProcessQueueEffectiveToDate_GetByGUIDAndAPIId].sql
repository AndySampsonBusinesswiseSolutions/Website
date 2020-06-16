USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ProcessQueue_GetByGUIDAndAPIId]'))
    BEGIN
        exec('CREATE PROCEDURE [System].[ProcessQueue_GetByGUIDAndAPIId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Get EffectiveToDate info from [System].[ProcessQueue] table by GUID
-- =============================================

ALTER PROCEDURE [System].[ProcessQueue_GetByGUIDAndAPIId]
    @ProcessQueueGUID UNIQUEIDENTIFIER,
    @APIId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-02 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
        GUID,
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
        GUID = @ProcessQueueGUID
        AND APIId = @APIId
END
GO
