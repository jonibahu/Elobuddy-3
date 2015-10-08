using Azireno.Modes;
using EloBuddy;
using EloBuddy.SDK.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azireno.Plugin
{
    interface Champion
    {
        void Init();
        void InitVariables();
        void InitMenu();
        void OnCombo();
        void OnLaneClear();
        void OnGameUpdate(EventArgs args);
        void OnDraw(EventArgs args);
        void OnAfterAttack(AttackableUnit target, EventArgs args);
        void OnPossibleToInterrupt(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs interruptableSpellEventArgs);
        void OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e);
        void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args);
        void GameObjectOnCreate(GameObject sender, EventArgs args);
        void GameObjectOnDelete(GameObject sender, EventArgs args);
    }
}
