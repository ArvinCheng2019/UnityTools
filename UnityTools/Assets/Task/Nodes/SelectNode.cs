using Sirenix.OdinInspector;
using UnityEngine;
using XNode;

namespace Arvin.Task
{
    public class SelectNode : BaseNode
    {
        #region 输出项 ,交给子类
        
        [Output, LabelText("第一个分支")]
        public BaseNode One;
        
        
        [Output, LabelText("第二个分支")]
        public BaseNode Two;
        
        [Output, LabelText("第三个分支")]
        public BaseNode Three;
        
        [Output, LabelText("第四个分支")]
        public BaseNode Four;
        #endregion
    }
}