using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Client.Armitage.Startup.Service
{
    partial class Edge_Updater : ServiceBase
    {
        private string _path;

        /// <summary>
        /// Since Task Scheduler is kinda retarded, this can run the letter much faster.
        /// </summary>
        /// <param name="path"></param>
        public Edge_Updater(string path)
        {
            _path = path;

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // check first if the process is running

        }

        protected override void OnStop()
        {

        }
    }
}
