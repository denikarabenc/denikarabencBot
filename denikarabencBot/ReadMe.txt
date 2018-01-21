Thank you for using denikarabencBot. This is so early that is not even early access, but thank you for helping me test it!
If you know someone would also like to participate in testing, please, do not share the bot, but just ask that person to contact me via mail (denikarabenc+denikarabencBot@gmail.com) or twitch (twitch.tv/denikarabenc), I would gladly share the latest version and help with installation if needed.
I will try to send you the updates when I have them. I cannot promise regular updates, but if you find a bug, I will try to fix it as soon as possible. Other upgrades and features will be slower, but it will come eventually.

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

SYSTEM REQUIREMENTS:

If you are trying to stream, you probably have much better PC than needed, but here it goes:

OS: Windows 7 or above (Not suported on Linux and Mac)
Processor: Core 2 Duo
Memory: 1 GB RAM (Well, windows requires 1 on 32 bit or 2 on 64. Bot really needs 50MB)
Graphics: Discreet video card
Storage: 5 MB available space
Sound Card: No
Internet access.

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Here is the quick explanation of features bot currently has:

     Commands - basically, like every other bot has, you can make bot replay to some commands. Exaample would be if somebody types "!twitter" in chat, you can configure so bot replays with "My twitter is twitter.com/denikarabenc"
Also, you can use username of a user sending a message and message he typed. User is {0} and message he sent after command is {1}.
Example would be to make command like "!lick" and set message to "{0} licked {1}" and if user "deni" types "!lick denikarabenc", bot will return message "deni licked denikarabenc"
Also, command can be edited or added from chat by using "!addcommand [command] : [message]" or "!editcommand [existing command] : [new message]". User has to be mods or streamer. Example: "!addcommand !deathcount : death count is 23". "!editcommand !deathcount : death count is 24"

    Replay - You can replay last 30 seconds of your stream when somebody types "!replay" in chat. File is saved on HDD

	Clip - Typing "!clip" makes clip on twitch and plays it like replay.
	
	Change or check current title - Typing "!title" would show current title for every user. Typing "!title New title" will change your current title to "New title". User has to be mod or streamer to change the title.

	Automatic game changer - If you checked "Use auto game changer", whenever you change game on your pc, game will automatically be updated on twitch. That's it! NOTE: Obviously not working for console games.

	Played games - Typing !gamesplayed will get which games you played and for how long. This currently only works for games automatically changed. Soon it will work for every game change you make on twitch.

	Reminders - Typing "!remindme [reminder]" will get reminder in bot UI so chat can remind you to do stuff after stream. Example: "!remindme Check this speedrun https://www.youtube.com/watch?v=tcvkaEUbLik"

That is it, for now. If you want to disable some of the features above, you should just delete command from configuration xml found in \Serializables\buildInCommands

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Setting up bot:

Do not forget to mod the bot ;)

To use the auto change game on twitch you need to provide the steam id. SteamID is not your Steam name, but a number. You can find it here: steamidfinder.com (I do not own or have any association with that site)
Your steam profile must be public profile and you have to be online in chat.

There is small limit on how often game can be automatically changed. That is because of Twitch's slow API update time. That time is around 3 minutes. So if you started the game, and the game is not changed on twitch for 3 minutes, send me back the log so I can fix the bug in the future
If you are playing a non steam game, and the game is not changed, while in game, press the "log process" button and send me the name of the game you played, and a file named "loggedProcess.txt"

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Recently, Steam changed the way how we can get data about the game. If you notice that the game you started is not changed to correct game, please, send me which game you played, press the "log process" button and send me the name of the game you played, and a file named "loggedProcess.txt", and a log file, I will try to correct it as soon as possible

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Replay function is currently available only via OBS. "ReplayBuffer Save replay" shortcut must be set. Following explains how to set it correctly:

If you want to use the !replay function, following setting must me added

Edit the \AppData\Roaming\obs-studio\basic\profiles\[YOUR PROFILE]\basic.ini (quick navigate -> %AppData%\obs-studio\basic\profiles\)

if there is no [Hotkeys] part, add [Hotkeys] to the end of the file, and in the new line add (if there is [Hotkeys], just add the line in Hotkeys part)
ReplayBuffer={\n    "ReplayBuffer.Save": [\n        {\n            "key": "F13"\n        }\n    ]\n}

NOTE: If you have already used replay shortcut, this will not override your command, it will just add another one.

find [Simple] part and change RecRB=false to RecRB=true

find [AdvOut] part and add following lines:

RecRB=true
RecRBTime=30 (number of seconds you want replay to last.)
RecFilePath=[Path to video where clips will be saved] (make this path the same to the replay path when you start the bot)
RecFormat=mp4

We need mp4 format in order to evade problems with some PCs do not have necessary codexes.

When starting the stream, you have to start replay buffer as well so replay would work.
TIP: In OBS's general settings, you can set up for replay buffer to start automatically with the stream so you do not have to take care of starting the replay buffer as well.

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Setting Replay video layer is a bit of trouble right now, but it will be fixed in the future.
First, you need to set up replay at first, and then in sources add Window capture.
Start a bot and type !replay in chat. When replay window is opened, use that window as a source, And set WindowMatch Priority to "Window title must match". Also disable capture mouse cursor.

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Setting Clip video layer is a bit of trouble right now, but it will be fixed in the future.
First, you need to open OBS, add BrowserSource, and select Local file. In the local file path select file named Clips/clipHTML.html from your bot path.
Select Width = 1023 and Height = 575, FPS to your liking (60 if you are streaming at 60, 30 if 30 etc.)
Check both checkboxes (Shutdown source when not visible and Refresh browser when scene becomes active).
When source is added, make it invisible (this is important!)
Rename it to "Twitch clip" your wish if you want.

Next part is really tricky, so we need to do some of the stuff just so we can have easier navigation.
Open the setting and navigate to hotkeys.
Find the scene where we just added clips layer, and find show/hide "Twitch clip" (the name you give in previous steps) and add shortcut to something like "ctrl+shift+f12".
Save the settings and close the OBS.
Navigate to folder \AppData\Roaming\obs-studio\basic\scenes\[YOUR PROFILE NAME].ini (quick navigate -> %AppData%\obs-studio\basic\scenes\)
Open up search and find following lines
						"alt": true,
                        "control": true,
                        "key": "OBS_KEY_F12",
                        "shift": true
When you locate them, lines should be under  "libobs.hide_scene_item.Twitch Clip": and "libobs.hide_scene_item.Twitch Clip":, and "hotkeys" somewhere more up. This is just to check not to change some other shortcut accidentally.
Change all of the 4 lines ("alt": true,"control": true,"key": "OBS_KEY_F12","shift": true) to the "key": "OBS_KEY_F14" (quote marks should be copied as well and no comma after the quote marks!)
NOTE: This should be done twice, for showing and for hiding the layer.

If this is done, start OBS, go to settings -> hotkeys, find "Twitch clip" and check if hotkey is changed to OBS_KEY_F14 for show and hide. If it is, all good, ready to use clips!

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Permissions are currently in development phase. Permissions which are working at the moment are regular, mod and king. In the future, other permissions you can see at the commands screen will be implemented in the future

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

I'm working on automating this process in the future.

If you add two commands via UI with the same name, the last last one will be loaded.

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

If you noticed some youtube files, that is because it is work in progress, but song requsets are not working properly atm, so it is disabled.

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

If you are reporting a bug, please describe the problem as best as you can, add time when problem occured and send me logs. That is the best possible way for me to identify the problem and fix it quickly

Any questions, suggestions, feature requests, bug reports, whatever you want, you can send to denikarabenc+denikarabencBot@gmail.com