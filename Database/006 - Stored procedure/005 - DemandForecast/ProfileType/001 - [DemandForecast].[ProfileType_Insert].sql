USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[DemandForecast].[ProfileType_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [DemandForecast].[ProfileType_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-09
-- Description:	Insert new Profile Type into [DemandForecast].[ProfileType] table
-- =============================================

ALTER PROCEDURE [DemandForecast].[ProfileType_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ProfileTypeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-09 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [DemandForecast].[ProfileType]
    (
        CreatedByUserId,
        SourceId,
        ProfileTypeDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ProfileTypeDescription
    )
END
GO