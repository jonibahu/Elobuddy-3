using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK.Events;
using Azireno.Modes;
using Azireno.Util;
using System.Drawing;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Azireno.Plugin
{
    class Azir : ModeModel, Champion
    {
        public void GameObjectOnCreate(GameObject sender, EventArgs args)
        {
            throw new NotImplementedException();
        }

        public void GameObjectOnDelete(GameObject sender, EventArgs args)
        {
            throw new NotImplementedException();
        }

        public void OnAfterAttack(AttackableUnit target, EventArgs args)
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

        public void Init()
        {
            Bootstrap.Init(null);

            InitVariables();
            Orbwalker.OnPostAttack += OnAfterAttack;
            Gapcloser.OnGapcloser += OnGapCloser;
            Interrupter.OnInterruptableSpell += OnPossibleToInterrupt;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;

            Game.OnUpdate += OnGameUpdate;
            Drawing.OnDraw += OnDraw;
        }

        public void InitVariables()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 875, SkillShotType.Linear);
            W = new Spell.Active(SpellSlot.W, 450);
            E = new Spell.Active(SpellSlot.E, 1100);
            R = new Spell.Skillshot(SpellSlot.R, 250, SkillShotType.Linear, 250, 500, 400);
            InitMenu();

            if(_Player.Spellbook.GetSpell(SpellSlot.W).Level < 1)
                _Player.Spellbook.LevelSpell(SpellSlot.W); // Azir cannot user other skill lvl 1 so dur
        }

        public void InitMenu()
        {
            
        }
    }
}
