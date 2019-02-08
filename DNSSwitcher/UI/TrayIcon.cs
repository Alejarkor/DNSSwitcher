using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp1.Properties;
using DNSSwitcher.Network;

namespace DNSSwitcher.UI
{
    /// <inheritdoc />
    /// <summary>
    /// Class responsible for displaying the icon on the tray menu.
    /// </summary>
    public class TrayIcon : ApplicationContext
    {
        /// <summary>
        /// Icon that is displayed on the tray menu.
        /// </summary>
        private readonly NotifyIcon trayIcon = new NotifyIcon();

        /// <inheritdoc />
        /// <summary>
        /// Constructor.
        /// Initializes the icon.
        /// </summary>
        public TrayIcon()
        {
            ThemeManager.Initialize();

            trayIcon = new NotifyIcon {Icon = trayIcon.Icon = ThemeManager.CurrentTheme.ErrorIcon, Visible = true};
            InitializeContextMenu();
        }

        /// <summary>
        /// Initialize the icon on the tray.
        /// </summary>
        private void InitializeContextMenu()
        {
            var themesMenu = new MenuItem("Theme");
            foreach (var theme in ThemeManager.Themes)
            {
                var themeOption =
                    new MenuItem(theme.Name, (sender, args) => ChangeTheme(theme.Name))
                    {
                        Checked = theme == ThemeManager.CurrentTheme
                    };
                themesMenu.MenuItems.Add(themeOption);
            }

            var runOnStartUp = new MenuItem("Run on start up", SwitchStartUpProgram);
            var exit = new MenuItem("Exit", Exit);

            trayIcon.ContextMenu = new ContextMenu();
            trayIcon.ContextMenu.MenuItems.AddRange(new[] {themesMenu, runOnStartUp, exit});

            trayIcon.MouseClick += MouseClick;
            RefreshRunOnStartUp();

            // TODO: This setup for consistency shouldn't be here, probably.
            DnsHelper.SetDefaultDns();
            RefreshIcons();
        }


        /// <summary>
        /// Called when the mouse clicks on the icon.
        /// </summary>
        /// <param name="sender">The icon sends this event.</param>
        /// <param name="e">The arguments of the event.</param>
        private void MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            // TODO: this logic shouldn't be in a ui class (?).
            if (!DnsHelper.Connected) return;
            if (DnsHelper.UsingDefault)
                DnsHelper.SetGoogleDns();
            else
                DnsHelper.SetDefaultDns();
            RefreshIcons();
        }

        /// <summary>
        /// Changes the app theme.
        /// </summary>
        /// <param name="themeName">Theme name.</param>
        private void ChangeTheme(string themeName)
        {
            ThemeManager.ChangeTheme(themeName);
            RefreshIcons();
            RefreshSelectedTheme();
        }
        
        /// <summary>
        /// Switch if it is a windows start up program.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchStartUpProgram(object sender, EventArgs e)
        {
            Utilities.SwitchStartUpProgram();
            RefreshRunOnStartUp();
        }

        /// <summary>
        /// Refreshes the icons.
        /// TODO: Is there anyway to call this method if something changes?
        /// </summary>
        private void RefreshIcons()
        {
            if (!DnsHelper.Connected)
                trayIcon.Icon = ThemeManager.CurrentTheme.ErrorIcon;
            else
                trayIcon.Icon = DnsHelper.UsingDefault
                    ? ThemeManager.CurrentTheme.DefaultDnsIcon
                    : ThemeManager.CurrentTheme.PublicDnsIcon;
        }

        /// <summary>
        /// Refreshes the selected theme on the menu.
        /// </summary>
        private void RefreshSelectedTheme()
        {
            foreach (MenuItem menuItem in trayIcon.ContextMenu.MenuItems[0].MenuItems)
                menuItem.Checked = ThemeManager.CurrentTheme.Name == menuItem.Text;
        }

        /// <summary>
        /// Refreshes the run on start up button.
        /// </summary>
        private void RefreshRunOnStartUp() => trayIcon.ContextMenu.MenuItems[1].Checked = Utilities.IsStartUpProgram;

        /// <summary>
        /// Closes the app.
        /// TODO: this shouldn't be in a UI class.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void Exit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }
    }
}