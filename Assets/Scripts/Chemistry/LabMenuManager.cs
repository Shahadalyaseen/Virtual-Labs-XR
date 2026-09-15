using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LabMenuManager : MonoBehaviour
{
    [System.Serializable]
    public struct RecipeUIInfo
    {
        public string reactionName;     // اسم التفاعل (مثلاً: Water)
        public string chemicalFormula;  // الصيغة الكيميائية (مثلاً: H2O = H x2 + O x1)
        public Sprite recipeIcon;       // صورة التفاعل
    }

    [Header("UI Dependencies")]
    public RectTransform catalogPanel;
    public Transform contentParent;
    public GameObject cardPrefab;

    [Header("Reactions List")]
    public List<RecipeUIInfo> reactionsList = new List<RecipeUIInfo>();

    [Header("Animation Settings")]
    public Vector2 hiddenPosition = new Vector2(0, -800);
    public Vector2 visiblePosition = new Vector2(0, 0);
    public float animSpeed = 10f;

    private bool _isOpen = false;
    private Coroutine _animCoroutine;

    void Start()
    {
        BuildMenu();

        if (catalogPanel != null)
        {
            catalogPanel.anchoredPosition = hiddenPosition;
        }
    }

    void BuildMenu()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in reactionsList)
        {
            GameObject newCard = Instantiate(cardPrefab, contentParent);

            // 1. تجميع النص مع سطر جديد بين الاسم والصيغة
            string fullText = $"{item.reactionName}\n<size=80%>{item.chemicalFormula}</size>";

            // 2. تعبئة النص في الكارت
            TextMeshProUGUI tmpText = newCard.GetComponentInChildren<TextMeshProUGUI>(true);
            if (tmpText != null)
            {
                tmpText.text = fullText;
            }
            else
            {
                Text titleText = newCard.GetComponentInChildren<Text>(true);
                if (titleText != null)
                {
                    titleText.text = $"{item.reactionName}\n{item.chemicalFormula}";
                }
            }

            // 3. تعبئة الصورة
            Image iconImage = null;
            Image[] images = newCard.GetComponentsInChildren<Image>(true);
            foreach (var img in images)
            {
                if (img.gameObject.name == "ItemIcon")
                {
                    iconImage = img;
                    break;
                }
            }

            if (iconImage != null && item.recipeIcon != null)
            {
                iconImage.sprite = item.recipeIcon;
            }
        }
    }

    public void ToggleMenu()
    {
        _isOpen = !_isOpen;
        Vector2 targetPosition = _isOpen ? visiblePosition : hiddenPosition;

        if (_animCoroutine != null)
            StopCoroutine(_animCoroutine);

        _animCoroutine = StartCoroutine(AnimateMenu(targetPosition));
    }

    IEnumerator AnimateMenu(Vector2 target)
    {
        while (Vector2.Distance(catalogPanel.anchoredPosition, target) > 0.1f)
        {
            catalogPanel.anchoredPosition = Vector2.Lerp(catalogPanel.anchoredPosition, target, Time.deltaTime * animSpeed);
            yield return null;
        }
        catalogPanel.anchoredPosition = target;
    }
}