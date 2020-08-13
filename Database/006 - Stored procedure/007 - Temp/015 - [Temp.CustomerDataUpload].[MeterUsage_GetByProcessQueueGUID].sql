USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Temp.CustomerDataUpload].[MeterUsage_GetByProcessQueueGUID]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Temp.CustomerDataUpload].[MeterUsage_GetByProcessQueueGUID] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-21
-- Description:	Get MeterUsage from [Temp.CustomerDataUpload].[MeterUsage] table by Process Queue GUID
-- =============================================

ALTER PROCEDURE [Temp.CustomerDataUpload].[MeterUsage_GetByProcessQueueGUID]
    @ProcessQueueGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-21 -> Andrew Sampson -> Initial development of script
    -- 2020-08-13 -> Andrew Sampson -> Add CanCommit column
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
        ProcessQueueGUID,
        RowId,
        MPXN,
        Date,
        TimePeriod,
        Value,
        CanCommit
    FROM
        [Temp.CustomerDataUpload].[MeterUsage]
    WHERE
        ProcessQueueGUID = @ProcessQueueGUID
END
GO
