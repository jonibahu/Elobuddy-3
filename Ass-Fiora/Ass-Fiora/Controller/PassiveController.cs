using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;

namespace Ass_Fiora.Controller
{
    public class PassiveController
    {
        private static readonly List<string> FioraPassiveName = new List<string>()
        {
            "Fiora_Base_Passive_NE.troy",
            "Fiora_Base_Passive_SE.troy",
            "Fiora_Base_Passive_NW.troy",
            "Fiora_Base_Passive_SW.troy",
            "Fiora_Base_Passive_NE_Timeout.troy",
            "Fiora_Base_Passive_SE_Timeout.troy",
            "Fiora_Base_Passive_NW_Timeout.troy",
            "Fiora_Base_Passive_SW_Timeout.troy",
        };

        public static Vector3 PassivePosition(Obj_AI_Base target)
        {
            var passive = FioraPassiveObjects.FirstOrDefault(x => x.Position.Distance(target.Position) <= 50);
            var position = Prediction.Position.PredictUnitPosition(target, 250);
            if (passive == null) return new Vector3();

            if (passive.Name.Contains("NE"))
            {
                var pos = new Vector2
                {
                    X = position.X,
                    Y = position.Y + 150
                };
                return pos.To3D();
            }
            if (passive.Name.Contains("SE"))
            {
                var pos = new Vector2
                {
                    X = position.X - 150,
                    Y = position.Y
                };
                return pos.To3D();
            }
            if (passive.Name.Contains("NW"))
            {
                var pos = new Vector2
                {
                    X = position.X + 150,
                    Y = position.Y
                };
                return pos.To3D();
            }
            if (passive.Name.Contains("SW"))
            {
                var pos = new Vector2
                {
                    X = position.X,
                    Y = position.Y - 150
                };
                return pos.To3D();
            }
            return new Vector3();
        }

        public static List<Vector3> UltiPassivePos(Obj_AI_Base target)
        {
            var poses = new List<Vector3>();

            var passive = ObjectManager.Get<Obj_GeneralParticleEmitter>()
                .Where(x => x.Name.Contains("Fiora_Base_R_Mark") || (x.Name.Contains("Fiora_Base_R") && x.Name.Contains("Timeout_FioraOnly.troy")));

            var position = Prediction.Position.PredictUnitPosition(target, 250);

            foreach (var x in passive)
            {
                if (x.Name.Contains("NE"))
                {
                    var pos = new Vector2
                    {
                        X = position.X,
                        Y = position.Y + 150
                    };
                    poses.Add(pos.To3D());
                }
                else if (x.Name.Contains("SE"))
                {
                    var pos = new Vector2
                    {
                        X = position.X - 150,
                        Y = position.Y
                    };
                    poses.Add(pos.To3D());
                }
                else if (x.Name.Contains("NW"))
                {
                    var pos = new Vector2
                    {
                        X = position.X + 150,
                        Y = position.Y
                    };
                    poses.Add(pos.To3D());
                }
                else if (x.Name.Contains("SW"))
                {
                    var pos = new Vector2
                    {
                        X = position.X,
                        Y = position.Y - 150
                    };
                    poses.Add(pos.To3D());
                }
            }

            return poses;
        }

        public static bool HasUltiPassive(GameObject target)
        {
            return ObjectManager.Get<Obj_GeneralParticleEmitter>()
                .Where(x => x.Name.Contains("Fiora_Base_R_Mark") || (x.Name.Contains("Fiora_Base_R") && x.Name.Contains("Timeout_FioraOnly.troy"))).Any(x => x.Position.Distance(target.Position) <= 50);
        }

        public static bool HasPassive(Obj_AI_Base target)
        {
            return FioraPassiveObjects.Any(x => x.Position.Distance(target.Position) <= 50);
        }

        private static IEnumerable<Obj_GeneralParticleEmitter> FioraPassiveObjects
        {
            get
            {
                var x = ObjectManager.Get<Obj_GeneralParticleEmitter>().Where(a => FioraPassiveName.Contains(a.Name)).ToList();
                return x;
            }
        }

        public static List<Vector3> PassiveRadiusPoint(Obj_AI_Base target)
        {
            var passive = FioraPassiveObjects.FirstOrDefault(x => x.Position.Distance(target.Position) <= 50);
            var position = Prediction.Position.PredictUnitPosition(target, 250);
            if (passive == null) return new List<Vector3>();

            if (passive.Name.Contains("NE"))
            {
                var pos1 = new Vector2
                {
                    X = position.X + 150/(float) Math.Sqrt(2),
                    Y = position.Y + 150/(float) Math.Sqrt(2)
                };

                var pos2 = new Vector2
                {
                    X = position.X - 150/(float) Math.Sqrt(2),
                    Y = position.Y + 150/(float) Math.Sqrt(2)
                };

                return new List<Vector3>() { pos1.To3D(), pos2.To3D() };
            }
            if (passive.Name.Contains("SE"))
            {
                var pos1 = new Vector2
                {
                    X = position.X - 150/(float) Math.Sqrt(2),
                    Y = position.Y - 150/(float) Math.Sqrt(2)
                };

                var pos2 = new Vector2
                {
                    X = position.X - 150/(float) Math.Sqrt(2),
                    Y = position.Y + 150/(float) Math.Sqrt(2)
                };

                return new List<Vector3>() { pos1.To3D(), pos2.To3D() };
            }
            if (passive.Name.Contains("NW"))
            {
                var pos1 = new Vector2
                {
                    X = position.X + 150/(float) Math.Sqrt(2),
                    Y = position.Y - 150/(float) Math.Sqrt(2)
                };

                var pos2 = new Vector2
                {
                    X = position.X + 150/(float) Math.Sqrt(2),
                    Y = position.Y + 150/(float) Math.Sqrt(2)
                };

                return new List<Vector3>() { pos1.To3D(), pos2.To3D() };
            }
            if (passive.Name.Contains("SW"))
            {
                var pos1 = new Vector2
                {
                    X = position.X + 150/(float) Math.Sqrt(2),
                    Y = position.Y - 150/(float) Math.Sqrt(2)
                };
                var pos2 = new Vector2
                {
                    X = position.X - 150/(float) Math.Sqrt(2),
                    Y = position.Y - 150/(float) Math.Sqrt(2)
                };

                return new List<Vector3>() { pos1.To3D(), pos2.To3D() };
            }
            return new List<Vector3>();
        }

        public static int GetPassiveCount(Obj_AI_Base target)
        {
            var count = FioraPassiveObjects.Count(x => x.Position.Distance(target.Position) <= 50);

            return count;
        }

        public static double GetPassiveDamage(Obj_AI_Base target, int passiveCount)
        {
            return passiveCount * (.03f + Math.Min(Math.Max(.028f, (.027 + .001f * ObjectManager.Player.Level * ObjectManager.Player.FlatPhysicalDamageMod / 100f)), .45f)) * target.MaxHealth;
        }
    }
}
