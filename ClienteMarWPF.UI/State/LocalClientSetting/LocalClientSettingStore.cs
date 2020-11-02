
using ClienteMarWPF.Domain.Enums;
using ClienteMarWPF.Domain.Exceptions;
using ClienteMarWPF.Domain.Models.Dtos;

using System;
using System.IO;
using IniParser;
using IniParser.Model;

namespace ClienteMarWPF.UI.State.LocalClientSetting
{
    public sealed class LocalClientSettingStore : LocalClientBase, ILocalClientSettingStore
    {

        private LocalClientSettingDTO _localClientSettings;

        public LocalClientSettingDTO LocalClientSettings
        {
            get => _localClientSettings;
            set
            {
                _localClientSettings = value;
            }
        }


        public void WriteDesktopLocalSetting(LocalClientSettingDTO setting) => throw new NotImplementedException();


        public void ReadDektopLocalSetting()
        {
            try
            {
                switch (FileExtension)
                {
                    case MarSettingExt.ini:
                        ReadIniFile();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            catch
            {
                throw new MarFileReadException("Configuraciòn de banca no pudo cargar");
            }

        }//fin de metodo ReadLocalSetting( )



        private void ReadIniFile()
        {
            try
            {
                string fileDirectory = Path.Combine(new string[] { BaseDirectory, FileName });

                var parser = new FileIniDataParser();

                IniData setting = parser.ReadFile(fileDirectory);

                var clienteSetting = new LocalClientSettingDTO();
                clienteSetting.BancaId = Convert.ToInt32(setting[IniFileKey]["Banca"]);
                clienteSetting.LF = Convert.ToInt32(setting[IniFileKey]["LF"]);
                clienteSetting.Direccion = setting[IniFileKey]["Direccion"];
                clienteSetting.Identidad = Convert.ToInt32(setting[IniFileKey]["Identidad"]);
                clienteSetting.Tickets = Convert.ToInt32(setting[IniFileKey]["Tickets"]);
                clienteSetting.Espera = Convert.ToInt32(setting[IniFileKey]["Espera"]);
                clienteSetting.ServerIP = setting[IniFileKey]["ServerIP"];

                this.LocalClientSettings = clienteSetting;

            }
            catch
            {
                throw new MarFileReadException("Configuraciòn de banca no pudo cargar");
            }

        }//fin de metodo ReadIniFile( ) 









    }//fin de clase LocalClientSettingStore
}//fin de namespace
