# Viewer app for the knowledge base quality standards project

This project contains the web app for searching and browsing the quality standards knowledge base.  Currently written in F# using Suave.io web framework.  There is a component-oriented structure to the codebase with components in src/viewer/components.  Data related function that shuold be refactored out to APIs are found in src/viewer/data.


### Building

```
./build.sh
```

### Running in dev mode

This will run the server up with stubbed data from external dependancies

```
fsharpi bin/viewer/RunServer.fsx "dev"
```

### Running in production

This requires that you link/dns resolve the running docker container to the following containers


* elastic - container running elastic search on port 9200 (see src/viewer/data/Elastic.fs)
* resourceapi - container running http resource api on port 8082 (see src/viewer/pages/Resource.fs)

See docker-compose file in https://github.com/nhsevidence/ld-kb-qs for usage..

