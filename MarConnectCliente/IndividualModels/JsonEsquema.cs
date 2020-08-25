using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.IndividualModels
{
    public class JsonEsquema
    {
        public static readonly string TICKET_APUESTA_ESQUEMA = @"

            {
              '$schema': 'http://json-schema.org/draft-04/schema#',
              'type': 'object',
              'properties': {
                
                'NoTicket'         : {'type':'string', 'minLength' : 1},    
                'CodigoAgencia'    : {'type':'string', 'minLength' : 1}, 
                'NombreAgencia'    : {'type':'string', 'minLength' : 1},  
                'CodigoCaja'       : {'type':'string', 'minLength' : 1},  
                'NombreSupervisor' : {'type':'string', 'minLength' : 1},  
                'CedulaSupervisor' : {'type':'string', 'minLength' : 1}, 
                'NombreCajero'     : {'type':'string', 'minLength' : 1},   
                'CedulaCajero'     : {'type':'string', 'minLength' : 1},    
                'Fecha'            : {'type':'string', 'minLength' : 1}, 
                'MontoJugada'      : {'type':'number'},
                'Detalle'          : 
                 {
                  'type':'object',        
                  'properties': {        
                     'Juego': { 
            		 
            		   'type'    : 'array' ,
            		   'minItems': 1,
            		   'items' : {
            		    
            			   'type' : 'object',
            			   
            			   'properties' : {
            			   
                              'TipoJugadaID': {
                                'type': 'integer',
                                'description': 'Establece el tipo de la jugada'
                              },             
                              'EsquemaPagoID': {
                                'type': 'integer',
                                'description': 'Establece el esquema de pago'
                              },
                              'SorteoReferencia': {
                                'type': 'integer',
                                'description': 'Establece la referencia del sorteo'
                              },             
                              'Codigo': {
                                'type': 'string',
                                'description': 'Establece una referencia del sorteo ej: LaFechaDia, NacDia, etc.'
                              },      
                              'SorteoID': {
                                'type': 'integer',
                                'description': 'Id unico del sorteo.'
                              },           
                              'Monto': {
                                'type': 'number',
                                'description': 'Monto apostado por la jugada'
                              },   
                              'Jugada': {
                                'type': 'string',
                                'description': 'Jugada de apuesta',
                                'minLength' : 1
                              },   
                              'SorteoTipo': {
                                'type':  ['string', 'null'],
                                'description': 'tipo de sorteo'
                              }
                           }		   
            		   },
                       'required': [
            		   
                          'TipoJugadaID', 'EsquemaPagoID', 'SorteoReferencia', 'Codigo', 'SorteoID', 'Monto', 'Jugada' 
            			  
                       ]		   
            		 }
            
                   }                         
                 }    
              },
                
              'required': ['Fecha', 'CodigoCaja']              
            }            
        ";

    }// Fin Clase 'JsonEsquema' 

}//Fin Namespace 'IndividualsModels'
