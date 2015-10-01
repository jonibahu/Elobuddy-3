using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using Mech_Viktor.Plugin;
using System;

namespace Mech_Viktor.Util
{
    static class Misc
    {
        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static bool isChecked(Menu obj, String value)
        {
            return obj[value].Cast<CheckBox>().CurrentValue;
        }

        public static int getSliderValue(Menu obj, String value)
        {
            return obj[value].Cast<Slider>().CurrentValue;
        }   
    }

    class DmgLib
    {

        private static AIHeroClient viktor
        {
            get { return ObjectManager.Player; }
        }

        public static float possibleDamage(Obj_AI_Base target, bool usingUltimate = true)
        {
            var damage = 0f;
            if (Viktor.Q.IsReady())
                damage += Q(target);
            if (Viktor.E.IsReady())
                damage += Q(target);
            if (Viktor.R.IsReady() && usingUltimate)
                damage += Q(target);

            return 0;
        }

        public static float Q(Obj_AI_Base target)
        {
            return viktor.CalculateDamageOnUnit(target, DamageType.Magical,
                (new float[] { 0, 40, 60, 80, 100, 120 }[Viktor.Q.Level] + (0.2f * viktor.FlatMagicDamageMod)))
                +
                viktor.CalculateDamageOnUnit(target, DamageType.Magical,
                (((20f + 10.5f) * viktor.Level) + (0.5f * viktor.FlatMagicDamageMod) + (1.0f * viktor.FlatPhysicalDamageMod)));
        }

        public static float W(Obj_AI_Base target)
        {
            return 0;
        }
        public static float E(Obj_AI_Base target)
        {
            return viktor.CalculateDamageOnUnit(target, DamageType.Magical,
                (new float[] { 0, 70, 115, 160, 205, 250 }[Viktor.E.Level] + (0.70f * viktor.FlatMagicDamageMod)));
        }
        public static float R(Obj_AI_Base target)
        {
            return viktor.CalculateDamageOnUnit(target, DamageType.Magical,
                (new float[] { 0, 150, 250, 350 }[Viktor.R.Level] + (0.55f * viktor.FlatMagicDamageMod)));
        }
    }
}
