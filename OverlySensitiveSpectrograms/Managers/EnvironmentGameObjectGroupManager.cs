using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace OverlySensitiveSpectrograms.Managers;

internal class EnvironmentGameObjectGroupManager : IInitializable, IDisposable
{
    private readonly Dictionary<string, GameObject[]> _groups = new();

    private GameObject _environmentGameObject = null!;

    public void Initialize()
    {
        _environmentGameObject = GameObject.Find("Environment");
    }

    public void Dispose()
    {
        _groups.Clear();
    }

    public GameObject[] Get(string id)
    {
        if (!_groups.ContainsKey(id))
            throw new Exception($"Group with ID \"{id}\" doesn't exist!");

        return _groups[id];
    }

    public T[] Get<T>(string id) where T : MonoBehaviour
    {
        var components = new List<T>();
        var groups = Get(id);
        foreach (var group in groups)
        {
            components.AddRange(group.GetComponents<T>());
        }

        return components.ToArray();
    }

    public void Add<T>(string id) where T : MonoBehaviour
    {
        if (_groups.ContainsKey(id))
            throw new Exception($"Group with ID \"{id}\" already exists!");

        var gameObjects = new List<GameObject>();
        foreach (var monoBehaviour in _environmentGameObject.GetComponentsInChildren<T>())
        {
            if (monoBehaviour.gameObject.activeSelf)
                gameObjects.Add(monoBehaviour.gameObject);
        }

        _groups.Add(id, gameObjects.ToArray());
    }

    public void Remove(string id)
    {
        if (!_groups.ContainsKey(id))
            throw new Exception($"Group with ID \"{id}\" doesn't exist!");

        _groups.Remove(id);
    }

    public void SetVisible(string id, bool visible)
    {
        if (!_groups.ContainsKey(id))
            throw new Exception($"Group with ID \"{id}\" doesn't exist!");

        var gameObjects = _groups[id];
        foreach (var gameObject in gameObjects)
        {
            if (gameObject != null)
                gameObject.SetActive(visible);
        }
    }
}
