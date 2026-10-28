#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontierDispatch.Unity
{
    public sealed class HashLookupDemoController : MonoBehaviour
    {
        private readonly List<EntityView> _entities = new List<EntityView>();
        private readonly Dictionary<int, EntityView> _byId = new Dictionary<int, EntityView>();
        private string _targetText = "117";
        private string _status = "Choose a lookup method.";
        private Coroutine? _activeRoutine;

        private void Start()
        {
            const int count = 30;
            for (int i = 0; i < count; ++i)
            {
                var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.name = $"Entity_{100 + i}";
                go.transform.position = new Vector3((i % 10) - 4.5f, 0f, (i / 10) * 1.4f - 1.4f);
                go.transform.localScale = Vector3.one * 0.65f;
                var view = go.AddComponent<EntityView>();
                view.Initialize(100 + i);
                _entities.Add(view);
                _byId.Add(view.Id, view);
            }
        }

        private void ResetViews()
        {
            for (int i = 0; i < _entities.Count; ++i) _entities[i].SetNeutral();
        }

        private void StartLinear()
        {
            if (!int.TryParse(_targetText, out int id)) return;
            if (_activeRoutine != null) StopCoroutine(_activeRoutine);
            ResetViews();
            _activeRoutine = StartCoroutine(LinearLookup(id));
        }

        private IEnumerator LinearLookup(int id)
        {
            int inspected = 0;
            for (int i = 0; i < _entities.Count; ++i)
            {
                EntityView entity = _entities[i];
                inspected++;
                entity.SetInspected();
                yield return new WaitForSeconds(0.08f);
                if (entity.Id == id)
                {
                    entity.SetFound();
                    _status = $"Linear scan found {id} after inspecting {inspected} entities.";
                    yield break;
                }
            }
            _status = $"Linear scan did not find {id}; inspected all {inspected} entities.";
        }

        private void DictionaryLookup()
        {
            if (!int.TryParse(_targetText, out int id)) return;
            if (_activeRoutine != null) StopCoroutine(_activeRoutine);
            ResetViews();
            if (_byId.TryGetValue(id, out EntityView? entity))
            {
                entity.SetFound();
                _status = $"Dictionary lookup found {id} through the prepared ID index.";
            }
            else
            {
                _status = $"Dictionary lookup did not find {id}.";
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(20f, 20f, 470f, 230f), GUI.skin.box);
            GUILayout.Label("HASH LOOKUP - same question, different representation");
            GUILayout.Label("Entity ID (100-129):");
            _targetText = GUILayout.TextField(_targetText);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Linear scan")) StartLinear();
            if (GUILayout.Button("Dictionary lookup")) DictionaryLookup();
            GUILayout.EndHorizontal();
            GUILayout.Space(10f);
            GUILayout.Label(_status);
            GUILayout.Label("Yellow = inspected; green = found. The visual does not imply Dictionary is always the right choice: the representation should follow the operations you need.");
            GUILayout.EndArea();
        }
    }
}
