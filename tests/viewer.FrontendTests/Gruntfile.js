module.exports = function (grunt){

  grunt.initConfig({
    mochaTest : {
        test : {
            options : {
                reporter : 'spec',
                captureFile : 'results.txt',
                quiet : false,
                clearRequireCache : false
            },
            src : ['tests/*.js']
        }
    }
  });

  grunt.loadNpmTasks('grunt-mocha-test');

  grunt.registerTask('default', ['develop', 'watch']);

};
