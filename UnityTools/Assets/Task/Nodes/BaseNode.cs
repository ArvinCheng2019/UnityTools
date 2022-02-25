using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using XNode;

namespace Arvin.Task
{
    public class BaseNode : Node
    {
        [LabelText("讲述人")] public string Speaker;

        [PreviewField(Alignment = ObjectFieldAlignment.Left), LabelText("展示形象")]
        public Sprite head;

        [TextArea, LabelText("说话内容")] public List<string> contents;
        [LabelText("点击确定")] public NextType nextType;

        [ShowIf("nextType", NextType.OK), Output, LabelText("下一段对话")]
        public BaseNode nextDialogue;

        [Input, LabelText("从那里来")] public BaseNode from;
    }
}