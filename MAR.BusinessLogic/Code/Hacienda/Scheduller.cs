using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Code.Hacienda
{
    public class Scheduller
    {
        public class MyRegistry : Registry
        {
            public MyRegistry()
            {
                // Schedule an IJob to run at an interval
               // Schedule<EnviaTicketsFueraDeLinea>().ToRunNow().AndEvery(10).Hours();
            

                //Este metodo se debe correr desde que se inicia el dia para que siga corriendo automaticamente
                //Schedule<EnviaTicketsFueraDeLinea>().ToRunNow().AndEvery(10).Hours();

                //Este metodo se debe eventualmente basado en la base de datos
                //Schedule<CargarGanadoresDeCamioMillonario>().ToRunNow().AndEvery(30).Seconds();


                //// Schedule an IJob to run once, delayed by a specific time interval
                //Schedule<EnviaTicketsFueraDeLinea>().ToRunOnceIn(15).Minutes();

                //// Schedule a simple job to run at a specific time
                //Schedule(() => Console.WriteLine("It's 9:15 PM now.")).ToRunEvery(1).Days().At(21, 15);

                //// Schedule a more complex action to run immediately and on an monthly interval
                //Schedule<MyComplexJob>().ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Monday).At(3, 0);

                //// Schedule multiple jobs to be run in a single schedule
                //Schedule<MyJob>().AndThen<MyOtherJob>().ToRunNow().AndEvery(5).Minutes();
            }
       

        }
        public class EnviaTicketsFueraDeLinea : IJob
        {
            public void Execute()
            {
                MAR.BusinessLogic.Code.Hacienda.ApuestaFueraDeLinea.Apuesta();
            }
        }
        public class CargarGanadoresDeCamioMillonario: IJob
        {
            public void Execute()
            {
                MAR.BusinessLogic.Code.SorteosMar.PagoGanadorLogic.CargaGanadoresCamionMillonario(0,null,null, DateTime.Now.ToString());
            }
        }
    }
}
