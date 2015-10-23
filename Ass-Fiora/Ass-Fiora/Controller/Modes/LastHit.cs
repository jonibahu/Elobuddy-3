using System.Collections.Generic;
using System.Linq;
using Ass_Fiora.Helpers;
using Ass_Fiora.Model;
using Ass_Fiora.Model.Enum;
using BRSelector.Model;
using EloBuddy;
using EloBuddy.SDK;
using OneForWeek.Util.Misc;

namespace Ass_Fiora.Controller.Modes
{
    public sealed class LastHit : ModeBase
    {

        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
        }

        public override void Execute()
        {
            var q = PluginModel.Q;
            var w = PluginModel.W;

            var minionTarget = EntityManager.MinionsAndMonsters.EnemyMinions.Aggregate((curMin, x) => (curMin == null || x.HealthPercent < curMin.Health ? x : curMin));

            if (minionTarget == null || !minionTarget.IsValidTarget()) return;

            if (q.IsReady() && Misc.IsChecked(PluginModel.LastHitMenu, "lhQ") 
                && ManaManager.CanUseSpell(PluginModel.LastHitMenu, "lhMana")
                && q.IsInRange(minionTarget) 
                && Player.Instance.GetSpellDamage(minionTarget, SpellSlot.Q) > minionTarget.Health 
                && (!Orbwalker.CanAutoAttack || Player.Instance.IsInAutoAttackRange(minionTarget)))
            {
                q.Cast(minionTarget);
            }

            if (w.IsReady() && Misc.IsChecked(PluginModel.LastHitMenu, "lhW") && ManaManager.CanUseSpell(PluginModel.LastHitMenu, "lhMana") && w.IsInRange(minionTarget) && !Orbwalker.CanAutoAttack && !Player.Instance.IsInAutoAttackRange(minionTarget))
            {
                w.Cast(minionTarget);
            }
        }
    }
}
