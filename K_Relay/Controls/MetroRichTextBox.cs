using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Components;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;

namespace K_Relay.Controls
{
    public class MetroRichTextBox : Control, IMetroControl
    {
        private readonly List<Appending> appendings;

        public void AppendText(string text, Color color, bool bold) //Empty for use theme color
        {
            if (appendings.Count(_ => _.Text == text && _.Color == color && _.Bold == bold) == 0)
                appendings.Add(Appending.Create(text, color, bold));

            baseTextBox.SelectionStart = baseTextBox.TextLength;
            baseTextBox.SelectionLength = 0;
            if (bold)
                baseTextBox.SelectionFont = new Font(baseTextBox.Font, FontStyle.Bold);
            else
                baseTextBox.SelectionFont = new Font(baseTextBox.Font, FontStyle.Regular);
            baseTextBox.SelectionColor = color == Color.Empty ? MetroPaint.ForeColor.Label.Normal(Theme) : color;
            baseTextBox.AppendText(text);
            baseTextBox.SelectionColor = baseTextBox.ForeColor;
        }

        public void ReAppendText()
        {
            foreach (var app in appendings)
                AppendText(app.Text, app.Color, app.Bold);
        }

        public RichTextBox ToWinFormRTB()
        {
            return baseTextBox;
        }

        #region PromptedTextBox

        private class PromptedTextBox : RichTextBox
        {
            private const int OCM_COMMAND = 0x2111;
            private const int WM_PAINT = 15;

            private bool drawPrompt;

            private string promptText = "";

            public PromptedTextBox()
            {
                drawPrompt = Text.Trim().Length == 0;
            }

            [Browsable(true)]
            [EditorBrowsable(EditorBrowsableState.Always)]
            [DefaultValue("")]
            public string PromptText
            {
                get => promptText;
                set
                {
                    promptText = value.Trim();
                    Invalidate();
                }
            }

            private void DrawTextPrompt()
            {
                using (var graphics = CreateGraphics())
                {
                    DrawTextPrompt(graphics);
                }
            }

            private void DrawTextPrompt(Graphics g)
            {
                var flags = TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis;
                var clientRectangle = ClientRectangle;

                switch (SelectionAlignment)
                {
                    case HorizontalAlignment.Left:
                        clientRectangle.Offset(1, 1);
                        break;

                    case HorizontalAlignment.Right:
                        flags |= TextFormatFlags.Right;
                        clientRectangle.Offset(0, 1);
                        break;

                    case HorizontalAlignment.Center:
                        flags |= TextFormatFlags.HorizontalCenter;
                        clientRectangle.Offset(0, 1);
                        break;
                }

                TextRenderer.DrawText(g, promptText, Font, clientRectangle, SystemColors.GrayText, BackColor, flags);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                if (drawPrompt) DrawTextPrompt(e.Graphics);
            }

            protected override void OnTextChanged(EventArgs e)
            {
                base.OnTextChanged(e);
                drawPrompt = Text.Trim().Length == 0;
            }

            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);
                if ((m.Msg == WM_PAINT || m.Msg == OCM_COMMAND) && drawPrompt && !GetStyle(ControlStyles.UserPaint))
                    DrawTextPrompt();
            }
        }

        #endregion

        public class Appending
        {
            public string Text { get; set; }
            public Color Color { get; set; }
            public bool Bold { get; set; }

            internal static Appending Create(string text, Color color, bool bold)
            {
                return new Appending
                {
                    Text = text,
                    Color = color,
                    Bold = bold
                };
            }
        }

        #region Interface

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public event EventHandler<MetroPaintEventArgs> CustomPaintBackground;

        protected virtual void OnCustomPaintBackground(MetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaintBackground != null) CustomPaintBackground(this, e);
        }

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public event EventHandler<MetroPaintEventArgs> CustomPaint;

        protected virtual void OnCustomPaint(MetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaint != null) CustomPaint(this, e);
        }

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public event EventHandler<MetroPaintEventArgs> CustomPaintForeground;

        protected virtual void OnCustomPaintForeground(MetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaintForeground != null) CustomPaintForeground(this, e);
        }

        private MetroColorStyle metroStyle = MetroColorStyle.Default;

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        [DefaultValue(MetroColorStyle.Default)]
        public MetroColorStyle Style
        {
            get
            {
                if (DesignMode || metroStyle != MetroColorStyle.Default) return metroStyle;

                if (StyleManager != null && metroStyle == MetroColorStyle.Default) return StyleManager.Style;
                if (StyleManager == null && metroStyle == MetroColorStyle.Default) return MetroDefaults.Style;

                return metroStyle;
            }
            set => metroStyle = value;
        }

        private MetroThemeStyle metroTheme = MetroThemeStyle.Default;

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        [DefaultValue(MetroThemeStyle.Default)]
        public MetroThemeStyle Theme
        {
            get
            {
                if (DesignMode || metroTheme != MetroThemeStyle.Default) return metroTheme;

                if (StyleManager != null && metroTheme == MetroThemeStyle.Default) return StyleManager.Theme;
                if (StyleManager == null && metroTheme == MetroThemeStyle.Default) return MetroDefaults.Theme;

                return metroTheme;
            }
            set => metroTheme = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MetroStyleManager StyleManager { get; set; } = null;

        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool UseCustomBackColor { get; set; } = false;

        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool UseCustomForeColor { get; set; } = false;

        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool UseStyleColors { get; set; } = false;

        [Browsable(false)]
        [Category(MetroDefaults.PropertyCategory.Behaviour)]
        [DefaultValue(false)]
        public bool UseSelectable
        {
            get => GetStyle(ControlStyles.Selectable);
            set => SetStyle(ControlStyles.Selectable, value);
        }

        #endregion

        #region Fields

        private PromptedTextBox baseTextBox;

        private MetroTextBoxSize metroTextBoxSize = MetroTextBoxSize.Small;

        [DefaultValue(MetroTextBoxSize.Small)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroTextBoxSize FontSize
        {
            get => metroTextBoxSize;
            set
            {
                metroTextBoxSize = value;
                UpdateBaseTextBox();
            }
        }

        private MetroTextBoxWeight metroTextBoxWeight = MetroTextBoxWeight.Regular;

        [DefaultValue(MetroTextBoxWeight.Regular)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroTextBoxWeight FontWeight
        {
            get => metroTextBoxWeight;
            set
            {
                metroTextBoxWeight = value;
                UpdateBaseTextBox();
            }
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue("")]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public string PromptText
        {
            get => baseTextBox.PromptText;
            set => baseTextBox.PromptText = value;
        }

        private Image textBoxIcon;

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(null)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public Image Icon
        {
            get => textBoxIcon;
            set
            {
                textBoxIcon = value;
                Refresh();
            }
        }

        private bool textBoxIconRight;

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool IconRight
        {
            get => textBoxIconRight;
            set
            {
                textBoxIconRight = value;
                Refresh();
            }
        }

        private bool displayIcon = true;

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(true)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool DisplayIcon
        {
            get => displayIcon;
            set
            {
                displayIcon = value;
                Refresh();
            }
        }

        protected Size iconSize
        {
            get
            {
                if (displayIcon && textBoxIcon != null)
                {
                    var originalSize = textBoxIcon.Size;
                    var resizeFactor = (ClientRectangle.Height - 2) / (double)originalSize.Height;

                    var iconLocation = new Point(1, 1);
                    return new Size((int)(originalSize.Width * resizeFactor),
                        (int)(originalSize.Height * resizeFactor));
                }

                return new Size(-1, -1);
            }
        }

        #endregion

        #region Routing Fields

        public override ContextMenu ContextMenu
        {
            get => baseTextBox.ContextMenu;
            set
            {
                ContextMenu = value;
                baseTextBox.ContextMenu = value;
            }
        }

        public override ContextMenuStrip ContextMenuStrip
        {
            get => baseTextBox.ContextMenuStrip;
            set
            {
                ContextMenuStrip = value;
                baseTextBox.ContextMenuStrip = value;
            }
        }

        [DefaultValue(false)]
        public bool Multiline
        {
            get => baseTextBox.Multiline;
            set => baseTextBox.Multiline = value;
        }

        [DefaultValue(true)]
        public bool WordWrap
        {
            get => baseTextBox.WordWrap;
            set => baseTextBox.WordWrap = value;
        }

        public override string Text
        {
            get => baseTextBox.Text;
            set
            {
                baseTextBox.Text = value;
                appendings.Clear();
                appendings.Add(Appending.Create(baseTextBox.Text, Color.Empty, false));
            }
        }

        public string[] Lines
        {
            get => baseTextBox.Lines;
            set => baseTextBox.Lines = value;
        }

        [Browsable(false)]
        public string SelectedText
        {
            get => baseTextBox.SelectedText;
            set => baseTextBox.Text = value;
        }

        [DefaultValue(false)]
        public bool ReadOnly
        {
            get => baseTextBox.ReadOnly;
            set => baseTextBox.ReadOnly = value;
        }

        [DefaultValue(HorizontalAlignment.Left)]
        public HorizontalAlignment SelectionAlignment
        {
            get => baseTextBox.SelectionAlignment;
            set => baseTextBox.SelectionAlignment = value;
        }

        [DefaultValue(true)]
        public new bool TabStop
        {
            get => baseTextBox.TabStop;
            set => baseTextBox.TabStop = value;
        }

        public int MaxLength
        {
            get => baseTextBox.MaxLength;
            set => baseTextBox.MaxLength = value;
        }

        public RichTextBoxScrollBars ScrollBars
        {
            get => baseTextBox.ScrollBars;
            set => baseTextBox.ScrollBars = value;
        }

        public BorderStyle BorderStyle
        {
            get => baseTextBox.BorderStyle;
            set => baseTextBox.BorderStyle = value;
        }

        #endregion

        #region Constructor

        public MetroRichTextBox()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
            appendings = new List<Appending>();
            base.TabStop = false;
            GotFocus += MetroTextBox_GotFocus;
            CreateBaseTextBox();
            UpdateBaseTextBox();
            AddEventHandler();
        }

        private void MetroTextBox_GotFocus(object sender, EventArgs e)
        {
            baseTextBox.Focus();
        }

        #endregion

        #region Routing Methods

        public event EventHandler AcceptsTabChanged;

        private void BaseTextBoxAcceptsTabChanged(object sender, EventArgs e)
        {
            if (AcceptsTabChanged != null)
                AcceptsTabChanged(this, e);
        }

        private void BaseTextBoxSizeChanged(object sender, EventArgs e)
        {
            base.OnSizeChanged(e);
        }

        private void BaseTextBoxCursorChanged(object sender, EventArgs e)
        {
            base.OnCursorChanged(e);
        }

        private void BaseTextBoxContextMenuStripChanged(object sender, EventArgs e)
        {
            base.OnContextMenuStripChanged(e);
        }

        private void BaseTextBoxContextMenuChanged(object sender, EventArgs e)
        {
            base.OnContextMenuChanged(e);
        }

        private void BaseTextBoxClientSizeChanged(object sender, EventArgs e)
        {
            base.OnClientSizeChanged(e);
        }

        private void BaseTextBoxClick(object sender, EventArgs e)
        {
            base.OnClick(e);
        }

        private void BaseTextBoxChangeUiCues(object sender, UICuesEventArgs e)
        {
            base.OnChangeUICues(e);
        }

        private void BaseTextBoxCausesValidationChanged(object sender, EventArgs e)
        {
            base.OnCausesValidationChanged(e);
        }

        private void BaseTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            base.OnKeyUp(e);
        }

        private void BaseTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
        }

        private void BaseTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }

        private void BaseTextBoxTextChanged(object sender, EventArgs e)
        {
            base.OnTextChanged(e);
        }

        public void Select(int start, int length)
        {
            baseTextBox.Select(start, length);
        }

        public void SelectAll()
        {
            baseTextBox.SelectAll();
        }

        public void Clear()
        {
            appendings.Clear();
            baseTextBox.Clear();
        }

        public void AppendText(string text)
        {
            AppendText(text, Color.Empty, false);
        }

        #endregion

        #region Paint Methods

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                var backColor = BackColor;
                baseTextBox.BackColor = BackColor;

                if (!UseCustomBackColor)
                {
                    backColor = MetroPaint.BackColor.Button.Normal(Theme);
                    baseTextBox.BackColor = MetroPaint.BackColor.Button.Normal(Theme);
                }

                if (backColor.A == 255)
                {
                    e.Graphics.Clear(backColor);
                    return;
                }

                base.OnPaintBackground(e);

                OnCustomPaintBackground(new MetroPaintEventArgs(backColor, Color.Empty, e.Graphics));
            }
            catch
            {
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                if (GetStyle(ControlStyles.AllPaintingInWmPaint)) OnPaintBackground(e);

                OnCustomPaint(new MetroPaintEventArgs(Color.Empty, Color.Empty, e.Graphics));
                OnPaintForeground(e);
            }
            catch
            {
                Invalidate();
            }
        }

        protected virtual void OnPaintForeground(PaintEventArgs e)
        {
            if (UseCustomForeColor)
            {
                baseTextBox.ForeColor = ForeColor;
            }
            else
            {
                baseTextBox.ForeColor = MetroPaint.ForeColor.Button.Normal(Theme);
                baseTextBox.Clear();
                ReAppendText();
            }

            var borderColor = MetroPaint.BorderColor.Button.Normal(Theme);

            if (UseStyleColors)
                borderColor = MetroPaint.GetStyleColor(Style);

            using (var p = new Pen(borderColor))
            {
                e.Graphics.DrawRectangle(p, new Rectangle(0, 0, Width - 1, Height - 1));
            }

            DrawIcon(e.Graphics);
        }

        private void DrawIcon(Graphics g)
        {
            if (displayIcon && textBoxIcon != null)
            {
                var iconLocation = new Point(1, 1);
                if (textBoxIconRight) iconLocation = new Point(ClientRectangle.Width - iconSize.Width - 1, 1);

                g.DrawImage(textBoxIcon, new Rectangle(iconLocation, iconSize));

                UpdateBaseTextBox();
            }

            OnCustomPaintForeground(new MetroPaintEventArgs(Color.Empty, baseTextBox.ForeColor, g));
        }

        #endregion

        #region Overridden Methods

        public override void Refresh()
        {
            base.Refresh();
            UpdateBaseTextBox();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateBaseTextBox();
        }

        #endregion

        #region Private Methods

        private void CreateBaseTextBox()
        {
            if (baseTextBox != null) return;

            baseTextBox = new PromptedTextBox();

            baseTextBox.BorderStyle = BorderStyle.None;
            baseTextBox.Font = MetroFonts.TextBox(metroTextBoxSize, metroTextBoxWeight);
            baseTextBox.Location = new Point(3, 3);
            baseTextBox.Size = new Size(Width - 6, Height - 6);

            Size = new Size(baseTextBox.Width + 6, baseTextBox.Height + 6);

            baseTextBox.TabStop = true;
            //baseTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;

            Controls.Add(baseTextBox);
        }

        private void AddEventHandler()
        {
            baseTextBox.AcceptsTabChanged += BaseTextBoxAcceptsTabChanged;

            baseTextBox.CausesValidationChanged += BaseTextBoxCausesValidationChanged;
            baseTextBox.ChangeUICues += BaseTextBoxChangeUiCues;
            baseTextBox.Click += BaseTextBoxClick;
            baseTextBox.ClientSizeChanged += BaseTextBoxClientSizeChanged;
            baseTextBox.ContextMenuChanged += BaseTextBoxContextMenuChanged;
            baseTextBox.ContextMenuStripChanged += BaseTextBoxContextMenuStripChanged;
            baseTextBox.CursorChanged += BaseTextBoxCursorChanged;

            baseTextBox.KeyDown += BaseTextBoxKeyDown;
            baseTextBox.KeyPress += BaseTextBoxKeyPress;
            baseTextBox.KeyUp += BaseTextBoxKeyUp;

            baseTextBox.SizeChanged += BaseTextBoxSizeChanged;

            baseTextBox.TextChanged += BaseTextBoxTextChanged;
        }

        private void UpdateBaseTextBox()
        {
            if (baseTextBox == null) return;

            baseTextBox.Font = MetroFonts.TextBox(metroTextBoxSize, metroTextBoxWeight);

            if (displayIcon)
            {
                var textBoxLocation = new Point(iconSize.Width + 4, 3);
                if (textBoxIconRight) textBoxLocation = new Point(3, 3);

                baseTextBox.Location = textBoxLocation;
                baseTextBox.Size = new Size(Width - 7 - iconSize.Width, Height - 6);
            }
            else
            {
                baseTextBox.Location = new Point(3, 3);
                baseTextBox.Size = new Size(Width - 6, Height - 6);
            }
        }

        #endregion
    }
}