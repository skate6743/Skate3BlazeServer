# Skate 3 Custom Blaze Server
Barebones custom Skate 3 Blaze server for RPCS3 with functional matchmaking. Skate web features such as Import Skaters, Parks, etc. will be implemented eventually too :)

## Setup Guide
Testing locally: Set IP/Hosts switches in Network config to gosredirector.ea.com==127.0.0.1&&downloads.skate.online.ea.com==127.0.0.1

Hosting publicly: Port forward port 42100 for both UDP and TCP, and port 80 for TCP. Lastly set localhost to false in Program.cs and recompile.

## Special Thanks
[@Aim4Kill](https://github.com/Aim4kill) for making the [BlazeSDK](https://github.com/Aim4kill/BlazeSDK) (Saved so much time with having the packet structures there for almost all Blaze commands)

[@gamingrobot](https://github.com/gamingrobot) for amazing documentation on the TDF format used in Blaze servers.

[New Blaze Emulator](https://github.com/Tratos/New-Blaze-Emulator) by [@Tratos](https://github.com/Tratos) (Example Blaze server with working matchmaking)

[NPTicket](https://github.com/LittleBigRefresh/NPTicket) by [LittleBigRefresh Team](https://github.com/LittleBigRefresh) (Used for RPCN ticket validation)

## Disclaimer
Not affiliated, associated, authorized, endorsed by, or in any way officially connected with Electronic Arts Inc. or any of its subsidiaries or affiliates. The use of any trademarks, logos, or brand names is for identification purposes only and does not imply endorsement or affiliation.
