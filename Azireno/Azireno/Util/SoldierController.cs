using System;
using System.Collections.Generic;
using System.Linq;
using Azireno.Modes;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Azireno.Util
{
    static class SoldierController
    {
        public static void AutoPilot(List<Obj_AI_Minion> soldiers, Obj_AI_Base target)
        {
            if (target == null || !target.IsValidTarget()) return;

            var predictedPositions = new Dictionary<int, Tuple<int, PredictionResult>>();
            var predictionQ = ModeModel.Q.GetPrediction(target);
            var predictionW = ModeModel.W.GetPrediction(target);
            var pos = target.ServerPosition;
            pos = ModeModel._Player.Distance(target.ServerPosition) > ModeModel.W.Range ? ModeModel._Player.ServerPosition.Shorten(pos, -ModeModel.W.Range) : ModeModel._Player.ServerPosition.Extend(pos, ModeModel.W.Range).To3D();

            if (soldiers.Count == 0)
            {
                if (ModeModel.W.IsReady() && (ModeModel.W.IsInRange(target) || (ModeModel.Q.State == SpellState.Surpressed || ModeModel.Q.State == SpellState.Ready)))
                {
                    Player.CastSpell(SpellSlot.W,pos);
                }
            }
            else if (soldiers.Count == 1)
            {
                if (soldiers.Count(validAzirSoldier => validAzirSoldier.Distance(target) < Orbwalker.AzirSoldierAutoAttackRange) > 0) return;

                if (ModeModel.W.IsReady() && (ModeModel.W.IsInRange(target) || (ModeModel.Q.State == SpellState.Surpressed || ModeModel.Q.State == SpellState.Ready)))
                {
                    predictedPositions[target.NetworkId] = new Tuple<int, PredictionResult>(Environment.TickCount, predictionW);
                    Player.CastSpell(SpellSlot.W, pos);
                }

                if (ModeModel.Q.IsReady() && predictionQ.HitChance >= HitChance.Medium)
                {
                    predictedPositions[target.NetworkId] = new Tuple<int, PredictionResult>(Environment.TickCount, predictionW);
                    Player.CastSpell(SpellSlot.Q, predictionQ.CastPosition.Extend(predictionQ.UnitPosition, 115).To3D());
                }
            }
            else if (soldiers.Count == 2)
            {
                if (soldiers.Count(validAzirSoldier => validAzirSoldier.Distance(target) < Orbwalker.AzirSoldierAutoAttackRange) >= 1) return;

                if (ModeModel.Q.IsReady() && predictionQ.HitChance >= HitChance.Medium)
                {
                    predictedPositions[target.NetworkId] = new Tuple<int, PredictionResult>(Environment.TickCount,
                        predictionW);
                    Player.CastSpell(SpellSlot.Q, predictionQ.CastPosition.Extend(predictionQ.UnitPosition, 115).To3D());
                }
            }
            else
            {
                if (soldiers.Count(validAzirSoldier => validAzirSoldier.Distance(target) < Orbwalker.AzirSoldierAutoAttackRange) >= 2) return;

                if (ModeModel.Q.IsReady() && predictionQ.HitChance >= HitChance.Medium)
                {
                    predictedPositions[target.NetworkId] = new Tuple<int, PredictionResult>(Environment.TickCount,
                        predictionW);
                    Player.CastSpell(SpellSlot.Q, predictionQ.CastPosition);
                }
            }


        }


    }
}
