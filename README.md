# Skate 3 Custom Blaze Server
Barebones custom Skate 3 Blaze server for RPCS3 with functional matchmaking. Skate web features such as Import Skaters, Parks, etc. will be implemented eventually too :)

# Joining Our Public Server
Before proceeding with these instructions, if you are someone that plays on the Party Play EBOOT to have your own skater in Party Play, please download and install this original EBOOT to RPCS3/dev_hdd0/game/BLUS30464 (or BLES00760 for BLES)/USRDIR [ORIGINAL EBOOT LINK](https://www.mediafire.com/file/jdwv2z1k49wwu6v/EBOOT.BIN/file)
### Automated setup (Recommended for Windows):
1. Open RPCS3, and at the top bar click on Help->Check for Updates, and proceed with updating to latest version. After updating just close out of RPCS3.
2. Download this tool which auto adjusts your config with the right online related settings: [Skate 3 Config Adjuster by BWKingsnake](https://github.com/bwkingsnake/rpcs3-skate-3-config-editor/releases/download/configEditor/Release.7z)
3. Run the "configEditor.exe" file inside that archive and locate to your rpcs3.exe path when it asks.
4. Open your RPCS3, click on the "RPCN" icon (right next to pads settings), and go to Account->Create Account if you don't have one already. You will need to fully go through the account creation process with verifying your email, etc.
5. Now you can boot Skate 3 and connect to EA Nation!
### Manual setup (Recommended for Linux):
1. Open RPCS3, and at the top bar click on Help->Check for Updates, and proceed with updating to latest version.
2. On RPCS3 right click on Skate 3, and click on "Change Custom Configuration" (if you have not made one yet, click on "Create Custom Configuration")
3. Head over to the CPU tab on the custom config, and set "XFloat Accuracy" to "Approximate", and make sure "Enable SPU loop detection" is unticked.
4. Go to Network tab, and set Network Status to Connected, PSN Status to RPCN, turn UPNP on at the bottom.
5. For the IP/Hosts switches field, paste in this: gosredirector.ea.com==172.237.109.212&&downloads.skate.online.ea.com==172.237.109.212
6. Save custom configuration and exit this config editor window.
7. Click on the "RPCN" icon (right next to pads settings), and go to Account->Create Account if you don't have one already. You will need to fully go through the account creation process with verifying your email, etc.
8. Now you can boot Skate 3 and connect to EA Nation!
   
# Manual Hosting Guide
### Hosting publicly
Forward port 42100 for both UDP and TCP, and port 80 for TCP. Set LocalHost to false in settings.json and run the server executable. Now in RPCS3 config under Network tab you need to set IP/Hosts switches to "gosredirector.ea.com==YourPublicIPHere&&downloads.skate.online.ea.com==YourPublicIPHere"
### Playing locally with friends using Radmin VPN
Take your Radmin IP (26.x.x.x), and in the settings.json set LocalHost to true and LocalIPAddress to your Radmin IP. Now run the server executable and you and your friends must set their IP/Hosts switches in Network tab to "gosredirector.ea.com==YourRadminIPhere&&downloads.skate.online.ea.com==YourRadminIPhere" (Everyone sets their config to your Radmin IP since you are the one hosting the server!)
# Special Thanks
[@Aim4Kill](https://github.com/Aim4kill) for making the [BlazeSDK](https://github.com/Aim4kill/BlazeSDK) (Saved so much time with having the packet structures there for almost all Blaze commands)

[@gamingrobot](https://github.com/gamingrobot) for amazing documentation on the TDF format used in Blaze servers.

[New Blaze Emulator](https://github.com/Tratos/New-Blaze-Emulator) by [@Tratos](https://github.com/Tratos) (Example Blaze server with working matchmaking)

[NPTicket](https://github.com/LittleBigRefresh/NPTicket) by [LittleBigRefresh Team](https://github.com/LittleBigRefresh) (Used for RPCN ticket validation)

# Disclaimer
Not affiliated, associated, authorized, endorsed by, or in any way officially connected with Electronic Arts Inc. or any of its subsidiaries or affiliates. The use of any trademarks, logos, or brand names is for identification purposes only and does not imply endorsement or affiliation.
