PRAGMA foreign_keys = ON;

CREATE TABLE IF NOT EXISTS Encargados (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Codigo TEXT NOT NULL UNIQUE,
    Nombre TEXT NOT NULL,
    Telefono TEXT NOT NULL,
    Direccion TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Veterinarios (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Codigo TEXT NOT NULL UNIQUE,
    Nombre TEXT NOT NULL,
    Telefono TEXT NOT NULL,
    Colegiado TEXT NOT NULL UNIQUE,
    Especialidad TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Mascotas (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Codigo TEXT NOT NULL UNIQUE,
    Nombre TEXT NOT NULL,
    Especie TEXT NOT NULL,
    Raza TEXT NOT NULL DEFAULT '',
    Edad INTEGER NOT NULL CHECK (Edad >= 0),
    EncargadoId INTEGER NOT NULL,
    FOREIGN KEY (EncargadoId) REFERENCES Encargados(Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Medicamentos (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Codigo TEXT NOT NULL UNIQUE,
    Nombre TEXT NOT NULL,
    Stock INTEGER NOT NULL CHECK (Stock >= 0)
);

CREATE TABLE IF NOT EXISTS Citas (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    MascotaId INTEGER NOT NULL,
    VeterinarioId INTEGER NOT NULL,
    FechaHora TEXT NOT NULL,
    Motivo TEXT NOT NULL,
    Completada INTEGER NOT NULL DEFAULT 0 CHECK (Completada IN (0, 1)),
    FOREIGN KEY (MascotaId) REFERENCES Mascotas(Id) ON DELETE CASCADE,
    FOREIGN KEY (VeterinarioId) REFERENCES Veterinarios(Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Chequeos (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    MascotaId INTEGER NOT NULL,
    VeterinarioId INTEGER NOT NULL,
    Fecha TEXT NOT NULL,
    Diagnostico TEXT NOT NULL,
    MedicamentoId INTEGER NULL,
    Cantidad INTEGER NOT NULL DEFAULT 0 CHECK (Cantidad >= 0),
    FOREIGN KEY (MascotaId) REFERENCES Mascotas(Id) ON DELETE CASCADE,
    FOREIGN KEY (VeterinarioId) REFERENCES Veterinarios(Id) ON DELETE CASCADE,
    FOREIGN KEY (MedicamentoId) REFERENCES Medicamentos(Id) ON DELETE SET NULL
);

CREATE INDEX IF NOT EXISTS IX_Mascotas_EncargadoId ON Mascotas(EncargadoId);
CREATE INDEX IF NOT EXISTS IX_Citas_Veterinario_Fecha ON Citas(VeterinarioId, FechaHora);
CREATE INDEX IF NOT EXISTS IX_Chequeos_MascotaId ON Chequeos(MascotaId);

-- Datos iniciales (se ignoran si ya existen)
INSERT OR IGNORE INTO Veterinarios (Codigo, Nombre, Telefono, Colegiado, Especialidad) VALUES
    ('V01', 'Dr. Carlos Gomez', '5555-1234', 'COL-101', 'Perros'),
    ('V02', 'Dra. Ana Lopez',   '5555-5678', 'COL-102', 'Gatos');

INSERT OR IGNORE INTO Medicamentos (Codigo, Nombre, Stock) VALUES
    ('M01', 'Amoxicilina 250mg', 20),
    ('M02', 'Meloxicam Gotas', 15),
    ('M03', 'Desparasitante Canino', 30);

INSERT OR IGNORE INTO Encargados (Codigo, Nombre, Telefono, Direccion) VALUES
    ('E01', 'Juan Perez', '5555-9999', 'Zona 10, Ciudad'),
    ('3562', 'Francisco De Leon', '4208-7613', 'Zona 25');

INSERT OR IGNORE INTO Mascotas (Codigo, Nombre, Especie, Raza, Edad, EncargadoId)
    SELECT 'CARNET-001', 'Firulais', 'Perro', '', 3, Id FROM Encargados WHERE Codigo = 'E01';
INSERT OR IGNORE INTO Mascotas (Codigo, Nombre, Especie, Raza, Edad, EncargadoId)
    SELECT '5476', 'Pedri', 'Gato', '', 2, Id FROM Encargados WHERE Codigo = '3562';
