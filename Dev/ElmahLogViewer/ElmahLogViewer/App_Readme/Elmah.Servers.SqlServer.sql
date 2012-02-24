GO

/****** Object:  Table [dbo].[ELMAH_Servers]    Script Date: 06/28/2011 10:27:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ELMAH_Servers](
	[ServerId] [uniqueidentifier] NOT NULL,
	[ConnectionString] [nvarchar](500) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Environment] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_ELMAH_Servers] PRIMARY KEY CLUSTERED 
(
	[ServerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ELMAH_Servers] ADD  CONSTRAINT [DF_ELMAH_Servers_ServerId]  DEFAULT (newid()) FOR [ServerId]
GO

ALTER TABLE [dbo].[ELMAH_Servers] ADD  CONSTRAINT [DF_ELMAH_Servers_Environment]  DEFAULT ('') FOR [Environment]
GO

