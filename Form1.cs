namespace PasswordGenerator
{
    public partial class Form1 : Form
    {
        private readonly PasswordGeneratorEngine _passwordGenerator;

        public Form1()
        {
            InitializeComponent();
            _passwordGenerator = new PasswordGeneratorEngine();
            InitializeUIElements();
        }

        private void InitializeUIElements()
        {
            // Nastavení vlastností formuláøe
            this.Text = "Generátor Hesel";
            this.Size = new Size(500, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            // Popisek délky hesla
            Label lblLength = new Label
            {
                Text = "Délka hesla:",
                Location = new Point(20, 20),
                Size = new Size(150, 25),
                Font = new Font("Arial", 10, FontStyle.Regular)
            };
            this.Controls.Add(lblLength);

            // Label pro zobrazení aktuální hodnoty z TrackBar
            Label lblLengthValue = new Label
            {
                Name = "lblLengthValue",
                Text = "12",
                Location = new Point(380, 20),
                Size = new Size(40, 25),
                Font = new Font("Arial", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleRight
            };
            this.Controls.Add(lblLengthValue);

            // TrackBar pro délku hesla
            TrackBar trackBarLength = new TrackBar
            {
                Name = "trackBarLength",
                Location = new Point(180, 20),
                Size = new Size(190, 25),
                Minimum = 4,
                Maximum = 128,
                Value = 12,
                TickStyle = TickStyle.BottomRight,
                TickFrequency = 10
            };
            trackBarLength.ValueChanged += (sender, e) => lblLengthValue.Text = trackBarLength.Value.ToString();
            this.Controls.Add(trackBarLength);

            // Checkbox pro malá písmena
            CheckBox chkLowercase = new CheckBox
            {
                Name = "chkLowercase",
                Text = "Malá písmena (a-z)",
                Location = new Point(20, 70),
                Size = new Size(200, 25),
                Checked = true,
                Font = new Font("Arial", 10)
            };
            this.Controls.Add(chkLowercase);

            // Checkbox pro velká písmena
            CheckBox chkUppercase = new CheckBox
            {
                Name = "chkUppercase",
                Text = "Velká písmena (A-Z)",
                Location = new Point(20, 110),
                Size = new Size(200, 25),
                Checked = true,
                Font = new Font("Arial", 10)
            };
            this.Controls.Add(chkUppercase);

            // Checkbox pro èísla
            CheckBox chkDigits = new CheckBox
            {
                Name = "chkDigits",
                Text = "Èísla (0-9)",
                Location = new Point(20, 150),
                Size = new Size(200, 25),
                Checked = true,
                Font = new Font("Arial", 10)
            };
            this.Controls.Add(chkDigits);

            // Checkbox pro speciální znaky
            CheckBox chkSpecial = new CheckBox
            {
                Name = "chkSpecial",
                Text = "Speciální znaky (!@#$...)",
                Location = new Point(20, 190),
                Size = new Size(200, 25),
                Checked = false,
                Font = new Font("Arial", 10)
            };
            this.Controls.Add(chkSpecial);

            // Textbox pro zobrazení hesla
            TextBox txtPassword = new TextBox
            {
                Name = "txtPassword",
                Location = new Point(20, 250),
                Size = new Size(450, 40),
                ReadOnly = true,
                Font = new Font("Courier New", 12),
                Multiline = true
            };
            this.Controls.Add(txtPassword);

            // Tlaèítko Generovat
            Button btnGenerate = new Button
            {
                Name = "btnGenerate",
                Text = "Generovat heslo",
                Location = new Point(20, 310),
                Size = new Size(200, 40),
                Font = new Font("Arial", 11, FontStyle.Bold),
                BackColor = Color.LimeGreen,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnGenerate.Click += (sender, e) => BtnGenerate_Click(sender, e, trackBarLength, chkLowercase, chkUppercase, chkDigits, chkSpecial, txtPassword);
            this.Controls.Add(btnGenerate);

            // Tlaèítko Kopírovat
            Button btnCopy = new Button
            {
                Name = "btnCopy",
                Text = "Kopírovat",
                Location = new Point(240, 310),
                Size = new Size(230, 40),
                Font = new Font("Arial", 11, FontStyle.Bold),
                BackColor = Color.DeepSkyBlue,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnCopy.Click += (sender, e) => BtnCopy_Click(sender, e, txtPassword);
            this.Controls.Add(btnCopy);

            // Tlaèítko Vymazat
            Button btnClear = new Button
            {
                Name = "btnClear",
                Text = "Vymazat",
                Location = new Point(20, 370),
                Size = new Size(450, 40),
                Font = new Font("Arial", 11, FontStyle.Bold),
                BackColor = Color.OrangeRed,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnClear.Click += (sender, e) => BtnClear_Click(sender, e, txtPassword);
            this.Controls.Add(btnClear);
        }

        private void BtnGenerate_Click(object sender, EventArgs e, TrackBar trackBarLength, 
                                      CheckBox chkLowercase, CheckBox chkUppercase, 
                                      CheckBox chkDigits, CheckBox chkSpecial, TextBox txtPassword)
        {
            try
            {
                int length = trackBarLength.Value;
                string password = _passwordGenerator.GeneratePassword(
                    length,
                    chkLowercase.Checked,
                    chkUppercase.Checked,
                    chkDigits.Checked,
                    chkSpecial.Checked
                );

                txtPassword.Text = password;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnCopy_Click(object sender, EventArgs e, TextBox txtPassword)
        {
            if (!string.IsNullOrEmpty(txtPassword.Text))
            {
                Clipboard.SetText(txtPassword.Text);
                MessageBox.Show("Heslo bylo zkopírováno do schránky.", "Úspìch", 
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Nejdøíve generujte heslo.", "Upozornìní", 
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e, TextBox txtPassword)
        {
            txtPassword.Clear();
        }
    }
}
