using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Properties;

namespace WindowsFormsApp1
{
    public class MyApplicationContext : ApplicationContext
    {
        private NotifyIcon trayIcon = new NotifyIcon();
        Icon happy = Resources.pera_Yxk_icon;
        Icon sad = Resources.limon_and_icon;
        private bool state = false;

        // Initalize the NofifyIcon object's shortcut menu.
        private void InitializeContextMenu()
        {
            MenuItem[] menuList = new MenuItem[]{new MenuItem("Sign In"),
            new MenuItem("Get Help"), new MenuItem("Open")};
            ContextMenu clickMenu = new ContextMenu(menuList);
            trayIcon.ContextMenu = clickMenu;

            // Associate the event-handling method with 
            // the NotifyIcon object's click event.
            trayIcon.Click += new System.EventHandler(NotifyIcon1_Click);
        }


        // When user clicks the left mouse button display the shortcut menu.  
        // Use the SystemInformation.PrimaryMonitorMaximizedWindowSize property
        // to place the menu at the lower corner of the screen.
        private void NotifyIcon1_Click(object sender, System.EventArgs e)
        {
           
            state = !state;
            if (state)
            {
                trayIcon.Icon = happy;
            }
            else
            {
                trayIcon.Icon = sad; 
            }
        }

        private Point PointToClient(Point menuPoint)
        {
            throw new NotImplementedException();
        }

        public MyApplicationContext()
        {
            // Initialize Tray Icon
            trayIcon = new NotifyIcon()
            {
                Icon = trayIcon.Icon = sad,
                //ContextMenu = new ContextMenu(new MenuItem[] {
                //new MenuItem("Exit", Exit)
            //}),
                Visible = true
            };
            InitializeContextMenu();
        }

        void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            trayIcon.Visible = false;

            Application.Exit();
        }
    }
}
