USE [EMaas]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM System.APIAttribute WHERE APIAttributeDescription = 'APIName')
    BEGIN
        INSERT INTO [System].APIAttribute
        (
            APIAttributeDescription,
            CreatedByUserId
        )
        VALUES
        ('APIName', 1)
    END

IF NOT EXISTS(SELECT TOP 1 1 FROM System.APIAttribute WHERE APIAttributeDescription = 'ApplicationURL')
    BEGIN
        INSERT INTO [System].APIAttribute
        (
            APIAttributeDescription,
            CreatedByUserId
        )
        VALUES
        ('ApplicationURL', 1)
    END