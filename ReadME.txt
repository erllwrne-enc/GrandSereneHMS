// All the database work for checking guests in and out lives here, so the
// dialogs stay small. Both operations run as ONE transaction (all or nothing).
//
// Status words used (keep the whole team on these):
//   Reservation: Confirmed -> Checked-In -> Checked-Out        (or Cancelled)
//   Room:        Available -> Occupied -> Cleaning -> Available
//   (Housekeeping marks a Cleaning room as Available from the Rooms tab.)