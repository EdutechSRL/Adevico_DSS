using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace lm.Comol.Core.BaseModules.Editor
{
    public class RootObject
    {
        public static string ConfigurationFile(String editorPath)
        {
            return ConfigurationPath(editorPath) + "Config_Editor.config";
        }
        public static string ConfigurationPath(String editorPath)
        {
            return (editorPath.EndsWith("/") ? editorPath : editorPath + "/");
        }
        public static string ToolsBasePath(String editorPath)
        {
            return ConfigurationPath(editorPath) ;
        }
        public static string EditorBasePath()
        {
            return "Modules/Common/Editor/";
        }
    }
}