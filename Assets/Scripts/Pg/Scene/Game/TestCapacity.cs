#nullable enable
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Profiling;

namespace Pg.Scene.Game
{
    public class TestCapacity
        : MonoBehaviour
    {
        [SerializeField]
        int Capacity;

        [SerializeField]
        int IterationCount;

        void Update()
        {
            if (Keyboard.current.dKey.wasReleasedThisFrame)
            {
                Test();
                Test();
                Test();
                Test();
                Test();
            }
        }

        void Test()
        {
            Profiler.BeginSample($"{nameof(TestWithCapacity)}");
            TestWithCapacity();
            Profiler.EndSample();

            Profiler.BeginSample($"{nameof(TestWithoutCapacity)}");
            TestWithoutCapacity();
            Profiler.EndSample();
        }

        void TestWithCapacity()
        {
            var list = new List<int>(Capacity);

            for (var i = 0; i < IterationCount; ++i)
            {
                list.Add(i);
            }
        }

        void TestWithoutCapacity()
        {
            var list = new List<int>();

            for (var i = 0; i < IterationCount; ++i)
            {
                list.Add(i);
            }
        }
    }
}
