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

### Front end development (JS / HTML)

There are tasks for updating the js and html files after changes have been made, without having to rebuild to app.
```
./build.sh CopyClient
```

### Running in dev mode

This will run the server up with stubbed data from external dependancies

```
fsharpi bin/viewer/RunServer.fsx "dev"
```

Now visit localhost:8083/qs in a browser of your choice.


## Deploying to Rancher
There is a docker-compose file that is compatible with the rancher-compose tool for deploying to the rancher container service.
i.e:
```
rancher-compose -p resourceapi up
```

## Tracking
This app using hotjar and google analytics for capturing user behaviour.  To use these you need to provide the following account ids as environment variables:

* HOTJARID (hot jar id)
* GAID (google analytics id)

The server will still run if you dont provide these (dev mode or test)

