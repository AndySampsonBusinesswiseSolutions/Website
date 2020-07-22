USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

EXEC [Information].[GridSupplyPoint_Insert] @CreatedByUserId, @SourceId, '51D8A858-5ACB-4419-A9A1-2E755711BA8D' --_A
EXEC [Information].[GridSupplyPoint_Insert] @CreatedByUserId, @SourceId, '8B8535E7-9DFF-4BFE-830D-63EE49595D3E' --_B
EXEC [Information].[GridSupplyPoint_Insert] @CreatedByUserId, @SourceId, 'C9ABBC17-3E25-4C7D-95B9-3D9E845F3586' --_C
EXEC [Information].[GridSupplyPoint_Insert] @CreatedByUserId, @SourceId, '11DD26E5-8DBA-4BB2-9BD4-F62F58C544EC' --_D
EXEC [Information].[GridSupplyPoint_Insert] @CreatedByUserId, @SourceId, '9F28DEA0-F27F-43FF-82B0-D1FEA9CEEDDF' --_E
EXEC [Information].[GridSupplyPoint_Insert] @CreatedByUserId, @SourceId, 'FDB31F38-813F-4B86-A19B-19F44D32B049' --_F
EXEC [Information].[GridSupplyPoint_Insert] @CreatedByUserId, @SourceId, '2EDC3F49-CE4F-442D-8CE8-11C132D41F74' --_G
EXEC [Information].[GridSupplyPoint_Insert] @CreatedByUserId, @SourceId, '0635BC50-E623-4F6B-9596-D710FCCA84E9' --_P
EXEC [Information].[GridSupplyPoint_Insert] @CreatedByUserId, @SourceId, '94A7A2FA-8F07-4FA8-82FF-1587F21BF7E3' --_N
EXEC [Information].[GridSupplyPoint_Insert] @CreatedByUserId, @SourceId, '77EE3FAD-6CD6-4554-AD06-DE8EC3D203CE' --_J
EXEC [Information].[GridSupplyPoint_Insert] @CreatedByUserId, @SourceId, '186AEDE8-1669-451F-A390-EE8AF65E0D71' --_H
EXEC [Information].[GridSupplyPoint_Insert] @CreatedByUserId, @SourceId, '26ACBE07-7FAF-4FA6-8D5C-A3EAC80982A3' --_K
EXEC [Information].[GridSupplyPoint_Insert] @CreatedByUserId, @SourceId, 'BFDC9C47-EDB8-4930-B847-D644B0E98534' --_L
EXEC [Information].[GridSupplyPoint_Insert] @CreatedByUserId, @SourceId, '79910459-AAFD-4636-BA70-FF2C4EF1AE61' --_M