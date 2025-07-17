using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using TRexGame.Engine.Entities;
using TRexGame.Engine.Graphics;
using TRexGame.Engine.Tools;

namespace TRexGame.Engine.Audio
{
    public abstract class AudioSource : Entities.IGameComponent
    {
        #region CONSTRUCTOR
        public AudioSource(GameAudio audio)
        {
            Audio = audio;
        }
        #endregion

        #region FIELDS
        private GameEntity _myGameEntity;
        protected SoundEffectInstance _currentInstance;
        protected float _currentSfxPlayback = 0;
        protected float _currentSfxDuration = 0;
        #endregion

        #region PROPERTIES
        public GameEntity MyGameEntity { get { return _myGameEntity; } set { _myGameEntity = value; } }
        protected GameAudio Audio { get; }
        public SoundEffect CurrentSfx { get; protected set; }
        public bool IsPlaying { get; private set; }
        public bool IsLooping {  get => _currentInstance.IsLooped; }
        public float LoopTime { get; protected set; }
        #endregion

        #region PUBLIC METHODS
        public void UpdateAudioSource()
        {
            if (IsPlaying && _currentSfxPlayback <= _currentSfxDuration)
            {
                _currentSfxPlayback += (float)Time.DeltaTime;
            }
            else
            {
                Stop();
            }
        }

        public void PlayOneShot(SoundEffect fx)
        {
            if (IsPlaying) return;

            IsPlaying = true;
            CurrentSfx = fx;
            _currentSfxDuration = (float)CurrentSfx.Duration.TotalSeconds;
            _currentInstance = CurrentSfx.CreateInstance();
            _currentInstance.Play();
        }

        public void Play(SoundEffect fx, bool loop)
        {
            if (IsPlaying) return;

            IsPlaying = true;
            CurrentSfx = fx;
            _currentSfxDuration = (float)CurrentSfx.Duration.TotalSeconds;
            _currentInstance = CurrentSfx.CreateInstance();
            _currentInstance.IsLooped = loop;
            _currentInstance.Play();
        }

        public void Stop()
        {
            if (!IsPlaying) return;

            _currentInstance.Stop();
            IsPlaying = false;
            CurrentSfx = null;
            _currentSfxDuration = 0;
            _currentSfxPlayback = 0;
            _currentInstance.IsLooped = false;
            _currentInstance = null;
        }
        #endregion

        #region IGameComponent INTERFACE
        public void InitializeComponent()
        {

        }
        #endregion
    }
}
