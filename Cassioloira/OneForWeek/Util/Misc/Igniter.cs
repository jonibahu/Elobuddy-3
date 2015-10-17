using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace OneForWeek.Util.Misc
{
    internal class Igniter
    {
        public static Spell.Targeted Ignite;
        public static Menu IgniteMenu;

        public static void Init()
        {
            IgniteMenu = MainMenu.AddMenu("Igniter", "Igniter");
            IgniteMenu.AddGroupLabel("Draw");
            IgniteMenu.Add("useIgnite", new CheckBox("Use Igniter", true));
            IgniteMenu.Add("minRange", new Slider("Min Range to cast it: ", 400, 1, 600));
            IgniteMenu.Add("drawRange", new CheckBox("Draw ignite Range", false));

            Game.OnUpdate += OnGameUpdate;
        }

        private static void OnGameUpdate(EventArgs args)
        {
            if (ObjectManager.Player.IsDead || !Ignite.IsReady() || !Misc.IsChecked(IgniteMenu, "useIgnite")) return;

            var target2 = ObjectManager.Get<AIHeroClient>()
                    .Where(h => h.IsValidTarget(Ignite.Range) && h.Distance(Player.Instance) >= Misc.GetSliderValue(IgniteMenu, "minRange") && h.Health < ObjectManager.Player.GetSummonerSpellDamage(h, DamageLibrary.SummonerSpells.Ignite));

            Ignite.Cast(target2.First());
        }
    }
}
