using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Tickets.Models;
using System.Collections.Generic;
using Tickets.Queue;
using System.Linq;
using Newtonsoft.Json;

namespace Tickets
{
    public class AtencionTicket : Hub
    {

        private TicketsContextDB ticketsContext;
        private static bool readDB = false;

        public AtencionTicket(TicketsContextDB tc)
        {
            ticketsContext = tc;
        }

        public override async Task OnConnectedAsync()
        {
            var listQueue = new List<Client>();
            bool queueIsEmpty = ClientQueue.queueIsEmpty();
            if (!(readDB && queueIsEmpty))
            {
                listQueue = ticketsContext.Client.Where(c => c.attended == false).ToList();
                foreach (var client in listQueue)
                {
                    if(client.nameQueue == "cola-1")
                        ClientQueue.enqueueQ1(client);
                    else
                        ClientQueue.enqueueQ2(client);

                }
                readDB = true;
            }
            else{
                listQueue = ticketsContext.Client.Where(c => c.attended == false).ToList();
            }
            var jsonList = JsonConvert.SerializeObject(listQueue, Formatting.Indented);
            await Clients.All.SendAsync("startView", jsonList);            
        }

        public async Task NuevoCliente(string idClient, string nameClient)
        {
            Client client = new Client();
            client.idClient = idClient;
            client.nameCient = nameClient;
            client.attended = false;

            ClientQueue queue = ClientQueue.getInstance;
            string nameQueue = ClientQueue.addQueue(client);

            client.nameQueue = nameQueue;
            ticketsContext.Client.Add(client);
            ticketsContext.SaveChanges();

            await Clients.All.SendAsync("agregarCola", nameQueue, idClient, nameClient);
        }

       public async Task removeQueue(string queue)
        {
            if(queue == "q1")
            {
                int id = ticketsContext.Client.Where(c => c.nameQueue == "cola-1" && c.attended == false).Min(c => c.Id);
                Client client = ticketsContext.Client.Where(c => c.Id == id).FirstOrDefault();
                client.attended = true;
                ticketsContext.Client.Update(client);
                ticketsContext.SaveChanges();
                await Clients.All.SendAsync("eliminarCola", "cola-1");
            }
            else
            {
                int id = ticketsContext.Client.Where(c => c.nameQueue == "cola-2" && c.attended == false).Min(c => c.Id);
                Client client = ticketsContext.Client.Where(c => c.Id == id).FirstOrDefault();
                client.attended = true;
                ticketsContext.Client.Update(client);
                ticketsContext.SaveChanges();
                await Clients.All.SendAsync("eliminarCola", "cola-2");
            }
        }
    }
}
