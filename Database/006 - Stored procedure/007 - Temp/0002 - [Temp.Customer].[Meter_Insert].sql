USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Temp.Customer].[Meter_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Temp.Customer].[Meter_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-15
-- Description:	Insert new Meter into [Temp.Customer].[Meter] table
-- =============================================

ALTER PROCEDURE [Temp.Customer].[Meter_Insert]
    @ProcessQueueGUID UNIQUEIDENTIFIER,
    @Site VARCHAR(255),
    @MPXN VARCHAR(255),
    @ProfileClass VARCHAR(255),
    @MeterTimeswitchClass VARCHAR(255),
    @LineLossFactorClass VARCHAR(255),
    @Capacity VARCHAR(255),
    @LocalDistributionZone VARCHAR(255),
    @StandardOfftakeQuantity VARCHAR(255),
    @AnnualUsage VARCHAR(255),
    @DayUsage VARCHAR(255),
    @NightUsage VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-15 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Temp.Customer].[Meter]
    (
        ProcessQueueGUID,
        Site,
        MPXN,
        ProfileClass,
        MeterTimeswitchClass,
        LineLossFactorClass,
        Capacity,
        LocalDistributionZone,
        StandardOfftakeQuantity,
        AnnualUsage,
        DayUsage,
        NightUsage
    )
    VALUES
    (
        @ProcessQueueGUID,
        @Site,
        @MPXN,
        @ProfileClass,
        @MeterTimeswitchClass,
        @LineLossFactorClass,
        @Capacity,
        @LocalDistributionZone,
        @StandardOfftakeQuantity,
        @AnnualUsage,
        @DayUsage,
        @NightUsage
    )
END
GO
