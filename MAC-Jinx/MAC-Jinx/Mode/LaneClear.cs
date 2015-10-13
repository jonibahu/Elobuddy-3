using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using MAC_Jinx.Util;

namespace MAC_Jinx.Mode
{
    class LaneClear : ModeModel
    {
        public static Brain Brain = new Brain();

        public void Execute()
        {
            if (Misc.IsChecked(LaneClearMenu,"lcQ"))
                Brain.AutoSwitchQFarm();
        }
    }
}
