using UnityEngine;

namespace datuloar.documentation.Config
{
    public class DocumentationEditorWindowConfig : ScriptableObject
    {
        [field: SerializeField] public Color ActiveCatalogButtonColor { get; private set; } = new Color(0.19f, 0.96f, 0.90f);
        [field: SerializeField] public Color ClassButtonColor { get; private set; } = new Color(0.6933962f, 1, 0.8395938f);
        [field: SerializeField] public Vector2 WindowMinSize { get; private set; } = new Vector2(800, 600);
        [field: SerializeField] public Vector2 WindowMaxSize { get; private set; } = new Vector2(800, 600);
        [field: SerializeField] public float DocumentationTextHeight { get; private set; } = 50f;
    }
}
