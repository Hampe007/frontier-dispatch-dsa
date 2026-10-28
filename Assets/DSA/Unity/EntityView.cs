#nullable enable

using UnityEngine;

namespace FrontierDispatch.Unity
{
    public sealed class EntityView : MonoBehaviour
    {
        public int Id { get; private set; }
        private Renderer? _renderer;

        public void Initialize(int id)
        {
            Id = id;
            _renderer = GetComponent<Renderer>();
            SetNeutral();
        }

        public void SetNeutral() => SetColor(new Color(0.55f, 0.55f, 0.55f));
        public void SetInspected() => SetColor(new Color(0.95f, 0.75f, 0.15f));
        public void SetFound() => SetColor(new Color(0.2f, 0.85f, 0.35f));

        private void SetColor(Color color)
        {
            if (_renderer != null) _renderer.material.color = color;
        }
    }
}
