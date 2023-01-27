using İEA_SİGNALR_API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace İEA_SİGNALR_API.Hubs
{
    public class MyHub:Hub
    {
        private readonly Context _context;

        public MyHub(Context context)
        {
            _context = context;
        }

        public static List<string> Names { get; set; } = new List<string>(); //Names prop nin içine atama yapabilmek için bir nesneye çevirdik.

        private static int ClientCount { get; set; } = 0; //İstek sayısı
        public static int roomCount { get; set; } = 7; //başlangıçta odaya erişebilcek kişi sayısı atama yaptık.
        
        public async Task SendName(string name)
        {
            if (Names.Count >= roomCount)
            {
                await Clients.Caller.SendAsync("Error", $"Bir oda en fazla {roomCount} kişi olabilir");
            }
            else
            {
                Names.Add(name); //Names içine name değerleri eklendi
                await Clients.All.SendAsync("ReceiveName", name); //İstek yapan herkesi getir.(Burada Receive Name istemci üzerinden çağırılacak metodun ismi
            }
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
        public async Task SendNameByRoom(string name,string roomName)
        {
            var room = _context.Rooms.Where(x => x.Name == roomName).FirstOrDefault();
            if(room != null)
            {
                room.Users.Add(new User { Name = name });
            }
            else
            {
                var newRoom=new Room { Name = roomName };
                newRoom.Users.Add(new User { Name = name });
                _context.Rooms.Add(newRoom);
            }
            await _context.SaveChangesAsync();
            await Clients.Group(roomName).SendAsync("ReceiveMessageByGroup", name, room.Id);
        }
        public async Task GetNamesByGroup()
        {
            var rooms = _context.Rooms.Include(x => x.Users).Select(y => new
            {
                roomId = y.Id,
                Users = y.Users.ToList()
            });
            await Clients.All.SendAsync("ReceiveNamesByGroup", rooms);
        }
        public async Task AddToGroup(string roomName)//Odaya kişi ekliyor
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }
        public async Task RemoveToGroup(string roomName)//Odadan kişi siliyor
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }
    }
}
