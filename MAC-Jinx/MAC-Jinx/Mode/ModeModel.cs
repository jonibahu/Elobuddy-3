using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;

namespace MAC_Jinx.Mode
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
        public static Spell.Active Q;
        public static Spell.Skillshot W;
        public static Spell.Skillshot E;
        public static Spell.Skillshot R;

        /*
         Menus
         */

        public static Menu Menu,
            ComboMenu,
            LaneClearMenu,
            HarassMenu,
            MiscMenu,
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
