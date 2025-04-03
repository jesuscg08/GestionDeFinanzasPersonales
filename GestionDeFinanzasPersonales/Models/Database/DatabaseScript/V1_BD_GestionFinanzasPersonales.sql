CREATE DATABASE GestionFinanzasPersonales

USE GestionFinanzasPersonales

create table Usuario(
Id int IDENTITY (1,1) PRIMARY KEY,
Nombre VARCHAR(50),
Correo VARCHAR(50),
Clave VARCHAR(255)
);

CREATE TABLE Categoria(
IdCategoria INT IDENTITY (1,1) PRIMARY KEY,
NombreCategoria NVARCHAR(10) NOT NULL CHECK (NombreCategoria IN ('Ingreso', 'Gasto'))
);

CREATE TABLE Tipo(
IdTipo INT IDENTITY (1,1) PRIMARY KEY,
NombreTipo VARCHAR(50),
IdCategoria INT,

constraint fk_categoria foreign key(IdCategoria) references Categoria(IdCategoria)
);

CREATE TABLE CategoriaPresupuesto(
IdCategoriaPresupuesto INT PRIMARY KEY IDENTITY(1,1),
Nombre NVARCHAR(50) NOT NULL
);

CREATE TABLE Presupuesto (
    IdPresupuesto INT PRIMARY KEY IDENTITY(1,1),
    IdUsuario INT NOT NULL,
    IdCategoriaPresupuesto INT NOT NULL,
    Monto DECIMAL(18,2) NOT NULL,
    Mes INT NOT NULL CHECK (Mes BETWEEN 1 AND 12),
    Año INT NOT NULL CHECK (Año > 2000),
    FOREIGN KEY (IdUsuario) REFERENCES Usuario(Id),
    FOREIGN KEY (IdCategoriaPresupuesto) REFERENCES CategoriaPresupuesto(IdCategoriaPresupuesto),
    CONSTRAINT UC_Presupuesto UNIQUE (IdUsuario, IdCategoriaPresupuesto, Mes, Año)
);

CREATE TABLE MetaFinanciera (
    IdMeta INT PRIMARY KEY IDENTITY(1,1),
    IdUsuario INT NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    TipoMeta NVARCHAR(20) NOT NULL CHECK (TipoMeta IN (
		'Ahorro', 
        'Deuda', 
        'Inversion',
        'Educacion',
        'Emergencia',
        'Viaje',
        'Compra',
        'Salud',
        'Retiro',
        'Negocio',
        'Hogar',
        'Automovil',
        'Estudios',
        'Ocio')),
    MontoObjetivo DECIMAL(18,2) NOT NULL,
    MontoAcumulado DECIMAL(18,2) DEFAULT 0,
    FechaInicio DATE NOT NULL,
    FechaObjetivo DATE NOT NULL,
    FOREIGN KEY (IdUsuario) REFERENCES Usuario(Id)
);


create table Gestion(
IdGestion INT IDENTITY (1,1) PRIMARY KEY,
Monto DECIMAL(10, 2) NOT NULL,
IdUsuario INT,
IdTipo INT,
IdCategoriaPresupuesto INT NULL,
FechaCreacion datetime2 NOT NULL DEFAULT SYSDATETIME(),
FechaOperacion datetime2 NOT NULL,

constraint fk_usuario foreign key(IdUsuario) references Usuario(Id),
constraint fk_tipo foreign key(IdTipo) references Tipo(IdTipo),
constraint fk_categoriapresupuesto foreign key(IdCategoriaPresupuesto) REFERENCES CategoriaPresupuesto(IdCategoriaPresupuesto)

);


CREATE TABLE Notificacion (
    IdNotificacion INT PRIMARY KEY IDENTITY(1,1),
    IdUsuario INT NOT NULL,
    Titulo NVARCHAR(100) NOT NULL,
    Mensaje NVARCHAR(500) NOT NULL,
    Tipo NVARCHAR(30) NOT NULL CHECK (Tipo IN ('ExcesoPresupuesto', 'RecordatorioPago', 'SaldoBajo', 'MetaAlcanzada')),
    FechaCreacion DATETIME DEFAULT GETDATE(),
    Leida BIT DEFAULT 0,
    FOREIGN KEY (IdUsuario) REFERENCES Usuario(Id)
);


CREATE TABLE Recordatorio (
    IdRecordatorio INT PRIMARY KEY IDENTITY(1,1),
    IdUsuario INT NOT NULL,
    Descripcion NVARCHAR(200) NOT NULL,
    Fecha DATE NOT NULL,
    Repetir BIT DEFAULT 0,
    Frecuencia NVARCHAR(15) NULL CHECK (Frecuencia IN ('Diario', 'Semanal', 'Mensual', 'Anual')),
    FOREIGN KEY (IdUsuario) REFERENCES Usuario(Id),
    
);

-- Insertar datos iniciales

INSERT INTO Categoria (NombreCategoria) VALUES ('Ingreso'), ('Gasto');
INSERT INTO Tipo (NombreTipo, IdCategoria) VALUES ('Alquiler',1), ('Salario',1), ('Venta',1), ('Intereses',1), ('Donación',1), ('Otros ingresos',1);
INSERT INTO Tipo (NombreTipo, IdCategoria) VALUES ('Servicios públicos',2), ('Comida',2), ('Restaurante',2), ('Gasolina',2), ('Donación',2), ('Otros gastos',2);
INSERT INTO CategoriaPresupuesto(Nombre) VALUES('Transporte'),('Alimentación'), ('Educación'), ('Salud'),('Entretenimiento'),('Tecnología'),('Otros');

