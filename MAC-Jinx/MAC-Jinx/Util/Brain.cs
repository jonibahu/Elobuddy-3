using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using MAC_Jinx.Mode;
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

                    if (predictionR.HitChancePercent >= 65 && !predictionR.Collision)
                        Player.CastSpell(SpellSlot.R, aiHeroClient.ServerPosition);
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
            if (W.IsReady() && _Player.GetAutoAttackRange() + Q.Range < _Player.Distance(_target) && predictionW.HitChance >= HitChance.High
                && !predictionW.Collision)
            {
                W.Cast(predictionW.CastPosition);
            }
        }

        public void AutoSwitchQFarm()
        {
            if (Orbwalker.CanAutoAttack) return;

            if (IsCannon() && PassiveCounter() == 0)
            {
                Q.Cast();
            }
            else if(!IsCannon() && PassiveCounter() == 3)
            {
                Q.Cast();
            }
        }

        public void AutoSwitchQ(Obj_AI_Base target)
        {
            if(Orbwalker.CanAutoAttack) return;
            
            if (IsCannon())
            {
                if (PassiveCounter() == 0 || _Player.Distance(target) < _Player.GetAutoAttackRange() / 2)
                {
                    Q.Cast();
                }
            }
            else
            {
                if (PassiveCounter() > 2 || (_Player.GetAutoAttackRange() < _Player.Distance(target) && _Player.GetAutoAttackRange() + Q.Range < _Player.Distance(target)))
                {
                    Q.Cast();
                }
            }
        }

        public bool IsCannon()
        {
            return _Player.Buffs.Where(o => o.IsValid()).Select(buff => !buff.DisplayName.Contains("con")).FirstOrDefault();
        }

        public bool ManaManager(float maximumPercent)
        {
            return _Player.ManaPercent >= maximumPercent;
        }

        public int PassiveCounter()
        {
            if (_Player.HasBuff("JinxQRamp"))
            {
                return _Player.Buffs.Select(b => b.Name == "JinxQRamp").Count();
            }

            return 0;
        }


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
