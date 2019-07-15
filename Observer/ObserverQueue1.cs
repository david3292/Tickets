using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tickets.Observer
{
    public class ObserverQueue1 : IHostedService
    {
        public static HubConnection connection;
        public static readonly int tiempo1 = 120000;
        public static readonly int tiempo2 = 180000;
        public static int count = 0;
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if(connection == null)
                {
                    connection = new HubConnectionBuilder()
                        .WithUrl("ws://localhost:63420/servicio-tickets")
                        .Build();

                    await connection.StartAsync();

                    Console.WriteLine("Coneccion exitosa");                    
                }
                Console.WriteLine("Tarea iniciada");

                if(count == 0)
                {
                    await Task.Delay(120000);
                    await connection.InvokeAsync("removeQueue", "q1");
                    count++;
                }
                if (count == 1)
                {
                    await Task.Delay(60000);
                    await connection.InvokeAsync("removeQueue", "q2");
                    count++;
                }
                if (count == 2)
                {
                    await Task.Delay(60000);
                    await connection.InvokeAsync("removeQueue", "q1");
                    count++;
                }
                if (count == 3)
                {
                    await Task.Delay(120000);
                    await connection.InvokeAsync("removeQueue", "q1");
                    await connection.InvokeAsync("removeQueue", "q2");
                    count = 0;
                }
            }            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
