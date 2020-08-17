USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Temp.CustomerDataUpload].[Meter_GetByProcessQueueGUID]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Temp.CustomerDataUpload].[Meter_GetByProcessQueueGUID] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-21
-- Description:	Get Meter from [Temp.CustomerDataUpload].[Meter] table by Process Queue GUID
-- =============================================

ALTER PROCEDURE [Temp.CustomerDataUpload].[Meter_GetByProcessQueueGUID]
    @ProcessQueueGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-21 -> Andrew Sampson -> Initial development of script
    -- 2020-08-13 -> Andrew Sampson -> Add CanCommit column
    -- 2020-08-17 -> Andrew Sampson -> Added SitePostCode column
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
        ProcessQueueGUID,
        RowId,
        SiteName,
        SitePostCode,
        MPXN,
        GridSupplyPoint,
        ProfileClass,
        MeterTimeswitchCode,
        LineLossFactorClass,
        Capacity,
        LocalDistributionZone,
        StandardOfftakeQuantity,
        AnnualUsage,
        MeterSerialNumber,
        Area,
        ImportExport,
        CanCommit
    FROM
        [Temp.CustomerDataUpload].[Meter]
    WHERE
        ProcessQueueGUID = @ProcessQueueGUID
END
GO
