using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInstaller.Helper
{
    public static class RegistryExtension
    {
        public static bool IsRegistryExist(this RegistryKey rootKey, string subKey, string name)
        {
            RegistryKey key = rootKey.OpenSubKey(subKey);
            var subkeyNames = key.GetSubKeyNames();
            foreach (string keyName in subkeyNames)
            {
                if (keyName == name)
                {
                    rootKey.Close();
                    return true;
                }
            }
            rootKey.Close();
            return false;
        }
        public static bool IsRegistryValueExist(this RegistryKey rootKey, string subKey, string name)
        {
            RegistryKey key = rootKey.OpenSubKey(subKey);
            var subValueNames = key.GetValueNames();
            foreach (string valueName in subValueNames)
            {
                if (valueName == name)
                {
                    rootKey.Close();
                    return true;
                }
            }
            rootKey.Close();
            return false;
        }
        public static RegistryKey CreateRegistryKey(this RegistryKey rootKey, string subKey)
        {
            return rootKey.CreateSubKey(subKey);
        }
        /// <summary>
        /// //在HKEY_LOCAL_MACHINE\SOFTWARE\test下创建一个名为“test”，值为“博客园”的键值。如果该键值原本已经存在，则会修改替换原来的键值，如果不存在则是创建该键值。
        // 注意：SetValue()还有第三个参数，主要是用于设置键值的类型，如：字符串，二进制，Dword等等~~默认是字符串。如：
        // software.SetValue("test", "0", RegistryValueKind.DWord); //二进制信息
        /// </summary>
        /// <param name="rootKey"></param>
        /// <param name="subKey"></param>
        /// <param name="valueName"></param>
        /// <param name="value"></param>
        public static void CreateRegistryValue(this RegistryKey rootKey, string subKey, string valueName, string value)
        {
            RegistryKey key = rootKey.OpenSubKey(subKey, true); //该项必须已存在
            key.SetValue(valueName, value);
            rootKey.Close();
        }
    }
}
