
using ClienteMarWPFWin7.Domain.Enums;
using ClienteMarWPFWin7.Domain.Models.Dtos;

using System;
using System.IO;
using IniParser;
using IniParser.Model;

namespace ClienteMarWPFWin7.UI.State.LocalClientSetting
{
    public class LocalClientBase
    {
        protected const string BaseDirectory = @"C:\MAR";

        protected const string FileName = "Mar.ini";

        protected const string BackUp_BaseDirectory = @"C:\MAR\Log";

        protected const string BackUp_FileName = "Marbk.ini";

        protected const MarSettingExt FileExtension = MarSettingExt.ini;

        protected const string IniFileKey = "MAR Initialize 2.0";

        protected LocalClientSettingDTO LeerConfiguracionPorDefecto()
        {
            var configuracion = new LocalClientSettingDTO();
            configuracion.BancaId = 1;
            configuracion.LF = 1;
            configuracion.Direccion = "0.0.0.0";
            configuracion.Identidad = "0";
            configuracion.Tickets = 1;
            configuracion.Espera = 60;
            configuracion.ServerIP = string.Empty;
            return configuracion;
        }





        protected LocalClientSettingDTO ReadBackUp()
        {
            try
            {
                var fileDirectory = Path.Combine(new string[] { BackUp_BaseDirectory, BackUp_FileName });
                var parser = new FileIniDataParser();
                var setting = parser.ReadFile(fileDirectory);
                var localsetting = new LocalClientSettingDTO();

                localsetting.BancaId = Convert.ToInt32(setting[IniFileKey]["Banca"]);
                localsetting.LF = Convert.ToInt32(setting[IniFileKey]["LF"]);
                localsetting.Direccion = setting[IniFileKey]["Direccion"];
                localsetting.Identidad = setting[IniFileKey]["Identidad"];
                localsetting.Tickets = Convert.ToInt32(setting[IniFileKey]["Tickets"]);
                localsetting.Espera = Convert.ToInt32(setting[IniFileKey]["Espera"]);
                localsetting.ServerIP = setting[IniFileKey]["ServerIP"];

                return localsetting;
            }
            catch (Exception error)
            {
                throw error;
            }

        }//fin de metodo ReadBackUp 
        protected void CreateBackUp(LocalClientSettingDTO newsetting)
        {
            try
            {
                var fileDirectory = Path.Combine(new string[] { BackUp_BaseDirectory, BackUp_FileName });

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


                    if (!Directory.Exists(BackUp_BaseDirectory))
                    {
                        Directory.CreateDirectory(BackUp_BaseDirectory);
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
            }
            catch (Exception error)
            {
                throw error;
            }

        }//fin  de metodo CreateBackUp












    }//fin de clase LocalClientBase
}
