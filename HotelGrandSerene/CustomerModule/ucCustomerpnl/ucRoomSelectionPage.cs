using GrandSereneHotel.DatabaseAccess; // Allows this UserControl to access the existing DatabaseHelper class.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient; // Provides SQL Server database connection and command classes.
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelGrandSerene.CustomerModule.ucCustomerpnl
{
    public partial class ucRoomSelectionPage : UserControl
    {
        private DateTime selectedCheckInDate; // Stores the check-in date received from the customer booking page. (for the room selection page)
        private DateTime selectedCheckOutDate; // Stores the check-out date received from the customer booking page.    
        private int selectedRoomsCount; // Stores the number of rooms requested by the customer.        
        private int selectedAdultsCount; // Stores the number of adults.      
        private int selectedChildrenCount; // Stores the number of children.
        private bool isAddingAnotherRoom = false; // determine if adding a room
        // Provides access to the customer's selected check-in date.
        public DateTime CheckInDate => selectedCheckInDate;// (this is for the button select this room)

        // Provides access to the customer's selected check-out date.
        public DateTime CheckOutDate => selectedCheckOutDate; // (this is for the button select this room)

        // Stores all physical rooms selected by the customer. (this is for the button select this room)
        public List<SelectedRoomInfo> SelectedRooms { get; private set; }
            = new List<SelectedRoomInfo>();

        public event EventHandler RoomSelected;// Notifies the main form when a room is selected. (this is for the button select this room)

        public ucRoomSelectionPage()
        {
            InitializeComponent();
        }

        // Called by frmCustomerModule when the customer clicks
        public void PrepareToAddAnotherRoom() // the (Add another room button) in checkout
        {
            // Enable the mode for selecting exactly one additional room.
            isAddingAnotherRoom = true;
        }

        // This method receives and displays the booking information from the previous page.
        public void SetBookingDetails(DateTime checkIn,DateTime checkOut,int rooms, int adults, int children)
        {
            // Save the received booking dates for the database query. (for the variable at the top)
            selectedCheckInDate = checkIn;
            selectedCheckOutDate = checkOut;
            // Save the requested room and guest counts. (for the variable at the top)
            selectedRoomsCount = rooms;
            selectedAdultsCount = adults;
            selectedChildrenCount = children;

            // Calculate the number of nights between the selected dates.
            int numberOfNights = (checkOut - checkIn).Days;

            // Display the check-in date.
            lblDate1.Text = checkIn.ToString("MMMM dd, yyyy");

            // Display the check-out date.
            lblDate2.Text = checkOut.ToString("MMMM dd, yyyy");

            // Display the calculated number of nights.
            lblNumber.Text = $"{numberOfNights} Nights";

            // Calculate the total number of guests.
            int totalGuests = adults + children;

            // Display the room and guest summary.
            lblPersons.Text =
                $"{rooms} Room{(rooms > 1 ? "s" : "")}, " +
                $"{totalGuests} Guest{(totalGuests > 1 ? "s" : "")}";

            // Load room types that have available physical rooms.
            LoadAvailableRooms();
        }

        // Retrieves room types that have available physical rooms for the selected dates.
        private void LoadAvailableRooms() // this is a loading methode this is that methode for room avail room type(for the room selection page)
        {
            // SQL query retrieves room types with at least one available physical room.
            string query = @"
            SELECT
                RT.RoomTypeID,
                RT.TypeName,
                RT.Description,
                RT.Capacity,
                RT.PricePerNight,
                COUNT(R.RoomID) AS AvailableRoomCount

            FROM dbo.ROOM AS R

            INNER JOIN dbo.ROOM_TYPE AS RT
                ON R.RoomTypeID = RT.RoomTypeID

            WHERE R.Status = 'Available'

            -- Exclude rooms with overlapping confirmed reservations.
            AND NOT EXISTS
            (
                SELECT 1
                FROM dbo.RESERVATION AS RES
                WHERE RES.RoomID = R.RoomID
                    AND RES.Status = 'Confirmed'
                    AND RES.CheckInDate < @CheckOutDate
                    AND RES.CheckOutDate > @CheckInDate
            )

            -- Exclude rooms with overlapping temporary holds.
            AND NOT EXISTS
            (
                SELECT 1
                FROM dbo.TEMP_RESERVATION_HOLD AS H
                WHERE H.RoomID = R.RoomID
                    AND H.CheckInDate < @CheckOutDate
                    AND H.CheckOutDate > @CheckInDate
            )

            -- Group physical rooms by their room type.
            GROUP BY
                RT.RoomTypeID,
                RT.TypeName,
                RT.Description,
                RT.Capacity,
                RT.PricePerNight;";

            try
            {
                // Create a connection using the existing DatabaseHelper.
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    // Create a SQL command using the query and database connection.
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Pass the customer's check-in date to SQL Server.
                        command.Parameters.AddWithValue(
                            "@CheckInDate", selectedCheckInDate);

                        // Pass the customer's check-out date to SQL Server.
                        command.Parameters.AddWithValue(
                            "@CheckOutDate", selectedCheckOutDate);

                        // Create a DataTable to temporarily store SQL query results.
                        DataTable roomTable = new DataTable();

                        // Create an adapter that executes the query and retrieves results.
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            // Fill the DataTable with the available room type records.
                            adapter.Fill(roomTable);
                        }

                        // Display the retrieved room types in the existing UI cards.
                        DisplayAvailableRoomCards(roomTable);
                    }
                }
            }
            catch (Exception ex)
            {
                // Display an error if the database loading operation fails.
                MessageBox.Show(
                    "Unable to load available rooms.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        } // this the end (for the room selection page)

        // Updates the existing room cards using the records retrieved from SQL Server. (for the room selection page)
        private void DisplayAvailableRoomCards(DataTable roomTable) // methode for updating the room
        {
            // Hide all four room cards before displaying the available room types.
            PnlFirstRoom.Visible = false;
            PnlSecondRoom.Visible = false;
            PnlThirdRoom.Visible = false;
            PnlFourthRoom.Visible = false;

            // Loop through every available room type retrieved from the database.
            foreach (DataRow room in roomTable.Rows)
            {
                // Retrieve the room type name from the current database record.
                string roomType = room["TypeName"].ToString();

                // Retrieve the actual room type price from SQL Server.
                decimal price = Convert.ToDecimal(room["PricePerNight"]);

                // Retrieve the room type identifier from SQL Server.
                int roomTypeID = Convert.ToInt32(room["RoomTypeID"]);

                // Get the number of available physical rooms for this room type.
                int availableRoomCount =
                    Convert.ToInt32(room["AvailableRoomCount"]);

                // Identify which existing UI card belongs to this room type.
                switch (roomType)
                {
                    case "Standard Room":

                        // Update the Standard Room card and its availability label.
                        SetRoomCard(
                            PnlFirstRoom,
                            lblSereneKing,
                            lblAmount1,
                            BtnRoom1,
                            lblAvailable1,
                            roomType,
                            price,
                            roomTypeID,
                            availableRoomCount
                        );

                        break;

                    case "Deluxe Room":

                        // Update the Deluxe Room card and its availability label.
                        SetRoomCard(
                            PnlSecondRoom,
                            lblGrandDeluxe,
                            lblAmount2,
                            BtnRoom2,
                            lblAvailable2,
                            roomType,
                            price,
                            roomTypeID,
                            availableRoomCount
                        );

                        break;

                    case "Suite Room":

                        // Update the Suite Room card and its availability label.
                        SetRoomCard(
                            PnlThirdRoom,
                            lblSereneSuite,
                            lblAmount3,
                            BtnRoom3,
                            lblAvailable3,
                            roomType,
                            price,
                            roomTypeID,
                            availableRoomCount
                        );

                        break;

                    case "Executive Room":

                        // Update the Executive Room card and its availability label.
                        SetRoomCard(
                            PnlFourthRoom,
                            lblExecutiveSuite,
                            lblAmount4,
                            BtnRoom4,
                            lblAvailable4,
                            roomType,
                            price,
                            roomTypeID,
                            availableRoomCount
                        );

                        break;
                }
            }
        } // this the end (for the room selection page)

        // Updates the name, price, and visibility of one existing room card.
        private void SetRoomCard(Control roomPanel, Control roomNameLabel, Control roomPriceLabel, Control selectButton, Control availabilityLabel, string roomName,
            decimal price, int roomTypeID, int availableRoomCount) // this a helper methode for methode above this the DisplayAvailableRoomCards() methode 
        {
            // Display the room type name retrieved from SQL Server.
            roomNameLabel.Text = roomName;

            // Display the room price using Philippine peso formatting.
            // N2 means the amount will have two decimal places.
            roomPriceLabel.Text = $"₱{price:N2}";

            // Display the available physical room count.
            // The conditional operator chooses the correct singular or plural word.
            availabilityLabel.Text =
                $"{availableRoomCount} " +
                $"room{(availableRoomCount == 1 ? "" : "s")} available";

            // Make the availability text green.
            availabilityLabel.ForeColor = Color.ForestGreen;

            // Store the RoomTypeID inside the button's Tag property.
            // We will use this later when the customer selects a room.
            selectButton.Tag = roomTypeID;

            // Display the card because SQL Server returned at least one available room.
            roomPanel.Visible = true;
        }



        // Retrieves multiple available physical rooms based on the selected room type and booking dates.
        private void SelectRoom(int roomTypeID) // this is (for button select this room)
        {
            // SQL query retrieves the requested number of available physical rooms.
            string query = @"
        SELECT
            R.RoomID,
            R.RoomNumber,
            RT.TypeName,
            RT.PricePerNight
        FROM dbo.ROOM AS R
        INNER JOIN dbo.ROOM_TYPE AS RT
            ON R.RoomTypeID = RT.RoomTypeID
        WHERE R.RoomTypeID = @RoomTypeID
            AND R.Status = 'Available'

            -- Exclude rooms with overlapping confirmed reservations.
            AND NOT EXISTS
            (
                SELECT 1
                FROM dbo.RESERVATION AS RES
                WHERE RES.RoomID = R.RoomID
                    AND RES.Status = 'Confirmed'
                    AND RES.CheckInDate < @CheckOutDate
                    AND RES.CheckOutDate > @CheckInDate
            )

            -- Exclude rooms with overlapping temporary holds.
            AND NOT EXISTS
            (
                SELECT 1
                FROM dbo.TEMP_RESERVATION_HOLD AS H
                WHERE H.RoomID = R.RoomID
                    AND H.CheckInDate < @CheckOutDate
                    AND H.CheckOutDate > @CheckInDate
            )
        ORDER BY R.RoomID;";

            try
            {
                // Create a temporary list to store the rooms retrieved from SQL Server.
                List<SelectedRoomInfo> availableRooms = new List<SelectedRoomInfo>();

                int roomsToSelect = isAddingAnotherRoom ? 1 : selectedRoomsCount;// Determine how many rooms the customer wants to select.

                // Create a database connection using DatabaseHelper.
                using (SqlConnection connection = DatabaseHelper.GetConnection())
                {
                    // Create a SQL command using the query and connection.
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Pass the requested number of rooms.
                        command.Parameters.Add("@RoomsCount", SqlDbType.Int).Value =
                            selectedRoomsCount;

                        // Pass the selected room type ID.
                        command.Parameters.Add("@RoomTypeID", SqlDbType.Int).Value =
                            roomTypeID;

                        // Pass the customer's check-in date.
                        command.Parameters.Add("@CheckInDate", SqlDbType.Date).Value =
                            selectedCheckInDate.Date;

                        // Pass the customer's check-out date.
                        command.Parameters.Add("@CheckOutDate", SqlDbType.Date).Value =
                            selectedCheckOutDate.Date;

                        // Open the database connection.
                        connection.Open();

                        // Execute the query and read the returned room records.
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Read every available room returned by SQL Server.
                            while (reader.Read())
                            {
                                // Retrieve the physical room ID.
                                int roomID = Convert.ToInt32(reader["RoomID"]);

                                // Check whether this room was already selected.
                                bool alreadySelected = SelectedRooms.Any(
                                    existingRoom => existingRoom.RoomID == roomID
                                );

                                // Skip rooms that are already in the customer's selection.
                                if (alreadySelected)
                                {
                                    continue;
                                }

                                // Create an object containing the physical room information.
                                SelectedRoomInfo room = new SelectedRoomInfo();

                                // Store the physical room ID.
                                room.RoomID = roomID;

                                // Store the actual room number.
                                room.RoomNumber = reader["RoomNumber"].ToString();

                                // Store the room type name.
                                room.RoomName = reader["TypeName"].ToString();

                                // Store the actual price per night.
                                room.PricePerNight = Convert.ToDecimal(
                                    reader["PricePerNight"]
                                );

                                // Add the room to the temporary selection list.
                                availableRooms.Add(room);

                                // Stop once we have retrieved the required number of rooms.
                                if (availableRooms.Count >= roomsToSelect)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }

                // Check whether enough physical rooms were found.
                if (availableRooms.Count < roomsToSelect)
                {
                    // Inform the customer that there are not enough available rooms.
                    MessageBox.Show($"Only {availableRooms.Count} additional room(s) are available.",
                        "Not Enough Rooms",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    // Refresh the room availability displayed on the page.
                    LoadAvailableRooms();

                    return;
                }

                
                // Add the newly selected rooms to the existing collection.
                // Previously selected rooms will remain in the list.
                SelectedRooms.AddRange(availableRooms);

                // Turn off Add Another Room mode after a successful selection.
                isAddingAnotherRoom = false;

                // Notify frmCustomerModule that room selection is complete.
                RoomSelected?.Invoke(this, EventArgs.Empty);

                // Notify the main form that the rooms have been selected.
                RoomSelected?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                // Display an error if the room selection fails.
                MessageBox.Show(
                    "Unable to select rooms.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }




        private void ucRoomSelectionPage_Load(object sender, EventArgs e)
        {

        }

        private void guna2ComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void frmRoomSelection_Load(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel8_Click(object sender, EventArgs e)
        {

        }

        private void lblDescrip13_Click(object sender, EventArgs e)
        {

        }

        private void lblDate2_Click(object sender, EventArgs e)
        {

        }

        private void BtnRoom1_Click(object sender, EventArgs e) // Handles the Standard Room selection button. (this is for the button select this room)
        {
            // Retrieve the RoomTypeID stored in the button's Tag property.
            int roomTypeID = Convert.ToInt32(BtnRoom1.Tag);

            // Find and select an available physical room.
            SelectRoom(roomTypeID);
        }

        private void BtnRoom2_Click(object sender, EventArgs e) // Handles the Deluxe Room selection button. (this is for the button select this room)
        {
            // Retrieve the RoomTypeID stored in the button's Tag property.
            int roomTypeID = Convert.ToInt32(BtnRoom2.Tag);

            // Find and select an available physical room.
            SelectRoom(roomTypeID);
        }

        private void BtnRoom3_Click(object sender, EventArgs e) // Handles the Suite Room selection button. (this is for the button select this room)
        {
            // Retrieve the RoomTypeID stored in the button's Tag property.
            int roomTypeID = Convert.ToInt32(BtnRoom3.Tag);

            // Find and select an available physical room.
            SelectRoom(roomTypeID);
        }

        private void BtnRoom4_Click(object sender, EventArgs e) // Handles the Executive Room selection button. (this is for the button select this room)
        {
            // Retrieve the RoomTypeID stored in the button's Tag property.
            int roomTypeID = Convert.ToInt32(BtnRoom4.Tag);

            // Find and select an available physical room.
            SelectRoom(roomTypeID);
        }
    }

    // Stores the information of one selected physical room.
    public class SelectedRoomInfo
    {
        // Stores the unique ID of the physical room.
        public int RoomID { get; set; }

        // Stores the room number.
        public string RoomNumber { get; set; }

        // Stores the room type name.
        public string RoomName { get; set; }

        // Stores the price per night.
        public decimal PricePerNight { get; set; }
    }

}
