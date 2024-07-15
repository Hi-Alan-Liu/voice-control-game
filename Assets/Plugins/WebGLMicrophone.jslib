mergeInto(LibraryManager.library, {
    StartMicrophone: function() {
        navigator.mediaDevices.getUserMedia({ audio: true })
        .then(function(stream) {
            window.audioContext = new (window.AudioContext || window.webkitAudioContext)();
            window.microphone = window.audioContext.createMediaStreamSource(stream);
            window.analyser = window.audioContext.createAnalyser();
            window.microphone.connect(window.analyser);
            window.analyser.fftSize = 256;
            window.bufferLength = window.analyser.frequencyBinCount;
            window.dataArray = new Uint8Array(window.bufferLength);
        })
        .catch(function(err) {
            console.log('The following gUM error occured: ' + err);
        });
    },

    GetMicrophoneVolume: function() {
        if (window.analyser) {
            window.analyser.getByteTimeDomainData(window.dataArray);
            var sum = 0;
            for (var i = 0; i < window.bufferLength; i++) {
                sum += (window.dataArray[i] - 128) * (window.dataArray[i] - 128);
            }
            var rms = Math.sqrt(sum / window.bufferLength) / 128.0;
            return rms;
        } else {
            return 0;
        }
    }
});
