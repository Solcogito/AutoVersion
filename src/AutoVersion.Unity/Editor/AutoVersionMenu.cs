#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace AutoVersion.Unity.Editor
{
    public static class AutoVersionMenu
    {
        [MenuItem("Tools/AutoVersion/Bump Patch")]
        public static void BumpPatch()
        {
            Debug.Log("AutoVersion: Bump Patch (stub)");
        }
    }
}
#endif
