using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vayne.Util
{
    internal static class Globals
    {
        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static Menu DrawMenu
        {
            get { return Program.DrawMenu; }
        }

        public static Menu ComboMenu
        {
            get { return Program.ComboMenu; }
        }

        public static AIHeroClient _Target
        {
            get { return TargetSelector.GetTarget(_Player.GetAutoAttackRange(), DamageType.Physical); }
        }

        public static AttackableUnit _OrbTarget
        {
            get { return Orbwalker.GetTarget(); }
        }

        public static AIHeroClient _HeroOrbTarget
        {
            get { return _OrbTarget as AIHeroClient; }
        }
    }
}
