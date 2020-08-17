USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[MeterToSubMeter_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[MeterToSubMeter_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-17
-- Description:	Insert new mapping of a Meter to a SubMeter into [Mapping].[MeterToSubMeter] table
-- =============================================

ALTER PROCEDURE [Mapping].[MeterToSubMeter_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @MeterId BIGINT,
    @SubMeterId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-17 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Mapping].[MeterToSubMeter] WHERE MeterId = @MeterId
        AND SubMeterId = @SubMeterId 
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Mapping].MeterToSubMeter
            (
                CreatedByUserId,
                SourceId,
                MeterId,
                SubMeterId
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @MeterId,
                @SubMeterId                
            )
        END
END
GO
