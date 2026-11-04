-- ProductosDB: categorías y productos (relación 1:N)
-- En la nube, omite el CREATE DATABASE y el USE (la base ya está creada) y ejecuta desde CREATE TABLE.
IF DB_ID('ProductosDB') IS NULL
    CREATE DATABASE ProductosDB;
GO

USE ProductosDB;
GO

IF OBJECT_ID('Productos', 'U') IS NOT NULL DROP TABLE Productos;
IF OBJECT_ID('Categorias', 'U') IS NOT NULL DROP TABLE Categorias;
GO

CREATE TABLE Categorias (
    Id     INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Productos (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Nombre      NVARCHAR(100) NOT NULL,
    Precio      DECIMAL(10,2) NOT NULL,
    Stock       INT NOT NULL,
    CategoriaId INT NOT NULL,
    CONSTRAINT FK_Productos_Categorias FOREIGN KEY (CategoriaId) REFERENCES Categorias (Id)
);
GO

INSERT INTO Categorias (Nombre) VALUES ('Laptops'), ('Accesorios'), ('Monitores');

INSERT INTO Productos (Nombre, Precio, Stock, CategoriaId) VALUES
    ('Laptop Lenovo IdeaPad 15', 2599.90, 12, 1),
    ('Mouse Logitech M170',        49.90, 40, 2),
    ('Teclado Redragon Kumara',   189.00, 25, 2);
GO

SELECT * FROM Categorias;
SELECT * FROM Productos;
