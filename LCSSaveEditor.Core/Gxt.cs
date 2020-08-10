using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using GTASaveData;

namespace LCSSaveEditor.Core
{
    public class Gxt
    {
        public static Gxt TheText { get; private set; }

        static Gxt()
        {
            TheText = new Gxt();
        }

        private Dictionary<string, Dictionary<string, string>> m_masterTable;

        public Dictionary<string, string> this[string key]
        {
            get { return m_masterTable[key]; }
        }

        public Dictionary<string, Dictionary<string, string>>.KeyCollection TableNames
        {
            get { return m_masterTable.Keys; }
        }

        public Dictionary<string, Dictionary<string, string>>.ValueCollection Tables
        {
            get { return m_masterTable.Values; }
        }

        public Gxt()
        {
            m_masterTable = new Dictionary<string, Dictionary<string, string>>();
        }

        public bool ContainsTable(string name)
        {
            return m_masterTable.ContainsKey(name);
        }

        public bool TryGetTable(string name, out Dictionary<string, string> table)
        {
            return m_masterTable.TryGetValue(name, out table);
        }

        public bool TryGetValue(string table, string key, out string value)
        {
            if (TryGetTable(table, out var t))
            {
                return t.TryGetValue(key, out value);
            }

            value = default;
            return false;
        }

        public void Load(string path)
        {
            Load(File.ReadAllBytes(path));
        }

        public void Load(byte[] data)
        {
            int gxtSize = data.Length;
            int numEntries = 0;

            using (StreamBuffer buf = new StreamBuffer(data))
            {
                m_masterTable.Clear();
                Log.Info("Loading GXT...");

                // Read TABL header
                string tabl = buf.ReadString(4);
                int tablSectionSize = buf.ReadInt32();
                int tablSectionPos = buf.Position;
                if (tabl != "TABL") throw InvalidGxt();
                Debug.Assert(buf.Position + tablSectionSize <= gxtSize);

                // Read TABL
                while (tablSectionPos < tablSectionSize)
                {
                    var table = new Dictionary<string, string>();

                    // Read TABL entry
                    string tableName = buf.ReadString(8);
                    int tablePos = buf.ReadInt32();
                    tablSectionPos += 12;
                    Debug.Assert(tablePos < gxtSize);

                    // Jump to table and read table name
                    buf.Seek(tablePos);
                    string tableName2 = tableName;
                    if (tableName != "MAIN") tableName2 = buf.ReadString(8);
                    Debug.Assert(tableName == tableName2);

                    // Read TKEY header
                    string tkey = buf.ReadString(4);
                    int tkeySectionSize = buf.ReadInt32();
                    int tkeySectionStart = buf.Position;
                    int tkeySectionOffset = 0;
                    if (tkey != "TKEY") throw InvalidGxt();
                    Debug.Assert(buf.Position + tkeySectionSize <= gxtSize);

                    // Verify TDAT presence
                    buf.Skip(tkeySectionSize);
                    string tdat = buf.ReadString(4);
                    int tdatSectionSize = buf.ReadInt32();
                    int tdatSectionStart = buf.Position;
                    if (tdat != "TDAT") throw InvalidGxt();
                    Debug.Assert(buf.Position + tdatSectionSize <= gxtSize);

                    // Read TKEY
                    buf.Seek(tkeySectionStart);
                    while (tkeySectionOffset < tkeySectionSize)
                    {
                        // Read TKEY entry
                        int valueOffset = buf.ReadInt32();
                        string key = buf.ReadString(8);
                        tkeySectionOffset += 12;
                        Debug.Assert(valueOffset < tdatSectionSize);
                        Debug.Assert(!table.ContainsKey(key));

                        // Read value
                        buf.Seek(tdatSectionStart + valueOffset);
                        table[key] = buf.ReadString(unicode: true);      // zero-terminated
                        numEntries++;

                        buf.Seek(tkeySectionStart + tkeySectionOffset);
                    }

                    m_masterTable[tableName] = table;
                    buf.Seek(tablSectionPos);
                }
            }

            Log.Info($"Loaded {numEntries} GXT entries from {m_masterTable.Count} tables.");
        }

        private static InvalidDataException InvalidGxt()
        {
            return new InvalidDataException("Invalid GXT file!");
        }
    }
}
