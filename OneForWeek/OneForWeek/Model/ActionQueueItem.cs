using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneForWeek.Model
{
    using System;

    struct ActionQueueItem
    {
        public float Time;
        public Func<bool> PreConditionFunc;
        public Func<bool> ConditionToRemoveFunc;
        public Action ComboAction;
    }
}
