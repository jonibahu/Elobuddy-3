using Azireno.Plugin;
using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azireno.Modes;
using SharpDX;

namespace Azireno.Util
{
    class Brain : ModeModel
    {
        public void CastFly()
        {
            var pos = _Player.Position.Extend(Game.CursorPos, 875).To3D();
            pos = _Player.Distance(Game.CursorPos) > W.Range ? _Player.Position.Shorten(pos, -W.Range) : _Player.Position.Extend(pos, W.Range).To3D();
            W.Cast(pos);
            Core.DelayAction(() => CastQAfterE(_Player.Position.Shorten(_Player.Position.Extend(Game.CursorPos, 875).To3D(), -Q.Range)), 250);
        }

        public void CastFly(Vector3 pos)
        {
            if (!Q.IsReady() || !W.IsReady() || !E.IsReady()) return;
            pos = _Player.Distance(Game.CursorPos) > W.Range ? _Player.Position.Shorten(pos, -W.Range) : _Player.Position.Extend(pos, W.Range).To3D();
            W.Cast(pos);
            Core.DelayAction(() => CastQAfterE(_Player.Position.Shorten(_Player.Position.Extend(Game.CursorPos, 875).To3D(), -Q.Range)), 250);
        }

        public void CastQAfterE(Vector3 pos)
        {
            E.Cast();
            Core.DelayAction(() => Q.Cast(pos), 75);
        }
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
            if (ModeModel.Q.IsReady())
                damage += Q(target);
            if (ModeModel.E.IsReady())
                damage += E(target);
            if (ModeModel.R.IsReady() && usingUltimate)
                damage += R(target);

            return damage;
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
