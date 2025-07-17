using Box2D.NET;
using System.Diagnostics;
using TRexGame.Engine.Tools;

namespace TRexGame.Plugins
{
    public class Box2DTest
    {
        #region FIELDS
        // Simulation control
        private bool _update = true;
        private int _subStepCount = 4;

        // Stores the world handle
        private B2WorldId _worldId;

        // Stores the ground body
        private B2BodyId _groundId;

        // Stores the dynamic body
        private B2BodyId _bodyId;
        #endregion

        #region PRIVATE METHODS
        private void CreateWorld()
        {
            B2WorldDef worldDef = B2Types.b2DefaultWorldDef();
            worldDef.gravity = new B2Vec2(0.0f, -10.0f);
            _worldId = B2Worlds.b2CreateWorld(ref worldDef);
        }

        // Creating a STATIC body
        private void CreateGroundBox()
        {
            // Define a body with position, damping, etc.
            // Use the world id to create the body.
            // Define shapes with friction, density, etc.
            // Create shapes on the body.

            B2BodyDef groundBodyDef = B2Types.b2DefaultBodyDef();
            groundBodyDef.position = new B2Vec2(0.0f, -10.0f);
            _groundId = B2Bodies.b2CreateBody(_worldId, ref groundBodyDef);

            B2Polygon groundBox = B2Geometries.b2MakeBox(50.0f, 10.0f);

            B2ShapeDef groundShapeDef = B2Types.b2DefaultShapeDef();
            B2Shapes.b2CreatePolygonShape(_groundId, ref groundShapeDef, ref groundBox);
        }

        private void CreateDynamicBody()
        {
            // DynamicBody must have a mass
            // set to DynamicBody to respond to forces
            // set position on creation to avoid lag - starting at origin or moving after is not good

            B2BodyDef bodyDef = B2Types.b2DefaultBodyDef();
            bodyDef.type = B2BodyType.b2_dynamicBody;
            bodyDef.position = new B2Vec2(0.0f, 4.0f);
            _bodyId = B2Bodies.b2CreateBody(_worldId, ref bodyDef);

            B2Polygon dynamicBox = B2Geometries.b2MakeBox(1.0f, 1.0f);

            B2ShapeDef shapeDef = B2Types.b2DefaultShapeDef();
            shapeDef.density = 1.0f; // default is 1.0
            shapeDef.material.friction = 0.3f;

            B2Shapes.b2CreatePolygonShape(_bodyId, ref shapeDef, ref dynamicBox);
        }

        private void SimulateWorld()
        {
            B2Worlds.b2World_Step(_worldId, (float)Time.FixedDeltaTime, _subStepCount);
            B2Vec2 position = B2Bodies.b2Body_GetPosition(_bodyId);
            B2Rot rotation = B2Bodies.b2Body_GetRotation(_bodyId);

            Debug.WriteLine($"{position.X} {position.Y} {B2MathFunction.b2Rot_GetAngle(rotation)}");
        }

        private void Cleanup()
        {
            B2Worlds.b2DestroyWorld(_worldId);
        }
        #endregion

        #region PUBLIC METHODS
        public void InitSimulation()
        {
            CreateWorld();
            CreateGroundBox();
            CreateDynamicBody();
        }
        public void RunSimulation()
        {
            if (_update)
            {
                SimulateWorld();

                if (Time.TimeSinceStart > 1f)
                {
                    _update = false;
                    Cleanup();
                }
            }
        }
        #endregion
    }
}
