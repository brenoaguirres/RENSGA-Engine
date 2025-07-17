using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using TRexGame.GameEntities.TRex.TRexStates.States;

namespace TRexGame.GameEntities.TRex.TRexStates
{
    public class TRexStateMachine
    {
        #region CONSTRUCTOR
        public TRexStateMachine(TRex trex)
        {
            _context = trex;
            GenerateStates();
            SwitchStates(ETRexState.IDLE);
        }
        #endregion

        #region FIELDS
        private TRex _context;
        private Dictionary<ETRexState, TRexState> _states = new();
        #endregion

        #region PROPERTIES
        public TRexState CurrentState { get; private set; }
        #endregion

        #region PRIVATE METHODS
        private void GenerateStates()
        {
            _states.Add(ETRexState.IDLE, new IdleState(ETRexState.IDLE, _context));
            _states.Add(ETRexState.RUN, new RunState(ETRexState.RUN, _context));
            _states.Add(ETRexState.JUMP, new JumpState(ETRexState.JUMP, _context));
            _states.Add(ETRexState.DUCK, new DuckState(ETRexState.DUCK, _context));
            _states.Add(ETRexState.FALL, new FallState(ETRexState.FALL, _context));
        }
        #endregion

        #region PUBLIC METHODS
        public void SwitchStates(ETRexState state)
        {
            if (CurrentState != null)
                CurrentState.Exit();
            CurrentState = _states[state];
            CurrentState.Enter();

        }
        public void UpdateStateMachine()
        {
            ETRexState state = CurrentState.CheckTransitions();
            if (state != CurrentState.StateKey)
            {
                SwitchStates(state);
                return;
            }
            
            CurrentState.Update();
        }
        #endregion
    }
}
