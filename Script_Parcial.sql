CREATE DATABASE ParkingDB;
GO

USE ParkingDB;
GO

-- Tabla de Usuarios
CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(100) UNIQUE NOT NULL,
    Telefono NVARCHAR(20) NOT NULL,
    Contrasena NVARCHAR(255) NOT NULL,
    Rol NVARCHAR(50) NOT NULL CHECK (Rol IN ('Cliente', 'Empleado'))
);

-- Tabla de Sucursales
CREATE TABLE Sucursales (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Direccion NVARCHAR(255) NOT NULL,
    Telefono NVARCHAR(20) NOT NULL,
    AdministradorId INT NOT NULL,
    NumeroEspacios INT NOT NULL,
    FOREIGN KEY (AdministradorId) REFERENCES Usuarios(Id) ON DELETE CASCADE
);

-- Tabla de Espacios de Parqueo
CREATE TABLE EspaciosParqueo (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Numero INT NOT NULL,
    Ubicacion NVARCHAR(255) NOT NULL,
    CostoPorHora DECIMAL(10,2) NOT NULL,
    SucursalId INT NOT NULL,
    FOREIGN KEY (SucursalId) REFERENCES Sucursales(Id) ON DELETE CASCADE
);

-- Tabla de Reservas
CREATE TABLE Reservas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId INT NOT NULL,
    EspacioParqueoId INT NOT NULL,
    Fecha DATETIME NOT NULL,
    CantidadHoras INT NOT NULL CHECK (CantidadHoras > 0),
    Cancelada INT DEFAULT 0,
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id) ON DELETE CASCADE,  -- Elimina las reservas cuando se elimina un usuario
    FOREIGN KEY (EspacioParqueoId) REFERENCES EspaciosParqueo(Id) ON DELETE NO ACTION  -- Elimina las reservas cuando se elimina un espacio de parqueo
);



-- Inserciones de Usuarios
INSERT INTO Usuarios (Nombre, Correo, Telefono, Contrasena, Rol) VALUES
(N'Juan Pérez', N'juan.perez@email.com', N'123456789', N'hashed_password1', N'Cliente'),
(N'María López', N'maria.lopez@email.com', N'987654321', N'hashed_password2', N'Cliente'),
(N'Carlos Gómez', N'carlos.gomez@email.com', N'567890123', N'hashed_password3', N'Cliente'),
(N'Laura Torres', N'laura.torres@email.com', N'456123789', N'hashed_password4', N'Empleado'),
(N'Pedro Ramírez', N'pedro.ramirez@email.com', N'789321456', N'hashed_password5', N'Empleado'),
(N'Ana Castillo', N'ana.castillo@email.com', N'321654987', N'hashed_password6', N'Empleado');

-- Inserciones de Sucursales
INSERT INTO Sucursales (Nombre, Direccion, Telefono, AdministradorId, NumeroEspacios) VALUES
(N'Sucursal Centro', N'Calle 1, Ciudad', N'111111111', 4, 15),
(N'Sucursal Norte', N'Avenida 2, Ciudad', N'222222222', 5, 20),
(N'Sucursal Sur', N'Boulevard 3, Ciudad', N'333333333', 6, 25);

-- Inserciones de Espacios de Parqueo para Sucursal 1
INSERT INTO EspaciosParqueo (Numero, Ubicacion, CostoPorHora, SucursalId) VALUES
(1, N'Primer piso, esquina', 2.50, 1),
(2, N'Primer piso, centro', 2.50, 1),
(3, N'Primer piso, lateral', 2.50, 1),
(4, N'Segundo piso, entrada', 2.50, 1),
(5, N'Segundo piso, esquina', 2.50, 1),
(6, N'Segundo piso, centro', 2.50, 1),
(7, N'Tercer piso, cerca del ascensor', 2.50, 1),
(8, N'Tercer piso, esquina', 2.50, 1),
(9, N'Tercer piso, centro', 2.50, 1),
(10, N'Cuarto piso, lateral', 2.50, 1),
(11, 'Cuarto piso, centro', 2.50, 1),
(12, N'Cuarto piso, esquina', 2.50, 1),
(13, N'Quinto piso, cerca del ascensor', 2.50, 1),
(14, N'Quinto piso, centro', 2.50, 1),
(15, N'Quinto piso, esquina', 2.50, 1);

-- Inserciones de Espacios de Parqueo para Sucursal 2
INSERT INTO EspaciosParqueo (Numero, Ubicacion, CostoPorHora, SucursalId) VALUES
(1, N'Primer piso, entrada', 3.00, 2),
(2, N'Primer piso, centro', 3.00, 2),
(3, N'Primer piso, lateral', 3.00, 2),
(4, N'Segundo piso, cerca del ascensor', 3.00, 2),
(5, N'Segundo piso, esquina', 3.00, 2),
(6, N'Segundo piso, centro', 3.00, 2),
(7, N'Tercer piso, lateral', 3.00, 2),
(8, N'Tercer piso, centro', 3.00, 2),
(9, N'Tercer piso, esquina', 3.00, 2),
(10, N'Cuarto piso, cerca del ascensor', 3.00, 2),
(11, N'Cuarto piso, entrada', 3.00, 2),
(12, N'Cuarto piso, lateral', 3.00, 2),
(13, N'Quinto piso, centro', 3.00, 2),
(14, N'Quinto piso, esquina', 3.00, 2),
(15, N'Sexto piso, cerca del ascensor', 3.00, 2),
(16, N'Sexto piso, entrada', 3.00, 2),
(17, N'Sexto piso, lateral', 3.00, 2),
(18, N'Séptimo piso, centro', 3.00, 2),
(19, N'Séptimo piso, esquina', 3.00, 2),
(20, N'Séptimo piso, lateral', 3.00, 2);


INSERT INTO EspaciosParqueo (Numero, Ubicacion, CostoPorHora, SucursalId) VALUES
(1, N'Primer piso, entrada', 3.00, 3),
(2, N'Primer piso, centro', 3.00, 3),
(3, N'Primer piso, lateral', 3.00, 3),
(4, N'Segundo piso, cerca del ascensor', 3.00, 3),
(5, N'Segundo piso, esquina', 3.00, 3),
(6, N'Segundo piso, centro', 3.00, 3),
(7, N'Tercer piso, lateral', 3.00, 3),
(8, N'Tercer piso, centro', 3.00, 3),
(9, N'Tercer piso, esquina', 3.00, 3),
(10, N'Cuarto piso, cerca del ascensor', 3.00, 3),
(11, N'Cuarto piso, entrada', 3.00, 3),
(12, N'Cuarto piso, lateral', 3.00, 3),
(13, N'Quinto piso, centro', 3.00, 3),
(14, N'Quinto piso, esquina', 3.00, 3),
(15, N'Sexto piso, cerca del ascensor', 3.00, 3),
(16, N'Sexto piso, entrada', 3.00, 3),
(17, N'Sexto piso, lateral', 3.00, 3),
(18, N'Séptimo piso, centro', 3.00, 3),
(19, N'Séptimo piso, esquina', 3.00, 3),
(20, N'Séptimo piso, lateral', 3.00, 3),
(21, N'Octavo piso, cerca del ascensor', 3.00, 3),
(22, N'Octavo piso, centro', 3.00, 3),
(23, N'Octavo piso, lateral', 3.00, 3),
(24, N'Noveno piso, entrada', 3.00, 3),
(25, N'Noveno piso, centro', 3.00, 3);

-- Inserciones de Reservas para Sucursal 1 (Sucursal Centro)
INSERT INTO Reservas (UsuarioId, EspacioParqueoId, Fecha, CantidadHoras) VALUES
(1, 1, '2025-02-25 08:00:00', 3),
(2, 4, '2025-02-25 10:00:00', 2),
(3, 7, '2025-02-25 12:00:00', 4),
(1, 10, '2025-02-25 15:00:00', 5);

-- Inserciones de Reservas para Sucursal 2 (Sucursal Norte)
INSERT INTO Reservas (UsuarioId, EspacioParqueoId, Fecha, CantidadHoras) VALUES
(4, 18, '2025-02-25 09:00:00', 3),
(5, 26, '2025-02-25 11:00:00', 2),
(6, 28, '2025-02-25 14:00:00', 4);

-- Inserciones de Reservas para Sucursal 3 (Sucursal Sur)
INSERT INTO Reservas (UsuarioId, EspacioParqueoId, Fecha, CantidadHoras) VALUES
(1, 50, '2025-02-25 08:30:00', 2),
(2, 58, '2025-02-25 12:30:00', 3),
(4, 60, '2025-02-25 16:00:00', 1);




