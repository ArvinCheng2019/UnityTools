using Sirenix.OdinInspector;
using UnityEngine;

namespace Arvin.Task
{
    public class MakeSureNode : BaseNode
    {
        #region 输出项 ,交给子类
        
        [Output, LabelText("选确定")]
        public BaseNode Agree;
        
        
        [Output, LabelText("选取消")]
        public BaseNode Cancel;
        
        #endregion
    }
}