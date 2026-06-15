using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Quiz_App
{
    public partial class Welcome : BaseForm
    {
        protected override bool UseAutomaticResponsiveLayout => false;
        private bool hasAppliedResponsiveBounds;
        private Panel heroCard;
        private Panel databaseCard;
        private Label databaseStatusLabel;
        private Label databaseHintLabel;
        private Button localModeButton;
        private Button azureModeButton;
        private Button pauseModeButton;
        private Button testConnectionButton;
        private Panel localSetupCard;
        private Panel azureSetupCard;
        private TextBox localServerTextBox;
        private TextBox localDatabaseTextBox;
        private TextBox localUsernameTextBox;
        private TextBox localPasswordTextBox;
        private TextBox azureServerTextBox;
        private TextBox azureDatabaseTextBox;
        private TextBox azureUsernameTextBox;
        private TextBox azurePasswordTextBox;
        private Button saveLocalButton;
        private Button saveAzureButton;
        private Button connectionSetupButton;
        private Button closeLaunchButton;
        private bool isConnectionSetupVisible;
        private int collapsedHeroHeight;
        private int expandedHeroHeight;

        public Welcome()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (connection_class.CurrentMode == DatabaseMode.Offline)
            {
                MessageBox.Show(
                    "Database access is paused. Choose Local SQL or Azure SQL on this launch page before entering the platform.",
                    "Database Paused",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            Form1 roleSelectionForm = new Form1();
            roleSelectionForm.Show();
            Hide();
        }

        private void CloseLaunchButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Welcome_Load(object sender, EventArgs e)
        {
            ModernUi.ScaleForScreen(this);
            ApplyResponsiveBounds();
            ModernUi.ApplyTheme(this);
            ModernUi.AddGradientBackground(this, Color.FromArgb(9, 15, 29), Color.FromArgb(32, 50, 84));
            BuildWelcomeLayout();
            Shown += Welcome_Shown;
            Resize += Welcome_Resize;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void BuildWelcomeLayout()
        {
            SuspendLayout();

            BackColor = Color.FromArgb(9, 15, 29);
            FormBorderStyle = FormBorderStyle.None;

            int heroWidth = Math.Min(820, ClientSize.Width - 34);
            collapsedHeroHeight = Math.Min(470, ClientSize.Height - 26);
            expandedHeroHeight = Math.Min(700, ClientSize.Height - 18);
            int heroHeight = isConnectionSetupVisible ? expandedHeroHeight : collapsedHeroHeight;
            int heroLeft = (ClientSize.Width - heroWidth) / 2;
            int heroTop = Math.Max(8, (ClientSize.Height - heroHeight) / 2);

            if (heroCard == null)
            {
                heroCard = ModernUi.CreateCard(new Rectangle(heroLeft, heroTop, heroWidth, heroHeight));
                Controls.Add(heroCard);
                heroCard.SendToBack();
            }
            else
            {
                heroCard.Bounds = new Rectangle(heroLeft, heroTop, heroWidth, heroHeight);
            }

            if (closeLaunchButton == null)
            {
                closeLaunchButton = new Button();
                heroCard.Controls.Add(closeLaunchButton);
                ModernUi.StyleDangerButton(closeLaunchButton);
                closeLaunchButton.FlatAppearance.BorderSize = 0;
                closeLaunchButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
                closeLaunchButton.Text = "Close";
                closeLaunchButton.Click += CloseLaunchButton_Click;
            }

            closeLaunchButton.Parent = heroCard;
            closeLaunchButton.Size = new Size(92, 38);
            closeLaunchButton.Location = new Point(heroCard.Width - closeLaunchButton.Width - 28, 26);
            closeLaunchButton.BringToFront();

            pictureBox3.Parent = heroCard;
            pictureBox3.BackColor = Color.Transparent;
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.Size = isConnectionSetupVisible ? new Size(270, 120) : new Size(290, 150);
            pictureBox3.Location = new Point((heroCard.Width - pictureBox3.Width) / 2, isConnectionSetupVisible ? 92 : 104);

            label1.Parent = heroCard;
            label1.BackColor = Color.Transparent;
            label1.ForeColor = ModernUi.Ink;
            label1.Font = new Font("Segoe UI Semibold", isConnectionSetupVisible ? 20F : 22F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Text = "Modern Computer-Based Testing";
            label1.Size = new Size(760, 72);
            label1.Location = new Point((heroCard.Width - label1.Width) / 2, isConnectionSetupVisible ? 18 : 24);
            label1.TextAlign = ContentAlignment.MiddleCenter;

            label2.Parent = heroCard;
            label2.BackColor = Color.Transparent;
            label2.ForeColor = ModernUi.MutedInk;
            label2.Font = new Font("Segoe UI", 10.75F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Text = "Secure sessions, faster grading, cleaner exams, and flexible startup between local SQL and Azure SQL.";
            label2.Size = new Size(720, 64);
            label2.Location = new Point((heroCard.Width - label2.Width) / 2, isConnectionSetupVisible ? 210 : 250);
            label2.TextAlign = ContentAlignment.MiddleCenter;

            if (connectionSetupButton == null)
            {
                connectionSetupButton = new Button();
                heroCard.Controls.Add(connectionSetupButton);
                ModernUi.StyleSecondaryButton(connectionSetupButton);
                connectionSetupButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
                connectionSetupButton.Click += ConnectionSetupButton_Click;
            }

            connectionSetupButton.Parent = heroCard;
            connectionSetupButton.Size = new Size(160, 38);
            connectionSetupButton.Location = new Point((heroCard.Width - connectionSetupButton.Width) / 2, isConnectionSetupVisible ? 272 : 332);

            ModernUi.StylePrimaryButton(button1);
            button1.Parent = heroCard;
            button1.Text = "Enter Platform";
            button1.Size = new Size(200, 58);
            button1.Location = new Point((heroCard.Width - button1.Width) / 2, isConnectionSetupVisible ? 662 : 392);

            Label badge = ModernUi.CreateLabel(
                "Smart launch",
                new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point),
                ModernUi.Accent,
                new Point((heroCard.Width - 160) / 2, 76),
                new Size(160, 28),
                ContentAlignment.MiddleCenter);
            badge.Parent = heroCard;

            BuildDatabaseSelector();
            BuildConnectionEditors();
            LoadSavedConnectionDetails();
            RefreshDatabaseCard();
            ApplyConnectionEditorsVisibility();

            ResumeLayout();
        }

        private void Welcome_Shown(object sender, EventArgs e)
        {
            ApplyResponsiveBounds();
            BuildWelcomeLayout();
        }

        private void Welcome_Resize(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
            {
                BuildWelcomeLayout();
            }
        }

        private void ApplyResponsiveBounds()
        {
            Rectangle workingArea = Screen.FromControl(this).WorkingArea;

            int targetWidth = Math.Min(Math.Max(workingArea.Width - 160, 820), 980);
            int targetHeight = Math.Min(Math.Max(workingArea.Height - 120, 560), 760);

            if (!hasAppliedResponsiveBounds || Width > workingArea.Width || Height > workingArea.Height)
            {
                Size = new Size(targetWidth, targetHeight);
                Location = new Point(
                    workingArea.Left + Math.Max(0, (workingArea.Width - Width) / 2),
                    workingArea.Top + Math.Max(0, (workingArea.Height - Height) / 2));
                hasAppliedResponsiveBounds = true;
            }
            else
            {
                Left = workingArea.Left + Math.Max(0, (workingArea.Width - Width) / 2);
                Top = workingArea.Top + Math.Max(0, (workingArea.Height - Height) / 2);
            }
        }

        private void BuildDatabaseSelector()
        {
            int cardWidth = heroCard.Width - 96;
            int selectorTop = 320;

            if (databaseCard == null)
            {
                databaseCard = ModernUi.CreateCard(new Rectangle(48, selectorTop, cardWidth, 118));
                heroCard.Controls.Add(databaseCard);
            }
            else
            {
                databaseCard.Bounds = new Rectangle(48, selectorTop, cardWidth, 118);
            }

            databaseCard.Controls.Clear();

            Label heading = ModernUi.CreateLabel(
                "Database Mode",
                new Font("Segoe UI Semibold", 12.5F, FontStyle.Bold, GraphicsUnit.Point),
                ModernUi.Ink,
                new Point(24, 18),
                new Size(180, 28),
                ContentAlignment.MiddleLeft);
            databaseCard.Controls.Add(heading);

            databaseHintLabel = ModernUi.CreateLabel(
                "Choose where the app should connect before continuing. Pause keeps the app from opening any SQL connection.",
                new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point),
                ModernUi.MutedInk,
                new Point(24, 46),
                new Size(databaseCard.Width - 48, 18),
                ContentAlignment.MiddleLeft);
            databaseCard.Controls.Add(databaseHintLabel);

            localModeButton = new Button
            {
                Parent = databaseCard,
                Text = "Use Local SQL",
                Size = new Size(150, 44),
                Location = new Point(24, 58)
            };
            ModernUi.StyleSecondaryButton(localModeButton);
            localModeButton.Click += (sender, e) => SetDatabaseMode(DatabaseMode.Local);

            azureModeButton = new Button
            {
                Parent = databaseCard,
                Text = "Use Azure SQL",
                Size = new Size(150, 44),
                Location = new Point(188, 58)
            };
            ModernUi.StyleSecondaryButton(azureModeButton);
            azureModeButton.Click += (sender, e) => SetDatabaseMode(DatabaseMode.Azure);

            pauseModeButton = new Button
            {
                Parent = databaseCard,
                Text = "Pause Access",
                Size = new Size(136, 44),
                Location = new Point(352, 58)
            };
            ModernUi.StyleDangerButton(pauseModeButton);
            pauseModeButton.Click += (sender, e) => SetDatabaseMode(DatabaseMode.Offline);

            testConnectionButton = new Button
            {
                Parent = databaseCard,
                Text = "Test Connection",
                Size = new Size(150, 44),
                Location = new Point(databaseCard.Width - 174, 58)
            };
            ModernUi.StylePrimaryButton(testConnectionButton);
            testConnectionButton.Click += TestConnectionButton_Click;

            databaseStatusLabel = ModernUi.CreateLabel(
                string.Empty,
                new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point),
                ModernUi.Accent,
                new Point(24, 100),
                new Size(databaseCard.Width - 48, 22),
                ContentAlignment.MiddleLeft);
            databaseCard.Controls.Add(databaseStatusLabel);
        }

        private void BuildConnectionEditors()
        {
            int editorWidth = (heroCard.Width - 112) / 2;
            int editorTop = 468;
            int editorHeight = 146;

            if (localSetupCard == null)
            {
                localSetupCard = ModernUi.CreateCard(new Rectangle(48, editorTop, editorWidth, editorHeight));
                heroCard.Controls.Add(localSetupCard);
            }
            else
            {
                localSetupCard.Bounds = new Rectangle(48, editorTop, editorWidth, editorHeight);
            }

            if (azureSetupCard == null)
            {
                azureSetupCard = ModernUi.CreateCard(new Rectangle(heroCard.Width - 48 - editorWidth, editorTop, editorWidth, editorHeight));
                heroCard.Controls.Add(azureSetupCard);
            }
            else
            {
                azureSetupCard.Bounds = new Rectangle(heroCard.Width - 48 - editorWidth, editorTop, editorWidth, editorHeight);
            }

            BuildConnectionEditor(
                localSetupCard,
                "Local SQL Setup",
                "Set the machine server or LAN SQL details used inside your office or school.",
                out localServerTextBox,
                out localDatabaseTextBox,
                out localUsernameTextBox,
                out localPasswordTextBox,
                out saveLocalButton,
                SaveLocalButton_Click);

            BuildConnectionEditor(
                azureSetupCard,
                "Azure SQL Setup",
                "Set the Azure server, database, admin username, and password used online.",
                out azureServerTextBox,
                out azureDatabaseTextBox,
                out azureUsernameTextBox,
                out azurePasswordTextBox,
                out saveAzureButton,
                SaveAzureButton_Click);
        }

        private void ConnectionSetupButton_Click(object sender, EventArgs e)
        {
            isConnectionSetupVisible = !isConnectionSetupVisible;
            UpdateConnectionEditorsVisibility();
        }

        private void BuildConnectionEditor(
            Panel host,
            string title,
            string subtitle,
            out TextBox serverBox,
            out TextBox databaseBox,
            out TextBox userBox,
            out TextBox passwordBox,
            out Button saveButton,
            EventHandler saveHandler)
        {
            host.Controls.Clear();

            Label titleLabel = ModernUi.CreateLabel(
                title,
                new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point),
                ModernUi.Ink,
                new Point(18, 14),
                new Size(220, 24),
                ContentAlignment.MiddleLeft);
            host.Controls.Add(titleLabel);

            Label subLabel = ModernUi.CreateLabel(
                subtitle,
                new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point),
                ModernUi.MutedInk,
                new Point(18, 40),
                new Size(host.Width - 120, 18),
                ContentAlignment.MiddleLeft);
            host.Controls.Add(subLabel);

            int gap = 12;
            int fieldWidth = (host.Width - 36 - gap) / 2;

            CreateFieldLabel(host, "Server", 18, 56);
            CreateFieldLabel(host, "Database", 18 + fieldWidth + gap, 56);
            CreateFieldLabel(host, "Username", 18, 98);
            CreateFieldLabel(host, "Password", 18 + fieldWidth + gap, 98);

            serverBox = CreateEditorTextBox(host, 18, 70, fieldWidth);
            databaseBox = CreateEditorTextBox(host, 18 + fieldWidth + gap, 70, fieldWidth);
            userBox = CreateEditorTextBox(host, 18, 112, fieldWidth);
            passwordBox = CreateEditorTextBox(host, 18 + fieldWidth + gap, 112, fieldWidth);
            passwordBox.UseSystemPasswordChar = true;

            saveButton = new Button
            {
                Parent = host,
                Text = "Save",
                Size = new Size(72, 32),
                Location = new Point(host.Width - 90, 14)
            };
            ModernUi.StylePrimaryButton(saveButton);
            saveButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            saveButton.Click += saveHandler;
        }

        private static void CreateFieldLabel(Control parent, string text, int left, int top)
        {
            Label label = new Label
            {
                Parent = parent,
                Text = text,
                ForeColor = ModernUi.MutedInk,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point),
                Location = new Point(left, top),
                Size = new Size(120, 14),
                BackColor = Color.Transparent
            };
        }

        private TextBox CreateEditorTextBox(Control parent, int left, int top, int width)
        {
            TextBox box = new TextBox
            {
                Parent = parent,
                Location = new Point(left, top),
                Size = new Size(width, 24)
            };
            ModernUi.StyleTextInput(box);
            return box;
        }

        private void LoadSavedConnectionDetails()
        {
            PopulateEditor(connection_class.GetConnectionDetails(DatabaseMode.Local), localServerTextBox, localDatabaseTextBox, localUsernameTextBox, localPasswordTextBox, false);
            PopulateEditor(connection_class.GetConnectionDetails(DatabaseMode.Azure), azureServerTextBox, azureDatabaseTextBox, azureUsernameTextBox, azurePasswordTextBox, true);
        }

        private static void PopulateEditor(SqlConnectionStringBuilder builder, TextBox serverBox, TextBox databaseBox, TextBox userBox, TextBox passwordBox, bool normalizeAzureServer)
        {
            if (builder == null)
            {
                return;
            }

            string dataSource = builder.DataSource;
            if (normalizeAzureServer)
            {
                dataSource = dataSource.Replace("tcp:", string.Empty);
                if (dataSource.EndsWith(",1433", StringComparison.OrdinalIgnoreCase))
                {
                    dataSource = dataSource.Substring(0, dataSource.Length - 5);
                }
            }

            serverBox.Text = dataSource;
            databaseBox.Text = builder.InitialCatalog;
            userBox.Text = builder.UserID;
            passwordBox.Text = builder.Password;
        }

        private void SetDatabaseMode(DatabaseMode mode)
        {
            try
            {
                connection_class.SetMode(mode);
                RefreshDatabaseCard();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Could not switch database mode. {ex.Message}",
                    "Database Mode",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void TestConnectionButton_Click(object sender, EventArgs e)
        {
            bool connected = connection_class.TryOpenConnection(out string message);
            RefreshDatabaseCard(message, connected);

            MessageBox.Show(
                message,
                connected ? "Connection Successful" : "Connection Status",
                MessageBoxButtons.OK,
                connected ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }

        private void RefreshDatabaseCard(string overrideStatus = null, bool isHealthy = false)
        {
            DatabaseMode mode = connection_class.CurrentMode;
            string modeLabel = connection_class.GetModeLabel(mode);

            button1.Enabled = mode != DatabaseMode.Offline;
            testConnectionButton.Enabled = mode != DatabaseMode.Offline;

            StyleModeButton(localModeButton, mode == DatabaseMode.Local);
            StyleModeButton(azureModeButton, mode == DatabaseMode.Azure);

            pauseModeButton.BackColor = mode == DatabaseMode.Offline
                ? Color.FromArgb(180, 64, 64)
                : Color.FromArgb(112, 38, 46);

            string status = overrideStatus;
            if (string.IsNullOrWhiteSpace(status))
            {
                status = mode == DatabaseMode.Offline
                    ? "Database access is paused from the launch page."
                    : $"Active mode: {modeLabel}. Press Test Connection before login if you want to verify access.";
            }

            databaseStatusLabel.ForeColor = mode == DatabaseMode.Offline
                ? ModernUi.Warning
                : (isHealthy ? Color.FromArgb(108, 214, 141) : ModernUi.Accent);
            databaseStatusLabel.Text = status;

            databaseHintLabel.Text = mode == DatabaseMode.Azure
                ? "Azure SQL mode uses the 'quiz_azure' connection string in App.config. Update that value with your real Azure server details."
                : mode == DatabaseMode.Local
                    ? "Local SQL mode uses the 'quiz_local' connection string in App.config for the machine-hosted database."
                    : "Pause Access prevents new SQL connections from the app so users do not hit access-denied errors while the database is unavailable.";
        }

        private void UpdateConnectionEditorsVisibility()
        {
            BuildWelcomeLayout();
        }

        private void ApplyConnectionEditorsVisibility()
        {
            if (localSetupCard != null)
            {
                localSetupCard.Visible = isConnectionSetupVisible;
            }

            if (azureSetupCard != null)
            {
                azureSetupCard.Visible = isConnectionSetupVisible;
            }

            if (connectionSetupButton != null)
            {
                connectionSetupButton.Text = isConnectionSetupVisible ? "Hide Setup" : "Connection Setup";
            }

            if (databaseCard != null)
            {
                databaseCard.Visible = isConnectionSetupVisible;
            }
        }

        private static void StyleModeButton(Button button, bool active)
        {
            if (active)
            {
                ModernUi.StylePrimaryButton(button);
            }
            else
            {
                ModernUi.StyleSecondaryButton(button);
            }
        }

        private void SaveLocalButton_Click(object sender, EventArgs e)
        {
            try
            {
                connection_class.ConfigureLocalConnection(
                    localServerTextBox.Text.Trim(),
                    localDatabaseTextBox.Text.Trim(),
                    localUsernameTextBox.Text.Trim(),
                    localPasswordTextBox.Text);

                RefreshDatabaseCard("Local SQL setup saved successfully.", true);
                MessageBox.Show("Local SQL settings saved.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not save Local SQL settings. {ex.Message}", "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveAzureButton_Click(object sender, EventArgs e)
        {
            try
            {
                connection_class.ConfigureAzureConnection(
                    azureServerTextBox.Text.Trim(),
                    azureDatabaseTextBox.Text.Trim(),
                    azureUsernameTextBox.Text.Trim(),
                    azurePasswordTextBox.Text);

                RefreshDatabaseCard("Azure SQL setup saved successfully.", true);
                MessageBox.Show("Azure SQL settings saved.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not save Azure SQL settings. {ex.Message}", "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

