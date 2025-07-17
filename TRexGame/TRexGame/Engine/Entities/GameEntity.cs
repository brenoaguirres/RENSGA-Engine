using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TRexGame.Engine.Graphics;
using Entities = TRexGame.Engine.Entities;

namespace TRexGame.Engine.Entities
{
    public abstract class GameEntity
    {
        #region CONSTRUCTOR
        public GameEntity(
            string name = "", 
            int draworder = 0, 
            string tag = "", 
            Layer layer = Layer.Default
            )
        {
            Id = new Guid();
            Name = name;
            DrawOrder = draworder;
            Tag = tag;
            Layer = layer;

            EntityManager.Instance.Instantiate(this);
        }
        #endregion

        #region FIELDS
        protected List<Entities.IGameComponent> GameComponents { get; set; }
        protected RectTransform _rectTransform;
        #endregion

        #region PROPERTIES
        public Guid Id { get; init; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int DrawOrder { get; set; }
        public string Tag { get; set; }
        public Layer Layer { get; set; }
        public RectTransform RectTransform
        {
            get
            {
                RectTransform t = _rectTransform;
                if (t != null) return t;

                t = GetComponent<RectTransform>();
                if ( t != null ) return t;

                else return null;
            }
            protected set
            {
                _rectTransform = value;
            }
        }
        #endregion

        #region GAMEENTITY METHODS
        public T GetComponent<T>() where T : Entities.IGameComponent
        {
            return (T)GameComponents?.Find(x => x is T) ?? default;
        }
        public List<T> GetComponents<T>() where T : Entities.IGameComponent
        {
            return GameComponents.OfType<T>().ToList();
        }
        #endregion

        #region PUBLIC METHODS
        public virtual void InitializeComponents(List<Entities.IGameComponent> components)
        {
            GameComponents = components;
            foreach (var comp in GameComponents)
            {
                comp.MyGameEntity = this;
            }
            foreach (var comp in GameComponents)
            {
                comp.InitializeComponent();
            }
        }

        public abstract void Awake();
        public abstract void Start();
        public abstract void FixedUpdate();
        public abstract void Update();
        public abstract void OnRenderEntity();
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
        public abstract void OnDestroy();
        #endregion
    }
}
