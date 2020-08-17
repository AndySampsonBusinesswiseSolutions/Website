USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[MeterToSite_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[MeterToSite_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-17
-- Description:	Insert new mapping of a Meter to a Site into [Mapping].[MeterToSite] table
-- =============================================

ALTER PROCEDURE [Mapping].[MeterToSite_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @MeterId BIGINT,
    @SiteId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-17 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Mapping].[MeterToSite] WHERE MeterId = @MeterId
        AND SiteId = @SiteId 
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Mapping].MeterToSite
            (
                CreatedByUserId,
                SourceId,
                MeterId,
                SiteId
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @MeterId,
                @SiteId                
            )
        END
END
GO
