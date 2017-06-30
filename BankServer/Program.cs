// Copyright 2015, Google Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are
// met:
//
//     * Redistributions of source code must retain the above copyright
// notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following disclaimer
// in the documentation and/or other materials provided with the
// distribution.
//     * Neither the name of Google Inc. nor the names of its
// contributors may be used to endorse or promote products derived from
// this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Threading.Tasks;
using Bank;
using Grpc.Core;

namespace BankServer
{
  internal class BankImpl : Bank.Bank.BankBase
  {
    // Server side handler of the SayHello RPC
    public override Task<HelloReply> SayHello( HelloRequest request, ServerCallContext context )
    {
      return Task.FromResult( new HelloReply {Message = "Hello " + request.Name} );
    }
  }

  internal class Program
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
