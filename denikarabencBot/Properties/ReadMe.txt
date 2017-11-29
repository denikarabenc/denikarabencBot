Do not forget to mod the bot ;)

To use the auto change game on twitch you need to provide the steam id. SteamID is not your Steam name, but a number. You can find it here: steamidfinder.com
Your steam profile must be public profile. You do not have to be online in chat, just your profile needs to be public.

If you started the game, and the game is not changed on twitch for 3 minutes, send me back the log so I can fix the bug in the future
If you are playing a non steam game, and the game is not changed, while in game, press the "log process" button and send me the name of the game you played, and a file named "loggedProcess.txt"

Replay function is currently available only via OBS. ReplayBuffer Save replay shortcut must be set. Following explains how to set it correctly:

If you want to use the !replay function, following setting must me added

Open the \AppData\Roaming\obs-studio\basic\profiles\[YOUR PROFILE]\basic.ini

if there is no [Hotkeys] part, add [Hotkeys] to the end of the file, and in the new line add (if there is [Hotkeys], just add the line in Hotkeys part)
ReplayBuffer={\n    "ReplayBuffer.Save": [\n        {\n            "key": "F13"\n        }\n    ]\n}

find [Simple] part and change RecRB=false to RecRB=true

find [AdvOut] part and add following lines:

RecRB=true
RecRBTime=27 (number of seconds you want replay to last.)
RecFilePath=[Path to video where clips will be saved] (you typed this in the begining, when you opened the bot UI)
RecFormat=mp4

I'm working on automating this process in the future.

Also, if you want for you replay buffer to start when you start streaming, there is an option in the obs in the general tab.

Also work in progress for easier adding of the commands. As is now, the commands can be only be predefined or added via chat with !addcommand. But added commands are not saved when app is turned off. That will be developed as soon as possible.