using BrainDotExe.Util;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using System;
using System.Drawing;
using System.Linq;

namespace BrainDotExe.Draw
{
    class AttackRanges
    {
        public static Menu AttackRangeMenu;

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static void Init()
        {
            AttackRangeMenu = Program.Menu.AddSubMenu("Attack Range ", "attackRangeDraw");
            AttackRangeMenu.AddGroupLabel("Attack Range");
            AttackRangeMenu.Add("drawRanges", new CheckBox("Draw Ranges", true));
            AttackRangeMenu.Add("drawYLastHit", new CheckBox("Draw your Attack Range", true));
            AttackRangeMenu.Add("drawELastHit", new CheckBox("Draw enemies Attack Range", true));

            Drawing.OnDraw += AttackRange_OnDraw;
        }

        public static void AttackRange_OnDraw(EventArgs args)
        {
            if (Misc.isChecked(Program.DrawMenu, "drawDisable")) return;

            if (Misc.isChecked(AttackRangeMenu, "drawRanges"))
            {
                if(Misc.isChecked(AttackRangeMenu, "drawYLastHit"))
                    new Circle() { Color = Color.White, Radius = _Player.GetAutoAttackRange(), BorderWidth = 2f }.Draw(_Player.Position);

                if (Misc.isChecked(AttackRangeMenu, "drawELastHit"))
                {
                    foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(a => a.IsEnemy).Where(a => !a.IsDead).Where(a => _Player.Distance(a) <= 1650))
                    {
                        new Circle() { Color = Color.White, Radius = enemy.GetAutoAttackRange(), BorderWidth = 2f }.Draw(enemy.Position);
                    }
                }

            }
        }
    }
}
