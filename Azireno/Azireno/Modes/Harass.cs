using Azireno.Plugin;
using Azireno.Util;
using EloBuddy;
using EloBuddy.SDK;

namespace Azireno.Modes
{
    class Harass : ModeModel
    {
        public void Execute()
        {
            if(! Misc.isChecked(HarassMenu, "hsSoldiers")) return;
            var target = TargetSelector.GetTarget(1100, DamageType.Magical);
            SoldierController.AutoPilot(Azir.AzirSoldiers, target);
        }

    }
}
