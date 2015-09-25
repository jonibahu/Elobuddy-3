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

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static void Init()
        {
            LastHitMenu = Program.Menu.AddSubMenu("Last Hit Marker ", "lastHitDraw");
            LastHitMenu.AddGroupLabel("Last Hit Marker");
            LastHitMenu.Add("drawLastHit", new CheckBox("Draw Last Hit Marker", true));

            Drawing.OnDraw += DrawLastHit_OnDraw;
        }

        public static void DrawLastHit_OnDraw(EventArgs args)
        {
            if (Misc.isChecked(LastHitMenu, "drawLastHit"))
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
                        Misc.DrawMarkPoint(minion.Position, Color.Green, 15, 2f);
                        /*Vector3 pos = new Vector3(_Player.Position.To2D(), _Player.Position.Z);
                        var playpos = Drawing.WorldToScreen(_Player.Position);
                        var newpos = Drawing.WorldToScreen(_Player.Position);

                        Drawing.DrawLine(playpos, newpos, 2f, Color.Red);*/
                        //Drawing.DrawText(hpBar, Color.Green, ">                        <", 100); // [:] SIZE NOT WORKING [:]
                    }
                }
            }
        }
    }
}