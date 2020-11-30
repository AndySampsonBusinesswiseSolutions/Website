USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[TradeAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[TradeAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-28
-- Description:	Insert new trade attribute into [Customer].[TradeAttribute] table
-- =============================================

ALTER PROCEDURE [Customer].[TradeAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @TradeAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-28 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Customer].[TradeAttribute]
    (
        CreatedByUserId,
        SourceId,
        TradeAttributeDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @TradeAttributeDescription
    )
END
GO