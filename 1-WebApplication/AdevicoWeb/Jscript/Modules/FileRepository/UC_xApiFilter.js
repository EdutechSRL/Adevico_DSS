$(document).ready(function () {
    var cbxMinTimeStay = $('#CBXminTimeStay');
    if (cbxMinTimeStay) {
        cbxMinTimeStay.change(function () {
            $('#TXBhoursMinTimeStay').prop('disabled', !this.checked);
            $('#TXBminutesMinTimeStay').prop('disabled', !this.checked);
            $('#TXBsecondsMinTimeStay').prop('disabled', !this.checked);
        });
    }

    var cbxMinTimeVision = $('#CBXMinTimeVision');
    if (cbxMinTimeVision) {
        cbxMinTimeVision.change(function () {
            $('#TXBhoursMinTimeVision').prop('disabled', !this.checked);
            $('#TXBminutesMinTimeVision').prop('disabled', !this.checked);
            $('#TXBsecondsMinTimeVision').prop('disabled', !this.checked);
        });
    }

    var cbxMinPercenrtage = $('#CBXMinPercentageVision');
    if (cbxMinPercenrtage) {
        cbxMinPercenrtage.change(function () {
            $('#TXBMinPercentageVision').prop('disabled', !this.checked);
        });
    }

    var cbxMinScore = $('#CBXMinScore');
    if (cbxMinScore) {
        cbxMinScore.change(function () {
            $('#TXBMinScore').prop('disabled', !this.checked);
        });
    }

    var cbxMinSlide = $('#CBXMinSlide');
    if (cbxMinSlide) {
        cbxMinSlide.change(function () {
            $('#TXBMinSlide').prop('disabled', !this.checked);
        });
    }

});