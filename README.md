<!-- MANPAGE: BEGIN EXCLUDED SECTION -->
<div align="center">

[![Sarkui](https://raw.githubusercontent.com/sarkinios/sarkui/master/.github/banner.png)](#readme)

[![Windows](https://img.shields.io/badge/-Windows_x64-blue.svg?style=for-the-badge&label=Download&logo=windows&color=6272a4)](https://github.com/sarkinios/sarkui/releases/latest/download/SarkUI.v1.0.0.zip)
[![Crypto](https://img.shields.io/badge/_-Crypto-red.svg?logo=githubsponsors&labelColor=555555&style=for-the-badge&label=Donate)](https://github.com/sarkinios/sarkui/blob/main/.github/donate.md)
    
</div>
<!-- MANPAGE: END EXCLUDED SECTION -->

## README Translation
- [English](README.md)
- [Ελληνικά](README.el.md)

# Table of Contents
> * [Introduction](#introduction)
> * [Installation](#installation)
> * [Using SarkUI](#use)

# Introduction

This Software made for convenience of use. It's a wrap up of all programs used for media extracting, muxing (i.e. mkvtoolnix, mp4box etc) 
in all-in-one GUI for **multiple** media files **simultaneously**. 

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
needed apps| description | package
:----------------------|:---------------------:|:-----:
|- [ffmpeg](https://ffmpeg.org/download.html) | tested with latest release| part of ffmpeg package|
|- [mkvpropedit](https://www.fosshub.com/MKVToolNix.html) <br /> - [mkvmerge](https://www.fosshub.com/MKVToolNix.html)  <br /> - [mkvextract](https://www.fosshub.com/MKVToolNix.html)  |  tested with latest release|  parts of mkvtoolnix package|
|- [mp4box](https://gpac.wp.imt.fr/downloads/) |tested with mp4box 0.5.0| part of gpac package|
|- [sox](https://sourceforge.net/projects/sox/files/sox/) |tested with latest release| sox package|

All these programs can be found free online. You only need to add the installed location of these executables inside SarkUI.
Check that they run correctly from their paths before adding them to SarkUI. (sox and mp4box need their dll files, so don't move them outside their installed path)

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
         
![Tabs](https://imgur.com/zdH6V02.png)
         
#### Release Files

File|Description
:---|:---
[![Windows](https://img.shields.io/github/v/release/sarkinios/sarkui?color=f1fa8c&style=for-the-badge)](https://github.com/sarkinios/sarkui/releases/latest/download/SarkUI.v1.0.0.zip)|Windows (Win10) standalone x64 binary
