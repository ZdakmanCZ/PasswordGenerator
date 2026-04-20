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
            Label paswordLength = new Label
            {
                Text = "Délka hesla:",
                Location = new Point(20, 20),
                Size = new Size(150, 25),
                Font = new Font("Rubik", 10, FontStyle.Bold)
            };
            this.Controls.Add(paswordLength);

            // TextBox pro zadání / zobrazení délky hesla
            TextBox textLength = new TextBox
            {
                Name = "textLength",
                Text = "12",
                Location = new Point(380, 20),
                Size = new Size(40, 25),
                Font = new Font("Rubik", 10, FontStyle.Regular),
                TextAlign = HorizontalAlignment.Center
            };
            this.Controls.Add(textLength);

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
                //TickFrequency = 10   - èárky pod trackaberm jsou vypnuté
            };

            // Synchronizace: slider -> textbox
            trackBarLength.ValueChanged += (sender, e) =>
            {
                textLength.Text = trackBarLength.Value.ToString();
            };

            // Ošetøení vstupu v textboxu:
            // - povolit pouze èíslice a ovládací znaky pøi psaní
            // - pøi stisku Enter nebo opuštìní pole validovat a nastavit trackbar
            textLength.KeyPress += (sender, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };

            textLength.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    if (int.TryParse(textLength.Text, out int value))
                    {
                        value = Math.Clamp(value, trackBarLength.Minimum, trackBarLength.Maximum);
                        trackBarLength.Value = value;
                        textLength.Text = value.ToString();
                    }
                    else
                    {
                        MessageBox.Show($"Zadejte prosím èíslo mezi {trackBarLength.Minimum} a {trackBarLength.Maximum}.", "Neplatná hodnota", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textLength.Text = trackBarLength.Value.ToString();
                    }
                }
            };

            textLength.Leave += (sender, e) =>
            {
                if (int.TryParse(textLength.Text, out int value))
                {
                    value = Math.Clamp(value, trackBarLength.Minimum, trackBarLength.Maximum);
                    trackBarLength.Value = value;
                    textLength.Text = value.ToString();
                }
                else
                {
                    textLength.Text = trackBarLength.Value.ToString();
                }
            };

            this.Controls.Add(trackBarLength);

            // Popisek pro vlastní text
            Label labelCustomText = new Label
            {
                Text = "Vlastní text (volitelné):",
                Location = new Point(20, 80),
                Size = new Size(200, 25),
                Font = new Font("Rubik", 10, FontStyle.Regular)
            };
            this.Controls.Add(labelCustomText);

            // Textbox pro zadání vlastního textu
            TextBox textCustomText = new TextBox
            {
                Name = "textCustomText",
                Location = new Point(20, 105),
                Size = new Size(450, 25),
                Font = new Font("Rubik", 10)
            };
            this.Controls.Add(textCustomText);

            // Checkbox pro zamíchání vlastního textu
            CheckBox checkBoxShuffleText = new CheckBox
            {
                Name = "checkBoxShuffleText",
                Text = "Zamíchat vlastní text",
                Location = new Point(20, 135),
                Size = new Size(200, 25),
                Checked = true,
                Font = new Font("Rubik", 10)
            };
            this.Controls.Add(checkBoxShuffleText);

            // Checkbox pro malá písmena
            CheckBox checkBoxLowercase = new CheckBox
            {
                Name = "checkBoxLowercase",
                Text = "Malá písmena (a-z)",
                Location = new Point(20, 180),
                Size = new Size(200, 25),
                Checked = true,
                Font = new Font("Rubik", 10)
            };
            this.Controls.Add(checkBoxLowercase);

            // Checkbox pro velká písmena
            CheckBox checkBoxUppercase = new CheckBox
            {
                Name = "checkBoxUppercase",
                Text = "Velká písmena (A-Z)",
                Location = new Point(260, 180),
                Size = new Size(200, 25),
                Checked = true,
                Font = new Font("Rubik", 10)
            };
            this.Controls.Add(checkBoxUppercase);

            // Checkbox pro èísla
            CheckBox checkBoxDigits = new CheckBox
            {
                Name = "checkBoxDigits",
                Text = "Èísla (0-9)",
                Location = new Point(20, 220),
                Size = new Size(200, 25),
                Checked = true,
                Font = new Font("Rubik", 10)
            };
            this.Controls.Add(checkBoxDigits);

            // Checkbox pro speciální znaky
            CheckBox checkBoxSpecialSymbols = new CheckBox
            {
                Name = "checkBoxSpecialSymbols",
                Text = "Speciální znaky (!@#$...)",
                Location = new Point(260, 220),
                Size = new Size(200, 25),
                Checked = false,
                Font = new Font("Rubik", 10)
            };
            this.Controls.Add(checkBoxSpecialSymbols);

            // Textbox pro zobrazení hesla
            TextBox textPassword = new TextBox
            {
                Name = "textPassword",
                Location = new Point(20, 260),
                Size = new Size(450, 40),
                ReadOnly = true,
                Font = new Font("Courier New", 12),
                Multiline = true
            };
            this.Controls.Add(textPassword);

            // Tlaèítko Generovat
            Button btnGenerate = new Button
            {
                Name = "btnGenerate",
                Text = "Generovat heslo",
                Location = new Point(20, 320),
                Size = new Size(200, 40),
                Font = new Font("Rubik", 11, FontStyle.Bold),
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnGenerate.Click += (sender, e) => BtnGenerate_Click(sender, e, trackBarLength, checkBoxLowercase, checkBoxUppercase, checkBoxDigits, checkBoxSpecialSymbols, textPassword, textCustomText, checkBoxShuffleText);
            this.Controls.Add(btnGenerate);

            // Tlaèítko Kopírovat
            Button btnCopy = new Button
            {
                Name = "btnCopy",
                Text = "Kopírovat",
                Location = new Point(240, 320),
                Size = new Size(230, 40),
                Font = new Font("Rubik", 11, FontStyle.Bold),
                BackColor = Color.DeepSkyBlue,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnCopy.Click += (sender, e) => BtnCopy_Click(sender, e, textPassword);
            this.Controls.Add(btnCopy);

            // Tlaèítko Vymazat
            Button btnClear = new Button
            {
                Name = "btnClear",
                Text = "Vymazat",
                Location = new Point(20, 380),
                Size = new Size(450, 40),
                Font = new Font("Rubik", 11, FontStyle.Bold),
                BackColor = Color.OrangeRed,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnClear.Click += (sender, e) => BtnClear_Click(sender, e, textPassword, textCustomText);
            this.Controls.Add(btnClear);

            // --- Zajistíme, že po spuštìní nebude textLength mít fokus ---
            // Asynchronnì zrušíme ActiveControl a vymažeme výbìr v textLength
            this.Shown += (s, e) =>
            {
                this.BeginInvoke(new Action(() =>
                {
                    this.ActiveControl = null;
                    textLength.SelectionStart = textLength.Text.Length;
                    textLength.SelectionLength = 0;
                }));
            };
        }

        private void BtnGenerate_Click(object sender, EventArgs e, TrackBar trackBarLength, 
                                      CheckBox checkLowercase, CheckBox checkUppercase, 
                                      CheckBox checkDigits, CheckBox checkSpecial, TextBox textPassword, TextBox textCustomText, CheckBox checkShuffleText)
        {
            try
            {
                int length = trackBarLength.Value;
                string password;

                if (!string.IsNullOrEmpty(textCustomText.Text))
                {
                    password = _passwordGenerator.GeneratePasswordWithText(
                        length,
                        textCustomText.Text,
                        checkLowercase.Checked,
                        checkUppercase.Checked,
                        checkDigits.Checked,
                        checkSpecial.Checked,
                        checkShuffleText.Checked
                    );
                }
                else
                {
                    password = _passwordGenerator.GeneratePassword(
                        length,
                        checkLowercase.Checked,
                        checkUppercase.Checked,
                        checkDigits.Checked,
                        checkSpecial.Checked
                    );
                }

                textPassword.Text = password;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnCopy_Click(object sender, EventArgs e, TextBox textPassword)
        {
            if (!string.IsNullOrEmpty(textPassword.Text))
            {
                Clipboard.SetText(textPassword.Text);
                MessageBox.Show("Heslo bylo zkopírováno do schránky.", "Úspìch", 
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Nejdøíve generujte heslo.", "Upozornìní", 
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e, TextBox textPassword, TextBox textCustomText)
        {
            textPassword.Clear();
            textCustomText.Clear();
        }
    }
}
