# AudioBookSplitter
Command line to split a mp3 audio book according to its chapters

# Overview
This tool is based on the [ffprobe executable](https://ffmpeg.org/ffprobe.html) to extract the chapter list of a mp3 file using the following command:

```ffprobe.exe -v quiet -print_format json -show_chapters -show_format "<source.mp3>"```

ffprobe will return information about chapter start time, end time, ...

Then it used the [ffmpeg](https://ffmpeg.org/ffmpeg.html) to extract each chapter from the source file, based on the start time and duration (end time - start time).

The tool will create (if needed) the folder <target folder>\<album_artist>\<album>\<number> - <title>.


&lt;number&gt; will be deducted from the disc number (TAG:disc) or from the date (TAG:date) if the TAG:disc is missing.

For this reason, before extracting the mp3 source file, I've checked the mp3 tags are correctly filled.

For exemple, the Hyperion.mp3 file has the following properties:
![File properties](file%20properties.png)

```ffprobe -show_format "1 - Hypérion.mp3"``` will generate this output
```
...
[FORMAT]
filename=1 - Hypérion.mp3
nb_streams=2
nb_programs=0
format_name=mp3
format_long_name=MP2/3 (MPEG audio layer 2/3)
start_time=0.050111
duration=76906.083265
size=707636488
bit_rate=73610
probe_score=51
TAG:album=Hypérion
TAG:artist=Matthieu Dahan
TAG:album_artist=Dan Simmons
TAG:comment=Au 28e siècle, sur la planète Hypérion, les dangers s'amoncellent. Celui de la guerre avec l'approche de la flotte des Extros en perpétuel conflit avec l'Hégémonie. Celui du gritche, figure mythologique et meurtrière que révère l'Église des Templiers. Celui de l'ouverture des Tombeaux du Temps qui dérivent de l'avenir vers le passé à la rencontre d'une imprévisible catastrophe.

Dans l'espoir de sauver Hypérion et d'accomplir leurs destins suspendus, sept pèlerins se dirigent ensemble vers le sanctuaire du gritche. Il y a le père Lenar Hoyt, prêtre catholique, qui a vu l'enfer ; le colonel Kassad, dit le Boucher de Bressia, à la recherche d'un rêve ; Martin Silenus, le poète, qui a connu la Vieille Terre et perdu les mots ; Brawne Lamia, la belle détective, qui a aimé un John Keats synthétique : le Consul qui a régné sur Hypérion ; Sol Weintraub, l'érudit, dont la fille perd des années ; et le Templier Het Masteen, qui garde ses secrets.

Autant d'énigmes, autant d'histoires, qu'ils choisissent de conter avant d'affronter les labyrinthes d'Hypérion. Autant de styles différents.
TAG:copyright=©1991 Baror International / Robert Laffont. Traduit de l'américain par Guy Abadia (P)2017 Audible Studios
TAG:disc=1
TAG:encoder=Lavf57.14.100
TAG:genre=Audiobook
TAG:title=Hypérion
TAG:json64=son64
TAG:asin=sin
TAG:product_id=roduct_id
TAG:creation_time=reation_time
TAG:author=uthor
TAG:date=2017
...
```

# Usage
The tool accepts two arguments:
- the source file: The mp3 file to split.
- the target folder: the folder where the extracted chapters will be stored.

Used with ```--help``` as argument, it will display usage syntax.

```
C:\Tools\AudioBookSplitterCmd.exe --help
AudioBookSplitterCmd 1.1.82.7058
Copyright © 2019 Prometeo


  -s, --Source    Required. MP3 file to split
  -o, --Output    The folder where the files will be stored.
  --help          Display this help screen.
  --version       Display version information.
```


```
D:\Tmp>"d:\tools\Prometeo Audiobook splitter\AudioBookSplitterCmd.exe" -s "1 - Hypérion.mp3" -o "d:\tmp\output"
Extracting 1/18: '01 - Chapter 1' from 'Hypérion'
frame=    0 fps=0.0 q=0.0 Lsize=   28560kB time=01:00:55.29 bitrate=  64.0kbits/s speed=56.7x
Extracting 2/18: '02 - Chapter 2' from 'Hypérion'
frame=    0 fps=0.0 q=0.0 Lsize=   33721kB time=01:11:55.87 bitrate=  64.0kbits/s speed=56.5x
Extracting 3/18: '03 - Chapter 3' from 'Hypérion'
frame=    0 fps=0.0 q=0.0 Lsize=   29579kB time=01:03:05.74 bitrate=  64.0kbits/s speed=60.5x
Extracting 4/18: '04 - Chapter 4' from 'Hypérion'
frame=    0 fps=0.0 q=0.0 Lsize=   38340kB time=01:21:47.13 bitrate=  64.0kbits/s speed=70.2x
Extracting 5/18: '05 - Chapter 5' from 'Hypérion'
frame=    0 fps=0.0 q=0.0 Lsize=   22810kB time=00:48:39.31 bitrate=  64.0kbits/s speed=  57x
Extracting 6/18: '06 - Chapter 6' from 'Hypérion'
frame=    0 fps=0.0 q=0.0 Lsize=   32664kB time=01:09:40.66 bitrate=  64.0kbits/s speed=69.6x
Extracting 7/18: '07 - Chapter 7' from 'Hypérion'
frame=    0 fps=0.0 q=0.0 Lsize=   37306kB time=01:19:34.87 bitrate=  64.0kbits/s speed=78.9x
Extracting 8/18: '08 - Chapter 8' from 'Hypérion'
frame=    0 fps=0.0 q=0.0 Lsize=   38652kB time=01:22:27.12 bitrate=  64.0kbits/s speed=83.7x
Extracting 9/18: '09 - Chapter 9' from 'Hypérion'
frame=    0 fps=0.0 q=0.0 Lsize=   36858kB time=01:18:37.42 bitrate=  64.0kbits/s speed=84.8x
Extracting 10/18: '10 - Chapter 10' from 'Hypérion'
frame=    0 fps=0.0 q=0.0 Lsize=   26905kB time=00:57:23.48 bitrate=  64.0kbits/s speed=70.5x
Extracting 11/18: '11 - Chapter 11' from 'Hypérion'
frame=    0 fps=0.0 q=0.0 Lsize=   36344kB time=01:17:31.67 bitrate=  64.0kbits/s speed=92.6x
Extracting 12/18: '12 - Chapter 12' from 'Hypérion'
frame=    0 fps=0.0 q=0.0 Lsize=   31267kB time=01:06:41.88 bitrate=  64.0kbits/s speed=97.1x
Extracting 13/18: '13 - Chapter 13' from 'Hypérion'
frame=    0 fps=0.0 q=0.0 Lsize=   42404kB time=01:30:27.35 bitrate=  64.0kbits/s speed= 114x
Extracting 14/18: '14 - Chapter 14' from 'Hypérion'
frame=    0 fps=0.0 q=0.0 Lsize=   39735kB time=01:24:45.72 bitrate=  64.0kbits/s speed= 127x
Extracting 15/18: '15 - Chapter 15' from 'Hypérion'
frame=    0 fps=0.0 q=0.0 Lsize=   37468kB time=01:19:55.58 bitrate=  64.0kbits/s speed= 131x
Extracting 16/18: '16 - Chapter 16' from 'Hypérion'
frame=    0 fps=0.0 q=0.0 Lsize=   31024kB time=01:06:10.71 bitrate=  64.0kbits/s speed= 128x
Extracting 17/18: '17 - Chapter 17' from 'Hypérion'
frame=    0 fps=0.0 q=0.0 Lsize=   26190kB time=00:55:52.08 bitrate=  64.0kbits/s speed= 139x
Extracting 18/18: '18 - Chapter 18' from 'Hypérion'
frame=    0 fps=0.0 q=0.0 Lsize=   31051kB time=01:06:14.29 bitrate=  64.0kbits/s speed= 145x
Press any key to exit the program.

```

# Build
Open the ```Audio Book Splitter.sln``` file with VisualSudio and Build the solution.

# Third-party modules
1. [CommandLineParser](https://github.com/commandlineparser/commandline) - [Licence](https://github.com/commandlineparser/commandline/blob/master/License.md)
2. [Humanizer](https://github.com/Humanizr/Humanizer) - [Licence](https://github.com/Humanizr/Humanizer/blob/master/LICENSE)
3. [Newtonsoft.Json](https://www.newtonsoft.com/json) - [Licence](https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md)
4. [ffmpeg and ffprobe](https://github.com/FFmpeg/FFmpeg) - [Licence](https://github.com/FFmpeg/FFmpeg/blob/master/LICENSE.md)
