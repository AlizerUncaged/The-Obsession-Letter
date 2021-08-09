using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Client.Armitage.Startup.Service
{
    [RunInstaller(true)]
    public class Service_Metadata : Installer
    {
        ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();
        ServiceInstaller serviceInstaller = new ServiceInstaller();

        // todo, but for now test task scheduler priority
    }
}
