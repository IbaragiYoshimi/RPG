using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill
{
    [Header("Parry")]
    [SerializeField] private UI_SkillTreeSlot parryUnlockedButton;
    public bool parryUnlocked = false;
    
    [Header("Parry Restore")]
    [SerializeField] private UI_SkillTreeSlot parryRestoreUnlockedButton;
    public bool parryRestoreUnlocked = false;
    
    [Header("Parry with mirage")]
    [SerializeField] private UI_SkillTreeSlot parryWithMirageUnlockedButton;
    public bool parryWithMirageUnlocked = false;

    protected override void Start()
    {
        base.Start();
        parryUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        parryRestoreUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        parryWithMirageUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
    }

    public void UnlockParry()
    {
        Debug.Log("Attempting to unlock parry.");

        if (!parryUnlocked && PlayerManager.instance.currency >= 100)
        {
            PlayerManager.instance.currency -= 100;
            parryUnlocked = true;
            Debug.Log("Parry unlocked.");
            return;
        }
        else
        {
            Debug.Log("Parry unlocked. Or no enough money.");
            return;
        }
    }

    public void UnlockParryRestore()
    {
        Debug.Log("Attempting to unlock restore.");

        if (!parryRestoreUnlocked && PlayerManager.instance.currency >= 200)
        {
            PlayerManager.instance.currency -= 200;
            parryRestoreUnlocked = true;
            Debug.Log("parry restore unlocked.");
            return;
        }
        else
        {
            Debug.Log("Parry restore unlocked. Or no enough money.");
            return;
        }
    }

    public void UnlockParryWithMirage()
    {
        Debug.Log("Attempting to unlock parry with mirage.");

        if (!parryWithMirageUnlocked && PlayerManager.instance.currency >= 300)
        {
            PlayerManager.instance.currency -= 300;
            parryWithMirageUnlocked = true;
            Debug.Log("parry with mirage unlocked.");
            return;
        }
        else
        {
            Debug.Log("Parry unlocked. Or no enough money.");
            return;
        }
    }
    
    public override void UseSkill()
    {
        base.UseSkill();
    }
}
