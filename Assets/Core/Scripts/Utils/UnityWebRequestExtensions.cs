using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Utils
{
    public static class UnityWebRequestExtensions
    {
        public static Task<UnityWebRequest> AsTask(this UnityWebRequestAsyncOperation op)
        {
            var tcs = new TaskCompletionSource<UnityWebRequest>();
            op.completed += _ => tcs.SetResult(op.webRequest);
            return tcs.Task;
        }
    }

}