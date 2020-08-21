USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[LocalDistributionZoneToMeter_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[LocalDistributionZoneToMeter_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-15
-- Description:	Insert new mapping of a LocalDistributionZone to a Meter into [Mapping].[LocalDistributionZoneToMeter] table
-- =============================================

ALTER PROCEDURE [Mapping].[LocalDistributionZoneToMeter_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @LocalDistributionZoneId BIGINT,
    @MeterId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-15 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].LocalDistributionZoneToMeter
    (
        CreatedByUserId,
        SourceId,
        LocalDistributionZoneId,
        MeterId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @LocalDistributionZoneId,
        @MeterId
    )
END
GO
