APP_DIR=${APP_DIR:?"Need to set APP_DIR"}
docker run --rm -it --name ldviewer_devenv -v $APP_DIR:/app -w "/app" ocelotuproar/docker-alpine-fsharp:4.0 sh  
