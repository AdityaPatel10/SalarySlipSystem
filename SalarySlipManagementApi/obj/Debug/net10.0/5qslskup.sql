IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Departments] (
    [Id] int NOT NULL IDENTITY,
    [GlobalId] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [IsActive] bit NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Departments] PRIMARY KEY ([Id])
);

CREATE TABLE [Roles] (
    [Id] int NOT NULL IDENTITY,
    [GlobalId] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [IsActive] bit NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
);

CREATE TABLE [Employees] (
    [Id] int NOT NULL IDENTITY,
    [GlobalId] uniqueidentifier NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [Email] nvarchar(150) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [PhoneNumber] nvarchar(15) NOT NULL,
    [DateOfJoining] datetime2 NOT NULL,
    [BankAccountNumber] nvarchar(max) NOT NULL,
    [RoleId] int NOT NULL,
    [DepartmentId] int NOT NULL,
    [IsActive] bit NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Employee_Department] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Employee_Role] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [MonthlySalarySlips] (
    [Id] int NOT NULL IDENTITY,
    [GlobalId] uniqueidentifier NOT NULL,
    [Month] int NOT NULL,
    [Year] int NOT NULL,
    [BasicSalary] decimal(18,2) NOT NULL,
    [HRA] decimal(18,2) NOT NULL,
    [OtherAllowances] decimal(18,2) NOT NULL,
    [PFDeduction] decimal(18,2) NOT NULL,
    [TaxDeduction] decimal(18,2) NOT NULL,
    [GrossSalary] decimal(18,2) NOT NULL,
    [TotalDeduction] decimal(18,2) NOT NULL,
    [NetSalary] decimal(18,2) NOT NULL,
    [GeneratedOn] datetime2 NOT NULL,
    [EmployeeId] int NOT NULL,
    CONSTRAINT [PK_MonthlySalarySlips] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MonthlySalarySlip_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [SalaryStructures] (
    [Id] int NOT NULL IDENTITY,
    [BasicSalary] decimal(18,2) NOT NULL,
    [HRAPercentage] decimal(18,2) NOT NULL,
    [OtherAllowancesPercentage] decimal(18,2) NOT NULL,
    [PFDeductionPercentage] decimal(18,2) NOT NULL,
    [TaxDeductionPercentage] decimal(18,2) NOT NULL,
    [IsActive] bit NOT NULL,
    [EmployeeId] int NOT NULL,
    CONSTRAINT [PK_SalaryStructures] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SalaryStructure_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE NO ACTION
);

CREATE INDEX [IX_Employees_DepartmentId] ON [Employees] ([DepartmentId]);

CREATE UNIQUE INDEX [IX_Employees_Email] ON [Employees] ([Email]);

CREATE INDEX [IX_Employees_RoleId] ON [Employees] ([RoleId]);

CREATE INDEX [IX_MonthlySalarySlips_EmployeeId] ON [MonthlySalarySlips] ([EmployeeId]);

CREATE UNIQUE INDEX [IX_SalaryStructures_EmployeeId] ON [SalaryStructures] ([EmployeeId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260630091151_InitialCreate', N'10.0.9');

COMMIT;
GO

