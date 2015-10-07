using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK.Events;
using Azireno.Modes;

namespace Azireno.Plugin
{
    class Azir : ModeModel, Champion
    {
        public void Init()
        {
            throw new NotImplementedException();
        }

        public void InitVariables()
        {
            throw new NotImplementedException();
        }

        public void GameObjectOnCreate(GameObject sender, EventArgs args)
        {
            throw new NotImplementedException();
        }

        public void GameObjectOnDelete(GameObject sender, EventArgs args)
        {
            throw new NotImplementedException();
        }

        public void OnAfterAutoAttack(AttackableUnit target, EventArgs args)
        {
            throw new NotImplementedException();
        }

        public void OnCombo()
        {
            throw new NotImplementedException();
        }

        public void OnDraw(EventArgs args)
        {
            throw new NotImplementedException();
        }

        public void OnGameUpdate(EventArgs args)
        {
            throw new NotImplementedException();
        }

        public void OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnLaneClear()
        {
            throw new NotImplementedException();
        }

        public void OnPossibleToInterrupt(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs interruptableSpellEventArgs)
        {
            throw new NotImplementedException();
        }

        public void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
