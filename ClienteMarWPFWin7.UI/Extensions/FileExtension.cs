
using ClienteMarWPFWin7.UI.State.LocalClientSetting;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using System;
using System.IO;
using IniParser;
using IniParser.Model;

namespace ClienteMarWPFWin7.UI.Extensions
{
    public class FileExtension : LocalClientBase
    {

        public static void Iniciar() 
        {
            var extension = new FileExtension();
            extension.CreateAppRequiredFiles();        
        }


        private void CreateAppRequiredFiles()
        {
            if (!Directory.Exists(BaseDirectory))
            {
                try
                {
                    // Este codigo de ejecuta cuando la carpeta de MAR no ha sido instalada correctamente
                    Directory.CreateDirectory(BaseDirectory);
                    LocalClientSettingDTO configuracion = base.LeerConfiguracionPorDefecto();
                    CrearMarIni(configuracion);
                    CrearMarIniBackUp(configuracion);
                }
                catch  
                {

                }//fin de catch

            }//fin de if

        }//fin de metodo


        private void CrearMarIni(LocalClientSettingDTO configuracion) 
        {
            try
            {
                var fileDirectory = Path.Combine(new string[] { BaseDirectory, FileName });               
                var parser = new FileIniDataParser();
                var keyAndData = new KeyDataCollection();
                keyAndData.AddKey("Banca", configuracion.BancaId.ToString());
                keyAndData.AddKey("LF", configuracion.LF.ToString());
                keyAndData.AddKey("Direccion", configuracion.Direccion?.ToString() ?? "0.0.0.0");
                keyAndData.AddKey("Identidad", configuracion.Identidad.ToString());
                keyAndData.AddKey("Tickets", configuracion.Tickets.ToString());
                keyAndData.AddKey("Espera", configuracion.Espera.ToString());
                keyAndData.AddKey("ServerIP", configuracion?.ServerIP?.ToString() ?? string.Empty);

                var seccion = new SectionData(IniFileKey);
                seccion.Keys = keyAndData;

                var dataColeccion = new SectionDataCollection();
                dataColeccion.Add(seccion);

                IniData setting = new IniData(dataColeccion);

                parser.WriteFile(fileDirectory, setting);
            }
            catch 
            {

                  
            }//fin de catch 
        }//fin de metodo

        private void CrearMarIniBackUp(LocalClientSettingDTO configuracion) 
        {
            try
            {
                base.CreateBackUp(configuracion);
            }
            catch  
            {

            }//fin de catch

        }//fin de metodo






    }//fin de clase
}//fin de namespace
