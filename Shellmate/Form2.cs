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
            LoadCommandTable();

            this.Controls.Add(HomeMenuStrip);

        }


        /// <summary>
        /// Load the command table
        /// </summary>
        private void LoadCommandTable()
        {
            if (commandRow.Command == null || commandRow.Command.GetLength(0) == 0)
            {
                return;
            }


            dataGridView1.Rows.Clear();

            for (int i = 0; i < commandRow.Command.GetLength(0); i++)
            {
                int rowIndex = dataGridView1.Rows.Add();

                for (int j = 0; j < commandRow.Command.GetLength(1); j++)
                {
                    if (commandRow.Command[i, j] != null) 
                        dataGridView1.Rows[rowIndex].Cells[j].Value = commandRow.Command[i, j];
                }
            }
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
                ReleaseCapture(); 
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0); 
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
            int rowCount = dataGridView1.Rows.Count - 1; 

            List<string[]> newCommands = new List<string[]>(); 

            for (int i = 0; i < rowCount; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value != null && dataGridView1.Rows[i].Cells[1].Value != null)
                {
                    string cmdKey = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    string cmdValue = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    newCommands.Add(new string[] { cmdKey, cmdValue });
                }
            }

            // Conversion en tableau 2D
            commandRow.Command = new string[newCommands.Count, 2];
            for (int i = 0; i < newCommands.Count; i++)
            {
                commandRow.Command[i, 0] = newCommands[i][0];
                commandRow.Command[i, 1] = newCommands[i][1];
            }

            commandRow.SaveCommands(); // Sauvegarde dans le JSON
        }

    }
}
