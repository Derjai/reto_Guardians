USE [reto_Guardians]
GO

/****** Object:  Table [dbo].[Patrocinador]    Script Date: 23/11/2023 11:38:17 a. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Patrocinador](
	[id_patrocinador] [int] IDENTITY(1,1) NOT NULL,
	[id_heroe] [int] NOT NULL,
	[nombre] [varchar](50) NOT NULL,
	[origen] [varchar](50) NOT NULL,
	[monto] [float] NOT NULL,
 CONSTRAINT [PK_Patrocinador] PRIMARY KEY CLUSTERED 
(
	[id_patrocinador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Patrocinador]  WITH CHECK ADD  CONSTRAINT [Patrocinador_Heroe] FOREIGN KEY([id_heroe])
REFERENCES [dbo].[Heroe] ([id_heroe])
GO

ALTER TABLE [dbo].[Patrocinador] CHECK CONSTRAINT [Patrocinador_Heroe]
GO

