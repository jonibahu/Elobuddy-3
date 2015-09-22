using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using Vayne.Util;
using Color = System.Drawing.Color;
using EloBuddy.SDK.Rendering;

namespace Vayne
{
    class Events
    {
        public static Obj_AI_Base AAedTarget = null;

        public static void Gapcloser_OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (Program.isChecked(Program.ComboMenu, "comboAutoCondemnGapCloser"))
            {
                if (Program.E.IsReady() && Globals._Player.Distance(sender.Position) <= Program.E.Range && sender.IsFacing(Globals._Player))
                {
                    Program.E.Cast(sender);
                }
            }
        }

        public static void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                if (Program.Q.IsReady() && Program.isChecked(Program.ComboMenu, "comboQ") && target.Distance(Globals._Player) <= (int)Globals._Player.GetAutoAttackRange() + Program.Q.Range)
                {
                    if (!Program.isChecked(Program.ComboMenu, "comboQUsage"))
                    {
                        if (Program.isChecked(Program.ComboMenu, "comboQFoward"))
                        {
                            Program.Q.Cast(target.Position);
                        }
                        else
                        {
                            Program.Q.Cast(Game.CursorPos);
                        }
                    }
                }
            }
        }

        public static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                if (!Orbwalker.CanAutoAttack && Program.isChecked(Program.LaneClearMenu, "laneQIfCantAttack"))
                {
                    foreach (var enem in ObjectManager.Get<Obj_AI_Base>().Where(a => a.IsEnemy).Where(a => !a.IsDead))
                    {
                        if (enem.Health < (Globals._Player.GetAutoAttackDamage(enem) + (30 + (Program.Q.Level * 15))))
                        {
                            Program.Q.Cast(Game.CursorPos);
                        }
                    }
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                if (Program.Q.IsReady() && Program.isChecked(Program.ComboMenu, "comboQ"))
                {
                    if (Program.isChecked(Program.ComboMenu, "comboQUsage"))
                    {
                        if (Program.isChecked(Program.ComboMenu, "comboQFoward"))
                        {
                            Program.Q.Cast(target.Position);
                        }
                        else
                        {
                            Program.Q.Cast(Game.CursorPos);
                        }
                    }
                }
            }
        }


        public static void AIHeroClient_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;
            if (args.SData.Name.ToLower().Contains("vaynetumble"))
            {
                Core.DelayAction(Orbwalker.ResetAutoAttack, 250);
            }
        }
    }
}
