#nullable enable

using System.Collections.Generic;
using FrontierDispatch.Core;
using UnityEngine;

namespace FrontierDispatch.Unity
{
    public sealed class WorldMapDemoController : MonoBehaviour
    {
        private readonly Dictionary<int, GameObject> _views = new Dictionary<int, GameObject>();
        private WorldData? _world;
        private WorldQueries? _queries;
        private string _idText = "734";
        private string _status = "Baseline lookup uses a linear scan.";

        private void Start()
        {
            _world = SampleWorldFactory.CreateSmall();
            _queries = new WorldQueries(_world);

            for (int i = 0; i < _world.Locations.Count; ++i)
            {
                Location location = _world.Locations[i];
                var node = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                node.name = $"{location.Id}_{location.Name}";
                node.transform.position = new Vector3(location.Position.X, 0f, location.Position.Y);
                node.transform.localScale = Vector3.one * 0.7f;
                node.GetComponent<Renderer>().material.color = new Color(0.45f, 0.65f, 0.85f);
                _views.Add(location.Id, node);
            }

            for (int i = 0; i < _world.Connections.Count; ++i)
            {
                Connection c = _world.Connections[i];
                if (!_views.TryGetValue(c.FromLocationId, out GameObject? a) || !_views.TryGetValue(c.ToLocationId, out GameObject? b)) continue;
                if (c.FromLocationId > c.ToLocationId) continue; // draw bidirectional road once
                var road = new GameObject($"Road_{c.FromLocationId}_{c.ToLocationId}");
                var lr = road.AddComponent<LineRenderer>();
                lr.positionCount = 2;
                lr.SetPosition(0, a.transform.position + Vector3.up * 0.05f);
                lr.SetPosition(1, b.transform.position + Vector3.up * 0.05f);
                lr.startWidth = 0.08f;
                lr.endWidth = 0.08f;
                lr.material = new Material(Shader.Find("Sprites/Default"));
                lr.startColor = Color.gray;
                lr.endColor = Color.gray;
            }
        }

        private void ResetColors()
        {
            foreach (GameObject view in _views.Values)
            {
                view.GetComponent<Renderer>().material.color = new Color(0.45f, 0.65f, 0.85f);
            }
        }

        private void Find()
        {
            if (_queries == null || !int.TryParse(_idText, out int id)) return;
            ResetColors();
            Location? location = _queries.FindLocationById(id);
            if (location == null)
            {
                _status = $"Location {id} not found.";
                return;
            }

            _views[id].GetComponent<Renderer>().material.color = new Color(0.2f, 0.85f, 0.35f);
            int outgoing = _queries.FindOutgoingConnections(id).Count;
            _status = $"Found {location.Name}. Outgoing connections: {outgoing}. Baseline queries scan the stored collections.";
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(20f, 20f, 500f, 245f), GUI.skin.box);
            GUILayout.Label("FRONTIER DISPATCH - assignment world-map baseline");
            GUILayout.Label("This scene visualizes the same pure C# WorldData/WorldQueries compiled by the standalone .NET solution.");
            GUILayout.Label("Location ID:");
            _idText = GUILayout.TextField(_idText);
            if (GUILayout.Button("Find location + outgoing connections")) Find();
            GUILayout.Space(10f);
            GUILayout.Label(_status);
            GUILayout.Label("Students are assessed on the headless C# system. Unity visualization is optional.");
            GUILayout.EndArea();
        }
    }
}
