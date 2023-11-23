USE [reto_Guardians]
GO

INSERT INTO [dbo].[Combate]
           ([id_heroe]
           ,[id_villano]
           ,[lugar]
           ,[fecha]
           ,[resultado])
     VALUES
           (<id_heroe, int,>
           ,<id_villano, int,>
           ,<lugar, varchar(50),>
           ,<fecha, date,>
           ,<resultado, varchar(50),>)
GO

