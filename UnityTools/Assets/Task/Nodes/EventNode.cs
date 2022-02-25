using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Arvin.Task
{
    public class EventNode : Node
    {
        [LabelText("触发事件")] public TaskEvent Event;
        [LabelText("触发具体参数")] public List<string> EventDates;
    }
}