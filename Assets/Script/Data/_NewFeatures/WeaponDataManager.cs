using System;

namespace Feature_NewData
{

    public class WeaponDataManager {
        public WeaponData BaseWeaponData;
        public WeaponData OnUsePostWeaponAddingData;

        public void RecalculateNonTemoralsOnUsePostWeapon(NonTemporalOnUsePostWeaponAddings weaponAddings){
            // OnUsePostWeaponAddingData.SetNumeric(BaseWeaponData.GetNumeric * weaponAddings.weaponReferingRatioAmount);
            // OnUsePostWeaponAddingData.SetFunctional(BaseWeaponData.GetFunctional + weaponAddings.weaponAdditionalFunctionals);
        }


        public WeaponData GetWeaponData() {
            // return BaseWeaponData + OnUsePostWeaponAddingData;
            throw new NotImplementedException();
        }
    }
}