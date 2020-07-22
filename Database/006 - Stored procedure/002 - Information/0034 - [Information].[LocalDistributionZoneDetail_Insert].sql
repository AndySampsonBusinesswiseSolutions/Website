USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[LocalDistributionZoneDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[LocalDistributionZoneDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-22
-- Description:	Insert new Local Distribution Zone detail into [Information].[LocalDistributionZoneDetail] table
-- =============================================

ALTER PROCEDURE [Information].[LocalDistributionZoneDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @LocalDistributionZoneId BIGINT,
    @LocalDistributionZoneAttributeId BIGINT,
    @LocalDistributionZoneDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-22 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[LocalDistributionZoneDetail] WHERE LocalDistributionZoneId = @LocalDistributionZoneId 
        AND LocalDistributionZoneAttributeId = @LocalDistributionZoneAttributeId 
        AND LocalDistributionZoneDetailDescription = @LocalDistributionZoneDetailDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[LocalDistributionZoneDetail]
            (
                CreatedByUserId,
                SourceId,
                LocalDistributionZoneId,
                LocalDistributionZoneAttributeId,
                LocalDistributionZoneDetailDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @LocalDistributionZoneId,
                @LocalDistributionZoneAttributeId,
                @LocalDistributionZoneDetailDescription
            )
        END
END
GO
