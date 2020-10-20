using Extensions;
using UnityEngine;
using UnityEngine.UI;

public class BuildUiController : MonoBehaviour
{
    [SerializeField] private ShipBuildController shipBuildController;
    [SerializeField] private Transform elementsContainer;
    [SerializeField] private CategoryUI categoryElementPrefab;
    [SerializeField] private ModuleUI moduleUiPrefab;
    [SerializeField] private Button categoryBackBtn;
    [SerializeField] private ScrollRect scrollView;
    [SerializeField] private Button confirmBtn;
    [SerializeField] private GameObject exitWindow;
    [SerializeField] private Button windowBackBtn;
    private Category currentCategory;
    private void Start()
    {
        InstantiateCategories();
        confirmBtn.onClick.AddListener(ConfirmBuild);
    }
    private void InstantiateSubCategories(Category category)
    {
        currentCategory = category;
        Debug.Log(currentCategory.Name);
        elementsContainer.Clear();
        categoryBackBtn.gameObject.SetActive(true);
        foreach (var sub in category.SubCategories)
        {
            var btn = Instantiate(categoryElementPrefab);
            btn.transform.parent = elementsContainer;
            btn.transform.localPosition = Vector3.zero;
            btn.transform.localScale = Vector3.one;
            btn.SetElement(sub.Name);
            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                InstantiateModules(sub);
            });
        }
        categoryBackBtn.onClick.AddListener(InstantiateCategories);
    }
    
    private void InstantiateCategories()
    {
        elementsContainer.Clear();
        categoryBackBtn.gameObject.SetActive(false);
        foreach (var it in GameData.Instance.gameConfig.Categories)
        {
            var btn = Instantiate(categoryElementPrefab);
            btn.transform.parent = elementsContainer;
            btn.transform.localPosition = Vector3.zero;
            btn.transform.localScale = Vector3.one;
            btn.SetElement(it.Name);
            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                InstantiateSubCategories(it);
            });
        }
    }

    private void InstantiateModules(SubCategory subCategory)
    {
        elementsContainer.Clear();
        categoryBackBtn.gameObject.SetActive(true);
        foreach (var module in subCategory.ModuleTypes)
        {
            var moduleUi = Instantiate(moduleUiPrefab);
            moduleUi.transform.parent = elementsContainer;
            moduleUi.transform.localPosition = Vector3.zero;
            moduleUi.transform.localScale = Vector3.one;
            moduleUi.SetModule((int)module);
            moduleUi.onBeginDrag += i =>
            {
                shipBuildController.SpawnShipElement(i);
                EnablingScroll(false);
            };
            moduleUi.onDrag += shipBuildController.MoveShipElement;
            moduleUi.onEndDrag += () =>
            {
                shipBuildController.EndDrag();
                EnablingScroll(true);
            };
        }
        categoryBackBtn.onClick.AddListener(() =>
        {
            InstantiateSubCategories(currentCategory);
        });
    }

    private void EnablingScroll(bool value)
    {
        if (!value)
        {
            elementsContainer.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        scrollView.horizontal = value;
    }

    private void ConfirmBuild()
    {
        //if (shipBuildController.HasEmptyCells())
        //{
        //    exitWindow.SetActive(true);
        //    windowBackBtn.onClick.AddListener(() => {exitWindow.SetActive(false);});
        //}
        //else
        //{
            shipBuildController.SaveBuild();
        //}
    }
}
