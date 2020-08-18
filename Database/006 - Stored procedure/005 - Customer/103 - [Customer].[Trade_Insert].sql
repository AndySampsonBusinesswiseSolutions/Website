USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[Trade_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[Trade_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-18
-- Description:	Insert new Trade into [Customer].[Trade] table
-- =============================================

ALTER PROCEDURE [Customer].[Trade_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @TradeGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-18 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Customer].[Trade] WHERE TradeGUID = @TradeGUID
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Customer].[Trade]
            (
                CreatedByUserId,
                SourceId,
                TradeGUID
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @TradeGUID
            )
        END
END
GO
