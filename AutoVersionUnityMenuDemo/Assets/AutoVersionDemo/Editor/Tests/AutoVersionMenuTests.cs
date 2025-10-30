// ============================================================================
// File:        AutoVersionMenuTests.cs
// Project:     AutoVersion Unity Menu
// Version:     0.6.1
// Author:      Benoit Desrosiers (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Safe Editor tests ensuring AutoVersion menu items exist and
//   version.txt can be read in the Unity Editor context.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================
#if UNITY_EDITOR
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Reflection;

public class AutoVersionMenuTests
{
    [Test]
    public void AutoVersion_MenuItemsExist()
    {
        // Verify main AutoVersion menu commands are registered
        Assert.True(MenuItemExists("Tools/AutoVersion/Show Current Version"), "Missing: Show Current Version");
        Assert.True(MenuItemExists("Tools/AutoVersion/Bump Patch"), "Missing: Bump Patch");
    }

    private bool MenuItemExists(string menuPath)
    {
        // Reflection-safe scan through internal UnityEditor MenuItem objects
        var menuItems = Unsupported.GetSubmenus("Tools");
        return menuItems.Any(m => m.Equals(menuPath, System.StringComparison.OrdinalIgnoreCase));
    }

    [Test]
    public void VersionFile_ExistsAndHasValidVersion()
    {
        var versionPath = Path.Combine(Application.dataPath, "../version.txt");
        Assert.True(File.Exists(versionPath), "version.txt should exist in project root");

        var content = File.ReadAllText(versionPath).Trim();
        Assert.IsNotEmpty(content, "version.txt should not be empty");
        Debug.Log("[AutoVersion] Current version file content: " + content);
    }
}
#endif
