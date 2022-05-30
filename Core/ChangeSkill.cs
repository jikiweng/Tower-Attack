using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TowerAttack.AI;

public class ChangeSkill : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] SoldierPiece soldierPiece=null;
    [SerializeField] ChangeSkill hideTarget=null;
    [SerializeField] GameObject greyFrame=null;
    [SerializeField] Image image=null;
    [SerializeField] int skillType=1;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!soldierPiece.ChangeSkill(skillType)) return;

        setTrue();
        hideTarget.setFalse();
    }

    private void setTrue()
    {
        greyFrame.SetActive(true);
        image.color = new Color32(50, 50, 50, 255);
    }

    public void setFalse()
    {
        greyFrame.SetActive(false);
        image.color = new Color32(255, 255, 255, 255);
    }
}
