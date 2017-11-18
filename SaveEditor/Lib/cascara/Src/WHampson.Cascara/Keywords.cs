#region License
/* Copyright (c) 2017 Wes Hampson
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
#endregion

using System.Collections.Generic;

namespace WHampson.Cascara
{
    /// <summary>
    /// Convenience class that contains all valid keywords for the template
    /// processor.
    /// </summary>
    internal static class Keywords
    {
        // Root element
        public const string TemplateRoot = "cascaraBinaryTemplate";

        // Data types
        public const string Bool = "bool";
        public const string Bool8 = "bool8";
        public const string Bool16 = "bool16";
        public const string Bool32 = "bool32";
        public const string Bool64 = "bool64";
        public const string Byte = "byte";
        public const string Char = "char";
        public const string Char8 = "char8";
        public const string Char16 = "char16";
        //public const string Char32 = "char32";
        //public const string Char64 = "char64";
        public const string Double = "double";
        //public const string DWord = "dword";
        public const string Float = "float";
        public const string Int = "int";
        public const string Int8 = "int8";
        public const string Int16 = "int16";
        public const string Int32 = "int32";
        public const string Int64 = "int64";
        public const string Long = "long";
        //public const string QWord = "qword";
        public const string Short = "short";
        public const string Single = "single";
        public const string Struct = "struct";
        public const string UInt = "uint";
        public const string UInt8 = "uint8";
        public const string UInt16 = "uint16";
        public const string UInt32 = "uint32";
        public const string UInt64 = "uint64";
        public const string ULong = "ulong";
        public const string Union = "union";
        public const string UShort = "ushort";
        //public const string Word = "word";

        // Directives
        public const string Align = "align";
        public const string Echo = "echo";
        public const string Include = "include";
        public const string Local = "local";
        public const string Typedef = "typedef";

        // Modifiers
        public const string Comment = "comment";
        public const string Count = "count";
        public const string Kind = "kind";
        public const string Message = "message";
        public const string Name = "name";
        public const string Newline = "newline";
        public const string Path = "path";
        public const string Raw = "raw";
        //public const string Typename = "typename";
        public const string Value = "value";

        // Special variables
        public const string Filesize = "__FILESIZE__";
        public const string Offset = "__OFFSET__";

        /// <summary>
        /// The list of reserved words for the template processor.
        /// </summary>
        /// <remarks>
        /// Reserved words are words that cannot be used as variable names.
        /// </remarks>
        public static readonly IEnumerable<string> ReservedWords = new List<string>()
        {
            // Root element
            TemplateRoot,

            // Data types
            Bool,
            Bool8,
            Bool16,
            Bool32,
            Bool64,
            Byte,
            Char,
            Char8,
            Char16,
            Double,
            Float,
            Int,
            Int8,
            Int16,
            Int32,
            Int64,
            Long,
            Short,
            Single,
            Struct,
            UInt,
            UInt8,
            UInt16,
            UInt32,
            UInt64,
            ULong,
            Union,
            UShort,

            // Directives
            Align,
            Include,
            Echo,
            Local,
            Typedef,

            // Special Variables
            Filesize,
            Offset
        };
    }
}