using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace Vayne.Util
{
    public static class Checkers
    {

        /*
         * Stoled from flux Kappa
         * to lazy to make my own e.e
         */

        public static long LastCheck;
        public static int CheckCount;
        public static Spell.Skillshot ESpell;

        public static bool IsCondemable(this AIHeroClient unit, Vector2 pos = new Vector2())
        {
            if (unit.HasBuffOfType(BuffType.SpellImmunity) || unit.HasBuffOfType(BuffType.SpellShield) || LastCheck + 50 > Environment.TickCount || Globals._Player.IsDashing()) return false;
            var prediction = ESpell.GetPrediction(unit);
            var predictionsList = pos.IsValid() ? new List<Vector3>() { pos.To3D() } : new List<Vector3>
                        {
                            unit.ServerPosition,
                            unit.Position,
                            prediction.CastPosition,
                            prediction.UnitPosition
                        };

            var wallsFound = 0;
            Program.Points = new List<Vector2>();
            foreach (var position in predictionsList)
            {
                for (var i = 0; i < (470 - Program.getSliderValue(Program.ComboMenu, "condenmErrorMargin")); i += (int)unit.BoundingRadius)
                {
                    var cPos = Globals._Player.Position.Extend(position, Globals._Player.Distance(position) + i).To3D();
                    Program.Points.Add(cPos.To2D());
                    if (cPos.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Wall) || cPos.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Building))
                    {
                        wallsFound++;
                        break;
                    }
                }
            }
            if ((wallsFound / predictionsList.Count) >= 60 / 100f)
            {
                return true;
            }

            return false;
        }

        public static Vector2 GetFirstNonWallPos(Vector2 startPos, Vector2 endPos)
        {
            int distance = 0;
            for (int i = 0; i < (470 - Program.getSliderValue(Program.ComboMenu, "condenmErrorMargin")); i++)
            {
                var cell = startPos.Extend(endPos, endPos.Distance(startPos) + i).ToNavMeshCell().CollFlags;
                if (cell.HasFlag(CollisionFlags.Wall) || cell.HasFlag(CollisionFlags.Building))
                {
                    distance = i - 20;
                }
            }
            return startPos.Extend(endPos, distance + endPos.Distance(startPos));
        }

    }
}
