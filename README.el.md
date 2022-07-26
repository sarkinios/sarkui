<!-- MANPAGE: BEGIN EXCLUDED SECTION -->

<div align="center">

[![Sarkui](https://raw.githubusercontent.com/sarkinios/sarkui/master/.github/banner.png)](#readme)

[![Windows](https://img.shields.io/badge/-Windows_x64-blue.svg?style=for-the-badge&label=Download&logo=windows&color=6272a4)](#εκδόσεις-αρχείων)
[![Crypto](https://img.shields.io/badge/_-Crypto-ffb86c.svg?logo=githubsponsors&labelColor=555555&style=for-the-badge&label=Donate)](https://github.com/sarkinios/sarkui/blob/main/.github/donate.md)
[![ko-fi](https://img.shields.io/badge/_-Buy_me_a_coffee-red.svg?logo=kofi&labelColor=555555&style=for-the-badge)](https://ko-fi.com/sarkas)

</div>
<!-- MANPAGE: END EXCLUDED SECTION -->

## Μετάφραση README

-   [Αγγλικά](README.md)
-   [Ελληνικά](README.el.md)

# Πίνακας περιεχομένων

> -   [Εισαγωγή](#εισαγωγή)
> -   [Εγκατάσταση](#εγκατάσταση)
> -   [Χρησιμοποιώντας το SarkUI](#χρήση)
> -   [Εκδόσεις Αρχείων](#εκδόσεις-αρχείων)
> 
# Εισαγωγή

Αυτό το λογισμικό δημιουργήθηκε για ευκολία στη χρήση. Είναι μια σύνοψη όλων των προγραμμάτων που χρησιμοποιούνται για εξαγωγή πολυμέσων, muxing (π.χ. mkvtoolnix, mp4box κ.λπ.)
σε όλα σε ένα GUI για **πολλαπλά** αρχεία πολυμέσων **ΤΑΥΤΟΧΡΟΝΑ**.

Με το Sarkui μπορείτε να προσθέσετε όσα αρχεία ίδιου τύπου θέλετε και να κάνετε αλλαγές ταυτόχρονα.

-   Αλλαγή/Προσθήκη/Αφαίρεση/Εξαγωγή του εξωφύλλου σε πολλά αρχεία βίντεο
-   Μετονομάστε το όνομα και τη γλώσσα του τίτλου των μεταδεδομένων των κομματιών βίντεο, των υπότιτλων και των κομματιών ήχου
-   Μετατροπή πολλαπλών μέσων από ένα πακέτο σε άλλο (mkv σε mp4, ts σε mkv κ.λπ.)
-   Mux/Demux όλοι οι υπότιτλοι ή η συγκεκριμένη γλώσσα από πολλά αρχεία με ένα κλικ
-   Εμφάνιση πληροφοριών πολυμέσων ήχου και υποτίτλων από πολλά αρχεία
-   Μετατροπή ήχου σε άλλους τύπους αρχείων (με ή χωρίς διόρθωση τόνου και ταχύτητα)
-   Μετονομάστε πολλαπλούς υπότιτλους και αρχεία ήχου σύμφωνα με αρχεία βίντεο στον ίδιο φάκελο για να ταιριάζει με το όνομα του βίντεο.
    Περιλαμβάνεται Dark theme.

|                  gui Light                 |               gui Dark               |
| :--------------------------------------: | :--------------------------------------: |
| ![image1](https://imgur.com/36VIzQG.png) | ![image2](https://imgur.com/Av6UinI.png) |

# Εγκατάσταση

Κατεβάστε το zip και εξαγάγετε το. Το εκτελέσιμο αρχείο είναι έτοιμο για εκτέλεση.
Πριν ξεκινήσετε να χρησιμοποιείτε το πρόγραμμα, πρέπει να διαμορφώσετε τις διαδρομές τοποθεσίας στο Options για ΟΛΑ τα ακόλουθα προγράμματα:

Aπαιτούμενες εφαρμογές| περιγραφή | πακέτο
:----------------------|:------------------------------------------------------------:|:----:
|-[ffmpeg](https://ffmpeg.org/download.html)| δοκιμασμένο με την έκδοση n5.0.1.8| μέρος του πακέτου ffmpeg|
|-[mkvpropedit](https://www.fosshub.com/MKVToolNix.html)<br />-[mkvmerge](https://www.fosshub.com/MKVToolNix.html)<br />-[mkvextract](https://www.fosshub.com/MKVToolNix.html)| δοκιμασμένο με την έκδοση V67| μέρη του πακέτου mkvtoolnix|
|-[mp4box](https://gpac.wp.imt.fr/downloads/)|δοκιμασμένο με mp4box 2.0 rev0| μέρος του πακέτου gpac|
|-[sox](https://sourceforge.net/projects/sox/files/sox/)|δοκιμασμένο με την έκδοση 14.4.2| πακέτο sox|

Όλα αυτά τα προγράμματα μπορείτε να τα βρείτε δωρεάν στο διαδίκτυο. Χρειάζεται μόνο να προσθέσετε την εγκατεστημένη θέση αυτών των εκτελέσιμων στο SarkUI.
Ελέγξτε ότι εκτελούνται σωστά από τις διαδρομές τους πριν τις προσθέσετε στο SarkUI. (Το sox και το mp4box χρειάζονται τα αρχεία τους dll, επομένως μην τα μετακινήσετε έξω από την εγκατεστημένη διαδρομή τους)

![options](https://imgur.com/hY2zdya.png)

Αφού ορίσετε τις διαδρομές, μπορείτε να ξεκινήσετε να χρησιμοποιείτε το SarkUI.

### SarkUI video
![](https://github.com/sarkinios/sarkui/raw/main/.github/setup%20SarkUI.gif)

# Χρήση

1.  Επιλέξτε τον τύπο αρχείου με τον οποίο θέλετε να εργαστείτε είτε με drag n drop / browse από το κουμπί του προγράμματος περιήγησης πολλά αρχεία.
2.  Μετά το βήμα 1 μπορείτε:
    -   Από **Edit Source** Καρτέλα:
        - Αλλάξτε το εξώφυλλο όλων των επιλεγμένων αρχείων. Επιλέξτε την εικόνα που θέλετε και κάντε κλικ στην Προσθήκη εξωφύλλου. Μπορείτε να αφαιρέσετε το εξώφυλλο από τα επιλεγμένα αρχεία ή να το εξαγάγετε.
        - Προσθήκη τίτλου αρχείου στα μεταδεδομένα mediainfo
        -   Εάν ένα αρχείο περιέχει πολλά κομμάτια βίντεο/ήχου/υπότιτλων, μπορείτε να επιλέξετε αυτό που θέλετε και να αλλάξετε τίτλο/γλώσσα/προεπιλεγμένο κομμάτι
    -   Από **Convert** Καρτέλα:
        -   Μπορείτε να κάνετε ένα κλικ Μετατροπή των αρχείων από το βήμα 1 σε άλλα προγράμματα συσκευασίας
        -   Mux/Import ήχου και υπότιτλων που περιλαμβάνονται στην ίδια διαδρομή με τα αρχεία και επιλέξτε τη γλώσσα πριν την εισαγωγή τους (Οι υπότιτλοι και ο ήχος πρέπει να έχουν το ίδιο όνομα με τα αρχεία βίντεο)
        -   Demux/Εξαγωγή υπότιτλων και ήχου από τα αρχεία στο βήμα 1. Μπορείτε να επιλέξετε να εξαγάγετε όλους τους υπότιτλους ή τη συγκεκριμένη γλώσσα (Μπορείτε να ελέγξετε ποια αρχεία γλώσσας περιλαμβάνονται στο κουμπί πληροφοριών πολυμέσων)
    -   Από **Misc** Καρτέλα:
        -   Αλλάξτε το ρυθμό, την ταχύτητα ή μετατρέψτε τα αρχεία ήχου που επιλέξατε στο βήμα 1
        -   Audio/Sub Renamer: Μπορείτε να συμπεριλάβετε στη διαδρομή των αρχείων βίντεο υπότιτλους ή αρχεία ήχου (srt,mp3,aac κ.λπ.) και να τα μετονομάσετε/ταξινομήσετε όπως και τα αρχεία βίντεο.
          

![Tabs](https://imgur.com/zdH6V02.png)

# Εκδόσεις Αρχείων

#### Το 2ο πακέτο περιλαμβάνει όλα τα απαραίτητα εκτελέσιμα που χρειάζεται το SarkUI για να λειτουργήσει σωστά χωρίς να χρειαστεί να τα κατεβάσετε ξεχωριστά από το ίντερνετ. 
Προσθέστε απλά την διαδρομή τους μεσα στο Options.
(Για την άδεια χρήσης ανατρέξτε στη σελίδα της εκάστοτε εφαρμογής)

[![Changelog](https://img.shields.io/badge/changelog-blue.svg)](https://github.com/sarkinios/sarkui/releases/latest)


| Αρχείο                                                                                                                                                                                  | Περιγραφή                            |
| :-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :----------------------------------- |
[![Windows](https://img.shields.io/badge/Version-v1.0.2-informational?color=f1fa8c&style=for-the-badge&label=release)](https://github.com/sarkinios/sarkui/releases/latest/download/SarkUI.v1.0.2.zip)|Windows (Win10) standalone x64 binary|
[![Windows](https://img.shields.io/badge/Version-v1.0.2wTools-informational?color=f1fa8c&style=for-the-badge&label=release)](https://github.com/sarkinios/sarkui/releases/download/2022.07.25.v1.0.2/SarkUI.v1.0.2wTools.zip)|Windows (Win10) standalone x64 binary with ffmpeg/mkvpropedit/mkmerge/mp4box/mkvextract/sox|
