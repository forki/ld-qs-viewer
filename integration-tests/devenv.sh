APP_DIR=${APP_DIR:?"Need to set APP_DIR"}
docker run --rm -it --name e2etestdevenv -v $APP_DIR:/app -w "/app" node:6.5.0 bash
