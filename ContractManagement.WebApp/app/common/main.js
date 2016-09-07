(function () {
    'use strict';

    angular.module('app.controllers', []);

    angular.module('app.controllers')
    .controller("MainController", ['$scope', '$rootScope', function ($scope, $rootScope) {
        var updateTitle = function () {
            if ($rootScope.toState.title) {
                $scope.pageTitle = $rootScope.toState.title;
            }
            else if ($rootScope.projectName) {
                $scope.pageTitle = $rootScope.projectName;
            }
            else {
                $scope.pageTitle = ('HR Contract Management');
            }
        };
        $rootScope.$on('$stateChangeSuccess', updateTitle);
    }]);

})();
