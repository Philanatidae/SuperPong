using ECS;

namespace SuperPong.Systems
{
    public abstract class EntitySystem
    {
        readonly Engine _engine;

        public EntitySystem(Engine engine)
        {
            _engine = engine;
        }

        public abstract void Update(float dt);

        public Engine GetEngine()
        {
            return _engine;
        }

    }
}
