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
using EloBuddy.SDK.Rendering;

namespace Azireno.Plugin
{
    class Azir : ModeModel, Champion
    {
        static Combo combo = new Combo();
        static Flee flee = new Flee();
        static Harass harass = new Harass();
        static LaneClear laneClear = new LaneClear();

        public static float lastFlee = 0f;

        public void GameObjectOnCreate(GameObject sender, EventArgs args){}

        public void GameObjectOnDelete(GameObject sender, EventArgs args) { }

        public void OnAfterAttack(AttackableUnit target, EventArgs args) { }

        public void OnCombo()
        {
            combo.Execute();
        }

        public void OnLaneClear()
        {
            laneClear.Execute();
        }

        public void OnHarass()
        {
            harass.Execute();
        }

        public void OnFlee()
        {
            if(E.State == SpellState.Ready || E.State == SpellState.Surpressed)
                flee.Execute();
        }

        public void OnDraw(EventArgs args)
        {
            if(_target == null || !_target.IsValidTarget()) return;
            var finisherPos = _Player.Position.Extend(_target, _Player.Distance(_target) + 100).To3D();

            Circle.Draw(SharpDX.Color.Red, 60, finisherPos);
        }

        public void OnGameUpdate(EventArgs args)
        {
            _target = TargetSelector.GetTarget(875, DamageType.Magical);
            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    OnCombo();
                    break;
                case Orbwalker.ActiveModes.Flee:
                    OnFlee();
                    break;
                case Orbwalker.ActiveModes.Harass:
                    OnHarass();
                    break;
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                OnLaneClear();

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)){}

        }

        public void OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e) { }

        public void OnPossibleToInterrupt(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs interruptableSpellEventArgs) { }

        public void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args) { }

        public void Init()
        {
            Bootstrap.Init(null);

            Console.WriteLine("Azir injected!");

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
            W = new Spell.Skillshot(SpellSlot.W, 450, SkillShotType.Circular);
            E = new Spell.Active(SpellSlot.E, 1100);
            R = new Spell.Skillshot(SpellSlot.R, 250, SkillShotType.Linear);
            InitMenu();

            if(_Player.Spellbook.GetSpell(SpellSlot.W).Level < 1)
                _Player.Spellbook.LevelSpell(SpellSlot.W); // Azir cannot user other skill lvl 1 so dur
        }

        public void InitMenu()
        {
            
        }
    }
}
