using System;
using Bank;
using Grpc.Core;

namespace BankClient
{
  internal class ClientProgram
  {
    public static void Main( string[] args )
    {
      var channel = new Channel( "127.0.0.1:50051", ChannelCredentials.Insecure );

      var client = new Bank.Bank.BankClient( channel );
      var user = "you";
      var reply = client.SayHello( new HelloRequest {Name = user} );

      Console.WriteLine( "Greeting: " + reply.Message );
      Console.WriteLine( "Welcome to the echo chamber..." );

      var input = Console.ReadLine();
      while ( input != "exit" )
      {
        Console.WriteLine( input = Console.ReadLine() );
      }

      channel.ShutdownAsync().Wait();
    }


  }
}
