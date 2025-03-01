using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shellmate
{
    public partial class HomeWindow: Form
    {
        // simulate the clic on ControlBox 
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        /******************************* lib imports *****************************************/
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        /// <summary>
        /// Homewindow constructor
        /// </summary>
        public HomeWindow()
        {
            InitializeComponent();

            this.Controls.Add(HomeMenuStrip);

            CommandShell.Select();
        }

        /// <summary>
        /// Move the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveWindow(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture(); // Libère la capture de la souris
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0); // Simule un clic sur la barre de titre
            }
        }

        /// <summary>
        /// Close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

   

        private void CommandShell_Validated(object sender, EventArgs e)
        {
        }

        private void CommandShell_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar == (char)13))
            {
                return;
            }


            ExecuteButton.PerformClick();

            CommandShell.Text = "";



        }

        private void ExecuteButton_Click(object sender, EventArgs e)
        {
            if (CommandShell.Text == "")
            {
                return;
            }

            string command = CommandShell.Text;
            CommandRow commandRow = new CommandRow();
            commandRow.ExecuteCommand(command);
            this.Close();

        }

        /// <summary>
        /// Open the edit commands window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editCommandsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandEditor commandEditor = new CommandEditor();
            commandEditor.Show();
        }
    }
}
