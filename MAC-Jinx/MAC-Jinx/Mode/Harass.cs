using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using MAC_Jinx.Util;

namespace MAC_Jinx.Mode
{
    class Harass : ModeModel
    {
        public static Brain Brain = new Brain();

        public void Execute()
        {
            _target = TargetSelector.GetTarget(_Player.GetAutoAttackRange() + Q.Range, DamageType.Physical);

            if (_target == null || !_target.IsValidTarget()) return;

            if (Misc.IsChecked(HarassMenu, "hsQ"))
                Brain.AutoSwitchQ(_target);

            if (Misc.IsChecked(HarassMenu, "hsW"))
                Brain.WCast(_target);
            
        }
    }
}
