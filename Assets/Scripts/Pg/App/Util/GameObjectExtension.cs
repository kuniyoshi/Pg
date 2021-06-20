#nullable enable
using UnityEngine;
using UnityEngine.Assertions;

namespace Pg.App.Util
{
    public static class GameObjectExtension
    {
        public static T GetComponentInChildrenStrictly<T>(this GameObject self)
            where T : Component
        {
            var component = self.GetComponentInChildren<T>();
            Assert.IsNotNull(component, $"Could NOT get {typeof(T).Name} from {self.name}");

            return component;
        }

        public static T GetComponentStrictly<T>(this GameObject self)
            where T : Component
        {
            var component = self.GetComponent<T>();
            Assert.IsNotNull(component, $"Could NOT get {typeof(T).Name} from {self.name}");

            return component;
        }
    }
}
