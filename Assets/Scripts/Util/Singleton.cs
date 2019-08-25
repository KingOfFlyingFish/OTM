using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T mInstance = null;
    private static readonly object mSyncObj = new object();
    private static bool isAppClosing = false;    

    public static T GetInstance
    {
        get
        {
            if (isAppClosing)
                return null;

            lock (mSyncObj)
            {
                if (mInstance == null)
                {
                    T[] objs = FindObjectsOfType<T>();

                    if (objs.Length > 0)
                        mInstance = objs[0];

                    if (objs.Length > 1)
                        Debug.LogError($"여러개의 {typeof(T).Name} 메니저가 씬에 있습니다. 확인 요망");

                    if (mInstance == null)
                    {
                        string goName = typeof(T).ToString();
                        GameObject go = GameObject.Find(goName);
                        if (go == null)
                            go = new GameObject(goName);
                        mInstance = go.AddComponent<T>();
                    }
                }
                return mInstance;
            }
        }
    }

    /// <summary>
    /// When Unity quits, it destroys objects in a random order. (유니티가 종료될 때, 무작위로 오브젝트를 삭제합니다.)
    /// In principle, a Singleton is only destroyed when application quits. (원칙적으로 싱글턴은 앱이 종료될 때에만 제거됩니다.)
    /// If any script calls Instance after it have been destroyed, (만약 인스턴트가 파괴된 후 스트립트를 호출할 경우)
    ///   it will create a buggy ghost object that will stay on the Editor scene (에디터 씬에 계속 머무르는 유령 오브젝트가 생성될 수 있습니다.)
    ///   even after stopping playing the Application. Really bad!(앱 플레이를 멈췄을 경우에도 계속 남아있습니다.)
    /// So, this was made to be sure we're not creating that buggy ghost object. (그래서 유령 오브젝트를 만들지 않기 위한 작업입니다.)
    /// </summary>
    protected virtual void OnApplicationQuit()
    {
        // release reference on exit
        isAppClosing = true;
    }
}