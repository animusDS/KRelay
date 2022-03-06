using System;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Components;
using MetroFramework.Forms;
using Lib_K_Relay.Utilities;

namespace K_Relay
{
    partial class FrmMainMetro
    {
        private FixedStyleManager m_themeManager;

        private void InitSettings()
        {
            Invoke((MethodInvoker)delegate
            {
                m_themeManager = new FixedStyleManager(this);
                themeCombobox.Items.AddRange(Enum.GetNames(typeof(MetroThemeStyle)));
                styleCombobox.Items.AddRange(Enum.GetNames(typeof(MetroColorStyle)));

                themeCombobox.SelectedValueChanged += themeCombobox_SelectedValueChanged;
                styleCombobox.SelectedValueChanged += styleCombobox_SelectedValueChanged;

                themeCombobox.SelectedItem = Config.Default.Theme.ToString();
                styleCombobox.SelectedItem = Config.Default.Style.ToString();

                tglStartByDefault.Checked = Config.Default.StartProxyByDefault;
                lstServers.SelectedItem = Config.Default.DefaultServerName;

                m_themeManager.OnStyleChanged += m_themeManager_OnStyleChanged;
                m_themeManager_OnStyleChanged(null, null);
            });
        }

        private void styleCombobox_SelectedValueChanged(object sender, EventArgs e)
        {
            m_themeManager.Style =
                (MetroColorStyle)Enum.Parse(typeof(MetroColorStyle), (string)styleCombobox.SelectedItem, true);
        }

        private void themeCombobox_SelectedValueChanged(object sender, EventArgs e)
        {
            m_themeManager.Theme =
                (MetroThemeStyle)Enum.Parse(typeof(MetroThemeStyle), (string)themeCombobox.SelectedItem, true);
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            Config.Default.StartProxyByDefault = tglStartByDefault.Checked;
            Config.Default.DefaultServerName = lstServers.SelectedItem.ToString();
            Lib_K_Relay.Utilities.Config.Default.ProxyServerName = ProxyServerNameTxtBox.Text;
            Lib_K_Relay.Utilities.Config.Default.Save();
            Config.Default.Theme =
                (MetroThemeStyle)Enum.Parse(typeof(MetroThemeStyle), (string)themeCombobox.SelectedItem, true);
            Config.Default.Style =
                (MetroColorStyle)Enum.Parse(typeof(MetroColorStyle), (string)styleCombobox.SelectedItem, true);
            Config.Default.Save();

            MetroMessageBox.Show(this, "\nYour settings have been saved.", "Save Settings", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private class FixedStyleManager
        {
            private readonly MetroStyleManager m_manager;
            private MetroColorStyle m_colorStyle;
            private MetroThemeStyle m_themeStyle;


            public FixedStyleManager(MetroForm form)
            {
                m_manager = new MetroStyleManager(form.Container);
                m_manager.Owner = form;
            }

            public MetroColorStyle Style
            {
                get => m_colorStyle;
                set
                {
                    m_colorStyle = value;
                    Update();
                    if (OnStyleChanged != null) OnStyleChanged(this, new EventArgs());
                }
            }

            public MetroThemeStyle Theme
            {
                get => m_themeStyle;
                set
                {
                    m_themeStyle = value;
                    Update();
                    if (OnThemeChanged != null) OnThemeChanged(this, new EventArgs());
                }
            }

            public event EventHandler OnThemeChanged;
            public event EventHandler OnStyleChanged;

            public void Update()
            {
                (m_manager.Owner as MetroForm).Theme = m_themeStyle;
                (m_manager.Owner as MetroForm).Style = m_colorStyle;

                m_manager.Theme = m_themeStyle;
                m_manager.Style = m_colorStyle;
            }
        }
    }
}