using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Shellmate
{
    public partial class HomeWindow : Form
    {
        // Constantes pour le déplacement de fenêtre
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        // Constantes pour le hotkey
        private const int WM_HOTKEY = 0x0312;
        private const int MOD_ALT = 0x0001;
        private const int HOTKEY_ID = 9000; // Identifiant arbitraire pour le hotkey

        /******************************* Imports des librairies *****************************************/
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public HomeWindow()
        {
            InitializeComponent();
            this.Controls.Add(HomeMenuStrip);
            CommandShell.Select();
        }

        /// <summary>
        /// Au chargement, enregistre le hotkey et cache la fenêtre
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Enregistrement d'Alt+F2 comme hotkey
            RegisterHotKey(this.Handle, HOTKEY_ID, MOD_ALT, (int)Keys.F2);
            // Cache la fenêtre dès le démarrage
            this.Hide();
        }

        /// <summary>
        /// Si l'utilisateur tente de fermer la fenêtre, on l'annule et on la cache.
        /// Lors de la fermeture définitive (par exemple via Application.Exit), le hotkey est désenregistré.
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Si la fermeture est initiée par l'utilisateur, on cache la fenêtre au lieu de la fermer.
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
                return;
            }
            UnregisterHotKey(this.Handle, HOTKEY_ID);
            base.OnFormClosing(e);
        }

        /// <summary>
        /// Interception du message WM_HOTKEY pour révéler la fenêtre
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == HOTKEY_ID)
            {
                // Révèle la fenêtre
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.BringToFront();
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// Permet de déplacer la fenêtre en simulant le clic sur la barre de titre
        /// </summary>
        private void MoveWindow(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        /// <summary>
        /// Bouton de fermeture : cache la fenêtre au lieu de la fermer
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// Lorsqu'une commande est validée par la touche Entrée
        /// </summary>
        private void CommandShell_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)13)
                return;

            ExecuteButton.PerformClick();
            CommandShell.Text = "";
        }

        /// <summary>
        /// Exécute la commande et cache la fenêtre ensuite
        /// </summary>
        private void ExecuteButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CommandShell.Text))
                return;

            string command = CommandShell.Text;
            CommandRow commandRow = new CommandRow();
            commandRow.ExecuteCommand(command);
            this.Hide();
        }

        /// <summary>
        /// Ouvre la fenêtre d'édition des commandes
        /// </summary>
        private void editCommandsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandEditor commandEditor = new CommandEditor();
            commandEditor.Show();
        }
    }
}


