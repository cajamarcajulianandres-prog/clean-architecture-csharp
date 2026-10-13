-- Crear en una base propia (no usar master).
-- CREATE DATABASE OrdersDb;
-- GO
-- USE OrdersDb;
-- GO
CREATE TABLE Orders (
    Id           UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Customer     NVARCHAR(200)    NOT NULL,
    Product      NVARCHAR(200)    NOT NULL,
    Qty          INT              NOT NULL,
    Price        DECIMAL(18, 2)   NOT NULL,
    CreatedAtUtc DATETIME2        NOT NULL DEFAULT SYSUTCDATETIME()
);
