USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[Granularity_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[Granularity_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-29
-- Description:	Insert new Granularity into [Information].[Granularity] table
-- =============================================

ALTER PROCEDURE [Information].[Granularity_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @GranularityDescription VARCHAR(255),
    @GranularityDisplayDescription VARCHAR(255),
    @IsTimePeriod BIT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-29 -> Andrew Sampson -> Initial development of script
    -- 2020-07-30 -> Andrew Sampson -> Added IsTimePeriod
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[Granularity] WHERE GranularityDescription = @GranularityDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[Granularity]
            (
                CreatedByUserId,
                SourceId,
                GranularityDescription,
                GranularityDisplayDescription,
                IsTimePeriod
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @GranularityDescription,
                @GranularityDisplayDescription,
                @IsTimePeriod
            )
        END
END
GO
