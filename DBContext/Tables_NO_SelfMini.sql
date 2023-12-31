/****** Object: DATABASE HSPPItest Script Date: 3/8/2023 10:58:53 AM ******/
/******                                                              ******/
/******       MINI TEST                                              ******/
/******                                                              ******/
/******       Creator: Anatoly Mazkun                                ******/
/******       anatoly.mazkun@gmail.com                               ******/
/******       (C) Anatoly Mazkun                                     ******/
/******       License: MIT                                           ******/
/******                                                              ******/
/****** ************************************************************ ******/

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
EXEC sp_configure 'CONTAINED DATABASE AUTHENTICATION'

EXEC sp_configure 'CONTAINED DATABASE AUTHENTICATION', 1
GO

RECONFIGURE WITH OVERRIDE
GO
*/

SET NOCOUNT ON
GO

USE master
GO
if exists (select * from sysdatabases where name='HSPPItest')
		drop database HSPPItest
GO

/*
DECLARE @device_directory NVARCHAR(520)
SELECT @device_directory = SUBSTRING(filename, 1, CHARINDEX(N'master.mdf', LOWER(filename)) - 1)
FROM master.dbo.sysaltfiles WHERE dbid = 1 AND fileid = 1

EXECUTE (N'CREATE DATABASE HSPPItest
  ON PRIMARY (NAME = N''HSPPItest'', FILENAME = N''' + @device_directory + N'HSPPItest.mdf'')
  LOG ON (NAME = N''HSPPItest_log'',  FILENAME = N''' + @device_directory + N'HSPPItest.ldf'')')
GO
*/

CREATE DATABASE HSPPItest ON
(NAME = HSPPItest_dat,
    FILENAME = 'C:\Users\Render\source\repos\WpfApp5\HSPPItestdat.mdf',
    SIZE = 10,
    MAXSIZE = 50,
    FILEGROWTH = 5)
LOG ON
(NAME = HSPPItest_log,
    FILENAME = 'C:\Users\Render\source\repos\WpfApp5\HSPPItestlog.ldf',
    SIZE = 5 MB,
    MAXSIZE = 25 MB,
    FILEGROWTH = 5 MB);
GO


set quoted_identifier on
GO

/* Set DATEFORMAT so that the date strings are interpreted correctly regardless of
   the default DATEFORMAT on the server.
*/
SET DATEFORMAT mdy
GO

use "HSPPItest"
GO

/* TEST */
if exists (select * from sysobjects where id = object_id('dbo.Log') and sysstat & 0xf = 4)
	drop table "dbo"."Log"
GO
if exists (select * from sysobjects where id = object_id('dbo.Roles') and sysstat & 0xf = 4)
	drop table "dbo"."Roles"
GO
if exists (select * from sysobjects where id = object_id('dbo.Users') and sysstat & 0xf = 4)
	drop table "dbo"."Users"
GO
if exists (select * from sysobjects where id = object_id('dbo.Settings') and sysstat & 0xf = 4)
	drop table "dbo"."Settings"
GO


/*
ALTER DATABASE test_entity SET CONTAINMENT = PARTIAL
GO

CREATE USER user001 WITH PASSWORD = 'pass@001';
CREATE USER user002 WITH PASSWORD = 'pass@002';
CREATE USER user003 WITH PASSWORD = 'pass@003';
GO
*/

/****** USER ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [UserName]      NVARCHAR (50) NOT NULL,
    [FirstName]     NVARCHAR (50) NOT NULL,
    [LastName]      NVARCHAR (50) NOT NULL,
    [Active]        BIT           NOT NULL,
    [Suspended]     BIT           NOT NULL,
    [LastLoginDate] DATETIME2     NOT NULL,
    [CreatedOn]     DATETIME2     NOT NULL,
    [CreatedBy]     INT           NOT NULL,
    [ModifiedOn]    DATETIME2     NOT NULL,
    [ModifiedBy]    INT           NOT NULL,
    [Password]      NCHAR(18)     NOT NULL, 
    [Role]          INT           NOT NULL, 

    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC)
		WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
        ) ON [PRIMARY]
GO


/*** LOG ***/
CREATE TABLE [dbo].[Log](
	[Id]        INT IDENTITY(1,1) NOT NULL,
	[Date]      DATETIME2         NOT NULL,
    [Error]     BIT               NOT NULL,
	[User]      INT               NULL,
	[Address]   NVARCHAR(255)     NOT NULL,
	[Message]   NVARCHAR(4000)    NULL,

 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED ([Id] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) 
    ON [PRIMARY],
 CONSTRAINT [FK_UserLog] FOREIGN KEY([User]) REFERENCES Users(Id)
) ON [PRIMARY]

GO

/****** [Settings] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Settings](
	[Id]    INT IDENTITY(1,1)  NOT NULL,
    [User]  INT                NOT NULL,
	[Name]  NVARCHAR(50)       NOT NULL,
	[Value] NVARCHAR(max)      NOT NULL,

 CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED ([Id] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) 
    ON [PRIMARY],
 CONSTRAINT [FK_UserSettings] FOREIGN KEY([User]) REFERENCES Users(Id)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/*** ROLE ***/
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,		
 
    CONSTRAINT [PK_Role] 
        PRIMARY KEY CLUSTERED ([Id] ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) 
        ON [PRIMARY]
) 

ON [PRIMARY]
GO


/*   DATABASE MOKE DATA            */
/*   DATABASE MOKE DATA            */
/*   DATABASE MOKE DATA            */

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

SET IDENTITY_INSERT [dbo].[Roles] ON 
GO
INSERT [dbo].[Roles] ([Id], [Name]) VALUES (1, N'Administrator')
GO
INSERT [dbo].[Roles] ([Id], [Name]) VALUES (2, N'Manager')
GO
INSERT [dbo].[Roles] ([Id], [Name]) VALUES (3, N'Constructor')
GO
INSERT [dbo].[Roles] ([Id], [Name]) VALUES (4, N'Sales')
GO
INSERT [dbo].[Roles] ([Id], [Name]) VALUES (5, N'Shipper')
GO
INSERT [dbo].[Roles] ([Id], [Name]) VALUES (6, N'Supplier')
GO
INSERT [dbo].[Roles] ([Id], [Name]) VALUES (7, N'Operator')
GO
INSERT [dbo].[Roles] ([Id], [Name]) VALUES (8, N'Employer')
GO
INSERT [dbo].[Roles] ([Id], [Name]) VALUES (9, N'User')
GO
INSERT [dbo].[Roles] ([Id], [Name]) VALUES (10, N'Guest')
GO
INSERT [dbo].[Roles] ([Id], [Name]) VALUES (50, N'Developer')
GO
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO

SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([Id], [UserName], [FirstName], [LastName], [Active], [Suspended], [LastLoginDate],                              [CreatedOn],                                  [CreatedBy], [ModifiedOn],                                 [ModifiedBy], [Password], [Role]) 
VALUES                (1,   N'admin',   N'John',     N'Doe',     1,         0, CAST(N'2017-03-08 10:13:56.543' AS DateTime), CAST(N'2017-02-03 10:00:53.063' AS DateTime), 1, CAST(N'2017-02-03 10:00:53.063' AS DateTime), 1, N'admin', 1)
GO
INSERT [dbo].[Users] ([Id], [UserName], [FirstName], [LastName], [Active], [Suspended], [LastLoginDate],                              [CreatedOn],                                  [CreatedBy], [ModifiedOn],                                 [ModifiedBy], [Password], [Role]) 
VALUES                (2,   N'dev',      N'Sima', N'Foo',         1,        0, CAST(N'2018-04-18 11:43:56.543' AS DateTime), CAST(N'2018-02-03 16:00:53.063' AS DateTime), 1, CAST(N'2018-02-03 10:00:53.063' AS DateTime), 1, N'dev', 50)
GO
INSERT [dbo].[Users] ([Id], [UserName], [FirstName], [LastName], [Active], [Suspended], [LastLoginDate],                              [CreatedOn],                                  [CreatedBy], [ModifiedOn],                                 [ModifiedBy], [Password], [Role]) 
VALUES                (3,   N'manager',      N'Sima', N'Foo', 1,       0, CAST(N'2018-04-18 11:43:56.543' AS DateTime), CAST(N'2018-02-03 16:00:53.063' AS DateTime), 1, CAST(N'2018-02-03 10:00:53.063' AS DateTime), 1, N'manager', 2)
GO
INSERT [dbo].[Users] ([Id], [UserName], [FirstName], [LastName], [Active], [Suspended], [LastLoginDate],                              [CreatedOn],                                  [CreatedBy], [ModifiedOn],                                 [ModifiedBy], [Password], [Role]) 
VALUES                (4,   N'constructor',      N'Sima', N'Foo', 1,       0, CAST(N'2018-04-18 11:43:56.543' AS DateTime), CAST(N'2018-02-03 16:00:53.063' AS DateTime), 1, CAST(N'2018-02-03 10:00:53.063' AS DateTime), 1, N'constructor', 3)
GO
INSERT [dbo].[Users] ([Id], [UserName], [FirstName], [LastName], [Active], [Suspended], [LastLoginDate],                              [CreatedOn],                                  [CreatedBy], [ModifiedOn],                                 [ModifiedBy], [Password], [Role]) 
VALUES                (5,   N'sales',      N'Sima', N'Foo', 1,       0, CAST(N'2017-04-18 11:43:56.543' AS DateTime), CAST(N'2018-02-03 16:00:53.063' AS DateTime), 1, CAST(N'2018-02-03 10:00:53.063' AS DateTime), 1, N'sales', 4)
GO
INSERT [dbo].[Users] ([Id], [UserName], [FirstName], [LastName], [Active], [Suspended], [LastLoginDate],                              [CreatedOn],                                  [CreatedBy], [ModifiedOn],                                 [ModifiedBy], [Password], [Role]) 
VALUES                (6,   N'shipper',      N'Sima', N'Foo', 1,       0, CAST(N'2017-04-18 11:43:56.543' AS DateTime), CAST(N'2018-02-03 16:00:53.063' AS DateTime), 1, CAST(N'2018-02-03 10:00:53.063' AS DateTime), 1, N'shipper', 5)
GO
INSERT [dbo].[Users] ([Id], [UserName], [FirstName], [LastName], [Active], [Suspended], [LastLoginDate],                              [CreatedOn],                                  [CreatedBy], [ModifiedOn],                                 [ModifiedBy], [Password], [Role]) 
VALUES                (7,   N'supplier',      N'Sima', N'Foo', 1,       0, CAST(N'2017-04-18 11:43:56.543' AS DateTime), CAST(N'2018-02-03 16:00:53.063' AS DateTime), 1, CAST(N'2018-02-03 10:00:53.063' AS DateTime), 1, N'supplier', 6)
GO
INSERT [dbo].[Users] ([Id], [UserName], [FirstName], [LastName], [Active], [Suspended], [LastLoginDate],                              [CreatedOn],                                  [CreatedBy], [ModifiedOn],                                 [ModifiedBy], [Password], [Role]) 
VALUES                (8,   N'operator',      N'Sima', N'Foo', 1,       0, CAST(N'2017-04-18 11:43:56.543' AS DateTime), CAST(N'2018-02-03 16:00:53.063' AS DateTime), 1, CAST(N'2018-02-03 10:00:53.063' AS DateTime), 1, N'operator', 7)
GO
INSERT [dbo].[Users] ([Id], [UserName], [FirstName], [LastName], [Active], [Suspended], [LastLoginDate],                              [CreatedOn],                                  [CreatedBy], [ModifiedOn],                                 [ModifiedBy], [Password], [Role]) 
VALUES                (9,   N'employer',      N'Sima', N'Foo', 1,       0, CAST(N'2017-04-18 11:43:56.543' AS DateTime), CAST(N'2018-02-03 16:00:53.063' AS DateTime), 1, CAST(N'2018-02-03 10:00:53.063' AS DateTime), 1, N'employer', 8)
GO
INSERT [dbo].[Users] ([Id], [UserName], [FirstName], [LastName], [Active], [Suspended], [LastLoginDate],                              [CreatedOn],                                  [CreatedBy], [ModifiedOn],                                 [ModifiedBy], [Password], [Role]) 
VALUES                (10,   N'guest',      N'Sima', N'Foo', 1,       0, CAST(N'2017-04-18 11:43:56.543' AS DateTime), CAST(N'2018-02-03 16:00:53.063' AS DateTime), 1, CAST(N'2018-02-03 10:00:53.063' AS DateTime), 1, N'guest', 10)
GO
INSERT [dbo].[Users] ([Id], [UserName], [FirstName], [LastName], [Active], [Suspended], [LastLoginDate],                              [CreatedOn],                                  [CreatedBy], [ModifiedOn],                                 [ModifiedBy], [Password], [Role]) 
VALUES                (11,   N'user1',    N'Kuka', N'Zhu',        1,        0,         CAST(N'2017-04-19 10:13:56.543' AS DateTime), CAST(N'2017-02-03 10:00:53.063' AS DateTime), 1, CAST(N'2017-02-03 10:00:53.063' AS DateTime), 1, N'user1', 9)
GO
INSERT [dbo].[Users] ([Id], [UserName], [FirstName], [LastName], [Active], [Suspended], [LastLoginDate],                              [CreatedOn],                                  [CreatedBy], [ModifiedOn],                                 [ModifiedBy], [Password], [Role]) 
VALUES                (12,  N'user2',     N'John',    N'Doe',     1,        0,         CAST(N'2017-05-06 10:13:56.543' AS DateTime), CAST(N'2017-02-03 10:00:53.063' AS DateTime), 1,           CAST(N'2017-02-03 10:00:53.063' AS DateTime), 1,			N'user2',	9)
GO
INSERT [dbo].[Users] ([Id], [UserName], [FirstName], [LastName], [Active], [Suspended], [LastLoginDate],                              [CreatedOn],                                  [CreatedBy], [ModifiedOn],                                 [ModifiedBy], [Password], [Role]) 
VALUES                (13,  N'user3',     N'John',     N'Doe',    1,        0,        CAST(N'2017-03-08 10:13:56.543' AS DateTime), CAST(N'2017-02-03 10:00:53.063' AS DateTime), 1,           CAST(N'2017-02-03 10:00:53.063' AS DateTime), 2,            N'user3',   9)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO

SET IDENTITY_INSERT [dbo].[Settings] ON 
GO
INSERT [dbo].[Settings] ([Id], [User], [Name], [Value]) VALUES (1, 1, N'email.from.address', N'fromemail@example.com')
GO
INSERT [dbo].[Settings] ([Id], [User], [Name], [Value]) VALUES (2, 2, N'email.to.addresses', N'toemail1@example.com;toemail2@example.com')
GO
INSERT [dbo].[Settings] ([Id], [User], [Name], [Value]) VALUES (3, 3, N'email.smtp.host',    N'gmail.example.com')
GO
INSERT [dbo].[Settings] ([Id], [User], [Name], [Value]) VALUES (4, 4, N'website.url',        N'https://www.example.com')
GO
SET IDENTITY_INSERT [dbo].[Settings] OFF
GO

