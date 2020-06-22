USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ProcessQueue_GetHasErrorByProcessQueueGUID]'))
    BEGIN
        exec('CREATE PROCEDURE [System].[ProcessQueue_GetHasErrorByProcessQueueGUID] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-22
-- Description:	Get whether Process has errored from [System].[ProcessQueue] table by ProcessGUID
-- =============================================

ALTER PROCEDURE [System].[ProcessQueue_GetHasErrorByProcessQueueGUID]
    @ProcessQueueGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-22 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT TOP 1
        HasError
    FROM 
        [System].[ProcessQueue]
    WHERE 
        ProcessQueueGUID = @ProcessQueueGUID
        AND HasError = 1
END
GO
