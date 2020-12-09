namespace TextureTool
{
    using UnityEngine;
    
    internal static class SizeUnits    
    {
        public const ulong B = 1;
        public const ulong KB = 1024 * B;
        public const ulong MB = 1024 * KB;
        public const ulong GB = 1024 * MB;
        public const ulong TB = 1024 * GB;
    }
    
    internal static class ToolConfig
    {
        public readonly static string[] TargetDirectories = new string[] { "Assets" };  

        public readonly static string ProgressTitle = "正在处理中。。";
        public readonly static string CreatingMessage = "Creating...";
        public readonly static string LoadingMessage = "Loading...";

        public readonly static GUIContent WindowTitle = new GUIContent("图片查看器"); 

        public const int MB = 1024 * 1024; 
        public const int RedDataSize = 3 * MB; 
        public const int YellowTextureSize = 2000; 
        public const int RedTextureSize = 4000; 
        public const int RedMaxTextureSize = 2048; 

        public static int HeaderColumnNum => HeaderColumns.Length; 

        public static readonly TextureColumn[] HeaderColumns = new[] {
            new TextureColumn("Texture", 180f), // 0
            new TextureColumn("Texture Type", 105f), // 1
            new TextureColumn("Non Power of 2", 105f), // 2
            new TextureColumn("Max Size", 70f), // 3
            new TextureColumn("Generate\nMip Maps", 70f), // 4
            new TextureColumn("Alpha is\nTransparency", 96f), // 5
            new TextureColumn("Texture Size", 105f), // 6
            new TextureColumn("Data Size", 80f), // 7
            new TextureColumn("压缩方式",105f),// 8 
        };

        public static float InitialHeaderTotalWidth
        {
            get
            {
                float width = 0f;
                for (int i = 0; i < HeaderColumns.Length; i++)
                {
                    width += HeaderColumns[i].width;
                }
                return width;
            }
        }

    }
}