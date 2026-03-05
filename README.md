# Skate 3 Custom Blaze Server
Barebones custom Skate 3 Blaze server for RPCS3 with functional matchmaking. Skate web features such as Import Skaters, Parks, etc. will be implemented eventually too :)

# Joining Our Public Server
Before proceeding with these instructions, if you are someone that plays on the Party Play EBOOT to have your own skater in Party Play, please download and install this [Original EBOOT](https://www.mediafire.com/file/jdwv2z1k49wwu6v/EBOOT.BIN/file) to RPCS3/dev_hdd0/game/BLUS30464 (or BLES00760 for BLES)/USRDIR. If you don't know what the Party Play EBOOT is then you can skip this step!
### Automated setup (Recommended for Windows):
[VIDEO GUIDE](https://www.youtube.com/watch?v=i5fHEhsqzA4)
1. Open RPCS3, and at the top bar click on Help->Check for Updates, and proceed with updating to latest version. After updating just close out of RPCS3.
2. Download this tool which auto adjusts your config with the right online related settings: [Skate 3 Config Adjuster by BWKingsnake](https://github.com/bwkingsnake/rpcs3-skate-3-config-editor/releases/download/configEditor/Release.7z)
3. Run the "configEditor.exe" file inside that archive and locate to your rpcs3.exe path when it asks.
4. Open your RPCS3, click on the "RPCN" icon (right next to pads settings), and go to Account->Create Account if you don't have one already. You will need to fully go through the account creation process with verifying your email, etc.
5. Now you can boot Skate 3 and connect to EA Nation!
### Manual setup (Recommended for Linux):
1. Open RPCS3, and at the top bar click on Help->Check for Updates, and proceed with updating to latest version.
2. On RPCS3 right click on Skate 3, and click on "Change Custom Configuration" (If you have not made one yet, click on "Create Custom Configuration")
3. Head over to the CPU tab on the custom config, and set "XFloat Accuracy" to "Approximate", and make sure "Enable SPU loop detection" is unticked.
4. Go to Network tab, and set Network Status to Connected, PSN Status to RPCN, turn UPNP on at the bottom.
5. For the IP/Hosts switches field, paste in this: gosredirector.ea.com==172.237.109.212&&downloads.skate.online.ea.com==172.237.109.212
6. Save custom configuration and exit this config editor window.
7. Click on the "RPCN" icon (right next to pads settings), and go to Account->Create Account if you don't have one already. You will need to fully go through the account creation process with verifying your email, etc.
8. Now you can boot Skate 3 and connect to EA Nation!
   
# Manual Hosting Guide
Before proceeding with these instructions, please download the [Compiled Server Files](https://github.com/skate6743/Skate3BlazeServer/releases/download/release/Skate3BlazeServer.rar)
### Playing locally with friends using Radmin VPN
[VIDEO GUIDE](https://youtu.be/WWUAOQ3_M1g)
1. You and everyone you play with must download [Radmin VPN](https://www.radmin-vpn.com/)
2. Once Radmin is open, click the red button to go online.
3. Go to Network->Create Network and give the Network a name and password.
4. All your friends must go to Network->Join Network and type in your Network name and Password.
5. Right click on Skate 3, and click on Change Custom Configuration (If you haven't made a custom config yet then click on Create Custom Configuration)
6. In the CPU tab set XFloat Accuracy to Approximate and make sure Enable SPU loop detection is unticked.
7. Go to Network tab, and set Network Status to Connected, PSN Status to RPCN, and turn UPNP on at the bottom.
8. Take your Radmin IP (26.x.x.x), and set IP/Hosts switches to: gosredirector.ea.com==YourRadminIPhere&&downloads.skate.online.ea.com==YourRadminIPhere (All your friends will set theirs to YOUR Radmin IP, since you are the one hosting the server)
9. In settings.json of the server files, set LocalHost to true and the LocalIPAddress to your Radmin IP.
10. Run the server, now you and your friends can boot up Skate 3 and sign into EA Nation.
### Hosting publicly
Forward port 42100 for both UDP and TCP, and port 80 for TCP. Set LocalHost to false in settings.json and run the server executable. Now in RPCS3 config under Network tab you need to set IP/Hosts switches to "gosredirector.ea.com==YourPublicIPHere&&downloads.skate.online.ea.com==YourPublicIPHere"
# Special Thanks
[@Aim4Kill](https://github.com/Aim4kill) for making the [BlazeSDK](https://github.com/Aim4kill/BlazeSDK) (Saved so much time with having the packet structures there for almost all Blaze commands)

[@gamingrobot](https://github.com/gamingrobot) for amazing documentation on the TDF format used in Blaze servers.

[New Blaze Emulator](https://github.com/Tratos/New-Blaze-Emulator) by [@Tratos](https://github.com/Tratos) (Example Blaze server with working matchmaking)

[NPTicket](https://github.com/LittleBigRefresh/NPTicket) by [LittleBigRefresh Team](https://github.com/LittleBigRefresh) (Used for RPCN ticket validation)

[BWKingsnake](https://github.com/bwkingsnake) for making the [Skate 3 Online Config Tool](https://github.com/bwkingsnake/rpcs3-skate-3-config-editor) for setting all needed config values.

# Troubleshooting
Below are listed the most common problems and their explanations/solutions:
### P: I'm able to load into a lobby for few milliseconds and then lose connection to current game session
E: This is caused by desync, which means you have a RPCS3 setting in Skate 3 custom config set to a different value than other players in the lobby. Any setting that affects physics that you have set to different value will cause you to desync out of all lobbies immediately.


Steps to fix: Right click on Skate 3 on RPCS3 games list and click on "Change Custom Configuration", and go to CPU tab. Make sure your XFloat Accuracy is set to "Approximate", and "Enable SPU loop detection" is set to unticked. Lastly, you cannot be running any physics related mods from Native Menu or you will lose connection immediately.

### P: I'm getting isolated from other players, and when I try to join a friends lobby it's putting me into a lobby by myself
E: This is usually caused by a Skate 3 version mismatch, since old 1.00 update players will be put to different lobbies than latest 1.05 update players.


Steps to fix: Go to RPCS3 games list, and check which Version your Skate 3 is. If it says 1.00 (1.05 Available), then you need to download a update .pkg file and drag it to this RPCS3 games list. You can download the update .pkg file from these links: [BLUS30464 Update](https://www.mediafire.com/file/rxqhyorbzu4408p/UP0006-BLUS30464_00-SKATE030UPDATE05-A0105-V0100-PE.pkg/file) / [BLES00760 Update](https://www.mediafire.com/file/mvg67xw9fyf3hjh/EP0006-BLES00760_00-SKATE030UPDATE05-A0105-V0100-PE.pkg/file)

# Disclaimer
Not affiliated, associated, authorized, endorsed by, or in any way officially connected with Electronic Arts Inc. or any of its subsidiaries or affiliates. The use of any trademarks, logos, or brand names is for identification purposes only and does not imply endorsement or affiliation.
