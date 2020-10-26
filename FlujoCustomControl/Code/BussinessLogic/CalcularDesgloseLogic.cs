using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlujoCustomControl.Code.BussinessLogic
{
    class CalcularDesgloseLogic
    {
        private const int M1 = 1;
        private const int M5 = 5;
        private const int M10 = 10;
        private const int M25 = 25;
        private const int M50 = 50;
        private const int M100 = 100;
        private const int M200 = 200;
        private const int M500 = 500;
        private const int M1000 = 1000;
        private const int M2000 = 2000;

        public int result { get; set; }

        public void Calcular(string m1, string m5, string m10, string m25, string m50, string m100, string m200, string m500, string m1000, string m2000)
    {


            int md1 = m1 == "" ? 0 : M1 * int.Parse(m1); 
            int md5 = m5 == "" ? 0 : M5 * int.Parse(m5); 
            int md10 = m10 == "" ? 0 : M10 * int.Parse(m10); 
            int md25 = m25 == "" ? 0 : M25 * int.Parse(m25); 
            int md50 = m50 == "" ? 0 : M50 * int.Parse(m50); 
            int md100 = m100 == "" ? 0 : M100 * int.Parse(m100); 
            int md200 = m200 == "" ? 0 : M200 * int.Parse(m200); 
            int md500 = m500 == "" ? 0 : M500 * int.Parse(m500); 
            int md1000 = m1000 == "" ? 0 : M1000 * int.Parse(m1000); 
            int md2000 = m2000 == "" ? 0 : M2000 * int.Parse(m2000); 
   

            result =  md1 + md5 + md10 + md25 + md50 + md100 + md200 + md500 + md1000 + md2000;
    }





    }
}
