# GTA:LCS Save Editor
💾 This tool allows you to edit Grand Theft Auto: Liberty City Stories save files.

![](https://i.imgur.com/vW3wRov.png)

## Features
  - All versions of the game are supported: PS2, PSP, Android, and iOS.
  - Edit various player attributes including outfit, max health and armor,
    weapons, spawn point, and perks, including perks not used in the vanilla
    game like fast reload!
  - A comprehensive garage editor that lets you change many aspects of your
    saved vehicles. Edit vehicle model, color, extra parts like rollbars or
    liveries, vehicle orientation, and special properties like BP/CP/FP/EP and
    more! You can even spawn unused vehicles like the Maverick or Hunter.
  - A stats editor/viewer that lets you change any statistic in the game as
    well as preview how that change will look in the in-game Stats menu.
  - A global variables editor that gives you total control of the game state. 
  - A collectible map to help you find remaining Hidden Packages, Rampages, and
    Unique Stunt Jumps.
  - Edit many other aspects of the game, including game settings, weather, time,
    and even some unused features to spice up your Liberty City experience.
  - Built-in updater so you'll get the latest feature updates as soon as they're
    available.

## System Requirements
  - Windows PC
  - [.NET Core Runtime](https://dotnet.microsoft.com/download/dotnet-core)

You may also choose to run the standalone version of this tool, which includes
the .NET runtime bundled inside the executable, but at the cost of a much larger
file size.

## Download
Head over to the
[releases page](https://github.com/whampson/lcs-save-editor/releases) to download
the latest version!

## Extracting Save Files
In order to edit save files, you'll need to extract them from your console or
mobile device.

### Android
  - You'll need a rooted device to access save files.
  - Save files are located at `/data/data/com.rockstargames.gtalcs/files/`

### iOS:
  - You'll need a jailbroken device to access save files if you're using iOS 8.3
    or newer.
  - Save files are located in the Documents folder.

### PS2
  - You'll need a cheat device, memory card reader, or a softmodded console to
    extract save files from the memory card.
  - If you're using PCSX2, use
    [mymc](http://www.csclub.uwaterloo.ca:11068/mymc/) to access the files on
    the virtual memory card.
  - You may need to use a tool like
    [PS2 Save Builder](https://www.ps2savetools.com/download/ps2-save-builder/)
    to access the raw files depending on how the save was extracted.

### PSP
  - PSP saves are encrypted, so you'll have to decrypt them before the save
    editor will be able to read them. You can use PPSSPP, the PSP emulator, to
    decrypt saves.
    1) Download and install [PPSSPP](http://ppsspp.org/)
    2) Boot GTA:LCS in the emulator, play through the first mission, and save
       the game. This is done to create the save directory.
    3) Locate the GTA:LCS save directory by navigating to
       `<documents>/PPSSPP/memstick/PSP/SAVEDATA/<game_id>/`
    4) Overwrite the contents of that directory with the save you want to edit.
    5) Disable save encryption
        - Open the following file in a text editor:
          `<documents>/PPSSPP/memstick/PSP/SYSTEM/ppsspp.ini`
        - In the [SystemParam] section, add the following line:
          `EncryptSave = False`
        - Restart PPSSPP
    6) Boot up GTA:LCS, load your save, and save it again in a different slot.
    7) Your decrypted save file will be loacted at
       `<documents>/PPSSPP/memstick/PSP/SAVEDATA/<game_id>/DATA.BIN`

## Support
Questions? Comments? Suggestions? Bugs? 🐛🐜  
👉 [Open an issue](https://github.com/whampson/lcs-save-editor/releases) on
GitHub.  
👉 Visit the
[official GTAForums topic](https://gtaforums.com/index.php?showtopic=847469) and
partake in discussion.  

If you like my work, [buy me a coffee](https://ko-fi.com/thehambone) to show
your support! ☕

## Credits
Special thanks to `GTAKid667` for designing the logo and for extensive feedback
and support during development.

Thanks to `Packing_Heat`, `_CP_`, `Inadequate`, `Lethal Vaccine`, `Stallion458`,
`The Hero`, `Parik`, `AztecaEh`, `Username.gta`, `NightmanCometh96`, and
`GTAshnik177` for providing save files and other information for research and
testing.

## Version History
### 1.0.0
*20 September 2020*

  - Initial release

## Additional Screenshots
![](https://i.imgur.com/BsOjyoT.png)
![](https://i.imgur.com/FmSZjoN.png)
![](https://i.imgur.com/aGZY5he.png)
![](https://i.imgur.com/fXYwMKs.png)
![](https://i.imgur.com/UR6rOnI.png)
![](https://i.imgur.com/RFfTUkL.png)
![](https://i.imgur.com/zKEoly4.png)
![](https://i.imgur.com/heyHfb1.png)

## Legal
Copyright (C) 2016-2020 Wes Hampson
