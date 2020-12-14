using Dapper;

using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;

using MAR.AppLogic.MARHelpers;
using MAR.DataAccess.ControlEfectivoRepositories.Helpers;
using MAR.DataAccess.Tables.ControlEfectivoDTOs;

namespace MAR.DataAccess.ControlEfectivoRepositories
{
    public static class MultipleRepository
    {
        public static MultipleDTO<MUsuarioDTO, CajaDTO, TarjetaDTO> LeerUsuarioSuCajaYSuTarjetaPorPinDeUsuario(string pin)
        {
            try
            {

                var p = new DynamicParameters();
                p.Add("@pin", pin);

                var multi = new MultipleDTO<MUsuarioDTO, CajaDTO, TarjetaDTO>();

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();

                    using (var queryMultiple = db.QueryMultiple(MultipleHelper.LeerUsuarioSuCajaYSuTarjeta, p, commandType: CommandType.Text))
                    {
                        multi.PrimerDTO = queryMultiple.Read<MUsuarioDTO>().FirstOrDefault();
                        multi.SegundoDTO = queryMultiple.Read<CajaDTO>().FirstOrDefault();
                        multi.TercerDTO = queryMultiple.Read<TarjetaDTO>().FirstOrDefault();

                        if (multi.TercerDTO != null && multi.TercerDTO.JsonTokens != string.Empty)
                        {
                            multi.TercerDTO.Tokens = JSONHelper.CreateNewFromJSONNullValueIgnore<List<List<string>>>(multi.TercerDTO.JsonTokens);

                            multi.TercerDTO.InlineTokens = new List<string>();

                            for (int fila = 0; fila < multi.TercerDTO.Tokens.Count; fila++)
                            {
                                foreach (var item in multi.TercerDTO.Tokens[fila])
                                {
                                    multi.TercerDTO.InlineTokens.Add(item);
                                }
                            }

                        }// fin de if 

                    }// fin de using

                    db.Close();
                }

                return multi;
            }
            catch (Exception e)
            {
                throw e;
            }
        }







    }//fin de clase
}
