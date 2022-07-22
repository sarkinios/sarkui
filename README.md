<!-- MANPAGE: BEGIN EXCLUDED SECTION -->
<div align="center">

[![Sarkui](https://raw.githubusercontent.com/sarkinios/sarkui/master/.github/banner.png)](#readme)

[![Release version](https://img.shields.io/github/v/release/sarkinios/sarkui?color=brightgreen&label=Download&style=for-the-badge)](https://github.com/sarkinios/sarkui/releases/download/2022.07.23.v1/SarkUI.v1.0.0.zip))
[![Donate](https://img.shields.io/badge/_-Donate-red.svg?logo=githubsponsors&labelColor=555555&style=for-the-badge)](https://ko-fi.com/sarkas)
    
</div>
<!-- MANPAGE: END EXCLUDED SECTION -->



# Table of Contents
> * [Introduction](#introduction)
> * [Installation](#installation)
> * [Using SarkUI](#use)

# Introduction

This Software made for convienience of use. It's a wrap up of all programs used for media extracting, muxing (i.e. mkvtoolnix, mp4box etc) 
in all-in-one GUI. 

With Sarkui you can add as many files of same type you wish and make changes all at once.
You can:
- Change/Add/Remove/Extract the cover in multiple video files 
- Rename the video tracks metadata title, subtitles and audio tracks name and language
- Convert multiple media from one packager to another ( mkv to mp4 , ts to mkv etc)
- Mux/Demux all subtitles or specific language from multiple files with one click 
- Show mediainfo of audio and subtitles from multiple files
- Audio convert to other filetypes (with or without pitch correction and speed)
- Rename multiple subtitle and audio files according to video files in the same folder to match the video name.
Darktheme Included.

|gui light| gui dark |
:----------------------:|:----------------------:
![image1](https://imgur.com/36VIzQG.png)|![image2](https://imgur.com/Av6UinI.png)


# Installation

Download the zip and extract it. The executable is portable and ready to run. 
Before start using the program you need to configure the location paths under Options for the following programs: 
- [ffmpeg](https://ffmpeg.org/download.html)
- [mkvpropedit](https://mkvtoolnix.download/downloads.html)
- [mkvmerge](https://mkvtoolnix.download/downloads.html)
- [mkvextract](https://mkvtoolnix.download/downloads.html)
- [mp4box](https://gpac.wp.imt.fr/downloads/)
- [sox](https://sourceforge.net/projects/sox/files/sox/)

All these programs can be found free online

![options](https://imgur.com/hY2zdya.png)

After setting the paths you can start using SarkUI.

# Use

1. Select the filetype you want to work with and drag n Drop / select from browser button multiple files.
2. After step 1 you can:
     - From **Edit Source** Tab:
       - Change the Cover of all selected files. Select the image you want and click Add Cover. You can Remove Cover from the selected files , or extract it.
       - Add file Title in mediainfo metadata
       - If a file contain multiple video/audio/subtitle tracks, you can choose the one you want and change title/language/default track
     - From **Convert** Tab:
       - You can one Click Convert the files from step 1 to other packagers
       - Mux/Import audio and subtitles included in the same path with the files and choose the language before importing them (Subtitles and Audio must have same name as video files)
       - Demux/Extract subtitles and Audio from the files in step 1. You can choose to export all subtitles or specific language ones (You can check what language files included in the media info button)
     - From **Misc** Tab:
       - Change the tempo, speed or convert the audio files you select in step 1
       - Audio/Sub Renamer: You can include in your video files path subtitle or audio files (srt,mp3,aac etc) and rename/sort them the same as your video files.
         (Due to lexicographic sorting and not natural sort, your sub and audio must have zero in front of their numbering - i.e.  sub 01.srt sub 02.srt etc and not sub 1.srt sub 2.srt otherwise the sorting will not execute correctly) 
