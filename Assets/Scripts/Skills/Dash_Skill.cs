using UnityEngine;
using Button = UnityEngine.UI.Button;

public class Dash_Skill : Skill
{
    [Header("Dash")] public bool dashUnlocked;
    [SerializeField] private UI_SkillTreeSlot dashUnlockButton;
    [Header("Clone on Dash")] public bool cloneOnDashUnlocked;
    [SerializeField] private UI_SkillTreeSlot cloneOnDashUnlockButton;
    [Header("Clone on arrival")] public bool cloneOnArrivalUnlocked;
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalUnlockButton;
    public override void UseSkill()
    {
        base.UseSkill();

    }
    
    protected override void Start()
    {
        base.Start();

        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
    }

    public void UnlockDash()
    {
        Debug.Log("Attempt to unlock the dash");

        if (!dashUnlocked && PlayerManager.instance.currency >= 100)
        {
            dashUnlocked = true;
            PlayerManager.instance.currency -= 100;
            Debug.Log("Dash unlocked.");
            return;
        }
        else
        {
            Debug.Log("Dash unlocked. Or no enough money.");
            return;
        }

        
    }

    public void UnlockCloneOnDash()
    {
        Debug.Log("Attempt to unlock the clone on dash");

        if (!cloneOnDashUnlocked && PlayerManager.instance.currency >= 200)
        {
            cloneOnDashUnlocked = true;
            PlayerManager.instance.currency -= 200;
            Debug.Log("Clone on dash unlocked.");
            return;
        }
        else
        {
            Debug.Log("Clone on dash unlocked. Or no enough money.");
            return;
        }

        
    }
    
    public void UnlockCloneOnArrival()
    {
        Debug.Log("Attempt to unlock the clone on arrival");

        if (!cloneOnArrivalUnlocked && PlayerManager.instance.currency >= 300)
        {
            cloneOnArrivalUnlocked = true;
            PlayerManager.instance.currency -= 300;
            Debug.Log("Clone on arrival unlocked.");
            return;
        }
        else
        {
            Debug.Log("Clone on arrival unlocked. Or no enough money.");
            return;
        }
    }
}
