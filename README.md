# Discovery tool app for the knowledge base quality standards project

This project contains the web app for searching and browsing the quality standards knowledge base.  Currently written in F# using Suave.io web framework. This project is based on the [ProjectScaffold F# template](http://fsprojects.github.io/ProjectScaffold/), uses FAKE for task management and [Paket](http://fsprojects.github.io/Paket/) for packet management.  Please see the project scaffold docs for more information about this setup.

# Requirements
* docker
* docker-compose

### Building + running tests
To build and test tasks run:
```
docker build -t discoverytooldev .
```
See build.fsx for task and dependancy definitions

### Running in dev mode

This will run the server up with stubbed data from external dependancies (you will have to have run the build command above in 'building a running tests')

```
docker run --rm -e SERVER_MODE=dev -p 8083:8083 discoverytooldev
```

Now visit localhost:8083/qs in a browser of your choice.


## Deploying to Rancher
There is a docker-compose file that is compatible with the rancher-compose tool for deploying to the rancher container service.
i.e:
```
rancher-compose -p resourceapi up
```

## Environment variables
The environment variables can be set.  For a full list of variables see the docker-compose.rml environment section.

* HOTJARID (hot jar id)
* GAID (google analytics id)
* LOGGING_ENVIRONMENT (dev,test,prod)

The server will still run if you dont provide these (dev mode or test)

## Code structure
There is a component-oriented structure to the codebase with components found in src/viewer/components.  Data related functions that shuold be refactored out to APIs are found in src/viewer/data.   (currently retrieving vocabs and searching elasticsearch)

