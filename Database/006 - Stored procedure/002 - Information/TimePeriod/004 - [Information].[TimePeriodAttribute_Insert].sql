USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[TimePeriodAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[TimePeriodAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-02
-- Description:	Insert new TimePeriod attribute into [Information].[TimePeriodAttribute] table
-- =============================================

ALTER PROCEDURE [Information].[TimePeriodAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @TimePeriodAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-02 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[TimePeriodAttribute] WHERE TimePeriodAttributeDescription = @TimePeriodAttributeDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[TimePeriodAttribute]
            (
                CreatedByUserId,
                SourceId,
                TimePeriodAttributeDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @TimePeriodAttributeDescription
            )
        END
END
GO
