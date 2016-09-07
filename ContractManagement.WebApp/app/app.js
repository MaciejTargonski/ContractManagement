(function () {
    'use strict';

    var underscore = angular.module('underscore', []);

    underscore.factory('_', function () { return window._; });

    angular.module('app', [
        'ui.router',
        'ui.bootstrap.tpls',
        'ui.bootstrap.dropdown',
        'ui.bootstrap.buttons',
        'ui.utils',
        'ngAnimate',
        'ngResource',


        'app.controllers',

        'underscore',

        'ui.grid',
        'ui.grid.selection',
        'ui.grid.pagination',
        'ui.grid.resizeColumns',
        'ui.grid.exporter',

        'ui.grid.saveState',

        'ncy-angular-breadcrumb'
    ])

       
        .config([
            '$stateProvider',
            '$urlRouterProvider',
            '$breadcrumbProvider',
            '$httpProvider',
            function (
                $stateProvider,
                $urlRouterProvider,
                $breadcrumbProvider,
                $httpProvider) {

                (function routing() {
                    // Redirect unknown urls to the default state
                    $urlRouterProvider
                        .otherwise(function ($injector, $location) {

                            var path = $location.path();
                            var is404 = (path !== '' && path !== '/');
                            var destinationState = is404 ? 'contracts' : 'contracts';

                            $injector.get("$state").go(destinationState);
                        });


                    $stateProvider
                        .state('root', {
                            url: '',
                            abstract: true,
                            templateUrl: ''
                        })
                        .state('contracts', {
                            url: '/contracts',
                            templateUrl: 'app/contracts/contracts.html',
                            controller: 'ContractsController',
                            ncyBreadcrumb: {
                                skip: true
                            }
                        })
                        .state('newContract', {
                            url: '/newcontract',
                            templateUrl: 'app/contracts/newcontract.html',
                            controller: 'ContractsController',
                            ncyBreadcrumb: {
                                skip: true
                            }
                        })


                }());
                

            }])

        .run([
            '$rootScope',
            '$state',
            '$stateParams',
            '$window',
            '$location',
            '$q',
            '$log',
            '$exceptionHandler',

            function (
                $rootScope,
                $state,
                $stateParams,
                $window,
                $location,
                $q,
                $log,
                $exceptionHandler) {


                var isInitialDataFetched = false; 
                var appInitPromise = $q.defer(); 

                $rootScope.isAppInitInProgress = true; 

                $rootScope.$state = $state;
                $rootScope.$stateParams = $stateParams;


                $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
                    $rootScope.toState = toState;
                    $rootScope.toParams = toParams;
                });

                $rootScope.$on('$stateChangeError', function (event, toState, toParams, fromState, fromParams, error) {
                    $state.go('contracts');
                });



                appInitPromise.promise
                    .finally(function () {
                        $rootScope.isAppInitInProgress = false;
                    });
            }]);
})();