using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Quiz_App
{
    internal static class ModernUi
    {
        private const int BaseWidth = 1920;
        private const int BaseHeight = 1080;
        private static readonly HashSet<Form> ScaledForms = new HashSet<Form>();
        private static readonly HashSet<Form> ThemedForms = new HashSet<Form>();
        private static readonly HashSet<Form> GridWorkspaceForms = new HashSet<Form>();
        private static readonly HashSet<string> GridWorkspaceSkipForms = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "view_scores",
            "GradeTheoryAnswers",
            "Form2",
            "Dashboard",
            "Setexams",
            "add_question",
            "add_theory_questions",
            "add_short_answer_questions",
            "MasterSheetForm",
            "MasterSheetsSelect",
            "ReportCardForm"
        };

        public static readonly Color Ink = Color.FromArgb(236, 241, 248);
        public static readonly Color MutedInk = Color.FromArgb(156, 168, 188);
        public static readonly Color Surface = Color.FromArgb(18, 24, 38);
        public static readonly Color SurfaceRaised = Color.FromArgb(28, 36, 54);
        public static readonly Color SurfaceSoft = Color.FromArgb(34, 44, 66);
        public static readonly Color Border = Color.FromArgb(58, 74, 104);
        public static readonly Color Accent = Color.FromArgb(72, 213, 151);
        public static readonly Color AccentAlt = Color.FromArgb(88, 166, 255);
        public static readonly Color Warning = Color.FromArgb(255, 196, 92);
        public static readonly Color Danger = Color.FromArgb(255, 107, 107);

        public static void ScaleForScreen(Form form)
        {
            if (form == null || ScaledForms.Contains(form))
            {
                return;
            }

            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            float scaleX = (float)screenWidth / BaseWidth;
            float scaleY = (float)screenHeight / BaseHeight;
            float fontScale = Math.Min(scaleX, scaleY);

            form.Scale(new SizeF(scaleX, scaleY));
            ScaleFontsRecursive(form.Controls, fontScale);
            form.StartPosition = FormStartPosition.CenterScreen;
            ScaledForms.Add(form);
        }

        public static void ApplyTheme(Form form)
        {
            if (form == null || ThemedForms.Contains(form))
            {
                return;
            }

            form.BackColor = Surface;
            form.ForeColor = Ink;
            form.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            SetDoubleBuffered(form);
            ApplyThemeRecursive(form.Controls);
            ThemedForms.Add(form);
        }

        public static void FadeIn(Form form, int interval = 12)
        {
            if (form == null || form.IsDisposed)
            {
                return;
            }

            form.Opacity = 0D;
            Timer timer = new Timer { Interval = Math.Max(8, interval) };
            timer.Tick += (sender, e) =>
            {
                if (form.IsDisposed)
                {
                    timer.Stop();
                    timer.Dispose();
                    return;
                }

                form.Opacity = Math.Min(1D, form.Opacity + 0.12D);
                if (form.Opacity >= 1D)
                {
                    timer.Stop();
                    timer.Dispose();
                }
            };
            timer.Start();
        }

        public static void AddGradientBackground(Form form, Color topColor, Color bottomColor)
        {
            if (form == null)
            {
                return;
            }

            form.Paint += (sender, e) =>
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(form.ClientRectangle, topColor, bottomColor, 135f))
                {
                    e.Graphics.FillRectangle(brush, form.ClientRectangle);
                }

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (SolidBrush accentGlow = new SolidBrush(Color.FromArgb(28, Accent)))
                {
                    e.Graphics.FillEllipse(accentGlow, new Rectangle(form.Width - 240, -50, 280, 280));
                }

                using (SolidBrush altGlow = new SolidBrush(Color.FromArgb(24, AccentAlt)))
                {
                    e.Graphics.FillEllipse(altGlow, new Rectangle(-100, form.Height - 260, 320, 320));
                }
            };

            form.Resize += (sender, e) => form.Invalidate();
        }

        public static Panel CreateCard(Rectangle bounds)
        {
            Panel panel = new Panel
            {
                Bounds = bounds,
                BackColor = Color.Transparent
            };

            panel.Paint += (sender, e) =>
            {
                Rectangle drawBounds = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                using (GraphicsPath path = CreateRoundedPath(drawBounds, 26))
                using (SolidBrush fill = new SolidBrush(Color.FromArgb(210, SurfaceRaised)))
                using (Pen borderPen = new Pen(Color.FromArgb(130, Border), 1.2f))
                {
                    e.Graphics.FillPath(fill, path);
                    e.Graphics.DrawPath(borderPen, path);
                }
            };

            SetDoubleBuffered(panel);
            return panel;
        }

        public static Label CreateLabel(string text, Font font, Color color, Point location, Size size, ContentAlignment align)
        {
            return new Label
            {
                Text = text,
                Font = font,
                ForeColor = color,
                BackColor = Color.Transparent,
                Location = location,
                Size = size,
                TextAlign = align
            };
        }

        public static void StylePrimaryButton(Button button)
        {
            if (button == null)
            {
                return;
            }

            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = Accent;
            button.ForeColor = Color.FromArgb(15, 20, 32);
            button.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            button.Cursor = Cursors.Hand;
        }

        public static void StyleSecondaryButton(Button button)
        {
            if (button == null)
            {
                return;
            }

            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = Border;
            button.BackColor = SurfaceSoft;
            button.ForeColor = Ink;
            button.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point);
            button.Cursor = Cursors.Hand;
        }

        public static void StyleDangerButton(Button button)
        {
            if (button == null)
            {
                return;
            }

            StyleSecondaryButton(button);
            button.BackColor = Color.FromArgb(70, 37, 42);
            button.ForeColor = Color.FromArgb(255, 221, 221);
            button.FlatAppearance.BorderColor = Color.FromArgb(132, 74, 84);
        }

        public static void StyleTextInput(TextBox textBox)
        {
            if (textBox == null)
            {
                return;
            }

            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.BackColor = Color.FromArgb(14, 20, 32);
            textBox.ForeColor = Ink;
            textBox.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
        }

        public static void StyleComboBox(ComboBox comboBox)
        {
            if (comboBox == null)
            {
                return;
            }

            comboBox.BackColor = Color.FromArgb(14, 20, 32);
            comboBox.ForeColor = Ink;
            comboBox.FlatStyle = FlatStyle.Flat;
            comboBox.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
        }

        public static void StyleNumericUpDown(NumericUpDown numericUpDown)
        {
            if (numericUpDown == null)
            {
                return;
            }

            numericUpDown.BackColor = Color.FromArgb(14, 20, 32);
            numericUpDown.ForeColor = Ink;
            numericUpDown.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            numericUpDown.BorderStyle = BorderStyle.FixedSingle;
        }

        public static void WireHoverLift(Control control, int raiseBy = 8)
        {
            if (control == null)
            {
                return;
            }

            Point originalLocation = control.Location;
            control.MouseEnter += (sender, e) => control.Location = new Point(originalLocation.X, originalLocation.Y - raiseBy);
            control.MouseLeave += (sender, e) => control.Location = originalLocation;
        }

        public static void AddPanelChrome(Panel panel)
        {
            if (panel == null)
            {
                return;
            }

            panel.BackColor = SurfaceRaised;
            panel.Paint += (sender, e) =>
            {
                Rectangle bounds = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
                using (Pen borderPen = new Pen(Color.FromArgb(120, Border)))
                {
                    e.Graphics.DrawRectangle(borderPen, bounds);
                }
            };
        }

        public static void StyleDataGridView(DataGridView grid)
        {
            if (grid == null)
            {
                return;
            }

            grid.BackgroundColor = Color.FromArgb(16, 22, 35);
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.GridColor = Color.FromArgb(52, 68, 97);
            grid.EnableHeadersVisualStyles = false;
            grid.RowHeadersVisible = false;
            grid.AllowUserToResizeRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = true;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            grid.RowTemplate.Height = Math.Max(grid.RowTemplate.Height, 38);

            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grid.ColumnHeadersHeight = Math.Max(grid.ColumnHeadersHeight, 42);
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 41, 64);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Ink;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(30, 41, 64);
            grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Ink;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 8, 0);

            grid.DefaultCellStyle.BackColor = Color.FromArgb(22, 30, 46);
            grid.DefaultCellStyle.ForeColor = Ink;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(70, 112, 186);
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            grid.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            grid.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(26, 35, 54);
            grid.AlternatingRowsDefaultCellStyle.ForeColor = Ink;
            grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(70, 112, 186);
            grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;

            grid.RowsDefaultCellStyle.BackColor = Color.FromArgb(22, 30, 46);
            grid.RowsDefaultCellStyle.ForeColor = Ink;
            grid.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(70, 112, 186);
            grid.RowsDefaultCellStyle.SelectionForeColor = Color.White;

            foreach (DataGridViewColumn column in grid.Columns)
            {
                if (column is DataGridViewImageColumn)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                else if (column.AutoSizeMode == DataGridViewAutoSizeColumnMode.NotSet)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                column.SortMode = column.SortMode == DataGridViewColumnSortMode.NotSortable
                    ? DataGridViewColumnSortMode.NotSortable
                    : DataGridViewColumnSortMode.Automatic;
            }
        }

        public static void PrepareDataGridView(DataGridView grid)
        {
            if (grid == null)
            {
                return;
            }

            StyleDataGridView(grid);
            grid.DataBindingComplete -= HandleGridDataBindingComplete;
            grid.DataBindingComplete += HandleGridDataBindingComplete;
        }

        public static void ApplyGridWorkspace(Form form)
        {
            if (form == null || GridWorkspaceForms.Contains(form) || GridWorkspaceSkipForms.Contains(form.Name))
            {
                return;
            }

            DataGridView[] grids = GetDescendants(form).OfType<DataGridView>()
                .Where(grid => grid.Visible && grid.Parent == form)
                .ToArray();

            if (grids.Length == 0)
            {
                return;
            }

            foreach (DataGridView grid in grids)
            {
                PrepareDataGridView(grid);
            }

            List<Control> topControls = form.Controls
                .Cast<Control>()
                .Where(control => control.Visible && !(control is DataGridView) && !(control is PictureBox))
                .OrderBy(control => control.Top)
                .ToList();

            int contentTop = topControls.Count == 0 ? 110 : topControls.Max(control => control.Bottom) + 26;
            int margin = 36;
            int gap = 24;
            int availableWidth = Math.Max(420, form.ClientSize.Width - (margin * 2));
            int availableHeight = Math.Max(260, form.ClientSize.Height - contentTop - margin);

            if (grids.Length == 1)
            {
                grids[0].Bounds = new Rectangle(margin, contentTop, availableWidth, availableHeight);
                grids[0].Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                EnsureGridCard(form, grids[0], 0);
            }
            else
            {
                int perGridHeight = Math.Max(180, (availableHeight - gap) / grids.Length);

                for (int i = 0; i < grids.Length; i++)
                {
                    int top = contentTop + (i * (perGridHeight + gap));
                    int height = i == grids.Length - 1
                        ? Math.Max(180, form.ClientSize.Height - top - margin)
                        : perGridHeight;

                    grids[i].Bounds = new Rectangle(margin, top, availableWidth, height);
                    grids[i].Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                    EnsureGridCard(form, grids[i], i);
                }
            }

            GridWorkspaceForms.Add(form);
        }

        private static void ApplyThemeRecursive(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                SetDoubleBuffered(control);

                if (control is Panel panel && panel.BackColor == SystemColors.Control)
                {
                    panel.BackColor = SurfaceRaised;
                }
                else if (control is Button button)
                {
                    StylePrimaryButton(button);
                }
                else if (control is TextBox textBox)
                {
                    StyleTextInput(textBox);
                }
                else if (control is GroupBox groupBox)
                {
                    groupBox.ForeColor = Ink;
                    groupBox.BackColor = Color.Transparent;
                    groupBox.Font = new Font("Segoe UI Semibold", groupBox.Font.Size, FontStyle.Bold, GraphicsUnit.Point);
                }
                else if (control is Label label && label.ForeColor == Color.Black)
                {
                    label.ForeColor = Ink;
                }
                else if (control is ComboBox comboBox)
                {
                    StyleComboBox(comboBox);
                }
                else if (control is DataGridView grid)
                {
                    PrepareDataGridView(grid);
                }
                else if (control is Guna2Button gunaButton)
                {
                    gunaButton.FillColor = Accent;
                    gunaButton.ForeColor = Color.FromArgb(15, 20, 32);
                    gunaButton.BorderRadius = Math.Max(gunaButton.BorderRadius, 12);
                }
                else if (control is Guna2Panel gunaPanel)
                {
                    gunaPanel.FillColor = SurfaceRaised;
                    gunaPanel.BorderColor = Border;
                    gunaPanel.BorderRadius = Math.Max(gunaPanel.BorderRadius, 18);
                }
                else if (control is Guna2TextBox gunaTextBox)
                {
                    gunaTextBox.FillColor = Color.FromArgb(14, 20, 32);
                    gunaTextBox.ForeColor = Ink;
                    gunaTextBox.BorderColor = Border;
                    gunaTextBox.BorderRadius = Math.Max(gunaTextBox.BorderRadius, 10);
                }

                if (control.HasChildren)
                {
                    ApplyThemeRecursive(control.Controls);
                }
            }
        }

        private static void ScaleFontsRecursive(Control.ControlCollection controls, float scale)
        {
            foreach (Control control in controls)
            {
                control.Font = new Font(control.Font.FontFamily, Math.Max(8.5F, control.Font.Size * scale), control.Font.Style);

                if (control.HasChildren)
                {
                    ScaleFontsRecursive(control.Controls, scale);
                }
            }
        }

        private static GraphicsPath CreateRoundedPath(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Rectangle arc = new Rectangle(bounds.Location, new Size(diameter, diameter));
            GraphicsPath path = new GraphicsPath();

            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();

            return path;
        }

        private static void SetDoubleBuffered(Control control)
        {
            PropertyInfo property = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);
            property?.SetValue(control, true, null);
        }

        private static void HandleGridDataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (sender is DataGridView grid)
            {
                StyleDataGridView(grid);
            }
        }

        private static IEnumerable<Control> GetDescendants(Control root)
        {
            foreach (Control control in root.Controls)
            {
                yield return control;

                foreach (Control child in GetDescendants(control))
                {
                    yield return child;
                }
            }
        }

        private static void EnsureGridCard(Form form, DataGridView grid, int index)
        {
            string cardName = "__gridCard_" + index;
            Panel card = form.Controls.Find(cardName, false).OfType<Panel>().FirstOrDefault();

            Rectangle bounds = new Rectangle(
                Math.Max(8, grid.Left - 12),
                Math.Max(8, grid.Top - 12),
                Math.Min(form.ClientSize.Width - 16, grid.Width + 24),
                Math.Min(form.ClientSize.Height - 16, grid.Height + 24));

            if (card == null)
            {
                card = CreateCard(bounds);
                card.Name = cardName;
                form.Controls.Add(card);
                card.SendToBack();
            }
            else
            {
                card.Bounds = bounds;
                card.Invalidate();
            }

            grid.BringToFront();
        }
    }
}
