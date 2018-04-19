using System;
using System.Collections.Generic;
using WandererWorld.Components;

namespace WandererWorld.Manager
{
    public class EntityComponentManager
    {
        private static EntityComponentManager entityComponentManager = new EntityComponentManager();

        private Dictionary<int, Dictionary<Type, GenericComponent>> entityDictionary = new Dictionary<int, Dictionary<Type, GenericComponent>>();
        private Dictionary<Type, Dictionary<int, GenericComponent>> componentDictionary = new Dictionary<Type, Dictionary<int, GenericComponent>>();

        private int currentId = 0;

        public static EntityComponentManager GetManager()
        {
            return entityComponentManager;
        }

        public int CreateNewEntityId()
        {
            entityDictionary.Add(currentId, new Dictionary<Type, GenericComponent>());

            return currentId++;
        }

        public GenericComponent GetComponent(int entityId, Type componentType)
        {
            var component = componentDictionary[componentType];

            return component[entityId];
        }

        public void AddComponentToEntity(int entityId, GenericComponent componentToAdd)
        {
            if (!componentDictionary.ContainsKey(componentToAdd.GetType()))
            {
                componentDictionary.Add(componentToAdd.GetType(), new Dictionary<int, GenericComponent>());
            }
            entityDictionary[entityId].Add(componentToAdd.GetType(), componentToAdd);
            componentDictionary[componentToAdd.GetType()].Add(entityId, componentToAdd);
        }

        public Dictionary<int, GenericComponent> GetComponentByType(Type componentType)
        {
            try
            {
                return componentDictionary[componentType];
            }
            catch (KeyNotFoundException)
            {
                return new Dictionary<int, GenericComponent>();
            }
            
        }
    }
}
