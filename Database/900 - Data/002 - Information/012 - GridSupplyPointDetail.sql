USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @GridSupplyPointAttributeId BIGINT = (SELECT GridSupplyPointAttributeId FROM [Information].[GridSupplyPointAttribute] WHERE GridSupplyPointAttributeDescription = 'Grid Supply Point Group Id')

DECLARE @GridSupplyPointId BIGINT = (SELECT GridSupplyPointId FROM [Information].[GridSupplyPoint] WHERE GridSupplyPointGUID = '51D8A858-5ACB-4419-A9A1-2E755711BA8D')
EXEC [Information].[GridSupplyPointDetail_Insert] @CreatedByUserId, @SourceId, @GridSupplyPointId, @GridSupplyPointAttributeId, '_A'

SET @GridSupplyPointId = (SELECT GridSupplyPointId FROM [Information].[GridSupplyPoint] WHERE GridSupplyPointGUID = '8B8535E7-9DFF-4BFE-830D-63EE49595D3E')
EXEC [Information].[GridSupplyPointDetail_Insert] @CreatedByUserId, @SourceId, @GridSupplyPointId, @GridSupplyPointAttributeId, '_B'

SET @GridSupplyPointId = (SELECT GridSupplyPointId FROM [Information].[GridSupplyPoint] WHERE GridSupplyPointGUID = 'C9ABBC17-3E25-4C7D-95B9-3D9E845F3586')
EXEC [Information].[GridSupplyPointDetail_Insert] @CreatedByUserId, @SourceId, @GridSupplyPointId, @GridSupplyPointAttributeId, '_C'

SET @GridSupplyPointId = (SELECT GridSupplyPointId FROM [Information].[GridSupplyPoint] WHERE GridSupplyPointGUID = '11DD26E5-8DBA-4BB2-9BD4-F62F58C544EC')
EXEC [Information].[GridSupplyPointDetail_Insert] @CreatedByUserId, @SourceId, @GridSupplyPointId, @GridSupplyPointAttributeId, '_D'

SET @GridSupplyPointId = (SELECT GridSupplyPointId FROM [Information].[GridSupplyPoint] WHERE GridSupplyPointGUID = '9F28DEA0-F27F-43FF-82B0-D1FEA9CEEDDF')
EXEC [Information].[GridSupplyPointDetail_Insert] @CreatedByUserId, @SourceId, @GridSupplyPointId, @GridSupplyPointAttributeId, '_E'

SET @GridSupplyPointId = (SELECT GridSupplyPointId FROM [Information].[GridSupplyPoint] WHERE GridSupplyPointGUID = 'FDB31F38-813F-4B86-A19B-19F44D32B049')
EXEC [Information].[GridSupplyPointDetail_Insert] @CreatedByUserId, @SourceId, @GridSupplyPointId, @GridSupplyPointAttributeId, '_F'

SET @GridSupplyPointId = (SELECT GridSupplyPointId FROM [Information].[GridSupplyPoint] WHERE GridSupplyPointGUID = '2EDC3F49-CE4F-442D-8CE8-11C132D41F74')
EXEC [Information].[GridSupplyPointDetail_Insert] @CreatedByUserId, @SourceId, @GridSupplyPointId, @GridSupplyPointAttributeId, '_G'

SET @GridSupplyPointId = (SELECT GridSupplyPointId FROM [Information].[GridSupplyPoint] WHERE GridSupplyPointGUID = '0635BC50-E623-4F6B-9596-D710FCCA84E9')
EXEC [Information].[GridSupplyPointDetail_Insert] @CreatedByUserId, @SourceId, @GridSupplyPointId, @GridSupplyPointAttributeId, '_P'

SET @GridSupplyPointId = (SELECT GridSupplyPointId FROM [Information].[GridSupplyPoint] WHERE GridSupplyPointGUID = '94A7A2FA-8F07-4FA8-82FF-1587F21BF7E3')
EXEC [Information].[GridSupplyPointDetail_Insert] @CreatedByUserId, @SourceId, @GridSupplyPointId, @GridSupplyPointAttributeId, '_N'

SET @GridSupplyPointId = (SELECT GridSupplyPointId FROM [Information].[GridSupplyPoint] WHERE GridSupplyPointGUID = '77EE3FAD-6CD6-4554-AD06-DE8EC3D203CE')
EXEC [Information].[GridSupplyPointDetail_Insert] @CreatedByUserId, @SourceId, @GridSupplyPointId, @GridSupplyPointAttributeId, '_J'

SET @GridSupplyPointId = (SELECT GridSupplyPointId FROM [Information].[GridSupplyPoint] WHERE GridSupplyPointGUID = '186AEDE8-1669-451F-A390-EE8AF65E0D71')
EXEC [Information].[GridSupplyPointDetail_Insert] @CreatedByUserId, @SourceId, @GridSupplyPointId, @GridSupplyPointAttributeId, '_H'

SET @GridSupplyPointId = (SELECT GridSupplyPointId FROM [Information].[GridSupplyPoint] WHERE GridSupplyPointGUID = '26ACBE07-7FAF-4FA6-8D5C-A3EAC80982A3')
EXEC [Information].[GridSupplyPointDetail_Insert] @CreatedByUserId, @SourceId, @GridSupplyPointId, @GridSupplyPointAttributeId, '_K'

SET @GridSupplyPointId = (SELECT GridSupplyPointId FROM [Information].[GridSupplyPoint] WHERE GridSupplyPointGUID = 'BFDC9C47-EDB8-4930-B847-D644B0E98534')
EXEC [Information].[GridSupplyPointDetail_Insert] @CreatedByUserId, @SourceId, @GridSupplyPointId, @GridSupplyPointAttributeId, '_L'

SET @GridSupplyPointId = (SELECT GridSupplyPointId FROM [Information].[GridSupplyPoint] WHERE GridSupplyPointGUID = '79910459-AAFD-4636-BA70-FF2C4EF1AE61')
EXEC [Information].[GridSupplyPointDetail_Insert] @CreatedByUserId, @SourceId, @GridSupplyPointId, @GridSupplyPointAttributeId, '_M'
