USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[AssetDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[AssetDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-15
-- Description:	Insert new Asset detail into [Customer].[AssetDetail] table
-- =============================================

ALTER PROCEDURE [Customer].[AssetDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @AssetId BIGINT,
    @AssetAttributeId BIGINT,
    @AssetDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-15 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Customer].[AssetDetail] WHERE AssetId = @AssetId 
        AND AssetAttributeId = @AssetAttributeId 
        AND AssetDetailDescription = @AssetDetailDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Customer].[AssetDetail]
            (
                CreatedByUserId,
                SourceId,
                AssetId,
                AssetAttributeId,
                AssetDetailDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @AssetId,
                @AssetAttributeId,
                @AssetDetailDescription
            )
        END
END
GO
