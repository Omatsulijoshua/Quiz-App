using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Quiz_App
{
    public partial class Form2 : BaseForm
    {
        protected override bool UseAutomaticResponsiveLayout => false;
        private Label sectionCaptionLabel;
        private Label statusLabel;
        private Guna2Button activeNavButton;
        private bool shellLayoutEventsAttached;
        private bool shellReady;

        public Form2()
        {
            InitializeComponent();
            Opacity = 0;
        }

        public void loadform(object formInstance)
        {
            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            Form childForm = formInstance as Form;
            if (childForm == null)
            {
                return;
            }

            childForm.TopLevel = false;
            childForm.Dock = DockStyle.Fill;
            childForm.FormBorderStyle = FormBorderStyle.None;
            ModernUi.ApplyTheme(childForm);
            mainpanel.Controls.Add(childForm);
            mainpanel.Tag = childForm;
            childForm.Show();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            ModernUi.ApplyTheme(this);
            AttachShellLayoutEvents();
            sidepanel.Visible = false;
            panelheader.Visible = false;
            mainpanel.Visible = false;

            BeginInvoke(new Action(InitializeShellAfterShow));
        }

        private void AttachShellLayoutEvents()
        {
            if (shellLayoutEventsAttached)
            {
                return;
            }

            shellLayoutEventsAttached = true;
            Resize += (sender, e) =>
            {
                if (shellReady && WindowState != FormWindowState.Minimized)
                {
                    BuildAdminShell();
                }
            };
        }

        private void InitializeShellAfterShow()
        {
            if (shellReady)
            {
                return;
            }

            Rectangle workingArea = Screen.FromControl(this).WorkingArea;
            WindowState = FormWindowState.Normal;
            Bounds = workingArea;
            Location = workingArea.Location;
            Size = workingArea.Size;

            BuildAdminShell();

            sidepanel.Visible = true;
            panelheader.Visible = true;
            mainpanel.Visible = true;
            shellReady = true;
            Opacity = 1;
        }

        private void BuildAdminShell()
        {
            SuspendLayout();
            sidepanel.SuspendLayout();
            panelheader.SuspendLayout();
            mainpanel.SuspendLayout();

            BackColor = Color.FromArgb(8, 12, 24);
            sidepanel.Controls.Clear();
            panelheader.Controls.Clear();
            mainpanel.Controls.Clear();

            sidepanel.Dock = DockStyle.Left;
            int sidebarWidth = Math.Max(210, Math.Min(290, ClientSize.Width / 5));
            sidepanel.Width = sidebarWidth;
            sidepanel.WrapContents = false;
            sidepanel.FlowDirection = FlowDirection.TopDown;
            sidepanel.AutoScroll = true;
            sidepanel.Padding = new Padding(16, 18, 16, 18);
            sidepanel.BackColor = Color.FromArgb(11, 18, 31);

            panelheader.Dock = DockStyle.Top;
            panelheader.Height = 82;
            panelheader.Padding = new Padding(18, 14, 18, 14);
            panelheader.BackColor = Color.FromArgb(13, 21, 36);

            mainpanel.Dock = DockStyle.Fill;
            mainpanel.Padding = new Padding(14, 14, 14, 16);
            mainpanel.BackColor = Color.FromArgb(16, 24, 39);

            ModernUi.AddPanelChrome(panelheader);
            ModernUi.AddPanelChrome(mainpanel);

            BuildHeader();
            BuildSidebar();

            ResumeLayout(true);
            sidepanel.ResumeLayout(true);
            panelheader.ResumeLayout(true);
            mainpanel.ResumeLayout(true);

            ShowHomeDashboard();
        }

        private void BuildHeader()
        {
            Panel titleStack = new Panel
            {
                Dock = DockStyle.Left,
                Width = Math.Max(220, panelheader.ClientSize.Width - 280),
                BackColor = Color.Transparent
            };

            label5 = new Label
            {
                Text = "Admin Command Center",
                ForeColor = ModernUi.Ink,
                Font = new Font("Segoe UI Semibold", 13.5F, FontStyle.Bold, GraphicsUnit.Point),
                Location = new Point(0, 4),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            sectionCaptionLabel = new Label
            {
                Text = "Control exams, questions, scores, and reports from one place.",
                ForeColor = ModernUi.MutedInk,
                Font = new Font("Segoe UI", 8.75F, FontStyle.Regular, GraphicsUnit.Point),
                Location = new Point(2, 36),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            titleStack.Controls.Add(sectionCaptionLabel);
            titleStack.Controls.Add(label5);

            FlowLayoutPanel actionsPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Right,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 2, 0, 0),
                Margin = Padding.Empty
            };

            statusLabel = new Label
            {
                AutoSize = false,
                Width = 90,
                Height = 36,
                Text = "Workspace online",
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(117, 244, 193),
                Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold, GraphicsUnit.Point),
                BackColor = Color.FromArgb(20, 33, 53),
                Margin = new Padding(0, 0, 8, 0)
            };

            Guna2Button logoutButton = CreateHeaderButton("Log Out", Color.FromArgb(28, 42, 63), ModernUi.Ink, guna2Button7_Click);
            Guna2Button closeButton = CreateHeaderButton("Close", Color.FromArgb(76, 39, 47), Color.FromArgb(255, 227, 231), guna2Button6_Click);

            actionsPanel.Controls.Add(statusLabel);
            actionsPanel.Controls.Add(logoutButton);
            actionsPanel.Controls.Add(closeButton);

            panelheader.Controls.Add(actionsPanel);
            panelheader.Controls.Add(titleStack);
        }

        private void BuildSidebar()
        {
            sidepanel.Controls.Add(CreateBrandCard());
            sidepanel.Controls.Add(CreateQuickLaunchCard());
            AddSidebarSection("Question Bank", new[]
            {
                MenuItem("Objective Questions", (s, e) => OpenForm(new add_question(), (Guna2Button)s, "Question bank / objective questions")),
                MenuItem("Short Answer Questions", (s, e) => OpenForm(new add_short_answer_questions(), (Guna2Button)s, "Question bank / short answer questions")),
                MenuItem("Theory Questions", (s, e) => OpenForm(new add_theory_questions(), (Guna2Button)s, "Question bank / theory questions")),
                MenuItem("Past Objective Questions", (s, e) => OpenForm(new past_questions_view(), (Guna2Button)s, "Question bank / past objective questions")),
                MenuItem("Past Short Answer", (s, e) => OpenForm(new pas_question_view2(), (Guna2Button)s, "Question bank / past short answer questions")),
                MenuItem("Past Question Settings", (s, e) => OpenForm(new past_questions_settings(), (Guna2Button)s, "Question bank / past question settings"))
            });

            AddSidebarSection("Exam Operations", new[]
            {
                MenuItem("Allocate Exams", (s, e) => OpenForm(new Setexams(), (Guna2Button)s, "Exam operations / allocate exams")),
                MenuItem("Set Exam Duration", (s, e) => OpenForm(new Set_Exam_Duration(), (Guna2Button)s, "Exam operations / exam duration")),
                MenuItem("Enable Theory Exam", (s, e) => OpenTheoryExamAccessDialog((Guna2Button)s)),
                MenuItem("Question Number", (s, e) => OpenForm(new set_exam_question_number(), (Guna2Button)s, "Exam operations / question number")),
                MenuItem("Shuffle Settings", (s, e) => OpenForm(new Exam_Shuffle(), (Guna2Button)s, "Exam operations / shuffle settings")),
                MenuItem("Result Settings", (s, e) => OpenForm(new show_result(), (Guna2Button)s, "Exam operations / result settings")),
                MenuItem("Calculator And Score", (s, e) => OpenForm(new show_calculator_scorecs(), (Guna2Button)s, "Exam operations / calculator and score"))
            });

            AddSidebarSection("People And Records", new[]
            {
                MenuItem("Manage Admins", (s, e) => OpenForm(new add_admin(), (Guna2Button)s, "People and records / admins")),
                MenuItem("Manage Students", (s, e) => OpenForm(new add_student(), (Guna2Button)s, "People and records / students")),
                MenuItem("Manage Courses", (s, e) => OpenForm(new add_courses(), (Guna2Button)s, "People and records / courses")),
                MenuItem("Manage Scores", (s, e) => OpenForm(new view_scores(), (Guna2Button)s, "People and records / scores")),
                MenuItem("Mark Theory Exam", (s, e) => OpenForm(new GradeTheoryAnswers(), (Guna2Button)s, "People and records / grade theory answers")),
                MenuItem("CGPA", (s, e) => OpenForm(new single_GPA(), (Guna2Button)s, "People and records / cgpa")),
                MenuItem("GPA", (s, e) => OpenForm(new multi_GPA(), (Guna2Button)s, "People and records / gpa")),
                MenuItem("Set Course Credit", (s, e) => OpenForm(new set_course_credit(), (Guna2Button)s, "People and records / set course credit"))
            });

            AddSidebarSection("Reports And Finance", new[]
            {
                MenuItem("Master Sheet", null),
                MenuItem("Subscriptions", null),
                MenuItem("GPA Dashboard", null)
            });
        }

        private Control CreateBrandCard()
        {
            Panel card = CreateSidebarCard(200);
            card.Margin = new Padding(0, 0, 0, 18);

            pictureBox11.Dock = DockStyle.Top;
            pictureBox11.Height = 62;
            pictureBox11.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox11.BackColor = Color.Transparent;

            Label eyebrowLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                Text = "Modern admin workspace",
                ForeColor = Color.FromArgb(92, 240, 195),
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point),
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label titleLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 56,
                Text = "Exam operations made calm, fast, and clear.",
                ForeColor = ModernUi.Ink,
                Font = new Font("Segoe UI Semibold", 11.5F, FontStyle.Bold, GraphicsUnit.Point),
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label bodyLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 58,
                Text = "Use the sections below to manage questions, exams, results, and student records without the old cluttered menu.",
                ForeColor = ModernUi.MutedInk,
                Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point),
                TextAlign = ContentAlignment.MiddleLeft
            };

            card.Controls.Add(bodyLabel);
            card.Controls.Add(titleLabel);
            card.Controls.Add(eyebrowLabel);
            card.Controls.Add(pictureBox11);
            return card;
        }

        private Control CreateQuickLaunchCard()
        {
            Panel card = CreateSidebarCard(96);
            card.Margin = new Padding(0, 0, 0, 18);

            Label titleLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                Text = "Quick launch",
                ForeColor = ModernUi.Ink,
                Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point),
                TextAlign = ContentAlignment.MiddleLeft
            };

            FlowLayoutPanel buttonRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 8, 0, 0)
            };

            Guna2Button dashboardButton = CreatePrimaryButton("Dashboard");
            dashboardButton.Width = 96;
            dashboardButton.Click += (s, e) => ShowHomeDashboard(dashboardButton);

            Guna2Button logoutButton = CreateSecondaryButton("Log Out");
            logoutButton.Width = 96;
            logoutButton.Click += guna2Button7_Click;

            buttonRow.Controls.Add(dashboardButton);
            buttonRow.Controls.Add(logoutButton);

            card.Controls.Add(buttonRow);
            card.Controls.Add(titleLabel);
            return card;
        }

        private void AddSidebarSection(string title, IEnumerable<(string Label, EventHandler Handler)> items)
        {
            Label sectionLabel = new Label
            {
                Width = sidepanel.ClientSize.Width - sidepanel.Padding.Horizontal - 12,
                Height = 22,
                Text = title,
                ForeColor = Color.FromArgb(117, 244, 193),
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point),
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 0, 8),
                TextAlign = ContentAlignment.MiddleLeft
            };
            sidepanel.Controls.Add(sectionLabel);

            foreach ((string label, EventHandler handler) item in items)
            {
                Guna2Button button = CreateMenuButton(item.label);
                if (item.handler == null)
                {
                    button.Enabled = false;
                    button.FillColor = Color.FromArgb(18, 27, 42);
                    button.ForeColor = Color.FromArgb(103, 116, 138);
                    button.BorderColor = Color.FromArgb(44, 56, 78);
                    button.BorderThickness = 1;
                    button.Cursor = Cursors.No;
                }
                else
                {
                    button.Click += item.handler;
                }

                sidepanel.Controls.Add(button);
            }
        }

        private Panel CreateSidebarCard(int height)
        {
            Panel card = new Panel
            {
                Width = sidepanel.ClientSize.Width - sidepanel.Padding.Horizontal - 8,
                Height = height > 0 ? height : 120,
                BackColor = Color.FromArgb(17, 27, 44),
                Padding = new Padding(14, 12, 14, 12)
            };

            ModernUi.AddPanelChrome(card);
            return card;
        }

        private Guna2Button CreateMenuButton(string text)
        {
            Guna2Button button = new Guna2Button
            {
                Width = 286,
                Height = 36,
                Text = text,
                FillColor = Color.FromArgb(23, 35, 56),
                ForeColor = ModernUi.Ink,
                BorderRadius = 11,
                Font = new Font("Segoe UI Semibold", 8.75F, FontStyle.Bold, GraphicsUnit.Point),
                TextAlign = HorizontalAlignment.Left,
                Padding = new Padding(16, 0, 0, 0),
                Margin = new Padding(0, 0, 0, 8)
            };

            button.Width = sidepanel.ClientSize.Width - sidepanel.Padding.Horizontal - 10;
            button.HoverState.FillColor = Color.FromArgb(35, 52, 79);
            return button;
        }

        private Guna2Button CreatePrimaryButton(string text)
        {
            Guna2Button button = new Guna2Button
            {
                Height = 36,
                Text = text,
                FillColor = ModernUi.Accent,
                ForeColor = Color.FromArgb(8, 20, 28),
                BorderRadius = 12,
                Font = new Font("Segoe UI Semibold", 8.75F, FontStyle.Bold, GraphicsUnit.Point),
                Margin = new Padding(0, 0, 10, 0)
            };

            button.HoverState.FillColor = Color.FromArgb(92, 240, 195);
            return button;
        }

        private Guna2Button CreateSecondaryButton(string text)
        {
            Guna2Button button = new Guna2Button
            {
                Height = 36,
                Text = text,
                FillColor = Color.FromArgb(28, 42, 63),
                ForeColor = ModernUi.Ink,
                BorderRadius = 12,
                Font = new Font("Segoe UI Semibold", 8.75F, FontStyle.Bold, GraphicsUnit.Point)
            };

            button.HoverState.FillColor = Color.FromArgb(39, 58, 87);
            return button;
        }

        private Guna2Button CreateHeaderButton(string text, Color fillColor, Color foreColor, EventHandler handler)
        {
            Guna2Button button = new Guna2Button
            {
                Width = 70,
                Height = 36,
                Text = text,
                FillColor = fillColor,
                ForeColor = foreColor,
                BorderRadius = 12,
                Font = new Font("Segoe UI Semibold", 8.75F, FontStyle.Bold, GraphicsUnit.Point),
                Margin = new Padding(0, 0, 6, 0)
            };

            button.Click += handler;
            return button;
        }

        private (string Label, EventHandler Handler) MenuItem(string label, EventHandler handler)
        {
            return (label, handler);
        }

        private void OpenTheoryExamAccessDialog(Guna2Button sourceButton)
        {
            if (sourceButton != null)
            {
                SetActiveNavButton(sourceButton);
            }

            sectionCaptionLabel.Text = "Exam operations / theory exam access";
            statusLabel.Text = "Managing Theory Access";

            using (Form dialog = new Form())
            {
                dialog.Text = "Enable Theory Exam";
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;
                dialog.ClientSize = new Size(500, 280);
                dialog.BackColor = Color.FromArgb(11, 18, 31);

                Label title = new Label
                {
                    Text = "Theory Exam Access",
                    ForeColor = ModernUi.Ink,
                    Font = new Font("Segoe UI Semibold", 17F, FontStyle.Bold, GraphicsUnit.Point),
                    Location = new Point(24, 20),
                    Size = new Size(320, 34)
                };

                Label info = new Label
                {
                    Text = "Choose an exam, then decide whether students are allowed to continue to the theory exam after objective.",
                    ForeColor = ModernUi.MutedInk,
                    Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point),
                    Location = new Point(24, 62),
                    Size = new Size(448, 42)
                };

                ComboBox examCombo = new ComboBox
                {
                    Location = new Point(28, 120),
                    Size = new Size(320, 36),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                ModernUi.StyleComboBox(examCombo);

                CheckBox enableTheoryCheck = new CheckBox
                {
                    Text = "Allow students to open the theory exam",
                    ForeColor = ModernUi.Ink,
                    Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point),
                    Location = new Point(28, 176),
                    Size = new Size(360, 28),
                    BackColor = Color.Transparent
                };

                Button saveButton = new Button
                {
                    Text = "Save Setting",
                    Location = new Point(28, 220),
                    Size = new Size(140, 40)
                };
                ModernUi.StylePrimaryButton(saveButton);

                Button closeButton = new Button
                {
                    Text = "Close",
                    Location = new Point(184, 220),
                    Size = new Size(110, 40)
                };
                ModernUi.StyleSecondaryButton(closeButton);

                dialog.Controls.Add(title);
                dialog.Controls.Add(info);
                dialog.Controls.Add(examCombo);
                dialog.Controls.Add(enableTheoryCheck);
                dialog.Controls.Add(saveButton);
                dialog.Controls.Add(closeButton);

                LoadTheoryExamAccessExams(examCombo);

                examCombo.SelectedIndexChanged += (s, e) =>
                {
                    if (examCombo.SelectedValue != null && int.TryParse(examCombo.SelectedValue.ToString(), out int selectedExamId))
                    {
                        enableTheoryCheck.Checked = GetTheoryExamEnabled(selectedExamId);
                    }
                };

                saveButton.Click += (s, e) =>
                {
                    if (examCombo.SelectedValue == null || !int.TryParse(examCombo.SelectedValue.ToString(), out int selectedExamId))
                    {
                        MessageBox.Show("Please select an exam first.", "Select Exam", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    SaveTheoryExamEnabled(selectedExamId, enableTheoryCheck.Checked);
                    MessageBox.Show("Theory exam access updated successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };

                closeButton.Click += (s, e) => dialog.Close();

                if (examCombo.SelectedValue != null && int.TryParse(examCombo.SelectedValue.ToString(), out int initialExamId))
                {
                    enableTheoryCheck.Checked = GetTheoryExamEnabled(initialExamId);
                }

                dialog.ShowDialog(this);
            }
        }

        private void LoadTheoryExamAccessExams(ComboBox comboBox)
        {
            using (SqlConnection connection = connection_class.GetConnection())
            {
                connection.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT ex_id, ex_name FROM tbl_exams ORDER BY ex_name ASC", connection))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    comboBox.DataSource = table;
                    comboBox.DisplayMember = "ex_name";
                    comboBox.ValueMember = "ex_id";
                }
            }
        }

        private bool GetTheoryExamEnabled(int examId)
        {
            using (SqlConnection connection = connection_class.GetConnection())
            {
                connection.Open();
                EnsureTheoryExamEnabledColumn(connection);

                using (SqlCommand command = new SqlCommand("SELECT theory_exam_enabled FROM tbl_exam_settings WHERE ex_id = @examId", connection))
                {
                    command.Parameters.AddWithValue("@examId", examId);
                    object result = command.ExecuteScalar();
                    return result == null || result == DBNull.Value || Convert.ToBoolean(result);
                }
            }
        }

        private void SaveTheoryExamEnabled(int examId, bool enabled)
        {
            using (SqlConnection connection = connection_class.GetConnection())
            {
                connection.Open();
                EnsureTheoryExamEnabledColumn(connection);

                string query = @"
IF EXISTS (SELECT 1 FROM tbl_exam_settings WHERE ex_id = @examId)
    UPDATE tbl_exam_settings SET theory_exam_enabled = @enabled WHERE ex_id = @examId
ELSE
    INSERT INTO tbl_exam_settings (ex_id, theory_exam_enabled) VALUES (@examId, @enabled);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@examId", examId);
                    command.Parameters.AddWithValue("@enabled", enabled);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void EnsureTheoryExamEnabledColumn(SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand(
                "IF OBJECT_ID(N'dbo.tbl_exam_settings', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.tbl_exam_settings', N'theory_exam_enabled') IS NULL ALTER TABLE dbo.tbl_exam_settings ADD theory_exam_enabled BIT NOT NULL CONSTRAINT DF_tbl_exam_settings_theory_exam_enabled_runtime DEFAULT (1);",
                connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void OpenForm(Form form, Guna2Button sourceButton, string sectionCaption)
        {
            if (sourceButton != null)
            {
                SetActiveNavButton(sourceButton);
            }

            sectionCaptionLabel.Text = sectionCaption;
            statusLabel.Text = "Viewing " + form.Text.Trim();
            loadform(form);
        }

        private void ShowHomeDashboard(Guna2Button sourceButton = null)
        {
            if (sourceButton != null)
            {
                SetActiveNavButton(sourceButton);
            }

            sectionCaptionLabel.Text = "Live admin overview / students, courses, subjects, and database status";
            statusLabel.Text = "Viewing Admin Home";

            Panel dashboard = BuildHomeDashboard();
            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.Clear();
            }

            mainpanel.Controls.Add(dashboard);
        }

        private Panel BuildHomeDashboard()
        {
            Panel root = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.Transparent,
                Padding = new Padding(16)
            };

            string databaseName = "Unavailable";
            string serverName = "Unavailable";
            string modeName = connection_class.GetModeLabel(connection_class.CurrentMode);
            string connectionState = "Disconnected";

            int studentCount = 0;
            int courseCount = 0;
            int objectiveQuestionCount = 0;
            int theoryQuestionCount = 0;

            DataTable students = new DataTable();
            DataTable courses = new DataTable();
            DataTable questions = new DataTable();
            string loadError = null;

            try
            {
                SqlConnectionStringBuilder details = connection_class.GetConnectionDetails(connection_class.CurrentMode);
                if (details != null)
                {
                    databaseName = details.InitialCatalog;
                    serverName = details.DataSource;
                }

                using (SqlConnection conn = connection_class.GetConnection())
                {
                    conn.Open();
                    connectionState = "Connected";

                    studentCount = TryGetScalarCount(conn, "SELECT COUNT(*) FROM student_record");
                    courseCount = TryGetScalarCount(conn, "SELECT COUNT(*) FROM tbl_exams");
                    objectiveQuestionCount = TryGetScalarCount(conn, "SELECT COUNT(*) FROM tbl_questions");
                    theoryQuestionCount = TryGetScalarCount(conn, "SELECT COUNT(*) FROM tbl_theory_questions");

                    students = TryGetTable(conn, "SELECT std_id AS [ID], std_name AS [Student Name], std_batch_code AS [Batch], std_password AS [Password] FROM student_record ORDER BY std_name");
                    courses = TryGetTable(conn, "SELECT ex_id AS [ID], ex_name AS [Course / Subject] FROM tbl_exams ORDER BY ex_name");
                    questions = TryGetTable(conn, "SELECT TOP 100 ques_id AS [ID], q_title AS [Question], ex_id_fk AS [Subject ID], q_correctOpn AS [Correct Option] FROM tbl_questions ORDER BY ques_id DESC");
                }
            }
            catch (Exception ex)
            {
                connectionState = "Unavailable";
                loadError = ex.Message;
            }

            FlowLayoutPanel stack = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Width = Math.Max(mainpanel.ClientSize.Width - 8, 720),
                BackColor = Color.Transparent,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            root.Controls.Add(stack);

            Panel heroCard = CreateDashboardHeroCard();
            heroCard.Margin = new Padding(0, 0, 0, 18);
            stack.Controls.Add(heroCard);

            TableLayoutPanel metricsRow = new TableLayoutPanel
            {
                Width = Math.Max(mainpanel.ClientSize.Width - 8, 720),
                Height = 96,
                ColumnCount = 4,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 0, 18),
                Padding = Padding.Empty
            };
            metricsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            metricsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            metricsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            metricsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            metricsRow.Controls.Add(CreateMetricCard("Students", studentCount.ToString(), "Registered student accounts", "ST"), 0, 0);
            metricsRow.Controls.Add(CreateMetricCard("Courses", courseCount.ToString(), "Subjects / exam records", "CR"), 1, 0);
            metricsRow.Controls.Add(CreateMetricCard("Objective Questions", objectiveQuestionCount.ToString(), "Available CBT question bank", "OB"), 2, 0);
            metricsRow.Controls.Add(CreateMetricCard("Theory Questions", theoryQuestionCount.ToString(), "Written-response bank", "TH"), 3, 0);
            stack.Controls.Add(metricsRow);

            TableLayoutPanel mainRow = new TableLayoutPanel
            {
                Width = Math.Max(mainpanel.ClientSize.Width - 8, 720),
                Height = 520,
                ColumnCount = 3,
                RowCount = 2,
                BackColor = Color.Transparent,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            mainRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
            mainRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
            mainRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24F));
            mainRow.RowStyles.Add(new RowStyle(SizeType.Absolute, 220F));
            mainRow.RowStyles.Add(new RowStyle(SizeType.Absolute, 260F));

            Panel studentsCard = CreateDashboardSectionCard();
            Panel coursesCard = CreateDashboardSectionCard();
            Panel questionsCard = CreateDashboardSectionCard();
            Panel infoCard = CreateDashboardSectionCard();

            studentsCard.Margin = new Padding(0, 0, 18, 18);
            coursesCard.Margin = new Padding(0, 0, 18, 18);
            infoCard.Margin = new Padding(0, 0, 0, 18);
            questionsCard.Margin = new Padding(0, 0, 18, 0);

            mainRow.Controls.Add(studentsCard, 0, 0);
            mainRow.Controls.Add(coursesCard, 1, 0);
            mainRow.Controls.Add(infoCard, 2, 0);
            mainRow.SetRowSpan(infoCard, 2);
            mainRow.Controls.Add(questionsCard, 0, 1);
            mainRow.SetColumnSpan(questionsCard, 2);
            stack.Controls.Add(mainRow);

            PopulateGridCard(studentsCard, "All Students", students, "Student records available immediately when Form2 opens.");
            PopulateGridCard(coursesCard, "All Subjects / Courses", courses, "Exam subjects and course entries from the database.");
            PopulateGridCard(questionsCard, "Latest Objective Questions", questions, "Recent question bank entries for fast admin review.");
            PopulateInfoCard(infoCard, modeName, serverName, databaseName, connectionState, studentCount, courseCount, loadError);

            return root;
        }

        private Panel CreateDashboardHeroCard()
        {
            Panel card = CreateDashboardSectionCard();
            card.Height = 138;
            int innerWidth = Math.Max(420, mainpanel.ClientSize.Width - 120);

            Label eyebrow = ModernUi.CreateLabel(
                "Live overview",
                new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point),
                ModernUi.Accent,
                new Point(26, 20),
                new Size(220, 24),
                ContentAlignment.MiddleLeft);
            eyebrow.Parent = card;

            Label title = ModernUi.CreateLabel(
                "Everything important as soon as the admin workspace opens.",
                new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point),
                ModernUi.Ink,
                new Point(24, 48),
                new Size(Math.Max(260, innerWidth - 150), 38),
                ContentAlignment.MiddleLeft);
            title.Parent = card;

            Label subtitle = ModernUi.CreateLabel(
                "Students, courses, subjects, question volume, and the active database connection are shown here live.",
                new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point),
                ModernUi.MutedInk,
                new Point(26, 90),
                new Size(innerWidth, 24),
                ContentAlignment.MiddleLeft);
            subtitle.Parent = card;

            Panel accentPill = new Panel
            {
                Parent = card,
                Size = new Size(180, 42),
                Location = new Point(card.Width - 220, 26),
                BackColor = Color.FromArgb(18, 38, 44),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            ModernUi.AddPanelChrome(accentPill);

            Label accentLabel = ModernUi.CreateLabel(
                "Live startup view",
                new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point),
                Color.FromArgb(117, 244, 193),
                new Point(0, 0),
                new Size(180, 42),
                ContentAlignment.MiddleCenter);
            accentLabel.Parent = accentPill;

            return card;
        }

        private Panel CreateMetricCard(string heading, string value, string footnote, string badgeText)
        {
            Panel card = CreateDashboardSectionCard();
            card.Dock = DockStyle.Fill;
            card.Margin = new Padding(0, 0, 18, 0);
            card.BackColor = Color.FromArgb(23, 35, 56);

            Panel accentStrip = new Panel
            {
                Parent = card,
                Dock = DockStyle.Left,
                Width = 5,
                BackColor = ModernUi.Accent
            };

            Panel badge = new Panel
            {
                Parent = card,
                Size = new Size(46, 46),
                Location = new Point(card.Width - 66, 16),
                BackColor = Color.FromArgb(18, 42, 50),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            ModernUi.AddPanelChrome(badge);

            Label badgeLabel = ModernUi.CreateLabel(
                badgeText,
                new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point),
                Color.FromArgb(117, 244, 193),
                new Point(0, 0),
                new Size(46, 46),
                ContentAlignment.MiddleCenter);
            badgeLabel.Parent = badge;

            Label headingLabel = ModernUi.CreateLabel(
                heading,
                new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point),
                ModernUi.MutedInk,
                new Point(22, 16),
                new Size(220, 20),
                ContentAlignment.MiddleLeft);
            headingLabel.Parent = card;

            Label valueLabel = ModernUi.CreateLabel(
                value,
                new Font("Segoe UI Semibold", 30F, FontStyle.Bold, GraphicsUnit.Point),
                ModernUi.Ink,
                new Point(20, 40),
                new Size(160, 42),
                ContentAlignment.MiddleLeft);
            valueLabel.Parent = card;

            Label footLabel = ModernUi.CreateLabel(
                footnote,
                new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
                ModernUi.MutedInk,
                new Point(22, 88),
                new Size(card.Width - 90, 20),
                ContentAlignment.MiddleLeft);
            footLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            footLabel.Parent = card;

            return card;
        }

        private Panel CreateDashboardSectionCard()
        {
            Panel card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(20, 31, 50),
                Padding = new Padding(18, 16, 18, 16)
            };

            ModernUi.AddPanelChrome(card);
            return card;
        }

        private void PopulateGridCard(Panel card, string title, DataTable data, string subtitle)
        {
            card.Controls.Clear();

            Label titleLabel = ModernUi.CreateLabel(
                title,
                new Font("Segoe UI Semibold", 15F, FontStyle.Bold, GraphicsUnit.Point),
                ModernUi.Ink,
                new Point(18, 16),
                new Size(card.Width - 36, 26),
                ContentAlignment.MiddleLeft);
            titleLabel.Parent = card;

            Label subtitleLabel = ModernUi.CreateLabel(
                subtitle,
                new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point),
                ModernUi.MutedInk,
                new Point(18, 46),
                new Size(card.Width - 36, 34),
                ContentAlignment.TopLeft);
            subtitleLabel.Parent = card;

            Panel gridShell = new Panel
            {
                Parent = card,
                Location = new Point(18, 84),
                Size = new Size(card.Width - 36, card.Height - 102),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.FromArgb(11, 18, 31),
                Padding = new Padding(10)
            };
            ModernUi.AddPanelChrome(gridShell);

            DataGridView grid = new DataGridView
            {
                Parent = gridShell,
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                DataSource = data
            };

            ModernUi.StyleDataGridView(grid);
            grid.ClearSelection();
        }

        private void PopulateInfoCard(Panel card, string modeName, string serverName, string databaseName, string connectionState, int studentCount, int courseCount, string loadError)
        {
            card.Controls.Clear();

            Label titleLabel = ModernUi.CreateLabel(
                "Database Information",
                new Font("Segoe UI Semibold", 15F, FontStyle.Bold, GraphicsUnit.Point),
                ModernUi.Ink,
                new Point(18, 18),
                new Size(card.Width - 36, 28),
                ContentAlignment.MiddleLeft);
            titleLabel.Parent = card;

            Label bodyLabel = ModernUi.CreateLabel(
                "This panel confirms which database the app is using right now and whether live data was loaded successfully.",
                new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point),
                ModernUi.MutedInk,
                new Point(18, 52),
                new Size(card.Width - 36, 60),
                ContentAlignment.TopLeft);
            bodyLabel.Parent = card;

            int top = 138;
            top = AddInfoRow(card, "Mode", modeName, top);
            top = AddInfoRow(card, "Server", serverName, top);
            top = AddInfoRow(card, "Database", databaseName, top);
            top = AddInfoRow(card, "Status", connectionState, top);
            top = AddInfoRow(card, "Student Rows", studentCount.ToString(), top);
            top = AddInfoRow(card, "Course Rows", courseCount.ToString(), top);

            Label helpTitle = ModernUi.CreateLabel(
                "Startup summary",
                new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point),
                ModernUi.Ink,
                new Point(18, top + 18),
                new Size(card.Width - 36, 22),
                ContentAlignment.MiddleLeft);
            helpTitle.Parent = card;

            string summaryText = string.IsNullOrWhiteSpace(loadError)
                ? "Form2 is loading live records directly from your current connection. If the counts or tables change, this screen will reflect the database immediately the next time it opens."
                : "Live records could not be loaded. " + loadError;

            Label summaryLabel = ModernUi.CreateLabel(
                summaryText,
                new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point),
                string.IsNullOrWhiteSpace(loadError) ? ModernUi.MutedInk : Color.FromArgb(255, 205, 205),
                new Point(18, top + 46),
                new Size(card.Width - 36, 220),
                ContentAlignment.TopLeft);
            summaryLabel.Parent = card;
        }

        private int AddInfoRow(Panel card, string label, string value, int top)
        {
            Label keyLabel = ModernUi.CreateLabel(
                label,
                new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point),
                ModernUi.MutedInk,
                new Point(18, top),
                new Size(card.Width - 36, 18),
                ContentAlignment.MiddleLeft);
            keyLabel.Parent = card;

            Label valueLabel = ModernUi.CreateLabel(
                value,
                new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point),
                ModernUi.Ink,
                new Point(18, top + 20),
                new Size(card.Width - 36, 36),
                ContentAlignment.TopLeft);
            valueLabel.Parent = card;

            return top + 64;
        }

        private int GetScalarCount(SqlConnection conn, string sql)
        {
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                object result = cmd.ExecuteScalar();
                return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
            }
        }

        private int TryGetScalarCount(SqlConnection conn, string sql)
        {
            try
            {
                return GetScalarCount(conn, sql);
            }
            catch
            {
                return 0;
            }
        }

        private DataTable GetTable(SqlConnection conn, string sql)
        {
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        private DataTable TryGetTable(SqlConnection conn, string sql)
        {
            try
            {
                return GetTable(conn, sql);
            }
            catch
            {
                return new DataTable();
            }
        }

        private void SetActiveNavButton(Guna2Button button)
        {
            if (activeNavButton != null)
            {
                activeNavButton.FillColor = Color.FromArgb(23, 35, 56);
                activeNavButton.ForeColor = ModernUi.Ink;
            }

            activeNavButton = button;
            activeNavButton.FillColor = Color.FromArgb(76, 227, 179);
            activeNavButton.ForeColor = Color.FromArgb(6, 17, 26);
        }

        private bool ConfirmLogout()
        {
            DialogResult result = MessageBox.Show(
                "Sign out of the admin workspace?",
                "Logout Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Admin_Logincs loginForm = new Admin_Logincs();
                loginForm.Show();
                Hide();
                return true;
            }

            return false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            new question_type().Show();
            Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            new Exam_Settings().Show();
            Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            new add_student().Show();
            Hide();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            new Admin_Logincs().Show();
            Hide();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            new add_courses().Show();
            Hide();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            new add_admin().Show();
            Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            new view_scores().Show();
            Hide();
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = connection_class.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT TOP 1 user_type FROM tbl_exam_settings";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        int userType = Convert.ToInt32(result);
                        Form nextForm = userType == 1 ? (Form)new MasterSheetsSelect() :
                            userType == 0 ? new GPA() : null;

                        if (nextForm == null)
                        {
                            MessageBox.Show("The current settings contain an invalid user type.");
                            return;
                        }

                        nextForm.Show();
                        Hide();
                    }
                    else
                    {
                        MessageBox.Show("No exam user type is configured yet. Open exam settings first.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open this workflow: " + ex.Message);
            }
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            new select__pastquestions_settings().Show();
            Hide();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            new mastersheet_mode().Show();
            Hide();
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            ShowHomeDashboard();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
        }

        private void mainpanel_Paint_1(object sender, PaintEventArgs e)
        {
        }

        private void guna2Button25_Click(object sender, EventArgs e)
        {
        }

        private void guna2Button27_Click(object sender, EventArgs e) => loadform(new Setexams());
        private void guna2Button26_Click(object sender, EventArgs e) => loadform(new Set_Exam_Duration());
        private void guna2Button28_Click(object sender, EventArgs e) => loadform(new set_exam_question_number());
        private void guna2Button21_Click(object sender, EventArgs e) => loadform(new Exam_Shuffle());
        private void guna2Button20_Click(object sender, EventArgs e) => loadform(new show_result());
        private void guna2Button2_Click(object sender, EventArgs e) => loadform(new show_calculator_scorecs());
        private void guna2Button11_Click(object sender, EventArgs e) => loadform(new add_admin());
        private void guna2Button3_Click(object sender, EventArgs e) => loadform(new add_student());
        private void guna2Button9_Click(object sender, EventArgs e) => loadform(new add_courses());
        private void guna2Button31_Click(object sender, EventArgs e) => ShowHomeDashboard();
        private void guna2Button29_Click(object sender, EventArgs e) => loadform(new pas_question_view2());
        private void guna2Button30_Click(object sender, EventArgs e) => loadform(new past_questions_view());
        private void guna2Button15_Click(object sender, EventArgs e) => ShowHomeDashboard();
        private void guna2Button24_Click(object sender, EventArgs e) => loadform(new GradeTheoryAnswers());

        private void guna2Button8_Click(object sender, EventArgs e)
        {
        }

        private void guna2Button14_Click(object sender, EventArgs e) => loadform(new make_subscription());
        private void guna2Button22_Click(object sender, EventArgs e) => loadform(new set_course_credit());
        private void guna2Button10_Click(object sender, EventArgs e) => loadform(new mastersheet_mode());
        private void guna2Button23_Click(object sender, EventArgs e) => loadform(new past_questions_settings());
        private void guna2Button16_Click(object sender, EventArgs e) => loadform(new add_question());
        private void guna2Button18_Click(object sender, EventArgs e) => loadform(new add_short_answer_questions());
        private void guna2Button19_Click(object sender, EventArgs e) => loadform(new add_theory_questions());
        private void guna2Button12_Click(object sender, EventArgs e) => loadform(new view_scores());
        private void guna2Button7_Click(object sender, EventArgs e) => ConfirmLogout();
        private void guna2Button17_Click(object sender, EventArgs e) => ConfirmLogout();

        private void guna2Button32_Click(object sender, EventArgs e)
        {
        }
    }
}
