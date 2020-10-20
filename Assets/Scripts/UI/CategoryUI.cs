using UnityEngine;
using UnityEngine.UI;

public class CategoryUI : MonoBehaviour
{
    #region InspectorFields
    [SerializeField] private Text categoryName;
    #endregion

    #region PublicMethods
    public void SetElement(string name)
    {
        categoryName.text = name;
    }
    #endregion
}
