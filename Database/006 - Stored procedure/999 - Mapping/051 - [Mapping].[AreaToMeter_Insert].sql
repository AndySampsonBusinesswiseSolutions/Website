USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[AreaToMeter_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[AreaToMeter_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-15
-- Description:	Insert new mapping of a Area to a Meter into [Mapping].[AreaToMeter] table
-- =============================================

ALTER PROCEDURE [Mapping].[AreaToMeter_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @AreaId BIGINT,
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

    INSERT INTO [Mapping].AreaToMeter
    (
        CreatedByUserId,
        SourceId,
        AreaId,
        MeterId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @AreaId,
        @MeterId
    )
END
GO
