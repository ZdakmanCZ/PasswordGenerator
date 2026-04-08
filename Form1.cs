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
            this.Size = new Size(500, 530);
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

            // TextBox pro zadání / zobrazení délky hesla (nahrazuje Label)
            TextBox txtLength = new TextBox
            {
                Name = "txtLength",
                Text = "12",
                Location = new Point(380, 20),
                Size = new Size(40, 25),
                Font = new Font("Arial", 10, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Right
            };
            this.Controls.Add(txtLength);

            // TrackBar pro délku hesla
            TrackBar trackBarLength = new TrackBar
            {
                Name = "trackBarLength",
                Location = new Point(180, 20),
                Size = new Size(190, 25),
                Minimum = 4,
                Maximum = 128,
                Value = 12,
                TickStyle = TickStyle.None,
                //TickFrequency = 10
            };

            // Synchronizace: slider -> textbox
            trackBarLength.ValueChanged += (sender, e) =>
            {
                txtLength.Text = trackBarLength.Value.ToString();
            };

            // Ošetøení vstupu v textboxu:
            // - povolit pouze èíslice a ovládací znaky pøi psaní
            // - pøi stisku Enter nebo opuštìní pole validovat a nastavit trackbar
            txtLength.KeyPress += (sender, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };

            txtLength.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    if (int.TryParse(txtLength.Text, out int value))
                    {
                        value = Math.Clamp(value, trackBarLength.Minimum, trackBarLength.Maximum);
                        trackBarLength.Value = value;
                        txtLength.Text = value.ToString();
                    }
                    else
                    {
                        MessageBox.Show($"Zadejte prosím èíslo mezi {trackBarLength.Minimum} a {trackBarLength.Maximum}.", "Neplatná hodnota", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtLength.Text = trackBarLength.Value.ToString();
                    }
                }
            };

            txtLength.Leave += (sender, e) =>
            {
                if (int.TryParse(txtLength.Text, out int value))
                {
                    value = Math.Clamp(value, trackBarLength.Minimum, trackBarLength.Maximum);
                    trackBarLength.Value = value;
                    txtLength.Text = value.ToString();
                }
                else
                {
                    txtLength.Text = trackBarLength.Value.ToString();
                }
            };

            this.Controls.Add(trackBarLength);

            // Popisek pro vlastní text
            Label lblCustomText = new Label
            {
                Text = "Vlastní text (volitelnì):",
                Location = new Point(20, 60),
                Size = new Size(200, 25),
                Font = new Font("Arial", 10, FontStyle.Regular)
            };
            this.Controls.Add(lblCustomText);

            // Textbox pro zadání vlastního textu
            TextBox txtCustomText = new TextBox
            {
                Name = "txtCustomText",
                Location = new Point(20, 85),
                Size = new Size(450, 25),
                Font = new Font("Arial", 10)
            };
            this.Controls.Add(txtCustomText);

            // Checkbox pro zamíchání vlastního textu
            CheckBox chkShuffleText = new CheckBox
            {
                Name = "chkShuffleText",
                Text = "Zamíchat vlastní text",
                Location = new Point(20, 115),
                Size = new Size(200, 25),
                Checked = true,
                Font = new Font("Arial", 10)
            };
            this.Controls.Add(chkShuffleText);

            // Checkbox pro malá písmena
            CheckBox chkLowercase = new CheckBox
            {
                Name = "chkLowercase",
                Text = "Malá písmena (a-z)",
                Location = new Point(20, 160),
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
                Location = new Point(260, 160),
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
                Location = new Point(20, 200),
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
                Location = new Point(260, 200),
                Size = new Size(200, 25),
                Checked = false,
                Font = new Font("Arial", 10)
            };
            this.Controls.Add(chkSpecial);

            // Textbox pro zobrazení hesla
            TextBox txtPassword = new TextBox
            {
                Name = "txtPassword",
                Location = new Point(20, 260),
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
                Location = new Point(20, 320),
                Size = new Size(200, 40),
                Font = new Font("Arial", 11, FontStyle.Bold),
                BackColor = Color.LimeGreen,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnGenerate.Click += (sender, e) => BtnGenerate_Click(sender, e, trackBarLength, chkLowercase, chkUppercase, chkDigits, chkSpecial, txtPassword, txtCustomText, chkShuffleText);
            this.Controls.Add(btnGenerate);

            // Tlaèítko Kopírovat
            Button btnCopy = new Button
            {
                Name = "btnCopy",
                Text = "Kopírovat",
                Location = new Point(240, 320),
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
                Location = new Point(20, 380),
                Size = new Size(450, 40),
                Font = new Font("Arial", 11, FontStyle.Bold),
                BackColor = Color.OrangeRed,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnClear.Click += (sender, e) => BtnClear_Click(sender, e, txtPassword, txtCustomText);
            this.Controls.Add(btnClear);

            // --- Zajistíme, že po spuštìní nebude txtLength mít fokus ---
            // Asynchronnì zrušíme ActiveControl a vymažeme výbìr v txtLength
            this.Shown += (s, e) =>
            {
                this.BeginInvoke(new Action(() =>
                {
                    this.ActiveControl = null;
                    txtLength.SelectionStart = txtLength.Text.Length;
                    txtLength.SelectionLength = 0;
                }));
            };
        }

        private void BtnGenerate_Click(object sender, EventArgs e, TrackBar trackBarLength, 
                                      CheckBox chkLowercase, CheckBox chkUppercase, 
                                      CheckBox chkDigits, CheckBox chkSpecial, TextBox txtPassword, TextBox txtCustomText, CheckBox chkShuffleText)
        {
            try
            {
                int length = trackBarLength.Value;
                string password;

                if (!string.IsNullOrEmpty(txtCustomText.Text))
                {
                    password = _passwordGenerator.GeneratePasswordWithText(
                        length,
                        txtCustomText.Text,
                        chkLowercase.Checked,
                        chkUppercase.Checked,
                        chkDigits.Checked,
                        chkSpecial.Checked,
                        chkShuffleText.Checked
                    );
                }
                else
                {
                    password = _passwordGenerator.GeneratePassword(
                        length,
                        chkLowercase.Checked,
                        chkUppercase.Checked,
                        chkDigits.Checked,
                        chkSpecial.Checked
                    );
                }

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

        private void BtnClear_Click(object sender, EventArgs e, TextBox txtPassword, TextBox txtCustomText)
        {
            txtPassword.Clear();
            txtCustomText.Clear();
        }
    }
}
