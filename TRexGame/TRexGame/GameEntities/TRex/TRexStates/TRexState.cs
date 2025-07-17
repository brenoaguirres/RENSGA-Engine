using Microsoft.Xna.Framework;

namespace TRexGame.GameEntities.TRex.TRexStates
{
    public abstract class TRexState
    {
        #region CONSTRUCTOR
        public TRexState(ETRexState key, TRex context)
        {
            _context = context;
            StateKey = key;
        }
        #endregion

        #region FIELDS
        protected TRex _context { get; set; }
        #endregion

        #region PROPERTIES
        public ETRexState StateKey { get; set; }
        #endregion

        #region PUBLIC METHODS
        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual ETRexState CheckTransitions() 
        {
            return StateKey;
        }
        public virtual void Exit() { }
        #endregion
    }
}
