using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Sirenix.OdinInspector;
using UnityEngine;
using XNode;

namespace Arvin.Task
{
    public enum TaskEventMenu
    {
        NONE,
        ADD_ATTRIBUTE,
    }

    public class TaskEventNode : Node
    {
        [LabelText("消息类型")] public TaskEventMenu TaskEvent;
        [LabelText("实际参数")] public string TaskData;
        [Input] public BaseNode InputNode;
        [Output] public BaseNode OutputNode;
    }
}