#nullable enable
using UnityEngine;
using UnityEngine.Assertions;

namespace Pg.App.Util
{
    public static class ComponentExtension
    {
        public static T GetComponentInChildrenStrictly<T>(this Component self)
            where T : Component
        {
            var component = self.GetComponentInChildren<T>();
            Assert.IsNotNull(component, $"Could NOT get {typeof(T).Name} from {self.name}");

            return component;
        }

        public static T GetComponentStrictly<T>(this Component self)
            where T : Component
        {
            var component = self.GetComponent<T>();
            Assert.IsNotNull(component, $"Could NOT get {typeof(T).Name} from {self.name}");

            return component;
        }
    }
}
