using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Code
{
    public class BancaConfigLogic
    {
        private static IEnumerable<DataAccess.Tables.DTOs.MBancasConfig> BancaConfigs { get; set; }
      
        public BancaConfigLogic()
        {
            BancaConfigs = null;
            BancaConfigs = DataAccess.EFRepositories.BancasRepository.GetBancaConfig();
        }
        public bool IsBancaConfigTrue(Enums.BancaConfigEnums.BancaConfigKeyEnum pConfigKey, int pBancaId, Enums.BancaConfigEnums.BancaConfigValueEnum pConfigValue)
        {
            var config = BancaConfigs.Where(x => x.BancaID == pBancaId && x.ConfigKey == pConfigKey.ToString() && x.Activo &&  (pConfigValue == Enums.BancaConfigEnums.BancaConfigValueEnum.NULL || x.ConfigValue.ToUpper() == pConfigValue.ToString()));
            if (config.Any())
            {
                return true;
            }
            return false;
        }

        public void AgregarOActualizaBancaConfig(int pBancaId, Enums.BancaConfigEnums.BancaConfigKeyEnum pConfigKey, string pConfigValue, bool pActivo)
        {
            DataAccess.EFRepositories.BancasRepository.AgragaOActualizaBacanConfigRecord(pBancaId, pConfigKey.ToString(), pConfigValue, pActivo);
        }
     





    }
}
