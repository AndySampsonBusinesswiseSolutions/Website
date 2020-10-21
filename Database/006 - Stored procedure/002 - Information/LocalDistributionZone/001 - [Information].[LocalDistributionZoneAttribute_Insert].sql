USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[LocalDistributionZoneAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[LocalDistributionZoneAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-22
-- Description:	Insert new Local Distribution Zone attribute into [Information].[LocalDistributionZoneAttribute] table
-- =============================================

ALTER PROCEDURE [Information].[LocalDistributionZoneAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @LocalDistributionZoneAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-22 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[LocalDistributionZoneAttribute] WHERE LocalDistributionZoneAttributeDescription = @LocalDistributionZoneAttributeDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[LocalDistributionZoneAttribute]
            (
                CreatedByUserId,
                SourceId,
                LocalDistributionZoneAttributeDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @LocalDistributionZoneAttributeDescription
            )
        END
END
GO
