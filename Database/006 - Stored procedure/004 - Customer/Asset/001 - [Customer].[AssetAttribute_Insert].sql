USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[AssetAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[AssetAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-15
-- Description:	Insert new Asset attribute into [Customer].[AssetAttribute] table
-- =============================================

ALTER PROCEDURE [Customer].[AssetAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @AssetAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-15 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Customer].[AssetAttribute]
    (
        CreatedByUserId,
        SourceId,
        AssetAttributeDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @AssetAttributeDescription
    )
END
GO