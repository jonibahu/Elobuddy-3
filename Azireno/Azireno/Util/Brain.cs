using System.Linq;
using Azireno.Modes;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;

namespace Azireno.Util
{
    class Brain : ModeModel
    {
        public bool CastFly()
        {
            var pos = _Player.Position.Extend(Game.CursorPos, 875).To3D();
            pos = _Player.Distance(Game.CursorPos) > W.Range ? _Player.Position.Shorten(pos, -W.Range) : _Player.Position.Extend(pos, W.Range).To3D();
            W.Cast(pos);
            Core.DelayAction(() => CastQAfterE(_Player.Position.Shorten(_Player.Position.Extend(Game.CursorPos, 875).To3D(), -Q.Range)), 250);
            return true;
        }

        public bool CastFly(Vector3 pos)
        {
            pos = _Player.Position.Extend(pos, 875).To3D();
            pos = _Player.Distance(pos) > W.Range ? _Player.Position.Shorten(pos, -W.Range) : _Player.Position.Extend(pos, W.Range).To3D();
            W.Cast(pos);
            Core.DelayAction(() => CastQAfterE(_Player.Position.Shorten(_Player.Position.Extend(pos, 875).To3D(), -Q.Range)), 250);
            return true;
        }

        public void CastQAfterE(Vector3 pos)
        {
            E.Cast();
            Core.DelayAction(() => Q.Cast(pos), 75);
        }

        public void InsecTarget(Obj_AI_Base from, Obj_AI_Base to)
        {
            var insecToPos = to.Position.To2D();
            var insecPos = from.Position.To2D().Shorten(insecToPos, 100);

            if (!R.IsReady()) return;
            var flash = Player.Spells.FirstOrDefault(a => a.SData.Name == "summonerflash");

            if (insecPos.Distance(_Player) > (flash != null && flash.IsReady ? 1150 : 700))
            {
                Orbwalker.OrbwalkTo(Game.CursorPos);
            }
            else
            {
                if (_Player.Distance(insecPos) < R.Range)
                {
                    Player.CastSpell(SpellSlot.R, insecToPos.To3D());
                }

                if (Misc.isChecked(ComboMenu, "insecFlashForce") && flash != null && flash.IsReady && insecPos.Distance(_Player) < 400)
                {
                    CastFlash(insecPos.To3D());
                }
                else if (!CastFly(insecPos.To3D()))
                {
                    if (insecPos.Distance(_Player) < 400)
                    {
                        CastFlash(insecPos.To3D());
                        if (_Player.Distance(insecPos) < R.Range)
                        {
                            Player.CastSpell(SpellSlot.R, insecToPos.To3D());
                        }
                    }
                }
            }
        }

        public void CastFlash(Vector3 position)
        {
            var flash = Player.Spells.FirstOrDefault(a => a.SData.Name == "summonerflash");
            if (flash != null && position.IsValid() && flash.IsReady)
            {
                Player.CastSpell(flash.Slot, position);
            }
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
                (new float[] { 0, 65, 85, 105, 125, 145 }[ModeModel.Q.Level] + (0.5f * azir.FlatMagicDamageMod))) - target.FlatMagicReduction;
        }

        public static float W(Obj_AI_Base target)
        {
            return 0;
        }
        public static float E(Obj_AI_Base target)
        {
            return azir.CalculateDamageOnUnit(target, DamageType.Magical,
                (new float[] { 0, 60, 90, 120, 150, 180 }[ModeModel.E.Level] + (0.4f * azir.FlatMagicDamageMod))) - target.FlatMagicReduction;
        }
        public static float R(Obj_AI_Base target)
        {
            return azir.CalculateDamageOnUnit(target, DamageType.Magical,
                (new float[] { 0, 150, 225, 300 }[ModeModel.R.Level] + (0.60f * azir.FlatMagicDamageMod))) - target.FlatMagicReduction;
        }
    }
}
