(function() {

    angular.module('jogTracker')
        .factory('notificationUtils', notificationUtils);

    notificationUtils.$inject = ['toastr'];

    function notificationUtils(toastr) {

        var showErrorToast = function(err, title) {
            //Show error in a toast.
            if (err.data.Message) {
                toastr.error(err.data.Message, title);
            }
            else {
                toastr.error(err.statusText, title);
            }
        };

        var showSuccess = function(message, title) {
            toastr.success(message, title);
        };

        return {
            showErrorToast : showErrorToast,
            showSuccess: showSuccess
        };
    }
}());
