USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @UKOctoberClockChangeId BIGINT = (SELECT DateAttributeId FROM [Information].[DateAttribute] WHERE DateAttributeDescription = 'UK October Clock Change')

DECLARE @DateId BIGINT = (SELECT DateId FROM [Information].[Date] WHERE DateDescription = '2015-10-25')
EXEC [Information].[DateDetail_Insert] @CreatedByUserId, @SourceId, @DateId, @UKOctoberClockChangeId, 'True'

SET @DateId = (SELECT DateId FROM [Information].[Date] WHERE DateDescription = '2016-10-30')
EXEC [Information].[DateDetail_Insert] @CreatedByUserId, @SourceId, @DateId, @UKOctoberClockChangeId, 'True'

SET @DateId = (SELECT DateId FROM [Information].[Date] WHERE DateDescription = '2017-10-29')
EXEC [Information].[DateDetail_Insert] @CreatedByUserId, @SourceId, @DateId, @UKOctoberClockChangeId, 'True'

SET @DateId = (SELECT DateId FROM [Information].[Date] WHERE DateDescription = '2018-10-28')
EXEC [Information].[DateDetail_Insert] @CreatedByUserId, @SourceId, @DateId, @UKOctoberClockChangeId, 'True'

SET @DateId = (SELECT DateId FROM [Information].[Date] WHERE DateDescription = '2019-10-27')
EXEC [Information].[DateDetail_Insert] @CreatedByUserId, @SourceId, @DateId, @UKOctoberClockChangeId, 'True'

SET @DateId = (SELECT DateId FROM [Information].[Date] WHERE DateDescription = '2020-10-25')
EXEC [Information].[DateDetail_Insert] @CreatedByUserId, @SourceId, @DateId, @UKOctoberClockChangeId, 'True'

SET @DateId = (SELECT DateId FROM [Information].[Date] WHERE DateDescription = '2021-10-31')
EXEC [Information].[DateDetail_Insert] @CreatedByUserId, @SourceId, @DateId, @UKOctoberClockChangeId, 'True'

SET @DateId = (SELECT DateId FROM [Information].[Date] WHERE DateDescription = '2022-10-30')
EXEC [Information].[DateDetail_Insert] @CreatedByUserId, @SourceId, @DateId, @UKOctoberClockChangeId, 'True'

SET @DateId = (SELECT DateId FROM [Information].[Date] WHERE DateDescription = '2023-10-29')
EXEC [Information].[DateDetail_Insert] @CreatedByUserId, @SourceId, @DateId, @UKOctoberClockChangeId, 'True'

SET @DateId = (SELECT DateId FROM [Information].[Date] WHERE DateDescription = '2024-10-27')
EXEC [Information].[DateDetail_Insert] @CreatedByUserId, @SourceId, @DateId, @UKOctoberClockChangeId, 'True'

SET @DateId = (SELECT DateId FROM [Information].[Date] WHERE DateDescription = '2025-10-26')
EXEC [Information].[DateDetail_Insert] @CreatedByUserId, @SourceId, @DateId, @UKOctoberClockChangeId, 'True'

SET @DateId = (SELECT DateId FROM [Information].[Date] WHERE DateDescription = '2026-10-25')
EXEC [Information].[DateDetail_Insert] @CreatedByUserId, @SourceId, @DateId, @UKOctoberClockChangeId, 'True'

SET @DateId = (SELECT DateId FROM [Information].[Date] WHERE DateDescription = '2027-10-31')
EXEC [Information].[DateDetail_Insert] @CreatedByUserId, @SourceId, @DateId, @UKOctoberClockChangeId, 'True'

SET @DateId = (SELECT DateId FROM [Information].[Date] WHERE DateDescription = '2028-10-29')
EXEC [Information].[DateDetail_Insert] @CreatedByUserId, @SourceId, @DateId, @UKOctoberClockChangeId, 'True'

SET @DateId = (SELECT DateId FROM [Information].[Date] WHERE DateDescription = '2029-10-28')
EXEC [Information].[DateDetail_Insert] @CreatedByUserId, @SourceId, @DateId, @UKOctoberClockChangeId, 'True'

SET @DateId = (SELECT DateId FROM [Information].[Date] WHERE DateDescription = '2030-10-27')
EXEC [Information].[DateDetail_Insert] @CreatedByUserId, @SourceId, @DateId, @UKOctoberClockChangeId, 'True'