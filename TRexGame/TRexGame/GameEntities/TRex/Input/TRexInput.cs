using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TRexGame.Engine.Entities;
using Entities = TRexGame.Engine.Entities;

namespace TRexGame.GameEntities.TRex.Input
{
    public class TRexInput : Entities.IGameComponent
    {
        #region CONSTRUCTOR
        public TRexInput()
        {
        }
        #endregion

        #region FIELDS
        private GameEntity _myGameEntity;
        private KeyboardState _previousKBState;
        #endregion

        #region PROPERTIES
        public GameEntity MyGameEntity { get { return _myGameEntity; } set { _myGameEntity = value; } }
        public bool JumpInput { get; private set; }
        public bool CancelJumpInput { get; private set; }
        #endregion

        #region PUBLIC METHODS
        public void UpdateInputs()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (!_previousKBState.IsKeyDown(Keys.Up) && keyboardState.IsKeyDown(Keys.Up))
            {
                JumpInput = true;
            }
            else
            {
                JumpInput = false;
            }

            if (keyboardState.IsKeyDown(Keys.Down))
            {
                CancelJumpInput = true;
            }
            else
            {
                CancelJumpInput = false;
            }

                _previousKBState = keyboardState;
        }
        #endregion

        #region IGameComponent INTERFACE
        public void InitializeComponent()
        {

        }
        #endregion
    }
}
