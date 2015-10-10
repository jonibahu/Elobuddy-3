using Azireno.Util;

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
