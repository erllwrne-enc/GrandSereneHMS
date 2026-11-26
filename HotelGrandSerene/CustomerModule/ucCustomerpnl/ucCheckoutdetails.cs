using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelGrandSerene.CustomerModule.ucCustomerpnl
{
    public partial class ucCheckoutdetails : UserControl
    {
        // Notifies frmCustomerModule when the customer
        // clicks the Add another room button.
        public event EventHandler AddAnotherRoomClicked;
        // Notifies frmCustomerModule when the customer clicks (Back to room selection button)
        public event EventHandler BackToRoomSelectionClicked;
        // Stores the payment method selected by the customer.
        private string selectedPaymentMethod = "";
        // Stores the complete booking amount.
        private decimal checkoutTotal = 0m;

        // Stores the amount required for the 30% deposit.
        private decimal depositAmount = 0m;


        public ucCheckoutdetails()
        {
            InitializeComponent();

            // Connect the Add another room button to its Click handler.
            guna2Button6.Click += guna2Button6_Click;

            // Payment button: runs customer information validation.
            guna2Button7.Click += guna2Button7_Click;

            // Connect the Card Number textbox to its KeyPress event.
            guna2TextBox10.KeyPress += guna2TextBox10_KeyPress;

            // Connect the CVV textbox to its KeyPress event.
            guna2TextBox12.KeyPress += guna2TextBox12_KeyPress;

            // Connect the Expiration Date textbox to its KeyPress event.
            guna2TextBox11.KeyPress += guna2TextBox11_KeyPress;

            // Connect the Credit/debit card button.
            guna2Button4.Click += guna2Button4_Click;

            // Connect the Online banking button.
            guna2Button5.Click += guna2Button5_Click;

            // Connect the Back button to its Click event (Back to room selection button)
            guna2Button2.Click += guna2Button2_Click;
        }


        // Receives the selected rooms and booking dates from the Room Selection page. (this for select this room for details)
        public void SetCheckoutDetails(List<SelectedRoomInfo> selectedRooms, DateTime checkIn, DateTime checkOut)
        {
            // Calculate the number of nights.
            int numberOfNights = (checkOut - checkIn).Days;

            // Prepare a string to display all selected rooms.
            // Stores the name and physical room number of every selected room.
            List<string> roomDetails = new List<string>();

            // Store the total price of all selected rooms per night.
            decimal subtotalPerNight = 0;

            // Loop through all selected physical rooms.
            foreach (SelectedRoomInfo room in selectedRooms)
            {
                // Add the room name and actual room number to the summary.
                roomDetails.Add($"{room.RoomName} - Room {room.RoomNumber}");

                // Add this room's nightly price to the subtotal.
                subtotalPerNight += room.PricePerNight;
            }
            // Combine the selected rooms into separate lines for the checkout label.
            string roomSummary = string.Join("<br/>", roomDetails);

            // Adjust the checkout layout based on the number of selected rooms.
            AdjustCheckoutLayout(selectedRooms.Count); // Connected to: AdjustCheckoutLayout() below. (this for the design in right)

            // Calculate the total room cost for the entire stay.
            decimal subtotal = subtotalPerNight * numberOfNights;

            // Calculate 12% tax.
            decimal tax = subtotal * 0.12m;

            // Set the service fee to the existing ₱30 displayed in your design.
            decimal serviceFee = 30m;

            // Calculate the final amount.
            decimal total = subtotal + tax + serviceFee;

            // Save the total so other methods can use it. 
            checkoutTotal = total;

            // Calculate the required 30% deposit.
            depositAmount = checkoutTotal * 0.30m;

            // Display the updated deposit amount.
            guna2HtmlLabel10.Text = $"₱{depositAmount:N2}";

            // Display the complete payment amount.
            guna2HtmlLabel13.Text = $"₱{checkoutTotal:N2}";

            // Display the selected room information.
            guna2HtmlLabel16.Text = roomSummary;

            // Display the booking dates and number of nights.
            guna2HtmlLabel17.Text =
                $"{checkIn:MMM dd, yyyy} - {checkOut:MMM dd, yyyy} " +
                $"· {numberOfNights} Nights";

            // Display the room subtotal.
            guna2HtmlLabel21.Text = $"₱{subtotal:N2}";

            // Display the tax amount.
            guna2HtmlLabel22.Text = $"₱{tax:N2}";

            // Display the service fee.
            guna2HtmlLabel23.Text = $"₱{serviceFee:N2}";

            // Display the final total.
            guna2HtmlLabel25.Text = $"₱{total:N2}";
        }

        
        // Purpose: Adjust the checkout UI when multiple rooms are selected.
        private void AdjustCheckoutLayout(int numberOfRooms) // this is that methode inside SetCheckoutDetails() (for the design in right)
        {
            // Add 35 pixels for every additional room.
            // This gives each room name enough vertical space.
            int extraHeight = Math.Max(0, numberOfRooms - 1) * 44;

            // ROOM INFORMATION
            // Connected to: guna2HtmlLabel16 (room name and room number).
            guna2HtmlLabel16.AutoSize = false;
            guna2HtmlLabel16.Width = guna2Panel7.ClientSize.Width - guna2HtmlLabel16.Left - 13;
            guna2HtmlLabel16.Height = numberOfRooms * 44;

            // BOOKING DATES
            // Connected to: guna2HtmlLabel17.
            guna2HtmlLabel17.Top = 86 + extraHeight;

            // ROOM SUBTOTAL
            // Label: guna2HtmlLabel18.
            // Amount: guna2HtmlLabel21.
            guna2HtmlLabel18.Top = 112 + extraHeight;
            guna2HtmlLabel21.Top = 112 + extraHeight;

            // TAXES
            // Label: guna2HtmlLabel19.
            // Amount: guna2HtmlLabel22.
            guna2HtmlLabel19.Top = 142 + extraHeight;
            guna2HtmlLabel22.Top = 142 + extraHeight;

            // SERVICE FEE
            // Label: guna2HtmlLabel20.
            // Amount: guna2HtmlLabel23.
            guna2HtmlLabel20.Top = 174 + extraHeight;
            guna2HtmlLabel23.Top = 174 + extraHeight;

            // FINAL TOTAL
            // Label: guna2HtmlLabel24.
            // Amount: guna2HtmlLabel25.
            guna2HtmlLabel24.Top = 211 + extraHeight;
            guna2HtmlLabel25.Top = 211 + extraHeight;

            // ADD ANOTHER ROOM BUTTON
            // Connected to: guna2Button6.
            guna2Button6.Top = 263 + extraHeight;

            // SUMMARY PANEL
            // Connected to: guna2Panel7.
            guna2Panel7.Height = 328 + extraHeight;

            // AGREEMENT PANEL
            // Connected to: guna2Panel4.
            guna2Panel4.Top = 715 + extraHeight;

            // PAYMENT BUTTON
            // Connected to: guna2Button7.
            guna2Button7.Top = 804 + extraHeight;

            // DEMO PAYMENT MESSAGE
            // Connected to: guna2HtmlLabel26.
            // Keep the message below the payment button.
            guna2HtmlLabel26.Top = guna2Button7.Bottom + 5;
        }


        private bool ValidateCustomerInformation()
        {
            // Check if First Name is empty or contains only spaces.
            if (string.IsNullOrWhiteSpace(guna2TextBox1.Text))
            {
                // Show a warning if First Name is missing.
                MessageBox.Show("Please enter your First Name.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                // Focus the First Name textbox.
                guna2TextBox1.Focus();

                // Stop validation because the field is incomplete.
                return false;
            }

            // Check if Last Name is empty.
            if (string.IsNullOrWhiteSpace(guna2TextBox2.Text))
            {
                MessageBox.Show("Please enter your Last Name.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                guna2TextBox2.Focus();
                return false;
            }

            // Check if Email Address is empty.
            if (string.IsNullOrWhiteSpace(guna2TextBox4.Text))
            {
                MessageBox.Show("Please enter your Email Address.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                guna2TextBox4.Focus();
                return false;
            }
            // Check whether the email has a valid format.
            try
            {
                var email = new System.Net.Mail.MailAddress(guna2TextBox4.Text);

                // Reject the input if it does not match the entered address.
                if (email.Address != guna2TextBox4.Text.Trim())
                {
                    throw new FormatException();
                }
            }
            catch (FormatException)
            {
                // Show a warning when the email format is invalid.
                MessageBox.Show(
                    "Please enter a valid Email Address.",
                    "Invalid Email",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                // Return the cursor to the Email textbox.
                guna2TextBox4.Focus();

                // Stop validation.
                return false;
            }



            // Get the additional guest email and remove surrounding spaces.
            string additionalEmail = guna2TextBox8.Text.Trim();

            // Only validate the email if the customer entered something.
            if (!string.IsNullOrWhiteSpace(additionalEmail))
            {
                try
                {
                    // Check whether the entered text has a valid email format.
                    var email = new System.Net.Mail.MailAddress(additionalEmail);

                    // Reject the input if it doesn't match the entered address.
                    if (email.Address != additionalEmail)
                    {
                        throw new FormatException();
                    }
                }
                catch (FormatException)
                {
                    // Display a warning when the email format is invalid.
                    MessageBox.Show(
                        "Please enter a valid Additional Guest Email.",
                        "Invalid Email",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    // Return focus to the Additional Guest Email textbox.
                    guna2TextBox8.Focus();

                    // Stop validation because the email is invalid.
                    return false;
                }
            }




            // Check if Mobile Phone is empty.
            if (string.IsNullOrWhiteSpace(guna2TextBox5.Text))
            {
                MessageBox.Show("Please enter your Mobile Phone.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                guna2TextBox5.Focus();
                return false;
            }

            // Get the mobile number and remove any spaces around it.
            string mobileNumber = guna2TextBox5.Text.Trim();

            // Check if the number starts with 09 and contains 11 digits,
            // or starts with +639 and contains 13 characters.
            bool isValidMobile =
                System.Text.RegularExpressions.Regex.IsMatch(
                    mobileNumber,
                    @"^(09\d{9}|\+639\d{9})$");

            // Reject the mobile number if it doesn't match either format.
            if (!isValidMobile)
            {
                // Show a warning when the mobile number is invalid.
                MessageBox.Show(
                    "Please enter a valid Philippine mobile number.\nExample: 09171234567 or +639171234567",
                    "Invalid Mobile Number",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                // Return the cursor to the Mobile Phone textbox.
                guna2TextBox5.Focus();

                // Stop validation because the mobile number is invalid.
                return false;
            }


            // Get the card number and remove surrounding spaces.
            string cardNumber = guna2TextBox10.Text.Trim();

            // Remove spaces between the card number digits.
            string cleanCardNumber = cardNumber.Replace(" ", "");

            // Check whether the cleaned number contains exactly 16 digits.
            bool isValidCardNumber =
                System.Text.RegularExpressions.Regex.IsMatch(
                    cleanCardNumber,
                    @"^\d{16}$");

            // Reject the card number if it is invalid.
            if (!isValidCardNumber)
            {
                // Display a warning for an invalid card number.
                MessageBox.Show(
                    "Please enter a valid 16-digit card number.",
                    "Invalid Card Number",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                // Return focus to the Card Number textbox.
                guna2TextBox10.Focus();

                // Stop validation.
                return false;
            }



            // Get the expiration date entered by the customer.
            string expirationDate = guna2TextBox11.Text.Trim();

            // Check whether the date follows the MM/YY format.
            bool hasValidFormat =
                System.Text.RegularExpressions.Regex.IsMatch(
                    expirationDate,
                    @"^(0[1-9]|1[0-2])/\d{2}$");

            // Reject the date if its format or month is invalid.
            if (!hasValidFormat)
            {
                MessageBox.Show(
                    "Please enter a valid expiration date in MM/YY format.",
                    "Invalid Expiration Date",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                guna2TextBox11.Focus();
                return false;
            }

            // Convert the entered month into an integer.
            int expirationMonth = int.Parse(expirationDate.Substring(0, 2));

            // Convert the two-digit year into a four-digit year.
            int expirationYear = 2000 + int.Parse(expirationDate.Substring(3, 2));

            // Get the current month and year.
            DateTime currentDate = DateTime.Now;

            // Create a date representing the last day of the expiration month.
            DateTime cardExpiry = new DateTime(
                expirationYear,
                expirationMonth,
                DateTime.DaysInMonth(expirationYear, expirationMonth));

            // Reject the date if the card has already expired.
            if (cardExpiry < new DateTime(currentDate.Year, currentDate.Month, 1))
            {
                MessageBox.Show(
                    "Your card has expired. Please enter another expiration date.",
                    "Expired Card",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                guna2TextBox11.Focus();
                return false;
            }



            // Get the CVV entered by the customer.
            string cvv = guna2TextBox12.Text.Trim();

            // Check whether the CVV contains exactly 3 or 4 digits.
            bool isValidCVV =
                System.Text.RegularExpressions.Regex.IsMatch(
                    cvv,
                    @"^\d{3,4}$");

            // Reject the CVV if it doesn't match the required format.
            if (!isValidCVV)
            {
                // Display a warning when the CVV is invalid.
                MessageBox.Show(
                    "Please enter a valid 3 or 4-digit CVV.",
                    "Invalid CVV",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                // Return focus to the CVV textbox.
                guna2TextBox12.Focus();

                // Stop validation.
                return false;
            }



            // Get the name entered on the card.
            string nameOnCard = guna2TextBox13.Text.Trim();

            // Check whether the Name on Card field is empty.
            if (string.IsNullOrWhiteSpace(nameOnCard))
            {
                // Display a warning if the cardholder name is missing.
                MessageBox.Show(
                    "Please enter the Name on Card.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                // Return focus to the Name on Card textbox.
                guna2TextBox13.Focus();

                // Stop validation because the name is missing.
                return false;
            }



            // Check if Address is empty.
            if (string.IsNullOrWhiteSpace(guna2TextBox6.Text))
            {
                MessageBox.Show("Please enter your Address.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                guna2TextBox6.Focus();
                return false;
            }

            // Check if City is empty.
            if (string.IsNullOrWhiteSpace(guna2TextBox9.Text))
            {
                MessageBox.Show("Please enter your City.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                guna2TextBox9.Focus();
                return false;
            }

            // Check if ZIP/Postal Code is empty.
            if (string.IsNullOrWhiteSpace(guna2TextBox7.Text))
            {
                MessageBox.Show("Please enter your ZIP/Postal Code.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                guna2TextBox7.Focus();
                return false;
            }

            // Check if Province/Region is empty.
            if (string.IsNullOrWhiteSpace(guna2TextBox3.Text))
            {
                MessageBox.Show("Please enter your Province/Region.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                guna2TextBox3.Focus();
                return false;
            }


            // Check whether the customer selected a country.
            if (guna2ComboBox3.SelectedIndex == -1)
            {
                // Display a warning if no country was selected.
                MessageBox.Show(
                    "Please select your Country.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                // Focus the Country dropdown.
                guna2ComboBox3.Focus();

                // Stop validation because the country is missing.
                return false;
            }


            // Check if the customer accepted the Privacy Terms.
            if (!guna2CheckBox1.Checked)
            {
                MessageBox.Show("Please agree to the Privacy Terms.",
                    "Agreement Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                guna2CheckBox1.Focus();
                return false;
            }

            // Check if the customer accepted the Booking Conditions.
            if (!guna2CheckBox2.Checked)
            {
                MessageBox.Show("Please agree to the Booking Conditions.",
                    "Agreement Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                guna2CheckBox2.Focus();
                return false;
            }

            // Check if the customer accepted the Hotel Policies.
            if (!guna2CheckBox3.Checked)
            {
                MessageBox.Show("Please agree to the Hotel Policies.",
                    "Agreement Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                guna2CheckBox3.Focus();
                return false;
            }



            // Check whether the customer selected a payment method.
            if (string.IsNullOrWhiteSpace(selectedPaymentMethod))
            {
                // Display a warning if no payment method was selected.
                MessageBox.Show(
                    "Please select a payment method.",
                    "Payment Method Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                // Stop validation because no payment method was selected.
                return false;
            }


            // Return true if all required fields and agreements are valid.
            return true;
        }






        // Runs whenever the customer presses a key in the Expiration Date textbox.
        private void guna2TextBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow numbers, the slash character, and control keys such as Backspace.
            if (!char.IsDigit(e.KeyChar) &&
                e.KeyChar != '/' &&
                !char.IsControl(e.KeyChar))
            {
                // Block letters, spaces, and other special characters.
                e.Handled = true;
            }
        }



        // Runs whenever the customer presses a key in the CVV textbox.
        private void guna2TextBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only digits and control keys such as Backspace.
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                // Block letters, spaces, and special characters.
                e.Handled = true;
            }
        }



        // Runs whenever the customer presses a key in the Card Number textbox.
        private void guna2TextBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits, spaces, and control keys such as Backspace.
            if (!char.IsDigit(e.KeyChar) &&
                e.KeyChar != ' ' &&
                !char.IsControl(e.KeyChar))
            {
                // Prevent letters and other unwanted characters from appearing.
                e.Handled = true;
            }
        }



        private void guna2Button7_Click(object sender, EventArgs e) // this the yellow button 
        {
            // Run the customer information validation method.
            bool isValid = ValidateCustomerInformation();

            // Stop here if any required information is missing.
            if (!isValid)
            {
                return;
            }

            // Display a temporary message when validation succeeds.
            MessageBox.Show(
                "Customer information is complete!",
                "Validation Successful",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }




        // Runs when the customer selects Credit/debit card.
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            // Save the selected payment method.
            selectedPaymentMethod = "Credit/Debit Card";

            // Highlight the selected payment button.
            guna2Button4.FillColor = Color.LightBlue;

            // Restore the appearance of the unselected button.
            guna2Button5.FillColor = Color.WhiteSmoke;
        }

        // Runs when the customer selects Online banking.
        private void guna2Button5_Click(object sender, EventArgs e)
        {
            // Save the selected payment method.
            selectedPaymentMethod = "Online Banking";

            // Highlight the selected payment button.
            guna2Button5.FillColor = Color.LightBlue;

            // Restore the appearance of the unselected button.
            guna2Button4.FillColor = Color.WhiteSmoke;
        }


        // Runs when the customer clicks Back to room selection (Back to room selection button)
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            // Notify frmCustomerModule that the customer wants to go back.
            BackToRoomSelectionClicked?.Invoke(this, EventArgs.Empty);
        }


        // Notifies frmCustomerModule that the customer wants
        // to return to the Room Selection page.
        private void guna2Button6_Click(object sender, EventArgs e) // Runs when the customer clicks (Add another room button)
        {
            AddAnotherRoomClicked?.Invoke(this, EventArgs.Empty);
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2TextBox13_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
