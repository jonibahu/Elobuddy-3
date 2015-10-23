using System.Collections.Generic;
using Ass_Fiora.Model.Enum;

namespace Ass_Fiora.Model
{
    abstract class LanguageController
    {
        public Dictionary<EnumContext, string> Dictionary = new Dictionary<EnumContext, string>();
    }
}
