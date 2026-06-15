using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Quiz_App
{
    public class BaseForm : Form
    {
        private sealed class LayoutSnapshot
        {
            public Rectangle Bounds { get; set; }
            public float FontSize { get; set; }
        }

        private bool _initialized;
        private bool _responsiveBoundsApplied;
        private bool _layoutSnapshotsCaptured;
        private bool _applyingResponsiveLayout;
        private Size _originalClientSize;
        private readonly Dictionary<Control, LayoutSnapshot> _layoutSnapshots = new Dictionary<Control, LayoutSnapshot>();

        protected virtual bool UseAutomaticResponsiveLayout => true;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (_initialized)
            {
                return;
            }

            _initialized = true;
            ModernUi.ScaleForScreen(this);
            ApplyBaseStyling();

            if (TopLevel)
            {
                Shown += BaseForm_Shown;
                Resize += BaseForm_Resize;
            }

            if (UseAutomaticResponsiveLayout)
            {
                BeginInvoke(new Action(CaptureResponsiveLayout));
            }
        }

        protected virtual void ApplyBaseStyling()
        {
            ModernUi.ApplyTheme(this);
            ModernUi.ApplyGridWorkspace(this);

            if (TopLevel && FormBorderStyle == FormBorderStyle.None)
            {
                ModernUi.AddGradientBackground(this, Color.FromArgb(9, 15, 29), Color.FromArgb(20, 32, 52));
                ModernUi.FadeIn(this);
            }
        }

        private void BaseForm_Shown(object sender, EventArgs e)
        {
            ApplyResponsiveBounds();

            if (UseAutomaticResponsiveLayout)
            {
                CaptureResponsiveLayout();
                ApplyAutomaticResponsiveLayout();
            }
        }

        private void BaseForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                return;
            }

            if (TopLevel && WindowState == FormWindowState.Normal)
            {
                ApplyResponsiveBounds();
            }

            if (UseAutomaticResponsiveLayout)
            {
                ApplyAutomaticResponsiveLayout();
            }
        }

        protected void ApplyResponsiveBounds(int horizontalMargin = 80, int verticalMargin = 70)
        {
            if (!TopLevel || WindowState != FormWindowState.Normal)
            {
                return;
            }

            Rectangle workingArea = Screen.FromControl(this).WorkingArea;
            int maxWidth = Math.Max(720, workingArea.Width - horizontalMargin);
            int maxHeight = Math.Max(520, workingArea.Height - verticalMargin);

            bool shouldResize =
                !_responsiveBoundsApplied ||
                Width > maxWidth ||
                Height > maxHeight ||
                Right > workingArea.Right ||
                Bottom > workingArea.Bottom ||
                Left < workingArea.Left ||
                Top < workingArea.Top;

            if (shouldResize)
            {
                Width = Math.Min(Width, maxWidth);
                Height = Math.Min(Height, maxHeight);
            }

            Location = new Point(
                workingArea.Left + Math.Max(0, (workingArea.Width - Width) / 2),
                workingArea.Top + Math.Max(0, (workingArea.Height - Height) / 2));

            _responsiveBoundsApplied = true;
        }

        private void CaptureResponsiveLayout()
        {
            if (_layoutSnapshotsCaptured || ClientSize.Width <= 0 || ClientSize.Height <= 0)
            {
                return;
            }

            _layoutSnapshots.Clear();
            _originalClientSize = ClientSize;
            CaptureControlSnapshots(this);
            _layoutSnapshotsCaptured = true;
        }

        private void CaptureControlSnapshots(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (!_layoutSnapshots.ContainsKey(control))
                {
                    _layoutSnapshots[control] = new LayoutSnapshot
                    {
                        Bounds = control.Bounds,
                        FontSize = control.Font?.Size ?? Font.Size
                    };
                }

                if (control.Controls.Count > 0)
                {
                    CaptureControlSnapshots(control);
                }
            }
        }

        private void ApplyAutomaticResponsiveLayout()
        {
            if (_applyingResponsiveLayout || !_layoutSnapshotsCaptured || _originalClientSize.Width <= 0 || _originalClientSize.Height <= 0)
            {
                return;
            }

            _applyingResponsiveLayout = true;

            try
            {
                float scaleX = Math.Max(0.65f, (float)ClientSize.Width / _originalClientSize.Width);
                float scaleY = Math.Max(0.65f, (float)ClientSize.Height / _originalClientSize.Height);
                float fontScale = Math.Min(scaleX, scaleY);

                SuspendLayout();
                ApplyControlSnapshots(this, scaleX, scaleY, fontScale);
                ResumeLayout(true);
            }
            finally
            {
                _applyingResponsiveLayout = false;
            }
        }

        private void ApplyControlSnapshots(Control parent, float scaleX, float scaleY, float fontScale)
        {
            foreach (Control control in parent.Controls)
            {
                if (_layoutSnapshots.TryGetValue(control, out LayoutSnapshot snapshot))
                {
                    if (control.Dock == DockStyle.None)
                    {
                        control.Bounds = new Rectangle(
                            (int)Math.Round(snapshot.Bounds.X * scaleX),
                            (int)Math.Round(snapshot.Bounds.Y * scaleY),
                            Math.Max(24, (int)Math.Round(snapshot.Bounds.Width * scaleX)),
                            Math.Max(18, (int)Math.Round(snapshot.Bounds.Height * scaleY)));
                    }

                    if (control.Font != null)
                    {
                        float newSize = Math.Max(7.5f, snapshot.FontSize * fontScale);
                        if (Math.Abs(control.Font.Size - newSize) > 0.1f)
                        {
                            control.Font = new Font(control.Font.FontFamily, newSize, control.Font.Style, GraphicsUnit.Point);
                        }
                    }
                }

                if (control.Controls.Count > 0)
                {
                    ApplyControlSnapshots(control, scaleX, scaleY, fontScale);
                }
            }
        }
    }
}
