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
    class Combo : ModeModel
    {
        public static Brain Brain = new Brain();

        public void Execute()
        {
            if(Misc.IsChecked(ComboMenu,"comboR") && R.IsReady())
                Brain.RFinisher();

            _target = TargetSelector.GetTarget(W.Range, DamageType.Physical);

            if (_target == null || !_target.IsValidTarget()) return;

            if (Misc.IsChecked(ComboMenu, "comboQ") && Q.IsReady())
                Brain.AutoSwitchQ(_target);

            if (Misc.IsChecked(ComboMenu, "comboW") && W.IsReady())
                Brain.WCast(_target);

            if (Misc.IsChecked(ComboMenu , "comboE") && E.IsReady())
                Brain.ECast(_target);

        }
    }
}
