/*
This file is part of Super Pong.

Super Pong is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Super Pong is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Super Pong.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Input;

namespace SuperPong
{
    public class Settings
    {
        static Settings instance = new Settings();
        public static Settings Instance
        {
            get
            {
                return instance;
            }
        }

        public struct SettingsData
        {
            public Keys PrimaryKey1;
            public Keys PrimaryKey2;

            public Keys SecondaryKey1;
            public Keys SecondaryKey2;
        }

        public SettingsData Data;

        public void Load()
        {
            string path = GetPath();
            if (File.Exists(path))
            {
                Stream stream = File.Open(path, FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(SettingsData));
                Data = (SettingsData)serializer.Deserialize(stream);
                stream.Close();
            }
            else
            {
                Data = DefaultData();
                Save();
            }
        }

        public void Save()
        {
            string path = GetPath();
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            Stream stream = File.Open(path, FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(typeof(SettingsData));
            serializer.Serialize(stream, Data);
            stream.Close();
        }

        SettingsData DefaultData()
        {
            return new SettingsData
            {
                PrimaryKey1 = Keys.W,
                PrimaryKey2 = Keys.S,

                SecondaryKey1 = Keys.Up,
                SecondaryKey2 = Keys.Down
            };
        }

        string GetPath()
        {
            OperatingSystem os = Environment.OSVersion;
            PlatformID pid = os.Platform;
            string path;
            switch (pid)
            {
                case PlatformID.WinCE:
                case PlatformID.Win32S:
                case PlatformID.Win32NT:
                case PlatformID.Win32Windows:
                    path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                       "SuperPong");
                    break;
                case PlatformID.MacOSX:
                case PlatformID.Unix:
                    {
                        IntPtr buf = IntPtr.Zero;
                        try
                        {
                            buf = Marshal.AllocHGlobal(8192);
                            if (uname(buf) == 0)
                            {
                                string un = Marshal.PtrToStringAnsi(buf);
                                if (un == "Darwin")
                                {
                                    path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                                                       "Library", "Application Support", "com.philiprader.superpong");
                                }
                                else
                                {
                                    path = Directory.GetCurrentDirectory();
                                }
                            }
                            else
                            {
                                path = Directory.GetCurrentDirectory();
                            }
                        }
                        catch
                        {
                            path = Directory.GetCurrentDirectory();
                        }
                        finally
                        {
                            if (buf != IntPtr.Zero)
                            {
                                Marshal.FreeHGlobal(buf);
                            }
                        }
                    }
                    break;
                default:
                    path = Directory.GetCurrentDirectory();
                    break;
            }
            path = Path.Combine(path, "Settings.xml");

            return path;
        }

        [DllImport("libc")]
        static extern int uname(IntPtr buf);
    }
}
