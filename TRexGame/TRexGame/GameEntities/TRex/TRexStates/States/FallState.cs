using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

namespace TRexGame.GameEntities.TRex.TRexStates.States
{
    public class FallState : TRexState
    {
        #region CONSTRUCTOR
        public FallState(ETRexState key, TRex context) : base(key, context) { }
        #endregion

        #region FIELDS
        bool _checkGround = false;
        #endregion

        #region PRIVATE METHODS
        private void ResetState()
        {
            _checkGround = false;
        }
        #endregion

        #region STATE PATTERN
        public override void Enter()
        {
            base.Enter();
            ResetState();

            _context.Animator.State = StateKey;
        }

        public override ETRexState CheckTransitions()
        {
            if (_checkGround)
            {
                return ETRexState.RUN;
            }

            return StateKey;
        }

        public override void Update()
        {
            base.Update();

            if (_context.RectTransform.Position.Y >= _context.Rigidbody.GroundPos)
            {
                _checkGround = true;
            }
        }
        #endregion
    }
}
