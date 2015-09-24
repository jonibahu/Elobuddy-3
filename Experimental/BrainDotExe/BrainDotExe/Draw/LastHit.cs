using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using Color = System.Drawing.Color;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using BrainDotExe.Util;

namespace BrainDotExe.Draw
{
    internal class LastHit
    {

        public static Menu LastHitMenu;

        public static void Init()
        {

            LastHitMenu = Program.DrawMenu.AddSubMenu("Last Hit", "lastHitDraw");
            LastHitMenu.AddGroupLabel("Last Hit");
            LastHitMenu.Add("drawLastHit", new CheckBox("Draw Last Hit Marker", true));

        }

        public static void DrawLastHit_OnDraw(EventArgs args)
        {
            if(Misc.isChecked(LastHitMenu, "drawLastHit"))
            {
                var distM = Program._Player.GetAutoAttackRange() + 500;
                foreach (var minion in ObjectManager.Get<Obj_AI_Minion>()
                    .Where(m => m.CountEnemiesInRange(distM) >= Program._Player.CountEnemiesInRange(distM)
                                && m.IsEnemy))
                {
                    if (!minion.IsValidTarget(distM)) continue;
                    var hpBar = new Vector2(minion.HPBarPosition.X + 14, minion.HPBarPosition.Y + 12);
                    if (minion.Health <= Program._Player.GetAutoAttackDamage(minion, true))
                    {
                        Drawing.DrawText(hpBar, Color.Green, ">                        <", 100); // [:] SIZE NOT WORKING [:]
                    }
                }
            }
        }
    }
}