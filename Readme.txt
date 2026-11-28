seed_rooms.sql
- used to insert temporary/sample rooms

seed_admin.sql
- used to create an admin account if not yet available

DB_HMS.sql
- used to create the database

roles_fix.sql
- used to fix roles in database

Status words used (keep the whole team on these):
   Reservation: Confirmed -> Checked-In -> Checked-Out        (or Cancelled)
   Room:        Available -> Occupied -> Cleaning -> Available
   (Housekeeping marks a Cleaning room as Available from the Rooms tab.)
