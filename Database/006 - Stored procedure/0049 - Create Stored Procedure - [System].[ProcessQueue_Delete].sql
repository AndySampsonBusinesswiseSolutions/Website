USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ProcessQueue_Delete]'))
    BEGIN
        exec('CREATE PROCEDURE [System].[ProcessQueue_Delete] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Get EffectiveToDate info from [System].[ProcessQueue] table by GUID
-- =============================================

ALTER PROCEDURE [System].[ProcessQueue_Delete]
    @ProcessQueueGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-18 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DELETE
    FROM 
        [System].[ProcessQueue] 
    WHERE 
        ProcessQueueGUID = @ProcessQueueGUID
END
GO
