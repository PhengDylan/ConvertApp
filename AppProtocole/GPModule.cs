using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AppProtocole
{
    class GPModule
    {
        PhysicalAddress macAddress;
        bool state ;
        byte[] panidID;
       

        GPControlleur controlleur;

        public GPModule(GPControlleur controlleur, String strMacAddress)
        {
            this.controlleur = controlleur;
            this.macAddress = PhysicalAddress.Parse(strMacAddress);
            this.state = false;
            
        }
        public double GetConso()
        {

            const int cmdReadConso = 0x07;
            byte[] data =  Enumerable.Repeat((byte)0x00, 12).ToArray();
            byte [] rawResponse = controlleur.Send(GenReq(cmdReadConso, data));
            if (rawResponse == null) {
                Console.WriteLine("Err lecture conso");
                return -1;
            }
            else {
                return (double)(BitConverter.ToInt16(rawResponse,18)) / 317.3 * 230.0;
            }
        }
        public int GetState()
        {
            const int cmdReadState = 0x0C;
            byte[] data = Enumerable.Repeat((byte)0x00, 16).ToArray();
            byte[] rawResponse = controlleur.Send(GenReq(cmdReadState, data));
            if (rawResponse == null)
            {
                Console.WriteLine("Err lecture etat");
                return -1;
            }
            else
            {
                return (int)(BitConverter.ToInt16(rawResponse, 21));
            }

        }
        public void SetState(int value,byte[] panID)
        {
            const int cmdReadState = 0x0B;
            byte[] data1 = Enumerable.Repeat((byte)0x00, 9).ToArray();
            byte[] date = { 0x00, 0x7F, 0xFF, 0xFF };
            byte[] data2 = Enumerable.Repeat((byte)0x00, 5).ToArray();
            byte[] data
            


        }
      /*  public byte[] GetInfo()
        {
            const int cmdReadState = 0x0C;
            byte[] errorCode = { 0xFF, 0xFF };
            byte[] data = Enumerable.Repeat((byte)0x00, 16).ToArray();
            byte[] rawResponse = controlleur.Send(GenReq(cmdReadState, data));
            if (rawResponse == null)
            {
                Console.WriteLine("Err lecture etat");
                return errorCode;
            }
            else
            {

                return (int)(BitConverter.ToInt16(rawResponse, 21));
            }
        }*/

        private byte [] GenReq (int cmd, byte[] data, int moduleID = -1, int setState = -1)
        {
            
            byte[] bMacAddress = macAddress.GetAddressBytes();
            Array.Reverse(bMacAddress);

            byte[] req = new byte[data.Length+10]; // Taille  code init + code fonction + Mac Address(4) + data + checksum(2) + code fin(2)
            req[0] = 0x10;
            req[1] = (byte)cmd;
            if(moduleID != -1){
                req[2] = (byte)moduleID;
            }
            Array.Copy(bMacAddress, 0, req, 2, bMacAddress.Length);
            Array.Copy(data, 0, req, 2+ bMacAddress.Length, data.Length);

            int checksum = CalcChecksum(req, 2, req.Length - 2);
            int offsetChecksum = 2 + bMacAddress.Length + data.Length;
            req[offsetChecksum] = (byte)checksum;
            req[offsetChecksum + 1] = (byte)(checksum >> 8);

            req[offsetChecksum + 2] = 0x10;
            req[offsetChecksum + 3] = 0xFF;
            return req;
        }



        private int CalcChecksum(byte[] inputbuffer, int pos, int amount)
        {
            int chk = 0;
            int i;
            for (i = 0; i < amount; i++)
            {
                chk = chk + inputbuffer[pos + i];
            }
            chk = ~(chk);
            return (chk);
        }
    }
}
