USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[BasketAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[BasketAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-27
-- Description:	Insert new basket attribute into [Customer].[BasketAttribute] table
-- =============================================

ALTER PROCEDURE [Customer].[BasketAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @BasketAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-27 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Customer].[BasketAttribute] WHERE BasketAttributeDescription = @BasketAttributeDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Customer].[BasketAttribute]
            (
                CreatedByUserId,
                SourceId,
                BasketAttributeDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @BasketAttributeDescription
            )
        END
END
GO
