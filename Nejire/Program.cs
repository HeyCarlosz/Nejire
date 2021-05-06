using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;

namespace Nejire
{
    class Program
    {
        public static void Main(string[] args)
            => new Nejire().MainAsync().GetAwaiter().GetResult();
    }
}
