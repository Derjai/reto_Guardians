USE [reto_Guardians]
GO

/****** Object:  Table [dbo].[Heroe]    Script Date: 23/11/2023 11:39:03 a. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Heroe](
	[id_heroe] [int] IDENTITY(1,1) NOT NULL,
	[id_persona] [int] NOT NULL,
	[alias] [varchar](50) NOT NULL,
	[poder] [varchar](50) NOT NULL,
	[debilidad] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Heroe] PRIMARY KEY CLUSTERED 
(
	[id_heroe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Heroe]  WITH CHECK ADD  CONSTRAINT [Identidad_Heroe] FOREIGN KEY([id_persona])
REFERENCES [dbo].[Persona] ([id_persona])
GO

ALTER TABLE [dbo].[Heroe] CHECK CONSTRAINT [Identidad_Heroe]
GO

