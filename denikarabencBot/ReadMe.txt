Thank you for using denikarabencBot. This is so early that is not even early access, but thank you for helping me test it!
If you know someone would also like to participate in testing, please, do not share the bot, but just ask that person to contact me via mail or twitch, I would gladly share the latest version.
I will try to send you the updates when I have them. I cannot promise regular updates, but if you find a bug, I will try to fix it as soon as possible. Other upgrades and features will be slower, but it will come eventually

Nice, so let's start!

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Do not forget to mod the bot ;)

To use the auto change game on twitch you need to provide the steam id. SteamID is not your Steam name, but a number. You can find it here: steamidfinder.com
Your steam profile must be public profile. You do not have to be online in chat, just your profile needs to be public.

There is small limit on how often game can be automatically changed. That is because of Twitch's slow update time. That time is 3 minutes. So if you started the game, and the game is not changed on twitch for 3 minutes, send me back the log so I can fix the bug in the future
If you are playing a non steam game, and the game is not changed, while in game, press the "log process" button and send me the name of the game you played, and a file named "loggedProcess.txt"

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Recently, Steam changed the way how we can get data about the game. If you notice that the game you started is not changed to correct game, please, send be which game you played, and a log file, I will try to correct it as soon as possible

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Replay function is currently available only via OBS. ReplayBuffer Save replay shortcut must be set. Following explains how to set it correctly:

If you want to use the !replay function, following setting must me added

Open the \AppData\Roaming\obs-studio\basic\profiles\[YOUR PROFILE]\basic.ini

if there is no [Hotkeys] part, add [Hotkeys] to the end of the file, and in the new line add (if there is [Hotkeys], just add the line in Hotkeys part)
ReplayBuffer={\n    "ReplayBuffer.Save": [\n        {\n            "key": "F13"\n        }\n    ]\n}

find [Simple] part and change RecRB=false to RecRB=true

find [AdvOut] part and add following lines:

RecRB=true
RecRBTime=30 (number of seconds you want replay to last.)
RecFilePath=[Path to video where clips will be saved] (make this path the same to the replay path when you start the bot)
RecFormat=mp4

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Setting Replay video is a bit of trouble right now, but it will be fixed in the future.
First, you need to set up replay at first, and then in sources add Window capture. Pop a replay and when replay window is opened, use that window as a source. And set WindowMatch Priority to "Window title must match". You can disable capture mouse cursor if you want.

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Permissions are currently in development phase. Permissions which are working at the moment are regular, mod and king. In the future, other permissions you can see at the commands screen will be implemented in the future

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

I'm working on automating this process in the future.

If you want for you replay buffer to start when you start streaming, there is an option in the obs in the general tab.

If you add two commands via UI with the same name, the last last one will be loaded.

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

If you noticed some youtube files, that is because it is work in progress, but song requsets are not working properly atm, so it is disabled.

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

If you are reporting a bug, please describe the problem as best as you can, add time when problem occured and send me logs. That is the best possible way for me to identify the problem and fix it quickly

Any questions, suggestions, feature requests, bug reports, whatever you want, you can send to denikarabenc+denikarabencBot@gmail.com