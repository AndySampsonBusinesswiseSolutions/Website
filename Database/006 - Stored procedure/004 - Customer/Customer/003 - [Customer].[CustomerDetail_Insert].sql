USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[CustomerDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[CustomerDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-06
-- Description:	Insert new customer detail into [Customer].[CustomerDetail] table
-- =============================================

ALTER PROCEDURE [Customer].[CustomerDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @CustomerId BIGINT,
    @CustomerAttributeId BIGINT,
    @CustomerDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-06 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Customer].[CustomerDetail] WHERE CustomerId = @CustomerId 
        AND CustomerAttributeId = @CustomerAttributeId 
        AND CustomerDetailDescription = @CustomerDetailDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Customer].[CustomerDetail]
            (
                CreatedByUserId,
                SourceId,
                CustomerId,
                CustomerAttributeId,
                CustomerDetailDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @CustomerId,
                @CustomerAttributeId,
                @CustomerDetailDescription
            )
        END
END
GO
