USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[Customer_Insert]'))
    BEGIN
        exec('CREATE PROCEDURE [Customer].[Customer_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-06
-- Description:	Insert new customer into [Customer].[Customer] table
-- =============================================

ALTER PROCEDURE [Customer].[Customer_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @CustomerGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-06 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Customer].[Customer] WHERE CustomerGUID = @CustomerGUID
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Customer].[Customer]
            (
                CreatedByUserId,
                SourceId,
                CustomerGUID
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @CustomerGUID
            )
        END
END
GO
