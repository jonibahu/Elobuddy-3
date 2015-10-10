using Azireno.Util;
using Azireno.Util.Helpers;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Azireno.Modes
{
    class Combo : ModeModel
    {

        public void Execute()
        {
            var target = TargetSelector.GetTarget(1100, DamageType.Magical);

            if(target == null || !target.IsValidTarget()) return;

            var predictionR = R.GetPrediction(target);
            if (DmgLib.R(target) > target.Health && R.IsReady() && R.IsInRange(target) && predictionR.HitChance >= HitChance.Medium && Misc.isChecked(ComboMenu, "finisherR"))
                R.Cast(predictionR.CastPosition);

            if (DmgLib.E(target) > target.Health && E.IsReady() && Misc.isChecked(ComboMenu, "finisherE"))
            {
                foreach (var validAzirSoldier in Orbwalker.ValidAzirSoldiers)
                {
                    if (target.Position.Between(_Player.Position, validAzirSoldier.ServerPosition))
                    {
                        E.Cast();
                    }
                }
            }

            if(Misc.isChecked(ComboMenu, "comboSoldiers"))
                SoldierController.AutoPilot(Orbwalker.ValidAzirSoldiers, target);
        }
    }
}
