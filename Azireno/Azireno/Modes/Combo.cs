using Azireno.Plugin;
using Azireno.Util;
using EloBuddy.SDK;
namespace Azireno.Modes
{
    class Combo : ModeModel
    {

        public void Execute()
        {
            if (_target == null || !_target.IsValid) return;

            //Finisher Combo
            if(DmgLib.possibleDamage(_target) > _target.Health && R.IsReady() && _Player.Distance(_target) < E.Range)
            {
                if (E.IsReady() && Q.IsReady())
                {
                    if (Azir.AzirSoldiers.Count > 0)
                    {
                        E.Cast();
                        Core.DelayAction(() => Q.Cast(_target.Position), 75);
                    }
                    else
                    {
                        W.Cast();
                        Core.DelayAction(CastEandQ, 250);
                    }
                }

                if (_Player.Distance(_target) < 50)
                {
                    R.Cast(_target.Position.Extend(_Player, R.Range).To3D());
                }
                else if (_Player.Distance(_target) < R.Range - 50 && DmgLib.R(_target) > _target.Health)
                {
                    R.Cast(_target.Position);
                }
                
            }

            //Normal DMG
            if (Azir.AzirSoldiers.Count > 0)
            {
                int soldiersInRange = 0;
                foreach(var soldier in Azir.AzirSoldiers)
                {
                    if(soldier.Distance(_target) < 375)
                    {
                        soldiersInRange++;
                    }
                }

                if(Azir.AzirSoldiers.Count > soldiersInRange && _Player.Distance(_target) < 875 && Q.IsReady())
                {
                    Q.Cast(_target.Position);
                }

                if(Azir.AzirSoldiers.Count <= 2 && W.IsReady() && _Player.Distance(_target) < W.Range + 300)
                {
                    W.Cast(_target.Position);
                }
            }
            else
            {
                if (W.IsReady() && _Player.Distance(_target) < W.Range + 375 && !Q.IsReady())
                {
                    W.Cast(_target.Position);
                }
                else if(W.IsReady() && (_Player.Distance(_target) > W.Range + 375 && _Player.Distance(_target) < Q.Range + 300) && Q.IsReady())
                {
                    W.Cast(_Player.Position.Extend(_target, W.Range - 100).To3D());
                    Core.DelayAction(() => Q.Cast(_target.Position), 250);
                }
            }
        }

        public void CastEandQ()
        {
            E.Cast(_target.Position);
            Core.DelayAction(() => Q.Cast(_target.Position), 75);
        }
    }
}
