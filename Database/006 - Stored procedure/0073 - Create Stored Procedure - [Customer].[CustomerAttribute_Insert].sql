USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[CustomerAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[CustomerAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-06
-- Description:	Insert new customer attribute into [Customer].[CustomerAttribute] table
-- =============================================

ALTER PROCEDURE [Customer].[CustomerAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @CustomerAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-06 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Customer].[CustomerAttribute] WHERE CustomerAttributeDescription = @CustomerAttributeDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Customer].[CustomerAttribute]
            (
                CreatedByUserId,
                SourceId,
                CustomerAttributeDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @CustomerAttributeDescription
            )
        END
END
GO
