# Viewer app for the knowledge base quality standards project

This project contains the web app for searching and browsing the quality standards knowledge base.  Currently written in F# using Suave.io web framework

### Building

```
./build.sh
```

### Running in dev mode

This will run the server up with stubbed data 

```
fsharpi bin/viewer/RunServer.fsx "dev"
```

### Running in prod

This requires that you link the running docker container to another container called 'elastic'.
See docker-compose file in https://github.com/nhsevidence/ld-kb-qs for usage.
