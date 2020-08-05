USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[FlexContract_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[FlexContract_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-03
-- Description:	Insert new FlexContract into [Customer].[FlexContract] table
-- =============================================

ALTER PROCEDURE [Customer].[FlexContract_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @FlexContractGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-03 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Customer].[FlexContract] WHERE FlexContractGUID = @FlexContractGUID
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Customer].[FlexContract]
            (
                CreatedByUserId,
                SourceId,
                FlexContractGUID
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @FlexContractGUID
            )
        END
END
GO
