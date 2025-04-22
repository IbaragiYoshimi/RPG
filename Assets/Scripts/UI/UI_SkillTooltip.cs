using TMPro;
using UnityEngine;

public class UI_SkillTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;

    public void ShowTooltip(string _skillName, string _skillDescription)
    {
        skillName.text = _skillName;
        skillDescription.text = _skillDescription;
        gameObject.SetActive(true);
    }
    
    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
