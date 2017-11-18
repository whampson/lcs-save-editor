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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using WHampson.Cascara.Types;

using Pointer = WHampson.Cascara.Types.Pointer;

namespace WHampson.Cascara
{
    class Program
    {
        const string TestDataPath = "../../TestData";

        static void Main(string[] args)
        {
            UnionType();
            Console.WriteLine("All tests passed!");

            // Pause
            Console.ReadKey();
        }

        private static void Gta3Gxt()
        {
            string testPath = TestDataPath + "/Gta3Gxt";
            string binPath = testPath + "/american.gxt";
            string xmlPath = testPath + "/Gta3Gxt.xml";

            using (BinaryFile bFile = BinaryFile.Open(binPath))
            {
                bFile.ApplyTemplate(xmlPath);
                Pointer valueBase = bFile.GetPointer("ValueBlock.BasePointer");

                Dictionary<string, string> gxtData = new Dictionary<string, string>();
                Gta3GxtFile gxt = bFile.ExtractData<Gta3GxtFile>();
                foreach (GxtKey key in gxt.KeyBlock.KeyArray)
                {
                    Pointer<Char16> pVal = new Pointer<Char16>(valueBase + (int) key.ValueOffset.Value);
                    gxtData[ReadString8(key.KeyName)] = ReadString16(pVal);
                    //Console.WriteLine(string.Format("{0} => {1}", key, ReadString16(pVal)));
                }

                Debug.Assert(gxtData["HM1_3"] == "~g~The 'Nines' walk their turf in Wichita Gardens.");
                Debug.Assert(gxtData["TRAIN_1"] == "Kurowski Station");

                //Console.WriteLine(bFile.GetValue<int>("KeyBlock.KeyArray[0].ValueOffset"));

                //Pointer pChar = valueBase + bFile.GetValue<int>("KeyBlock.KeyArray[0].ValueOffset");
                //Pointer<Char16> p = (Pointer<Char16>) Convert.ChangeType(pChar, typeof(Pointer<Char16>));
                //Console.WriteLine(ReadString16(p));
            }
        }

        class Gta3GxtFile
        {
            public KeyBlock KeyBlock { get; set; }
        }

        class KeyBlock
        {
            public GxtKey[] KeyArray { get; set; }
        }

        class GxtKey
        {
            public Pointer<uint> ValueOffset { get; set; }
            public ArrayPointer<Char8> KeyName { get; set; }

            public override string ToString()
            {
                return "{ " + string.Format("{0,6} : {1,8}", ValueOffset.Value, ReadString8(KeyName)) + " }";
                //return "{ " + ReadString8(KeyName) + " }";
            }
        }

        //[StructLayout(LayoutKind.Explicit, Pack = 0, Size = 12)]
        //unsafe struct GxtKey
        //{
        //    [FieldOffset(0)]
        //    public uint ValuePointer;

        //    [FieldOffset(4)]
        //    public fixed sbyte KeyName[8];
        //}

        private static string ReadString8(Pointer<Char8> ptr)
        {
            string s = "";
            if (Pointer.IsArrayPointer(ptr))
            {
                int len = ((ArrayPointer<Char8>) ptr).Count;
                for (int i = 0; i < len; i++)
                {
                    if ((char) ptr.Value == '\0')
                    {
                        break;
                    }

                    s += ptr.Value;
                    ptr += 1;
                }

                return s;
            }

            while ((char) ptr.Value != '\0')
            {
                s += ptr.Value;
                ptr += 1;
            }

            return s;
        }

        private static string ReadString16(Pointer<Char16> ptr)
        {
            string s = "";
            while ((char) ptr.Value != '\0')
            {
                s += ptr.Value;
                ptr += 1;
            }

            return s;
        }

        private static void Gta3Save()
        {
            // TODO: fix paths
            string xmlPath = TestDataPath + "/PCSave.xml";
            string binPath = /*BinDir +*/ "/PC/GTA3sf1.b";

            using (BinaryFile bFile = BinaryFile.Open(binPath))
            {
                bFile.ApplyTemplate(xmlPath);
                Gta3PCSave gameSave = bFile.ExtractData<Gta3PCSave>();
                ArrayPointer<int> GlobalVariables = bFile.GetArrayPointer<int>("SimpleVars.Scripts.Data.GlobalVariables");
                for (int i = 0; i < GlobalVariables.Count; i++)
                {
                    GlobalVariables[i] = 0;
                }

                //bFile.Write(BinDir + "/PC/GTA3sf2.b");
            }
        }

        private static void IncludeDirective()
        {
            string testPath = TestDataPath + "/IncludeDirective";
            string binPath = testPath + "/IncludeTest.bin";
            string xmlPath = testPath + "/IncludeTest.xml";

            using (BinaryFile bFile = BinaryFile.Open(binPath))
            {
                bFile.ApplyTemplate(xmlPath);

                Debug.Assert(bFile.GetValue<float>("CircleData[0].Center.X") == 58.14f);
                Debug.Assert(bFile.GetValue<float>("CircleData[0].Center.Y") == -42.41f);
                Debug.Assert(bFile.GetValue<float>("CircleData[0].Radius") == 1.0f);
            }
        }

        private static void UnionType()
        {
            string testPath = TestDataPath + "/UnionType";
            string binPath = testPath + "/UnionTest.bin";
            string xmlPath = testPath + "/UnionTest.xml";

            using (BinaryFile bFile = BinaryFile.Open(binPath))
            {
                bFile.ApplyTemplate(xmlPath);

                Debug.Assert(bFile.GetOffset("TestUnion.Float1") == 0);
                Debug.Assert(bFile.GetOffset("TestUnion.InnerStruct.Float1") == 0);
                Debug.Assert(bFile.GetOffset("TestUnion.InnerStruct.Int1") == 4);
                Debug.Assert(bFile.GetOffset("TestUnion.InnerStruct.InnerUnion.Int1") == 8);
                Debug.Assert(bFile.GetOffset("TestUnion.InnerStruct.InnerUnion.Float1") == 8);
                Debug.Assert(bFile.GetOffset("TestUnion.InnerStruct.Bool1") == 12);
                Debug.Assert(bFile.GetOffset("TestUnion.Int1") == 0);

                Debug.Assert(bFile.GetValue<float>("TestUnion.Float1") == 1.0f);
                Debug.Assert(bFile.GetValue<float>("TestUnion.InnerStruct.Float1") == 1.0f);
                Debug.Assert(bFile.GetValue<int>("TestUnion.InnerStruct.Int1") == 1);
                Debug.Assert(bFile.GetValue<int>("TestUnion.InnerStruct.InnerUnion.Int1") == 2);
                Debug.Assert(bFile.GetValue<float>("TestUnion.InnerStruct.InnerUnion.Float1") == 2.802597e-45f);
                Debug.Assert(bFile.GetValue<bool>("TestUnion.InnerStruct.Bool1") == true);
                Debug.Assert(bFile.GetValue<int>("TestUnion.Int1") == 1065353216);

                UnionTest ut = bFile.ExtractData<UnionTest>();
                Debug.Assert(ut.TestUnion.Float1 == 1.0f);
                Debug.Assert(ut.TestUnion.InnerStruct.Float1 == 1.0f);
                Debug.Assert(ut.TestUnion.InnerStruct.Int1 == 1);
                Debug.Assert(ut.TestUnion.InnerStruct.InnerUnion.Int1 == 2);
                Debug.Assert(ut.TestUnion.InnerStruct.InnerUnion.Float1 == 2.802597e-45f);
                Debug.Assert(ut.TestUnion.InnerStruct.Bool1 == true);
                Debug.Assert(ut.TestUnion.Int1 == 1065353216);
            }
        }

        class UnionTest
        {
            public TestUnion TestUnion { get; set; }
        }

        class TestUnion
        {
            public float Float1 { get; set; }
            public int Int1 { get; set; }
            public UnionTestInnerStruct InnerStruct { get; set; }
        }

        class UnionTestInnerStruct
        {
            public float Float1 { get; set; }
            public int Int1 { get; set; }
            public bool Bool1 { get; set; }
            public UnionTestInnerStructInnerUnion InnerUnion { get; set; }
        }

        class UnionTestInnerStructInnerUnion
        {
            public float Float1 { get; set; }
            public int Int1 { get; set; }
        }

        private static void PrintString(BinaryFile bFile, string name)
        {
            int len = bFile.GetElemCount(name);
            name = Regex.Replace(name, @"\[\d+\]$", "");

            string s = "";
            for (int i = 0; i < len; i++)
            {
                char ch = bFile.GetValue<char>(name + "[" + i + "]");
                if (ch == 0)
                {
                    break;
                }
                s += ch;
            }

            Console.WriteLine(s);
        }

        private static string GetStringValue(Pointer<Char16> ptr)
        {
            string s = "";
            int i = 0;
            char c = (char) ptr[0];
            while (c != 0)
            {
                s += c;
                c = (char) ptr[++i];
            }

            return s;
        }

        private static bool IsCharType(object o)
        {
            if (!(o is IConvertible))
            {
                return false;
            }

            IConvertible conv = (IConvertible) o;
            return conv.GetTypeCode() == TypeCode.Char;
        }

        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            Type[] interfaceTypes = givenType.GetInterfaces();

            foreach (Type type in interfaceTypes)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }

            Type baseType = givenType.BaseType;
            if (baseType == null)
            {
                return false;
            }

            return IsAssignableToGenericType(baseType, genericType);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 0, Size = 12)]
    struct RwV3d
    {
        public RwV3d(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public override string ToString()
        {
            return string.Format("<{0}, {1}, {2}>", X, Y, Z);
        }
    }

    class SimpleVars
    {
        public ArrayPointer<Char16> SaveTitle { get; set; }
        public Pointer<RwV3d> CameraCoords { get; set; }
    }

    class Gta3PCSave
    {
        public SimpleVars SimpleVars { get; set; }
        public Pointer<uint> Checksum { get; set; }
    }
}
