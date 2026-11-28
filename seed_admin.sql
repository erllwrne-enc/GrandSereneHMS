-- 03_seed_admin.sql
-- Creates the default Super Admin account IF the EMPLOYEE table is empty.
--   Username: admin
--   Password: Admin@123
-- Change the password after your first login:
--   Super Admin tab -> double-click the account -> type a New Password -> Save.
-- Safe to run more than once.
USE [DB_HMS];
GO

IF NOT EXISTS (SELECT 1 FROM EMPLOYEE)
BEGIN
    INSERT INTO EMPLOYEE (FirstName, LastName, Username, Password, Role, ContactNumber, IsActive)
    VALUES ('System', 'Administrator', 'admin',
            'kh21Tow7sPnf12/9+7T5zw==:Sk3WeThpcA/w99OW3nuFtF3QRujplibTqkYAXDuulmo=',
            'Super Admin', NULL, 1);
END
GO

SELECT EmployeeID, Username, Role, IsActive FROM EMPLOYEE;