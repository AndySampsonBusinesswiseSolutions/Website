USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Temp.Customer].[Customer_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Temp.Customer].[Customer_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-20
-- Description:	Insert new Customer into [Temp.Customer].[Customer] table
-- =============================================

ALTER PROCEDURE [Temp.Customer].[Customer_Insert]
    @ProcessQueueGUID UNIQUEIDENTIFIER,
    @CustomerName VARCHAR(255),
    @ContactName VARCHAR(255),
    @ContactTelephoneNumber VARCHAR(255),
    @ContactEmailAddress VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-20 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Temp.Customer].[Customer]
    (
        ProcessQueueGUID,
        CustomerName,
        ContactName,
        ContactTelephoneNumber,
        ContactEmailAddress
    )
    VALUES
    (
        @ProcessQueueGUID,
        @CustomerName,
        @ContactName,
        @ContactTelephoneNumber,
        @ContactEmailAddress
    )
END
GO
