using System;
using Microsoft.Xna.Framework;

namespace TRexGame.Engine.Tools
{
    public static class Time
    {
        #region FIELDS
        public static double _accumulator = 0.0;
        #endregion

        #region PROPERTIES
        public static double DeltaTime { get; private set; } = 0.0;
        public static float TimeScale { get; set; } = 1.0f;
        public static double TimeSinceStart { get; private set; } = 0.0;
        public static double RealTimeSinceStart { get; private set; } = 0.0;
        public static double FixedDeltaTime { get; set; } = 1.0 / 60.0;
        public static bool IsFixedUpdateFrame { get; private set; } = false;
        #endregion

        #region PUBLIC METHODS
        public static void FixedUpdate()
        {
            IsFixedUpdateFrame = false;
            _accumulator += DeltaTime;
            while (_accumulator >= FixedDeltaTime)
            {
                IsFixedUpdateFrame = true;
                _accumulator -= FixedDeltaTime;
            }
        }
        public static void Update(GameTime gameTime)
        {
            DeltaTime = gameTime.ElapsedGameTime.TotalSeconds * TimeScale;
            TimeSinceStart = gameTime.TotalGameTime.TotalSeconds * TimeScale;
            RealTimeSinceStart = gameTime.TotalGameTime.TotalSeconds;
        }
        #endregion
    }
}
