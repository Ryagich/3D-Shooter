using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponInterfaceController : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TMP_Text _ammoText, _stateText;

    private HandItemController handItemController;
    private WeaponController weaponC;
    private AmmoController ammoC;

    private void Awake()
    {
        handItemController = GetComponent<HandItemController>();
        handItemController.OnChangeHandItem += ChangeWeapon;
        InventoryModel.OnUpdateInventory += UpdateAmmoText;
    }

    private void ChangeWeapon(HandItem item)
    {
        var w = item?.GetComponent<WeaponController>();
        if (weaponC)
        {
            weaponC.OnShoot -= UpdateAmmoText;
            weaponC.OnChangeState -= UpdateStateText;
            ammoC.OnReload -= UpdateAmmoText;
        }
        if (!w)
        {
            weaponC = null;
            ammoC = null;
            _stateText.text = "";
        }
        else
        {
            weaponC = w;
            ammoC = item.GetComponent<AmmoController>();
            weaponC.OnShoot += UpdateAmmoText;
            weaponC.OnChangeState += UpdateStateText;
            ammoC.OnReload += UpdateAmmoText;
            UpdateStateText(weaponC.GetCurrState);
        }
        UpdateAmmoText();
    }

    private void UpdateStateText(ShootState state)
    {
        if (!weaponC)
            _stateText.text = "";
        else
            _stateText.text = state.ToString();
    }

    private void UpdateAmmoText()
    {
        if (!ammoC)
            _ammoText.text = "";
        else
            _ammoText.text = $"{ammoC.CurrentAmmo} / {ammoC.GetTotalCount()}";
    }
}
