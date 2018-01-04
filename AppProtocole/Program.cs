using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppProtocole
{
    class Program
    {
        const string ipAddress = "192.168.1.178";
        const int port = 33333;
        const string macAddress = "9E-A0-01-36";
        const byte[] panidID = { 0xc4, 0x6d };

        static void Main(string[] args)
        {
            GPControlleur controlleur = new GPControlleur(ipAddress,port);
            GPModule[] module  = new GPModule[101]; 
            module[0] = new GPModule(controlleur, macAddress);
            while (true) { 
                Console.WriteLine(module[0].GetConso());
                Console.WriteLine(module[0].GetState());
                Thread.Sleep(500);
            }
        }
    }
}
