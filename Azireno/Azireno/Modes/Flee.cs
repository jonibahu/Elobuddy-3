using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azireno.Plugin;
using Azireno.Util;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;

namespace Azireno.Modes
{
    class Flee : ModeModel
    {
        public void Execute()
        {
            new Brain().CastFly();
        }
    }
}
