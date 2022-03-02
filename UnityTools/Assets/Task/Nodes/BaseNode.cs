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


        #region 可选项

        [LabelText("修正属性")] public bool UpdateAttribute;

        [ShowIf("UpdateAttribute", true), Input, LabelText("修正属性")]
        public PlayerAttributeNode TaskAttribute;


        [LabelText("发送消息")] public bool SendEvent = false;

        [ShowIf("SendEvent", true), LabelText("消息列表")]
        public Dictionary<TaskEventMenu, string> TaskEvents;

        #endregion

        #region 输入项

        [Input, LabelText("从那里来")] public BaseNode from;

        #endregion

        // #region 输出项 ,交给子类
        //
        // [Output, LabelText("下一段对话")]
        // public BaseNode nextDialogue;
        //
        // #endregion
    }
}