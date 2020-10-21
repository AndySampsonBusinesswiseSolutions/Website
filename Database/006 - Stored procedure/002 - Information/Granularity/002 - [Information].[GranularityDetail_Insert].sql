USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[GranularityDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[GranularityDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-17
-- Description:	Insert new granularity detail into [Information].[GranularityDetail] table
-- =============================================

ALTER PROCEDURE [Information].[GranularityDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @GranularityId BIGINT,
    @GranularityAttributeId BIGINT,
    @GranularityDetailDescription VARCHAR(MAX)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-17 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[GranularityDetail] WHERE GranularityId = @GranularityId 
        AND GranularityAttributeId = @GranularityAttributeId 
        AND GranularityDetailDescription = @GranularityDetailDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[GranularityDetail]
            (
                CreatedByUserId,
                SourceId,
                GranularityId,
                GranularityAttributeId,
                GranularityDetailDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @GranularityId,
                @GranularityAttributeId,
                @GranularityDetailDescription
            )
        END
END
GO
