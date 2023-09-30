CREATE TABLE [dbo].[авто]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [марка_авто] NVARCHAR(100) NOT NULL, 
    [модель] NVARCHAR(100) NOT NULL, 
    [год_выпуска] DATE NOT NULL, 
    [vin] NVARCHAR(50) NULL, 
    CONSTRAINT [PK_Table] PRIMARY KEY ([Id])
)
