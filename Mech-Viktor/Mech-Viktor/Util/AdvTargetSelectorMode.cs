using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mech_Viktor.Misc
{
    public enum AdvTargetSelectorMode
    {
        /// <summary>
        ///     Focuses targets based on how many auto attacks it takes to kill the units.
        /// </summary>
        LessAttacksToKill = 0,

        /// <summary>
        ///     Focuses targets based on the amount of AP they have.
        /// </summary>
        MostAbilityPower = 1,

        /// <summary>
        ///     Focuses targets based on the amount of AD they have.
        /// </summary>
        MostAttackDamage = 2,

        /// <summary>
        ///     Focuses targets based on the distance between the player and target.
        /// </summary>
        Closest = 3,

        /// <summary>
        ///     Focuses targets base on the distance between the target and the mouse.
        /// </summary>
        NearMouse = 4,

        /// <summary>
        ///     Focuses targets by their class.
        /// </summary>
        AutoPriority = 5,

        /// <summary>
        ///     Focuses targets by their health.
        /// </summary>
        LeastHealth = 6
    }
}

