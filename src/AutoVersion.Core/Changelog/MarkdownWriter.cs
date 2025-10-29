// ============================================================================
// File:        MarkdownWriter.cs
// Project:     AutoVersion Lite
// Version:     0.3.0
// Author:      Recursive Architect (Solcogito S.E.N.C.)
// ----------------------------------------------------------------------------
// Description:
//   Writes changelog markdown entries to disk, preserving prior entries.
// ----------------------------------------------------------------------------
// License:     MIT
// ============================================================================

using System.IO;
using System.Text;

namespace Solcogito.AutoVersion.Core.Changelog
{
    public static class MarkdownWriter
    {
        public static void Append(string path, string content)
        {
            if (!File.Exists(path))
            {
                File.WriteAllText(path, content, Encoding.UTF8);
                return;
            }

            var old = File.ReadAllText(path);
            var updated = $"{content}\n\n{old}";
            File.WriteAllText(path, updated, Encoding.UTF8);
        }
    }
}
