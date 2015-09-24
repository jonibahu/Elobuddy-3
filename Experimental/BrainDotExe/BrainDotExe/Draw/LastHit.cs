using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using Color = System.Drawing.Color;

namespace BrainDotExe.Draw
{
    internal class LastHit
    {
        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static void DrawLastHit_OnDraw(EventArgs args)
        {
            var distM = _Player.GetAutoAttackRange() + 500;
            foreach (var minion in ObjectManager.Get<Obj_AI_Minion>()
                .Where(m => m.CountEnemiesInRange(distM) >= _Player.CountEnemiesInRange(distM)
                            && m.IsEnemy))
            {
                if (!minion.IsValidTarget(distM)) continue;
                var hpBar = new Vector2(minion.HPBarPosition.X + 14, minion.HPBarPosition.Y + 12);
                if (minion.Health <= _Player.GetAutoAttackDamage(minion, true))
                {
                    Drawing.DrawText(hpBar, Color.White, ">                        <", 100); // [:] SIZE NOT WORKING [:]
                }
            }
        }
    }
}