// ============================================================================
// File:        ReflectionUtils.cs
// Project:     AutoVersion Lite (Unit Tests)
// Author:      Solcogito S.E.N.C.
// ----------------------------------------------------------------------------
// Description:
//   Reflection helpers for testing private schema builders without changing
//   production visibility.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System;
using System.Reflection;

namespace AutoVersion.Tests.TestUtils
{
    public static class ReflectionUtils
    {
        public static T InvokePrivateStatic<T>(Type t, string methodName, params object[] args)
        {
            var mi = t.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            if (mi == null)
                throw new InvalidOperationException("Method not found: " + t.FullName + "." + methodName);

            object? result = mi.Invoke(null, args);
            if (result is T ok)
                return ok;

            throw new InvalidOperationException("Unexpected return type from " + methodName);
        }
    }
}
