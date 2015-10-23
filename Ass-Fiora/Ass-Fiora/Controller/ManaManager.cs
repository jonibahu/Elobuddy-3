using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK.Menu;
using OneForWeek.Util.Misc;

namespace Ass_Fiora.Controller
{
    public static class ManaManager
    {

        public static bool CanUseSpell(Menu menu , string proprety)
        {
            return Player.Instance.ManaPercent >= Misc.GetSliderValue(menu, proprety);
        }
    }
}
