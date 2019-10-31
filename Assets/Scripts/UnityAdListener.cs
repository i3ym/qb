using System.Threading.Tasks;
using UnityEngine.Advertisements;

namespace qb.Unity
{
    public class UnityAdListener : IUnityAdsListener
    {
        protected TaskCompletionSource<bool> Completion;
        protected bool Completed = false;

        public UnityAdListener(TaskCompletionSource<bool> completion) => Completion = completion;

        public void OnUnityAdsDidError(string message) { }
        public void OnUnityAdsDidStart(string placementId) { }
        public void OnUnityAdsReady(string placementId) { }
        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            if (Completed) return;

            Completion.SetResult(showResult == ShowResult.Finished);
            Completed = true;
        }
    }
}