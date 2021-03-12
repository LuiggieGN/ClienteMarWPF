
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

        }
        public void WriteDesktopLocalSetting(LocalClientSettingDTO setting)
        {
            try
            {
                switch (FileExtension)
                {
                    case MarSettingExt.ini:
                        WriteIniFile(setting);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            catch
            {
                throw new MarFileReadException("Configuraciòn de banca no pudo cargar");
            }

        }


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
                clienteSetting.Identidad =setting[IniFileKey]["Identidad"];
                clienteSetting.Tickets = Convert.ToInt32(setting[IniFileKey]["Tickets"]);
                clienteSetting.Espera = Convert.ToInt32(setting[IniFileKey]["Espera"]);
                clienteSetting.ServerIP = setting[IniFileKey]["ServerIP"];

                this.LocalClientSettings = clienteSetting;

            }
            catch
            {
                this.LocalClientSettings = null;
                throw new MarFileReadException("Configuraciòn de banca no pudo cargar");
            }
        }  

        private void WriteIniFile(LocalClientSettingDTO newsetting) 
        {
            try
            {
                if (newsetting == null)
                {
                    this.LocalClientSettings = null;
                    return;
                }

                var fileDirectory = Path.Combine(new string[] { BaseDirectory, FileName });


                if (File.Exists(fileDirectory))
                {                  
                    var parser = new FileIniDataParser();
                    var setting = parser.ReadFile(fileDirectory); 

                    setting[IniFileKey]["Banca"] = newsetting.BancaId.ToString();
                    setting[IniFileKey]["LF"] = newsetting.LF.ToString();
                    setting[IniFileKey]["Direccion"] = newsetting.Direccion?.ToString() ?? "0.0.0.0";
                    setting[IniFileKey]["Identidad"] = newsetting.Identidad.ToString();
                    setting[IniFileKey]["Tickets"] = newsetting.Tickets.ToString();
                    setting[IniFileKey]["Espera"] = newsetting.Espera.ToString();
                    setting[IniFileKey]["ServerIP"] = newsetting.ServerIP ?? string.Empty;

                    parser.WriteFile(fileDirectory, setting);


                }
                else
                {
                    var parser = new FileIniDataParser(); 
                    var keyAndData = new KeyDataCollection();
                    
                    keyAndData.AddKey("Banca", newsetting.BancaId.ToString());
                    keyAndData.AddKey("LF", newsetting.LF.ToString());
                    keyAndData.AddKey("Direccion", newsetting.Direccion?.ToString()??"0.0.0.0");
                    keyAndData.AddKey("Identidad", newsetting.Identidad.ToString());
                    keyAndData.AddKey("Tickets", newsetting.Tickets.ToString());
                    keyAndData.AddKey("Espera", newsetting.Espera.ToString());
                    keyAndData.AddKey("ServerIP", newsetting.ServerIP?.ToString()??string.Empty);

                    var seccion = new SectionData(IniFileKey);
                    seccion.Keys = keyAndData;

                    var dataColeccion = new SectionDataCollection();
                    dataColeccion.Add(seccion);

                    IniData setting = new IniData(dataColeccion);               

                    parser.WriteFile(fileDirectory, setting);

                }


                this.LocalClientSettings = newsetting;   //@@ aqui almaceno el nuevo archivo de configuracion ini que se va a utilizar atravaes de la aplicacion completa                


            }
            catch
            {
                this.LocalClientSettings = null;
                throw new MarFileWriteException("Hubo un error al crear o actualizar configuracion local", FileName);
            }

        }//fin de metodo WriteIniFile( )








    }//fin de clase LocalClientSettingStore
}//fin de namespace
