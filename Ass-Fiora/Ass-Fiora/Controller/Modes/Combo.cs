using System.Collections.Generic;
using System.Linq;
using Ass_Fiora.Helpers;
using Ass_Fiora.Model;
using Ass_Fiora.Model.Enum;
using BRSelector.Model;
using EloBuddy;
using EloBuddy.SDK;
using OneForWeek.Util.Misc;
using SharpDX;

namespace Ass_Fiora.Controller.Modes
{
    public sealed class Combo : ModeBase
    {

        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            var q = PluginModel.Q;
            var w = PluginModel.W;
            var r = PluginModel.R;

            var target = AdvancedTargetSelector.GetTarget(w.Range, DamageType.Physical);

            if (target == null || !target.IsValidTarget()) return;

            PluginModel.ActiveMode = EnumModeManager.Combo;

            Orbwalker.ForcedTarget = target;

            if (PassiveController.HasUltiPassive(target))
            {
                var targetpos = Prediction.Position.PredictUnitPosition(target, 250);
                var poses = PassiveController.UltiPassivePos(target);
                var castpos = poses.OrderByDescending(x => x.Distance(targetpos)).FirstOrDefault();

                q.Cast(castpos);
            }

            if (q.IsReady() && Misc.IsChecked(PluginModel.ComboMenu, "comboQ"))
            {
                var targetpos = Prediction.Position.PredictUnitPosition(target, 250);

                if (Misc.IsChecked(PluginModel.ComboMenu, "comboQPassiveRange") && q.IsInRange(targetpos.To3D()) &&
                    PassiveController.HasPassive(target))
                {
                    var castPos = PassiveController.PassivePosition(target);
                    var validPositions = PassiveController.PassiveRadiusPoint(target);

                    if (castPos.IsValid() && castPos.Distance(Player.Instance.ServerPosition) <= 300 &&
                        castPos.InTheCone(validPositions, castPos.To2D()))
                    {
                        Player.CastSpell(SpellSlot.Q, castPos);
                    }
                    else
                    {
                        Player.CastSpell(SpellSlot.Q, Game.CursorPos);
                    }
                }
                else
                {
                    Player.CastSpell(SpellSlot.Q, target.ServerPosition);
                }
            }

            if (w.IsReady() && Misc.IsChecked(PluginModel.ComboMenu, "comboW"))
            {
                if (q.IsReady() || !(target.Distance(Player.Instance) > Player.Instance.GetAutoAttackRange())) return;

                var prediction = w.GetPrediction(target);

                if (prediction.HitChancePercent >= 70)
                {
                    w.Cast(prediction.CastPosition);
                }
            }

            if (r.IsReady() && Misc.IsChecked(PluginModel.ComboMenu, "comboR") && Player.Instance.HealthPercent < 50)
            {
                r.Cast(target);
            }

            if (Player.Instance.IsInAutoAttackRange(target))
            {
                ItemManager.UseYomu();
            }

            if (Player.Instance.HealthPercent < target.HealthPercent)
            {
                ItemManager.UseCastables(target);
            }
        }
    }
}
