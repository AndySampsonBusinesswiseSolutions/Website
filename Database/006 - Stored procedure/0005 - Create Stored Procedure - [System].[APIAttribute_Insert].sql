USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[APIAttribute_Insert]'))
    BEGIN
        exec('CREATE PROCEDURE [System].[APIAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Insert new API attribute into [System].[APIAttribute] table
-- =============================================

ALTER PROCEDURE [System].[APIAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @APIAttributeDescription VARCHAR(255),
    @AllowsMultipleActiveInstances BIT = 0
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-02 -> Andrew Sampson -> Initial development of script
    -- 2020-06-17 -> Andrew Sampson -> Updated as part of code refactor
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [System].[APIAttribute] WHERE APIAttributeDescription = @APIAttributeDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [System].[APIAttribute]
            (
                CreatedByUserId,
                SourceId,
                APIAttributeDescription,
                AllowsMultipleActiveInstances
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @APIAttributeDescription,
                @AllowsMultipleActiveInstances
            )
        END
END
GO
