USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[Asset_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[Asset_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-15
-- Description:	Insert new Asset into [Customer].[Asset] table
-- =============================================

ALTER PROCEDURE [Customer].[Asset_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @AssetGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-15 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Customer].[Asset] WHERE AssetGUID = @AssetGUID
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Customer].[Asset]
            (
                CreatedByUserId,
                SourceId,
                AssetGUID
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @AssetGUID
            )
        END
END
GO
