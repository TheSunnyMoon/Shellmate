using Shellmate.Properties;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Shellmate
{
    public partial class CommandEditor: Form
    {
        // simulate the clic on ControlBox 
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;
        private CommandRow commandRow = new CommandRow();

        /******************************* lib imports *****************************************/
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        public CommandEditor()
        {
            InitializeComponent();

            foreach (String row in commandRow.Command)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.dataGridView1.Rows.Add(commandRow.Command.GetLength(0));
            this.Controls.Add(HomeMenuStrip);

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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            commandRow.Command = new string[dataGridView1.RowCount, 2];

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[1].Value != null)
                {
                    commandRow.Command[row.Index, 0] = row.Cells[0].Value.ToString();
                    commandRow.Command[row.Index, 1] = row.Cells[1].Value.ToString();
                }
            }
            commandRow.writeCommand(commandRow.Command);
        }
    }
}
