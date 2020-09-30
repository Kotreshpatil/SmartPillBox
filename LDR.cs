using System;
using System.Collections.Generic;
using System.Text;
using System.Device.Gpio;
namespace sqlconnectionapp
{
    public class LDR
    {
        public void LdrMethod()
        {
            int mpin = 17;
            int tpin = 27;
            double cap = 0.000001;
            double adj = 2.130620985;
            int i, t = 0;
            while (true)
            {
                GpioController controller = new GpioController;
                controller.OpenPin(mpin, PinMode.Output);
                controller.OpenPin(tpin, PinMode.Output);
                controller.Write(mpin, PinValue.Low);
                controller.Write(tpin, PinValue.Low);
                System.Threading.Thread.Sleep(2);
                controller.OpenPin(mpin, PinMode.Input);
                System.Threading.Thread.Sleep(2);
                controller.Write(mpin, PinValue.High);
                //DateTime starttime = DateTime.UtcNow;
                //DateTime endtime = DateTime.UtcNow;
                //while (controller.OpenPin(mpin, PinMode.Input) == PinValue.Low)
                //{
                    //endtime = DateTime.UtcNow;
                //}
                //double measuredresistance = endtime - starttime;
                //double res = (measuredresistance / cap) * adj;
                //i = i + 1;
                //t = t + res
                //if (i == 10)
                //{
                    //t = t / i;
                    //Console(t);
                    //DbUpdate A = new DbUpdate();
                    //A.UpdateDbMethod(' ', 0, 1);
                //}
               // i = 0;
               // t = 0;
            }
        }
    }
}
