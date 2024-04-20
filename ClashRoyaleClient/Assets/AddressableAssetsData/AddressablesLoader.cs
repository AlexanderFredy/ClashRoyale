using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using UnityEngine.Video;

//ÂÑ¨ ×ÒÎ ÇÀÃĞÓÇÈËÈ ÍÓÆÍÎ ÇÀÏÎÌÈÍÀÒÜ Â ÑÑÛËÊÈ, ×ÒÎÁÛ ÏÎÒÎÌ ÈÕ ÓÄÀËßÒÜ ÈÇ ÎÏÅĞÀÒÈÂÊÈ ×ÅĞÅÇ Addressables.Release ÈËÈ Addressables.ReleaseInstance

public class AddressablesLoader : MonoBehaviour
{
    #region String
    [SerializeField] private string _path = string.Empty;

    [Button]
    private void LoadFromPath()
    {
        Addressables.ClearDependencyCacheAsync(_path);

        AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(_path);

        asyncOperationHandle.Completed += CompleteLoad;
    }
    #endregion

    #region Reference
    [SerializeField] private AssetReference _prefabReference;

    [Button]
    private void LoadFromReference()
    {
        AsyncOperationHandle<GameObject> asyncOperationHandle = _prefabReference.LoadAssetAsync<GameObject>();

        asyncOperationHandle.Completed += CompleteLoad;
    }

    [Button]
    private void AsyncCreateFromReference()
    {
        AsyncOperationHandle<GameObject> asyncOperationHandle = _prefabReference.InstantiateAsync();

        asyncOperationHandle.Completed += CompleteInstantiate;
    }

    private void CompleteInstantiate(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogWarning("Can't create instance from prefab!");
            return;
        }

        GameObject instance = handle.Result;
        Addressables.ReleaseInstance(instance);
    }
    #endregion

    #region Label
    [SerializeField] private AssetLabelReference _labelReference;

    [Button]
    private void LoadFromLabel() 
    {
        print("Start loading...");
        AsyncOperationHandle<IList<GameObject>> asyncOperationHandle = Addressables.LoadAssetsAsync<GameObject>(_labelReference,CreateInstance);

        asyncOperationHandle.Completed += CompleteLoadFromLabel;
    }

    private void CreateInstance(GameObject prefab)
    {
        if (prefab.GetComponent<Unit>())
            Instantiate(prefab, new Vector3(0, 0, -5f), Quaternion.identity);
        else if (prefab.GetComponent<Tower>())
            Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        else
            Instantiate(prefab, new Vector3(-0.14f, 5.77f, -9.76f), Quaternion.identity);
    }

    private void CompleteLoadFromLabel(AsyncOperationHandle<IList<GameObject>> handle)
    {
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogWarning("Can't load prefab from label");
            return;
        }

        print("Was loaded next prefabs from label:\n");
        foreach (var item in handle.Result)
        {
            print(item.ToString() + "\n");
        }
    }


    #endregion
    private void CompleteLoad(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogWarning("Can't load prefab from " + _path);
            return;
        }

        Instantiate(handle.Result,new Vector3(0,0,-5f),Quaternion.identity);
    }
  
}

[System.Serializable]//åñëè õî÷ó çàãğóæàòü ñâîé òèï àññåòîâ
public class AssetReferenceVideoClip : AssetReferenceT<VideoClip>
{
    public AssetReferenceVideoClip(string guid) : base(guid)//guid ïîñìîòğåòü â èíñïåêòîğå â ğåæèìå debug
    {

    }
}
