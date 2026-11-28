-- Puts every account on one of the 4 official roles and stops stray values
-- (like 'UI' or 'Backend Developer') from being saved again.
-- Safe to run more than once. Run in SSMS against DB_HMS.
USE [DB_HMS];
GO
 
-- 1. Map old names to the official ones
UPDATE EMPLOYEE SET Role = 'Super Admin' WHERE Role = 'Admin';
UPDATE EMPLOYEE SET Role = 'Front Desk'  WHERE Role = 'Receptionist';
 
-- 2. Anything else that is not official becomes the lowest role
UPDATE EMPLOYEE SET Role = 'Front Desk'
WHERE Role NOT IN ('Front Desk', 'Housekeeping', 'Manager', 'Super Admin');
 
-- 3. SAFETY: make sure somebody can still manage accounts.
--    If nobody is a Super Admin, promote the oldest account (usually the first admin).
IF NOT EXISTS (SELECT 1 FROM EMPLOYEE WHERE Role = 'Super Admin')
    UPDATE EMPLOYEE SET Role = 'Super Admin'
    WHERE EmployeeID = (SELECT MIN(EmployeeID) FROM EMPLOYEE);
GO
 
-- 4. Lock it in: the database now refuses any other role value
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_EMPLOYEE_Role')
    ALTER TABLE EMPLOYEE
        ADD CONSTRAINT CK_EMPLOYEE_Role
        CHECK (Role IN ('Front Desk', 'Housekeeping', 'Manager', 'Super Admin'));
GO
 
SELECT EmployeeID, Username, FirstName, LastName, Role, IsActive FROM EMPLOYEE;
 