using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    private Player player;

    [Header("Hand Properties")]
    [SerializeField] private Transform handPos;
    [SerializeField] private GameObject handModel;
    [SerializeField] private Animator handAnimator;
    [SerializeField] private RuntimeAnimatorController defaultHandAnimator;


    //equipments
    public WeaponBase Weapon { get; private set; }
    private Armor helmet;
    private Armor armor;
    private Armor pants;
    private Armor shield;
    private Armor boots;
    private Armor grove;

    //skill system
    [SerializeField] private SkillSystem skillSystem;


    public void Init(Player player)
    {
        this.player = player;

        handAnimator.runtimeAnimatorController = defaultHandAnimator;

        skillSystem.Init(player);

        Managers.Event.AddEvent<OnEquippedEquipment>(HandleOnEquippedEquipment);
        Managers.Event.AddEvent<OnUnEquippedEquipment>(HandleOnUnequippedEquipment);
    }


    private void OnDestroy()
    {
        Managers.Event.RemoveEvent<OnEquippedEquipment>(HandleOnEquippedEquipment);
        Managers.Event.RemoveEvent<OnUnEquippedEquipment>(HandleOnUnequippedEquipment);
    }


    //장비 장착
    private void HandleOnEquippedEquipment(OnEquippedEquipment e)
    {
        ItemObject equipment = e.InventoryItem;

        if (equipment == null) return;

        EquipmentData equipmentData = equipment.ItemData as EquipmentData;
        EEquipmentType equipmentType = equipmentData.EquipmentType;
        InitSkills(equipmentData);

        Managers.SoundManager.PlaySFX(KeyDefines.AudioKey.EQUIP_SFX);

        if (equipmentType == EEquipmentType.WEAPON)
        {
            this.Weapon = CreateWeapon(equipment, equipmentData);
            handModel.transform.position = Weapon.transform.position;
            handModel.transform.SetParent(handPos);
            handModel.transform.rotation = Quaternion.identity;
        }
        else
        {
            //방어구 장착
            GameObject armorObject = new GameObject(equipmentData.ItemName);
            Armor newArmor = armorObject.AddComponent<Armor>();
            newArmor.Init(player, equipment);
            newArmor.transform.SetParent(transform);
            newArmor.Equip();

            switch (equipmentType)
            {
                case EEquipmentType.HELMET:
                    helmet = newArmor;
                    break;
                case EEquipmentType.ARMOR:
                    armor = newArmor;
                    break;
                case EEquipmentType.PANTS:
                    pants = newArmor;
                    break;
                case EEquipmentType.BOOTS:
                    boots = newArmor;
                    break;
                case EEquipmentType.GLOVE:
                    grove = newArmor;
                    break;
                case EEquipmentType.SHIELD:
                    shield = newArmor;
                    break;
            }
        }


        Logger.Log($"{equipmentData.ItemName} 을 장착하여 월드에 생성했습니다.");
    }

    private WeaponBase CreateWeapon(ItemObject equipment, EquipmentData equipmentData)
    {
        WeaponData weaponData = equipmentData as WeaponData;

        GameObject weapon = new GameObject(weaponData.ItemName);
        GameObject weaponModel = Instantiate(weaponData.WeaponModel);
        weaponModel.transform.SetParent(weapon.transform);
        weaponModel.transform.localPosition = new Vector3(-weaponData.GrabPos.x, -weaponData.GrabPos.y, weaponData.GrabPos.z);

        weapon.transform.position = handPos.position;
        weapon.transform.rotation = handPos.rotation;
        weapon.transform.SetParent(handPos);
        
        WeaponBase wb = weapon.AddComponent<WeaponBase>();
        wb.InitHandAnimator(handAnimator);
        wb.Init(player, equipment);
        wb.Equip();

        return wb;
    }


    //장비에 담긴 스킬들을 초기화합니다.
    private void InitSkills(EquipmentData equipmentData)
    { 
        //스킬등록
        List<SkillData> skills = equipmentData.SkillDatas;

        foreach (var skill in skills)
        {
            skillSystem.AddSkill(skill);
        }
    }


    //등록된 스킬들을 해제합니다.

    private void ReleaseSkills(EquipmentData equipmentData)
    {
        List<SkillData> skills = equipmentData.SkillDatas;

        foreach (var skill in skills)
        {
            skillSystem.RemoveSkill(skill);
        }
    }


    //장비 해제
    private void HandleOnUnequippedEquipment(OnUnEquippedEquipment e)
    {
        EEquipmentType equipmentType = e.equipmentType;

        if (equipmentType == EEquipmentType.WEAPON)
        {
            Logger.Log($"{Weapon.name} 을 해제 하였습니다.");
            handModel.transform.SetParent(handPos);
            handModel.transform.localPosition = Vector3.zero;
            Weapon.UnEquip();
            ReleaseSkills(Weapon.EquipmentData);
            Destroy(Weapon.gameObject);
            Weapon = null;
            handAnimator.runtimeAnimatorController = defaultHandAnimator;
        }
        else
        {
            Armor unequippedArmor = null;
            switch (equipmentType)
            {
                case EEquipmentType.HELMET:
                    unequippedArmor = helmet;
                    break;
                case EEquipmentType.ARMOR:
                    unequippedArmor = armor;
                    break;
                case EEquipmentType.PANTS:
                    unequippedArmor = pants;
                    break;
                case EEquipmentType.BOOTS:
                    unequippedArmor = boots;
                    break;
                case EEquipmentType.GLOVE:
                    unequippedArmor = grove;
                    break;
                case EEquipmentType.SHIELD:
                    unequippedArmor = shield;
                    break;
            }

            if (unequippedArmor != null)
            {
                unequippedArmor.UnEquip();
                ReleaseSkills(unequippedArmor.EquipmentData);
                Destroy(unequippedArmor.gameObject);
            }
        }
    }


}
