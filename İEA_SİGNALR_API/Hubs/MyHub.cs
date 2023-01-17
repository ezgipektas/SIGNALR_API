using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace İEA_SİGNALR_API.Hubs
{
    public class MyHub:Hub
    {
        public static List<string> Names { get; set; } = new List<string>(); //Names prop nin içine atama yapabilmek için bir nesneye çevirdik.

        private static int ClientCount { get; set; } = 0; //İstek sayısı
        public async Task SendName(string name)
        {
            Names.Add(name); //Names içine name değerleri eklendi
            await Clients.All.SendAsync("ReceiveName", name); //İstek yapan herkesi getir.(Burada Receive Name istemci üzerinden çağırılacak metodun ismi
        }
        public async Task GetNames()
        {
            await Clients.All.SendAsync("ReceiveNames", Names); //Tüm isimleri getirecek.
        }
        public async override Task OnConnectedAsync() //Tüm Olumlu bağlantı sayısını getircek
        {
            ClientCount++;
            await Clients.All.SendAsync("ReceiveClientCount", ClientCount);
        }

        public async override Task OnDisconnectedAsync(Exception exception)//Tüm Olumsuz bağlantı sayısını getircek
        {
            ClientCount--;
            await Clients.All.SendAsync("ReceiveClientCount", ClientCount);
        }
    }
}
