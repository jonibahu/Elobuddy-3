using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace Vayne
{
    class Events
    {
        public static Obj_AI_Base AAedTarget = null;
        public static long LastAa;
        public static int AaStacks;

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                if (Program.Q.IsReady() && Program.isChecked(Program.ComboMenu, "comboQ") && target.Distance(_Player) <= (int)_Player.GetAutoAttackRange() + Program.Q.Range)
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
            if (Program.ComboMenu["autoCondemnToggle"].Cast<KeyBind>().CurrentValue && TargetSelector2.GetTarget(Program.E.Range, DamageType.Physical) != null)
            {
                Program.E.Cast(TargetSelector2.GetTarget(Program.E.Range, DamageType.Physical));
                if (!Program.E.IsReady())
                {
                    Program.ComboMenu["autoCondemnToggle"].Cast<KeyBind>().CurrentValue = false;
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                if (!Orbwalker.CanAutoAttack && Program.isChecked(Program.LaneClearMenu, "laneQIfCantAttack"))
                {
                    foreach (var enem in ObjectManager.Get<Obj_AI_Base>().Where(a => a.IsEnemy).Where(a => _Player.Distance(a) < _Player.GetAutoAttackRange() + Program.Q.Range).Where(a => !a.IsDead))
                    {
                        if (enem.Health < (_Player.GetAutoAttackDamage(enem) + (30 + (Program.Q.Level * 15))))
                        {
                            Program.Q.Cast(Game.CursorPos);
                        }
                    }
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                if (Program.Q.IsReady() && Program.isChecked(Program.ComboMenu, "comboQ") && target.Distance(_Player) <= (int)_Player.GetAutoAttackRange() + Program.Q.Range)
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

        public static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, InterruptableSpellEventArgs e)
        {
            
        }

    }
}
