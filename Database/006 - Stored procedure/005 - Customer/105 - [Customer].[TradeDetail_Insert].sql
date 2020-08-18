USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[TradeDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[TradeDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-18
-- Description:	Insert new Trade detail into [Customer].[TradeDetail] table
-- =============================================

ALTER PROCEDURE [Customer].[TradeDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @TradeId BIGINT,
    @TradeAttributeId BIGINT,
    @TradeDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-18 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Customer].[TradeDetail] WHERE TradeId = @TradeId 
        AND TradeAttributeId = @TradeAttributeId 
        AND TradeDetailDescription = @TradeDetailDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Customer].[TradeDetail]
            (
                CreatedByUserId,
                SourceId,
                TradeId,
                TradeAttributeId,
                TradeDetailDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @TradeId,
                @TradeAttributeId,
                @TradeDetailDescription
            )
        END
END
GO
