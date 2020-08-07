USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Temp.CustomerDataUpload].[Meter_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Temp.CustomerDataUpload].[Meter_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-15
-- Description:	Insert new Meter into [Temp.CustomerDataUpload].[Meter] table
-- =============================================

ALTER PROCEDURE [Temp.CustomerDataUpload].[Meter_Insert]
    @ProcessQueueGUID UNIQUEIDENTIFIER,
    @RowId INT,
    @SiteName VARCHAR(255),
    @MPXN VARCHAR(255),
    @GridSupplyPoint VARCHAR(255),
    @ProfileClass VARCHAR(255),
    @MeterTimeswitchCode VARCHAR(255),
    @LineLossFactorClass VARCHAR(255),
    @Capacity VARCHAR(255),
    @LocalDistributionZone VARCHAR(255),
    @StandardOfftakeQuantity VARCHAR(255),
    @AnnualUsage VARCHAR(255),
    @MeterSerialNumber VARCHAR(255),
    @Area VARCHAR(255),
    @ImportExport VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-15 -> Andrew Sampson -> Initial development of script
    -- 2020-07-20 -> Andrew Sampson -> Updates to handle new upload template
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Temp.CustomerDataUpload].[Meter]
    (
        ProcessQueueGUID,
        RowId,
        SiteName,
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
        ImportExport
    )
    VALUES
    (
        @ProcessQueueGUID,
        @RowId,
        @SiteName,
        @MPXN,
        @GridSupplyPoint,
        @ProfileClass,
        @MeterTimeswitchCode,
        @LineLossFactorClass,
        @Capacity,
        @LocalDistributionZone,
        @StandardOfftakeQuantity,
        @AnnualUsage,
        @MeterSerialNumber,
        @Area,
        @ImportExport
    )
END
GO
