CREATE DATABASE Parcial2Jdhf;
USE master
GO

CREATE LOGIN usrparcial2 WITH PASSWORD = '12345678',
	CHECK_POLICY = ON,
	CHECK_EXPIRATION = OFF,
	DEFAULT_DATABASE = Parcial2Jdhf
GO
USE Parcial2Jdhf
GO
CREATE USER usrparcial2 FOR LOGIN usrparcial2
GO
ALTER ROLE db_owner ADD MEMBER usrparcial2
GO



DROP TABLE IF EXISTS Programa
DROP TABLE IF EXISTS Canal
DROP TABLE IF EXISTS CategoriaPrograma

CREATE TABLE Canal(
	id INT IDENTITY(1,1) PRIMARY KEY,
	nombre VARCHAR(50) NOT NULL,
	frecuencia VARCHAR(20),
	estado SMALLINT NOT NULL DEFAULT(1)
)

CREATE TABLE Programa(
	id INT IDENTITY(1, 1) PRIMARY KEY,
	idCanal INT NOT NULL,
	idCategoriaPrograma INT NOT NULL,
	titulo VARCHAR(100) NOT NULL,
	descripcion VARCHAR(250) NOT NULL,
	duracion INT NOT NULL,
	productor VARCHAR(100) NOT NULL,
	fechaEstreno DATE NOT NULL,
	estado SMALLINT NOT NULL DEFAULT(1),
	CONSTRAINT fk_Programa_Canal FOREIGN KEY (idCanal) REFERENCES Canal(id),
	CONSTRAINT fk_Programa_CategoriaPrograma FOREIGN KEY (idCategoriaPrograma) REFERENCES CategoriaPrograma(id),
)

CREATE TABLE CategoriaPrograma(
	id INT IDENTITY(1, 1) PRIMARY KEY,
	nombre varchar(30) NOT NULL,
	estado SMALLINT NOT NULL DEFAULT(1)
)

ALTER TABLE Canal ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE Canal ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();

ALTER TABLE Programa ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE Programa ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();

ALTER TABLE CategoriaPrograma ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE CategoriaPrograma ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();


GO
DROP PROC IF EXISTS paCanalListar;
GO

CREATE PROC paCanalListar
    @parametro VARCHAR(50)
AS
BEGIN
    SELECT
        c.id,
        c.nombre,
        c.frecuencia,
        c.estado,
        c.usuarioRegistro,
        c.fechaRegistro
    FROM Canal c
    WHERE c.estado = 1
        AND (
            c.nombre +
            c.frecuencia
        ) LIKE '%' + REPLACE(@parametro, ' ', '%') + '%'
    ORDER BY
        c.estado DESC,
        c.nombre ASC;
END;
GO

EXEC paCanalListar '';

GO
DROP PROC IF EXISTS paProgramaListar;
GO

CREATE PROC paProgramaListar
    @parametro VARCHAR(50)
AS
BEGIN
    SELECT
        p.id,
        p.idCanal,
        p.idCategoriaPrograma,
        c.nombre AS nombreCanal,
        cp.nombre AS nombreCategoriaPrograma,
        p.titulo,
        p.descripcion,
        p.duracion,
        p.productor,
        p.fechaEstreno,
        p.estado,
        p.usuarioRegistro,
        p.fechaRegistro
    FROM Programa p
        INNER JOIN Canal c
            ON c.id = p.idCanal
        INNER JOIN CategoriaPrograma cp
            ON cp.id = p.idCategoriaPrograma
    WHERE p.estado > -1
        AND (
            p.titulo +
            p.descripcion +
            c.nombre +
            cp.nombre
        ) LIKE '%' + REPLACE(@parametro, ' ', '%') + '%'
    ORDER BY
        p.estado DESC,
        p.titulo ASC;
END;
GO

EXEC paProgramaListar '';



GO
DROP PROC IF EXISTS paCategoriaProgramaListar;
GO

CREATE PROC paCategoriaProgramaListar
    @parametro VARCHAR(50)
AS
BEGIN
    SELECT
        cp.id,
        cp.nombre,
        cp.estado,
        cp.usuarioRegistro,
        cp.fechaRegistro
    FROM CategoriaPrograma cp
    WHERE cp.estado = 1
      AND cp.nombre LIKE '%' + REPLACE(@parametro, ' ', '%') + '%'
    ORDER BY cp.estado DESC, cp.nombre ASC;
END;
GO

INSERT INTO CategoriaPrograma (nombre) VALUES
('Infantil'),
('Informativo'),
('Entretenimiento'),
('Educativo'),
('Deportivo'),
('Cultural')

SELECT * FROM CategoriaPrograma;


INSERT INTO Canal (nombre, frecuencia) VALUES
('Animal Planet', 'Canal 67'),
('Tyc Sports', 'Canal 7'),
('Cartoon Network', 'Canal 21');

SELECT * FROM Programa;


INSERT INTO Programa (idCanal, idCategoriaPrograma, titulo, descripcion, duracion, productor, fechaEstreno) VALUES
(1, 1, 'Planet Earth', 'Serie documental', 60, 'BBC Studios', '2006-03-05');

SELECT * FROM Programa;

