using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using TRexGame.Engine.Entities;
using TRexGame.Engine.Graphics;
using TRexGame.Engine.Physics;
using TRexGame.Engine.Resources;
using TRexGame.GameEntities.TRex.Input;
using TRexGame.GameEntities.TRex.TRexStates;
using Entities = TRexGame.Engine.Entities;
using TRexGame.Engine.Tools;

namespace TRexGame.GameEntities.TRex
{
    #region ENUM
    public enum ETRexState
    {
        IDLE,
        RUN,
        JUMP,
        DUCK,
        FALL,
    }
    #endregion

    public class TRex : GameEntity
    {
        #region CONSTANTS
        private const float MASS = 60;
        private const float JUMPFORCE = -600;
        private const float SPEED = 10;

        private const int TREX_START_POS_X = 1;
        private const int TREX_START_POS_Y = 16;
        #endregion

        #region CONSTRUCTOR
        public TRex(
            GameResources gameResources,
            int SCR_WID, 
            int SCR_HEI,
            string name = "TRex",
            int draworder = 0,
            string tag = "",
            Layer layer = Layer.Player
            ) : base(name, draworder, tag, layer)
        {
            _gameResources = gameResources;
            _screenWidth = SCR_WID;
            _screenHeight = SCR_HEI;
        }
        #endregion

        #region FIELDS
        private GameResources _gameResources;
        private int _screenWidth;
        private int _screenHeight;
        #endregion

        #region PROPERTIES
        // State
        public TRexStateMachine StateMachine { get; private set; }

        // Components
        public Rigidbody Rigidbody { get; set; }
        public SpriteRenderer SpriteRenderer { get; set; }
        public TRexAnimator Animator { get; private set; }
        public TRexAudioSource AudioSource { get; private set; }
        public TRexInput Input { get; private set; }

        // Game
        public float Speed { get; private set; }
        public float JumpForce { get; private set; }
        public bool IsAlive { get; private set; }
        public Vector2 StartingPosition { get; private set; }
        #endregion

        #region GAME ENTITY CALLBACKS
        public override void Awake()
        {
            RectTransform = new(
                new(
                    TREX_START_POS_X,
                    120
                )
                );
            Animator = new(_gameResources.TexSpritesheet);
            Rigidbody = new(MASS);
            SpriteRenderer = new();
            AudioSource = new(new TRexAudio(_gameResources));
            Input = new();

            InitializeComponents(
                new List<Entities.IGameComponent>
                {
                    Rigidbody,
                    RectTransform,
                    SpriteRenderer,
                    Animator,
                    AudioSource,
                    Input
                }
            );


            Speed = SPEED;
            JumpForce = JUMPFORCE;
            IsAlive = true;
            StartingPosition = RectTransform.Position;

            StateMachine = new TRexStateMachine(this);

        }
        public override void Start() { }
        public override void FixedUpdate() { }
        public override void Update()
        {
            Rigidbody.UpdatePhysics();
            Input.UpdateInputs();
            StateMachine.UpdateStateMachine();
            Animator.UpdateAnimator();
            AudioSource.UpdateAudioSource();
        }
        public override void OnRenderEntity() { }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            SpriteRenderer.Draw(spriteBatch, gameTime);
        }
        public override void OnDestroy() { }
        #endregion
    }
}
