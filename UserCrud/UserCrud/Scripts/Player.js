
    var playlist = [
        "https://pagalfree.com/musics/128-Tu%20Laut%20Aa%20-%20Yasser%20Desai%20128%20Kbps.mp3",
        "https://pagalfree.com/musics/128-Mitra%20Re%20-%20Runway%2034%20128%20Kbps.mp3",
        "https://pagalfree.com/musics/128-Jaan%20Hai%20Meri%20(Lofi)%20-%20Radhe%20Shyam%20128%20Kbps.mp3",
        "https://pagalworld.com.se/files/download/id/3265",
        "https://wynk.in/music/song/have-a-great-day/um_00050087476601-USHR12141891",
        "https://pagalfree.com/musics/128-Mitwa%20-%20Kabhi%20Alvida%20Naa%20Kehna%20128%20Kbps.mp3",
        "https://pagalfree.com/musics/128-Iktara%20-%20Wake%20Up%20Sid%20128%20Kbps.mp3",
        
    ];
    var currentSongIndex = 0; // Keep track of the current song index in the playlist

    var musicPlayer = document.getElementById('musicPlayer');

    function playMusic() {
        musicPlayer.src = playlist[currentSongIndex];
    musicPlayer.play();
    }

    function pauseMusic() {
        musicPlayer.pause();
    }

    function nextSong() {
        currentSongIndex++;
        if (currentSongIndex >= playlist.length) {
        currentSongIndex = 0; // Wrap around to the first song in the playlist
        }
    playMusic();
    }

    function previousSong() {
        currentSongIndex--;
    if (currentSongIndex < 0) {
        currentSongIndex = playlist.length - 1; // Wrap around to the last song in the playlist
        }
    playMusic();
    }

