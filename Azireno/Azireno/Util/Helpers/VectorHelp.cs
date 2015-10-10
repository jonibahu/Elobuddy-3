using System;
using SharpDX;

namespace Azireno.Util.Helpers
{
    static class VectorHelp
    {
        public static bool Between(this Vector3 checkPos, Vector3 source, Vector3 destination)
        {
            return Math.Abs(((source.X * checkPos.Y) + (source.Y * destination.X) + (checkPos.X * destination.Y)) - ((checkPos.Y * destination.X) + (source.X * destination.Y) + (source.Y * checkPos.X))) < 5;
        }
    }
}
