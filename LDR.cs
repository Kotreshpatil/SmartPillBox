using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Device.Gpio;
using System.Reflection;

namespace sqlconnectionapp
{
    public class LDR
    {
        public void LdrMethod()
        {
            double value = 0; //used to store LDR value
            int ldr = 17;
            int led = 11;
            GpioController gpioController = new GpioController();
            gpioController.OpenPin(led, PinMode.Output);

            static int rcTime(int ldr)
            {
                int count = 0;
                GpioController gpioController = new GpioController();
                gpioController.SetPinMode(ldr, PinMode.Output);
                gpioController.Write(ldr, false);
                System.Threading.Thread.Sleep(2);
                gpioController.SetPinMode(ldr, PinMode.Input);
                while (gpioController.Read(ldr) == 0)
                {
                    count++;
                }

                return count;
            }
            while (true)
            {
                Console.WriteLine("The LDR Values are");
                value = rcTime(ldr);
                Console.WriteLine(value);
                if (value >= 100000)
                {
                    DbUpdate A = new DbUpdate();
                    A.UpdateDbMethod(' ', 0, 1);
                }

            }
        }
    }
}
