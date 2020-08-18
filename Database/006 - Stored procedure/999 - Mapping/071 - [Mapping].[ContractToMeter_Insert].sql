USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ContractToMeter_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ContractToMeter_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-18
-- Description:	Insert new mapping of a Contract to a Meter into [Mapping].[ContractToMeter] table
-- =============================================

ALTER PROCEDURE [Mapping].[ContractToMeter_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ContractId BIGINT,
    @MeterId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-18 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].ContractToMeter
    (
        CreatedByUserId,
        SourceId,
        ContractId,
        MeterId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ContractId,
        @MeterId
    )
END
GO
