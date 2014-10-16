USE [Article]
GO

/****** Object:  Table [dbo].[ARTICLE_INFO]    Script Date: 10/15/2014 11:15:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ARTICLE_INFO](
	[ID] [int] NOT NULL,
	[NO] [int] NOT NULL,
	[TITLE] [varchar](50) NOT NULL,
	[CONTENTS] [varchar](max) NULL,
	[WRITER] [varchar](50) NOT NULL,
	[DATE] [varchar](50) NOT NULL,
	[PASSWORD] [varchar](50) NULL,
	[HITS] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

