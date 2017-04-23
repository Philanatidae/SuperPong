using System;
using System.Collections.Generic;
using ECS.Exceptions;

namespace ECS
{
    public class Entity
    {
        readonly Engine _engine;
        List<IComponent> _components = new List<IComponent>();

        internal Entity(Engine engine)
        {
            _engine = engine;
            Family.All(typeof(Family)).Get();
        }

        public bool HasComponent<T>() where T : IComponent
        {
            return HasComponent(typeof(T));
        }

        public bool HasComponent(Type componentType)
        {
            if (!componentType.IsComponent())
            {
                throw new TypeNotComponentException();
            }

            return GetComponent(componentType) != null;
        }

        public T GetComponent<T>() where T : IComponent
        {
            return (T)GetComponent(typeof(T));
        }

        public object GetComponent(Type componentType)
        {
            if (!componentType.IsComponent())
            {
                throw new TypeNotComponentException();
            }

            IComponent foundComp = _components.Find((IComponent comp) =>
            {
                return comp.GetType() == componentType;
            });

            return foundComp;
        }

        public void AddComponent(IComponent component)
        {
            if (HasComponent(component.GetType()))
            {
                throw new ComponentAlreadyExistsException();
            }

            _components.Add(component);
            _engine.UpdateFamilyBags(this);
        }

        public void RemoveComponent<T>() where T : IComponent
        {
            RemoveComponent(typeof(T));
        }

        public void RemoveComponent(Type componentType)
        {
            if (!componentType.IsComponent())
            {
                throw new TypeNotComponentException();
            }

            if (!HasComponent(componentType))
            {
                throw new ComponentNotFoundException();
            }

            IComponent componentToRemove = (IComponent)GetComponent(componentType);

            _components.Remove(componentToRemove);
            _engine.UpdateFamilyBags(this);
        }

    }
}
