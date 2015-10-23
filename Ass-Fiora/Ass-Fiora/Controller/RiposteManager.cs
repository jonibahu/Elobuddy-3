using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ass_Fiora.Model;
using EloBuddy;
using EloBuddy.SDK.Constants;

namespace Ass_Fiora.Controller
{
    static class RiposteManager
    {
        public static void CheckDangerousSpellsIncoming(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            foreach (var spell in Spells)
            {
                if (args.SData.Name != spell) continue;

                if (sender is AIHeroClient && sender.IsEnemy && args.Target.IsMe && !args.SData.IsAutoAttack() &&
                    PluginModel.W.IsReady())
                {
                    Player.CastSpell(SpellSlot.W, args.Start);
                }
            }
        }


        #region Dangerous Spells

        static readonly string[] Spells = {"AhriSeduce"
                                          , "InfernalGuardian"
                                          , "EnchantedCrystalArrow"
                                          , "InfernalGuardian"
                                          , "EnchantedCrystalArrow"
                                          , "RocketGrab"
                                          , "BraumQ"
                                          , "CassiopeiaPetrifyingGaze"
                                          , "DariusAxeGrabCone"
                                          , "DravenDoubleShot"
                                          , "DravenRCast"
                                          , "EzrealTrueshotBarrage"
                                          , "FizzMarinerDoom"
                                          , "GnarBigW"
                                          , "GnarR"
                                          , "GragasR"
                                          , "GravesChargeShot"
                                          , "GravesClusterShot"
                                          , "JarvanIVDemacianStandard"
                                          , "JinxW"
                                          , "JinxR"
                                          , "KarmaQ"
                                          , "KogMawLivingArtillery"
                                          , "LeblancSlide"
                                          , "LeblancSoulShackle"
                                          , "LeonaSolarFlare"
                                          , "LuxLightBinding"
                                          , "LuxLightStrikeKugel"
                                          , "LuxMaliceCannon"
                                          , "UFSlash"
                                          , "DarkBindingMissile"
                                          , "NamiQ"
                                          , "NamiR"
                                          , "OrianaDetonateCommand"
                                          , "RengarE"
                                          , "rivenizunablade"
                                          , "RumbleCarpetBombM"
                                          , "SejuaniGlacialPrisonStart"
                                          , "SionR"
                                          , "ShenShadowDash"
                                          , "SonaR"
                                          , "ThreshQ"
                                          , "ThreshEFlay"
                                          , "VarusQMissilee"
                                          , "VarusR"
                                          , "VeigarBalefulStrike"
                                          , "VelkozQ"
                                          , "Vi-q"
                                          , "Laser"
                                          , "xeratharcanopulse2"
                                          , "XerathArcaneBarrage2"
                                          , "XerathMageSpear"
                                          , "xerathrmissilewrapper"
                                          , "yasuoq3w"
                                          , "ZacQ"
                                          , "ZiggsW"
                                          , "ZiggsE"
                                          , "ZileanQ"
                                          , "ZyraGraspingRoots"
                                      };

        #endregion
    }
}
