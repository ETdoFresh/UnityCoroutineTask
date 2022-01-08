using System.Collections;
using UnityEngine;

namespace CoroutineAsync
{
    public class StaticCoroutine : MonoBehaviour
    {
        private static StaticCoroutine _instance;

        private void Awake()
        {
            if (_instance)
            {
                DestroyImmediate(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private static void CreateInstance()
        {
            var gameObject = new GameObject("StaticCoroutine");
            gameObject.AddComponent<StaticCoroutine>();
        }

        public new static Coroutine StartCoroutine(IEnumerator routine)
        {
            if (!_instance) CreateInstance();
            return ((MonoBehaviour) _instance).StartCoroutine(routine);
        }

        public new static void StopCoroutine(Coroutine routine)
        {
            if (!_instance) CreateInstance();
            ((MonoBehaviour) _instance).StopCoroutine(routine);
        }
    }
}