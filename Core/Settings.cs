#region License
/* Copyright(c) 2016-2019 Wes Hampson
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
using System.IO;
using System.Xml.Serialization;

namespace LcsSaveEditor.Core
{
    /// <summary>
    /// An XML-serializable application settings interface.
    /// </summary>
    [XmlRoot("SaveEditorSettings")]
    public class Settings
    {
        /// <summary>
        /// Default path for settings file.
        /// </summary>
        public const string DefaultSettingsPath = "settings.xml";

        static Settings()
        {
            Current = new Settings();
        }

        /// <summary>
        /// Gets or sets the <see cref="Settings"/> object
        /// currently in use by the application.
        /// </summary>
        public static Settings Current { get; set; }

        /// <summary>
        /// Creates a <see cref="Settings"/> object from data
        /// in the specified XML file.
        /// </summary>
        /// <param name="path">The path to the XML file.</param>
        /// <returns>
        /// A new <see cref="Settings"/> whose properties contain
        /// the values stored in the XML file.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if an error occurs during deserialization.
        /// </exception>
        public static Settings Load(string path)
        {
            using (StreamReader r = new StreamReader(path)) {
                XmlSerializer xs = new XmlSerializer(typeof(Settings));
                return xs.Deserialize(r) as Settings;
            }
        }

        /// <summary>
        /// Creates a new <see cref="Settings"/> object.
        /// </summary>
        public Settings()
        {
            RecentFiles = new List<string>();
        }

        public string SaveDataFileDialogDirectory { get; set; }

        public string CustomVariablesFileDialogDirectory { get; set; }

        public string OtherFileDialogDirectory { get; set; }

        public string CustomVariablesAndroid { get; set; }

        public string CustomVariablesIOS { get; set; }

        public string CustomVariablesPS2 { get; set; }

        public string CustomVariablesPSP { get; set; }

        [XmlArrayItem("RecentFile")]
        public List<string> RecentFiles { get; set; }

        public string GetCustomVariablesFile(GamePlatform fileType)
        {
            switch (fileType) {
                case GamePlatform.Android:
                    return CustomVariablesAndroid;
                case GamePlatform.IOS:
                    return CustomVariablesIOS;
                case GamePlatform.PS2:
                    return CustomVariablesPS2;
                case GamePlatform.PSP:
                    return CustomVariablesPSP;
            }

            return null;
        }

        public void SetCustomVariablesFile(GamePlatform fileType, string path)
        {
            switch (fileType) {
                case GamePlatform.Android:
                    CustomVariablesAndroid = path;
                    break;
                case GamePlatform.IOS:
                    CustomVariablesIOS = path;
                    break;
                case GamePlatform.PS2:
                    CustomVariablesPS2 = path;
                    break;
                case GamePlatform.PSP:
                    CustomVariablesPSP = path;
                    break;
            }
        }

        /// <summary>
        /// Writes the settings to the specified XML file.
        /// </summary>
        /// <param name="path">The path to the XML file.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if an error occurs during serialization.
        /// </exception>
        public void Store(string path)
        {
            using (StreamWriter w = new StreamWriter(path)) {
                XmlSerializer xs = new XmlSerializer(typeof(Settings));
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");     // Get rid of those unncecessary default namespaces
                xs.Serialize(w, this, ns);
            }
        }
    }
}
