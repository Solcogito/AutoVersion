// ============================================================================
// File:        VersionDisplay.cs
// Project:     AutoVersion Unity Menu Demo
// Version:     0.6.0
// Author:      Recursive Architect (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Displays the current version from version.txt in a TextMeshPro component.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================
using UnityEngine;
using TMPro;
using System.IO;

public class VersionDisplay : MonoBehaviour
{
    public TMP_Text label;

    private void Start()
    {
        string path = Path.Combine(Application.dataPath, "..", "version.txt");
        if (File.Exists(path))
            label.text = $"Version {File.ReadAllText(path).Trim()}";
        else
            label.text = "Version file missing";
    }
}
