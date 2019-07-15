using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tickets.Models;

namespace Tickets.Queue
{

    public class ClientQueue
    {
        private static Queue<Client> queue1 = new Queue<Client>();
        private static Queue<Client> queue2 = new Queue<Client>();

        private static ClientQueue _instance = new ClientQueue();

        public static ClientQueue getInstance {
            get
            {
                return _instance;
            }
        }

        public static string addQueue(Client client)
        {
            if (queue1.Count == 0 && queue2.Count == 0)
            {
                queue1.Enqueue(client);
                return "cola-1";
            }
            else
            {
                if (queue1.Count > 1 && queue2.Count == 0)
                {
                    queue2.Enqueue(client);
                    return "cola-2";
                }
                else
                {
                    var timeQ1 = queue1.Count * 2;
                    var timeQ2 = queue2.Count * 3;
                    if (timeQ1 <= timeQ2)
                    {
                        queue1.Enqueue(client);
                        return "cola-1";
                    }
                    if (timeQ2 < timeQ1)
                    {
                        queue2.Enqueue(client);
                        return "cola-2";
                    }
                    return "";
                }
            }

            
        }

        public static void removeQueue(string queueName)
        {
            if (queueName == "q1")
            {
                if(queue1.Count > 0)
                    queue1.Dequeue();
            }
            else
            {
                if (queue1.Count > 0)
                    queue2.Dequeue();
            }
        }

        public static bool queueIsEmpty()
        {
            if (queue1.Count == 0 && queue2.Count == 0) return true;
            else return false;
        }

        public static void enqueueQ1(Client client)
        {
            queue1.Enqueue(client);
        }

        public static void enqueueQ2(Client client)
        {
            queue2.Enqueue(client);
        }
    }
}
