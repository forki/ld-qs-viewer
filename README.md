# Discovery tool app for the knowledge base quality standards project

This project contains the web app for searching and browsing the quality standards knowledge base.  Currently written in F# using Suave.io web framework. This project is based on the [ProjectScaffold F# template](http://fsprojects.github.io/ProjectScaffold/), uses FAKE for task management and [Paket](http://fsprojects.github.io/Paket/) for packet management.  Please see the project scaffold docs for more information about this setup.

## Code structure
There is a component-oriented structure to the codebase with components found in src/viewer/components.  Data related functions that shuold be refactored out to APIs are found in src/viewer/data.   (currently retrieving vocabs and searching elasticsearch)


### Building + running tests
To run the FAKE task runner to run the build and test tasks run:
```
./build.sh
```
See build.fsx for task and dependancy definitions

### Tests
Tests are run using NUnit with the FsUnit assertion framework.

### Running in dev mode

This will run the server up with stubbed data from external dependancies

```
fsharpi bin/viewer/RunServer.fsx "dev"
```

Now visit localhost:8083/qs in a browser of your choice.

### Running in production

This requires that you link/dns resolve the running docker container to the following containers


* elastic - container running elastic search on port 9200 (see src/viewer/data/Elastic.fs)
* resourceapi - container running http resource api on port 8082 (see src/viewer/pages/Resource.fs)

See docker-compose file in https://github.com/nhsevidence/ld-kb-qs for usage..

# Search 
This app uses elastic for a search index

