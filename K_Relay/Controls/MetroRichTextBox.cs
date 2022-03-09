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
        private readonly List<Appending> _appendings;

        public void AppendText(string text, Color color, bool bold) //Empty for use theme color
        {
            if (_appendings.Count(_ => _.Text == text && _.Color == color && _.Bold == bold) == 0)
                _appendings.Add(Appending.Create(text, color, bold));

            _baseTextBox.SelectionStart = _baseTextBox.TextLength;
            _baseTextBox.SelectionLength = 0;
            if (bold)
                _baseTextBox.SelectionFont = new Font(_baseTextBox.Font, FontStyle.Bold);
            else
                _baseTextBox.SelectionFont = new Font(_baseTextBox.Font, FontStyle.Regular);
            _baseTextBox.SelectionColor = color == Color.Empty ? MetroPaint.ForeColor.Label.Normal(Theme) : color;
            _baseTextBox.AppendText(text);
            _baseTextBox.SelectionColor = _baseTextBox.ForeColor;
        }

        public void ReAppendText()
        {
            foreach (var app in _appendings)
                AppendText(app.Text, app.Color, app.Bold);
        }

        public RichTextBox ToWinFormRtb()
        {
            return _baseTextBox;
        }

        #region PromptedTextBox

        private class PromptedTextBox : RichTextBox
        {
            private const int OcmCommand = 0x2111;
            private const int WmPaint = 15;

            private bool _drawPrompt;

            private string _promptText = "";

            public PromptedTextBox()
            {
                _drawPrompt = Text.Trim().Length == 0;
            }

            [Browsable(true)]
            [EditorBrowsable(EditorBrowsableState.Always)]
            [DefaultValue("")]
            public string PromptText
            {
                get => _promptText;
                set
                {
                    _promptText = value.Trim();
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

                TextRenderer.DrawText(g, _promptText, Font, clientRectangle, SystemColors.GrayText, BackColor, flags);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                if (_drawPrompt) DrawTextPrompt(e.Graphics);
            }

            protected override void OnTextChanged(EventArgs e)
            {
                base.OnTextChanged(e);
                _drawPrompt = Text.Trim().Length == 0;
            }

            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);
                if ((m.Msg == WmPaint || m.Msg == OcmCommand) && _drawPrompt && !GetStyle(ControlStyles.UserPaint))
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

        private MetroColorStyle _metroStyle = MetroColorStyle.Default;

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        [DefaultValue(MetroColorStyle.Default)]
        public MetroColorStyle Style
        {
            get
            {
                if (DesignMode || _metroStyle != MetroColorStyle.Default) return _metroStyle;

                if (StyleManager != null && _metroStyle == MetroColorStyle.Default) return StyleManager.Style;
                if (StyleManager == null && _metroStyle == MetroColorStyle.Default) return MetroDefaults.Style;

                return _metroStyle;
            }
            set => _metroStyle = value;
        }

        private MetroThemeStyle _metroTheme = MetroThemeStyle.Default;

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        [DefaultValue(MetroThemeStyle.Default)]
        public MetroThemeStyle Theme
        {
            get
            {
                if (DesignMode || _metroTheme != MetroThemeStyle.Default) return _metroTheme;

                if (StyleManager != null && _metroTheme == MetroThemeStyle.Default) return StyleManager.Theme;
                if (StyleManager == null && _metroTheme == MetroThemeStyle.Default) return MetroDefaults.Theme;

                return _metroTheme;
            }
            set => _metroTheme = value;
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

        private PromptedTextBox _baseTextBox;

        private MetroTextBoxSize _metroTextBoxSize = MetroTextBoxSize.Small;

        [DefaultValue(MetroTextBoxSize.Small)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroTextBoxSize FontSize
        {
            get => _metroTextBoxSize;
            set
            {
                _metroTextBoxSize = value;
                UpdateBaseTextBox();
            }
        }

        private MetroTextBoxWeight _metroTextBoxWeight = MetroTextBoxWeight.Regular;

        [DefaultValue(MetroTextBoxWeight.Regular)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroTextBoxWeight FontWeight
        {
            get => _metroTextBoxWeight;
            set
            {
                _metroTextBoxWeight = value;
                UpdateBaseTextBox();
            }
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue("")]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public string PromptText
        {
            get => _baseTextBox.PromptText;
            set => _baseTextBox.PromptText = value;
        }

        private Image _textBoxIcon;

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(null)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public Image Icon
        {
            get => _textBoxIcon;
            set
            {
                _textBoxIcon = value;
                Refresh();
            }
        }

        private bool _textBoxIconRight;

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool IconRight
        {
            get => _textBoxIconRight;
            set
            {
                _textBoxIconRight = value;
                Refresh();
            }
        }

        private bool _displayIcon = true;

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(true)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool DisplayIcon
        {
            get => _displayIcon;
            set
            {
                _displayIcon = value;
                Refresh();
            }
        }

        protected Size IconSize
        {
            get
            {
                if (_displayIcon && _textBoxIcon != null)
                {
                    var originalSize = _textBoxIcon.Size;
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
            get => _baseTextBox.ContextMenu;
            set
            {
                ContextMenu = value;
                _baseTextBox.ContextMenu = value;
            }
        }

        public override ContextMenuStrip ContextMenuStrip
        {
            get => _baseTextBox.ContextMenuStrip;
            set
            {
                ContextMenuStrip = value;
                _baseTextBox.ContextMenuStrip = value;
            }
        }

        [DefaultValue(false)]
        public bool Multiline
        {
            get => _baseTextBox.Multiline;
            set => _baseTextBox.Multiline = value;
        }

        [DefaultValue(true)]
        public bool WordWrap
        {
            get => _baseTextBox.WordWrap;
            set => _baseTextBox.WordWrap = value;
        }

        public override string Text
        {
            get => _baseTextBox.Text;
            set
            {
                _baseTextBox.Text = value;
                _appendings.Clear();
                _appendings.Add(Appending.Create(_baseTextBox.Text, Color.Empty, false));
            }
        }

        public string[] Lines
        {
            get => _baseTextBox.Lines;
            set => _baseTextBox.Lines = value;
        }

        [Browsable(false)]
        public string SelectedText
        {
            get => _baseTextBox.SelectedText;
            set => _baseTextBox.Text = value;
        }

        [DefaultValue(false)]
        public bool ReadOnly
        {
            get => _baseTextBox.ReadOnly;
            set => _baseTextBox.ReadOnly = value;
        }

        [DefaultValue(HorizontalAlignment.Left)]
        public HorizontalAlignment SelectionAlignment
        {
            get => _baseTextBox.SelectionAlignment;
            set => _baseTextBox.SelectionAlignment = value;
        }

        [DefaultValue(true)]
        public new bool TabStop
        {
            get => _baseTextBox.TabStop;
            set => _baseTextBox.TabStop = value;
        }

        public int MaxLength
        {
            get => _baseTextBox.MaxLength;
            set => _baseTextBox.MaxLength = value;
        }

        public RichTextBoxScrollBars ScrollBars
        {
            get => _baseTextBox.ScrollBars;
            set => _baseTextBox.ScrollBars = value;
        }

        public BorderStyle BorderStyle
        {
            get => _baseTextBox.BorderStyle;
            set => _baseTextBox.BorderStyle = value;
        }

        #endregion

        #region Constructor

        public MetroRichTextBox()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
            _appendings = new List<Appending>();
            base.TabStop = false;
            GotFocus += MetroTextBox_GotFocus;
            CreateBaseTextBox();
            UpdateBaseTextBox();
            AddEventHandler();
        }

        private void MetroTextBox_GotFocus(object sender, EventArgs e)
        {
            _baseTextBox.Focus();
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
            _baseTextBox.Select(start, length);
        }

        public void SelectAll()
        {
            _baseTextBox.SelectAll();
        }

        public void Clear()
        {
            _appendings.Clear();
            _baseTextBox.Clear();
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
                _baseTextBox.BackColor = BackColor;

                if (!UseCustomBackColor)
                {
                    backColor = MetroPaint.BackColor.Button.Normal(Theme);
                    _baseTextBox.BackColor = MetroPaint.BackColor.Button.Normal(Theme);
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
                _baseTextBox.ForeColor = ForeColor;
            }
            else
            {
                _baseTextBox.ForeColor = MetroPaint.ForeColor.Button.Normal(Theme);
                _baseTextBox.Clear();
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
            if (_displayIcon && _textBoxIcon != null)
            {
                var iconLocation = new Point(1, 1);
                if (_textBoxIconRight) iconLocation = new Point(ClientRectangle.Width - IconSize.Width - 1, 1);

                g.DrawImage(_textBoxIcon, new Rectangle(iconLocation, IconSize));

                UpdateBaseTextBox();
            }

            OnCustomPaintForeground(new MetroPaintEventArgs(Color.Empty, _baseTextBox.ForeColor, g));
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
            if (_baseTextBox != null) return;

            _baseTextBox = new PromptedTextBox();

            _baseTextBox.BorderStyle = BorderStyle.None;
            _baseTextBox.Font = MetroFonts.TextBox(_metroTextBoxSize, _metroTextBoxWeight);
            _baseTextBox.Location = new Point(3, 3);
            _baseTextBox.Size = new Size(Width - 6, Height - 6);

            Size = new Size(_baseTextBox.Width + 6, _baseTextBox.Height + 6);

            _baseTextBox.TabStop = true;
            //baseTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;

            Controls.Add(_baseTextBox);
        }

        private void AddEventHandler()
        {
            _baseTextBox.AcceptsTabChanged += BaseTextBoxAcceptsTabChanged;

            _baseTextBox.CausesValidationChanged += BaseTextBoxCausesValidationChanged;
            _baseTextBox.ChangeUICues += BaseTextBoxChangeUiCues;
            _baseTextBox.Click += BaseTextBoxClick;
            _baseTextBox.ClientSizeChanged += BaseTextBoxClientSizeChanged;
            _baseTextBox.ContextMenuChanged += BaseTextBoxContextMenuChanged;
            _baseTextBox.ContextMenuStripChanged += BaseTextBoxContextMenuStripChanged;
            _baseTextBox.CursorChanged += BaseTextBoxCursorChanged;

            _baseTextBox.KeyDown += BaseTextBoxKeyDown;
            _baseTextBox.KeyPress += BaseTextBoxKeyPress;
            _baseTextBox.KeyUp += BaseTextBoxKeyUp;

            _baseTextBox.SizeChanged += BaseTextBoxSizeChanged;

            _baseTextBox.TextChanged += BaseTextBoxTextChanged;
        }

        private void UpdateBaseTextBox()
        {
            if (_baseTextBox == null) return;

            _baseTextBox.Font = MetroFonts.TextBox(_metroTextBoxSize, _metroTextBoxWeight);

            if (_displayIcon)
            {
                var textBoxLocation = new Point(IconSize.Width + 4, 3);
                if (_textBoxIconRight) textBoxLocation = new Point(3, 3);

                _baseTextBox.Location = textBoxLocation;
                _baseTextBox.Size = new Size(Width - 7 - IconSize.Width, Height - 6);
            }
            else
            {
                _baseTextBox.Location = new Point(3, 3);
                _baseTextBox.Size = new Size(Width - 6, Height - 6);
            }
        }

        #endregion
    }
}