using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Shellmate
{
    class CommandRow
    {
        private string[,] command;
        private readonly string filePath;

        public CommandRow()
        {
            filePath = Path.Combine(Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), "commands.json");
            LoadCommands();
        }

        public string[,] Command { get => command; set => command = value; }

        private void LoadCommands()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    command = !string.IsNullOrWhiteSpace(json) ? JsonConvert.DeserializeObject<string[,]>(json) : new string[0, 2];
                }
                else
                {
                    command = new string[0, 2];
                    SaveCommands();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des commandes : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                command = new string[0, 2];
            }
        }

        public void SaveCommands()
        {
            try
            {
                string json = JsonConvert.SerializeObject(command, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'écriture des commandes : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ExecuteCommand(string picommand)
        {
            try
            {
                for (int i = 0; i < command.GetLength(0); i++)
                {
                    if (command[i, 0] == picommand)
                    {
                        string cmd = command[i, 1];
                        ProcessStartInfo psi = new ProcessStartInfo()
                        {
                            FileName = "powershell.exe",
                            Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{cmd}\"",
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };
                        Process.Start(psi);
                        Process.Start(psi);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'exécution de la commande : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

