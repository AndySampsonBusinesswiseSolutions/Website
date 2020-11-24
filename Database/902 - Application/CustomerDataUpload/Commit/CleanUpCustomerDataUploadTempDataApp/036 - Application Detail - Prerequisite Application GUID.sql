USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @ApplicationId BIGINT = (SELECT ApplicationId FROM [System].[Application] WHERE ApplicationGUID = '51655869-D1B1-4C1E-9F21-6377EF4189D6')
DECLARE @ApplicationAttributeId BIGINT = (SELECT ApplicationAttributeId FROM [System].[ApplicationAttribute] WHERE ApplicationAttributeDescription = 'Prerequisite Application GUID')

EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, '8C14B5FD-90D4-409D-A69C-140FC103814A'
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, '994CB601-B7A4-4CC8-95F4-1467E03E0DAC'
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, '95DDBA83-F519-437D-A31D-21D8E0061131'
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, '6A49C79C-37F3-4D86-BCAE-27602E5DF416'
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, '41327B22-24FE-4EED-AED1-36B801D78F5E'
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, 'B0026B5C-CA13-42ED-9BA7-3CEBEC589FE9'
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, 'D69532B2-384D-44E0-B72E-43934276D0B6'
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, '664A01BE-A6E2-4557-80DC-4BF52E0533E4'
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, 'A397F2B9-4B0F-4432-8E0C-627A8B2753AB'
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, '26EBBD73-45DA-4514-B663-662D9127C65B'
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, 'CD2E7213-9A18-4BEA-9C4E-7193433192BE'
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, '2A904C29-A848-40BB-9993-916A2376B571'
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, '8CF42052-7798-40FD-A786-987CB53A159E'
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, '68E818C7-6F72-4586-8F75-A01E52680AE5'
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, 'ED61F240-9A67-412E-BA74-B80DA2A85443'
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, 'FF487D3F-89E2-4B9A-B6AF-C5133A19C609'
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, '8D6FE490-F451-4F2F-B429-CBBD3C217220'
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, 'BBA77F1B-F9A3-4CAA-B213-D11285994B0A'
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, '89E15527-A337-446F-9C67-E270D23D6CE2'
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, '0E525DC3-C108-4A00-8A5C-E69CF9DF8359'
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, '5BAA55DE-1F5B-40EF-988A-E8E85E2B594C'
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, 'EB061C46-A893-4C44-9EB7-E9D5CF6B0895'
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, '94475084-969D-431F-A8D8-EB591CCFCE26'
EXEC [System].[ApplicationDetail_Insert] @CreatedByUserId, @SourceId, @ApplicationId, @ApplicationAttributeId, 'A246C65A-5471-43F2-9836-F02306B8ECF5'

