USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[ContractMeter_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[ContractMeter_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-16
-- Description:	Insert new ContractMeter into [Customer].[ContractMeter] table
-- =============================================

ALTER PROCEDURE [Customer].[ContractMeter_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ContractMeterGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-16 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Customer].[ContractMeter]
    (
        CreatedByUserId,
        SourceId,
        ContractMeterGUID
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ContractMeterGUID
    )
END
GO