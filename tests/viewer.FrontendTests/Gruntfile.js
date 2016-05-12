module.exports = function (grunt){

  grunt.initConfig({
    watch: {
      server: {
        files: [
          '*.js',
          '*.json',
          'lib/*.js'
        ],
        tasks: ['develop'],
        options: { nospawn: true }
      },
      deps : {
        files : [
          'package.json'
        ],
        tasks : ['npm-install', 'develop'],
        options : { nospawn : true}
      },
      test : {
        files : ['tests/*.js'],
        tasks : ['mochaTest']
      }
    },
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

  grunt.loadNpmTasks('grunt-develop');
  grunt.loadNpmTasks('grunt-npm-install');
  grunt.loadNpmTasks('grunt-contrib-watch');
  grunt.loadNpmTasks('grunt-mocha-test');

  grunt.registerTask('default', ['develop', 'watch']);

};
