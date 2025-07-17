using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TRexGame.Engine.Entities;
using TRexGame.Engine.Graphics;
using TRexGame.Engine.Tools;
using TRexGame.GameEntities.TRex;
using Entities = TRexGame.Engine.Entities;

namespace TRexGame.Engine.Animation
{
    public abstract class Animator : Entities.IGameComponent
    {
        #region CONSTRUCTOR
        public Animator(IGameGraphics graphics)
        {
            Graphics = graphics;
        }
        #endregion

        #region FIELDS
        private GameEntity _myGameEntity;
        private SpriteRenderer _myRenderer;
        #endregion

        #region PROPERTIES
        public GameEntity MyGameEntity { get { return _myGameEntity; } set { _myGameEntity = value; } }
        public IGameGraphics Graphics { get; protected set; }
        public AnimationClip DefaultAnimation => Graphics.DefaultAnimation;
        public AnimationClip CurrentAnimation { get; protected set; }

        public bool IsPlaying = false;
        public float PlaybackTime { get; protected set; }
        #endregion

        #region EVENTS
        public Action<AnimationClip> OnAnimationEnter;
        public Action<AnimationClip> OnAnimationExit;
        #endregion

        #region PUBLIC METHODS
        public virtual void UpdateStates()
        {

        }
        public void UpdateAnimator()
        {
            UpdateStates();
            ValidateDefaultAnimation();

            if (IsPlaying)
            {
                if (PlaybackTime >= CurrentAnimation.ClipDuration)
                {
                    if (!CurrentAnimation.LoopAnimation)
                        Stop();
                    else
                    {
                        _myRenderer.Sprite = CurrentAnimation.NextSprite();
                        Play(CurrentAnimation);
                    }
                }

                if (PlaybackTime >= CurrentAnimation.CurrentFrameTimestamp + CurrentAnimation.TotalFrameTime)
                    _myRenderer.Sprite = CurrentAnimation.NextSprite();

                PlaybackTime += (float)Time.DeltaTime;
            }
        }

        public void Play(AnimationClip animationClip)
        {
            if (animationClip == null)
                throw new ArgumentNullException(nameof(animationClip));

            if (CurrentAnimation != null && CurrentAnimation != animationClip)
                Stop();

            CurrentAnimation = animationClip;
            PlaybackTime = 0;
            CurrentAnimation.ResetClip();
            IsPlaying = true;
            OnAnimationEnter?.Invoke(CurrentAnimation);
            _myRenderer.Sprite = CurrentAnimation.CurrentFrameSprite;
        }

        public void Pause()
        {
            IsPlaying = false;
        }

        public void Default()
        {
            Stop();
            Play(DefaultAnimation);
        }

        public void Stop()
        {
            if (CurrentAnimation == null) return;

            IsPlaying = false;
            PlaybackTime = 0;
            CurrentAnimation.ResetClip();
            OnAnimationExit?.Invoke(CurrentAnimation);
        }

        public void ValidateDefaultAnimation()
        {
            if (CurrentAnimation == null)
            {
                Play(DefaultAnimation);
                return;
            }
        }
        #endregion

        #region IGameComponent INTERFACE
        public void InitializeComponent()
        {
            _myRenderer = _myGameEntity.GetComponent<SpriteRenderer>();
            Play(DefaultAnimation);
        }
        #endregion
    }
}
