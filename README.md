# Skate 3 Custom Blaze Server
Barebones custom Skate 3 Blaze server for RPCS3 with functional matchmaking. Skate web features such as Import Skaters, Parks, etc. will be implemented eventually too :)

# Setup Guide
## Joining our public server:
##Automated setup (Recommended for Windows):
1. Open RPCS3, and at the top bar click on Help->Check for Updates, and proceed with updating to latest version. After updating just close out of RPCS3.
2. Download this tool which auto adjusts your config with the right online related settings: [Skate 3 Config Adjuster by BWKingsnake](https://github.com/bwkingsnake/rpcs3-skate-3-config-editor/releases/download/configEditor/Release.7z)
3. Run the "configEditor.exe" file inside that archive and locate to your rpcs3.exe path when it asks. After this you can close out of the configEditor.
4. Open your RPCS3, click on the "RPCN" icon (right next to pads settings), and go to Account->Create Account if you don't have one already. You will need to fully go through the account creation process with verifying your email, etc.
5. Now you can boot Skate 3 and connect to EA Nation
##Manual setup (Recommended for Linux):
1. testing
## Testing locally:
Set IP/Hosts switches in Network config to gosredirector.ea.com==127.0.0.1&&downloads.skate.online.ea.com==127.0.0.1
## Hosting publicly:
Port forward port 42100 for both UDP and TCP, and port 80 for TCP. Lastly set LocalHost to false in settings.json and launch the server.

## Playing with friends via Radmin VPN:
Take your Radmin IP (26.x.x.x), and in the settings.json set LocalHost to true and LocalIPAddress to your Radmin IP. Lastly you and your friends must set their IP/Hosts switches in  Network tab to "gosredirector.ea.com==RadminIPhere&&downloads.skate.online.ea.com==RadminIPhere"

# Special Thanks
[@Aim4Kill](https://github.com/Aim4kill) for making the [BlazeSDK](https://github.com/Aim4kill/BlazeSDK) (Saved so much time with having the packet structures there for almost all Blaze commands)

[@gamingrobot](https://github.com/gamingrobot) for amazing documentation on the TDF format used in Blaze servers.

[New Blaze Emulator](https://github.com/Tratos/New-Blaze-Emulator) by [@Tratos](https://github.com/Tratos) (Example Blaze server with working matchmaking)

[NPTicket](https://github.com/LittleBigRefresh/NPTicket) by [LittleBigRefresh Team](https://github.com/LittleBigRefresh) (Used for RPCN ticket validation)

# Disclaimer
Not affiliated, associated, authorized, endorsed by, or in any way officially connected with Electronic Arts Inc. or any of its subsidiaries or affiliates. The use of any trademarks, logos, or brand names is for identification purposes only and does not imply endorsement or affiliation.
