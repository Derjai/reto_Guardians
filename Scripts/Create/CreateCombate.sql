USE [reto_Guardians]
GO

/****** Object:  Table [dbo].[Combate]    Script Date: 23/11/2023 11:37:21 a. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Combate](
	[id_combate] [int] IDENTITY(1,1) NOT NULL,
	[id_heroe] [int] NOT NULL,
	[id_villano] [int] NOT NULL,
	[lugar] [varchar](50) NOT NULL,
	[fecha] [date] NOT NULL,
	[resultado] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Combate] PRIMARY KEY CLUSTERED 
(
	[id_combate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Combate]  WITH CHECK ADD  CONSTRAINT [Heroe_Luchador] FOREIGN KEY([id_heroe])
REFERENCES [dbo].[Heroe] ([id_heroe])
GO

ALTER TABLE [dbo].[Combate] CHECK CONSTRAINT [Heroe_Luchador]
GO

ALTER TABLE [dbo].[Combate]  WITH CHECK ADD  CONSTRAINT [Villano_Luchador] FOREIGN KEY([id_villano])
REFERENCES [dbo].[Villano] ([id_villano])
GO

ALTER TABLE [dbo].[Combate] CHECK CONSTRAINT [Villano_Luchador]
GO

