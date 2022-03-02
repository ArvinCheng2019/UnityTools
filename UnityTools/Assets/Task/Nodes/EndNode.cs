using System.Collections;
using System.Collections.Generic;
using Arvin.Task;
using Sirenix.OdinInspector;
using UnityEngine;
using XNode;

namespace Arvin.Task
{
    public class EndNode : Node
    {
        #region 输入项

        [Input, LabelText("结束")] public BaseNode from;

        #endregion
    }
}