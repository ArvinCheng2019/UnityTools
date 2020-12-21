/** ********************************************************************************
* Texture Viewer
* @ 2019 RNGTM
***********************************************************************************/

namespace TextureTool
{
    /** ********************************************************************************
    * @summary ヘッダー識別子
    ***********************************************************************************/
    internal enum EHeaderColumnId
    {
        TextureName = 0,
        TextureType,
        NPot, // Non Power of two
        MaxSize, // テクスチャ Maxサイズ
        GenerateMips, // Generate Mip Maps
        AlphaIsTransparency,
        TextureSize,
        DataSize,
        TextureCompress, // 图片压缩格式
        TextureReference, //引用计数
    }
}