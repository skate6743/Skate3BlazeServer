# Project structure

## Overview
The "Servers" folder contains all 3 mandatory servers for having online functioning. This includes a HTTP server, a QoS

UDP server and finally the main Blaze TCP server.


Below are explanations for each one:

## HttpServer.cs
Mandatory server for authentication flow, serves a PS3.xml config file (found in the wwwroot folder), which

contains online related variables such as map moving timers, etc. and you can freely edit these in the .xml file.

The HTTP server also provides a XML which includes the UDP QoS server port and IP (when game makes a request to "qos/qos"), this

is mandatory for the game to know where to send those UDP requests during QoS tests and matchmaking won't work without it.

## QoSServer.cs
UDP Server used during the authentication flow for testing network connection. Here is the flow:

1. Game sends a ping UDP request thats 20 bytes to the QoS Server -> QoS server echoes with the same bytes but includes

users IP and port as bytes at the end of the request.

2. After the ping request, the game will send x amount of "probes" to the QoS Server -> QoS server always echoes with the

exact same bytes back.


NOTE: The amount of probes and the size of them is given to the game by the HTTP server in the "qos/qos" request


## BlazeServer.cs
This is the main TCP server responsible for handling all Blaze components like Authentication, Gamemanager, Util, etc.

You can find all command packet structures and their Handlers in the "Blaze/Components" folder, which is used for handling all commands.

Further explanations for authentication and matchmaking flows, etc. coming soon!