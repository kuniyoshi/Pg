#nullable enable
using UnityEngine;

namespace Pg.App.Util
{
    public static class ObjectExtension
    {
        public static T? NullPropagation<T>(this T? self)
        where T : Object
        {
            return self == null ? null : self;
        }
    }
}
