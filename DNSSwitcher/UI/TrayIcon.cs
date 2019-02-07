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

        /// <summary>
        /// Icon for the happy face.
        /// </summary>
        private readonly Icon happyIcon = Resources.pera_Yxk_icon;

        /// <summary>
        /// Icon for the sad face.
        /// </summary>
        private readonly Icon sadIcon = Resources.limon_and_icon;

        /// <summary>
        /// Error icon.
        /// </summary>
        private readonly Icon errorIcon = Resources.sad2_xZP_icon;

        /// <inheritdoc />
        /// <summary>
        /// Constructor.
        /// Initializes the icon.
        /// </summary>
        public TrayIcon()
        {
            trayIcon = new NotifyIcon {Icon = trayIcon.Icon = sadIcon, Visible = true};
            InitializeContextMenu();
        }

        /// <summary>
        /// Initialize the icon on the tray.
        /// </summary>
        private void InitializeContextMenu()
        {
            trayIcon.ContextMenu = new ContextMenu(new[] {new MenuItem("Exit", Exit)});
            trayIcon.MouseClick += MouseClick;

            // TODO: This setup for consistency shouldn't be here, probably.
            if (!DnsHelper.Connected)
                trayIcon.Icon = errorIcon;
            else
            {
                trayIcon.Icon = sadIcon;
                DnsHelper.SetDefaultDns();
            }
        }


        /// <summary>
        /// Called when the mouse clicks on the icon.
        /// </summary>
        /// <param name="sender">The icon sends this event.</param>
        /// <param name="e">The arguments of the event.</param>
        private void MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            // TODO: this logic shouldn't be in a ui class.
            if (!DnsHelper.Connected) return;
            if (DnsHelper.UsingDefault)
            {
                trayIcon.Icon = happyIcon;
                DnsHelper.SetGoogleDns();
            }
            else
            {
                trayIcon.Icon = sadIcon;
                DnsHelper.SetDefaultDns();
            }
        }

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