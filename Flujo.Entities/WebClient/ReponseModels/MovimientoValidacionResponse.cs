using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Flujo.Entities.WebClient.RequestModels;
using Flujo.Entities.WebClient.POCO;
namespace Flujo.Entities.WebClient.ReponseModels
{
    public class MovimientoValidacionResponse
    {
        public List<FormMovimientoError> Errors { get; set; }
        public MovimientoRequestModel Request { get; set; }
    }
}
