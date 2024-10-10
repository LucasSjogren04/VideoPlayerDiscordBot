.Net Console app meant to run locally on Windows.
Commands to the app are done through a discord app (discord bot).

Features:
  Lets your friends add youtube videos to a queue and have them automatically start playing on the machine running the app.

Dependencies:
  mpv
  yt-dlp (ffmpeg reccomended for higher video quality)


For the app to work it needs a "settings.txt" in the root of the application.

Example settings.txt file:
```
token:exampletoken123
guildId:123456789
downloadPath:C:\Users\USERNAME\Videos
