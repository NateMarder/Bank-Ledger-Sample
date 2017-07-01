using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Bank;
using Grpc.Core;

namespace BankServer
{
    internal class BankImpl : Bank.Bank.BankBase
    {
        // Server side handler of the Interact RPC
        public override Task<HelloReply> Interact( InteractionRequest request, ServerCallContext context )
        {
            Task<HelloReply> reply = Task.FromResult( new HelloReply
            {
                Message = "Hello " + request.Name
            } );
            return reply;
        }
    }

    internal class ServerProgram
    {
        private const int Port = 50051;

        public static void Main( string[] args )
        {
            var server = new Server
            {
                Services = {Bank.Bank.BindService( new BankImpl() )},
                Ports = {new ServerPort( "localhost", Port, ServerCredentials.Insecure )}
            };
            server.Start();

            Console.WriteLine( "Bank server listening on port " + Port );
            Console.WriteLine( "Press any key to stop the server..." );
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}