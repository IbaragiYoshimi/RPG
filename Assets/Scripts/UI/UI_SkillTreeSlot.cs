using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string skillName;
    [FormerlySerializedAs("description")]
    [TextArea]
    [SerializeField] private string skillDescription;
    
    [SerializeField] private Color lockedColor;

    [SerializeField] private UI ui;
    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;

    public int skillCurrency;
    
    public bool unlocked = false;

    // Prerequisites.
    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    // Mutually exclusive conditions.
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;

    [SerializeField] private Image skillImage;

    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot -  " + skillName;
    }

    private void Start()
    {
        skillImage = GetComponent<Image>();
        skillImage.color = lockedColor;

        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
        ui = GetComponentInParent<UI>();
    }

    public void UnlockSkillSlot()
    {
        // Judge prerequisites.
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlocked == false)
            {
                Debug.Log("Can't unlock skill slot, because prerequisites had not been met.");
                return;
            }
        }
        
        // Judge mutually exclusive conditions.
        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked == true)
            {
                Debug.Log("Can't unlock skill slot, because mutually exclusive conditions had been met.");
                return;
            }
        }

        if (PlayerManager.instance.currency <= skillCurrency)
        {
            Debug.Log("Can't unlock skill! Not enough currency!");
        }
        
        unlocked = true;
        skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.SkillTooltip.ShowTooltip(skillName, skillDescription);
        
        /*
        Vector2 mousePos = Input.mousePosition;
        if (mousePos.x > Screen.width / 2)
            xOffset *= -1;

        if (mousePos.y < Screen.width / 2)
            yOffset *= -1;
        
        ui.SkillTooltip.transform.position = new Vector2(mousePos.x + xOffset, mousePos.y + yOffset);
        Debug.Log("Entered skill slot.");*/
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.SkillTooltip.HideTooltip();
    }
}
