using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Color = System.Drawing.Color;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using BrainDotExe.Util;

namespace BrainDotExe.Draw
{
    internal class LastHit
    {
        public static Menu LastHitMenu;

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static void Init()
        {
            LastHitMenu = Program.Menu.AddSubMenu("Last Hit Marker ", "lastHitDraw");
            LastHitMenu.AddGroupLabel("Last Hit Marker");
            LastHitMenu.Add("drawLastHit", new CheckBox("Draw Last Hit Marker", false));

            Drawing.OnDraw += LastHit_OnDraw;
        }

        public static void LastHit_OnDraw(EventArgs args)
        {
            if (Misc.isChecked(Program.DrawMenu, "drawDisable")) return;

            if (Misc.isChecked(LastHitMenu, "drawLastHit"))
            {
                var distM = Program._Player.GetAutoAttackRange() + 500;
                foreach (var minion in ObjectManager.Get<Obj_AI_Minion>()
                    .Where(m => m.CountEnemiesInRange(distM) >= Program._Player.CountEnemiesInRange(distM)
                                && m.IsEnemy))
                {
                    if (!minion.IsValidTarget(distM)) continue;
                    if (minion.Health <= Program._Player.GetAutoAttackDamage(minion, true))
                    {
                        Misc.DrawMarkPoint(minion, Color.Red, 15, 2f);
                    }else if (minion.Health <= (Program._Player.GetAutoAttackDamage(minion, true) + Program._Player.GetAutoAttackDamage(minion, true)))
                    {
                        Misc.DrawMarkPoint(minion, Color.Yellow, 15, 2f);
                    }
                }
            }
        }
    }
}
