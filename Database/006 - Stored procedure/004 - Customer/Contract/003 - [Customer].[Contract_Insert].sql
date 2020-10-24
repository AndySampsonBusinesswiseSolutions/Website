USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[Contract_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[Contract_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-16
-- Description:	Insert new Contract into [Customer].[Contract] table
-- =============================================

ALTER PROCEDURE [Customer].[Contract_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ContractGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-16 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Customer].[Contract]
    (
        CreatedByUserId,
        SourceId,
        ContractGUID
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ContractGUID
    )
END
GO