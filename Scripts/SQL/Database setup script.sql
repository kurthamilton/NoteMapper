IF (OBJECT_ID('Users') IS NULL)
BEGIN
	CREATE TABLE dbo.Users
	(
		UserId UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
		CreatedUtc DATETIME NOT NULL,
		Email NVARCHAR(255) NOT NULL,
		CONSTRAINT PK_Users PRIMARY KEY (UserId),
		CONSTRAINT UQ_Users_Email UNIQUE (Email)
	)	
END

IF (OBJECT_ID('UserPasswords') IS NULL)
BEGIN
	CREATE TABLE dbo.UserPasswords
	(
		UserPasswordId UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
		UserId UNIQUEIDENTIFIER NOT NULL,
		Hash NVARCHAR(255) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
		Salt NVARCHAR(255) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
		ResetCode NVARCHAR(255) COLLATE SQL_Latin1_General_CP1_CS_AS NULL,
		ResetCodeExpiresUtc DATETIME,
		CONSTRAINT PK_UserPasswords PRIMARY KEY (UserPasswordId),
		CONSTRAINT FK_UserPasswords_Users_UserId FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
	)		

	CREATE UNIQUE INDEX UIX_UserPasswords_UserId ON UserPasswords(UserId)
END

IF (OBJECT_ID('UserPasswordResetCodes') IS NULL)
BEGIN
	CREATE TABLE dbo.UserPasswordResetCodes
	(
		UserPasswordResetCodeId UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
		UserId UNIQUEIDENTIFIER NOT NULL,
		CreatedUtc DATETIME NOT NULL,
		ExpiresUtc DATETIME NOT NULL,
		Code NVARCHAR(255) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
		CONSTRAINT PK_UserPasswordResetCodes PRIMARY KEY NONCLUSTERED (UserPasswordResetCodeId),
		CONSTRAINT FK_UserPasswordResetCodes_Users_UserId FOREIGN KEY (UserId) REFERENCES Users (UserId) ON DELETE CASCADE
	)

	CREATE CLUSTERED INDEX CIX_UserPasswordResetCodes_UserId ON UserPasswordResetCodes (UserId)
END

IF (OBJECT_ID('UserActivations') IS NULL)
BEGIN
	CREATE TABLE dbo.UserActivations
	(
		UserActivationId UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
		UserId UNIQUEIDENTIFIER NOT NULL,
		CreatedUtc DATETIME NOT NULL,
		ExpiresUtc DATETIME NOT NULL,
		Code NVARCHAR(255) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
		CONSTRAINT PK_UserActivations PRIMARY KEY (UserActivationId),
		CONSTRAINT FK_UserActivations_Users_UserId FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
	)

	CREATE INDEX IX_UserActivations_UserId ON UserActivations (UserId)
END

IF (OBJECT_ID('UserLoginTokens') IS NULL)
BEGIN
	CREATE TABLE dbo.UserLoginTokens
	(
		UserLoginTokenId UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
		UserId UNIQUEIDENTIFIER NOT NULL,
		CreatedUtc DATETIME NOT NULL,
		ExpiresUtc DATETIME NOT NULL,
		Token NVARCHAR(255) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
		CONSTRAINT PK_UserLoginTokens PRIMARY KEY (UserLoginTokenId),
		CONSTRAINT FK_UserLoginTokens_Users_UserId FOREIGN KEY (UserId) REFERENCES Users (UserId) ON DELETE CASCADE
	)
	
	CREATE INDEX IX_UserLoginTokens_UserId ON UserLoginTokens (UserId)
END

IF (OBJECT_ID('RegistrationCodes') IS NULL)
BEGIN
	CREATE TABLE dbo.RegistrationCodes
	(
		RegistrationCodeId UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
		CreatedUtc DATETIME NOT NULL,
		ExpiresUtc DATETIME NULL,
		Code NVARCHAR(255) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
		Note NVARCHAR(MAX)
		CONSTRAINT PK_RegistrationCodes PRIMARY KEY (RegistrationCodeId),
		CONSTRAINT UQ_RegistrationCodes_Code UNIQUE (Code)
	)

	INSERT INTO RegistrationCodes (CreatedUtc, Code) VALUES (GETUTCDATE(), 'NM-DEV')
END

IF (OBJECT_ID('UserRegistrationCodes') IS NULL)
BEGIN
	CREATE TABLE dbo.UserRegistrationCodes 
	(
		UserRegistrationCodeId UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
		CreatedUtc DATETIME NOT NULL,
		UserId UNIQUEIDENTIFIER NOT NULL,
		RegistrationCodeId UNIQUEIDENTIFIER NOT NULL,
		CONSTRAINT PK_UserRegistrationCodes PRIMARY KEY (UserRegistrationCodeId),
		CONSTRAINT FK_UserRegistrationCodes_Users_UserId FOREIGN KEY (UserId) REFERENCES Users (UserId) ON DELETE CASCADE,
		CONSTRAINT FK_UserRegistrationCodes_RegistrationCodes_RegistrationCodeId FOREIGN KEY (RegistrationCodeId) REFERENCES RegistrationCodes (RegistrationCodeId),
		CONSTRAINT UQ_UserRegistrationCodes_UserId UNIQUE (UserId)
	)
END