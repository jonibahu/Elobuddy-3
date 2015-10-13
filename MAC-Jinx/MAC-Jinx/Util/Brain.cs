using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using MAC_Jinx.Mode;
using MAC_Jinx.Plugin;
using SharpDX;

namespace MAC_Jinx.Util
{
    class Brain : ModeModel
    {
        public void RFinisher()
        {
            var target = EntityManager.Heroes.Enemies;

            foreach (var aiHeroClient in target)
            {
                if (!aiHeroClient.IsValidTarget()) break;

                if (aiHeroClient.Health < DmgLib.R(aiHeroClient) && _Player.Distance(aiHeroClient) < 5000 && _Player.Distance(aiHeroClient) > Misc.GetSliderValue(ComboMenu, "minRangeR"))
                {
                    var predictionR = R.GetPrediction(aiHeroClient);
                    R.AllowedCollisionCount = int.MaxValue;

                    if(!predictionR.Collision)
                        Player.CastSpell(SpellSlot.R, predictionR.CastPosition);
                }
            }
        }

        public void ECast(Obj_AI_Base target)
        {
            var predictionE = E.GetPrediction(_target);
            if (predictionE.HitChancePercent >= 70 || predictionE.HitChance == HitChance.Immobile)
            {
                E.Cast(predictionE.CastPosition);
            }
        }

        public void WCast(Obj_AI_Base target)
        {
            var predictionW = W.GetPrediction(_target);
            if (W.IsReady() && _Player.GetAutoAttackRange() < _Player.Distance(_target) && predictionW.HitChancePercent >= 70
                && !predictionW.Collision)
            {
                W.Cast(predictionW.CastPosition);
            }
        }

        public void AutoSwitchQFarm()
        {
            /*if (Orbwalker.CanAutoAttack) return;

            Console.WriteLine();

            if (IsCannon() && PassiveCounter() == 0)
            {
                Q.Cast();
            }
            else if(!IsCannon() && PassiveCounter() == 3)
            {
                Q.Cast();
            }*/

            if (Orbwalker.CanAutoAttack) return;

            var minions = EntityManager.MinionsAndMonsters.EnemyMinions;

            if (minions == null || minions.Count() == 0) return;

            var killableminions = minions.Count(objAiMinion => objAiMinion.Health < _Player.GetAutoAttackRange(objAiMinion) && objAiMinion.Distance(_Player) < _Player.GetAutoAttackRange());

            if (killableminions > Misc.GetSliderValue(LaneClearMenu, "minMinionsForSwitch") && !IsCannon())
            {
                Q.Cast();
            }
            else if (IsCannon())
            {
                Q.Cast();
            }
        }

        public void AutoSwitchQ(Obj_AI_Base target)
        {
            if(Orbwalker.CanAutoAttack || !Orbwalker.GetTarget().IsValidTarget() || !Q.IsReady()) return;

            var obwTarget = Orbwalker.GetTarget();
            var enemiesNear = Orbwalker.GetTarget().CountEnemiesInRange(AoeRadius);

            if(obwTarget.Distance(_Player) > _Player.GetAutoAttackRange() && !IsCannon())
            {
                Q.Cast();
                return;
            }

            if(obwTarget.Distance(_Player) < 525 && IsCannon() && enemiesNear <= 1)
            {
                Q.Cast();
                return;
            }

            if(!IsCannon() && enemiesNear >= 2)
            {
                Q.Cast();
                return;
            }

            if(IsCannon() && _Player.CountEnemiesInRange(2000) == 0)
            {
                Q.Cast();
            }

        }

        public bool IsCannon()
        {
             return _Player.AttackRange > 525;
        }

        public bool ManaManager(float maximumPercent)
        {
            return _Player.ManaPercent >= maximumPercent;
        }

        public int PassiveCounter()
        {
           return Jinx.qPassiveCount;
        }

        public static float FishBonesBonus
        {
            get { return 75f + 25f * Q.Level; }
        }

        public const int AoeRadius = 200;

    }

    static class DmgLib
    {

        private static AIHeroClient Jinx
        {
            get { return ObjectManager.Player; }
        }

        public static float PossibleDamage(Obj_AI_Base target, bool usingUltimate = true)
        {
            var damage = 0f;
            if (ModeModel.R.IsReady() && usingUltimate)
                damage += R(target);

            return damage;
        }

        public static float R(Obj_AI_Base target)
        {
            return Jinx.CalculateDamageOnUnit(target, DamageType.Physical,
                (target.MaxHealth - target.Health * new[] { 0, 0.25f, 0.30f, 0.45f }[ModeModel.R.Level]) - target.FlatPhysicalReduction);
        }
        
    }
}
