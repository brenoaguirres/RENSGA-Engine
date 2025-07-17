using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TRexGame.Engine.Animation;
using TRexGame.Engine.Graphics;
using TRexGame.Engine.Entities;
using System;
using System.Drawing;
using TRexGame.Engine.Tools;

namespace TRexGame.GameEntities.TRex
{
    public class TRexAnimator : Animator
    {
        #region CONSTRUCTOR
        public TRexAnimator(Texture2D texture) : base (new TRexGraphics(texture))
        {
        }
        #endregion

        #region FIELDS
        private ETRexState _state;

        // idle
        private bool _defaultIdle = true;
        private float _idleReset = 2f;
        private float _idleTimer = 0f;

        // jump
        private bool _jumpStart = false;

        #endregion

        #region PROPERTIES
        public ETRexState State 
        { 
            get => _state; 
            set
            {
                if (_state != value)
                {
                    StateChange(value);
                }
                _state = value;
            }
        }
        #endregion

        #region PRIVATE METHODS
        private void AnimateIdleState(TRexGraphics graphics)
        {
            if (_idleTimer <= 0)
            {
                if (!_defaultIdle)
                {
                    Play(graphics.IdleAnimation);
                    _defaultIdle = true;
                }
                else
                {
                    Play(graphics.BlinkAnimation);
                    _defaultIdle = false;
                }

                _idleTimer = _idleReset;
            }
            else
            {
                _idleTimer -= (float)Time.DeltaTime;
            }
        }
        private void AnimateRunState(TRexGraphics graphics)
        {
            if (!IsPlaying || CurrentAnimation != graphics.RunAnimation) 
                Play(graphics.RunAnimation);
        }
        private void AnimateJumpState(TRexGraphics graphics)
        {
            if (!_jumpStart)
            {
                BeginJumpAnimation(graphics);
                return;
            }
            ContinueJumpAnimation(graphics);
        }
        private void AnimateDuckState(TRexGraphics graphics)
        {
            if (!IsPlaying || CurrentAnimation != graphics.DuckAnimation)
                Play(graphics.DuckAnimation);
        }
        private void AnimateFallState(TRexGraphics graphics)
        {
            Play(graphics.FallAnimation);
            _jumpStart = false;
        }
        #endregion

        #region ANIMATION HELPERS
        private void BeginJumpAnimation(TRexGraphics graphics)
        {
            _jumpStart = true;
            Play(graphics.BeginJumpAnimation);
        }

        private void ContinueJumpAnimation(TRexGraphics graphics)
        {
            if (_jumpStart && !IsPlaying)
                Play(graphics.JumpAnimation);
        }
        #endregion

        #region STATE CHANGE
        private void StateChange(ETRexState state)
        {
            switch (state)
            {
                default:
                case ETRexState.IDLE:
                    _idleTimer = 0;
                    break;
                case ETRexState.RUN:
                    break;
                case ETRexState.JUMP:
                    break;
                case ETRexState.DUCK:
                    break;
                case ETRexState.FALL:
                    break;
            }
        }
        #endregion

        #region PUBLIC METHODS
        public override void UpdateStates()
        {
            TRexGraphics graphics = Graphics as TRexGraphics;

            switch (State)
            {
                default:
                case ETRexState.IDLE:
                    AnimateIdleState(graphics);
                    break;
                case ETRexState.RUN:
                    AnimateRunState(graphics);
                    break;
                case ETRexState.JUMP:
                    AnimateJumpState(graphics);
                    break;
                case ETRexState.DUCK:
                    AnimateDuckState(graphics);
                    break;
                case ETRexState.FALL:
                    AnimateFallState(graphics);
                    break;
            }
        }
        #endregion
    }
}
