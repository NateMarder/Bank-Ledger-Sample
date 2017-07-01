gRPC in 3 minutes (C#)
========================

BACKGROUND
-------------
For this sample, we've already generated the server and client stubs from [helloworld.proto][].


# About GRPC:
-------------

## What is GRPC?
- http://blog.codeclimate.com/images/posts/pb_json.png
- “Protocol Buffers are a way of encoding structured data in an efficient yet extensible format.”
- Google developed Protocol Buffers for use in their internal services. 
- Protocol buffers provide an encoding format that allows you to specify a schema for your data using a specification language, like so:
<code>

	message BankTeller {
		required int32 id = 1;
		required string name = 2;
		optional string content = 3;
	}

</code>

- Windows: .NET Framework 4.5+, Visual Studio 2013 or 2015
- Linux: Mono 4+, MonoDevelop 5.9+
- Mac OS X: Xamarin Studio 5.9+

You can also run the server and client directly from the IDE.
On Linux or Mac, use `mono BankServer.exe` and `mono BankClient.exe` to run the server and client.
