using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azireno.Modes
{
    abstract class ModeModel
    {
        #region Global Variables

        /*
         Config
         */
        
        public static string G_version = "1.0.1";
        public static string G_charname = _Player.ChampionName;

        /*
         Spells
         */
        public static Spell.Skillshot Q;
        public static Spell.Active W;
        public static Spell.Active E;
        public static Spell.Skillshot R;

        /*
         Menus
         */

        public static Menu Menu,
            ComboMenu,
            LaneClearMenu,
            CondemnMenu,
            KSMenu,
            DrawMenu;

        /*
         Misc
         */

        public static AIHeroClient _target;

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        #endregion
    }
}