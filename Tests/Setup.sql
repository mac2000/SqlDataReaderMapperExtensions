IF NOT EXISTS(SELECT * FROM sys.databases WHERE name='SqlDataReaderMapperExtensionTests')
BEGIN
	CREATE DATABASE SqlDataReaderMapperExtensionTests
END
GO

USE SqlDataReaderMapperExtensionTests
GO

PRINT 'Creating tables'
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Tag' AND xtype='U')
CREATE TABLE Tag (
	Id INT IDENTITY(1, 1) PRIMARY KEY, 
	Name NVARCHAR(50) NOT NULL
)

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Post' AND xtype='U')
CREATE TABLE Post (
	Id INT IDENTITY(1, 1) PRIMARY KEY, 
	Title NVARCHAR(50) NOT NULL,
	Published BIT NOT NULL DEFAULT 0,
	CreatedAt DATETIME2 NOT NULL DEFAULT CONVERT(DATE, GETDATE()),
	PublishedAt DATETIME2 NULL DEFAULT NULL
)

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PostTags' AND xtype='U')
CREATE TABLE PostTags (
	PostId INT NOT NULL,
	TagId INT NOT NULL,
	PRIMARY KEY (PostId, TagId),
	CONSTRAINT FK_Post FOREIGN KEY (PostId) REFERENCES Post(Id) ON DELETE CASCADE,
	CONSTRAINT FK_Tag FOREIGN KEY (TagId) REFERENCES Tag(Id) ON DELETE CASCADE
)
GO


DECLARE @PostCount INT = (SELECT COUNT(*) FROM Post)
DECLARE @TagCount INT = (SELECT COUNT(*) FROM Tag)
DECLARE @PostTagsCount INT = (SELECT COUNT(*) FROM PostTags)

IF @PostCount <> 3 OR @TagCount <> 2 OR @PostTagsCount <> 3
BEGIN
	PRINT 'Clean tables'

	DELETE FROM PostTags
	DELETE FROM Tag
	DELETE FROM Post

	PRINT 'Seed tables'

	SET NOCOUNT ON

	SET IDENTITY_INSERT Tag ON
	INSERT INTO Tag (Id, Name) VALUES 
		(1, 'Tag 1'),
		(2, 'Tag 2');
	SET IDENTITY_INSERT Tag OFF

	SET IDENTITY_INSERT Post ON
	INSERT INTO Post (Id, Title, Published, PublishedAt) VALUES 
		(1, 'Post 1', 1, CONVERT(DATE, GETDATE())),
		(2, 'Post 2', 0, NULL),
		(3, 'Post 3', 1, CONVERT(DATE, GETDATE()));
	SET IDENTITY_INSERT Post OFF

	INSERT INTO PostTags (PostId, TagId) VALUES 
		(1, 1),
		(2, 1), (2, 2);
END

PRINT 'Recreate view'
GO

IF EXISTS (SELECT * FROM sysobjects WHERE name='SampleView' AND xtype='V') DROP VIEW SampleView
GO

CREATE VIEW SampleView AS
SELECT
	Id,
	Title,
	CreatedAt,
	Published,
	PublishedAt,
	
	ISNULL((SELECT DISTINCT
		LTRIM(RTRIM(T.Name))
		FROM PostTags AS PT
		JOIN Tag AS T ON PT.TagId = T.Id AND PT.PostId = P.Id
        FOR XML PATH ('string'), ROOT('ArrayOfString'), TYPE), '<ArrayOfString></ArrayOfString>') AS ListString,

	ISNULL((SELECT DISTINCT
		LTRIM(RTRIM(T.Id))
		FROM PostTags AS PT
		JOIN Tag AS T ON PT.TagId = T.Id AND PT.PostId = P.Id
        FOR XML PATH ('int'), ROOT('ArrayOfInt'), TYPE), '<ArrayOfInt></ArrayOfInt>') AS ListInt,

	ISNULL((SELECT DISTINCT
		T.Id AS Id,
		LTRIM(RTRIM(T.Name)) AS Name
		FROM PostTags AS PT
		JOIN Tag AS T ON PT.TagId = T.Id AND PT.PostId = P.Id
        FOR XML PATH ('Tag'), ROOT('ArrayOfTag'), TYPE), '<ArrayOfTag></ArrayOfTag>') AS ListTag

FROM Post AS P
GO


