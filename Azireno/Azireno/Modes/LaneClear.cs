using System.Linq;
using Azireno.Util;
using EloBuddy.SDK;

namespace Azireno.Modes
{
    class LaneClear : ModeModel
    {
        /*
            Stoled from Kk2 steal Kappa
            */
        public void Execute()
        {
            if (!Misc.isChecked(LaneClearMenu, "lcSoldiers")) return;

            var minion = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, _Player.Position,
                W.Range);

            if (minion == null || minion.Count == 0) return;

            var m = minion.FirstOrDefault();

            if (m == null || m.Name.ToLower().Contains("ward")) return;

            var bestFarm =
                Misc.GetBestCircularFarmLocation(
                    EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.Distance(_Player) <= W.Range)
                        .Select(xm => xm.ServerPosition.To2D())
                        .ToList(), Orbwalker.AzirSoldierAutoAttackRange, W.Range);

            if (Orbwalker.ValidAzirSoldiers.Count > 0)
            {
                foreach (var validAzirSoldier in Orbwalker.ValidAzirSoldiers)
                {
                    if (validAzirSoldier.Distance(bestFarm.Position) < 50)
                    {
                        return;
                    }
                    else if (Orbwalker.ValidAzirSoldiers.Count >= 2)
                    {
                        Q.Cast(m);
                        return;
                    }
                }
            }

            W.Cast(bestFarm.Position.To3D());
        }
    }
}
