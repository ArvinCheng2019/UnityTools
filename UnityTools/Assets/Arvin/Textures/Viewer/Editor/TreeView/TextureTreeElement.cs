
namespace TextureTool
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;


    internal class TextureTreeElement
    {
        private ulong textureByteLength = 0;
        private string textureDataSizeText = "";
        public string AssetPath { get; set; } 
        public string AssetName { get; set; } 
        public ulong TextureByteLength => textureByteLength; 
        public string TextureDataSizeText => textureDataSizeText;
        public Texture2D Texture { get; set; } 
        public TextureImporter TextureImporter { get; set; } 
        public  TextureImporterFormat TextureImporterFormat { get; set; }
        public int Index { get; set; } 
        public TextureTreeElement Parent { get; private set; } 
        public List<TextureTreeElement> Children { get; } = new List<TextureTreeElement>(); 
        
        public GUIStyle GetLabelStyle(EHeaderColumnId id)
        {
            GUIStyle labelStyle = MyStyle.DefaultLabel;
            switch (id)
            {
                case EHeaderColumnId.TextureName:
                case EHeaderColumnId.TextureType:
                    break;
                case EHeaderColumnId.NPot:
                    if (TextureImporter.npotScale == TextureImporterNPOTScale.None)
                    {
                        labelStyle = MyStyle.RedLabel;
                    }
                    break;
                case EHeaderColumnId.MaxSize:
                    if (TextureImporter.maxTextureSize > ToolConfig.RedMaxTextureSize)
                    {
                        labelStyle = MyStyle.RedLabel;
                    }
                    break;
                case EHeaderColumnId.GenerateMips:
                    if (TextureImporter.mipmapEnabled == true)
                    {
                        labelStyle = MyStyle.RedLabel;
                    }
                    break;
                case EHeaderColumnId.AlphaIsTransparency:
                    break;
                case EHeaderColumnId.TextureSize:
                    switch (Mathf.Min(Texture.width, Texture.height))
                    {
                        case int minSize when minSize > ToolConfig.RedTextureSize:
                            labelStyle = MyStyle.RedLabel;
                            break;
                        case int minSize when minSize > ToolConfig.YellowTextureSize:
                            labelStyle = MyStyle.YellowLabel;
                            break;
                        default:
                            labelStyle = MyStyle.DefaultLabel;
                            break;
                    }
                    break;
                case EHeaderColumnId.DataSize:
                    switch ((int)TextureByteLength)
                    {
                        case int len when len > ToolConfig.RedDataSize:
                            labelStyle = MyStyle.RedLabel;
                            break;
                        //case int len when len > ToolConfig.YellowDataSize:
                        //    labelStyle = MyStyle.YellowLabel;
                        //    break;
                        default:
                            labelStyle = MyStyle.DefaultLabel;
                            break;
                    }
                    break;

            }
            return labelStyle;
        }
        
        public object GetDisplayData(EHeaderColumnId id)
        {
            switch (id)
            {
                case EHeaderColumnId.TextureName:
                    return Texture.name;
                case EHeaderColumnId.TextureType:
                    return TextureImporter.textureType;
                case EHeaderColumnId.NPot:
                    return TextureImporter.npotScale;
                case EHeaderColumnId.MaxSize:
                    return TextureImporter.maxTextureSize;
                case EHeaderColumnId.GenerateMips:
                    return TextureImporter.mipmapEnabled;
                case EHeaderColumnId.AlphaIsTransparency:
                    return TextureImporter.alphaIsTransparency;
                case EHeaderColumnId.TextureSize:
                    return new Vector2Int(Texture.width, Texture.height);
                case EHeaderColumnId.DataSize:
                    return TextureByteLength;
                default:
                    return -1;
            }
        }
        
        public string GetDisplayText(EHeaderColumnId id)
        {
            switch (id)
            {
                case EHeaderColumnId.TextureName:
                    return Texture.name;
                case EHeaderColumnId.TextureType:
                    return TextureImporter.textureType.ToString();
                case EHeaderColumnId.NPot:
                    return TextureImporter.npotScale.ToString();
                case EHeaderColumnId.MaxSize:
                    return TextureImporter.maxTextureSize.ToString();
                case EHeaderColumnId.GenerateMips:
                    return TextureImporter.mipmapEnabled.ToString();
                case EHeaderColumnId.AlphaIsTransparency:
                    return TextureImporter.alphaIsTransparency.ToString();
                case EHeaderColumnId.TextureSize:
                    return $"{Texture.width}x{Texture.height}";
                case EHeaderColumnId.DataSize:
                    return textureDataSizeText;
                default:
                    return "---";
            }
        }
        
        public void UpdateDataSize()
        {
            textureByteLength = (Texture != null) ? (ulong)Texture?.GetRawTextureData().Length : 0;
            textureDataSizeText = Utils.ConvertToHumanReadableSize(textureByteLength);
        }
        
        internal void AddChild(TextureTreeElement child)
        {
            if (child.Parent != null)
            {
                child.Parent.RemoveChild(child);
            }

            Children.Add(child);
            child.Parent = this;
        }
        
        public void RemoveChild(TextureTreeElement child)
        {
            if (Children.Contains(child))
            {
                Children.Remove(child);
                child.Parent = null;
            }
        }

        public bool DoesItemMatchSearch(SearchState[] searchState)
        {
            for (int columnIndex = 0; columnIndex < ToolConfig.HeaderColumnNum; columnIndex++)
            {
                if (!DoesItemMatchSearchInternal(searchState, columnIndex))
                {
                    return false;
                }
            }
            return true;
        }
        
        private bool DoesItemMatchSearchInternal(SearchState[] searchStates, int columnIndex)
        {
            var searchState = searchStates[columnIndex];

            return searchState.DoesItemMatch((EHeaderColumnId)columnIndex, this);
        }
    }
}