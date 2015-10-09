using System;
using System.Linq;
using Azireno.Plugin;
using Azireno.Util;
using EloBuddy.SDK;
using SharpDX;

namespace Azireno.Modes
{
    class Combo : ModeModel
    {

        public void Execute()
        {
            if (_target == null || !_target.IsValidTarget()) return;

            var finisherPos = _Player.Position.Extend(_target, _Player.Distance(_target) + 100).To3D();

            Console.WriteLine("Combo Started");

            if (DmgLib.possibleDamage(_target) > _target.Health && R.IsReady() && _Player.Distance(finisherPos) < 875)
            {
                Console.WriteLine("Finisher");
                new Brain().CastFly(finisherPos);
            }
            else
            {
                Console.WriteLine(Orbwalker.ValidAzirSoldiers.Count);
                if (Orbwalker.AzirSoldiers.Count > 0)
                {
                    var countInRange = Orbwalker.ValidAzirSoldiers.Count(validAzirSoldier => validAzirSoldier.Distance(_target) < Orbwalker.AzirSoldierAutoAttackRange);
                    Console.WriteLine(countInRange);
                    if (countInRange > 0)
                    {
                    }
                    else
                    {
                        if (Q.IsReady() && Q.IsInRange(_target)) Q.Cast(_Player.Position.Extend(_target.Position, 100).To3D());
                        if (!Q.IsReady() && W.IsInRange(_target)) W.Cast(_Player.Position.Extend(_target.Position, 100).To3D());
                    }
                }
                else
                {
                    var pos = _target.Position;
                    pos = _Player.Distance(_target) > W.Range ? _Player.Position.Shorten(pos, -W.Range) : _Player.Position.Extend(pos, W.Range).To3D();
                    W.Cast(pos);
                }

            }

        }

        public void CastEandQ(Vector3 pos)
        {
            Q.Cast(pos);
            Core.DelayAction(() => E.Cast(), 75);
        }
    }
}
