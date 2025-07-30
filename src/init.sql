IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Accounts] (
    [Id] int NOT NULL,
    [AccountName] nvarchar(150) NOT NULL,
    [AccountNumber] nvarchar(50) NOT NULL,
    [TempId] uniqueidentifier NULL,
    [Name] nvarchar(100) NULL,
    [CreatedDate] datetime2 NOT NULL,
    [CreatedBy] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Accounts] PRIMARY KEY ([Id])
);

CREATE TABLE [AuditLogs] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(50) NOT NULL,
    [EntityName] nvarchar(50) NOT NULL,
    [Action] nvarchar(50) NOT NULL,
    [OldValue] nvarchar(max) NULL,
    [NewValue] nvarchar(max) NULL,
    [AffectedColumns] nvarchar(500) NULL,
    [PrimaryKey] nvarchar(100) NULL,
    [Timestamp] datetime2 NOT NULL,
    [TempId] uniqueidentifier NULL,
    [CreatedDate] datetime2 NOT NULL,
    [CreatedBy] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_AuditLogs] PRIMARY KEY ([Id])
);

CREATE TABLE [Collages] (
    [Id] int NOT NULL,
    [CollageName] nvarchar(100) NOT NULL,
    [TempId] uniqueidentifier NULL,
    [CreatedDate] datetime2 NOT NULL,
    [CreatedBy] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Collages] PRIMARY KEY ([Id])
);

CREATE TABLE [Dailies] (
    [Id] int NOT NULL,
    [DailyDate] date NOT NULL,
    [DailyType] nvarchar(50) NOT NULL,
    [AccountItem] nvarchar(20) NULL,
    [TempId] uniqueidentifier NULL,
    [Name] nvarchar(100) NULL,
    [CreatedDate] datetime2 NOT NULL,
    [CreatedBy] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Dailies] PRIMARY KEY ([Id])
);

CREATE TABLE [Funds] (
    [Id] int NOT NULL,
    [FundName] nvarchar(100) NOT NULL,
    [FundCode] nvarchar(max) NOT NULL,
    [CollageId] int NOT NULL,
    [TempId] uniqueidentifier NULL,
    [Name] nvarchar(100) NULL,
    [CreatedDate] datetime2 NOT NULL,
    [CreatedBy] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Funds] PRIMARY KEY ([Id])
);

CREATE TABLE [Forms] (
    [Id] int NOT NULL,
    [FormName] nvarchar(150) NOT NULL,
    [CollageId] int NULL,
    [FundId] int NULL,
    [Num224] nvarchar(150) NULL,
    [Num55] nvarchar(150) NULL,
    [TotalDebit] decimal(18,2) NULL,
    [TotalCredit] decimal(18,2) NULL,
    [DailyId] int NOT NULL,
    [AuditorName] nvarchar(150) NULL,
    [Details] nvarchar(500) NULL,
    [TempId] uniqueidentifier NULL,
    [CreatedDate] datetime2 NOT NULL,
    [CreatedBy] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_Forms] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Forms_Collages_CollageId] FOREIGN KEY ([CollageId]) REFERENCES [Collages] ([Id]),
    CONSTRAINT [FK_Forms_Dailies_DailyId] FOREIGN KEY ([DailyId]) REFERENCES [Dailies] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Forms_Funds_FundId] FOREIGN KEY ([FundId]) REFERENCES [Funds] ([Id])
);

CREATE TABLE [GeneralJournal] (
    [Id] int NOT NULL,
    [FormId] int NOT NULL,
    [AccountId] int NOT NULL,
    [Debit] decimal(18,2) NULL,
    [Credit] decimal(18,2) NULL,
    [AccountType] nvarchar(50) NOT NULL,
    [TempId] uniqueidentifier NULL,
    [CreatedDate] datetime2 NOT NULL,
    [CreatedBy] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_GeneralJournal] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_GeneralJournal_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_GeneralJournal_Forms_FormId] FOREIGN KEY ([FormId]) REFERENCES [Forms] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [SubsidiaryJournals] (
    [Id] int NOT NULL,
    [FormDetailsId] int NOT NULL,
    [AccountId] int NOT NULL,
    [Amount] decimal(18,2) NULL,
    [CollageId] int NULL,
    [FundId] int NULL,
    [TransactionSide] nvarchar(10) NOT NULL,
    [AccountType] nvarchar(max) NULL,
    [AccountItem] nvarchar(max) NULL,
    [TempId] uniqueidentifier NULL,
    [CreatedDate] datetime2 NOT NULL,
    [CreatedBy] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_SubsidiaryJournals] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SubsidiaryJournals_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_SubsidiaryJournals_Collages_CollageId] FOREIGN KEY ([CollageId]) REFERENCES [Collages] ([Id]),
    CONSTRAINT [FK_SubsidiaryJournals_Funds_FundId] FOREIGN KEY ([FundId]) REFERENCES [Funds] ([Id]),
    CONSTRAINT [FK_SubsidiaryJournals_GeneralJournal_FormDetailsId] FOREIGN KEY ([FormDetailsId]) REFERENCES [GeneralJournal] ([Id])
);

CREATE INDEX [IX_Forms_CollageId] ON [Forms] ([CollageId]);

CREATE INDEX [IX_Forms_DailyId] ON [Forms] ([DailyId]);

CREATE INDEX [IX_Forms_FundId] ON [Forms] ([FundId]);

CREATE INDEX [IX_GeneralJournal_AccountId] ON [GeneralJournal] ([AccountId]);

CREATE INDEX [IX_GeneralJournal_FormId] ON [GeneralJournal] ([FormId]);

CREATE INDEX [IX_SubsidiaryJournals_AccountId] ON [SubsidiaryJournals] ([AccountId]);

CREATE INDEX [IX_SubsidiaryJournals_CollageId] ON [SubsidiaryJournals] ([CollageId]);

CREATE INDEX [IX_SubsidiaryJournals_FormDetailsId] ON [SubsidiaryJournals] ([FormDetailsId]);

CREATE INDEX [IX_SubsidiaryJournals_FundId] ON [SubsidiaryJournals] ([FundId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250323223653_Initial-1', N'9.0.6');

CREATE TABLE [SubAccount] (
    [Id] int NOT NULL,
    [AccountId] int NOT NULL,
    [SubAccountName] nvarchar(150) NOT NULL,
    [SubAccountNumber] nvarchar(50) NOT NULL,
    [TempId] uniqueidentifier NULL,
    [Name] nvarchar(100) NULL,
    [CreatedDate] datetime2 NOT NULL,
    [CreatedBy] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_SubAccount] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SubAccount_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_SubAccount_AccountId] ON [SubAccount] ([AccountId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250324220930_Initial-2', N'9.0.6');

ALTER TABLE [SubsidiaryJournals] DROP CONSTRAINT [FK_SubsidiaryJournals_Accounts_AccountId];

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SubAccount]') AND [c].[name] = N'Name');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [SubAccount] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [SubAccount] DROP COLUMN [Name];

EXEC sp_rename N'[SubsidiaryJournals].[AccountId]', N'SubAccountId', 'COLUMN';

EXEC sp_rename N'[SubsidiaryJournals].[IX_SubsidiaryJournals_AccountId]', N'IX_SubsidiaryJournals_SubAccountId', 'INDEX';

ALTER TABLE [SubsidiaryJournals] ADD CONSTRAINT [FK_SubsidiaryJournals_SubAccount_SubAccountId] FOREIGN KEY ([SubAccountId]) REFERENCES [SubAccount] ([Id]) ON DELETE CASCADE;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250324221608_Initial-3', N'9.0.6');

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[GeneralJournal]') AND [c].[name] = N'AccountType');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [GeneralJournal] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [GeneralJournal] DROP COLUMN [AccountType];

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250402161620_Initial-4', N'9.0.6');

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Accounts]') AND [c].[name] = N'Name');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Accounts] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Accounts] DROP COLUMN [Name];

ALTER TABLE [Accounts] ADD [AccountStatus] nvarchar(50) NOT NULL DEFAULT N'';

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250402172445_Initial-6', N'9.0.6');

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Funds]') AND [c].[name] = N'Name');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Funds] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Funds] DROP COLUMN [Name];

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250403152223_Initial-7', N'9.0.6');

ALTER TABLE [SubsidiaryJournals] DROP CONSTRAINT [FK_SubsidiaryJournals_Collages_CollageId];

ALTER TABLE [SubsidiaryJournals] DROP CONSTRAINT [FK_SubsidiaryJournals_Funds_FundId];

DROP INDEX [IX_SubsidiaryJournals_CollageId] ON [SubsidiaryJournals];

DROP INDEX [IX_SubsidiaryJournals_FundId] ON [SubsidiaryJournals];

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SubsidiaryJournals]') AND [c].[name] = N'AccountItem');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [SubsidiaryJournals] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [SubsidiaryJournals] DROP COLUMN [AccountItem];

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SubsidiaryJournals]') AND [c].[name] = N'AccountType');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [SubsidiaryJournals] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [SubsidiaryJournals] DROP COLUMN [AccountType];

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SubsidiaryJournals]') AND [c].[name] = N'CollageId');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [SubsidiaryJournals] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [SubsidiaryJournals] DROP COLUMN [CollageId];

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SubsidiaryJournals]') AND [c].[name] = N'FundId');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [SubsidiaryJournals] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [SubsidiaryJournals] DROP COLUMN [FundId];

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SubsidiaryJournals]') AND [c].[name] = N'TransactionSide');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [SubsidiaryJournals] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [SubsidiaryJournals] DROP COLUMN [TransactionSide];

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250609174043_fix-SubSidaryTable', N'9.0.6');

EXEC sp_rename N'[SubsidiaryJournals].[Amount]', N'Debit', 'COLUMN';

ALTER TABLE [SubsidiaryJournals] ADD [Credit] decimal(18,2) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250613085511_Fix-SubSidary', N'9.0.6');

ALTER TABLE [SubAccount] DROP CONSTRAINT [FK_SubAccount_Accounts_AccountId];

ALTER TABLE [SubsidiaryJournals] DROP CONSTRAINT [FK_SubsidiaryJournals_SubAccount_SubAccountId];

ALTER TABLE [SubAccount] DROP CONSTRAINT [PK_SubAccount];

EXEC sp_rename N'[SubAccount]', N'SubAccounts', 'OBJECT';

EXEC sp_rename N'[SubAccounts].[IX_SubAccount_AccountId]', N'IX_SubAccounts_AccountId', 'INDEX';

ALTER TABLE [SubAccounts] ADD CONSTRAINT [PK_SubAccounts] PRIMARY KEY ([Id]);

ALTER TABLE [SubAccounts] ADD CONSTRAINT [FK_SubAccounts_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]) ON DELETE CASCADE;

ALTER TABLE [SubsidiaryJournals] ADD CONSTRAINT [FK_SubsidiaryJournals_SubAccounts_SubAccountId] FOREIGN KEY ([SubAccountId]) REFERENCES [SubAccounts] ([Id]) ON DELETE CASCADE;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250613215936_Fix-SubAccounts', N'9.0.6');

ALTER TABLE [Forms] ADD [EntryType] nvarchar(50) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250629133601_add-entrytype-to-form', N'9.0.6');

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Forms]') AND [c].[name] = N'EntryType');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [Forms] DROP CONSTRAINT [' + @var9 + '];');
UPDATE [Forms] SET [EntryType] = 0 WHERE [EntryType] IS NULL;
ALTER TABLE [Forms] ALTER COLUMN [EntryType] int NOT NULL;
ALTER TABLE [Forms] ADD DEFAULT 0 FOR [EntryType];

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250629152253_Fix-Entry-Type', N'9.0.6');

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [UserSubAccounts] (
    [UserId] nvarchar(450) NOT NULL,
    [SubAccountId] int NOT NULL,
    CONSTRAINT [PK_UserSubAccounts] PRIMARY KEY ([UserId], [SubAccountId]),
    CONSTRAINT [FK_UserSubAccounts_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserSubAccounts_SubAccounts_SubAccountId] FOREIGN KEY ([SubAccountId]) REFERENCES [SubAccounts] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

CREATE INDEX [IX_UserSubAccounts_SubAccountId] ON [UserSubAccounts] ([SubAccountId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250703072437_Identity', N'9.0.6');

DROP TABLE [UserSubAccounts];

CREATE TABLE [UserAccounts] (
    [UserId] nvarchar(450) NOT NULL,
    [AccountId] int NOT NULL,
    CONSTRAINT [PK_UserAccounts] PRIMARY KEY ([UserId], [AccountId]),
    CONSTRAINT [FK_UserAccounts_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserAccounts_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_UserAccounts_AccountId] ON [UserAccounts] ([AccountId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250703194516_Fix-User-Account-Permisson', N'9.0.6');

ALTER TABLE [UserAccounts] ADD [AppUserId] nvarchar(450) NULL;

ALTER TABLE [UserAccounts] ADD [CreatedBy] nvarchar(100) NOT NULL DEFAULT N'';

ALTER TABLE [UserAccounts] ADD [CreatedDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

ALTER TABLE [UserAccounts] ADD [TempId] uniqueidentifier NULL;

CREATE INDEX [IX_UserAccounts_AppUserId] ON [UserAccounts] ([AppUserId]);

ALTER TABLE [UserAccounts] ADD CONSTRAINT [FK_UserAccounts_AspNetUsers_AppUserId] FOREIGN KEY ([AppUserId]) REFERENCES [AspNetUsers] ([Id]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250712203338_Fix-User-Account', N'9.0.6');

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserAccounts]') AND [c].[name] = N'TempId');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [UserAccounts] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [UserAccounts] DROP COLUMN [TempId];

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SubsidiaryJournals]') AND [c].[name] = N'TempId');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [SubsidiaryJournals] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [SubsidiaryJournals] DROP COLUMN [TempId];

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SubAccounts]') AND [c].[name] = N'TempId');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [SubAccounts] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [SubAccounts] DROP COLUMN [TempId];

DECLARE @var13 sysname;
SELECT @var13 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[GeneralJournal]') AND [c].[name] = N'TempId');
IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [GeneralJournal] DROP CONSTRAINT [' + @var13 + '];');
ALTER TABLE [GeneralJournal] DROP COLUMN [TempId];

DECLARE @var14 sysname;
SELECT @var14 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Funds]') AND [c].[name] = N'TempId');
IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [Funds] DROP CONSTRAINT [' + @var14 + '];');
ALTER TABLE [Funds] DROP COLUMN [TempId];

DECLARE @var15 sysname;
SELECT @var15 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Forms]') AND [c].[name] = N'TempId');
IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [Forms] DROP CONSTRAINT [' + @var15 + '];');
ALTER TABLE [Forms] DROP COLUMN [TempId];

DECLARE @var16 sysname;
SELECT @var16 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Dailies]') AND [c].[name] = N'TempId');
IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [Dailies] DROP CONSTRAINT [' + @var16 + '];');
ALTER TABLE [Dailies] DROP COLUMN [TempId];

DECLARE @var17 sysname;
SELECT @var17 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Collages]') AND [c].[name] = N'TempId');
IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [Collages] DROP CONSTRAINT [' + @var17 + '];');
ALTER TABLE [Collages] DROP COLUMN [TempId];

DECLARE @var18 sysname;
SELECT @var18 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AuditLogs]') AND [c].[name] = N'TempId');
IF @var18 IS NOT NULL EXEC(N'ALTER TABLE [AuditLogs] DROP CONSTRAINT [' + @var18 + '];');
ALTER TABLE [AuditLogs] DROP COLUMN [TempId];

DECLARE @var19 sysname;
SELECT @var19 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Accounts]') AND [c].[name] = N'AccountNumber');
IF @var19 IS NOT NULL EXEC(N'ALTER TABLE [Accounts] DROP CONSTRAINT [' + @var19 + '];');
ALTER TABLE [Accounts] DROP COLUMN [AccountNumber];

DECLARE @var20 sysname;
SELECT @var20 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Accounts]') AND [c].[name] = N'TempId');
IF @var20 IS NOT NULL EXEC(N'ALTER TABLE [Accounts] DROP CONSTRAINT [' + @var20 + '];');
ALTER TABLE [Accounts] DROP COLUMN [TempId];

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250713183704_remove-account-number', N'9.0.6');

DECLARE @var21 sysname;
SELECT @var21 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SubAccounts]') AND [c].[name] = N'SubAccountNumber');
IF @var21 IS NOT NULL EXEC(N'ALTER TABLE [SubAccounts] DROP CONSTRAINT [' + @var21 + '];');
ALTER TABLE [SubAccounts] DROP COLUMN [SubAccountNumber];

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250714151602_Remove-SubAccount-Number', N'9.0.6');

DECLARE @var22 sysname;
SELECT @var22 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Dailies]') AND [c].[name] = N'AccountItem');
IF @var22 IS NOT NULL EXEC(N'ALTER TABLE [Dailies] DROP CONSTRAINT [' + @var22 + '];');
ALTER TABLE [Dailies] DROP COLUMN [AccountItem];

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250714163521_Remove-Daily-Item', N'9.0.6');

COMMIT;
GO

