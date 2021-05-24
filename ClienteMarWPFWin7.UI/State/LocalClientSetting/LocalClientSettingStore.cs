
using ClienteMarWPFWin7.Domain.Enums;
using ClienteMarWPFWin7.Domain.Exceptions;
using ClienteMarWPFWin7.Domain.Models.Dtos;

using System;
using System.IO;
using IniParser;
using IniParser.Model;

namespace ClienteMarWPFWin7.UI.State.LocalClientSetting
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
            string fileDirectory = Path.Combine(new string[] { BaseDirectory, FileName });

            try
            {

                var parser = new FileIniDataParser();

                IniData setting = parser.ReadFile(fileDirectory);

                var clienteSetting = new LocalClientSettingDTO();
                clienteSetting.BancaId = Convert.ToInt32(setting[IniFileKey]["Banca"]);
                clienteSetting.LF = Convert.ToInt32(setting[IniFileKey]["LF"]);
                clienteSetting.Direccion = setting[IniFileKey]["Direccion"];
                clienteSetting.Identidad = setting[IniFileKey]["Identidad"];
                clienteSetting.Tickets = Convert.ToInt32(setting[IniFileKey]["Tickets"]);
                clienteSetting.Espera = Convert.ToInt32(setting[IniFileKey]["Espera"]);
                clienteSetting.ServerIP = setting[IniFileKey]["ServerIP"];

                this.LocalClientSettings = clienteSetting;

            }
            catch
            {
                #region Leyendo BackUp cuando el archivo no se puede leer
                try
                {
                    if (File.Exists(fileDirectory))
                    {
                        File.Delete(fileDirectory);
                    }

                    WriteIniFile(base.ReadBackUp());
                }
                catch
                {
                    try
                    {
                        #region Leyendo configuracion por defecto si la lectura de BackUp falla
                        WriteIniFile(base.LeerConfiguracionPorDefecto());
                        #endregion
                    }
                    catch  
                    {
                        this.LocalClientSettings = null;
                        throw new MarFileReadException("Configuraciòn de banca no pudo cargar");
                    }
                }

                #endregion
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
                    setting[IniFileKey]["ServerIP"] = newsetting?.ServerIP?.ToString() ?? string.Empty;
                    parser.WriteFile(fileDirectory, setting);


                }
                else
                {

                    if (!Directory.Exists(BaseDirectory))
                    {
                        Directory.CreateDirectory(BaseDirectory);
                    }


                    var parser = new FileIniDataParser();
                    var keyAndData = new KeyDataCollection();

                    keyAndData.AddKey("Banca", newsetting.BancaId.ToString());
                    keyAndData.AddKey("LF", newsetting.LF.ToString());
                    keyAndData.AddKey("Direccion", newsetting.Direccion?.ToString() ?? "0.0.0.0");
                    keyAndData.AddKey("Identidad", newsetting.Identidad.ToString());
                    keyAndData.AddKey("Tickets", newsetting.Tickets.ToString());
                    keyAndData.AddKey("Espera", newsetting.Espera.ToString());
                    keyAndData.AddKey("ServerIP", newsetting?.ServerIP?.ToString() ?? string.Empty);

                    var seccion = new SectionData(IniFileKey);
                    seccion.Keys = keyAndData;

                    var dataColeccion = new SectionDataCollection();
                    dataColeccion.Add(seccion);

                    IniData setting = new IniData(dataColeccion);

                    parser.WriteFile(fileDirectory, setting);
                }


                try
                {
                    base.CreateBackUp(newsetting);
                }
                catch
                {

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
