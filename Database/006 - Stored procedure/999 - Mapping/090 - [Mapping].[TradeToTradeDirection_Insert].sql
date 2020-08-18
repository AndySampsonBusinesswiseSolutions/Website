USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[TradeToTradeDirection_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[TradeToTradeDirection_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-18
-- Description:	Insert new mapping of a Trade to a TradeDirection into [Mapping].[TradeToTradeDirection] table
-- =============================================

ALTER PROCEDURE [Mapping].[TradeToTradeDirection_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @TradeId BIGINT,
    @TradeDirectionId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-18 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].TradeToTradeDirection
    (
        CreatedByUserId,
        SourceId,
        TradeId,
        TradeDirectionId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @TradeId,
        @TradeDirectionId
    )
END
GO
