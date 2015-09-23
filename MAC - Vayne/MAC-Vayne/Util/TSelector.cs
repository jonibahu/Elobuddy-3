using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using MAC_Vayne.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAC_Vayne.Util
{
    static class TSelector
    {

        public static Menu TargetMenu;

        private static void initMenu(Menu config)
        {
            TargetMenu = config.AddSubMenu("Target Selector", "TargetSelecta");
            TargetMenu.AddGroupLabel("Type");
            TargetMenu.Add("drawDisable", new CheckBox("Turn off all drawings", false));
        }

        public static AIHeroClient getTarget()
        {

            return null;
        }
        
    }
}
