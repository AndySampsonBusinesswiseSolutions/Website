USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [Information].[Granularity_Insert] @CreatedByUserId, @SourceId, '4D55BB09-9F8F-4AB6-917E-23B1D09E71AD' --Five Minute
EXEC [Information].[Granularity_Insert] @CreatedByUserId, @SourceId, 'CEA433FB-5327-4747-95CB-0FEFD1D2AD6B' --Half Hour
EXEC [Information].[Granularity_Insert] @CreatedByUserId, @SourceId, '71C54EC0-6415-42D4-9C8C-6D8B8513F2FE' --Date
EXEC [Information].[Granularity_Insert] @CreatedByUserId, @SourceId, '8FD4C63A-84D5-4A03-B488-1A99C2331726' --Week
EXEC [Information].[Granularity_Insert] @CreatedByUserId, @SourceId, '2AB4DC83-C0D0-4C5F-AC95-6A948802E430' --Month
EXEC [Information].[Granularity_Insert] @CreatedByUserId, @SourceId, '8029270C-1ECB-43B0-B313-9082890CDC8B' --Quarter
EXEC [Information].[Granularity_Insert] @CreatedByUserId, @SourceId, '3799717D-303B-458F-8A38-5DFA934ED431' --Year