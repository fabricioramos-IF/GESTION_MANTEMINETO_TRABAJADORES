--CREACION DE LA BASE DE DATOS
CREATE DATABASE TrabajadoresPrueba;
GO
--USO DE LA DB_TrabajadoresPrueba
USE TrabajadoresPrueba;
GO

--CREACION DE LA TABLA Trabajador
CREATE TABLE Trabajador (
    IdTrabajador INT IDENTITY(1,1) NOT NULL,
    Nombres NVARCHAR(100) NOT NULL,
    Apellidos NVARCHAR(100) NOT NULL,
    TipoDocumento NVARCHAR(20) NOT NULL,
    NumeroDocumento NVARCHAR(20) NOT NULL,
    Sexo CHAR(1) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    Foto NVARCHAR(255) NULL,
    Direccion NVARCHAR(200) NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    Activo BIT NOT NULL DEFAULT 1,
    CONSTRAINT PK_Trabajador PRIMARY KEY (IdTrabajador),
    CONSTRAINT CK_Trabajador_Sexo CHECK (Sexo IN ('M','F')),
    CONSTRAINT UQ_Trabajador_Documento UNIQUE (TipoDocumento, NumeroDocumento)
);
GO

--CREACION DE LOS PROCEDIMIENTOS ALMACENADOS
CREATE PROCEDURE sp_listar_trabajadores
    @Sexo CHAR(1) = NULL
AS
BEGIN
    SELECT
        IdTrabajador,
        Nombres,
        Apellidos,
        TipoDocumento,
        NumeroDocumento,
        Sexo,
        FechaNacimiento,
        Foto,
        Direccion
    FROM Trabajador
    WHERE Activo = 1
      AND (@Sexo IS NULL OR Sexo = @Sexo)
    ORDER BY Apellidos, Nombres;
END;
GO

-- PROCEDIMIENTO PARA INSERTAR UN TRABAJADOR
CREATE PROCEDURE sp_insertar_trabajador
(
    @Nombres NVARCHAR(100),
    @Apellidos NVARCHAR(100),
    @TipoDocumento NVARCHAR(20),
    @NumeroDocumento NVARCHAR(20),
    @Sexo CHAR(1),
    @FechaNacimiento DATE,
    @Foto NVARCHAR(255) = NULL,
    @Direccion NVARCHAR(200) = NULL
)
AS
BEGIN
    INSERT INTO Trabajador
    (
        Nombres,
        Apellidos,
        TipoDocumento,
        NumeroDocumento,
        Sexo,
        FechaNacimiento,
        Foto,
        Direccion
    )
    VALUES
    (
        @Nombres,
        @Apellidos,
        @TipoDocumento,
        @NumeroDocumento,
        @Sexo,
        @FechaNacimiento,
        @Foto,
        @Direccion
    );
END;
GO

-- PROCEDIMIENTO PARA ACTUALIZAR TRABAJADOR
CREATE PROCEDURE sp_actualizar_trabajador
(
    @IdTrabajador INT,
    @Nombres NVARCHAR(100),
    @Apellidos NVARCHAR(100),
    @TipoDocumento NVARCHAR(20),
    @NumeroDocumento NVARCHAR(20),
    @Sexo CHAR(1),
    @FechaNacimiento DATE,
    @Foto NVARCHAR(255) = NULL,
    @Direccion NVARCHAR(200) = NULL
)
AS
BEGIN
    UPDATE Trabajador
    SET
        Nombres = @Nombres,
        Apellidos = @Apellidos,
        TipoDocumento = @TipoDocumento,
        NumeroDocumento = @NumeroDocumento,
        Sexo = @Sexo,
        FechaNacimiento = @FechaNacimiento,
        Foto = @Foto,
        Direccion = @Direccion
    WHERE IdTrabajador = @IdTrabajador
      AND Activo = 1;
END;
GO

-- PROCEDIMIENTO PARA ELIMINAR TRABAJADOR
CREATE PROCEDURE sp_eliminar_trabajador
(
    @IdTrabajador INT
)
AS
BEGIN
    UPDATE Trabajador
    SET Activo = 0
    WHERE IdTrabajador = @IdTrabajador;
END;
GO