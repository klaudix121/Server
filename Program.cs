using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Program
    {

        static TcpListener tcpListener;

        static void Main(string[] args)
        {

            Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelKeyPress);


            //Skapa ett tcpListener objekt, börja lyssna och vänta på anslutning
            IPAddress myIp = IPAddress.Parse("127.0.0.1");
            tcpListener = new TcpListener(myIp, 8001);
            tcpListener.Start();
            
            while (true)
            {

                try
                {
                    Console.WriteLine("Väntar på anslutning...");
                    //Någon försöker avsluta. Acceptera anslutningen 
                    Socket socket = tcpListener.AcceptSocket();
                    Console.WriteLine("Anslutning accepterad från " + socket.RemoteEndPoint);

                    //Tag emot meddelandet
                    Byte[] bMessage = new Byte[256];
                    int messageSize = socket.Receive(bMessage);
                    Console.WriteLine("Meddelandet mottags...");

                    //Konvertera meddelandet till en string-variabel och skriv ut
                    string message = "";
                    for (int i = 0; i < messageSize; i++)
                        message += Convert.ToChar(bMessage[i]);
                    Console.WriteLine("Meddelande: " + message);

                    //kod för att skicka meddelande till klienten
                    Byte[] bSend = System.Text.Encoding.ASCII.GetBytes("Tack!");
                    socket.Send(bSend);
                    Console.WriteLine("Svar skickat");

                    //Stäng anslutningen mot just den klienten
                    socket.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
         
            }

            

        }

        static void CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            tcpListener.Stop();
            Console.WriteLine("Servern stängdes av!");
        }


    }   
}
