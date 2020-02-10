# Simple Smart Chat App

Simple realtime chat application developed using .Net Core 3.1, SignalR, RabbitMQ, Worker Service, and C# programming language.

# Installation

Getting a working copy requires the following steps to be completed:
- Clone the repository using git on cmd or using Github Desktop `git clone https://github.com/carlos0202/simple-smart-chat.git`.
- Configure RabbitMQ connection for local or remote use. In both projects inside the repository, the rabbitMQ connection points to the default values while running on a docker container, follow [this](https://levelup.gitconnected.com/rabbitmq-with-docker-on-windows-in-30-minutes-172e88bb0808) link for more information about setting up rabbitMQ locally using docker.
- Open the contained solution inside the repository using Visual Studio 2019 (16.4+ version) or with Visual Studio Code with ASP.NET Core 3.1 SDK installed (Already included with Visual Studio 2019 16.4).
- Run entity framework migrations to restore the database to the correct version and with the right schema. SQL Server Express localdb is the current database used to test the project.
- With RabbitMQ already running launch both projects from the console or the Visual Studio 2019 IDE taking into consideration that *Simple.Smart.Chat.App* app requires *Simple.Smart.Chat.CommandBot* already running to process commands correctly.
Open required browser instances to test real-time chat functionality across different logged in users (Account creation is needed to use the chat).


# System Requirements

In order to run the App in your local machine with the default options configured, you must comply with the next list of system/software requirements:

-  [Windows Client: 7, 8.1, 10 (1607+)](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-1/)
- [Microsoft SQL Server Express LocalDb](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver15) 13.0.4001.0 o superior
- [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download/visual-studio-sdks)
- [Visual studio 2019 16.4](https://visualstudio.microsoft.com/es/downloads/) ([*Recommended*](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-1/))
