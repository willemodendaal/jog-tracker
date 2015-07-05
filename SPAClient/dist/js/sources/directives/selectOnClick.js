// Focus directive code from here: http://stackoverflow.com/questions/14995884/select-text-on-input-focus
(function () {
    angular
        .module('jogTracker')
        .directive('selectOnClick', ['$window', function ($window) {
            return {
                restrict: 'A',
                link: function (scope, element, attrs) {
                    element.on('click', function () {
                        if (!$window.getSelection().toString()) {
                            this.setSelectionRange(0, this.value.length)
                        }
                    });
                }
            };
    }]);

}());
