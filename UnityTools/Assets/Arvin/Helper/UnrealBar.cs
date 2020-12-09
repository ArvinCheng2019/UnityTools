using System.Threading.Tasks;
using UnityEditor;

namespace Arvin
{
    public class UnrealBar
    {
        public static async void ShowUnrealBar(string title = "正在处理", string msg = "处理中", int duration = 1)
        {
            EditorUtility.DisplayProgressBar(title, msg, 0);
            await Task.Delay(duration * 500);
            EditorUtility.DisplayProgressBar(title, msg, 0.8f);
            await Task.Delay(duration * 300);
            EditorUtility.DisplayProgressBar(title, msg, 0.8f);
            await Task.Delay(duration * 200);
            EditorUtility.ClearProgressBar();
        }
    }
}