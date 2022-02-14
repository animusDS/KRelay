using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Components;
using MetroFramework.Controls;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;

namespace K_Relay.Controls
{
    public class MetroListBox : Control, IMetroControl
    {
        private MetroScrollBar scrollBar;

        public MetroListBox()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
            TabStop = false;

            InitializeBaseListBox();
            UpdateBaseListBox();
            AddEventHandler();
        }

        public ListBoxBase ListBox { get; private set; }

        public BorderStyle BorderStyle
        {
            get => ListBox.BorderStyle;
            set => ListBox.BorderStyle = value;
        }

        public bool FormattingEnabled
        {
            get => ListBox.FormattingEnabled;
            set => ListBox.FormattingEnabled = value;
        }

        public bool IntegralHeight
        {
            get => ListBox.IntegralHeight;
            set => ListBox.IntegralHeight = value;
        }

        public int ItemHeight
        {
            get => ListBox.ItemHeight;
            set => ListBox.ItemHeight = value;
        }

        public event EventHandler SelectedIndexChanged;

        private void InitializeBaseListBox()
        {
            SuspendLayout();
            if (ListBox != null) return;

            scrollBar = new MetroScrollBar();
            ListBox = new ListBoxBase();

            ListBox.BorderStyle = BorderStyle.None;
            ListBox.Location = new Point(3, 3);
            ListBox.Size = new Size(Width - 16, Height - 6);
            scrollBar.Scroll += scrollBar_Scroll;

            Size = new Size(ListBox.Width + 16, ListBox.Height + 6);

            ListBox.TabStop = true;

            scrollBar.Maximum = ItemHeight * ListBox.Items.Count;
            scrollBar.Minimum = 0;

            scrollBar.LargeChange = scrollBar.Maximum / scrollBar.Height + Height;
            scrollBar.SmallChange = 15;

            Controls.Add(scrollBar);
            Controls.Add(ListBox);
        }

        private void AddEventHandler()
        {
            ListBox.CausesValidationChanged += BaseListBoxCausesValidationChanged;
            ListBox.ChangeUICues += BaseListBoxChangeUiCues;
            ListBox.Click += BaseListBoxClick;
            ListBox.ClientSizeChanged += BaseListBoxClientSizeChanged;
            ListBox.ContextMenuChanged += BaseListBoxContextMenuChanged;
            ListBox.ContextMenuStripChanged += BaseListBoxContextMenuStripChanged;
            ListBox.CursorChanged += BaseListBoxCursorChanged;

            ListBox.KeyDown += BaseListBoxKeyDown;
            ListBox.KeyPress += BaseListBoxKeyPress;
            ListBox.KeyUp += BaseListBoxKeyUp;

            ListBox.SizeChanged += BaseListBoxSizeChanged;

            ListBox.TextChanged += BaseListBoxTextChanged;
            ListBox.SelectedIndexChanged += BaseListBoxSelectedIndexChanged;
        }

        private void BaseListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedIndexChanged != null)
                SelectedIndexChanged(sender, e);
        }

        private void scrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            ListBox.TopIndex = Math.Max(scrollBar.Value / (ItemHeight + 1), 0);
            scrollBar.Refresh();
            Application.DoEvents();
        }

        public class ListBoxBase : ListBox
        {
            protected override CreateParams CreateParams
            {
                get
                {
                    var cp = base.CreateParams;
                    cp.Style = cp.Style & ~0x200000;
                    return cp;
                }
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

        #region Paint Methods

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                var backColor = BackColor;
                ListBox.BackColor = BackColor;

                if (!UseCustomBackColor)
                {
                    backColor = MetroPaint.BackColor.Button.Normal(Theme);
                    ListBox.BackColor = MetroPaint.BackColor.Button.Normal(Theme);
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
                ListBox.ForeColor = ForeColor;
            else
                ListBox.ForeColor = MetroPaint.ForeColor.Button.Normal(Theme);

            var borderColor = MetroPaint.BorderColor.Button.Normal(Theme);

            if (UseStyleColors)
                borderColor = MetroPaint.GetStyleColor(Style);

            using (var p = new Pen(borderColor))
            {
                e.Graphics.DrawRectangle(p, new Rectangle(0, 0, Width - 1, Height - 1));
            }
        }

        #endregion

        #region Overridden Methods

        public override void Refresh()
        {
            base.Refresh();
            UpdateBaseListBox();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateBaseListBox();
        }

        private void UpdateBaseListBox()
        {
            if (ListBox == null) return;
            ListBox.Location = new Point(3, 3);
            ListBox.Size = new Size(Width - 16, Height - 6);
            scrollBar.Height = ListBox.Height;
            scrollBar.Location = new Point(ListBox.Width + 2, 3);

            if (scrollBar.Height == 0) return;

            scrollBar.Maximum = ItemHeight * ListBox.Items.Count;
            scrollBar.Minimum = 0;

            scrollBar.LargeChange = scrollBar.Maximum / scrollBar.Height + Height;
            scrollBar.SmallChange = 15;
        }

        #endregion


        #region Routing Methods

        public event EventHandler AcceptsTabChanged;

        private void BaseListBoxAcceptsTabChanged(object sender, EventArgs e)
        {
            if (AcceptsTabChanged != null)
                AcceptsTabChanged(this, e);
        }

        private void BaseListBoxSizeChanged(object sender, EventArgs e)
        {
            base.OnSizeChanged(e);
        }

        private void BaseListBoxCursorChanged(object sender, EventArgs e)
        {
            base.OnCursorChanged(e);
        }

        private void BaseListBoxContextMenuStripChanged(object sender, EventArgs e)
        {
            base.OnContextMenuStripChanged(e);
        }

        private void BaseListBoxContextMenuChanged(object sender, EventArgs e)
        {
            base.OnContextMenuChanged(e);
        }

        private void BaseListBoxClientSizeChanged(object sender, EventArgs e)
        {
            base.OnClientSizeChanged(e);
        }

        private void BaseListBoxClick(object sender, EventArgs e)
        {
            base.OnClick(e);
        }

        private void BaseListBoxChangeUiCues(object sender, UICuesEventArgs e)
        {
            base.OnChangeUICues(e);
        }

        private void BaseListBoxCausesValidationChanged(object sender, EventArgs e)
        {
            base.OnCausesValidationChanged(e);
        }

        private void BaseListBoxKeyUp(object sender, KeyEventArgs e)
        {
            base.OnKeyUp(e);
        }

        private void BaseListBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
        }

        private void BaseListBoxKeyDown(object sender, KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }

        private void BaseListBoxTextChanged(object sender, EventArgs e)
        {
            base.OnTextChanged(e);
        }

        #endregion
    }
}