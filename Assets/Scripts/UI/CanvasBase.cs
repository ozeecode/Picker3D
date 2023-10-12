using UnityEngine;


namespace Picker3D
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasBase : MonoBehaviour
    {
        [SerializeField] private bool isVisibleOnStart;
        protected CanvasGroup canvasGroup;
        protected virtual void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        protected virtual void Start()
        {
            if (isVisibleOnStart)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }
        public virtual void Hide()
        {
            SetVisible(false);
        }

        public virtual void Show()
        {
            SetVisible(true);
        }

        public virtual void SetVisible(bool isVisible)
        {
            canvasGroup.alpha = isVisible ? 1 : 0;
            canvasGroup.interactable = isVisible;
            canvasGroup.blocksRaycasts = isVisible;
        }
    }
}
