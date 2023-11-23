USE [reto_Guardians]
GO

INSERT INTO [dbo].[Villano]
           ([id_persona]
           ,[alias]
           ,[origen]
           ,[poder]
           ,[debilidad])
     VALUES
           (<id_persona, int,>
           ,<alias, varchar(50),>
           ,<origen, varchar(50),>
           ,<poder, varchar(50),>
           ,<debilidad, varchar(50),>)
GO

