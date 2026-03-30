using UnityEngine;

/// <summary>
/// Automatically creates the GameManager and HUD at runtime if they
/// are not already present in the scene.
/// </summary>
public static class GameBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        if (Object.FindFirstObjectByType<CatGameManager>() != null)
            return; // already in scene, nothing to do

        var go = new GameObject("GameManager");
        go.AddComponent<CatGameManager>();
        go.AddComponent<CatHUD>();
    }
}
