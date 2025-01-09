CREATE TABLE EmployeeTimeClock (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeId INT NOT NULL,
    ClockInTime DATETIME NOT NULL,
    ClockOutTime DATETIME NULL,
    Location VARCHAR(255) NOT NULL,
    FOREIGN KEY (EmployeeId) REFERENCES Employees(Id)
) 