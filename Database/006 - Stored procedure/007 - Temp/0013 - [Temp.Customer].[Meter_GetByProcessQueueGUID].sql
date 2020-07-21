USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Temp.Customer].[Meter_GetByProcessQueueGUID]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Temp.Customer].[Meter_GetByProcessQueueGUID] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-21
-- Description:	Get Meter from [Temp.Customer].[Meter] table by Process Queue GUID
-- =============================================

ALTER PROCEDURE [Temp.Customer].[Meter_GetByProcessQueueGUID]
    @ProcessQueueGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-21 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
        ProcessQueueGUID,
        RowId,
        SiteName,
        MPXN,
        GridSupplyPoint,
        ProfileClass,
        MeterTimeswitchClass,
        LineLossFactorClass,
        Capacity,
        LocalDistributionZone,
        StandardOfftakeQuantity,
        AnnualUsage,
        MeterSerialNumber,
        Area,
        ImportExport
    FROM
        [Temp.Customer].[Meter]
    WHERE
        ProcessQueueGUID = @ProcessQueueGUID
END
GO
