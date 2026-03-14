# Project structure

## Overview
The "Servers" folder contains all 3 mandatory servers for having online functioning. This includes a HTTP server, UDP Relay servers used for lobbies and finally the main Blaze TCP server.

Below are explanations for each one:

## HttpServer.cs
Mandatory server for authentication flow, serves a PS3.xml config file (found in the wwwroot folder), which contains online related variables such as map moving timers, etc. and you can freely edit these in the .xml file.


The HTTP server also provides a XML which includes the UDP QoS server port and IP (when game makes a request to "qos/qos"), this 
is mandatory for the game to know where to send those UDP requests during QoS tests and matchmaking won't work without it.

## RelayServer.cs
Relay server used for lobbies to prevent players connecting via Peer to Peer. These relay servers fix NAT type compatibility issues when matchmaking and protect players IPs from being exposed to each other.


Player sends a UDP request to relay server -> Relay server forwards that request to all other players in the lobby.


## BlazeServer.cs
This is the main TCP server responsible for handling all Blaze components like Authentication, Gamemanager, Util, etc.


You can find all command packet structures and their Handlers in the "Blaze/Components" folder, which is used for handling all commands.


Further explanations for authentication and matchmaking flows, etc. coming soon!
