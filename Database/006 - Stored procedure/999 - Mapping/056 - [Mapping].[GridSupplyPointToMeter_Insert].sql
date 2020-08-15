USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[GridSupplyPointToMeter_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[GridSupplyPointToMeter_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-15
-- Description:	Insert new mapping of a GridSupplyPoint to a Meter into [Mapping].[GridSupplyPointToMeter] table
-- =============================================

ALTER PROCEDURE [Mapping].[GridSupplyPointToMeter_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @GridSupplyPointId BIGINT,
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

    INSERT INTO [Mapping].GridSupplyPointToMeter
    (
        CreatedByUserId,
        SourceId,
        GridSupplyPointId,
        MeterId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @GridSupplyPointId,
        @MeterId
    )
END
GO
