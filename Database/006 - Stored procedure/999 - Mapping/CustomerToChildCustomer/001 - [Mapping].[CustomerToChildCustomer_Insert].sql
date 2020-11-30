USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[CustomerToChildCustomer_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[CustomerToChildCustomer_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-06
-- Description:	Insert new mapping of a Customer to a ChildCustomer into [Mapping].[CustomerToChildCustomer] table
-- =============================================

ALTER PROCEDURE [Mapping].[CustomerToChildCustomer_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @CustomerId BIGINT,
    @ChildCustomerId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-06 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].CustomerToChildCustomer
    (
        CreatedByUserId,
        SourceId,
        CustomerId,
        ChildCustomerId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @CustomerId,
        @ChildCustomerId
    )
END
GO
