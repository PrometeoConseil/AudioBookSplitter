# AudioBookSplitter
Command line to split a mp3 audio book according to its chapters

# Overview
This tool is based on the [ffprobe executable](https://ffmpeg.org/ffprobe.html) to extract the chapter list of a mp3 file using the following command:

```ffprobe.exe -v quiet -print_format json -show_chapters -show_format "<source.mp3>"```

ffprobe will return information about chapter start time, end time, ...

Then it used the [ffmpeg](https://ffmpeg.org/ffmpeg.html) to extract each chapter from the source file, based on the start time and duration (end time - start time).


# Usage
The tool accepts two arguments:
- the source file: The mp3 file to split.
- the target folder: the folder where the extracted chapters will be stored.

The tool will create (if needed) the the folder <target folder>\<album_artist>\<album>\<number> - <title>.

For this reason, before extracting the mp3 source file, I've checked the mp3 tags are correctly filled.
Fo exemple, the Hyperion.mp3 file has the following properties:

