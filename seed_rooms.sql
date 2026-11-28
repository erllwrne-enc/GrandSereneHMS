-- 02_seed_minimal.sql
-- Run in SSMS against the DB_HMS database (after the tables exist).
-- Safe to run twice: it only inserts if the tables are empty.
USE [DB_HMS];
GO

IF NOT EXISTS (SELECT 1 FROM ROOM_TYPE)
BEGIN
    INSERT INTO ROOM_TYPE (TypeName, Description, Capacity, PricePerNight) VALUES
        ('Standard Room',  'Cozy room with a queen bed',            2, 2500.00),
        ('Deluxe Room',    'Spacious room with a city view',        3, 4000.00),
        ('Executive Room', 'Premium room with a lounge area',       4, 6000.00),
        ('Suite Room',     'Two-bedroom suite with a living area',  6, 9500.00);
END
GO

IF NOT EXISTS (SELECT 1 FROM ROOM)
BEGIN
    INSERT INTO ROOM (RoomNumber, RoomTypeID, Floor)
    SELECT v.RoomNumber, rt.RoomTypeID, v.Floor
    FROM (VALUES
        ('101', 'Standard Room',  1), ('102', 'Standard Room',  1),
        ('103', 'Standard Room',  1), ('104', 'Standard Room',  1),
        ('201', 'Deluxe Room',    2), ('202', 'Deluxe Room',    2),
        ('203', 'Deluxe Room',    2), ('204', 'Deluxe Room',    2),
        ('301', 'Executive Room', 3), ('302', 'Executive Room', 3),
        ('401', 'Suite Room',     4), ('402', 'Suite Room',     4)
    ) AS v(RoomNumber, TypeName, Floor)
    JOIN ROOM_TYPE rt ON rt.TypeName = v.TypeName;
    -- Status is not listed on purpose: it defaults to 'Available'
END
GO

-- Quick check: you should see 4 room types and 12 rooms
SELECT (SELECT COUNT(*) FROM ROOM_TYPE) AS RoomTypes,
       (SELECT COUNT(*) FROM ROOM)      AS Rooms;