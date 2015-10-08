using Azireno.Plugin;
using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azireno.Util
{
    class Brain
    {

    }

    static class DmgLib
    {

        private static AIHeroClient azir
        {
            get { return ObjectManager.Player; }
        }

        public static float possibleDamage(Obj_AI_Base target, bool usingUltimate = true)
        {
            var damage = 0f;
            if (Modes.ModeModel.Q.IsReady())
                damage += Q(target);
            if (Modes.ModeModel.E.IsReady())
                damage += E(target);
            if (Modes.ModeModel.R.IsReady() && usingUltimate)
                damage += R(target);
            return 0;
        }

        public static float Q(Obj_AI_Base target)
        {
            return azir.CalculateDamageOnUnit(target, DamageType.Magical,
                (new float[] { 0, 65, 85, 105, 125, 145 }[Modes.ModeModel.Q.Level] + (0.5f * azir.FlatMagicDamageMod))) - target.FlatMagicReduction;
        }

        public static float W(Obj_AI_Base target)
        {
            return 0;
        }
        public static float E(Obj_AI_Base target)
        {
            return azir.CalculateDamageOnUnit(target, DamageType.Magical,
                (new float[] { 0, 60, 90, 120, 150, 180 }[Modes.ModeModel.E.Level] + (0.4f * azir.FlatMagicDamageMod))) - target.FlatMagicReduction;
        }
        public static float R(Obj_AI_Base target)
        {
            return azir.CalculateDamageOnUnit(target, DamageType.Magical,
                (new float[] { 0, 150, 225, 300 }[Modes.ModeModel.R.Level] + (0.60f * azir.FlatMagicDamageMod))) - target.FlatMagicReduction;
        }
    }
}
