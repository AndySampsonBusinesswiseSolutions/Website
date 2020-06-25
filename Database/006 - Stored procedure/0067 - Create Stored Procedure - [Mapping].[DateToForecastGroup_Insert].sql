USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[DateToForecastGroup_Insert]'))
    BEGIN
        exec('CREATE PROCEDURE [Mapping].[DateToForecastGroup_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-25
-- Description:	Insert new mapping of a Date to a ForecastGroup into [Mapping].[DateToForecastGroup] table
-- =============================================

ALTER PROCEDURE [Mapping].[DateToForecastGroup_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @DateId BIGINT,
    @ForecastGroupId BIGINT,
    @Priority INT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-25 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].DateToForecastGroup
    (
        CreatedByUserId,
        SourceId,
        DateId,
        ForecastGroupId,
        Priority
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @DateId,
        @ForecastGroupId,
        @Priority
    )
END
GO
