using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using XNode;

namespace Arvin.Task
{
    [System.Serializable]
    public class PlayerAttributeNode : Node
    {
        [LabelText("生命")] public int PlayerHp;
        [LabelText("智慧")] public int PlayerKnowledge;
        [LabelText("财富")] public int PlayerRich;
        [LabelText("运气")] public int PlayerLuck;
        
        [Output, LabelText("输出属性")]
        public BaseNode nextDialogue;
    }
}