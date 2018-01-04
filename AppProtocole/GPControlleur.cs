using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace AppProtocole
{
    class GPControlleur
    {

        UdpClient listener;
        IPEndPoint ipEndPoint;
       // byte[] panidID = null;

        /// <summary>
        /// Création de la socket pour la COM controlleur
        /// </summary>
        /// <param name="strIpAddress"> Addresse IP du module maitre GreenPriz</param>
        /// <param name="port"> Port du module maitre GreenPriz </param>
        /// 


        public GPControlleur(String strIpAddress, int port)
        {
            IPAddress ipAddress = IPAddress.Parse(strIpAddress);
            this.listener = new UdpClient(port);
            this.ipEndPoint = new IPEndPoint(ipAddress, port);
            this.listener.Connect(this.ipEndPoint);
        }

        ~GPControlleur()
        {
            this.listener.Close();
        }

        public byte[] Send(byte[] request, int timeout = 1000)
        {
            this.listener.Client.ReceiveTimeout = timeout;
            this.listener.Client.SendTimeout = timeout;

            byte[] respons = null;
            try {
                this.listener.Send(request, request.Length);
                IPEndPoint tmp = new IPEndPoint(IPAddress.Any, 0);
                respons = this.listener.Receive(ref tmp); //TMP
               // Array.Reverse(respons);
            }
            catch (Exception e)
            {
                Console.WriteLine("Err conn: " + e.Message);
            }

            return respons;
        }
    }
   
}
