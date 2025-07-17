using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TRexGame.Engine.Tools;

namespace TRexGame.Engine.Entities
{
    public class EntityManager
    {
        #region SINGLETON PATTERN
        public static EntityManager _instance;
        public static EntityManager Instance 
        { 
            get
            {
                if (_instance == null)
                    _instance = new EntityManager();

                return _instance;
            }
        }
        #endregion

        #region FIELDS
        private List<GameEntity> _instancedEntities = new List<GameEntity>();
        private List<GameEntity> _gameEntities = new List<GameEntity>();
        private List<GameEntity> _cleanupEntities = new List<GameEntity>();
        private List<GameEntity> _awakeEntities = new List<GameEntity>();
        private List<GameEntity> _startEntities = new List<GameEntity>();
        #endregion

        #region PROPERTIES
        public IEnumerable<GameEntity> Entities => new ReadOnlyCollection<GameEntity>(_gameEntities);
        #endregion

        #region EVENTS
        private Action OnAwake;
        private Action OnStart;
        private Action OnRenderEntity;
        private Action OnUpdate;
        private Action OnFixedUpdate;
        #endregion

        #region PUBLIC METHODS
        public void Instantiate(GameEntity entity)
        {
            if (entity == null ||
                _instance._gameEntities.Contains(entity) ||
                _instance._cleanupEntities.Contains(entity)
                )
                return;

            _instancedEntities.Add(entity);
        }

        public void Destroy(GameEntity entity)
        {
            if (entity == null ||
                !_instance._gameEntities.Contains(entity) ||
                _instance._cleanupEntities.Contains(entity)
                )
                return;

            entity.OnDestroy();

            _instance._cleanupEntities.Add(entity);
            _instance.OnUpdate -= entity.Update;
            _instance.OnRenderEntity -= entity.OnRenderEntity;
        }
        public void InstantiationStage()
        {
            foreach (GameEntity entity in _instancedEntities)
            {
                _instance._gameEntities.Add(entity);
                _instance._awakeEntities.Add(entity);
                _instance._startEntities.Add(entity);

                _instance.OnAwake += entity.Awake;
                _instance.OnStart += entity.Start;
                _instance.OnFixedUpdate += entity.FixedUpdate;
                _instance.OnUpdate += entity.Update;
                _instance.OnRenderEntity += entity.OnRenderEntity;
            }

            _instancedEntities.Clear();
        }
        public void AwakeStage()
        {
            OnAwake?.Invoke();

            foreach (var entity in _instance._awakeEntities)
                _instance.OnAwake -= entity.Awake;

            _awakeEntities.Clear();
        }
        public void StartStage()
        {
            OnStart?.Invoke();

            foreach (var entity in _instance._startEntities)
                _instance.OnStart -= entity.Start;

            _startEntities.Clear();
        }
        public void FixedUpdateStage()
        {
            Time.FixedUpdate();
            if (Time.IsFixedUpdateFrame)
            {
                OnFixedUpdate?.Invoke();
            }
        }
        public void UpdateStage(GameTime gameTime)
        {
            OnUpdate?.Invoke();
            Time.Update(gameTime);
        }
        public void RenderEntityStage(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var entity in _gameEntities.OrderBy(e => e.DrawOrder))
                entity.Draw(spriteBatch, gameTime);
            
            OnRenderEntity?.Invoke();
        }
        public void CleanupStage()
        {
            for (int i = 0; i < _cleanupEntities.Count; i++)
            {
                if (_gameEntities.Contains(_cleanupEntities[i]))
                {
                    _gameEntities.Remove(_cleanupEntities[i]);
                    _cleanupEntities[i] = null;
                }
            }

            _cleanupEntities.Clear();
        }
        #endregion
    }
}