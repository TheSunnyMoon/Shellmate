using Shellmate.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;

namespace Shellmate
{
    class CommandRow
    {
        private string[,] command;


        /// <summary>
        /// Constructor and initialise the command array
        /// </summary>
        public CommandRow()
        {
            command = JsonConvert.DeserializeObject<string[,]>(File.ReadAllText((Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)).ToString() + "/commands.json"));

        }

        public string[,] Command { get => command; set => command = value; }

        public void writeCommand(string[,] table)
        {
            
            File.WriteAllText((Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)).ToString() + "/commands.json", JsonConvert.SerializeObject(table));
        }
    }
}
